using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNForms.FormObjects.EDI_Functions;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase51205 : MTNBase
    {


        private VoyageOperationsForm _voyageOperationsForm;
        protected static string ediFile1 = "M_51205_CustomsMessage.xml";

        private const string TestCaseNumber = @"51205";
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
             "<?xml version='1.0'?>\n<DocumentMetaData xmlns='urn:wco:datamodel:WCO:DM:1'>\n  <WCODataModelVersion>3.2</WCODataModelVersion>\n  <WCODocumentName>RES</WCODocumentName>\n  <CountryCode>NZ</CountryCode>\n  <AgencyAssignedCustomizedDocumentName>RESICR</AgencyAssignedCustomizedDocumentName>\n  <AgencyAssignedCustomizedDocumentVersion>V1.0</AgencyAssignedCustomizedDocumentVersion>\n  <CommunicationMetaData>\n  \n    <Recipient>\n      <ID>40513451K</ID>\n      <RoleCode>TT</RoleCode>\n    </Recipient>\n  </CommunicationMetaData>\n  <Response xmlns='urn:wco:datamodel:WCO:ResponseModel:1'>\n    <IssueDateTime formatCode='204'>20210217151810</IssueDateTime>\n    <FunctionalReferenceID>45426</FunctionalReferenceID>\n    <FunctionCode>24</FunctionCode>\n    <AdditionalDocument>\n      <CategoryCode>BAC</CategoryCode>\n      <ImageBinaryObject mimeCode='application/pdf' uri='c640d1a9-28d0-4b6d-9933-562c3f3c7ef6-lodgement' filename='PDF7-37650682-2021-02-17-1502218018.pdf'>Attached</ImageBinaryObject>\n    </AdditionalDocument>\n    <OverallDeclaration>\n    \n      <Declaration>\n        <ID>37650682</ID>\n        <AcceptanceDateTime formatCode='204'>20210217151810</AcceptanceDateTime>\n        <FunctionalReferenceID>02172112790</FunctionalReferenceID>\n        <VersionID>1</VersionID>\n        <Submitter>\n        \n          <Name>Mediterranean Shipping Company (Aust.) Pty. Limited</Name>\n        </Submitter>\n        <BorderTransportMeans>\n          <Name>JOLLY DIAMANTE</Name>\n          <ID>9471238</ID>\n          <TypeCode>1</TypeCode>\n          <ArrivalDateTime formatCode='102'>20210220</ArrivalDateTime>\n          <FirstArrivalLocationID>NZBLU</FirstArrivalLocationID>\n          <JourneyID>51202_VOY</JourneyID>\n        </BorderTransportMeans>\n        <Consignment>\n          <GoodsStatusCode StatusType='CLEARANCE'>IDR</GoodsStatusCode>\n          <GoodsStatusCode StatusType='MOVEMENT'>DTA</GoodsStatusCode>\n          <SequenceNumeric>3</SequenceNumeric>\n          <AdditionalInformation>\n            <StatementCode>1C</StatementCode>\n            <StatementTypeCode>MTT</StatementTypeCode>\n          </AdditionalInformation>\n          <Consignee>\n          \n            <Name>SPICERS (NZ) LIMITED</Name>\n          </Consignee>\n          <ConsignmentItem>\n            <SequenceNumeric>1</SequenceNumeric>\n            <GoodsMeasure>\n            \n              <GrossMassMeasure unitCode='KGM'>24687.8</GrossMassMeasure>\n            </GoodsMeasure>\n            <Packaging>\n              <SequenceNumeric>1</SequenceNumeric>\n              <QuantityQuantity>38</QuantityQuantity>\n              <TypeCode>PE</TypeCode>\n            </Packaging>\n          </ConsignmentItem>\n          <Consignor>\n          \n            <Name>PAPER AUSTRALIA PTY LTD (AUSTRALIAN</Name>\n          </Consignor>\n          <GoodsLocation>\n          \n            <Name>PORTNZWLG</Name>\n          </GoodsLocation>\n          <TransitDestination>\n          \n            <Name>PORTNZNSN</Name>\n          </TransitDestination>\n          <TransportContractDocument>\n            <ID>MEDUAE486686</ID>\n            <TypeCode>BM</TypeCode>\n          </TransportContractDocument>\n          <TransportEquipment>\n            <SequenceNumeric>1</SequenceNumeric>\n            <FullnessCode>5</FullnessCode>\n            <ID>JLG51205A01</ID>\n            <Seal>\n              <SequenceNumeric>1</SequenceNumeric>\n              <ID>0081012</ID>\n            </Seal>\n          </TransportEquipment>\n          <UnloadingLocation>\n          \n            <ID>NZTRG</ID>\n          </UnloadingLocation>\n        </Consignment>\n       \n        <ResponsibleGovernmentAgency>\n        \n          <ID>MPIBIO</ID>\n        </ResponsibleGovernmentAgency>\n      </Declaration>\n    </OverallDeclaration>\n    <Status>\n      <EffectiveDateTime formatCode='204'>20210217151810</EffectiveDateTime>\n      <NameCode>B08</NameCode>\n      <Pointer>\n      \n        <DocumentSectionCode>07B</DocumentSectionCode>\n      </Pointer>\n      <Pointer>\n      \n        <DocumentSectionCode>42A</DocumentSectionCode>\n      </Pointer>\n      <Pointer>\n        <SequenceNumeric>1</SequenceNumeric>\n        <DocumentSectionCode>08B</DocumentSectionCode>\n        <TagID>G007</TagID>\n      </Pointer>\n    </Status>\n  </Response>\n</DocumentMetaData>");

            MTNSignon(TestContext, userName);
        }

        [TestMethod]
        public void RetainStorageasImportStopConfigurationForTSWMessageProcessing()
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
                @"Stops (dbl click)", @"STOP_51202, Retain Storage Stop");

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
                @"ID^JLG51205A01~Location^51202_VOY~Total Quantity^1~Cargo Type^ISO Container", ClickType.ContextClick, rowHeight: 16);*/
            _voyageOperationsForm.TblOnVessel1.FindClickRow(
                new[] { "ID^JLG51205A01~Location^51202_VOY~Total Quantity^1~Cargo Type^ISO Container" }, ClickType.ContextClick);
            _voyageOperationsForm.ContextMenuSelect(@"Actual Discharge | Actual Discharge Selected ");

            //Step 11 - 13
            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
                    
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"General");
            cargoEnquiryForm.GetGenericTabTableDetails(@"General", @"4042");

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"IMEX Status", @"Storage");
          
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4087");

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus,
                @"Stops (dbl click)", @"Retain Storage Stop");

            //Step 14 - 15
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            EDIOperationsForm ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT2");

            //Delete Existing EDI messages relating to this test.
            ediOperationsForm.DeleteEDIMessages(@"Custom Stops", @"51205");

            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Loaded", @"Load To DB");

            //Step 16 - 18
            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
           
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4087");

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus,
                @"Stops", @"");

            //Step 20
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId, rowHeight: 18);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for " + CargoId + " TT2");
            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                @"Type^Edited~Details^EDIAction => Remove Stop Retain Storage Stop");
            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                @"Type^Stop Cleared~Details^Stop Cleared: Retain Storage Stop.");
            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
               @"Type^Stop Cancelled~Details^Stop Cancelled: STOP_51202.");


        }



        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT2'>\n      <TestCases>51202</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG51205A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MHBS01</locationId>\n      <weight>6000.0000</weight>\n	       <imexStatus>Storage</imexStatus>\n      <dischargePort>USJAX</dischargePort>\n      <voyageCode>51202_VOY</voyageCode>\n           <commodity>MT</commodity>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n\n		  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Create Cargo On Ship
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT2'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG51205A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>51202_VOY</voyageCode>\n            <operatorCode>MSC</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <weight>6000.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>MT</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n     </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }

}




