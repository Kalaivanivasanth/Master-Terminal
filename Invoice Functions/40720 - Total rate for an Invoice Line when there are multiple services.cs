using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNAutomationTests.BaseClasses;
using MTNAutomationTests.Form.MasterTerminal;
using MTNAutomationTests.BaseClasses.MasterTerminal;

namespace MTNAutomationTests.MTNTests.Invoice
{
    [TestClass, TestCategory("WinApp")]
    public class TestCase40720 : MTNBase
    {
               
        [ClassInitialize]
        static public void ClassInitialize(TestContext testContext)
        {

            // Setup data
            searchFor = @"_40720_";
            SetupAndLoadInitializeData(testContext);

            // Start Master Terminal
            BaseClassInitialize(testContext);

            // Signon Master Terminal
            SignonPageObject signonForm = new SignonPageObject();
            signonForm.Signon(testContext);
            signonForm.ClickSaveButton();

        }

        [TestInitialize]
        public void TestInitialize()
        {
           
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            //base.TestCleanup();            
        }
       

        [TestMethod]
        public void TotalRateForInvoiceLineWhereMultipleServices()
        {

            CargoEnquiryForm cargoEnquiryForm = null;
            CargoEnquiryForm.FindCargoInCargoEnquiryUsingQuickFindAutomation(@"F CARGO D=JLG40720A01 M=Q T=tblData R=billOfLading",
                @"JLG40921A01", ref cargoEnquiryForm, clickType:ClickType.ContextClick);
            ContextMenuSelection(@"Form Context Menu", @"Cargo|Add Tasks....");

            /*
                      
            // Step 4
            NavigationMenuSelection(@"Invoice Functions | Invoice Lines");
            InvoiceLinesForm invoiceLinesForm = new InvoiceLinesForm();
           
            // Step 5
            invoiceLinesForm.GetSearcherDetailsInvoiceListByDebtor();
            SendTextToCombobox(invoiceLinesForm.cmbBusinessUnit, @"DFBU");
            SendTextToCombobox(invoiceLinesForm.cmbDebtor, @"MK3");
            invoiceLinesForm.ClickButton(invoiceLinesForm.btnFind);

            Thread.Sleep(500);
            invoiceLinesForm.GetMainFormDetails();
                       
            // Step 6
            invoiceLinesForm.GetForm().Focus();
            FindClickRowInTable(invoiceLinesForm.tblDetails,
               @"Description^Received-Road~Rate^500.00~Total^500.00~Status^Normal~Item Details^JLG44008A01~Transaction^ReceiveRoad~Debtor^MK3", 
               countOffset: -1, xOffset: invoiceLinesForm.tblDetails.BoundingRectangle.Width - 50, rowHeight: 16);
            invoiceLinesForm.ClickButton(invoiceLinesForm.btnOverrideRate);            

            // Step 7
            invoiceLinesForm.GetRatesOverrideDetails();
            SendTextToTextbox(invoiceLinesForm.txtNewOverridingRate, @"-1.00");
            invoiceLinesForm.ClickButton(invoiceLinesForm.btnSetRate);
            invoiceLinesForm.ClickButton(invoiceLinesForm.btnSave);

            // Step 8
            FindClickRowInTable(invoiceLinesForm.tblDetails,
                @"Description^Received-Road~Rate^-1.00~Total^-1.00~Status^Normal~Item Details^JLG44008A01~Transaction^ReceiveRoad~Debtor^MK3",
                countOffset: -1, xOffset: invoiceLinesForm.tblDetails.BoundingRectangle.Width - 50, rowHeight: 16);
            invoiceLinesForm.ClickButton(invoiceLinesForm.btnOverrideRate);
            
            // Step 9
            FindClickRowInTable(invoiceLinesForm.tblRates, @"Service^Received-Road~Current Rate^-1.00~New Overriding Rate^-1.00");
            invoiceLinesForm.ClickButton(invoiceLinesForm.btnRemoveRate);
            invoiceLinesForm.ClickButton(invoiceLinesForm.btnSave);

            FindClickRowInTable(invoiceLinesForm.tblDetails,
               @"Description^Received-Road~Rate^500.00~Total^500.00~Status^Normal~Item Details^JLG44008A01~Transaction^ReceiveRoad~Debtor^MK3",
               countOffset: -1, xOffset: invoiceLinesForm.tblDetails.BoundingRectangle.Width - 50, rowHeight: 16);

    */
        }

        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

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
