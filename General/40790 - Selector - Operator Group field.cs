using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects.LogInOutBO;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40790 : MTNBase
    {
        SelectorQueryForm _selectorQueryForm;

        string[] _detailsToCheck;

        readonly string[] _modes =
        {
            "Break-Bulk Cargo",
            "Cargo Transaction",
            "ISO Container",
            "Tracked Item mode"
        };
       
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void OperatorGroupField()
        {
            MTNInitialize();

            FormObjectBase.MainForm.OpenSelectorFromToolbar();

            _selectorQueryForm = new SelectorQueryForm();
            _selectorQueryForm.ChkSuppressConfirmationMessage.DoClick();

            // Mode - Break-Bulk
            SetModeAndOperatorGroupCodeLine(_modes[0]);
            _selectorQueryForm.btnFind.DoClick();

            DoColumnOrganiserAndCheckCorrectTableDetails(_modes[0],
                new[] { "Id^JLG40790A03~Operator^OP01~Cargo Type^Break-Bulk Cargo~Operator Group Code^OG01,OG02" });

            // Mode - Cargo Transaction
            SetModeAndOperatorGroupCodeLine(_modes[1]);

            _selectorQueryForm.SelectRowInQueryDetailsTable(@"Transaction Type equal to Received - Ship");
            _selectorQueryForm.QueryDetailsArgumentCombo();
            _selectorQueryForm.cmbArgument.SetValue(@"Received - Road", doDownArrow: true, searchSubStringTo: 10);
            _selectorQueryForm.btnUpdateLine.DoClick();

            _selectorQueryForm.SelectRowInQueryDetailsTable(@"Transaction Date greater than");
            _selectorQueryForm.QueryDetailsDate();
            _selectorQueryForm.txtArgumentDate.SetValue(@"26082018");
            _selectorQueryForm.btnUpdateLine.DoClick();
            
            _selectorQueryForm.btnFind.DoClick();

            _detailsToCheck = new []
            {
                @"Transaction Type^Received - Road~Id^JLG40790A01~Details^Received~Operator Group Code^OG01,OG02",
                @"Transaction Type^Received - Road~Id^JLG40790A02~Details^Received~Operator Group Code^OG02",
                @"Transaction Type^Received - Road~Id^JLG40790A03~Details^Received~Operator Group Code^OG01,OG02"
            };
            DoColumnOrganiserAndCheckCorrectTableDetails(_modes[1], _detailsToCheck);

            // Mode - ISO Container
            SetModeAndOperatorGroupCodeLine(_modes[2]);
            _selectorQueryForm.btnFind.DoClick();

            _detailsToCheck = new []
            {
                @"Id^JLG40790A01~Cargo Type^ISO Container~Operator^OP01~Operator Group Code^OG01,OG02",
                @"Id^JLG40790A02~Cargo Type^ISO Container~Operator^OP02~Operator Group Code^OG02"
            };
            DoColumnOrganiserAndCheckCorrectTableDetails(_modes[2], _detailsToCheck);

            // Mode - Tracked Item Mode
            SetModeAndOperatorGroupCodeLine(_modes[3]);
            _selectorQueryForm.btnFind.DoClick();

            _detailsToCheck = new []
            {
                @"Id^JLG40790A03~Operator^OP01~Cargo Type^Break-Bulk Cargo~Operator Group Code^OG01,OG02",
                @"Id^JLG40790A01~Operator^OP01~Cargo Type^ISO Container~Operator Group Code^OG01,OG02",
                @"Id^JLG40790A02~Operator^OP02~Cargo Type^ISO Container~Operator Group Code^OG02"
            };
            DoColumnOrganiserAndCheckCorrectTableDetails(_modes[3], _detailsToCheck);
        }

       

        private void SetModeAndOperatorGroupCodeLine(string mode)
        {
            _selectorQueryForm.cmbMode.SetValue(mode);
            _selectorQueryForm.QueryDetailsProperty();

            _selectorQueryForm.cmbProperty.SetValue(@"Operator Group Code");
            _selectorQueryForm.QueryDetailsOperation();
            _selectorQueryForm.QueryDetailsArgumentUpper();
            _selectorQueryForm.cmbOperation.SetValue(@"is one of");
            _selectorQueryForm.GetOGCFindButton();
            _selectorQueryForm.btnOGCFind.DoClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            _selectorQueryForm.GetOGCControls();
            if (_selectorQueryForm.chkOGCAll.IsCheckboxChecked() == true)
                _selectorQueryForm.chkOGCAll.DoClick(false);

            _selectorQueryForm.chkOGCAll.DoClick();
            _selectorQueryForm.btnOGCClose.DoClick();

            _selectorQueryForm.btnAddLine.DoClick();
        }


        private void DoColumnOrganiserAndCheckCorrectTableDetails(string mode, IEnumerable<string> detailsToCheck)
        {
            // Column Organiser
            _selectorQueryForm.tblQueryResults1.GetElement().RightClick();
            MTNControlBase.ContextMenuSelection(@"Result Context", @"Organise Columns...");

            mode = mode.Contains("mode") ? mode : mode + " mode";
            
            var columnOrganiserForm =
                new ColumnOrganiserForm($"Column Organiser Selector - {mode} TT1", "4009");

            columnOrganiserForm.lstSelect.MoveItemsBetweenList(columnOrganiserForm.lstSelect.LstLeft,
                new[] { "Operator Group Code" });
            columnOrganiserForm.btnOK.DoClick();

            _selectorQueryForm.SetFocusToForm();

            var detailsNotFound =
                (from detailToCheck in detailsToCheck
                    let detailsFound =
                        MTNControlBase.FindClickRowInTable(_selectorQueryForm.tblQueryResults1.GetElement(), detailToCheck,
                            doAssert: false)
                    where !detailsFound
                    select detailToCheck).Aggregate<string, string>(null,
                    (current, detailToCheck) => current + (detailToCheck + "\n"));

            Assert.IsTrue(string.IsNullOrEmpty(detailsNotFound),
                $"TestCase40790::DoColumnOrganiserAndCHeckCorrectTableDetails - The following table rows were NOT found: {detailsNotFound}");
        }

    }

}
