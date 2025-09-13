using System;
using DataObjects.LogInOutBO;
using FlaUI.Core.Tools;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44065 : MTNBase
    {
        private SystemAdminForm _systemAdminForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {


            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();

            //////base.TestInitialize();
        }

        [TestMethod]
        public void ImportFreightForwarderTrue()
        {
            MTNInitialize();
            
            SetRequiresImportFreightForwarderForOperator(true);
           
            // call Jade to load file(s)
            searchFor = @"_44065_";
            
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            string[] detailsToSearchFor =
            {
                @"Cargo Type^ISO Container^^^",
                @"Cargo ID^JLG44065B01^^^",
                @"Site (Current)^On Site^^^"
            };

            // Step 8 - 11
            cargoEnquiryForm.SearchForCargoItems(detailsToSearchFor, @"Location ID^MKBS01~ID^JLG44065B01");
            cargoEnquiryForm.CloseForm();

            // Step 12 - 15
            string[] warningErrorToCheck = null;
            ReleaseContainer(@"TC44065", @"JLG44065B01", warningErrorToCheck);

            // Step 15 - 20
            DeleteViaRoadOperations(@"TC44065");

            // Step 21 - 24
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, 
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", "JLG44065B01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", "On Site", fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.SearchToolbar,
                (int)CargoEnquiryForm.Toolbar.SearcherToolbar.Search, "Search");

            
            EditCargo(@"JLG44065B01", detailsToSearchFor, @"Location ID^MKBS01~ID^JLG44065B01");

            // SEtp 26 - 28
            warningErrorToCheck = new string[]
            {
                "Code :75783. The value entered for Cargo Freight Forwarder is invalid. " + @"JLG44065B01"
            };
            ReleaseContainer(@"TC44065", @"JLG44065B01", warningErrorToCheck);

        }
       
        private void DeleteViaRoadOperations(string vehicleId)
        {
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^" + vehicleId,
             // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date    ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { $"Vehicle Id^{vehicleId}" }, ClickType.ContextClick);
            roadOperationsForm.DeleteCurrentEntry(@"TESTING");

            roadOperationsForm.SetFocusToForm();
            roadOperationsForm.CloseForm();
        }

        private void ReleaseContainer(string registration, string cargoId, string[] warningErrorToCheck, bool checkForMatch = false)
        {
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"44065");
            
            roadGateForm.cmbCarrier.SetValue(Carrier.CARRIER1);
            Retry.WhileException(() =>
            {
                roadGateForm.txtRegistration.SetValue(registration, waitTime:150, checkForMatch: true);

                if (checkForMatch)
                {
                    roadGateForm.txtRegistration.ValidateText(registration);
                }

            }, TimeSpan.FromSeconds(30), null, true, "Failed to validate the text so retrying...");
            roadGateForm.txtDriverCode.SetValue(@"TEST");
            roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.btnReleaseFull.DoClick();

            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm(formTitle:
                @"Release Full Container  TT1");

            roadGateDetailsReleaseForm.TxtCargoId.SetValue(cargoId);
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            if (warningErrorToCheck?.Length > 0)
            {
                warningErrorForm = new WarningErrorForm(formTitle: @"Errors for Gate In/Out TT1");
                warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
                warningErrorForm.btnCancel.DoClick();

                roadGateDetailsReleaseForm.DoCancel();

                roadGateDetailsReleaseForm.UnsavedChanges();

                roadGateForm.SetFocusToForm();
                roadGateForm.btnCancel.DoClick();

                roadGateForm.VehicleCancel(@"TESTING");
            }
            else
            {
                roadGateForm.SetFocusToForm();
                roadGateForm.btnSave.DoClick();

                if(warningErrorToCheck?.Length > 0)
                {
                    warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
                    warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
                    warningErrorForm.btnSave.DoClick();
                }
            }

            roadGateForm.CloseForm();
        }

        private void SetRequiresImportFreightForwarderForOperator(bool setRequiresImportFreightForwarder)
        {
            if (_systemAdminForm == null)
            {
                FormObjectBase.MainForm.OpenSystemAdminFromToolbar();
                _systemAdminForm = new SystemAdminForm(@"System Administration");
            }
            else
            {
                _systemAdminForm.SetFocusToForm();
            }

            _systemAdminForm.cmbTable.SetValue(@"Operators");

            _systemAdminForm.TblAdministrationItemsRH19A.FindClickRow(["Code^BALT~Description^BALTICON"], searchType: SearchType.Exact);
            _systemAdminForm.DoEdit();
            MTNControlBase.SetValueInEditTable(_systemAdminForm.tblDetails, @"Requires Import Freight Forwarder",
                Convert.ToInt32(setRequiresImportFreightForwarder).ToString(), EditRowDataType.CheckBox);
            _systemAdminForm.DoSave();
        }


        private void EditCargo(string cargoId, string[] detailsToSearchFor, string cargoItemToFindInTable)
        {
            cargoEnquiryForm.DoEdit();
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"IMEX Status", @"Import",
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);

            cargoEnquiryForm.DoSave();

            WarningErrorForm warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Tracked Item Update TT1");
            string[] warningErrorToCheck = new string[]
            {
                "Code :75016. The Container Id (" + cargoId + ") failed the validity checks and may be incorrect."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();

            cargoEnquiryForm.GetGenericTabTableDetails(@"Status", @"4087");

            MTNControlBase.FindClickRowInTableVHeader(cargoEnquiryForm.tblGeneric, @"Stops (dbl click)", clickType: ClickType.DoubleClick);

            StopsForm stopsForm = new StopsForm();
            // MTNControlBase.FindClickRowInTable(stopsForm.tblStops, @"Stop^STOP_39019", xOffset: 130);
            stopsForm.TblStops.FindClickRow(["Stop^STOP_39019"], xOffset: 130);            stopsForm.btnSaveAndClose.DoClick();

            cargoEnquiryForm.CloseForm();


        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            
            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44065</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44065B01</id>\n      <isoType>2200</isoType>\n      <operatorCode>BALT</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Storage</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44065</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44065B01</id>\n      <isoType>2200</isoType>\n      <operatorCode>BALT</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Storage</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            CallJadeToLoadFiles(testContext);
            
        }

    }

}
