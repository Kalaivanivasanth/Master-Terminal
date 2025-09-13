using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using FlaUI.Core.Input;
using HardcodedData.SystemData;
using MTNForms.Controls.Table;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44969 : MTNBase
    {

        CargoTypesForm _cargoTypesForm;
        SystemAdminForm _systemAdminForm;
        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;

        const string TestCaseNumber = @"44969";
        const string CargoId = @"JLG" + TestCaseNumber + @"A01";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();

            InitializeDetails();
        }


        [TestMethod]
        public void UseTareCargoWeightSetToYes()
        {

            MTNInitialize();
            

            // Step 6 - 12
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"TC" + TestCaseNumber);
            roadGateForm.SetRegoCarrierGate(TestCaseNumber, "CARRIER1");
            roadGateForm.btnReceiveCargo.DoClick();

            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive General Cargo TT1");
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(CargoId);
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("41.200", "MT");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive General Cargo~Detail^" + CargoId + "(); MSC; Bauxite");
            roadGateForm.TblGateItems.FindClickRow([$"Type^Receive General Cargo~Detail^{CargoId}(); MSC; Bauxite"]);
            roadGateForm.btnSave.DoClick();

            // Step 14 - 15: Columns have now been permanently selected
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Bauxite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                // @"ID^" + CargoId + @"~Total Weight MT^41.200~Cargo Weight MT^41.200");
            cargoEnquiryForm.tblData2.FindClickRow([$"ID^{CargoId}~Total Weight MT^41.200~Cargo Weight MT^41.200"]);

            // Step 16 - 19
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^" + TestCaseNumber, ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow([$"Vehicle Id^{TestCaseNumber}"], ClickType.ContextClick);
            roadOperationsForm.DeleteCurrentEntry(@"TESTING");

            roadOperationsForm.SetFocusToForm();
            roadOperationsForm.CloseForm();

            // Step 20 : Covered by TerminalSystemResetConfigs

        }

        private void InitializeDetails()
        {

            // Change Cargo Type
            FormObjectBase.NavigationMenuSelection(@"System Ops|Cargo Types");
            _cargoTypesForm = new CargoTypesForm();

            MTNControlBase.SetValueInEditTable(_cargoTypesForm.tblSearch, @"Abbreviation", @"BAUX");
            //_cargoTypesForm.btnSearch.DoClick();
            _cargoTypesForm.DoSearch();
             // MTNControlBase.FindClickRowInTable(_cargoTypesForm.tblCargoDetails,
                // @"Abbreviation^BAUX~Description^Bauxite~Track As^Bulk");
            //_cargoTypesForm.TabGeneric.TableWithHeader.FindClickRow(["Abbreviation^BAUX~Description^Bauxite~Track As^Bulk"]);
            _cargoTypesForm.tblCargoDetails1.FindClickRow(["Abbreviation^BAUX~Description^Bauxite~Track As^Bulk"]);
            _cargoTypesForm.GetDetailsTab();

            //_cargoTypesForm.btnEdit.DoClick();
            _cargoTypesForm.DoEdit();
            MTNControlBase.SetValueInEditTable(_cargoTypesForm.tblDetails, @"Commodity Required", @"1", EditRowDataType.CheckBox);
            MTNControlBase.SetValueInEditTable(_cargoTypesForm.tblDetails, @"Use Tare/Cargo Weight", @"1", EditRowDataType.CheckBox, doReverse: true);
            _cargoTypesForm.DoSave();
            _cargoTypesForm.CloseForm();

            // Set Commodity GEN Cargo Types to include BAUX
            //FormObjectBase.NavigationMenuSelection(@"System Admin", forceReset: false);
            FormObjectBase.MainForm.OpenSystemAdminFromToolbar();
            _systemAdminForm = new SystemAdminForm(@"System Administration");

            _systemAdminForm.cmbTable.SetValue(@"Commodities");
            _systemAdminForm.txtFilter.SetValue(@"GEN");
            Wait.UntilResponsive(_systemAdminForm.TblAdministrationItemsRH19A.GetElement());
           
            _systemAdminForm.TblAdministrationItemsRH19A.FindClickRow(["Commodity Code^GEN~Description^General"], searchType: SearchType.Exact);
            _systemAdminForm.DoEdit();

            _systemAdminForm.GetGenericTabAndLists(@"Cargo Types", "4089");
            
            _systemAdminForm.lstGeneric.MoveItemsBetweenList(_systemAdminForm.lstGeneric.LstLeft, new [] { "Bauxite" });
            _systemAdminForm.DoSave();
            _systemAdminForm.CloseForm();

            SetupAndLoadInitializeData(TestContext);

        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber + "_";

            // Delete Prenote
            CreateDataFileToLoad(@"DeletePrenote.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED / EDI_STATUS_DBLOADED_PARTIAL / EDI_STATUS_DBLOADED_PARTIAL_X</operationsToPerformStatuses>\n        <Prenote Terminal='TT1'>\n            <cargoTypeDescr>Bauxite</cargoTypeDescr>\n			<id>JLG44969A01</id>\n            <imexStatus>Export</imexStatus>\n			<cargoType>BAUX</cargoType>\n            <weight>41200</weight>\n			<voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS01</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>D</messageMode>\n        </Prenote>\n    </AllPrenote>\n</SystemXMLPrenote>\n\n");

            // Create Prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <Prenote Terminal='TT1'>\n            <cargoTypeDescr>Bauxite</cargoTypeDescr>\n			<id>JLG44969A01</id>\n            <imexStatus>Export</imexStatus>\n			<cargoType>BAUX</cargoType>\n            <weight>245454000</weight>\n			<voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS01</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n\n    </AllPrenote>\n</SystemXMLPrenote>\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
