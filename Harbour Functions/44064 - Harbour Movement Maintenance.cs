using DataObjects.LogInOutBO;
using FlaUI.Core.AutomationElements;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Harbour_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Harbour_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44064 : MTNBase
    {

        MovementMaintenanceForm movementMaintenanceForm;
        ConfirmationFormYesNo confirmationFormYesNo;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void MovementMaintenanceChangeToBerth()
        {
            MTNInitialize();

            //1. Go to Movement Maintenance Form and find voyage test case 44064
            FormObjectBase.NavigationMenuSelection(@"Harbour Functions|Movement Maintenance");
            movementMaintenanceForm = new MovementMaintenanceForm();
            //MTNControlBase.SetValue(movementMaintenanceForm.cmbVoyage, @"Test Case (44064)");
            movementMaintenanceForm.cmbVoyage.SetValue(TT1.Voyage.TestCase44064);
            //movementMaintenanceForm.btnEdit.DoClick();
            //movementMaintenanceForm.DoToolbarClick(movementMaintenanceForm.MainToolbar,
            //    (int)MovementMaintenanceForm.Toolbar.MainToolbar.Edit, "Edit");
            movementMaintenanceForm.DoEdit();

            //2. set a couple of values for later use
            string strToBerth = movementMaintenanceForm.cmbMovementToBerth.GetValue();
            string strMoveStatus = movementMaintenanceForm.txtMoveStatus.GetText();
            

            //3. ensure we are changing the To Berth value from something to something else.
            string strBerthToChangeTo = "To be Confirmed (TBC)";
            if (strToBerth.Contains(@"TBC"))
            {
                strBerthToChangeTo = @"TC44064 (B07)";
            }
            

            //4. Update the Berth
            //MTNControlBase.SetValue(movementMaintenanceForm.cmbMovementToBerth, strBerthToChangeTo);
            movementMaintenanceForm.cmbMovementToBerth.SetValue(strBerthToChangeTo);
            //movementMaintenanceForm.btnSave.DoClick();
            //movementMaintenanceForm.DoToolbarClick(movementMaintenanceForm.MainToolbar,
            //    (int)MovementMaintenanceForm.Toolbar.MainToolbar.Save, "Save");
            movementMaintenanceForm.DoSave();

            //5. if the voyage is in a planned status you get a confirmation message - include this in case.
            if (strMoveStatus == @"Planned")
            {
                confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm 'ACTUAL' Time");
                confirmationFormYesNo.btnNo.DoClick();
            }

            //movementMaintenanceForm.CloseForm();

        }

    }

}