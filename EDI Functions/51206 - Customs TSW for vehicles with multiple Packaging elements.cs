using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using FlaUI.Core.Input;
using System.Drawing;
using HardcodedData.SystemData;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase51206 : MTNBase
    {

        private VoyageBOLMaintenanceForm _voyageBOLMaintenanceForm;
        protected static string ediFile1 = "M_51206_CustomsMessage.xml";
        
        private const string TestCaseNumber = @"51206";
        private readonly string[] CargoId =
        {
            @"JLG" + TestCaseNumber + @"A01",
            @"JLG" + TestCaseNumber + @"A02"
        };

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
            "<?xml version='1.0'?>    \n    <DocumentMetaData xmlns='urn:wco:datamodel:WCO:DM:1'>    \n    <WCODataModelVersion>3.2</WCODataModelVersion>    \n    <WCODocumentName>RES</WCODocumentName>    \n    <CountryCode>NZ</CountryCode>    \n    <AgencyAssignedCustomizedDocumentName>RESIM1</AgencyAssignedCustomizedDocumentName>    \n    <AgencyAssignedCustomizedDocumentVersion>V1.1</AgencyAssignedCustomizedDocumentVersion>    \n    <CommunicationMetaData>    \n        \n    <Recipient>    \n    <ID>40522745C</ID>    \n    <RoleCode>GC</RoleCode>    \n    </Recipient>    \n    </CommunicationMetaData>    \n    <Response xmlns='urn:wco:datamodel:WCO:ResponseModel:1'>    \n    <IssueDateTime formatCode='204'>20210204162630</IssueDateTime>    \n    <FunctionalReferenceID>122698</FunctionalReferenceID>    \n    <FunctionCode>24</FunctionCode>    \n    <AdditionalInformation>    \n    <StatementDescription>2 LOOSE PACKAGE(S) OR ITEM(S)</StatementDescription>    \n    <StatementTypeCode>DIN</StatementTypeCode>    \n    </AdditionalInformation>    \n    <OverallDeclaration>    \n        \n    <Declaration>    \n    <ID>47905771</ID>    \n    <AcceptanceDateTime formatCode='204'>20210204162630</AcceptanceDateTime>    \n    <FunctionalReferenceID>S00014121</FunctionalReferenceID>    \n    <TotalGrossMassMeasure unitCode='KGM'>3002</TotalGrossMassMeasure>    \n    <VersionID>1</VersionID>    \n    <JurisdictionDateTime formatCode='102'>20210214</JurisdictionDateTime>    \n    <Submitter>    \n        \n    <Name>Taurus Logistics Limited</Name>    \n    </Submitter>    \n    <Agent>    \n        \n    <Name>Taurus Logistics Limited</Name>    \n    </Agent>    \n    <BorderTransportMeans>    \n    <Name>MSC KATYA R.</Name>    \n    <ID>9326079</ID>    \n    <TypeCode>1</TypeCode>    \n    <JourneyID>VOY_51206</JourneyID>    \n    </BorderTransportMeans>    \n    <Carrier>    \n        \n    <Name>TOYOFUJI SHIPPING LINE CO LTD</Name>    \n    </Carrier>    \n    <GoodsShipment>    \n        \n    <Consignment>    \n        \n    <TransportContractDocument>    \n    <ID>BOL51206</ID>    \n    <TypeCode>BM</TypeCode>    \n    <Pointer>    \n        \n    <DocumentSectionCode>42A</DocumentSectionCode>    \n    </Pointer>    \n    <Pointer>    \n    <SequenceNumeric>1</SequenceNumeric>    \n    <DocumentSectionCode>93A</DocumentSectionCode>    \n    </Pointer>    \n    <Pointer>    \n    <SequenceNumeric>2</SequenceNumeric>    \n    <DocumentSectionCode>93A</DocumentSectionCode>    \n    </Pointer>    \n    </TransportContractDocument>    \n    </Consignment>    \n    </GoodsShipment>    \n    <Importer>    \n        \n    <Name>Mark Stuart Watson</Name>    \n    </Importer>    \n    <Packaging>    \n    <SequenceNumeric>1</SequenceNumeric>    \n    <QuantityQuantity>1</QuantityQuantity>    \n    <TypeCode>VN</TypeCode>    \n    </Packaging>    \n    <Packaging>    \n    <SequenceNumeric>2</SequenceNumeric>    \n    <QuantityQuantity>1</QuantityQuantity>    \n    <TypeCode>VN</TypeCode>    \n    </Packaging>    \n    <UnloadingLocation>    \n        \n    <ID>NZWLG</ID>    \n    </UnloadingLocation>    \n    <ResponsibleGovernmentAgency>    \n        \n    <ID>NZCS</ID>    \n    </ResponsibleGovernmentAgency>    \n    </Declaration>    \n    </OverallDeclaration>    \n    <Status>    \n    <EffectiveDateTime formatCode='204'>20210204162630</EffectiveDateTime>    \n    <NameCode>819</NameCode>    \n    <ReleaseDateTime formatCode='204'>20210204162630</ReleaseDateTime>    \n    <Pointer>    \n        \n    <DocumentSectionCode>07B</DocumentSectionCode>    \n    </Pointer>    \n    <Pointer>    \n        \n    <DocumentSectionCode>42A</DocumentSectionCode>    \n    </Pointer>    \n    <Pointer>    \n    <SequenceNumeric>1</SequenceNumeric>    \n    <DocumentSectionCode>08B</DocumentSectionCode>    \n    <TagID>G007</TagID>    \n    </Pointer>    \n    </Status>    \n    </Response>    \n    </DocumentMetaData>");

            MTNSignon(TestContext, userName);
        }

        [TestMethod]
        public void CustomsTSWForVehiclesWithMultiplePackagingElements()
        {
            MTNInitialize();

            //Step 3 - 7 - Check for the Stops on the cargo items

            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry");
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT2");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"Motor Vehicle", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG51206A");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Ship", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG51206A01");

            cargoEnquiryForm.SetFocusToForm();
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4093");

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus,
                @"Stops (dbl click)", @"Customs Doc 51202, STOP_51202");

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG51206A02");

            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4093");

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus,
                @"Stops (dbl click)", @"Customs Doc 51202, STOP_51202");

            //Step 8 - 11 - Assign cargo items to BOL (Drag and Drop)

            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage BOL Maintenance", forceReset: true);
            _voyageBOLMaintenanceForm = new VoyageBOLMaintenanceForm();
            _voyageBOLMaintenanceForm.GetSearcher();
            //_voyageBOLMaintenanceForm.SetValue(_voyageBOLMaintenanceForm.cmbVoyage, @"VOY_51206");
            _voyageBOLMaintenanceForm.cmbVoyage.SetValue(Voyage.VOY_51206, doDownArrow: true);
            //_voyageBOLMaintenanceForm.btnFind.DoClick();
            _voyageBOLMaintenanceForm.DoFind();

            _voyageBOLMaintenanceForm.CreateBOLMaintenanceTables();
            _voyageBOLMaintenanceForm.CargoItems();

            
            MTNControlBase.FindClickRowInTable(_voyageBOLMaintenanceForm.tblBOLMaintUnassignedCargo,
                @"ID^" + CargoId[0], ClickType.Click);
            var startPoint = Mouse.Position;
        
            var endPoint = new Point(_voyageBOLMaintenanceForm.tblCargoItems.BoundingRectangle.X
                + (_voyageBOLMaintenanceForm.tblCargoItems.BoundingRectangle.Width / 2),
                _voyageBOLMaintenanceForm.tblCargoItems.BoundingRectangle.Y
                + (_voyageBOLMaintenanceForm.tblCargoItems.BoundingRectangle.Height / 2));

            Mouse.Click(endPoint);

            Mouse.Drag(startPoint, endPoint);
            // Loading the Cargo Onship EDI file with BOL ID in automatically attach the cargo to BOL
            // so changing this test to cover both scenario - cargo is assigned automatically to BOL
            // and when it doesn't have BOL ID in EDI file, it remains Unassigned cargo on ship
            /*
            MTNControlBase.FindClickRowInTable(_voyageBOLMaintenanceForm.tblBOLMaintUnassignedCargo,
              @"ID^" + CargoId[1], ClickType.Click);
            var _startPoint = Mouse.Position;

            var _endPoint = new Point(_voyageBOLMaintenanceForm.tblCargoItems.BoundingRectangle.X
                + (_voyageBOLMaintenanceForm.tblCargoItems.BoundingRectangle.Width / 2),
                _voyageBOLMaintenanceForm.tblCargoItems.BoundingRectangle.Y
                + (_voyageBOLMaintenanceForm.tblCargoItems.BoundingRectangle.Height / 2));
            Mouse.Click(_endPoint);

            Mouse.Drag(_startPoint, _endPoint);
            */
            //Step 12 - 13
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            EDIOperationsForm ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT2");

            //Delete Existing EDI messages relating to this test.
            ediOperationsForm.DeleteEDIMessages(@"Custom Stops", @"51206");

            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Loaded", @"Verify");
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Verified", @"Load To DB");

            //Step 14 - Check for the Stops on the cargo items

            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG51206A01");

            
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
           // MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4093");

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus,
                @"Stops (dbl click)", @"");

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG51206A02");

            cargoEnquiryForm.SetFocusToForm();
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4093");

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus,
                @"Stops (dbl click)", @"");          

        }



        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;


            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>    \n    <JMTInternalCargoOnShip xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>    \n    <AllCargoOnShip>    \n    <operationsToPerform>Verify;Load To DB</operationsToPerform>    \n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>    \n    <CargoOnShip Terminal='TT2'>    \n    <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>    \n    <id>JLG51206A01</id>    \n    <voyageCode>VOY_51206</voyageCode>    \n    <operatorCode>COS</operatorCode>    \n    <dischargePort>NZAKL</dischargePort>    \n    <locationId>VOY_51206</locationId>    \n    <weight>5500.0000</weight>    \n    <imexStatus>Import</imexStatus>    \n    <weight>5000</weight>    \n    <commodity>MCAR</commodity>    \n    <messageMode>D</messageMode>    \n    </CargoOnShip>    \n    <CargoOnShip Terminal='TT2'>    \n    <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>    \n    <id>JLG51206A02</id>    \n    <voyageCode>VOY_51206</voyageCode>    \n    <operatorCode>COS</operatorCode>    \n    <dischargePort>NZAKL</dischargePort>    \n    <locationId>VOY_51206</locationId>    \n    <weight>5500.0000</weight>    \n    <imexStatus>Import</imexStatus>    \n    <weight>5000</weight>    \n    <commodity>MCAR</commodity>    \n    <messageMode>D</messageMode>    \n    </CargoOnShip>    \n    </AllCargoOnShip>    \n    </JMTInternalCargoOnShip>    \n        \n    ");
            
            //Create BOL
            CreateDataFileToLoad(@"BOL_Add.xml",
                "<?xml version='1.0' encoding='UTF-8'?>    \n    <JMTInternalBOL     \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalBOL.xsd'>    \n    <AllBOLHeader>    \n    <operationsToPerform>Verify;Load To DB</operationsToPerform>    \n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>    \n    <BOLHeader Terminal='TT2'>    \n    <id>BOL51206</id>    \n    <messageMode>A</messageMode>    \n    <operatorCode>COS</operatorCode>    \n    <voyageCode>VOY_51206</voyageCode>    \n    <dischargePort>NZAKL</dischargePort>    \n    <AllBOLDetails>    \n    <BOLDetails>    \n    <messageMode>A</messageMode>    \n    <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>    \n    <commodity>MCAR</commodity>    \n    </BOLDetails>    \n    </AllBOLDetails>    \n    </BOLHeader>    \n    </AllBOLHeader>    \n    </JMTInternalBOL>");

            // Create Cargo On Ship
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>    \n    <JMTInternalCargoOnShip xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>    \n    <AllCargoOnShip>    \n    <operationsToPerform>Verify;Load To DB</operationsToPerform>    \n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>    \n    <CargoOnShip Terminal='TT2'>    \n    <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>    \n    <id>JLG51206A01</id>    \n    <voyageCode>VOY_51206</voyageCode>    \n    <operatorCode>COS</operatorCode>    \n    <dischargePort>NZAKL</dischargePort>    \n    <locationId>VOY_51206</locationId>    \n    <weight>5500.0000</weight>    \n    <imexStatus>Import</imexStatus>    \n    <weight>5000</weight>    \n    <commodity>MCAR</commodity>    \n      \n    <messageMode>A</messageMode>    \n    </CargoOnShip>    \n    <CargoOnShip Terminal='TT2'>    \n    <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>    \n    <id>JLG51206A02</id>    \n    <voyageCode>VOY_51206</voyageCode>    \n    <operatorCode>COS</operatorCode>    \n    <dischargePort>NZAKL</dischargePort>    \n    <locationId>VOY_51206</locationId>    \n    <weight>5500.0000</weight>    \n    <imexStatus>Import</imexStatus>    \n    <weight>5000</weight>    \n    <commodity>MCAR</commodity>    \n    <bol>BOL51206</bol>    \n    <messageMode>A</messageMode>    \n    </CargoOnShip>    \n    </AllCargoOnShip>    \n    </JMTInternalCargoOnShip>    \n        \n    ");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }

}




