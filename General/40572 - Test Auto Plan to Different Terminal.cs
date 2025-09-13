using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40572 : MTNBase
    {

        SelectorQueryForm selectorQueryForm;
        private string[,] _valuesToSetUnset;
        TestAutoPlanForm testAutoPlanForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}


        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            userName = "USER40572";
            LogInto<MTNLogInOutBO>(userName);
        }


        [TestMethod]
        public void TestAutoPlanToDifferentTerminal()
        {
            MTNInitialize();

            // Select the second terminal first, otherwise NavigationMenuSelection doesn't work
            // if Test Terminal 1 is already selected
            Keyboard.Press(VirtualKeyShort.DOWN);

            // 2. Navigate to Selector Query
            FormObjectBase.NavigationMenuSelection(@"Test Terminal 1 - Non Cash  (TT1)|General Functions|Selector");

            // 3. Select Mode as ISO Container
            selectorQueryForm = new SelectorQueryForm();
             //MTNControlBase.SetValue(selectorQueryForm.cmbMode, @"ISO Container", 1500);
            selectorQueryForm.cmbMode.SetValue(@"ISO Container");
            selectorQueryForm.QueryDetailsProperty();

            // 4. Select Site equal to Pre-Notified
            selectorQueryForm.tblQueryDetails.Click();
            Keyboard.Press(VirtualKeyShort.DOWN);
            selectorQueryForm.QueryDetailsArgumentCombo();
             //MTNControlBase.SetValue(selectorQueryForm.cmbArgument, @"Pre-Notified");
            selectorQueryForm.cmbArgument.SetValue(@"Pre-Notified");
            selectorQueryForm.BtnUpdateLine.DoClick();

            // 5. Add line for Id starts with JLG40572A
            selectorQueryForm.QueryDetailsProperty();
             //MTNControlBase.SetValue(selectorQueryForm.cmbProperty, @"Id");
            selectorQueryForm.cmbProperty.SetValue(@"Id");
            selectorQueryForm.QueryDetailsOperation();
            selectorQueryForm.QueryDetailsArgumentUpper();
             //MTNControlBase.SetValue(selectorQueryForm.cmbOperation, @"starts with");
            selectorQueryForm.cmbOperation.SetValue(@"starts with");
             //MTNControlBase.SetValue(selectorQueryForm.txtArgument, @"JLG40572A");
            selectorQueryForm.txtArgument.SetValue(@"JLG40572A");
            selectorQueryForm.btnAddLine.DoClick();

            // 6. Click Find button
            selectorQueryForm.btnFind.DoClick();

            // 7. Select the three items and right click and select Send To Screen...
            /*MTNControlBase.FindClickRowInTable(selectorQueryForm.tblQueryResults, @"Id^JLG40572A01", ClickType.Click, SearchType.Contains, 1, 16);
            MTNControlBase.FindClickRowInTable(selectorQueryForm.tblQueryResults, @"Id^JLG40572A02", ClickType.Click, SearchType.Contains, 1, 16);
            MTNControlBase.FindClickRowInTable(selectorQueryForm.tblQueryResults, @"Id^JLG40572A03", ClickType.Click, SearchType.Contains, 1, 16);

            Keyboard.TypeSimultaneously(VirtualKeyShort.CONTROL, VirtualKeyShort.UP);
            Keyboard.TypeSimultaneously(VirtualKeyShort.CONTROL, VirtualKeyShort.UP);
            Mouse.RightClick();
            selectorQueryForm.ContextMenuSelect(@"Send To Screen...");*/

            string[] detailsToSendToScreen =
            {
                "Id^JLG40572A01", 
                "Id^JLG40572A02", 
                "Id^JLG40572A03"
            };
            Keyboard.Pressing(VirtualKeyShort.CONTROL);
            selectorQueryForm.tblQueryResults1.FindClickRow(detailsToSendToScreen, ClickType.ContextClick);
            Keyboard.Release(VirtualKeyShort.CONTROL);
            selectorQueryForm.tblQueryResults1.contextMenu.MenuSelect(SelectorQueryForm.ContextMenuText.SendToScreen);

            // 8. Tick Cargo Enquiry and click Send button
            selectorQueryForm.ShowSendToScreen();
            _valuesToSetUnset = new string[,]
            {
                { @"Cargo Enquiry", @"1" }
            };

            MTNControlBase.SetUnsetValueCheckboxTable(selectorQueryForm.tblSendToScreenOptions, _valuesToSetUnset);
            selectorQueryForm.btnSend.DoClick();

            // 9. In Cargo Enquiry, select the three items, right click and select Test Auto Plan JLG40572A03...
            cargoEnquiryForm = new CargoEnquiryForm();
            //cargoEnquiryForm.btnSelectAll.DoClick();
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.MainToolbar, (int)CargoEnquiryForm.Toolbar.MainToolbar.SelectAll, "Select All");
            cargoEnquiryForm.tblSearchCriteria.RightClick();

            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Test AutoPlan JLG40572A01...");

            // 10. In Test AutoPlan form, select Terminal - Test Terminal 02 and click Test Auto Plan button
            testAutoPlanForm = new TestAutoPlanForm(@"Test Auto Plan TT1");
            //MTNControlBase.SetValue(testAutoPlanForm.cmbTestAutoplanTo, @"Test Terminal 02 - 37793 ONLY");
            testAutoPlanForm.cmbTestAutoplanTo.SetValue(@"Test Terminal 02 - 37793 ONLY");
            testAutoPlanForm.btnTestAutoPlan.DoClick();

            // check that the cargo items would plan as expected
            testAutoPlanForm.GetWouldPlanTable();
            // MTNControlBase.FindClickRowInTable(testAutoPlanForm.tblWouldPlanCargoItems, @"ID^JLG40572A01~Would Plan to^TAP01", ClickType.None);
            // MTNControlBase.FindClickRowInTable(testAutoPlanForm.tblWouldPlanCargoItems, @"ID^JLG40572A02~Would Plan to^TAP01", ClickType.None);
            // MTNControlBase.FindClickRowInTable(testAutoPlanForm.tblWouldPlanCargoItems, @"ID^JLG40572A03~Would Plan to^TAP01", ClickType.None);
            testAutoPlanForm.TblWouldPlanCargoItems.FindClickRow([
                "ID^JLG40572A01~Would Plan to^TAP01",
                "ID^JLG40572A02~Would Plan to^TAP01",
                "ID^JLG40572A03~Would Plan to^TAP01"
            ], ClickType.None);
          }
    }

}
