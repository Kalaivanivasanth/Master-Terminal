using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45199 : MTNBase
    {
        InvoiceTransactionMapForm _transactionMapForm;
        VoyageOperationsForm _voyageOperationsForm;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void ResyncCreateTransactionUpdatesDischargePortandIMEX()
        {
            MTNInitialize();

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
            _transactionMapForm.lstTransactionMaps.FindItemInList(@"Cargo Allocations|TEST45199");  //, hasExpandIcon: true);

            // Step 7 Select TEST45199 and from the context menu select Set To Active
            _transactionMapForm.lstTransactionMaps.DoRightClick();
            _transactionMapForm.ContextMenuSelect(@"Set To Active");

            // Step 8 Open Voyage Functions | Operations | Voyage Operations
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions | Operations | Voyage Operations ", forceReset: true);
            _voyageOperationsForm = new VoyageOperationsForm();
            _voyageOperationsForm.GetSearcherTab_ClassicMode();
            _voyageOperationsForm.GetSearcherTab();

            // Step 9 Enter LOLO Bays - << Selected >>, select MSCK000002 from Voyage dropdown and click the Select (Return) button
            _voyageOperationsForm.ChkLoloBays.DoClick();

            //voyageOperationsForm.SetValue(voyageOperationsForm.cmbVoyage, @"MSCK000002", 1000);
            _voyageOperationsForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            //voyageOperationsForm.btnSelect.DoClick();
            _voyageOperationsForm.DoSelect();
            _voyageOperationsForm.GetMainDetails();
            // MTNControlBase.FindClickRowInTable(_voyageOperationsForm.TblOnVessel,
                // @"ID^JLG45199A01~Location^MSCK000002", ClickType.ContextClick, rowHeight: 16);
            _voyageOperationsForm.TblOnVessel1.FindClickRow(["ID^JLG45199A01~Location^MSCK000002"], ClickType.ContextClick);
            /*voyageOperationsForm.tblOnVessel1.FindClickRow(
                new[] { "ID^JLG45199A01~Location^MSCK000002" }, ClickType.ContextClick);*/
            _voyageOperationsForm.ContextMenuSelect(@"Tranship...");

            // Step 11 Enter Outbound Voyage -GRIG001 JOLLY GRIGIO, Discharge Port -AKL(NZ) and click the OK button
            TranshipForm transhipForm = new TranshipForm(@"JLG45199A01 TT1");

            //MTNControlBase.SetValue(transhipForm.cmbOutboundVoyage, @"GRIG001");
            transhipForm.cmbOutboundVoyage.SetValue(TT1.Voyage.GRIG001, doDownArrow: true,
                searchSubStringTo: TT1.Voyage.GRIG001.Length - 1);
            
            //MTNControlBase.SetValue(transhipForm.cmbDischargePort, @"AKL (NZ) Auckland");
            transhipForm.cmbDischargePort.SetValue(Port.AKLNZ, doDownArrow: true, searchSubStringTo: Port.AKLNZ.Length - 1);
            transhipForm.btnOK.DoClick();

            // Step 12 Enter Discharge To -MKBS01
            //voyageOperationsForm.SetValue(voyageOperationsForm.cmbDischargeTo, TT1.TerminalArea.MKBS01);
            _voyageOperationsForm.CmbDischargeTo.SetValue(TT1.TerminalArea.MKBS01);
            // MTNControlBase.FindClickRowInTable(_voyageOperationsForm.TblOnVessel,
                // @"ID^JLG45199A01~Location^MSCK000002", ClickType.ContextClick, rowHeight: 16);
            _voyageOperationsForm.TblOnVessel1.FindClickRow(["ID^JLG45199A01~Location^MSCK000002"], ClickType.ContextClick);
            /*voyageOperationsForm.tblOnVessel1.FindClickRow(
                new[] { "ID^JLG45199A01~Location^MSCK000002" }, ClickType.ContextClick);*/
            _voyageOperationsForm.ContextMenuSelect(@"Actual Discharge | Actual Discharge Selected");

            // Step 14 Click Save button on Warnings window
            warningErrorForm = new WarningErrorForm(@"Warnings for Discharge Cargo TT1");
            warningErrorForm.btnSave.DoClick();

            _voyageOperationsForm.CloseForm();

            // Step 15 Open General Functions | Cargo Enquiry  
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");

            // Step 16 Enter Cargo ID - JLG45199A01 and click the Search button
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG45199A01");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            // Step 17 Click on the View Transactions button in the toolbar
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            // Step 18 Select the Created transaction and select Re-sync... button in the toolbar
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
           // _cargoEnquiryTransactionForm.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
            //  @"Type^Created~Charged^No~User^SUPERUSER");
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(
                ["Type^Created~Charged^No~User^SUPERUSER"]);

            //cargoEnquiryTransactionForm.btnResync.DoClick();
            _cargoEnquiryTransactionForm.DoResync40542();

            // Step 19 Select Log the Resynchronize checkbox and click the Save button
            TransactionResyncForm transactionResyncForm = new TransactionResyncForm();
            transactionResyncForm.chkLogResynchronize.DoClick();
            transactionResyncForm.btnSave.DoClick();

            // Check logging is as expected
            LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm();
            string[] linesToCheck =
            {
                @"'Created' Transaction JLG45199A01",
                @"Invoice line map TEST45199",
                @"Invoice type 'TEST45199' from Line Type TEST45199",
                @"Rate for 'TEST45199' derived from Debtor 'RDT' found",
                @"CM_Charge::getRateFromTable Rate is 200",
                @"CM_Charge::evaluateRate Rate change from 0 to 200"
            };
            loggingDetailsForm.FindStringsInTable(linesToCheck);
            loggingDetailsForm.btnCanel.DoClick();

            // Step 20 Back in the Cargo Allocations Transaction Map select TEST41742 and from the context menu select Set To Active
            // This is done as part of Reset Terminal configs script
        }

        

            #region - Setup and Run Data Loads

            private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_45199_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG45199A01</id>\n         <operatorCode>MSC</operatorCode>\n         <weight>2000</weight>\n         <imexStatus>Tranship</imexStatus>\n         <commodity>GEN</commodity>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>GRIG001</voyageCode>\n         <isoType>2200</isoType>\n		 <locationId>MKBS01</locationId>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n	      \n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n");

            // Create Cargo OnShip
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG45199A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n            <dischargePort>NZLYT</dischargePort>\n            <locationId>MSCK000002</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Remain On Board</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GEN</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n    </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }
}
