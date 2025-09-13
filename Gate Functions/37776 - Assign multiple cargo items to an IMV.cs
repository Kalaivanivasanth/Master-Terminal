using DataObjects.LogInOutBO;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Quick_Find;
using MTNForms.FormObjects.Yard_Functions;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects.Yard_Functions.Radio_Telemetry_Queue__RTQ_;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase37776 : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void AssignMuliptleCargoItemsToAnIMV()
        {
            MTNInitialize();
            
            // Step 4 - 7
            // 27/01/2025 navmh5 FormObjectBase.NavigationMenuSelection("General Functions|Cargo Enquiry");
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG37776A");
            cargoEnquiryForm.DoSearch();

            // 27/01/2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, "ID^JLG37776A01");
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG37776A01" });
            
            // Step 8 - 12
            /*// 27/01/2025 navmh5
            string[] cargoIds = 
            {
                @"JLG37776A01",
                @"JLG37776A02"
            };*/
            MoveItemUsingMoveIt(new[] { @"JLG37776A01", @"JLG37776A02" });
          
            // Step 13 - 15
            Keyboard.TypeSimultaneously(VirtualKeyShort.F10);
            QuickFindForm quickFindForm = new QuickFindForm();
            Keyboard.Type(@"F RTQ", 100);
            quickFindForm.btnFind.DoClick();

            RadioTelemetryQueueForm radioTelemetryQueueForm = new RadioTelemetryQueueForm();
            quickFindForm.CloseForm();

            // Step 16 - 18
            string cargoId = @"JLG37776A01";
            radioTelemetryQueueForm.FindCargoUsingFindCargoInTable(cargoId);

            // Step 19 - 20
            radioTelemetryQueueForm.FindClickRowInQueuedItemsTable(@"Master Queue " + cargoId + "^Unactioned^Pickup^MKBS07^IMV^MSCK000002");
            radioTelemetryQueueForm.ContextMenuSelect(@"Set Task Status|Set Complete");

            // Step 21 - 22
            RTQCompleteTaskForm rtqCompleteTaskForm = new RTQCompleteTaskForm(@"Complete Task " + radioTelemetryQueueForm.GetTerminalName());
            rtqCompleteTaskForm.GetItemsForFirstTimeIntoComplete();
            rtqCompleteTaskForm.cmbSelectTaskMachine.SetValue(@"FK14");
            rtqCompleteTaskForm.cmbSelectIMV.SetValue(@"IM01");
            rtqCompleteTaskForm.btnOK.DoClick();

            // Step 23 - 26
            cargoId = @"JLG37776A02";
            radioTelemetryQueueForm.FindCargoUsingFindCargoInTable(cargoId);

            // Step 27 - 28
            radioTelemetryQueueForm.FindClickRowInQueuedItemsTable(@"Master Queue " + cargoId + "^Unactioned^Pickup^MKBS07^IMV^MSCK000002");
            radioTelemetryQueueForm.ContextMenuSelect(@"Set Task Status|Set Complete");

            // Step 29 - 30
            rtqCompleteTaskForm = new RTQCompleteTaskForm(@"Complete Task " + radioTelemetryQueueForm.GetTerminalName());
            rtqCompleteTaskForm.GetItemsForFirstTimeIntoComplete();
            rtqCompleteTaskForm.cmbSelectTaskMachine.SetValue(@"FK14");
            rtqCompleteTaskForm.CmbSelectIMV.SetValue(@"IM01");
            rtqCompleteTaskForm.btnOK.DoClick();

            // Step 31
            string[] warningErrorsToCheck = 
            {
                @"Code :84248. IMV IM01 can not add container " + cargoId + " as it is already full."
            };
            /*// 27/01/2025 navmh5
            warningErrorForm = new WarningErrorForm(@"Errors for Complete RT Entry TT1");
            warningErrorForm.CheckWarningsErrorsExist(warningErrorsToCheck);
            warningErrorForm.btnCancel.DoClick();*/
            WarningErrorForm.CheckErrorMessagesExist("Errors for Complete RT Entry TT1", warningErrorsToCheck, doCancel: true);

            // Step 32 - 35
            cargoId = @"JLG37776A01";
            radioTelemetryQueueForm.FindCargoUsingFindCargoInTable(cargoId);

            // Step 36 - 37
            radioTelemetryQueueForm.FindClickRowInQueuedItemsTable(@"MKBS08 " + cargoId + "^Unactioned^Pickup^IM01^MKBS08^MSCK000002");
            radioTelemetryQueueForm.ContextMenuSelect(@"Set Task Status|Set Complete");

            // Step 38
            rtqCompleteTaskForm = new RTQCompleteTaskForm(@"Complete Task " + radioTelemetryQueueForm.GetTerminalName());
            rtqCompleteTaskForm.GetItemsForSubsequentTimeIntoComplete();
            rtqCompleteTaskForm.cmbSelectTaskMachine.SetValue(@"FK14");
            rtqCompleteTaskForm.btnOK.DoClick();
         
            warningErrorsToCheck = new []
           {
                $"Code :82959. Item {cargoId} does not match Allocation E CONT MSC 51908 at location MKBS08",
                $"Code :82959. Item {cargoId} does not match Allocation E CONT MSC 51908 at location MKBS08"
           };
            /*// 27/01/2025 navmh5
            warningErrorForm = new WarningErrorForm(@"Warnings for Complete RT Entry TT1");
            warningErrorForm.CheckWarningsErrorsExist(warningErrorsToCheck);
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Complete RT Entry TT1", warningErrorsToCheck);

            // Step 39 - 42
            cargoId = @"JLG37776A02";
            radioTelemetryQueueForm.SetToTopOfItemsTable();

            // Step 42 - 44
            radioTelemetryQueueForm.FindClickRowInQueuedItemsTable(@"Master Queue " + cargoId + "^Unactioned^Pickup^MKBS07^IMV^MSCK000002");
            radioTelemetryQueueForm.ContextMenuSelect(@"Set Task Status|Set Complete");

            // Step 45 - 46
            rtqCompleteTaskForm = new RTQCompleteTaskForm(@"Complete Task " + radioTelemetryQueueForm.GetTerminalName());
            rtqCompleteTaskForm.GetItemsForFirstTimeIntoComplete();
            rtqCompleteTaskForm.cmbSelectTaskMachine.SetValue(@"FK14");
             rtqCompleteTaskForm.cmbSelectIMV.SetValue(@"IM01");
             rtqCompleteTaskForm.btnOK.DoClick();
         

            // Step 47 - 50
            cargoId = @"JLG37776A02";
            radioTelemetryQueueForm.FindCargoUsingFindCargoInTable(cargoId);

            // Step 51 - 52
            radioTelemetryQueueForm.FindClickRowInQueuedItemsTable(@"MKBS08 " + cargoId + "^Unactioned^Pickup^IM01^MKBS08^MSCK000002");
            radioTelemetryQueueForm.ContextMenuSelect(@"Set Task Status|Set Complete");

            // Step 53
            rtqCompleteTaskForm = new RTQCompleteTaskForm(@"Complete Task " + radioTelemetryQueueForm.GetTerminalName());
            rtqCompleteTaskForm.GetItemsForSubsequentTimeIntoComplete();
            rtqCompleteTaskForm.cmbSelectTaskMachine.SetValue(@"FK14");
            rtqCompleteTaskForm.btnOK.DoClick();
            
            warningErrorsToCheck = new []
            {
                @"Code :82959. Item " + cargoId + " does not match Allocation E CONT MSC 51908 at location MKBS08",
                @"Code :82959. Item " + cargoId + " does not match Allocation E CONT MSC 51908 at location MKBS08"
            };
            /*
            // 27/01/2025 navmh5 
            warningErrorForm = new WarningErrorForm(@"Warnings for Complete RT Entry TT1");
            warningErrorForm.CheckWarningsErrorsExist(warningErrorsToCheck);
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Complete RT Entry TT1", warningErrorsToCheck);

            // Step 54 - 56 - Changed since Cargo Enquiry is currently open
            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.tblData2.FindClickRow(new[]
                { "Location ID^MKBS08^ID^JLG37776A01", "Location ID^MKBS08^ID^JLG37776A02" });
            
        }

       

        private void MoveItemUsingMoveIt(string[] cargoIds)
        {
            foreach (var cargoId in cargoIds)
            {
                // 27/01/2025 navmh5MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + cargoId, ClickType.ContextClick);
                cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{cargoId}" }, ClickType.ContextClick);
                cargoEnquiryForm.ContextMenuSelect(@"Cargo|Move It...");
              
                // fields to set
                string[] fieldsToSet = 
                {
                @"Move Mode^" + CargoMoveItForm.MoveModeString(MoveItMoveMode.Queued),
                @"To Terminal Area^MKBS08^20"
                };

                // warning message(s) to check
                string[] warningErrorToCheck = 
                {
                @"Code :82959. Item " + cargoId + " does not match Allocation E CONT MSC 51908 at location MKBS08"
                };

                //  27/01/2025 navmh5 cargoEnquiryForm.MoveCargoItemUsingMoveItForm(fieldsToSet, warningsToCheck:warningErrorToCheck);
                cargoEnquiryForm.MoveCargoItemUsingMoveItForm(fieldsToSet,
                    warningsToCheck: new[]
                    { $"Code :82959. Item {cargoId} does not match Allocation E CONT MSC 51908 at location MKBS08"
                    });
                cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{cargoId}~RT^Q~Queued Location^MKBS08" });
            }
        }



        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            //fileOrder = 1;
            searchFor = @"_37776_";

            // Cargo on Site Delete
            CreateDataFileToLoad(@"CargoOnSiteDelete.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37776</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37776A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS08</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37776</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37776A02</id>\n      <isoType>2200</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS08</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>APPL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");
            
            // Cargo On Site Add
            CreateDataFileToLoad(@"CargoOnSiteAdd.xml",
               "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37776</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37776A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS07</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37776</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37776A02</id>\n      <isoType>2200</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS07</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>APPL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }


    }

}
