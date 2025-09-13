using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Reefer_Activities_Reefer_API_App
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase51109 : MTNBase
    {
        
       
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() { base.TestCleanup(); }

        void MTNInitialize()
        {
            searchFor = @"_51109_";
            userName = "REEFTT3";
            LogInto<MTNLogInOutBO>(userName);
        }

        [TestMethod]
        public void CheckCantDeleteRowsColumnsWithWalkPaths()
        {
            
            MTNInitialize();
            
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Area Definition");
            TerminalAreaDefinitionForm terminalAreaDefinitionForm =
                new TerminalAreaDefinitionForm(@"Terminal Area Definition TT3");
           // terminalAreaDefinitionForm.GetTerminalAreasTable();
            // MTNControlBase.FindClickRowInTable(terminalAreaDefinitionForm.tblTerminalAreas,
                // @"ID^RF1109~Type^Gridded Block Stack", ClickType.Click, rowHeight: 16);
            terminalAreaDefinitionForm.TblTerminalAreas.FindClickRow(["ID^RF1109~Type^Gridded Block Stack"], ClickType.Click);
            //terminalAreaDefinitionForm.btnEdit.DoClick();
            terminalAreaDefinitionForm.DoEdit();
           
            terminalAreaDefinitionForm.GetDefinitionData();
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, @"Length (cols)", @"3");
            //terminalAreaDefinitionForm.btnSave.DoClick();
            terminalAreaDefinitionForm.DoSave();

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"Terminal Area Definition TT3");
            confirmationFormYesNo.btnYes.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Errors for Terminal Area Definition TT3");
            string[] warningErrorToCheck = 
            {
                "Code :91527. Column '4' may not be removed as it is still declared in Reefer Walk Path 'WLK2'.",
                "Code :91527. Column '5' may not be removed as it is still declared in Reefer Walk Path 'WLK1'.",
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnCancel.DoClick();

            terminalAreaDefinitionForm.SetFocusToForm();
            //terminalAreaDefinitionForm.btnCancel.DoClick();
            terminalAreaDefinitionForm.DoCancel();

            confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Cancel");
            confirmationFormYesNo.btnYes.DoClick();

            //terminalAreaDefinitionForm.btnEdit.DoClick();
            terminalAreaDefinitionForm.DoEdit();

            terminalAreaDefinitionForm.GetDefinitionData();
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, @"Width (rows)", @"4");
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, @"Length (cols)", @"4");
            //terminalAreaDefinitionForm.btnSave.DoClick();
            terminalAreaDefinitionForm.DoSave();

            confirmationFormYesNo = new ConfirmationFormYesNo(@"Terminal Area Definition TT3");
            confirmationFormYesNo.btnYes.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Errors for Terminal Area Definition TT3");
            warningErrorToCheck = new string[]
            {
                "Code :91527. Column '05' may not be removed as it is still declared in Reefer Walk Path 'WLK2'.",
                "Code :91527. Column '5' may not be removed as it is still declared in Reefer Walk Path 'WLK1'.",
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnCancel.DoClick();

            terminalAreaDefinitionForm.SetFocusToForm();
            //terminalAreaDefinitionForm.btnCancel.DoClick();
            terminalAreaDefinitionForm.DoCancel();

            confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Cancel");
            confirmationFormYesNo.btnYes.DoClick();
        }
       
    }

}
