using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Invoice_Functions.Maintenance;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase58325 : MTNBase
    {
        AuditEnquiryForm _auditEnquiryForm;
        ConfirmationFormOK _confirmationFormOK;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialise() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void INCOTermAddUpdateDelete()
        {
            MTNInitialise();

            FormObjectBase.MainForm.OpenInvoiceINCOTermsFromToolbar();
            var invoiceINCOForm = new InvoiceINCOTerms(@"Invoice INCO Terms Enquiry DFBU TT1");

            // Step 3 Click the New button 
            invoiceINCOForm.btnNew.DoClick();

            var invoiceINCOTermsMaintenance = new InvoiceINCOTermsMaintenance(@"Invoice INCO Terms Maintenance DFBU TT1");
            invoiceINCOTermsMaintenance.txtCode.SetValue("TEST012345678901234567890123456");
            invoiceINCOTermsMaintenance.txtDescription.SetValue("TEST012345678901234567890123456789TEST012345678901234567890123456");
            invoiceINCOTermsMaintenance.btnSave.DoClick();

            invoiceINCOTermsMaintenance.btnClose.DoClick();

            // Step 5 Click the Update button
            invoiceINCOForm.TblINCOTerms.FindClickRow(
                new[] { "Code^TEST01234567890123456789012345~Description^TEST012345678901234567890123456789TEST0123456789012345678901" });
            invoiceINCOForm.btnUpdate.DoClick();
            invoiceINCOTermsMaintenance = new InvoiceINCOTermsMaintenance(@"Invoice INCO Terms Maintenance DFBU TT1");
            invoiceINCOTermsMaintenance.txtCode.SetValue("TEST01");
            invoiceINCOTermsMaintenance.txtDescription.SetValue("TEST012");
            invoiceINCOTermsMaintenance.btnSave.DoClick();

            _confirmationFormOK = new ConfirmationFormOK(@"Message");
            Assert.IsTrue(_confirmationFormOK.CheckMessageMatch(@"INCO Terms update successful"));
            _confirmationFormOK.btnOK.DoClick();

            invoiceINCOTermsMaintenance.btnClose.DoClick();

            // Step 6 Click the Audit button
            invoiceINCOForm.btnAudits.DoClick();

            _auditEnquiryForm = new AuditEnquiryForm();
            _auditEnquiryForm.btnSearch.DoClick();
            // MTNControlBase.FindClickRowInTable(_auditEnquiryForm.tblAuditItems, @"Description^Invoice INCO Terms - TEST01~Audit Type^Created", rowHeight: 16, findInstance: 1);
            // MTNControlBase.FindClickRowInTable(_auditEnquiryForm.tblAuditItems, @"Description^Invoice INCO Terms - TEST01~Audit Type^Updated", rowHeight: 16, findInstance: 1);
            _auditEnquiryForm.TblAuditItems.FindClickRow([
                "Description^Invoice INCO Terms - TEST01~Audit Type^Created",
                "Description^Invoice INCO Terms - TEST01~Audit Type^Updated"
            ], rowInstance: 1);
;            _auditEnquiryForm.CloseForm();

            // Step 7 Click the Remove button
            invoiceINCOForm.TblINCOTerms.FindClickRow(new[] { "Code^TEST01~Description^TEST012" });
            invoiceINCOForm.btnRemove.DoClick();

            // Step 8 Click the Yes button on the confirmation dialog
            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo("Confirm Removal");
            confirmationFormYesNo.btnYes.DoClick();
        }
    }
}
