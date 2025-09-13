using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44868 : MTNBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
       
        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();
            CallJadeScriptToRun(TestContext, "resetData_44868");
            SetupAndLoadInitializeData(TestContext);
        }

        [TestMethod]
        public void ImportFreightForwarderFalse()
        {
            MTNInitialize();

            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            string[] detailsToSearchFor =
            {
                @"Cargo Type^ISO Container^^^",
                @"Cargo ID^JLG44868B01^^^",
                @"Site (Current)^On Site^^^"
            };

            // Step 8 - 11
            cargoEnquiryForm.SearchForCargoItems(detailsToSearchFor, @"Location ID^MKBS01~ID^JLG44868B01");
            cargoEnquiryForm.CloseForm();

            // Step 12 - 15
            ReleaseContainer(@"TC44868C", @"JLG44868B01");

            // Step 15 - 20
            DeleteViaRoadOperations(@"TC44868C");

            // Step 21 - 24
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            detailsToSearchFor = new []
            {
                @"Cargo Type^ISO Container^^^",
                @"Cargo ID^JLG44868B01^^^",
                @"Site (Current)^On Site^^^"
            };
            EditCargo(@"JLG44868B01", detailsToSearchFor, @"Location ID^MKBS01~ID^JLG44868B01");

            // Step 26 - 28
            ReleaseContainer(@"TC44868C", @"JLG44868B01");

            // Step 29 - 31
            DeleteViaRoadOperations(@"TC44868C");

        }
       
        private void DeleteViaRoadOperations(string vehicleId)
        {
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^" + vehicleId,
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date     ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { $"Vehicle Id^{vehicleId}", }, ClickType.ContextClick); 
            roadOperationsForm.DeleteCurrentEntry(@"TESTING");

            roadOperationsForm.SetFocusToForm();
            roadOperationsForm.CloseForm();
        }

        private void ReleaseContainer(string registration, string cargoId, string[] warningErrorToCheck = null)
        {
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: $"Road Gate {terminalId}");
            roadGateForm.cmbCarrier.SetValue(Carrier.CARRIER1);
            roadGateForm.txtRegistration.SetValue(registration);
            roadGateForm.txtDriverCode.SetValue(@"TEST");
            roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.btnReleaseFull.DoClick();

            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm(formTitle:
                $"Release Full Container  {terminalId}");
            roadGateDetailsReleaseForm.TxtCargoId.SetValue(cargoId);
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();

            try
            {
                /*// Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date 
                warningErrorForm = new WarningErrorForm(formTitle: $@"Warnings for Gate In/Out {terminalId}");
                //warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
                warningErrorForm.btnSave.DoClick();*/
                WarningErrorForm.CompleteWarningErrorForm($"Warnings for Gate In/Out {terminalId}");
            }
            catch { }

       
        }

        private void EditCargo(string cargoId, string[] detailsToSearchFor, string cargoItemToFindInTable)
        {
            cargoEnquiryForm.SearchForCargoItems(detailsToSearchFor, cargoItemToFindInTable);
            cargoEnquiryForm.DoEdit();
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"IMEX Status", @"Import",
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSave();

            /*// Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date 
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Tracked Item Update TT1");
            string[] warningErrorToCheck = new []
            {
                "Code :75016. The Container Id (" + cargoId + ") failed the validity checks and may be incorrect."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm($"Warnings for Tracked Item Update {terminalId}", new []
            {  "Code :75016. The Container Id (" + cargoId + ") failed the validity checks and may be incorrect." });

            cargoEnquiryForm.GetGenericTabTableDetails(@"Status", @"4087");

            MTNControlBase.FindClickRowInTableVHeader(cargoEnquiryForm.tblGeneric, @"Stops (dbl click)", clickType: ClickType.DoubleClick);

            StopsForm stopsForm = new StopsForm();
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(stopsForm.tblStops, @"Stop^STOP_39019", xOffset: 130);
            stopsForm.TblStops.FindClickRow(new[] { "Stop^STOP_39019" }, xOffset: 130);
            stopsForm.btnSaveAndClose.DoClick();

            cargoEnquiryForm.CloseForm();


        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_44868_";
            
             // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44868</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44868B01</id>\n      <isoType>2200</isoType>\n      <operatorCode>BALT</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Storage</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44868</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44868B01</id>\n      <isoType>2200</isoType>\n      <operatorCode>BALT</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Storage</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            CallJadeToLoadFiles(testContext);
            
        }

    }

}
