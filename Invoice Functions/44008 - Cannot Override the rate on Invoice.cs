using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Invoice_Functions;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44008 : MTNBase
    {


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_NewForCASH(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
      
        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();            
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>(userName);
        }
       

        [TestMethod]
        public void CannotOverrideTheRateOnInvoice()
        {
            
            MTNInitialize();

            // Step 4
            //FormObjectBase.NavigationMenuSelection(@"Invoice Functions | Invoice Lines");
            FormObjectBase.MainForm.OpenInvoiceLinesFromToolbar();
            InvoiceLinesForm invoiceLinesForm = new InvoiceLinesForm();
           
            // Step 5
            invoiceLinesForm.GetSearcher(InvoiceLinesForm.SearcherForm.ByCargo);
            invoiceLinesForm.cmbBusinessUnit.SetValue(@"DFBU", doDownArrow: true);
            invoiceLinesForm.cmbSite.SetValue(Site.OnSite, doDownArrow: true, searchSubStringTo: 5);
            invoiceLinesForm.cmbCargoType.SetValue(CargoType.ISOContainer, doDownArrow: true, searchSubStringTo: 9);
            invoiceLinesForm.cmbCargoId.SetValue(@"JLG44008A01");
            invoiceLinesForm.DoFind();

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
                       
            // Step 6
            invoiceLinesForm.SetFocusToForm();
            invoiceLinesForm.GetMainFormDetails();
            MTNControlBase.FindClickRowInTable(invoiceLinesForm.tblDetails,
              @"Invoice Line Type^Received-Road~Rate^500.00~Total Excl Tax^500.00~Status^Normal~Item Details^JLG44008A01~Transaction^Received - Road~Debtor^MK3",
              countOffset: -1, rowHeight: 16);
            
            invoiceLinesForm.DoOverrideRate44008();
            
            // Step 7
            invoiceLinesForm.GetRatesOverrideDetails();
            invoiceLinesForm.txtNewOverridingRate.SetValue(@"-1.00");
            invoiceLinesForm.DoOverrideRateSetRate();
            invoiceLinesForm.DoOverrideRateSave();

            // Step 8
            MTNControlBase.FindClickRowInTable(invoiceLinesForm.tblDetails,
                @"Invoice Line Type^Received-Road~Rate^-1.00~Total Excl Tax^-1.00~Status^Normal~Item Details^JLG44008A01~Transaction^Received - Road~Debtor^MK3",
                countOffset: -1, xOffset: invoiceLinesForm.tblDetails.BoundingRectangle.Width - 50, rowHeight: 16);
            invoiceLinesForm.DoOverrideRate44008();
            
            // Step 9
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(invoiceLinesForm.tblRates, @"Service^Received-Road~Current Rate^-1.00~New Overriding Rate^-1.00");
            invoiceLinesForm.TblRates.FindClickRow(new [] { "Service^Received-Road~Current Rate^-1.00~New Overriding Rate^-1.00" });
            invoiceLinesForm.DoOverrateRemoveRate();
            invoiceLinesForm.DoOverrideRateSave();

            MTNControlBase.FindClickRowInTable(invoiceLinesForm.tblDetails,
               @"Invoice Line Type^Received-Road~Rate^500.00~Total Excl Tax^500.00~Status^Normal~Item Details^JLG44008A01~Transaction^Received - Road~Debtor^MK3",
               countOffset: -1, xOffset: invoiceLinesForm.tblDetails.BoundingRectangle.Width - 50, rowHeight: 16);
        }

        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = "_44008_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='CASH'>\n      <TestCases>44008</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44008A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MKOP</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>JDIAM0001</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='CASH'>\n      <TestCases>44008</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44008A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MKOP</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>JDIAM0001</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }

}
