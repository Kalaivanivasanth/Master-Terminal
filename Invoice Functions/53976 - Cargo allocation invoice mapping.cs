using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNUtilityClasses.Classes;
using System.Drawing;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class _53976___Cargo_allocation_invoice_mapping : MTNBase
    {
        private InvoiceTransactionMapForm _transactionMapForm;
        private CargoInvoiceAllocationForm _cargoInvoiceAllocationForm;
        private InvoiceLineMappingForm _invoiceLineMappingForm;
        private VoyageOperationsForm _voyageOperationsForm;
        private CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

        private const string TestCaseNumber = @"53976";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A17";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
            BaseClassInitialize_New(testContext);
        }
        
        [TestInitialize]
        public void TestInitalize() {}

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        [TestMethod]
        public void VerifyCargoAllocationInvoiceMappingFunction()
        {
            MTNInitialize();

            // Step 3 - 4
         
        }

        void MTNInitialize()
        {
            
            CallJadeScriptToRun(TestContext, $"resetData_{TestCaseNumber}" );
            
            SetupAndLoadInitializeData(TestContext);

            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();

            InitializeDetails();
        }

        private void InitializeDetails()
        {
            
            // Step 4 Open Invoice Functions | Maintenance | Transaction Map 
            FormObjectBase.NavigationMenuSelection(@"Invoice Functions|Maintenance|Transaction Map");

            _transactionMapForm = new InvoiceTransactionMapForm();

            // Step 5 In the Searcher, select Cargo Allocation and click the Search button 
            _transactionMapForm.chkCargo.DoClick(false);
            _transactionMapForm.chkEvent.DoClick(false);
            _transactionMapForm.chkVoyage.DoClick(false);
            _transactionMapForm.chkSnapshotSummary.DoClick(false);
            _transactionMapForm.chkCargoAllocation.DoClick();
            _transactionMapForm.chkBillOfLading.DoClick(false);
            //transactionMapForm.btnSearch.DoClick();
            _transactionMapForm.DoSearch();

            // Step 6 Double click on the Cargo Allocations to expand it 
            _transactionMapForm.lstTransactionMaps.FindItemInList($"Cargo Allocations|TEST{TestCaseNumber}");  //, hasExpandIcon: true);

            // Step 7 Select TEST53976 and from the context menu select Set To Active
            _transactionMapForm.lstTransactionMaps.DoRightClick();
            _transactionMapForm.ContextMenuSelect(@"Set To Active");

            //transactionMapForm.btnProperties.DoClick();
            _transactionMapForm.DoProperties();

            _cargoInvoiceAllocationForm = new CargoInvoiceAllocationForm();
            _cargoInvoiceAllocationForm.btnNew.DoClick();

            _invoiceLineMappingForm = new InvoiceLineMappingForm(@"Invoice Line Mapping TT1");
            _invoiceLineMappingForm.txtCargoType.SetValue(@"CONT");
            _invoiceLineMappingForm.txtIMEXStatus.SetValue(@"IMP");
            _invoiceLineMappingForm.txtOperatorVoyage.SetValue("MSK");
            _invoiceLineMappingForm.txtTransactionType.SetValue(@"RECEIVED - SHIP");
            _invoiceLineMappingForm.btnSave.DoClick();

            // Step 8 Open Voyage Functions | Operations | Voyage Operations
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions | Operations | Voyage Operations ", forceReset: true);
            _voyageOperationsForm = new VoyageOperationsForm();
            _voyageOperationsForm.GetSearcherTab_ClassicMode();
            _voyageOperationsForm.GetSearcherTab();

            // Step 9 Enter LOLO Bays - << Selected >>, select MSCK000010 from Voyage dropdown and click the Select (Return) button
            _voyageOperationsForm.ChkLoloBays.DoClick();
            //voyageOperationsForm.chkShowOffsiteCargo.DoClick();

            _voyageOperationsForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000010, doDownArrow: true);
            //voyageOperationsForm.btnSelect.DoClick();
            _voyageOperationsForm.DoSelect();
            _voyageOperationsForm.GetMainDetails();

            Point positionToClick = new Point(_voyageOperationsForm.TblOnVessel1.GetElement().BoundingRectangle.X + 5,
            _voyageOperationsForm.TblOnVessel1.GetElement().BoundingRectangle.Y + 5);
            Mouse.RightClick(positionToClick);
            //MTNControlBase.ContextMenuSelection(_loloPlanning.contextMenus[1], @"Print...");
            _voyageOperationsForm.ContextMenuSelect(@"Cargo|Create New Cargo On Vessel...");

            RoadGateDetailsReceiveForm roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Add Cargo TT1");
            //roadGateDetailsReceiveForm.GetCargoDetails();

            //roadGateDetailsReceiveForm.ShowCargoType();
            roadGateDetailsReceiveForm.CmbCargoType.SetValue(CargoType.ISOContainer, doDownArrow: true, searchSubStringTo: CargoType.ISOContainer.Length - 1); //, 120);
            
            //roadGateDetailsReceiveForm.ShowContainerDetails();
            roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);

            roadGateDetailsReceiveForm.TxtCargoId.SetValue(CargoId);

           // roadGateDetailsReceiveForm.ShowCommodity();
            roadGateDetailsReceiveForm.CmbCommodity.SetValue(Commodity.GEN, doDownArrow: true);

            roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("2000");
            
            roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.MSK,  doDownArrow: true);
            roadGateDetailsReceiveForm.TxtLocation.SetValue(@"MSCK000010");
            roadGateDetailsReceiveForm.BtnSave.DoClick();

            // Step 10
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Pre-Notes TT1");
            warningErrorForm.btnSave.DoClick();

            /*MTNControlBase.FindClickRowInTable(voyageOperationsForm.tblOnVessel,
                @"ID^"+CargoId+"~Location^MSCK000010~Total Quantity^1~Cargo Type^ISO Container~ISO Type^2200", rowHeight: 20);*/
            _voyageOperationsForm.TblOnVessel1.FindClickRow(
                new[] { "ID^"+CargoId+"~Location^MSCK000010~Total Quantity^1~Cargo Type^ISO Container~ISO Type^2200" });
           // _voyageOperationsForm.cmbDischargeTo.SetValue(@"MKBS10");
           _voyageOperationsForm.SetDischargeTo("MKBS10");

            /*MTNControlBase.FindClickRowInTable(voyageOperationsForm.tblOnVessel,
                @"ID^"+CargoId+"~Location^MSCK000010~Total Quantity^1~Cargo Type^ISO Container~ISO Type^2200", ClickType.ContextClick, rowHeight: 20);*/
            _voyageOperationsForm.TblOnVessel1.FindClickRow(
                new[] { "ID^"+CargoId+"~Location^MSCK000010~Total Quantity^1~Cargo Type^ISO Container~ISO Type^2200" }, 
                ClickType.ContextClick);
            _voyageOperationsForm.ContextMenuSelect(@"Actual Discharge|Actual Discharge Selected");

            _voyageOperationsForm.CloseForm();

            // Step 11 Open General Functions | Cargo Enquiry  
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");

            // Step 12 Enter Cargo ID - JLG53976A17 and click the Search button
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            // Step 13 Click on the View Transactions button in the toolbar
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            // Step 14 Select the Created transaction and select Re-sync... button in the toolbar
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            _cargoEnquiryTransactionForm.GetTransactionTab();
            /*_cargoEnquiryTransactionForm.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
              @"Type^Received - Ship~Charged^No~User^USERDWAT");*/
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(
                ["Type^Received - Ship~Charged^No~User^USERDWAT"]);

            //cargoEnquiryTransactionForm.btnResync.DoClick();
            _cargoEnquiryTransactionForm.DoResync();

            // Step 15 Select Log the Resynchronize checkbox and click the Save button
            TransactionResyncForm transactionResyncForm = new TransactionResyncForm();
            transactionResyncForm.chkLogResynchronize.DoClick();
            transactionResyncForm.btnSave.DoClick();

            // Check logging is as expected
            LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm();
            string[] linesToCheck =
            {
                @"Cargo invoice allocation map TEST45199"
            };
            loggingDetailsForm.FindStringsInTable(linesToCheck);
            loggingDetailsForm.btnCanel.DoClick();
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>"+CargoId+ "</id>\n            <operatorCode>MSK</operatorCode>\n            <locationId>MKBS10</locationId>\n            <weight>2000</weight>\n            <imexStatus>Import</imexStatus>\n            <commodity>GEN</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000010</voyageCode>\n            <isoType>2200</isoType>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n      </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }


    }
}
