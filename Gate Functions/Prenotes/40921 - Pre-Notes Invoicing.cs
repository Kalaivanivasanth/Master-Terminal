using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Prenotes
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40921 : MTNBase
    {
              
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup()
        {
            // Step 16
            FormObjectBase.MainForm.OpenPreNotesFromToolbar();
            PreNoteForm preNoteForm = new PreNoteForm(@"Pre-Notes " + terminalId);
        
            // Step 17  
            preNoteForm.DeletePreNotes(@"JLG40921A01", @"ID^JLG40921A01");

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void PrenoteInvoicing()
        {
            MTNInitialize();
            
            // Step 4 - 5
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.BagOfSand, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", @"JLG40921A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.PreNotified, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);

            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^JLG40921A01~Voyage^MSCK000002" });

            // Step 6
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            
            // Step 7
            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            
            //MTNControlBase.FindTabOnForm(cargoEnquiryTransactionForm.tabTransaction, "Charges", strictCompare: false);
            cargoEnquiryTransactionForm.GetChargesTab();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblCharges,
                // @"Debtor^RDT~Qty^40~Status^Normal~Type^Invoice Line~Narration^TEST40921~Amount^120,000.00", ClickType.ContextClick);
            cargoEnquiryTransactionForm.tblCharges1.FindClickRow(["Debtor^RDT~Qty^40~Status^Normal~Type^Invoice Line~Narration^TEST40921~Amount^120,000.00"], ClickType.ContextClick);            cargoEnquiryTransactionForm.ContextMenuSelect(@"Show Invoice...");

            // Step 9
            InvoiceLinesForm invoiceLinesForm = new InvoiceLinesForm();
            invoiceLinesForm.SetFocusToForm();
            invoiceLinesForm.GetMainFormDetails();
            MTNControlBase.FindClickRowInTable(invoiceLinesForm.tblDetails,
                @"Invoice Line Type^TEST40921~Qty^40~Rate^3,000.00~Total Excl Tax^120,000.00~Status^Normal~Item Details^JLG40921A01~Transaction^Create~Debtor^RDT", 
                ClickType.Click, countOffset: -1);
            //invoiceLinesForm.btnCancel.DoClick();
            invoiceLinesForm.DoCancel40921();

            // Step 10
            cargoEnquiryForm.SetFocusToForm();
            Wait.UntilResponsive(cargoEnquiryForm.GetForm(), TimeSpan.FromMilliseconds(1000));

            // Step 11
            //cargoEnquiryForm.btnEdit.DoClick();
            cargoEnquiryForm.DoEdit();
            cargoEnquiryForm.GetGenericTabTableDetails(@"General", @"4042");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Commodity", @"SANC	Course Sand", 
                EditRowDataType.ComboBoxEdit, doDownArrow: true, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSave.DoClick();
            cargoEnquiryForm.DoSave();

            // Step 12
            cargoEnquiryTransactionForm.SetFocusToForm();
            //MTNControlBase.FindTabOnForm(cargoEnquiryTransactionForm.tabTransaction, "Transactions", strictCompare: false);
            cargoEnquiryTransactionForm.GetTransactionTab();
           // Keyboard.Type(VirtualKeyShort.F5);
            
            // Step 13
            cargoEnquiryTransactionForm.FindClickRowInTable(cargoEnquiryTransactionForm.TblTransactions2.GetElement(),
             @"Type^Created~Charged^Yes~User^SUPERUSER");
            //var found = cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(
               // ["Type^Created~Charged^Yes~User^SUPERUSER"]);
            //MTNKeyboard.Press(VirtualKeyShort.DOWN);
            cargoEnquiryTransactionForm.ContextMenuSelect(@"Resync Transactions...");

            // Step 14
            TransactionResyncForm transactionResyncForm = new TransactionResyncForm();
            //FormObjectBase.ClickButton(transactionResyncForm.btnSelectAll);
            transactionResyncForm.btnSelectAll.DoClick();
            transactionResyncForm.chkLogResynchronize.DoClick();
            transactionResyncForm.chkReadOnly.DoClick();
            //FormObjectBase.ClickButton(transactionResyncForm.btnSave);
            transactionResyncForm.btnSave.DoClick();

            // Step 15
            /*// Monday, 7 April 2025 navmh5 Can be removed 6 months after specified date 
            LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm();
            loggingDetailsForm.FindStringInTable(@"VOID - Void invoice item, no invoice line created");
            loggingDetailsForm.btnCanel.DoClick();*/
            LoggingDetailsForm.ValidateLogDetails(new [] { @"VOID - Void invoice item, no invoice line created" });
           
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_40921_";

            // Create prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n	<JMTInternalPrenote xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalPrenote.xsd'>\n\n		<AllPrenote>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n			<Prenote Terminal='TT1'>\n				<cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n				<product>SMSAND</product>\n          <commodity>PRNT</commodity>\n				<dischargePort>NZAKL</dischargePort>\n				<id>JLG40921A01</id>\n				<imexStatus>Export</imexStatus>\n				<messageMode>A</messageMode>\n				<operatorCode>MSC</operatorCode>\n				<weight>1000</weight>\n				<voyageCode>MSCK000002</voyageCode>\n				<totalQuantity>40</totalQuantity>\n			</Prenote>\n		</AllPrenote>\n	</JMTInternalPrenote>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
           

        }


    }

}
