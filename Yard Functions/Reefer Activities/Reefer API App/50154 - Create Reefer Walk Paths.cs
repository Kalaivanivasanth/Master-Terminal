using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNDesktopFlaUI.Classes;
using MTNForms.Controls;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions.Reefer;
using MTNUtilityClasses.Classes;
using System;
using System.Diagnostics;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Reefer_Activities_Reefer_API_App
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase50154 : MTNBase
    {
        
        private ReeferMonitoringForm _reeferMonitoringForm;
        private ReeferWalkPathDefinitionForm _reeferWalkPathDefinitionForm;
        private AutomationElement _form;

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
            // Thursday, 30 January 2025 navmh5 MTNSignon(TestContext, userName);
            userName = "REEFTT3";
            terminalId = "TT3";
            LogInto<MTNLogInOutBO>(userName);
            
            CallJadeScriptToRun(TestContext, @"resetData_50154");
        }


        [TestMethod]
        public void AddReeferWalkPaths()
        {
            
            MTNInitialize();

            // Friday, 28 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Yard Functions|Reefer Activities|Reefer Monitoring");
            FormObjectBase.MainForm.OpenReeferMonitoringFromToolbar();
            _reeferMonitoringForm = new ReeferMonitoringForm();

            _reeferMonitoringForm.DoWalkPathDefinition();

            _reeferWalkPathDefinitionForm = new ReeferWalkPathDefinitionForm();
            _reeferWalkPathDefinitionForm.cmbGriddedArea.SetValue("RFPMGT");

            AddReeferWalkPath(@"50154ROW3",
                new[]
                {
                    "RFPMGT 0601 2", "RFPMGT 0601 3", "RFPMGT 0801 1", "RFPMGT 0801 2", "RFPMGT 0801 3",
                    "RFPMGT 1001 1", "RFPMGT 1001 2", "RFPMGT 1001 3", "RFPMGT 1201 1", "RFPMGT 1201 2",
                    "RFPMGT 1201 3",
                });

            // Non alphanumeric name
           AddReeferWalkPathInvalidCharacter(@"50154_ROW4", new [] { "RFPMGT 0101 1" });

            // Length = 4
            AddReeferWalkPath("5015", new [] { "RFPMGT 0101 1" });

            // Length under 4
            AddReeferWalkPath("501",  new [] { "RFPMGT 0101 1" });

        }

        
        private void AddReeferWalkPath(string walkPathName, string[] detailsToMove)
        {
           _reeferWalkPathDefinitionForm.btnAdd.DoClick();
            _reeferWalkPathDefinitionForm.txtBlockName.SetValue(walkPathName);


            _reeferWalkPathDefinitionForm.positionsLists.MoveItemsBetweenList(
                _reeferWalkPathDefinitionForm.positionsLists.LstLeft, detailsToMove);
            _reeferWalkPathDefinitionForm.btnUpdate.DoClick();

            if (walkPathName.Length > 4)
            {
                /*// Friday, 28 February 2025 navmh5 Can be removed 6 months after specified date 
                warningErrorForm = new WarningErrorForm(formTitle: $"Warnings for Reefer Walk Path {terminalId}");
                string[] warningErrorToCheck = new []
                {
                    "Code :91531. Name '" + walkPathName + "' is too big. Entering more than 4 characters may cause the Walk Path " +
                    @"to not display correctly in the Reefer Monitor application"
                };
                warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
                warningErrorForm.btnSave.DoClick();*/
                WarningErrorForm.CompleteWarningErrorForm($"Warnings for Reefer Walk Path {terminalId}", new []
                {
                    $"Code :91531. Name '{walkPathName}' is too big. Entering more than 4 characters may cause the Walk Path to not display correctly in the Reefer Monitor application"
                });
            }

            // Friday, 28 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_reeferWalkPathDefinitionForm.tblBlockName, @"Reefer Walk - Block Name^" + walkPathName,
             // Friday, 28 February 2025 navmh5 Can be removed 6 months after specified date    rowHeight: 16);
            _reeferWalkPathDefinitionForm.TblBlockName.FindClickRow(new[] { $"Reefer Walk - Block Name^{walkPathName}" });
            _reeferWalkPathDefinitionForm.positionsLists.ValidateInCorrectList(
                _reeferWalkPathDefinitionForm.positionsLists.LstRight, detailsToMove);
        }

        private void AddReeferWalkPathInvalidCharacter(string walkPathName, string[] detailsToMove)
        {
           _reeferWalkPathDefinitionForm.btnAdd.DoClick();
            _reeferWalkPathDefinitionForm.txtBlockName.SetValue(walkPathName);

            _reeferWalkPathDefinitionForm.positionsLists.MoveItemsBetweenList(
                _reeferWalkPathDefinitionForm.positionsLists.LstLeft, detailsToMove);
            _reeferWalkPathDefinitionForm.btnUpdate.DoClick();

            // Need to handle non alpha numeric characters first
            var formName = $"Errors for Reefer Walk Path {terminalId}";

            Retry.WhileException(() =>
            {
                _form = MTNDesktop.Instance.desktop
                    .FindFirstDescendant(cf => cf.ByName(formName).And(cf.ByControlType(ControlType.Pane)))
                    .AsWindow();

            }, TimeSpan.FromSeconds(10), null, true);

            Trace.TraceInformation(@"formName: {0}    _form.Name: {1}", formName, _form?.Name);
            // Thursday, 30 January 2025 navmh5 
            /*warningErrorForm = new WarningErrorForm(formName);
            string[] warningErrorToCheck = new string[]
            {
                $"Code :91530. Name not valid. '{walkPathName}' should only include alphanumeric characters"
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnCancel.DoClick();*/
            WarningErrorForm.CheckErrorMessagesExist(formName,
                new[] { $"Code :91530. Name not valid. '{walkPathName}' should only include alphanumeric characters" });
    
        }


    }

}
