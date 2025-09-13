using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNUtilityClasses.Classes;
using MTNWindowDialogs.WindowsDialog;
using System;
using System.IO;
using System.Linq;
using HardcodedData.SystemData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase58586 : MTNBase
    {
        private SystemAdminForm _systemAdminForm;
        WindowsSaveDialog _windowsSaveDialog;
        EDIDataTranslationsForm _ediDataTranslationForm;
        EDITranslationMaintenanceForm _ediTranslationMaintenanceForm;
        string _fileName = $"{saveDirectory}SortingMethodExport.csv";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        private void MTNInitialise()
        {
            CallJadeScriptToRun(TestContext, @"resetData_58586");
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void SortingMethodAddUpdateDelete()
        {
            MTNInitialise();

            FormObjectBase.NavigationMenuSelection(@"System Ops|System Admin");
            _systemAdminForm = new SystemAdminForm(@"System Administration");
            _systemAdminForm.cmbTable.SetValue(@"Sorting Method");

            // Click New button
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(1));
            _systemAdminForm.DoNew();

            // Enter Code
            MTNControlBase.SetValueInEditTable(_systemAdminForm.tblDetails, @"Code", @"TESTA58586");
            
            // Click the Save button
            _systemAdminForm.DoSave();

            _systemAdminForm.TblAdministrationItemsRH16A.FindClickRow(["Code^TESTA"], clickType: ClickType.None, searchType: SearchType.Exact);

            // Click New button
            _systemAdminForm.DoNew();

            // Enter Code, Description, Not Available for Entry
            MTNControlBase.SetValueInEditTable(_systemAdminForm.tblDetails, @"Code", @"TESTB");
            MTNControlBase.SetValueInEditTable(_systemAdminForm.tblDetails, @"Description", @"Test 58586 Description");
            MTNControlBase.SetValueInEditTable(_systemAdminForm.tblDetails, @"Not Available for Entry", @"True");

            // Click the Save button
            _systemAdminForm.DoSave();

            _systemAdminForm.TblAdministrationItemsRH16A.FindClickRow(["Code^TESTB"], clickType: ClickType.None, searchType: SearchType.Exact);

            // Click New button
            _systemAdminForm.DoNew();

            // Enter Description
            MTNControlBase.SetValueInEditTable(_systemAdminForm.tblDetails, @"Description", @"Test 58586 Description");
            
            // Click the Save button
            _systemAdminForm.DoSave();

            // Check Error message displayed
            warningErrorForm = new WarningErrorForm(formTitle: @"Errors for Sorting Method");
            string[] errorToCheck =
             {
                   "Code :88973. An input is required for the Code field."
             };
            warningErrorForm.CheckWarningsErrorsExist(errorToCheck);
            warningErrorForm.btnCancel.DoClick();

            // Update exising sorting method
            _systemAdminForm.TblAdministrationItemsRH16A.FindClickRow(["Code^TESTB"], clickType: ClickType.None, searchType: SearchType.Exact);
            _systemAdminForm.DoEdit();
            MTNControlBase.SetValueInEditTable(_systemAdminForm.tblDetails, @"Code", @"TESTC");
            MTNControlBase.SetValueInEditTable(_systemAdminForm.tblDetails, @"Description", @"Test 58586 Description Update");
            MTNControlBase.SetValueInEditTable(_systemAdminForm.tblDetails, @"Not Available for Entry", @"");

            // Click the Save button
            _systemAdminForm.DoSave();

            _systemAdminForm.TblAdministrationItemsRH16A.FindClickRow(["Code^TESTC~Description^Test 58586 Description Update"], clickType: ClickType.None, searchType: SearchType.Exact);

            // Delete the sorting method
            _systemAdminForm.DoDelete();

            /*ConfirmationFormYesNo confirmationForm = new ConfirmationFormYesNo("Confirm Deletion",
                automationIdMessage: "1", automationIdYes: "3", automationIdNo: "4");
            confirmationForm.ConfirmationFormYes();*/

            // Check the sorting method has been deleted
           Assert.IsFalse(_systemAdminForm.TblAdministrationItemsRH16.FindClickRow(
               @"Code^TESTC~Description^Test 58586 Description Update", clickType: ClickType.None,
               searchType: SearchType.Exact, doAssert: false));

            // Export list of sorting methods
            _systemAdminForm.DoExport();

            /*_windowsSaveDialog = new WindowsSaveDialog("Save As");
            _windowsSaveDialog.txtFileName.SetValue(_fileName);
            _windowsSaveDialog.btnSave.DoClick();*/
            WindowsSaveDialog.DoWindowsSaveDialog(WindowsSaveDialog.FormTitleSaveAs, _fileName);

            // Check the file saved
            /*string csvFile = File.ReadAllText(_fileName);
            Assert.IsTrue(csvFile.Contains("TESTA"));
            Assert.IsTrue(csvFile.Contains("TESTB"));
            Assert.IsTrue(csvFile.Contains("FIFO"));
            Assert.IsTrue(csvFile.Contains("LIFO"));*/
            var csvFile = File.ReadAllText(_fileName);
            var requiredStrings = new string[] {"TESTA", "TESTB", "FIFO", "LIFO" };
            
            var allStringPresent = requiredStrings.All(str => csvFile.Contains(str));
            Assert.IsTrue(allStringPresent);

            _systemAdminForm.CloseForm();

            // Sorting method EDI data translation can be created
            FormObjectBase.NavigationMenuSelection(@"System Ops|EDI Data Translations", forceReset: true);
            _ediDataTranslationForm = new EDIDataTranslationsForm(@"EDI Data Translations");
            _ediDataTranslationForm.SetTranslationType(@"Sorting Method");
            _ediDataTranslationForm.cmbOperator.SetValue(Operator.EUP,  doDownArrow: true);
            _ediDataTranslationForm.DoSearch();

            // create an EDI Data Translation for Shipping Agent 
            _ediDataTranslationForm.DoAdd();
            _ediTranslationMaintenanceForm = new EDITranslationMaintenanceForm(@"EDI Translation Maintenance ");

            _ediTranslationMaintenanceForm.txtForeignIn.SetValue(@"SM58586");
            _ediTranslationMaintenanceForm.cmbSystemCode.SetValue(@" [TESTA]", doDownArrow: true, searchSubStringTo: 10);
            _ediTranslationMaintenanceForm.txtForeignOut.SetValue(@"SM58586");
            _ediTranslationMaintenanceForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_ediDataTranslationForm.tblTranslations, @"Foreign In^SM58586~System Code^ [TESTA]~Foreign Out^SM58586", rowHeight: 16, doAssert: false);
            _ediDataTranslationForm.TblTranslations.FindClickRow(["Foreign In^SM58586~System Code^ [TESTA]~Foreign Out^SM58586"], doAssert: false);


        }
    }
}
