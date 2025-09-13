using System;
using System.Drawing;
using DataObjects.LogInOutBO;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNDesktopFlaUI.Classes;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Rail_Activities;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;
using static MTNForms.Controls.MTNControlBase;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    /// --------------------------------------------------------------------------------------------------
    /// Date         Person          Reason for change
    /// ==========   ======          =================================
    /// 13/03/2022   navmh5          Initial creation
    /// --------------------------------------------------------------------------------------------------
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44072: MTNBase
    {
        
        CargoEnquiryForm _cargoEnquiryForm1;
        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        RailOperationsForm _railOperations;
        ConfirmationFormOKwithText _confirmationFormOKwithText;

        const string TestCaseNumber = "44072";
        const string VehicleId = TestCaseNumber;

        readonly string[] _cargoId =
        {
            "JLG" + TestCaseNumber + "B01",
            "JLG" + TestCaseNumber + "A02",
            "JLG" + TestCaseNumber + "A01",
            "JLG" + TestCaseNumber + "C01"
        };

        bool _menuItemFound;
        Point _pointToClick;
        Point _startPoint;
        Point _endPoint;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = "_" + TestCaseNumber + "_";
            DeleteForms = false;
            BaseClassInitialize_New(testContext);
            //userName = $"MTN{TestCaseNumber}";
        }
        
        [TestInitialize]
        public new void TestInitialize() {}


        [TestCleanup]
        public new void TestCleanup()
        {
            MTNDesktop.theApp.Close(true);
            base.TestCleanup();
        }

        private void MTNInitialize()
        {
            Console.WriteLine($"userName: {userName}");
            //MTNSignon(TestContext, userName, titleToLookFor: "[Cargo Enquiry TT1]");
            LogInto<MTNLogInOutBO>($"MTN{TestCaseNumber}", "[Cargo Enquiry TT1]");

            CallJadeScriptToRun(TestContext, $"resetData_{TestCaseNumber}");

            SetupAndLoadInitializeData(TestContext);
        }

        [TestMethod]
        public void PackCargoWhichIsOnTruckRailVoyage()
        {
            MTNInitialize();

            // Step 4 - 12
            //FormObjectBase.NavigationMenuSelection($"Gate Functions|{RoadGateForm.FormTitle}");
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: $"{RoadGateForm.FormTitle} {terminalId}", vehicleId: VehicleId);
            roadGateForm.SetRegoCarrierGate(VehicleId);
            SaveCargoForRoadGate(_cargoId[2], $"{RoadGateDetailsReceiveForm.FormTitleReceiveEmptyContainer} {terminalId}",
                $"{WarningErrorForm.FormTitleWarningGateInOut} {terminalId}");
            SaveCargoForRoadGate(_cargoId[1], $"{RoadGateDetailsReceiveForm.FormTitleReceiveGeneralCargo} {terminalId}",
                $"{WarningErrorForm.FormTitleWarningGateInOut} {terminalId}");
            roadGateForm.btnSave.DoClick();

            try
            {
                warningErrorForm = new WarningErrorForm($"{WarningErrorForm.FormTitleWarningGateInOut} {terminalId}");
                warningErrorForm.btnSave.DoClick();
            }
            catch {}

            roadGateForm.CloseForm();

            // Step 13 - 14
            //FormObjectBase.NavigationMenuSelection($"Yard Functions|{RoadOperationsForm.FormTitle}");
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm($"{RoadOperationsForm.FormTitle} {terminalId}");

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date  roadOperationsForm.tblYard1.FindClickRow($"Vehicle Id^{VehicleId}~Cargo ID^{_cargoId[2]}",
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date      ClickType.ContextClick);
            roadOperationsForm.TblYard2.FindClickRow(new[] {$"Vehicle Id^{VehicleId}~Cargo ID^{_cargoId[2]}" },
                ClickType.ContextClick);
            roadOperationsForm.contextMenu.MenuSelect(RoadOperationsForm.ContextMenuItems.YardMoveSelected);

            // Step 15 - 19 - Cargo Enquiry Check
            cargoEnquiryForm = new CargoEnquiryForm($"{CargoEnquiryForm.FormTitle} {terminalId}");
            CheckForUnpackCargoMenuItemNotAvailable(_cargoId[1], CargoEnquiryForm.SiteType.OnSite);
            CheckForUnpackCargoMenuItemNotAvailable(_cargoId[0], CargoEnquiryForm.SiteType.OnShip);
            MTNKeyboard.Press(VirtualKeyShort.ESC);
            
            // Step 20 - 23 - Wagon Check
            //FormObjectBase.NavigationMenuSelection($"Yard Functions | Rail Activities | {RailOperationsForm.FormTitle}", true);
            FormObjectBase.MainForm.OpenRailOperationsFromToolbar();
            _railOperations = new RailOperationsForm($"000000 Rail Area  - ACTUAL {RailOperationsForm.FormTitle.ToUpper()} {terminalId}");
            //_railOperations.btnSelectTrain.DoClick();
            _railOperations.DoSelectTrainFromToolbar();
            _railOperations.GetSelectTrainsPlanning();
            //_railOperations.tblTrainsPlanning.FindClickRow(
             //   $"Code^TRAIN{TestCaseNumber}~Description^Train for Test{TestCaseNumber}");
            _railOperations.TblTrainsPlanning.FindClickRow(
                [$"Code^TRAIN{TestCaseNumber}~Description^Train for Test{TestCaseNumber}"]);
            _railOperations.btnShow.DoClick();
            
            _railOperations.GetWagonGraphicArea();
            _pointToClick = new Point(_railOperations.grpWagonGraphicArea.BoundingRectangle.Left + 55,
                _railOperations.grpWagonGraphicArea.BoundingRectangle.Top + 32);
            Mouse.Click(_pointToClick);
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(1));
            _pointToClick = new Point(_pointToClick.X - 5, _pointToClick.Y);
            Mouse.Click(_pointToClick);
            Mouse.RightClick();
            
            _menuItemFound = _railOperations.contextMenu.MenuSelect(RailOperationsForm.ContextMenuItems.WGCargoPackCargo, validateOnly: true);
            Assert.IsFalse(_menuItemFound, 
                $"Test Case 44072 - '{RailOperationsForm.FormTitle} - Cargo | Pack Cargo/Unpack Cargo' menu item should NOT have been found");
            
            // Step 24 - 26
            cargoEnquiryForm.SetFocusToForm();
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type",
                CargoEnquiryForm.CargoTypes.BottlesOfBeer, fromCargoEnquiry: true, rowDataType: EditRowDataType.ComboBoxEdit);
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", _cargoId[1]);
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)",
                CargoEnquiryForm.SiteType.OnSite, fromCargoEnquiry: true, doDownArrow: true, searchSubStringTo: CargoEnquiryForm.SiteType.OnSite.Length - 1, rowDataType: EditRowDataType.ComboBoxEdit);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date cargoEnquiryForm.TblData.FindClickRow($"ID^{_cargoId[1]}");
            cargoEnquiryForm.tblData2.FindClickRow(new[] {$"ID^{_cargoId[1]}"});
            _startPoint = Mouse.Position;

            _cargoEnquiryForm1 = new CargoEnquiryForm($"{CargoEnquiryForm.FormTitle} {terminalId}");
            SetValueInEditTable(_cargoEnquiryForm1.tblSearchCriteria, "Cargo Type",
                CargoEnquiryForm.CargoTypes.ISOContainer, fromCargoEnquiry: true, rowDataType: EditRowDataType.ComboBoxEdit);
            SetValueInEditTable(_cargoEnquiryForm1.tblSearchCriteria, "Cargo ID", _cargoId[2]);
            SetValueInEditTable(_cargoEnquiryForm1.tblSearchCriteria, "Site (Current)",
                CargoEnquiryForm.SiteType.OnSite, fromCargoEnquiry: true, doDownArrow: true, searchSubStringTo: CargoEnquiryForm.SiteType.OnSite.Length - 1, rowDataType: EditRowDataType.ComboBoxEdit);
            //_cargoEnquiryForm1.btnSearch.DoClick();
            _cargoEnquiryForm1.DoSearch();

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date _cargoEnquiryForm1.TblData.FindClickRow($"ID^{_cargoId[2]}");
            _cargoEnquiryForm1.tblData2.FindClickRow(new[] {$"ID^{_cargoId[2]}"});
            _endPoint = Mouse.Position;
            
            Mouse.Drag(_startPoint, _endPoint);
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date _cargoEnquiryForm1.TblData.FindClickRow($"ID^{_cargoId[2]}~Children^");
            _cargoEnquiryForm1.tblData2.FindClickRow(new[] {$"ID^{_cargoId[2]}~Children^"});
            
            cargoEnquiryForm.SetFocusToForm();
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type",
                CargoEnquiryForm.CargoTypes.BottlesOfBeer, fromCargoEnquiry: true, rowDataType: EditRowDataType.ComboBoxEdit);
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", _cargoId[0]);
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)",
                CargoEnquiryForm.SiteType.OnShip, fromCargoEnquiry: true, doDownArrow: true, searchSubStringTo: CargoEnquiryForm.SiteType.OnShip.Length - 1, rowDataType: EditRowDataType.ComboBoxEdit);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date cargoEnquiryForm.TblData.FindClickRow($"ID^{_cargoId[0]}");
            cargoEnquiryForm.tblData2.FindClickRow(new[] {$"ID^{_cargoId[0]}"});
            _startPoint = Mouse.Position;
            
            Mouse.Drag(_startPoint, _endPoint);
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date _cargoEnquiryForm1.TblData.FindClickRow($"ID^{_cargoId[2]}~Children^");
            _cargoEnquiryForm1.tblData2.FindClickRow(new[] {$"ID^{_cargoId[2]}~Children^"});
            
            cargoEnquiryForm.SetFocusToForm();
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type",
                CargoEnquiryForm.CargoTypes.BottlesOfBeer, fromCargoEnquiry: true, rowDataType: EditRowDataType.ComboBoxEdit);
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", _cargoId[3]);
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)",
                CargoEnquiryForm.SiteType.PreNotified, fromCargoEnquiry: true, doDownArrow: true, searchSubStringTo: CargoEnquiryForm.SiteType.PreNotified.Length - 1, rowDataType: EditRowDataType.ComboBoxEdit);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date cargoEnquiryForm.TblData.FindClickRow($"ID^{_cargoId[3]}");
            cargoEnquiryForm.tblData2.FindClickRow(new[] {$"ID^{_cargoId[3]}"});
            _startPoint = Mouse.Position;
            
            Mouse.Drag(_startPoint, _endPoint);
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date _cargoEnquiryForm1.TblData.FindClickRow($"ID^{_cargoId[2]}~Children^");
            _cargoEnquiryForm1.tblData2.FindClickRow(new[] {$"ID^{_cargoId[2]}~Children^"});
            
            // Step 31 - 34
            roadOperationsForm.SetFocusToForm();
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date roadOperationsForm.tblYard1.FindClickRow($"Vehicle Id^{VehicleId}~Cargo ID^{_cargoId[1]}",
             // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date    ClickType.ContextClick);
            roadOperationsForm.TblYard2.FindClickRow(new[] {$"Vehicle Id^{VehicleId}~Cargo ID^{_cargoId[1]}" },
                ClickType.ContextClick);
            roadOperationsForm.contextMenu.MenuSelect(RoadOperationsForm.ContextMenuItems.YardDeleteCurrentEntry);
     
            _confirmationFormOKwithText = new ConfirmationFormOKwithText("Enter the Cancellation value:", controlType: ControlType.Pane);
            //_confirmationFormOKwithText.SetValue(_confirmationFormOKwithText.txtInput, $"Cancellation: Test {TestCaseNumber}");
            _confirmationFormOKwithText.txtInput.SetValue($"Cancellation: Test {TestCaseNumber}");
            _confirmationFormOKwithText.btnOK.DoClick();
            
            
            warningErrorForm = new WarningErrorForm(formTitle: $"{WarningErrorForm.FormTitleWarningRoadOps} {terminalId}");
            warningErrorForm.btnSave.DoClick();

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date roadOperationsForm.tblYard1.FindClickRow($"Vehicle Id^{VehicleId}~Cargo ID^{_cargoId[2]}",
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date     ClickType.ContextClick);
            roadOperationsForm.TblYard2.FindClickRow(new[] {$"Vehicle Id^{VehicleId}~Cargo ID^{_cargoId[2]}" },
                ClickType.ContextClick);
            roadOperationsForm.contextMenu.MenuSelect(RoadOperationsForm.ContextMenuItems.YardProcessRoadExit);
            
        }

        /// <summary>
        /// Check that Unpack Cargo menu item doesn't exist
        /// </summary>
        /// <param name="cargoId">Cargo Id</param>
        /// <param name="siteType">Site type.  e.g. On Site; On Ship</param>
        void CheckForUnpackCargoMenuItemNotAvailable(string cargoId, string siteType)
        {
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type",
                CargoEnquiryForm.CargoTypes.BottlesOfBeer, fromCargoEnquiry: true, doDownArrow: true,
                searchSubStringTo: CargoEnquiryForm.CargoTypes.BottlesOfBeer.Length - 1);
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", cargoId);
            SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", siteType, fromCargoEnquiry: true,
                doDownArrow: true, searchSubStringTo: siteType.Length - 1);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date cargoEnquiryForm.TblData.FindClickRow($"ID^{cargoId}", ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new[] {$"ID^{cargoId}"}, ClickType.ContextClick);
            _menuItemFound = cargoEnquiryForm.contextMenu.MenuSelect(CargoEnquiryForm.ContextMenuItems.CargoCargoUnPack,
                validateOnly: true);
            _menuItemFound = cargoEnquiryForm.contextMenu.MenuSelect(CargoEnquiryForm.ContextMenuItems.CargoCargoPack,
                validateOnly: true);
            Assert.IsFalse(_menuItemFound, 
                $"Test Case 44072 - '{CargoEnquiryForm.FormTitle} - Cargo | Pack Cargo/Unpack Cargo' menu item should NOT have been found");
            MTNKeyboard.Press(VirtualKeyShort.ESC);
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date cargoEnquiryForm.TblData.FindClickRow($"ID^{cargoId}", ClickType.Click);
            cargoEnquiryForm.tblData2.FindClickRow(new[] {$"ID^{cargoId}"});
        }

        /// <summary>
        /// Save cargo on the Road Gate form
        /// </summary>
        /// <param name="cargoId">Cargo Id to save</param>
        /// <param name="formTitle">Receive from title</param>
        /// <param name="warningFormTitle">Warning Form title</param>
        void SaveCargoForRoadGate(string cargoId, string formTitle, string warningFormTitle)
        {
            //SetValue(roadGateForm.txtNewItem, cargoId);
            roadGateForm.txtNewItem.SetValue(cargoId);
            Keyboard.Press(VirtualKeyShort.TAB);

            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle);
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            try
            {
                warningErrorForm = new WarningErrorForm(formTitle: warningFormTitle);
                warningErrorForm.btnSave.DoClick();
            }
            catch  {}
            
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

           

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG44072A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>MKBS01</locationId>\n            <weight>5000</weight>\n            <imexStatus>Import</imexStatus>\n            <commodity>MT</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <isoType>2200</isoType>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bottles of Beer</cargoTypeDescr>\n            <id>JLG44072A02</id>\n            <operatorCode>MSL</operatorCode>\n            <weight>5000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n			<totalQuantity>30</totalQuantity>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");

            // Delete Prenote
            CreateDataFileToLoad(@"DeletePrenote.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED/EDI_STATUS_DBLOADED_PARTIAL_X</operationsToPerformStatuses>\n 	  <Prenote Terminal='TT1'>\n            <cargoTypeDescr>Bottles of Beer</cargoTypeDescr>\n            <id>JLG44072A02</id>\n            <imexStatus>Export</imexStatus>\n             <weight>8000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n	                <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<totalQuantity>30</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>D</messageMode>\n        </Prenote>\n    </AllPrenote>\n</JMTInternalPrenote>\n\n\n");
          
            // Create Prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <Prenote Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG44072A01</id>\n			<isoType>2200</isoType>\n            <imexStatus>Export</imexStatus>\n             <weight>8000</weight>\n			 <commodity>MT</commodity>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n		  <Prenote Terminal='TT1'>\n            <cargoTypeDescr>Bottles of Beer</cargoTypeDescr>\n            <id>JLG44072A02</id>\n            <imexStatus>Export</imexStatus>\n             <weight>8000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n	                <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<totalQuantity>30</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n    </AllPrenote>\n</JMTInternalPrenote>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }
}
