using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.Yard_Functions.Cargo_Review;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase59987 : MTNBase
    {
        private const string CargoId = "59987-CUSTOMS1";
        private const string RemarksValue = "TEST59987";
        private const string DocumentType1 = "DOC1\tDOC1";
        private const string DocumentType2 = "DOC2\tDOC2";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }

       
        
        [TestMethod]
        public void CustomsStatusOfCargoInCargoEnquiry()
        {
            MTNInitialize();

            // Initial search and status check
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Metal, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            VerifyCustomsStatus("Free");

            // Create first custom document and verify status
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Cargo Review", forceReset: true);
            CargoReviewForm.DoSelectorQuery(new[]
            {
                new SelectorQueryArguments(CargoReviewForm.Property.Site, CargoReviewForm.Operation.EqualTo, Site.OnSite),
                new SelectorQueryArguments(CargoReviewForm.Property.Id, CargoReviewForm.Operation.EqualTo, "59987-CUSTOMS1"),
                new SelectorQueryArguments(CargoReviewForm.Property.Mark, CargoReviewForm.Operation.StartsWith, "59987"),
            });
            CreateCustomDocument(RemarksValue, DocumentType1);
            VerifyCustomsStatus("Customs");

            // Create second custom document and verify status
            CreateCustomDocument(RemarksValue, DocumentType2);
            VerifyCustomsStatus("Cleared");
        }

        /// <summary>
        /// To search a cargo and check its customs status in cargo enquiry screen
        /// </summary>
        /// <param name="expectedStatus">status might be free, customs, cleared</param>
        private void VerifyCustomsStatus(string expectedStatus)
        {
            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.tblData2.FindClickRow(new[]
                { $"ID^59987-CUSTOMS1~Location ID^BULK~Total Quantity^1~Customs Status^{expectedStatus}" });
        }
       
        /// <summary>
        /// To create custom document from cargo review screen
        /// </summary>
        /// <param name="remarksValue">Remarks Value to enter</param>
        /// <param name="documentType">Document Type of the Custom Document</param>
        private void CreateCustomDocument(string remarksValue, string documentType)
        {
            //Console.WriteLine("remarksValue: " + remarksValue);
            //Console.WriteLine("documentType: " + documentType);
            CargoReviewForm.DoSearch();
            CargoReviewForm.OpenRequiredContextMenuForCreate(CargoReviewForm.QueryResultsContextMenu.CreateCustomDocument);
            var customDocumentForm = new CustomDocumentMaintenanceForm("Custom Document Maint TT1");
            customDocumentForm.DoCustomDocumentProcess(remarksValue, documentType);
        }
        
        
        private void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = "_59987_";

            // Delete Cargo OnShip
            CreateDataFileToLoad("DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>METAL</cargoTypeDescr>\n            <id>59987-CUSTOMS1</id>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>NZCHC</dischargePort>\n            <locationId>BULK</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>EXPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n                 <mark>59987</mark>\n                <countryOfOriginCode>US</countryOfOriginCode>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnShip
            CreateDataFileToLoad("CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>METAL</cargoTypeDescr>\n            <id>59987-CUSTOMS1</id>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>NZCHC</dischargePort>\n            <locationId>BULK</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>EXPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n                 <mark>59987</mark>\n                <countryOfOriginCode>US</countryOfOriginCode>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }
}
