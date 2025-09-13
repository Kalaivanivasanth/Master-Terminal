using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using MTNForms.FormObjects;
using FlaUI.Core.Tools;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40720 : MTNBase
    {
        ToDoTaskForm _toDoTaskForm;
        ConfirmationFormOK _confirmationFormOK;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;
        InvoiceLinesForm _invoiceLinesForm;
        RateHistoryForm _rateHistoryForm;

        private const string TestCaseNumber = @"40720";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup()
        {
            SetRateHistoryRate("100.00");
            base.TestCleanup();
        }


        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
            SetRateHistoryRate("100.00");
        }

        [TestMethod]
        public void TotalRateforInvoiceLineWithMultipleServices()
        {

            MTNInitialize();

            // Step 3 Open General Functions | Cargo Enquiry, enter Cargo ID - JLG40720A01 and click the Search button 
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG40720A01");
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^JLG40720A01" }, clickType: ClickType.ContextClick);

            // Step 4 Right click and select Cargo | Add Tasks.... 
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Add Tasks...");

            // Step 5 Check the checkbox for task Add Seal and click the Save & Close button
            _toDoTaskForm = new ToDoTaskForm(@"JLG40720A01 TT1");
            _toDoTaskForm.AddCompleteTask(@"Add Seal", _toDoTaskForm.btnSaveAndClose);

            // Step 6 Click the OK button 
            _confirmationFormOK = new ConfirmationFormOK(@"Tasks Added");
            _confirmationFormOK.btnOK.DoClick();

            // Step 7 Click View Transactions button in the toolbar 
            cargoEnquiryForm.DoViewTransactions();

            // Step 8 Click the Charges tab 
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>("Transactions for JLG40720A01 TT1");
            _cargoEnquiryTransactionForm.GetChargesTab(@"Charges (1)");
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblCharges,
                // @"Debtor^RDT~Qty^1~Status^Normal~Type^Invoice Line~Narration^TEST40720~Amount^300.00",
                // ClickType.ContextClick);
            _cargoEnquiryTransactionForm.tblCharges1.FindClickRow(["Debtor^RDT~Qty^1~Status^Normal~Type^Invoice Line~Narration^TEST40720~Amount^300.00"], ClickType.ContextClick);

            _cargoEnquiryTransactionForm.ContextMenuSelect(@"Show Invoice...");

            // Step 10 Select the last item and check the Total and Rate column 
            _invoiceLinesForm = new InvoiceLinesForm();
            _invoiceLinesForm.SetFocusToForm();
            _invoiceLinesForm.GetMainFormDetails();
            MTNControlBase.FindClickRowInTable(_invoiceLinesForm.tblDetails,
                @"Invoice Line Type^TEST40720~Total Excl Tax^300.00~Rate^300.00~Status^Normal~Item Details^JLG40720A01~Transaction^Seal~Debtor^RDT", ClickType.Click,countOffset:-1);

            // Step 11 Open Invoice Functions | Maintenance | Rate History 
            SetRateHistoryRate("50.00");

            // Step 15 Go back to Invoice Items for Invoice form and click the Refresh button 
            _invoiceLinesForm.SetFocusToForm();
            _invoiceLinesForm.DoRefresh();
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(2));

            MTNControlBase.FindClickRowInTable(_invoiceLinesForm.tblDetails,
                "Invoice Line Type^TEST40720~Total Excl Tax^250.00~Rate^250.00~Status^Normal~Item Details^JLG40720A01~Transaction^Seal~Debtor^RDT", ClickType.Click, countOffset: -1);

            _invoiceLinesForm.CloseForm();

        }


        void SetRateHistoryRate(string rateToSet)
        {
            if (_rateHistoryForm == null)
            {
                FormObjectBase.NavigationMenuSelection(@"Invoice Functions|Maintenance|Rate History", forceReset: true);
                _rateHistoryForm = new RateHistoryForm(@"Rate History TT1");

                // Step 12 In the Searcher, select Debtor - RDT and Service - TEST40720A and click the Find button  
                _rateHistoryForm.GetSearcher();
                _rateHistoryForm.cmbDebtor.SetValue(@"RDT");
                _rateHistoryForm.cmbService.SetValue(@"TEST40720A");
                _rateHistoryForm.DoFind();
            }
            else
            {
                _rateHistoryForm.SetFocusToForm();
            }
            // MTNControlBase.FindClickRowInTable(_rateHistoryForm.tblRates, @"Rate Owner^RDT", ClickType.DoubleClick);
            _rateHistoryForm.TblRates.FindClickRow(["Rate Owner^RDT"], ClickType.DoubleClick);
            //rateHistoryForm.DoEdit();

            // Step 14 In Cost Per Unit, enter 100.00 and click the Save button 
            _rateHistoryForm.GetEditRate();
            Retry.WhileException(() =>
            {
                _rateHistoryForm.txtCostPerUnit.SetValue(rateToSet);
            }, TimeSpan.FromSeconds(2), null, true);
            
            _rateHistoryForm.DoSave();
        }

        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber + "_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite\n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40720A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite\n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40720A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads
        

    }
}
