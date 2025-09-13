using MTNBaseClasses.BaseClasses.MasterTerminal;
using System;
using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNForms.Controls;
using MTNForms.FormObjects;
using FlaUI.Core.Input;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Invoice_Functions.Invoice_Notes;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase52837 : MTNBase
    {
        private InvoiceListForm _invoiceListForm;
        private InvoiceNotes _invoiceNotes;
        private AuditDetailsForInvoiceNotes _auditDetails;

        private const string TestCaseNumber = @"52837";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";
        private string newInvoiceText = @"An invoice 1234567890_- !@#$%^&*()";
        private string updateInvoiceText = @"update invoice text";
        private string saveMessage = @"successfully";
        private string updateMessage = @"Invoice Notes updated successfully";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        [TestMethod]
        public void VerifyInvoiceNoteFunction()
        {
            MTNInitialize();

            var invoiceNotesTitle = "Invoice Notes TT1 - Invoice DFBU-(1)";
            
            // Step 3 - 4
            _invoiceNotes = new InvoiceNotes(invoiceNotesTitle);
            _invoiceNotes.EnterInvoiceNotesAndClickSaveButton(newInvoiceText);
            _invoiceNotes.VerifySuccessMessageAndClose(saveMessage);
            MTNControlBase.FindClickRowInTable(_invoiceListForm.tblDetails, $"Number^1~Invoice Notes^{newInvoiceText}~Notes - Internal Use^0", ClickType.ContextClick, countOffset: -1);
            _invoiceListForm.contextMenu.MenuSelect("Invoice Notes…");
            
            _invoiceNotes = new InvoiceNotes(invoiceNotesTitle);
            _invoiceNotes.RemoveInvoiceNotes();
            MTNControlBase.FindClickRowInTable(_invoiceListForm.tblDetails, @"Number^1", ClickType.ContextClick, countOffset: -1);
            _invoiceListForm.contextMenu.MenuSelect("Invoice Notes…");

            _invoiceNotes = new InvoiceNotes(invoiceNotesTitle);
            _invoiceNotes.EnterInvoiceNotesAndClickSaveButton(updateInvoiceText);
            _invoiceNotes.VerifySuccessMessageAndClose(updateMessage);
            MTNControlBase.FindClickRowInTable(_invoiceListForm.tblDetails, $"Number^1~Invoice Notes^{updateInvoiceText}", ClickType.ContextClick, countOffset: -1);
            _invoiceListForm.contextMenu.MenuSelect("Invoice Notes…");

            _invoiceNotes = new InvoiceNotes(invoiceNotesTitle);
            _invoiceNotes.chkInternalUse.DoClick();
            _invoiceNotes.btnSave.DoClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            _invoiceNotes.btnClose.DoClick();
            MTNControlBase.FindClickRowInTable(_invoiceListForm.tblDetails, @"Number^1~Notes - Internal Use^1", ClickType.ContextClick, countOffset: -1);
            _invoiceListForm.contextMenu.MenuSelect("Invoice Notes…");

            _invoiceNotes = new InvoiceNotes(invoiceNotesTitle);
            _invoiceNotes.RemoveInvoiceNotes();
            MTNControlBase.FindClickRowInTable(_invoiceListForm.tblDetails, $"Number^1~Invoice Notes^{null}", ClickType.ContextClick, countOffset: -1);
            _invoiceListForm.contextMenu.MenuSelect("Invoice Notes…");
            
            _invoiceNotes = new InvoiceNotes(invoiceNotesTitle);
            _invoiceNotes.btnAudits.DoClick();

            _auditDetails = new AuditDetailsForInvoiceNotes();
            _auditDetails.txtSearchFilter.SetValue(updateInvoiceText);
            // MTNControlBase.FindClickRowInTable(_auditDetails.tblAuditDetails, @"Audit Type^Updated");
            _auditDetails.TblAuditDetails.FindClickRow(["Audit Type^Updated"]);
            _auditDetails.btnClearFilter.DoClick();
            
        }

        void MTNInitialize()
        {
            searchFor = @"_" + TestCaseNumber + "_";

            LogInto<MTNLogInOutBO>();

            InitializeDetails();
        }

        private void InitializeDetails()
        {
            
            CallJadeScriptToRun(TestContext, "resetData_52837");

            // Change Cargo Type
            FormObjectBase.NavigationMenuSelection(@"Invoice Functions|Invoice List");
            _invoiceListForm = new InvoiceListForm();

            //_invoiceListForm.btnSearchInvoice.DoClick();
            _invoiceListForm.DoSearchForInvOrInvLine();
            _invoiceListForm.GetSearchForInvoiceOrInvoiceLineTab();

            _invoiceListForm.txtInvoiceLineNumber.SetValue(@"1");

            //_invoiceListForm.btnFindForInvoiceListForm.DoClick();
            _invoiceListForm.DoFind();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            // Before invoice flow get invoice number
            _invoiceListForm.GetDetailsTable();
            MTNControlBase.FindClickRowInTable(_invoiceListForm.tblDetails, @"Number^1", ClickType.ContextClick, countOffset: -1);
            
            _invoiceListForm.contextMenu.MenuSelect("Invoice Notes…");

        }

    }

}
