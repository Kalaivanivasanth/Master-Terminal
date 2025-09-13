using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General.Future_Storage
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45742 : MTNBase
    {
        private CargoMoveItForm _cargoMoveItForm;
        private ConfirmationFormYesNo _confirmationFormYesNo;
        BackgroundApplicationForm _backgroundApplicationForm;
        
        string _tableDetailsNotFound;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();
            
            SetupAndLoadInitializeData(TestContext);

            //FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Operations|Voyage Operations");
            FormObjectBase.MainForm.OpenVoyageOperationsFromToolbar();
            VoyageOperationsForm voyageOperationsForm = new VoyageOperationsForm();
            voyageOperationsForm.DoSearchForVoyageGetDetails(new VoyageOperationsSearcherArguments
            {
                Voyage = "MSCK000002 MSC KATYA R.",
                LOLOBays = true,
            });
            voyageOperationsForm.GetMainDetails();

            //voyageOperationsForm.SetValue(voyageOperationsForm.cmbDischargeTo, @"FSA3");
            voyageOperationsForm.CmbDischargeTo.SetValue(TT1.TerminalArea.FSA3);
            /*MTNControlBase.FindClickRowInTable(voyageOperationsForm.tblOnVessel,
                @"ID^JLG45742A01~Location^MSCK000002~Total Quantity^300~Cargo Type^STEEL", ClickType.ContextClick, rowHeight: 16);*/
            voyageOperationsForm.TblOnVessel1.FindClickRow(
                new[] { "ID^JLG45742A01~Location^MSCK000002~Total Quantity^300~Cargo Type^STEEL" }, 
                ClickType.ContextClick);
            voyageOperationsForm.ContextMenuSelect(@"Actual Discharge|Actual Discharge Selected");
            ConfirmQuantityToMoveForm confirmQuantityToMoveForm = new ConfirmQuantityToMoveForm(formTitle: @"Confirm quantity to move");
            confirmQuantityToMoveForm.btnOK.DoClick();
        }


        [TestMethod]
        public void FutureStorageMergeCargo()
        {
            MTNInitialize();
            var cargoId = "JLG45742A01";

            CheckFutureStorageBGPExists(
                new FutureStorageDetailsToValidateOrDeleteArgs
                {
                    CargoId = cargoId,
                    DetailsToValidateOrDelete = new[]
                    {
                        "Request Type^Future Storage Process~Terminal^TT1~Cargo Id^JLG45742A01~Cargo Type^STEEL~Cargo Subtype^Pipes~Location^FSA3~Cargo Qty^300"
                    }
                });
            
            // Step 3
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            // Step 4 - 5
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Steel, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG45742A01");
            cargoEnquiryForm.DoSearch();

            MoveCargoCheckBGPFutureStorage(@"ID^JLG45742A01~Total Quantity^300~Location ID^FSA3",
                new FutureStorageDetailsToValidateOrDeleteArgs
                {
                    CargoId = cargoId,
                    DetailsToValidateOrDelete = new[]
                    {
                        "Request Type^Future Storage Process~Terminal^TT1~Cargo Id^JLG45742A01~Cargo Type^STEEL~Cargo Subtype^Pipes~Location^FSA3~Cargo Qty^290",
                        "Request Type^Future Storage Process~Terminal^TT1~Cargo Id^JLG45742A01~Cargo Type^STEEL~Cargo Subtype^Pipes~Location^FSA4~Cargo Qty^10",
                    }});

            MoveCargoCheckBGPFutureStorage("ID^JLG45742A01~Total Quantity^290~Location ID^FSA3",
                new FutureStorageDetailsToValidateOrDeleteArgs
                {
                    CargoId = cargoId,
                    DetailsToValidateOrDelete = new[]
                    {
                        "Request Type^Future Storage Process~Terminal^TT1~Cargo Id^JLG45742A01~Cargo Type^STEEL~Cargo Subtype^Pipes~Location^FSA3~Cargo Qty^280",
                        "Request Type^Future Storage Process~Terminal^TT1~Cargo Id^JLG45742A01~Cargo Type^STEEL~Cargo Subtype^Pipes~Location^FSA4~Cargo Qty^20"
                    }});
        }

        private void MoveCargoCheckBGPFutureStorage(string cargoEnquirySearchDetails, FutureStorageDetailsToValidateOrDeleteArgs args)
        {
            if (cargoEnquiryForm == null)
            {
                FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
                cargoEnquiryForm = new CargoEnquiryForm();
            }
            else
            {
                cargoEnquiryForm.SetFocusToForm();
            }
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, cargoEnquirySearchDetails);
            cargoEnquiryForm.tblData2.FindClickRow([cargoEnquirySearchDetails]);
            cargoEnquiryForm.DoMoveIt();

            _cargoMoveItForm = new CargoMoveItForm(formTitle: "Move JLG45742A01 TT1");
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"Move Mode", @"Instant Move", EditRowDataType.ComboBox,waitTime: 150);
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"To Area Type", @"Block Stack", EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"To Terminal Area", @"FSA4", EditRowDataType.ComboBoxEdit, waitTime: 150);
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"Quantity", @"10",
                waitTime: 150);
             //FormObjectBase.ClickButton(_cargoMoveItForm.btnMoveIt);
             //_cargoMoveItForm.btnMoveIt.DoClick();
             _cargoMoveItForm.DoMoveIt();

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Move - 1 Items");
             _confirmationFormYesNo.btnYes.DoClick();
            _cargoMoveItForm.CloseForm();
            
            CheckFutureStorageBGPExists(args);
        }
      
        void CheckFutureStorageBGPExists(FutureStorageDetailsToValidateOrDeleteArgs args)
        {
            if (_backgroundApplicationForm == null)
            {
                FormObjectBase.MainForm.OpenBackgroundProcessingFromToolbar();
                _backgroundApplicationForm = new BackgroundApplicationForm();
            }
            else
                _backgroundApplicationForm.SetFocusToForm();
            
            _backgroundApplicationForm.CheckFutureStorageBGP(new FutureStorageDetailsToValidateOrDeleteArgs
            {
                CargoId = args.CargoId,
                DetailsToValidateOrDelete = args.DetailsToValidateOrDelete
            }, out _tableDetailsNotFound); 
            
            
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_45742_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n			<cargoTypeDescr>STEEL</cargoTypeDescr>\n            <product>PIPE</product>\n            <id>JLG45742A01</id>\n            <operatorCode>PAC</operatorCode>\n            <locationId>FSA3</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>30</totalQuantity>\n		   <messageMode>D</messageMode>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n			<cargoTypeDescr>STEEL</cargoTypeDescr>\n            <product>PIPE</product>\n            <id>JLG45742A01</id>\n            <operatorCode>PAC</operatorCode>\n            <locationId>FSA4</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>30</totalQuantity>\n		   <messageMode>D</messageMode>\n        </CargoOnSite>\n          </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n");

            // Delete Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n			<cargoTypeDescr>STEEL</cargoTypeDescr>\n            <product>PIPE</product>\n            <id>JLG45742A01</id>\n            <operatorCode>PAC</operatorCode>\n            <locationId>MSCK000002</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>300</totalQuantity>\n		   <messageMode>A</messageMode>\n        </CargoOnShip>\n          </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n");


            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
