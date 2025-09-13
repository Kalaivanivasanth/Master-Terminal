using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Invoice_Functions;
using InvoiceTransactionMapForm = MTNForms.FormObjects.Invoice_Functions.InvoiceTransactionMapForm;
using FlaUI.Core.Input;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41742 : MTNBase
    {
        InvoiceTransactionMapForm _transactionMapForm;
        CargoInvoiceAllocationForm _cargoInvoiceAllocationForm;
        InvoiceLineMappingForm _invoiceLineMappingForm;
        TransactionTypeSelectionForm _transactionTypeSelectionForm;
        ConfirmationFormYesNo _confirmationForm;

       [ClassInitialize]
       public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

       [TestInitialize]
       public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void CargoAllocationTransMapUpdateDeleteSelectedEntry()
        {
            
            MTNInitialize();
            
            // Step 1 Open Invoice Functions | Maintenance | Transaction Map 
            FormObjectBase.NavigationMenuSelection(@"Invoice Functions|Maintenance|Transaction Map");

            _transactionMapForm = new InvoiceTransactionMapForm();

            // Step 2 In the Searcher, select Cargo Allocation and click the Search button 
            _transactionMapForm.chkCargo.DoClick(false);
            _transactionMapForm.chkEvent.DoClick(false);
            _transactionMapForm.chkVoyage.DoClick(false);
            _transactionMapForm.chkSnapshotSummary.DoClick(false);
            _transactionMapForm.chkCargoAllocation.DoClick();
            _transactionMapForm.chkBillOfLading.DoClick(false);
            _transactionMapForm.DoSearch();

            // Step 3 Double click on the Cargo Allocations to expand it 
            _transactionMapForm.lstTransactionMaps.FindItemInList(@"Cargo Allocations|TEST41742 (Active)"); //, hasExpandIcon: true);

            // Step 4 Select TEST41742 and in the toolbar, click the Properties... button 
            _transactionMapForm.DoProperties();

            _cargoInvoiceAllocationForm = new CargoInvoiceAllocationForm(@"TEST41742 - Cargo Invoice Allocation TT1");

            // Step 5 Click the New button 
            _cargoInvoiceAllocationForm.btnNew.DoClick();

            _invoiceLineMappingForm = new InvoiceLineMappingForm(@"Invoice Line Mapping TT1");

            // Step 6 Click the Find button for Transaction Type 
            _invoiceLineMappingForm.DoRowStuff(new [] { "Transaction Type^" }, true);

            _transactionTypeSelectionForm = new TransactionTypeSelectionForm(@"Transaction Type TT1");

            // Step 7 Select Attached and click the OK button 
            MTNControlBase.SetUnsetValueCheckboxTable(_transactionTypeSelectionForm.tblTransactions,
                new string[,] { { @"Attached", @"1" } });

            _transactionTypeSelectionForm.btnOK.DoClick();

            // Step 8 Click the Save button 
            _invoiceLineMappingForm.SetFocusToForm();
            _invoiceLineMappingForm.btnSave.DoClick();

            _cargoInvoiceAllocationForm.SetFocusToForm();

            // Step 9 Select the first entry, i.e. ADD DOCUMENT ACTION TASK 
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_cargoInvoiceAllocationForm.tblCargoInvoiceAllocations,
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date     @"Allocation^ADD DOCUMENT ACTION TASK", ClickType.Click, rowHeight: 12);
            _cargoInvoiceAllocationForm.TblCargoInvoiceAllocations.FindClickRow(new [] { "Allocation^ADD DOCUMENT ACTION TASK" });

            // Step 10 Press the Down arrow button from the keyboard  
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(1));
            Keyboard.Press(VirtualKeyShort.DOWN);

            // Step 11 Press the Down arrow button from the keyboard  
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(1));
            Keyboard.Press(VirtualKeyShort.DOWN);

            // Step 12 Click the Update button 
            _cargoInvoiceAllocationForm.btnUpdate.DoClick();

            _invoiceLineMappingForm = new InvoiceLineMappingForm(@"Invoice Line Mapping TT1");

            var description = _invoiceLineMappingForm.txtDescription.GetText();
            
            Assert.IsTrue(description.Equals("ATTACHED"), $"Description - Expected: ATTACHED    Actual: {description}");

            // Step 13 Change the Description to TEST 
            _invoiceLineMappingForm.txtDescription.SetValue(@"TEST");

            // Step 14 Click the Save button 
            _invoiceLineMappingForm.btnSave.DoClick();

            _cargoInvoiceAllocationForm.SetFocusToForm();

            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_cargoInvoiceAllocationForm.tblCargoInvoiceAllocations,
             // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date    @"Allocation^TEST~Transaction^Attached", ClickType.Click);
            _cargoInvoiceAllocationForm.TblCargoInvoiceAllocations.FindClickRow(new [] { "Allocation^TEST~Transaction^Attached" });

            // Step 15 Click the Delete button 
            _cargoInvoiceAllocationForm.btnDelete.DoClick();

            // Step 16 Click the Yes button 
            _confirmationForm = new ConfirmationFormYesNo(@"Confirm Delete");
            _confirmationForm.btnYes.DoClick();
        }
    }
}
