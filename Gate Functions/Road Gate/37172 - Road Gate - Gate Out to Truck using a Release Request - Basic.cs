using DataObjects.LogInOutBO;
using FlaUI.Core.WindowsAPI;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Quick_Find;
using MTNForms.FormObjects.Yard_Functions;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects.Yard_Functions.Radio_Telemetry_Queue__RTQ_;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase37172 : MTNBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
     
        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();
            
            CallJadeScriptToRun(TestContext, @"resetData_37172");
            SetupAndLoadInitializeData(TestContext);
        }


        [TestMethod]
        public void GateOutToTruckUsingReleaseRequestBasic()
        {
            MTNInitialize();
            
            // Step 6 - 9
            Keyboard.TypeSimultaneously(VirtualKeyShort.F10);
            var quickFindForm = new QuickFindForm();
            Keyboard.Type(@"F GATE", 100);
            quickFindForm.btnFind.DoClick();
            quickFindForm.CloseForm();

            roadGateForm = new RoadGateForm(formTitle: $"Road Gate {terminalId}");
            roadGateForm.SetRegoCarrierGate("37172");
            roadGateForm.txtNewItem.SetValue(@"JLGRRROAD37172A01", 10);

            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm($"Release Full Container  {terminalId}");

            Assert.IsTrue(roadGateDetailsReleaseForm.TxtRelease.GetText().Equals(@"JLGRRROAD37172A01"),
               @"TestCase37172 - Release - Actual: " + roadGateDetailsReleaseForm.TxtRelease.GetText() +
               " doesn't match Expected: JLGRRROAD37172A01");
            Assert.IsTrue(roadGateDetailsReleaseForm.CmbReleaseItem.GetValue().Equals(@"1 x 2200  - GENERAL (0 released )"),
                @"TestCase37172 - Release Item - Actual: " + roadGateDetailsReleaseForm.CmbReleaseItem.GetValue() +
                " doesn't match Expected: 1 x 2200  - GENERAL (0 released )");
            Assert.IsTrue(roadGateDetailsReleaseForm.TxtNumberOfItems.GetText().Equals(@"1"),
                @"TestCase37172 - Number of Items - Actual: " + roadGateDetailsReleaseForm.TxtNumberOfItems.GetText() +
                " doesn't match Expected: 1");

            Assert.IsTrue(roadGateDetailsReleaseForm.CmbOperator.GetValue().Equals(@"MSL	Messina Line"),
                @"TestCase37172 - Release Item - Actual: " + roadGateDetailsReleaseForm.CmbOperator.GetValue() +
                " doesn't match Expected: MSL	Messina Line");
            Assert.IsTrue(roadGateDetailsReleaseForm.CmbISOType.GetValue().Equals(@"2200	GENERAL"),
                @"TestCase37172 - Release Item - Actual: " + roadGateDetailsReleaseForm.CmbISOType.GetValue() +
                " doesn't match Expected: 2200	GENERAL");
            Assert.IsTrue(roadGateDetailsReleaseForm.CmbIMEX.GetValue().Equals(@"Import"),
                @"TestCase37172 - Release Item - Actual: " + roadGateDetailsReleaseForm.CmbIMEX.GetValue() +
                " doesn't match Expected: 1 x 2200  - GENERAL (0 released )");
            Assert.IsTrue(roadGateDetailsReleaseForm.CmbVoyage.GetValue().Equals(@"MSCK000002	MSC KATYA R."),
                @"TestCase37172 - Release Item - Actual: " + roadGateDetailsReleaseForm.CmbVoyage.GetValue() +
                " doesn't match Expected: MSCK000002	MSC KATYA R.");
            Assert.IsTrue(roadGateDetailsReleaseForm.CmbSubTerminal.GetValue().Equals(@"Depot"),
                @"TestCase37172 - Release Item - Actual: " + roadGateDetailsReleaseForm.CmbSubTerminal.GetValue() +
                " doesn't match Expected: Depot");

            // Step 10
            roadGateDetailsReleaseForm.TxtCargoId.SetValue("JLG37172A01");

            // Step 11
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            // Step 12
            /*// Tuesday, 28 January 2025 navmh5 
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            string[] warningErrorToCheck =
            {
                "Code :79688. Cargo JLG37172A01's availability status of Unavailable for Release does not match the request's availability status of Available for release"
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm($"Warnings for Gate In/Out {terminalId}",
                new[]
                {
                    "Code :79688. Cargo JLG37172A01's availability status of Unavailable for Release does not match the request's availability status of Available for release"
                });

            // Step 13
            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();
            roadGateForm.CloseForm();

            // Step 14 - 15
            Keyboard.TypeSimultaneously(VirtualKeyShort.F10);
            quickFindForm = new QuickFindForm();
            Keyboard.Type(@"F RTQ", 100);
            quickFindForm.btnFind.DoClick();

            quickFindForm.CloseForm();

            // Step 16 - 17
            var cargoId = @"JLG37172A01";
            var radioTelemetryQueueForm = new RadioTelemetryQueueForm();
            radioTelemetryQueueForm.FindCargoUsingFindCargoInTable(cargoId);

            // Step 18
            radioTelemetryQueueForm.FindClickRowInQueuedItemsTable(
                $"MKGB_Q {cargoId}^Unactioned^Pickup^MKGB01 001002 1^37172(1) Road^MSCK000002");
            radioTelemetryQueueForm.ContextMenuSelect(@"Set Task Status|Set Complete");

            // Step 19
            RTQCompleteTaskForm rtqCompleteTaskForm =
                new RTQCompleteTaskForm($"Complete Task {radioTelemetryQueueForm.GetTerminalName()}");
            rtqCompleteTaskForm.GetItemsForFirstTimeIntoComplete();
            rtqCompleteTaskForm.cmbSelectTaskMachine.SetValue(@"FK21");
            rtqCompleteTaskForm.btnOK.DoClick();
            radioTelemetryQueueForm.CloseForm();

            // Step 20 - 21
            RoadExitForm.ProcessRoadExit(@"37172");
            
            // Step 22 
            /*Keyboard.TypeSimultaneously(VirtualKeyShort.F10);
            quickFindForm = new QuickFindForm();
            Keyboard.Type(@"F CARGO", 100);
            quickFindForm.btnFind.DoClick();
            quickFindForm.CloseForm();*/

            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm($"Cargo Enquiry {terminalId}");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Blank, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG37172A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", Site.OffSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.tblData2.FindClickRow(new [] { @"ID^JLG37172A01~State^Off Site~Status^MT" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");
           var cargoEnquiryTransactionForm = 
                FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG37172A01 TT1");

            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[]
            {
                "Type^Moved~User^USERDWAT~Details^From 37172 Road to Nowhere",
                "Type^Released - Road~User^USERDWAT~Details^Release",
                "Type^Moved - Set Down~User^USERDWAT~Details^From FK21 to 37172 Road  by MACH FK21",
                "Type^Moved - Picked Up~User^USERDWAT~Details^From MKGB01 001002 1 to FK21  by MACH FK21",
                "Type^Queued~User^USERDWAT~Details^From MKGB01 001002 1 to 37172 Road",
                "Type^Edited~User^USERDWAT~Details^deliveryRelease  => JLGRRROAD37172A01"
            });

        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {

            searchFor = "_37172_";
            
            // Create Cargo on Site 
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37172</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37172A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKGB01 001002</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Release Request
            CreateDataFileToLoad(@"CreateReleaseRequest.xml",
                "<?xml version='1.0'?>\n<JMTInternalReleaseRequest \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalRelaseRequest.xsd'>\n    <AllReleaserequest>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <Releaserequest Terminal='TT1'>\n            <allowCargoMultiReleases/>\n            <canBeHiLoCube/>\n            <checkISOGroup/>\n            <clean/>\n            <customsAssistance/>\n            <directDelivery/>\n            <heavyDuty/>\n            <isGeneratorAttachment/>\n            <mtRelease/>\n            <releaseByType>true</releaseByType>\n            <removeStops/>\n            <requestDateDoUpdate/>\n            <weight/>\n            <deliveryReason>Send to Shipper</deliveryReason>\n            <quantity>1</quantity>\n            <releaseType/>\n            <availabilityGrades/>\n            <billOfLading/>\n            <bookingConfirmation/>\n            <bookingRef/>\n            <businessUnit/>\n            <cargoSubType/>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <carrierCode/>\n            <consignee/>\n            <consignor/>\n            <consignorVoyage/>\n            <customsAgent/>\n            <deliveryReleaseNumber/>\n            <description/>\n            <destTerminal/>\n            <destinationPort/>\n            <dischargePort>NZNPE</dischargePort>\n            <freightForwarder/>\n            <fullOrMT>E</fullOrMT>\n            <generatorType/>\n            <id/>\n            <ids/>\n            <isoType>2200</isoType>\n            <isoTypeGroup/>\n            <loadPort>USJAX</loadPort>\n            <operatorCode>MSL</operatorCode>\n            <operatorGroup/>\n            <releaseRequestNumber>JLGRRROAD37172A01</releaseRequestNumber>\n            <releaseTypeStr>Road</releaseTypeStr>\n            <remarks/>\n            <senderCode/>\n            <sequence/>\n            <statusBulkRelease>Active</statusBulkRelease>\n            <subTerminalCode>Depot</subTerminalCode>\n            <trailerType/>\n            <vesselName/>\n            <voyageCode>MSCK000002</voyageCode>\n            <estimatedRequiredDate/>\n            <expiryTimeStamp/>\n            <requestDate>28/03/2018 15:11:00</requestDate>\n            <messageMode>A</messageMode>\n        </Releaserequest>\n    </AllReleaserequest>\n</JMTInternalReleaseRequest>");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
