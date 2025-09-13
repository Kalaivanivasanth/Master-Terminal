using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40866 : MTNBase
    {
        VoyageEnquiryForm _voyageEnquiryForm;
        VoyageDefinitionForm _voyageDefinitionForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
       
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void SelectVoyageByPartialSearch()
        {
            MTNInitialize(); 
            
            // 1. Navigate to Voyage enquiry form and find voyage MSCK000002 - double click to open
            // Monday, 7 April 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry");
            FormObjectBase.MainForm.OpenVoyageEnquiryFromToolbar();
            _voyageEnquiryForm = new VoyageEnquiryForm();

            _voyageEnquiryForm.FindVoyageByVoyageCode(TT1.Voyage.MSCK000002);
            // Monday, 7 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages,$"Code^{TT1.Voyage.MSCK000002}", ClickType.DoubleClick);
            _voyageEnquiryForm.TblVoyages.FindClickRow(new[] { $"Code^{TT1.Voyage.MSCK000002}" }, ClickType.DoubleClick);

            // 2. send partial text to voyage combo box
            _voyageDefinitionForm = new VoyageDefinitionForm();
            _voyageDefinitionForm.cmbVoyageCodeCombo.SetValue(@"Jolly Diamante 2 OFFICIAL - (MESDAI200001)", doDownArrow: true, searchSubStringTo: 16);
            

            // 3. validate the correct voyage is retrieved
            _voyageDefinitionForm.txtVoyageCode.ValidateText(TT1.Voyage.MESDAI200001);

            // 4. try another voyage
            _voyageDefinitionForm.cmbVoyageCodeCombo.SetValue("MSC KATYA R. - (42500)", doDownArrow: true, searchSubStringTo: 5);
            _voyageDefinitionForm.txtVoyageCode.ValidateText(TT1.Voyage.V42500);
        }

    }

}
