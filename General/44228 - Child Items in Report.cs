using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Print_Preview;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44228 : MTNBase
    {
        CargoLabelPrintRequestForm cargoLabelPrintRequestForm;
        AutoCargoIdConfigForm autoCargoIdConfigForm;
        PrintPreviewJadeForm printPreviewJadeForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)  => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void PrintCargoLabel()
        {
            MTNInitialize();

            string reportNameCM = @"RptCM " + GetUniqueId() + ".png";
            string reportNameGL = @"RptGL " + GetUniqueId() + ".png";

            //1. Find cargo JLG44091A01 in cargo enquiry (parent cargo)
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
           
            //2. run through the 2 print cycles with parent cargo and then child cargo to ensure printing O.K.
            PrintCycle(@"ISO Container",@"44228A01",@"CM_CargoIdLabel", reportNameCM);
            PrintCycle(@"Break-Bulk Cargo", @"44228A02", @"GL_CargoIdLabel", reportNameGL);

        }

        public void PrintCycle(string strCargoType, string strCargoID, string strLabelFormat, string reportName)

        {

            string [] imagetextToCheck = new string[]
{
                // finding the following values will (at least) assert the report has been printed
                strCargoID,
                @"Discharge Port",
                @"MSCK",
                @"Lyttelton",
                @"(NZLYT)",
};

            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", strCargoType, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", strCargoID);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + strCargoID, clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^" + strCargoID], clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Print Cargo Label(s)...");

            cargoLabelPrintRequestForm = new CargoLabelPrintRequestForm();
            //FormObjectBase.ClickButton(cargoLabelPrintRequestForm.btnConfig);
            cargoLabelPrintRequestForm.btnConfig.DoClick();

            autoCargoIdConfigForm = new AutoCargoIdConfigForm(@"Auto Id Configuration TT1");
            //MTNControlBase.SetValue(autoCargoIdConfigForm.txtLabelFormat, strLabelFormat);
            autoCargoIdConfigForm.txtLabelFormat.SetValue(strLabelFormat);
            autoCargoIdConfigForm.btnOK.DoClick();

            ConfirmationFormOK confirmationFormOK = new ConfirmationFormOK("Information", automationIdOK: "4");
            confirmationFormOK.btnOK.DoClick();

            autoCargoIdConfigForm.SetFocusToForm();
            autoCargoIdConfigForm.btnCancel.DoClick();

            cargoLabelPrintRequestForm.SetFocusToForm();
            cargoLabelPrintRequestForm.chkPrintPreview.DoClick();
            //FormObjectBase.ClickButton(cargoLabelPrintRequestForm.btnPrint);
            cargoLabelPrintRequestForm.btnPrint.DoClick();

            printPreviewJadeForm = new PrintPreviewJadeForm(@"Preview of printed page 1 of 1 pages");
            Miscellaneous.CaptureElementAsImage(TestContext, printPreviewJadeForm.rptPane, reportName);
            printPreviewJadeForm.btnCancel.DoClick();

            ValidateTextInImage(reportName, imagetextToCheck);
            
        }


    }

}
