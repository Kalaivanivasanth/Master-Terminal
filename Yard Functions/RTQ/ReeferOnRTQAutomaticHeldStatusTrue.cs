using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Quick_Find;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Yard_Functions.Radio_Telemetry_Queue__RTQ_;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.RTQ
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase37058 : MTNBase
    {
        private TerminalConfigForm _terminalConfigForm;
        private RadioTelemetryQueueForm _radioTelemetryQueueForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            SetupInitializeDataLoad(TestContext);

            LogInto<MTNLogInOutBO>();

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            _terminalConfigForm = new TerminalConfigForm();

            _terminalConfigForm.SetTerminalConfiguration("Settings",
                @"Set connected containers RT Task automatically to Held", @"1", rowDataType: EditRowDataType.CheckBox);
            _terminalConfigForm.CloseForm();
        }


        [TestMethod]
        public void AutomaticHeldStatusTrue()
        {
            
            MTNInitialize();

            // Step 7 - 15
            ConnectToPowerMoveCargo(@"JLG37058A01");

            // Step 16 - 17
            Keyboard.TypeSimultaneously(VirtualKeyShort.F10);
            QuickFindForm quickFindForm = new QuickFindForm();
            Keyboard.Type(@"F RTQ", 100);
            quickFindForm.btnFind.DoClick();
            quickFindForm.CloseForm();

            // Step 18
            _radioTelemetryQueueForm = new RadioTelemetryQueueForm();
            _radioTelemetryQueueForm.FindClickRowInQueuedItemsTable(
                @"MKRTQ01 JLG37058A01^System Held^Pickup^MKBS01^MKBS02^MSCK000002", false);
            _radioTelemetryQueueForm.CloseForm();

            // Step 21 - 24
            cargoEnquiryForm = new CargoEnquiryForm();
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date     @"Location ID^MKBS01~ID^JLG37058A01~Cargo Type^ISO Container~Opr^FLS~Type^2230",
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date     ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new[]
                { "Location ID^MKBS01~ID^JLG37058A01~Cargo Type^ISO Container~Opr^FLS~Type^2230" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Unqueue JLG37058A01");

            /*// Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date 
            warningErrorForm = new WarningErrorForm(@"Warnings for Unqueue TT1");
            string[] warningErrorToCheck =
            {
                @"Code :79885. Job JLG37058A01 is currently Held in RT Queue MKRTQ01. This action may cause side effects that you may need to correct yourself. Do you want to continue?"
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm($"Warnings for Unqueue {terminalId}",
                new[]
                { "Code :79885. Job JLG37058A01 is currently Held in RT Queue MKRTQ01. This action may cause side effects that you may need to correct yourself. Do you want to continue?" });
        }


        private void ConnectToPowerMoveCargo(string cargoID)
        {
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            string[] searchForDetails =
            {
                @"Cargo ID^" + cargoID + "^^^"
            };
            cargoEnquiryForm.SearchForCargoItems(searchForDetails,
                $"Location ID^MKBS01~ID^{cargoID}~Cargo Type^ISO Container~Opr^FLS~Type^2230");
            cargoEnquiryForm.tblData2.GetElement().RightClick();
            cargoEnquiryForm.ContextMenuSelect($"Cargo|{cargoID} Connect To Power");

            ConnectDisconnectToPowerForm connectDisconnectToPower =
                new ConnectDisconnectToPowerForm($"{cargoID} Connect To Power");
            connectDisconnectToPower.btnOK.DoClick();

            cargoEnquiryForm.tblData2.GetElement().RightClick();
            cargoEnquiryForm.ContextMenuSelect("Cargo|Move It...");

            var fieldsToSet = new[]
            {
                @"Move Mode^" + CargoMoveItForm.MoveModeString(MoveItMoveMode.Queued),
                @"To Terminal Area^MKBS02"
            };

            // warning message(s) to check
            var warningErrorToCheck = new[]
            {
                @"Code :73022. Warning: Container " + cargoID + " is still connected to power.",
                //@"Code :82959. Item " + cargoID + " does not match Allocation E 44072 at location MKBS02"
            };
            cargoEnquiryForm.MoveCargoItemUsingMoveItForm(fieldsToSet, warningsToCheck: warningErrorToCheck);
        }

        private static void SetupInitializeDataLoad(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = "_37058_";

            //Delete Cargo Onsite
            CreateDataFileToLoad(@"DeleteCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG37058A01</id>\n         <operatorCode>FLS</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>2000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>REEF</commodity>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000002</voyageCode>\n         <isoType>2230</isoType>\n         <temperature>-5</temperature>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //Create Cargo Onsite
            CreateDataFileToLoad(@"CreateCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG37058A01</id>\n         <operatorCode>FLS</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>2000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>REEF</commodity>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000002</voyageCode>\n         <isoType>2230</isoType>\n         <temperature>-5</temperature>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Load the files
            CallJadeToLoadFiles(testContext);

        }




    }

}
