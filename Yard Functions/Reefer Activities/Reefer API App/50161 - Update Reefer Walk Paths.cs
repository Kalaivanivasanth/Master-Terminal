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
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Reefer_Activities_Reefer_API_App
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase50161 : MTNBase
    {
        
        private ReeferMonitoringForm _reeferMonitoringForm;
        private ReeferWalkPathDefinitionForm _reeferWalkPathDefinitionForm;
       
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"resetData_50161");

            // Thursday, 30 January 2025 navmh5 MTNSignon(TestContext, userName);
            userName = "REEFTT3";
            LogInto<MTNLogInOutBO>(userName);
        }


        [TestMethod]
        public void UpdateReeferWalkPaths()
        {
            MTNInitialize();
            
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Reefer Activities|Reefer Monitoring");
            _reeferMonitoringForm = new ReeferMonitoringForm();

            _reeferMonitoringForm.DoWalkPathDefinition();

            _reeferWalkPathDefinitionForm = new ReeferWalkPathDefinitionForm();
            _reeferWalkPathDefinitionForm.cmbGriddedArea.SetValue(@"RFPMGT");

            _reeferWalkPathDefinitionForm.cmbGriddedArea.SetValue(@"RFPMGT");
            string[] detailsToMove =
            {
                 "RFPMGT 1005 1",
                 "RFPMGT 1005 2",
                 "RFPMGT 1005 3",
                 "RFPMGT 1205 1",
            };
            AddReeferWalkPath(@"50161ROW1", detailsToMove);
            // MTNControlBase.FindClickRowInTable(_reeferWalkPathDefinitionForm.tblBlockName,
                // @"Reefer Walk - Block Name^50161ROW1", rowHeight: 16);
            _reeferWalkPathDefinitionForm.TblBlockName.FindClickRow(["Reefer Walk - Block Name^50161ROW1"]);
            
            detailsToMove = new string[]
            {
                "RFPMGT 1405 2",
                "RFPMGT 1405 3",
                "RFPMGT 1505 1",
               // "RFPMGT 1505 1",
            };
            _reeferWalkPathDefinitionForm.positionsLists.MoveItemsBetweenList(
                _reeferWalkPathDefinitionForm.positionsLists.LstLeft, detailsToMove);
            _reeferWalkPathDefinitionForm.btnUpdate.DoClick();
            // MTNControlBase.FindClickRowInTable(_reeferWalkPathDefinitionForm.tblBlockName, @"Reefer Walk - Block Name^50161ROW1",
                // rowHeight: 16);
            _reeferWalkPathDefinitionForm.TblBlockName.FindClickRow(["Reefer Walk - Block Name^50161ROW1"]);


            detailsToMove = new string[]
            {
                "RFPMGT 1205 1",

            };

            _reeferWalkPathDefinitionForm.positionsLists.MoveItemsBetweenList(
                _reeferWalkPathDefinitionForm.positionsLists.LstRight, detailsToMove, fromLeftList: false);
            _reeferWalkPathDefinitionForm.btnUpdate.DoClick();
            // MTNControlBase.FindClickRowInTable(_reeferWalkPathDefinitionForm.tblBlockName, @"Reefer Walk - Block Name^50161ROW1",
                // rowHeight: 16);
            _reeferWalkPathDefinitionForm.TblBlockName.FindClickRow(["Reefer Walk - Block Name^50161ROW1"]);


            detailsToMove = new string[]
            {
                @"RFPMGT 1005 2",
                @"RFPMGT 1005 3",
                @"RFPMGT 1005 1",
                @"RFPMGT 1405 2",
                @"RFPMGT 1405 3",
                @"RFPMGT 1505 1",
               // @"RFPMGT 1505 1",
            };

            _reeferWalkPathDefinitionForm.positionsLists.ValidateInCorrectList(_reeferWalkPathDefinitionForm.positionsLists.LstRight,
                detailsToMove);
            // MTNControlBase.FindClickRowInTable(_reeferWalkPathDefinitionForm.tblBlockName, @"Reefer Walk - Block Name^50161ROW1",
                // rowHeight: 16);
            _reeferWalkPathDefinitionForm.TblBlockName.FindClickRow(["Reefer Walk - Block Name^50161ROW1"]);

            _reeferWalkPathDefinitionForm.txtBlockName.SetValue(@"50161 ROW1");
            _reeferWalkPathDefinitionForm.btnUpdate.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Errors for Reefer Walk Path TT3");
            string[] warningErrorToCheck = new string[]
            {
                "Code :91530. Name not valid. '" + _reeferWalkPathDefinitionForm.txtBlockName.GetText() +
                "' should only include alphanumeric characters"
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnCancel.DoClick();
          
            _reeferWalkPathDefinitionForm.txtBlockName.SetValue(@"50161-ROW1");
            _reeferWalkPathDefinitionForm.btnUpdate.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Errors for Reefer Walk Path TT3");
            warningErrorToCheck = new string[]
            {
                "Code :91530. Name not valid. '" + _reeferWalkPathDefinitionForm.txtBlockName.GetText() +
                                              "' should only include alphanumeric characters"
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnCancel.DoClick();
          
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
                string formName = @"Warnings for Reefer Walk Path TT3";

                Retry.WhileException(() =>
                {
                    AutomationElement form = MTNDesktop.Instance.desktop
                        .FindFirstDescendant(cf => cf.ByName(formName).And(cf.ByControlType(ControlType.Pane)))
                        .AsWindow();

                }, TimeSpan.FromSeconds(10), null, true);

                warningErrorForm = new WarningErrorForm(formName);
                string[] warningErrorToCheck = new string[]
                {
                    "Code :91531. Name '" + walkPathName + "' is too big. Entering more than 4 characters may cause the Walk Path " +
                    @"to not display correctly in the Reefer Monitor application"
                };
                warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
                 warningErrorForm.btnSave.DoClick();
            }
            // MTNControlBase.FindClickRowInTable(_reeferWalkPathDefinitionForm.tblBlockName, @"Reefer Walk - Block Name^" + walkPathName,
                // rowHeight: 16);
            _reeferWalkPathDefinitionForm.TblBlockName.FindClickRow(["Reefer Walk - Block Name^" + walkPathName]);
            _reeferWalkPathDefinitionForm.positionsLists.ValidateInCorrectList(
                _reeferWalkPathDefinitionForm.positionsLists.LstRight, detailsToMove);
        }

       
    }

}
