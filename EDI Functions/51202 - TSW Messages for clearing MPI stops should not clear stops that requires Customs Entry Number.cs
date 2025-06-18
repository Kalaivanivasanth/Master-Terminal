using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNForms.FormObjects.EDI_Functions;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase51202 : MTNBase
    {

       
        private VoyageOperationsForm _voyageOperationsForm;
        protected static string ediFile1 = "M_51202_CustomsMessage.xml";

        private const string TestCaseNumber = @"51202";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
            userName = "USER51202";
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            CreateDataFile(ediFile1,
                "<?xml version='1.0' encoding='utf-16'?> <JMTInternalCustomStop xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>   <AllCustomStops>\n    <CustomStops>\n      <isUSCustomMessage>false</isUSCustomMessage>\n      <packCount>1</packCount>\n      \n    \n      <cargoTypeDescr>CONT</cargoTypeDescr>\n      <clientRef>02172112790</clientRef>\n      <consignee>SPICERS (NZ) LIMITED</consignee>\n   \n      <customsRef>RESICR</customsRef>\n      <entryNumber>37650682</entryNumber>\n      <fullOrMT>F</fullOrMT>\n      <id>JLG51202A01</id>\n      \n      <messageMode>M</messageMode>\n      <responseStatus>B07</responseStatus>\n      <vesselName>JOLLY DIAMANTE</vesselName>\n      <voyageCode>51202_VOY</voyageCode>\n      <transmissionDate>0001-01-01T00:00:00</transmissionDate>\n    </CustomStops>\n    \n  </AllCustomStops>\n</JMTInternalCustomStop>");

            MTNSignon(TestContext, userName);
        }


        [TestMethod]
        public void TSWMessagesForClearingMPIStopsShouldNotClearStopsThatRequiresCustomsEntryNumber()
        {
            MTNInitialize();

            //Step 3 - 5
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry");
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT2");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Ship", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.SetFocusToForm();
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4087");

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus,
                @"Stops (dbl click)", @"Customs Doc 51202, STOP_51202");

            //Step 6 
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Operations|Voyage Operations", forceReset: true);
            _voyageOperationsForm = new VoyageOperationsForm();
            _voyageOperationsForm.GetSearcherTab_ClassicMode();
            _voyageOperationsForm.GetSearcherTab();

            //Step 7 - 8
            _voyageOperationsForm.ChkLoloBays.DoClick();
            //_voyageOperationsForm.SetValue(_voyageOperationsForm.cmbVoyage, @"51202_VOY");
            _voyageOperationsForm.CmbVoyage.SetValue(Voyage.VOY_51202, doDownArrow: true);
            //_voyageOperationsForm.btnSelect.DoClick();
            _voyageOperationsForm.DoSelect();
            _voyageOperationsForm.GetMainDetails();

            //Step 9
            //_voyageOperationsForm.SetValue(_voyageOperationsForm.cmbDischargeTo, @"MHBS01");
            _voyageOperationsForm.CmbDischargeTo.SetValue(@"MHBS01");

            //Step 10
            /*MTNControlBase.FindClickRowInTable(_voyageOperationsForm.tblOnVessel,
                @"ID^JLG51202A01~Location^51202_VOY~Total Quantity^1~Cargo Type^ISO Container", ClickType.ContextClick, rowHeight: 16);*/
            _voyageOperationsForm.TblOnVessel1.FindClickRow(
                new[] { "ID^JLG51202A01~Location^51202_VOY~Total Quantity^1~Cargo Type^ISO Container" }, 
                ClickType.ContextClick);
            _voyageOperationsForm.ContextMenuSelect(@"Actual Discharge | Actual Discharge Selected ");



            //Step 11 - 12
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            EDIOperationsForm ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT2");

            // 1b. Delete Existing EDI messages relating to this test.
            ediOperationsForm.DeleteEDIMessages(@"Custom Stops", @"51202");

            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Loaded", @"Verify");
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Verified", @"Load To DB");

            //Step 13 - 15
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT2");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.SetFocusToForm();
           // cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4087");

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus,
                @"Stops (dbl click)", @"Customs Doc 51202");

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId, rowHeight: 18);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for " + CargoId + " TT2");
            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                @"Type^Edited~Details^EDIAction => Remove Stop STOP_51202");
            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                @"Type^Stop Cleared~Details^Stop Cleared: STOP_51202.");


        }



        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT2'>\n      <TestCases>51202</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG51202A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MHBS01</locationId>\n      <weight>6000.0000</weight>\n	       <imexStatus>Import</imexStatus>\n      <dischargePort>USJAX</dischargePort>\n      <voyageCode>51202_VOY</voyageCode>\n           <commodity>GENL</commodity>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n\n		  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Create Cargo On Ship
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT2'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG51202A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>51202_VOY</voyageCode>\n            <operatorCode>MSC</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <weight>6000.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GENL</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n     </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }

}




