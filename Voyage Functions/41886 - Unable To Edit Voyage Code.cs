using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41886: MTNBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"resetData_41886");
            LogInto<MTNLogInOutBO>("USER41886");
        }

        [TestMethod]
        public void UnableToEditVoyageCode()
        {
            MTNInitialize();
            
            // Step 2 - 3
            SetConfigurationForVoyageCannotUpdateVoyageCodes(@"0");

            // Step 4
            FormObjectBase.NavigationMenuSelection(@"Test Terminal 1 - Non Cash  (TT1)|Voyage Functions|Admin|Voyage Enquiry");
            VoyageEnquiryForm voyageEnquiryFormTT1 = new VoyageEnquiryForm(@"Voyage Enquiry TT1");
            // MTNControlBase.FindClickRowInTable(voyageEnquiryFormTT1.tblVoyages, @"Code^TESTI0000~Vessel Name^Test41886-A");
            voyageEnquiryFormTT1.TblVoyages.FindClickRow(["Code^TESTI0000~Vessel Name^Test41886-A"]);

            // Step 5
            FormObjectBase.NavigationMenuSelection(
                @"Test Terminal 02 - 37793 ONLY  (TT2)|Voyage Functions|Admin|Voyage Enquiry", forceReset: true);
            VoyageEnquiryForm voyageEnquiryFormTT2 = new VoyageEnquiryForm(@"Voyage Enquiry TT2");
            // MTNControlBase.FindClickRowInTable(voyageEnquiryFormTT2.tblVoyages, @"Code^TESTB0000~Vessel Name^Test41886-A");
            voyageEnquiryFormTT2.TblVoyages.FindClickRow(["Code^TESTB0000~Vessel Name^Test41886-A"]);
            voyageEnquiryFormTT2.btnEditVoyage.DoClick();

            VoyageDefinitionForm voyageDefinitionFormTT2 = new VoyageDefinitionForm(@"Test41886-A - (TESTB0000) Voyage Definition TT2");
            voyageDefinitionFormTT2.txtVoyageCode.SetValue(@"TESTI0000");
            voyageDefinitionFormTT2.DoSave();
            warningErrorForm = new WarningErrorForm(@"Warnings for Voyage Definition TT2");
            warningErrorForm.btnSave.DoClick();

            voyageDefinitionFormTT2.SetFocusToForm();
            voyageDefinitionFormTT2.CloseForm();

            voyageEnquiryFormTT2.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(voyageEnquiryFormTT2.tblVoyages, @"Code^TESTI0000~Vessel Name^Test41886-A");
            voyageEnquiryFormTT2.TblVoyages.FindClickRow(["Code^TESTI0000~Vessel Name^Test41886-A"]);
        }

        private static void SetConfigurationForVoyageCannotUpdateVoyageCodes(string value)
        {
            FormObjectBase.NavigationMenuSelection(
                @"Test Terminal 02 - 37793 ONLY  (TT2)|Voyage Functions|Admin|Voyage Configuration");
            VoyageConfigurationForm voyageConfigurationForm = new VoyageConfigurationForm();

            voyageConfigurationForm.GetConfigurationForTerminalDetails();

            // voyageConfigurationForm.btnEdit.DoClick();
            voyageConfigurationForm.DoEdit();
            MTNControlBase.SetValueInEditTable(voyageConfigurationForm.tblConfighrationForTerminal, @"Cannot Update Voyage Code",
                value, EditRowDataType.CheckBox, doReverse: true);
            MTNControlBase.SetValueInEditTable(voyageConfigurationForm.tblConfighrationForTerminal, @"Cannot Update External Voyage Code",
                value, EditRowDataType.CheckBox, doReverse: true);
            voyageConfigurationForm.DoSave();
            voyageConfigurationForm.CloseForm();
        }
    }

}
