using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40826 : MTNBase
    {

        SelectorQueryForm _selectorQueryForm;
        ColumnOrganiserForm _columnOrganiserForm;
        
        const string ConsigneeConsignorGroup = "Consignee / Consignor Group";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void AddFieldToSelectorQuery()
        {
            MTNInitialize();

            // 1. Navigate to Voyage Selector Query
            // Tuesday, 28 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"General Functions|Selector");
            FormObjectBase.MainForm.OpenSelectorFromToolbar();

            // 2. Do a query in Tracked Item Mode and add consignee / consignor group field from column organiser
            _selectorQueryForm = new SelectorQueryForm();
            
            _selectorQueryForm.ChkSuppressConfirmationMessage.DoClick();
            
            _selectorQueryForm.cmbMode.SetValue("Tracked Item mode");
            _selectorQueryForm.CmbProperty.SetValue(ConsigneeConsignorGroup);
            _selectorQueryForm.CmbOperation.SetValue("equal to");
            _selectorQueryForm.CmbArgument.SetValue("CG01	 (40286_Test)", doDownArrow: true);
            _selectorQueryForm.btnAddLine.DoClick();
            _selectorQueryForm.btnFind.DoClick();
            _selectorQueryForm.tblQueryResults1.GetElement().RightClick();

            _selectorQueryForm.ContextMenuSelect("Organise Columns...");

            _columnOrganiserForm = new ColumnOrganiserForm(@"Column Organiser Selector - Tracked Item mode TT1", "4009");
            
            _columnOrganiserForm.lstSelect.MakeSureInCorrectList(_columnOrganiserForm.lstSelect.LstLeft,
                new[] { ConsigneeConsignorGroup });
            _columnOrganiserForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_selectorQueryForm.tblQueryResults, $"{ConsigneeConsignorGroup}^CG01",
                // ClickType.Click, SearchType.Contains, 1, 16);
            _selectorQueryForm.tblQueryResults1.FindClickRow([$"{ConsigneeConsignorGroup}^CG01"]);

            //3. Do a query in Cargo Transaction Mode and add consignee / consignor group field from column organiser
            _selectorQueryForm.cmbMode.SetValue("Cargo Transaction");
            _selectorQueryForm.CmbProperty.SetValue($"Cargo {ConsigneeConsignorGroup}");
            _selectorQueryForm.CmbOperation.SetValue("equal to");
            _selectorQueryForm.CmbArgument.SetValue("CG01	 (40286_Test)", doDownArrow: true);
            _selectorQueryForm.btnAddLine.DoClick();


            _selectorQueryForm.tblQueryDetails.Click();
            Keyboard.TypeSimultaneously(VirtualKeyShort.ALT, VirtualKeyShort.HOME);

            _selectorQueryForm.QueryDetailsArgumentCombo();
            _selectorQueryForm.cmbArgument.SetValue(@"Received - Road", additionalWaitTimeout: 1000, doDownArrow: true, searchSubStringTo: 10);
            _selectorQueryForm.btnUpdateLine.DoClick();
            _selectorQueryForm.CmbProperty.SetValue(@"Transaction Date");

            _selectorQueryForm.CmbOperation.SetValue(@"greater than");
            _selectorQueryForm.QueryDetailsDate();
            _selectorQueryForm.txtArgumentDate.SetValue(@"19102018");
            _selectorQueryForm.btnUpdateLine.DoClick();
            _selectorQueryForm.btnFind.DoClick();
            _selectorQueryForm.tblQueryResults1.GetElement().RightClick();

            _selectorQueryForm.ContextMenuSelect("Organise Columns...");
            _columnOrganiserForm = new ColumnOrganiserForm(@"Column Organiser Selector - Cargo Transaction mode TT1", "4009");
            
            _columnOrganiserForm.lstSelect.MakeSureInCorrectList(_columnOrganiserForm.lstSelect.LstLeft,
                new[] { $"Cargo {ConsigneeConsignorGroup}" });
            _columnOrganiserForm.btnOK.DoClick();
           // MTNControlBase.FindClickRowInTable(_selectorQueryForm.tblQueryResults,
           //    / $"Cargo {ConsigneeConsignorGroup}^CG01", ClickType.Click, SearchType.Contains, 1, 16);
            _selectorQueryForm.tblQueryResults1.FindClickRow([$"Cargo {ConsigneeConsignorGroup}^CG01"]);


            //4. Do a query in ISO Container Mode and add consignee / consignor group field from column organiser
            _selectorQueryForm.cmbMode.SetValue(@"ISO Container");
            _selectorQueryForm.CmbProperty.SetValue(ConsigneeConsignorGroup);
            _selectorQueryForm.CmbOperation.SetValue(@"equal to");
            _selectorQueryForm.CmbArgument.SetValue(@"CG01	 (40286_Test)", doDownArrow: true);
            _selectorQueryForm.btnAddLine.DoClick();
            _selectorQueryForm.btnFind.DoClick();
            _selectorQueryForm.tblQueryResults1.GetElement().RightClick();
            _selectorQueryForm.ContextMenuSelect("Organise Columns...");
            _columnOrganiserForm = new ColumnOrganiserForm(@"Column Organiser Selector - ISO Container mode TT1", "4009");

            _columnOrganiserForm.lstSelect.MoveItemsBetweenList(_columnOrganiserForm.lstSelect.LstLeft,
                new[] { ConsigneeConsignorGroup }, true);
            _columnOrganiserForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_selectorQueryForm.tblQueryResults, $"{ConsigneeConsignorGroup}^CG01",
                // ClickType.Click, SearchType.Contains, 1, 16);
            _selectorQueryForm.tblQueryResults1.FindClickRow([$"{ConsigneeConsignorGroup}^CG01"]);

    

        }



    }

}
