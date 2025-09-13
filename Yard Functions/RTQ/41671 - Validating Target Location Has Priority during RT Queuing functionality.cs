using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Cargo_Storage_Areas;
using MTNUtilityClasses.Classes;
using System.Drawing;
using DataObjects;
using DataObjects.LogInOutBO;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.WindowsAPI;
using MTNForms.Controls.Lists;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Yard_Functions.Radio_Telemetry_Queue__RTQ_;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.RTQ
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41671 : MTNBase
    {
        private TerminalConfigForm _terminalConfigForm;
        private RadioTelemetryQueueForm _radioTelemetryQueueForm;
        private LocationEnquiryForm _locationEnquiryFormXXBS08;
        private LocationEnquiryForm _locationEnquiryFormXXBS09;

        private const string TestCaseNumber = @"41671";

        private string[] CargoId =
        {
            @"JLG" + TestCaseNumber + @"A01",
            @"JLG" + TestCaseNumber + @"A02"
        };

        private Point _startPoint;
        private Point _endPoint;
        private Point _clickPoint;
        private AutomationElement _form;


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        private void MTNInitialize()
        {
            DeleteForms = false;
            CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);

            TestRunDO.GetInstance().SetForceCloseAppToTrue();
            TestRunDO.GetInstance().SetUserName("USER41671");
            LogInto<MTNLogInOutBO>();

            SetTerminalConfigValue(@"Terminal Ops|Terminal Config", @"1");
        }

     
        [TestMethod]
        public void TargetLocationHasPriorityDuringRTQueuingFunctionality()
        {
            MTNInitialize();
            
            MTNContextMenu windowContextMenu = new MTNContextMenu(null, new [] { "Windows" });

            MTNKeyboard.TypeSimultaneously(VirtualKeyShort.ALT, VirtualKeyShort.KEY_W);
            windowContextMenu.MenuSelect(@"XXBS08 Location Enquiry TT1");


            // Step 6 - 12
            _locationEnquiryFormXXBS08 = new LocationEnquiryForm(@"XXBS08 Location Enquiry TT1");
            _locationEnquiryFormXXBS08.SetFocusToForm();
            _locationEnquiryFormXXBS08.DoToolbarClick(_locationEnquiryFormXXBS08.SearchToolbar,
                (int)LocationEnquiryForm.Toolbar.SearcherToolbar.Search, "Search");

            // Step 19 At the bottom right corner right-click on the Instant Mode and select Queued Mode
            _locationEnquiryFormXXBS08 = new LocationEnquiryForm(@"XXBS08 Location Enquiry TT1");
            Miscellaneous.ClickAtSpecifiedPoint(_locationEnquiryFormXXBS08.moveModeOptions, ClickType.RightClick);
            _locationEnquiryFormXXBS08.ContextMenuSelect(@"Queued Mode");
            // MTNControlBase.FindClickRowInTable(_locationEnquiryFormXXBS08.tblSearchResults, @"ID^" + CargoId[0]);
            _locationEnquiryFormXXBS08.TblSearchResults1.FindClickRow([$"ID^{CargoId[0]}"]);
            _startPoint = Mouse.Position;

            MTNKeyboard.TypeSimultaneously(VirtualKeyShort.ALT, VirtualKeyShort.KEY_W);
            windowContextMenu.MenuSelect(@"XXBS09 Location Enquiry TT1");

            _locationEnquiryFormXXBS09 = new LocationEnquiryForm(@"XXBS09 Location Enquiry TT1");
            _locationEnquiryFormXXBS09.SetFocusToForm();
            _locationEnquiryFormXXBS09.DoToolbarClick(_locationEnquiryFormXXBS09.SearchToolbar,
                (int)LocationEnquiryForm.Toolbar.SearcherToolbar.Search, "Search");

            // Step 19 At the bottom right corner right-click on the Instant Mode and select Queued Mode
            Miscellaneous.ClickAtSpecifiedPoint(_locationEnquiryFormXXBS09.moveModeOptions, ClickType.RightClick);
            _locationEnquiryFormXXBS09.ContextMenuSelect(@"Queued Mode");

            _form = _locationEnquiryFormXXBS09.GetForm();
            _clickPoint = new Point(_form.BoundingRectangle.X + (_form.BoundingRectangle.Width / 2),
                _form.BoundingRectangle.Y + (_form.BoundingRectangle.Height / 2));
            Mouse.Click(_clickPoint);
            _endPoint = Mouse.Position;

            Mouse.Drag(_startPoint, _endPoint);

            // Step 13 - 15
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Radio Telemetry Queues", true);
            _radioTelemetryQueueForm = new RadioTelemetryQueueForm(@"Radio Telemetry Queue TT1");
            
            _radioTelemetryQueueForm.lstQueues.FindItemInList($"XXBS09 (1 entries)|{CargoId[0]}- Internal {CargoId[0]}", SearchType.Contains);
            //_radioTelemetryQueueForm.lstQueues.FindItemInList(@"XXBS09 (1 entries)|" + CargoId[0] + @"- Internal " + CargoId[0]);
            _radioTelemetryQueueForm.CloseForm();

            // Step 16 - 17
            SetTerminalConfigValue("Terminal Ops|Terminal Config", @"0");

            // Step 18 - 20
            _locationEnquiryFormXXBS09.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_locationEnquiryFormXXBS09.tblSearchResults, @"ID^" + CargoId[1]);
            _locationEnquiryFormXXBS09.TblSearchResults1.FindClickRow(["ID^" + CargoId[1]]);
            _startPoint = Mouse.Position;

            _locationEnquiryFormXXBS08.SetFocusToForm();
            _form = _locationEnquiryFormXXBS08.GetForm();
            _clickPoint = new Point(_form.BoundingRectangle.X + (_form.BoundingRectangle.Width / 2),
                _form.BoundingRectangle.Y + (_form.BoundingRectangle.Height / 2));
            Mouse.Click(_clickPoint);
            _endPoint = Mouse.Position;

            Mouse.Drag(_startPoint, _endPoint);

            // Step 20
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Radio Telemetry Queues", true);
            _radioTelemetryQueueForm = new RadioTelemetryQueueForm(@"Radio Telemetry Queue TT1");
            _radioTelemetryQueueForm.lstQueues.FindItemInList($"XXBS08 (1 entries)|{CargoId[1]}- Internal {CargoId[1]}", SearchType.StartsWith);

            // Step 21 - 23 : This is handled by the resetData script as if they failed you couldn't guarantee that the
            //                would be unqueued for the next run and loading EDI fails if the are still queued 
        }

       

        private void SetTerminalConfigValue(string menuText, string valueToSet)
        {
            if (_terminalConfigForm == null)
            {
                FormObjectBase.NavigationMenuSelection(menuText, true);
                _terminalConfigForm = new TerminalConfigForm();
            }
            else
            {
                _terminalConfigForm.SetFocusToForm();
            }

            _terminalConfigForm.SetTerminalConfiguration(@"Settings",
                @"Target Location Has Priority During RT Queuing", valueToSet, rowDataType: EditRowDataType.CheckBox);
        }

    }

}
