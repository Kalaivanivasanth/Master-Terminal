using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNDesktopFlaUI.Classes;
using MTNForms.Controls;
using MTNForms.FormObjects.Logs;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using System.Drawing;
using DataObjects.LogInOutBO;
using FlaUI.Core.Definitions;
using MTNForms.Controls.Button;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Logs
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40739 : MTNBase
    {

        private LogVoyageOperationsForm _logVoyageOperationsForm = null;

        private const string TestCaseNumber = @"40739";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)  => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>("ALLPORTS");
       

        [TestMethod]
        public void CreateAndEditALogVoyage()
        {
            MTNInitialize();
     
            Keyboard.Press(VirtualKeyShort.ENTER);
            /*FormObjectBase.NavigationMenuSelection(
                @"Test Terminal 1 - Non Cash  (TT1)|Log Functions|Log Voyage Functions|Log Voyage Operations");*/
            FormObjectBase.MainForm.OpenLogVoyageOperationsFromToolbar();
            _logVoyageOperationsForm = new LogVoyageOperationsForm();

            _logVoyageOperationsForm.GetPortsTabDetails();
            _logVoyageOperationsForm.GetVoyageAdminDetails();

            var voyageFound = MTNControlBase.FindClickRowInList(_logVoyageOperationsForm.lstPorts, @"VOY40739  MSCK",
                doAssert: false);
            
            if (voyageFound)
            {
                _logVoyageOperationsForm.btnVoyageAdminDelete.DoClick();

                warningErrorForm = new WarningErrorForm(@"Warnings for Voyage Ops " + terminalId);
                warningErrorForm.btnSave.DoClick();
            }

                // Step 3
            _logVoyageOperationsForm.btnVoyageAdminNew.DoClick();

            // Step 4
            MTNControlBase.SetValueInEditTable(_logVoyageOperationsForm.tblVoyageAdminVoyageDetails, @"Voyage Code", @"VOY40739");
            MTNControlBase.SetValueInEditTable(_logVoyageOperationsForm.tblVoyageAdminVoyageDetails, @"Vessel Name", @"MSCK");
            MTNControlBase.SetValueInEditTable(_logVoyageOperationsForm.tblVoyageAdminVoyageDetails, @"Hatches", @"2");

            // Step 5                                                                                           
            string[] terminalsToCheckFor =
            {
                @"Cash Enabled Terminal",
                @"Pelindo Terminal",
                @"Test Terminal 1 - Non Cash"
            };
            _logVoyageOperationsForm.unassignedAssigned.MakeSureInCorrectList(
                _logVoyageOperationsForm.unassignedAssigned.LstLeft, terminalsToCheckFor);

            // Step 6
            _logVoyageOperationsForm.btnVoyageAdminSave.DoClick();
            _logVoyageOperationsForm.GetPortsTabDetails();

            string[] detailsToCheck =  
            {
                //@"Global Voyages Ports",
                @"VOY40739  MSCK",
                @"Test Terminal 1 - Non Cash",
                @"Pelindo Terminal",
                @"Cash Enabled Terminal"
            };
            _logVoyageOperationsForm.CheckGlobalVoyagesPorts(detailsToCheck);
            _logVoyageOperationsForm.CloseForm();

            // Step 7
            /*// Monday, 7 April 2025 navmh5 Can be removed 6 months after specified date 
            var btnLogOff = new MTNButton(@"4021", MTNDesktop.Instance.mainWindow, @"Log Off", ControlType.Pane);
            btnLogOff.DoClick();*/
            FormObjectBase.MainForm.LogOffFromToolbar();
            //LogOut<MTNLogInOutBO>();

            // Step 8
           LogInto<MTNLogInOutBO>("ALLPORTS");

            // Step 9
            Keyboard.Press(VirtualKeyShort.ENTER);
            //FormObjectBase.NavigationMenuSelection(@"Test Terminal 1 - Non Cash  (TT1)|Log Functions|Log Voyage Functions|Log Voyage Operations");
            FormObjectBase.MainForm.OpenLogVoyageOperationsFromToolbar();
            _logVoyageOperationsForm = new LogVoyageOperationsForm();
            _logVoyageOperationsForm.GetVoyageAdminDetails();
            _logVoyageOperationsForm.GetPortsTabDetails();

            // Step 10
            MTNControlBase.FindClickRowInList(_logVoyageOperationsForm.lstPorts, @"VOY40739  MSCK");
            var point = new Point(_logVoyageOperationsForm.lstPorts.BoundingRectangle.X + 25,
                _logVoyageOperationsForm.lstPorts.BoundingRectangle.Y + 25);
            Mouse.Click(point);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            detailsToCheck = new string[]
            {
                @"Global Voyages Ports",
                @"VOY40739  MSCK",
                @"Test Terminal 1 - Non Cash",
            };
            _logVoyageOperationsForm.CheckGlobalVoyagesPorts(detailsToCheck);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            
            _logVoyageOperationsForm.GetVoyageAdminTab();
            _logVoyageOperationsForm.GetVoyageTabDetails();

            // Step 11
           _logVoyageOperationsForm.btnVoyageEdit.DoClick();

            // Step 12
            var date = DateTime.Today;
            _logVoyageOperationsForm.tblVoyageTerminalPortDetails.Focus();
            point = new Point(_logVoyageOperationsForm.tblVoyageTerminalPortDetails.BoundingRectangle.X + 150,
                _logVoyageOperationsForm.tblVoyageTerminalPortDetails.BoundingRectangle.Y + 20);
            Mouse.Click(point);
            Keyboard.Press(VirtualKeyShort.TAB);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(100));
            Keyboard.Press(VirtualKeyShort.TAB);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(100));
            Keyboard.Press(VirtualKeyShort.TAB);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(100));
            Keyboard.Type(date.ToString(@"ddMMyyyy"));
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(100));

            // Step 13
            _logVoyageOperationsForm.btnVoyageSave.DoClick();

        }

   
    }

}
