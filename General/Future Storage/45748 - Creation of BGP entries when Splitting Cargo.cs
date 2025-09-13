using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Lander;
using System;
using System.Threading;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNBaseClasses.BaseClasses.Web;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNWebPages.PageObjects.Mobile_Apps;

namespace MTNAutomationTests.TestCases.Master_Terminal.General.Future_Storage
{
    [TestClass, TestCategory(TestCategories.MTNWeb)]
    public class TestCase45748 : MobileAppsBase
    {

        private VoyageEnquiryForm _voyageEnquiryForm;
        private VoyageDefinitionForm _voyageDefinitionForm;
        private VoyageOperationsForm _voyageOperationsForm;
        private CargoMoveItForm _cargoMoveItForm;
        private ConfirmationFormYesNo _confirmationFormYesNo;
        private BackgroundApplicationForm _backgroundApplicationForm;
        private CargoEnquiryDirectForm _cargoEnquiryDirectForm;
        private ConfirmWeightToMoveForm _confirmWeightToMoveForm;

        private const string TestCaseNumber = @"45748";
        private const string VoyageCode = @"MSCK0" + TestCaseNumber + @"001";
        private DateTime _currentDateTime = DateTime.Now;

         readonly string[] _cargoId =
        {
            @"JLG" + TestCaseNumber + @"A01",
            @"JLG" + TestCaseNumber + @"A02"
        };
        
        string _tableDetailsNotFound;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        
        [ClassCleanup]
        public new static void ClassCleanUp() => WebBase.ClassCleanUp();

        [TestInitialize]
        public new void TestInitialize() {}
      
        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
            MTNBase.MTNTestCleanup(TestContext);
            MTNBase.MTNCleanup();
        }


        private void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void CreationBGPEntriesWhenSplittingCargo()
        {

            MTNInitialize();

            // Step 3 - 6
            FormObjectBase.MainForm.OpenVoyageEnquiryFromToolbar();
            _voyageEnquiryForm = new VoyageEnquiryForm();

            _voyageEnquiryForm.FindVoyageByVoyageCode(VoyageCode);
            // MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages, @"Code^" + VoyageCode, ClickType.DoubleClick);
            _voyageEnquiryForm.TblVoyages.FindClickRow(["Code^" + VoyageCode], ClickType.DoubleClick);

            _voyageDefinitionForm = new VoyageDefinitionForm(@"MSC KATYA R. 45748 - (MSCK045748001) Voyage Definition " + terminalId);
            _voyageDefinitionForm.ExchangeDetailsTab();
            _voyageDefinitionForm.DoEdit();
            var now = DateTime.Now;
            var thirdOfTheMonth = new DateTime(now.Year, now.Month, 3);
            _voyageDefinitionForm.TxtGeneralLandingDate.SetValue(thirdOfTheMonth.ToString(@"ddMMyyyy"));
            _voyageDefinitionForm.DoSave();
            

            // Step 7 - 9
            FormObjectBase.MainForm.OpenVoyageOperationsFromToolbar();
            _voyageOperationsForm = new VoyageOperationsForm(@"Voyage Operations " + terminalId);
          
            _voyageOperationsForm.DoSearchForVoyageGetDetails(new VoyageOperationsSearcherArguments
            {
                Voyage = "MSCK045748001 MSC KATYA R. 45748",
                LOLOBays = true,
                ShowOffsiteCargo = true,
                ShowPrenotedCargo = true
            });
            _voyageOperationsForm.GetMainDetails();
            //_voyageOperationsForm.GetPrenotedDetails();
            
            MoveItViaCargoEnquiryDirect(_voyageOperationsForm.TblPrenotes1.GetElement(), 0);

            // Step 13 - 14
            var expiryDate = _currentDateTime.AddDays(10);
            var anniversary = _currentDateTime.AddDays(10);
            CheckFreeStorageDateCargoDirect(expiryDate, anniversary, @"ID^" + _cargoId[0] + "~Location^FSA3");

            // Step 15 - 16
            FormObjectBase.MainForm.OpenBackgroundProcessingFromToolbar();
            _backgroundApplicationForm = new BackgroundApplicationForm();
            
            _backgroundApplicationForm.TblBackgroundProcesses.FindClickRow(new[] { "Application Type^Future Timer Runner" });
            
            var actionFutureDate = _currentDateTime.AddDays(11);
            _backgroundApplicationForm.CheckFutureStorageBGP(new FutureStorageDetailsToValidateOrDeleteArgs
            {
                CargoId = _cargoId[0],
                DetailsToValidateOrDelete = new []
                    { $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate:dd/MM/yyyy hh:mm}~Terminal^TT1~Cargo Id^{_cargoId[0]}~Location^FSA3" }, 
            }, out _tableDetailsNotFound);

            // Step 17 - 20
            _voyageOperationsForm.SetFocusToForm();
            _voyageOperationsForm.GetMainDetails();
            _voyageOperationsForm.CmbDischargeTo.SetValue(@"FSA4");

            //_voyageOperationsForm.GetPrenotedDetails();
            // MTNControlBase.FindClickRowInTable(_voyageOperationsForm.TblPrenotes,
                // @"ID^" + _cargoId[0], ClickType.Click, rowHeight: 16);
            _voyageOperationsForm.TblPrenotes1.FindClickRow(["ID^" + _cargoId[0]], ClickType.Click);
            var startPoint = Mouse.Position;
            // MTNControlBase.FindClickRowInTable(_voyageOperationsForm.TblOnTerminal,
                // @"ID^" + _cargoId[0] + "~Location^FSA3", ClickType.Click, rowHeight: 16);
            _voyageOperationsForm.TblOnTerminal1.FindClickRow(["ID^" + _cargoId[0] + "~Location^FSA3"], ClickType.Click);
            var endPoint = Mouse.Position;

            Mouse.Drag(startPoint, endPoint);

            _confirmWeightToMoveForm = new ConfirmWeightToMoveForm();
            MTNControlBase.SetWeightValues(_confirmWeightToMoveForm.tblWeight, @"2.000", @"MT");
            _confirmWeightToMoveForm.btnOK.DoClick();

            CheckFreeStorageDateCargoDirect(expiryDate, anniversary, @"ID^" + _cargoId[0] + "~Location^FSA4");
            
            // Step 21 - 23
           _backgroundApplicationForm.SetFocusToForm();
           _backgroundApplicationForm.CheckFutureStorageBGP(new FutureStorageDetailsToValidateOrDeleteArgs
           {
               CargoId = _cargoId[0],
               DetailsToValidateOrDelete = new []
               { $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate:dd/MM/yyyy hh:MM}~Terminal^TT1~Cargo Id^{_cargoId[0]}~Location^FSA4" }, 
           }, out _tableDetailsNotFound);

            // Step 24 - 29
            MoveItViaCargoEnquiryDirect(_voyageOperationsForm.TblOnVessel1.GetElement(), 1);

            expiryDate = new DateTime(_currentDateTime.Year, _currentDateTime.Month, 13);
            anniversary = new DateTime(_currentDateTime.Year, _currentDateTime.Month, 13); 
            CheckFreeStorageDateCargoDirect(expiryDate, anniversary, @"ID^" + _cargoId[1] + "~Location^FSA3");

            // Step 30 - 31
            actionFutureDate = new DateTime(_currentDateTime.Year, _currentDateTime.Month, 14);
             ////CheckFutureStorageBGP(@"FSA3", 1, actionFutureDate);
             _backgroundApplicationForm.SetFocusToForm();
            _backgroundApplicationForm.CheckFutureStorageBGP(new FutureStorageDetailsToValidateOrDeleteArgs
            {
                CargoId = _cargoId[1],
                DetailsToValidateOrDelete = new []
                    { $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate:dd/MM/yyyy hh:mm}~Terminal^TT1~Cargo Id^{_cargoId[1]}~Location^FSA3" }, 
            }, out _tableDetailsNotFound);

            // Step 32 - 36
            _voyageOperationsForm.SetFocusToForm();
            _voyageOperationsForm.GetMainDetails();
            _voyageOperationsForm.CmbDischargeTo.SetValue(@"FSA4");

            _voyageOperationsForm.TblOnVessel1.FindClickRow(
                new[] { "ID^" + _cargoId[1] });
            startPoint = Mouse.Position;
            // MTNControlBase.FindClickRowInTable(_voyageOperationsForm.TblOnTerminal,
                // @"ID^" + _cargoId[1] + "~Location^FSA3", rowHeight: 16);
            _voyageOperationsForm.TblOnTerminal1.FindClickRow(["ID^" + _cargoId[1] + "~Location^FSA3"]);
            endPoint = Mouse.Position;

            Mouse.Drag(startPoint, endPoint);

            _confirmWeightToMoveForm = new ConfirmWeightToMoveForm();
            MTNControlBase.SetWeightValues(_confirmWeightToMoveForm.tblWeight, @"2.000", @"MT");
            _confirmWeightToMoveForm.btnOK.DoClick();

            CheckFreeStorageDateCargoDirect(expiryDate, anniversary, @"ID^" + _cargoId[1] + "~Location^FSA4");

            // Step 37 - 38
            _backgroundApplicationForm.SetFocusToForm();
            _backgroundApplicationForm.CheckFutureStorageBGP(new FutureStorageDetailsToValidateOrDeleteArgs
            {
                CargoId = _cargoId[1],
                DetailsToValidateOrDelete = new []
                    { $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate:dd/MM/yyyy hh:mm}~Terminal^TT1~Cargo Id^{_cargoId[1]}~Location^FSA4" }, 
            }, out _tableDetailsNotFound);

            // Step 39 - 45
            LogInto<MobileAppsLogInOutBO>();
            
            HomePage = new MA_TilesPage(TestContext);
            HomePage.ClickTile(HomePage.BtnReactCargoLander);
            MA_CargoLanderVoyagesPage cargoLanderVoyages = new MA_CargoLanderVoyagesPage(TestContext);

            cargoLanderVoyages.DoVoyageCargoProcess(VoyageCode, MA_CargoLanderVoyagesPage.constDischargeMachine);

            MA_CargoLanderSearchPage cargoLanderSearch = new MA_CargoLanderSearchPage(TestContext);
            cargoLanderSearch.DoSearchForCargoID(_cargoId[1]);
            cargoLanderSearch.DoClickChevron(_cargoId[1]);

            MA_CargoLanderMovePage movePage = new MA_CargoLanderMovePage(TestContext);
            string[,] fieldValueToSet =
            {
                { MA_CargoLanderMovePage.ConstNewLocation, @"FSA5"},
                { MA_CargoLanderMovePage.ConstWeightToMoveMT, @"MT"},
                { MA_CargoLanderMovePage.ConstWeightToMove, @"2.000" }
            };
            movePage.SetFieldsOnPage(fieldValueToSet);
            movePage.DoComplete();

            cargoLanderSearch = new MA_CargoLanderSearchPage(TestContext);
            cargoLanderSearch.ReturnToTilesPage();

            // Step 46 - 47
            FormObjectBase._mainWindow.Focus();

            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry " + terminalId);

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Clinker, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", _cargoId[1]);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Terminal Area", @"FSA5", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true, doDownArrow: true,
                searchSubStringTo: 3);
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.SearchToolbar,
                (int)CargoEnquiryForm.Toolbar.SearcherToolbar.Search, "Search");

            cargoEnquiryForm.tblData2.FindClickRow(new[]
            {
                $"ID^{_cargoId[1]}~Total Quantity^1~Location ID^FSA5~Cargo Type^Clinker~Free Storage Expiry Date^" +
                expiryDate.ToString(@"dd/MM/yyyy") + @"~Future Storage Anniversary^" +
                anniversary.ToString(@"dd/MM/yyyy")
            });
           
            // Step 48 - 49
            _backgroundApplicationForm.SetFocusToForm();
            _backgroundApplicationForm.CheckFutureStorageBGP(new FutureStorageDetailsToValidateOrDeleteArgs
            {
                CargoId = _cargoId[1],
                DetailsToValidateOrDelete = new []
                    { $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate:dd/MM/yyyy hh:mm}~Terminal^TT1~Cargo Id^{_cargoId[1]}~Location^FSA5" }, 
            }, out _tableDetailsNotFound);

            // #51525 Checking
            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^" + _cargoId[1] + @"~Location ID^FSA5~Cargo Type^Clinker~Total Weight MT^2.000" });

        }

        /// <summary>
        /// Moves cargo via the Cargo Enquiry Direct form.
        /// Selects the specified cargo in the provided table, initiates the move process,
        /// sets the move details (area type, terminal area, weight), confirms the move,
        /// and closes the related forms.
        /// </summary>
        /// <param name="tableToCheck">The automation element representing the table to search for the cargo.</param>
        /// <param name="cargoIndex">The index of the cargo in the _cargoId array to move.</param>
        private void MoveItViaCargoEnquiryDirect(AutomationElement tableToCheck, int cargoIndex)
        {
            _voyageOperationsForm.SetFocusToForm();

            MTNControlBase.FindClickRowInTable(tableToCheck,
                @"ID^" + _cargoId[cargoIndex], ClickType.DoubleClick, rowHeight: 16);

            // Step 10 - 14
            _cargoEnquiryDirectForm = new CargoEnquiryDirectForm();
            _cargoEnquiryDirectForm.DoMoveIt();

            _cargoMoveItForm = new CargoMoveItForm(formTitle: "Move " + _cargoId[cargoIndex] + " TT1");
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"To Area Type", @"Block Stack",
                EditRowDataType.ComboBoxEdit, waitTime: 150);
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"To Terminal Area", @"FSA3",
                EditRowDataType.ComboBoxEdit, waitTime: 150);
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"Weight", @"1",
                EditRowDataType.Text, waitTime: 150);

            _cargoMoveItForm.DoMoveIt();
            
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(3));
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Move - 1 Items");
            _confirmationFormYesNo.btnYes.DoClick();

            _cargoMoveItForm.SetFocusToForm();
            _cargoMoveItForm.CloseForm();
        }


        private void CheckFreeStorageDateCargoDirect(DateTime expiryDate, DateTime anniversary, string detailsToSearchFor)
        {
            _voyageOperationsForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_voyageOperationsForm.TblOnTerminal,
                // detailsToSearchFor, ClickType.DoubleClick, rowHeight: 16);
            _voyageOperationsForm.TblOnTerminal1.FindClickRow([detailsToSearchFor], ClickType.DoubleClick);

            _cargoEnquiryDirectForm = new CargoEnquiryDirectForm();
            MTNControlBase.ValidateValueInEditTable(_cargoEnquiryDirectForm.tblGeneral, @"Free Storage Days Assigned",
                @"10");
            MTNControlBase.ValidateValueInEditTable(_cargoEnquiryDirectForm.tblGeneral, @"Free Storage Expiry Date",
                expiryDate.ToString(@"dd/MM/yyyy"));
            MTNControlBase.ValidateValueInEditTable(_cargoEnquiryDirectForm.tblGeneral, @"Future Storage Anniversary",
                anniversary.ToString(@"dd/MM/yyyy"));
            _cargoEnquiryDirectForm.CloseForm();
        }


        #region - Setup and Run Data Loads
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber + "_";

            // Delete Prenote
            CreateDataFileToLoad(@"DeletePrenote.xml",
                "<?xml version='1.0'?>\n<JMTInternalPrenote xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalPrenote.xsd'>\n	<AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_LOADED;EDI_STATUS_VERIFIED_WARNINGS / EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<Prenote Terminal='TT1'>\n				<cargoTypeDescr>Clinker</cargoTypeDescr>\n				<commodity>CLNK</commodity>\n				<dischargePort>USJAX</dischargePort>\n				<id>JLG45748A01</id>\n				<imexStatus>Import</imexStatus>\n				<messageMode>D</messageMode>\n				<operatorCode>PAC</operatorCode>\n				<weight>6000</weight>\n				<voyageCode>" + VoyageCode + "</voyageCode>\n				<totalQuantity>200</totalQuantity>\n			</Prenote>\n	</AllPrenote>\n</JMTInternalPrenote>\n");

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n			<cargoTypeDescr>Clinker</cargoTypeDescr>\n			<commodity>CLNK</commodity>\n            <id>JLG45748A02</id>\n            <operatorCode>PAC</operatorCode>\n            <locationId>FSA3</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>" + VoyageCode + "</voyageCode>\n            <totalQuantity>300</totalQuantity>\n		   <messageMode>D</messageMode>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n			<cargoTypeDescr>Clinker</cargoTypeDescr>\n			<commodity>CLNK</commodity>\n			<dischargePort>USJAX</dischargePort>\n			<id>JLG45748A01</id>\n			<imexStatus>Import</imexStatus>\n			<locationId>FSA3</locationId>\n			<operatorCode>PAC</operatorCode>\n			<weight>6000</weight>\n			<voyageCode>" + VoyageCode + "</voyageCode>\n			<totalQuantity>200</totalQuantity>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n			<cargoTypeDescr>Clinker</cargoTypeDescr>\n			<commodity>CLNK</commodity>\n            <id>JLG45748A02</id>\n            <operatorCode>PAC</operatorCode>\n            <locationId>FSA4</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>" + VoyageCode + "</voyageCode>\n            <totalQuantity>300</totalQuantity>\n		   <messageMode>D</messageMode>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n			<cargoTypeDescr>Clinker</cargoTypeDescr>\n			<commodity>CLNK</commodity>\n			<dischargePort>USJAX</dischargePort>\n			<id>JLG45748A01</id>\n			<imexStatus>Import</imexStatus>\n			<locationId>FSA4</locationId>\n			<operatorCode>PAC</operatorCode>\n			<weight>6000</weight>\n			<voyageCode>" + VoyageCode + "</voyageCode>\n			<totalQuantity>200</totalQuantity>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n			<cargoTypeDescr>Clinker</cargoTypeDescr>\n			<commodity>CLNK</commodity>\n            <id>JLG45748A02</id>\n            <operatorCode>PAC</operatorCode>\n            <locationId>FSA5</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>" + VoyageCode + "</voyageCode>\n            <totalQuantity>300</totalQuantity>\n		   <messageMode>D</messageMode>\n        </CargoOnSite>\n          </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Delete Cargo On Ship
            CreateDataFileToLoad(@"DeleteCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>Clinker</cargoTypeDescr>\n			<commodity>CLNK</commodity>\n            <id>JLG45748A02</id>\n            <operatorCode>PAC</operatorCode>\n            <locationId>" + VoyageCode + "</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>" + VoyageCode + "</voyageCode>\n            <totalQuantity>300</totalQuantity>\n		   <messageMode>D</messageMode>\n        </CargoOnShip>\n          </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // Create Cargo On Ship
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n			<cargoTypeDescr>Clinker</cargoTypeDescr>\n			<commodity>CLNK</commodity>\n            <id>JLG45748A02</id>\n            <operatorCode>PAC</operatorCode>\n            <locationId>" + VoyageCode + "</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>" + VoyageCode + "</voyageCode>\n            <totalQuantity>300</totalQuantity>\n		   <messageMode>A</messageMode>\n        </CargoOnShip>\n          </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // Create Prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0'?>\n<JMTInternalPrenote xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalPrenote.xsd'>\n	<AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<Prenote Terminal='TT1'>\n				<cargoTypeDescr>Clinker</cargoTypeDescr>\n				<commodity>CLNK</commodity>\n				<dischargePort>USJAX</dischargePort>\n				<id>JLG45748A01</id>\n				<imexStatus>Import</imexStatus>\n				<messageMode>A</messageMode>\n				<operatorCode>PAC</operatorCode>\n				<weight>6000</weight>\n				<voyageCode>" + VoyageCode + "</voyageCode>\n				<totalQuantity>200</totalQuantity>\n			</Prenote>\n	</AllPrenote>\n</JMTInternalPrenote>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }

}
