using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.FormObjects;
using MTNUtilityClasses.Classes;
using MTNForms.Controls;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase58667 : MTNBase

    {
        const string TestCaseNumber = "58667";
        EDIDataTranslationsForm _ediDataTranslationForm;
        EDITranslationMaintenanceForm _ediTranslationMaintenanceForm;
        
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
            LogInto<MTNLogInOutBO>();

            CallJadeScriptToRun(TestContext, $"resetData_{TestCaseNumber}");
        }

        [TestMethod]
        public void EDI_Data_Translations()
        {
            MTNInitialize();

            // 1. Navigate to EDI Data Translations
            FormObjectBase.NavigationMenuSelection(@"System Ops|EDI Data Translations");
            _ediDataTranslationForm = new EDIDataTranslationsForm(@"EDI Data Translations");
            //ediDataTranslationForm.cmbTranslationType.SetValue(@"Shipping Agent", doDownArrow: true, doAssert: true, additionalWaitTimeout:1000, searchSubStringTo:9);
            _ediDataTranslationForm.SetTranslationType(@"Shipping Agent");
            _ediDataTranslationForm.cmbOperator.SetValue(Operator.MSK,  doDownArrow: true);
            _ediDataTranslationForm.DoSearch();

            // create an EDI Data Translation for Shipping Agent 
            _ediDataTranslationForm.DoAdd();
            _ediTranslationMaintenanceForm = new EDITranslationMaintenanceForm(@"EDI Translation Maintenance ");

            _ediTranslationMaintenanceForm.txtForeignIn.SetValue(@"SA58667");
            _ediTranslationMaintenanceForm.cmbSystemCode.SetValue(@"Nelson Shipping Agencies Ltd [NSA]", doDownArrow: true, searchSubStringTo:10);
            _ediTranslationMaintenanceForm.txtForeignOut.SetValue(@"SA58667");
            _ediTranslationMaintenanceForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_ediDataTranslationForm.tblTranslations, @"Foreign In^SA58667~System Code^Nelson Shipping Agencies Ltd [NSA]~Foreign Out^SA58667", rowHeight: 16, doAssert: false);
            _ediDataTranslationForm.TblTranslations.FindClickRow(["Foreign In^SA58667~System Code^Nelson Shipping Agencies Ltd [NSA]~Foreign Out^SA58667"], doAssert: false);

            // create an EDI Data Translation for Shipping Line 
            _ediDataTranslationForm.SetTranslationType(@"Shipping Line");
            _ediDataTranslationForm.cmbOperator.SetValue(Operator.MSK,  doDownArrow: true);
            _ediDataTranslationForm.DoSearch();

            _ediDataTranslationForm.DoAdd();
            _ediTranslationMaintenanceForm = new EDITranslationMaintenanceForm(@"EDI Translation Maintenance ");
            _ediTranslationMaintenanceForm.txtForeignIn.SetValue(@"SL58667");
            _ediTranslationMaintenanceForm.cmbSystemCode.SetValue(@"ABC Shipping Line [ABC]", doDownArrow: true, searchSubStringTo: 10);
            _ediTranslationMaintenanceForm.txtForeignOut.SetValue(@"SL58667");
            _ediTranslationMaintenanceForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_ediDataTranslationForm.tblTranslations, @"Foreign In^SL58667~System Code^ABC Shipping Line [ABC]~Foreign Out^SL58667", rowHeight: 16, doAssert: false);
            _ediDataTranslationForm.TblTranslations.FindClickRow(["Foreign In^SL58667~System Code^ABC Shipping Line [ABC]~Foreign Out^SL58667"], doAssert: false);

            // create an EDI Data Translation for Trailer Type 
            _ediDataTranslationForm.SetTranslationType(@"System - Trailer Type");
            _ediDataTranslationForm.DoSearch();

            _ediDataTranslationForm.DoAdd();
            _ediTranslationMaintenanceForm = new EDITranslationMaintenanceForm(@"EDI Translation Maintenance ");
            _ediTranslationMaintenanceForm.txtForeignIn.SetValue(@"TT58667");
            _ediTranslationMaintenanceForm.SetSystemCode(@"TRAILER39771 [TRAILER39771]");
            _ediTranslationMaintenanceForm.txtForeignOut.SetValue(@"TT58667");
            _ediTranslationMaintenanceForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_ediDataTranslationForm.tblTranslations,
                // @"Foreign In^TT58667~System Code^TRAILER39771 [TRAILER39771]~Foreign Out^TT58667", rowHeight: 16,
                // doAssert: false);
            _ediDataTranslationForm.TblTranslations.FindClickRow(["Foreign In^TT58667~System Code^TRAILER39771 [TRAILER39771]~Foreign Out^TT58667"], doAssert: false);

            // create an EDI Data Translation for Instruction Type 
            _ediDataTranslationForm.SetTranslationType(@"Instruction Types");
            _ediDataTranslationForm.cmbOperator.SetValue(Operator.MSK,  doDownArrow: true);
            _ediDataTranslationForm.cmbTerminal.SetValue(@"MTNQ	MTNQ");
            _ediDataTranslationForm.DoSearch();

            _ediDataTranslationForm.DoAdd();
            _ediTranslationMaintenanceForm = new EDITranslationMaintenanceForm(@"EDI Translation Maintenance ");
            _ediTranslationMaintenanceForm.txtForeignIn.SetValue(@"IT58667");
            _ediTranslationMaintenanceForm.cmbSystemCode.SetValue(@"Instruction Type2 [Instruction Type2]", doDownArrow: true, searchSubStringTo: 10);
            _ediTranslationMaintenanceForm.txtForeignOut.SetValue(@"IT58667");
            _ediTranslationMaintenanceForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_ediDataTranslationForm.tblTranslations, @"Foreign In^IT58667~System Code^Instruction Type2 [Instruction Type2]~Foreign Out^IT58667", rowHeight: 16, doAssert: false);
            _ediDataTranslationForm.TblTranslations.FindClickRow(["Foreign In^IT58667~System Code^Instruction Type2 [Instruction Type2]~Foreign Out^IT58667"], doAssert: false);
        }

    }
}
