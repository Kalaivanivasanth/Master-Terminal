using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.Yard_Functions.Cargo_Review;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Cargo_Review
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase66327 : MTNBase
    {
        private const string LicenseType = "CL1\tCustom License 1";
        private const string DocumentType = "L1D\tLicense 1 Document";
        private const string ErrorMessage = "Code :96318. HS code is a mandatory field.";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>("MTNCOO");
        }


        [TestMethod]
        public void CustomDocumentDefaults()
        {
            MTNInitialize();

            FormObjectBase.MainForm.OpenCargoReviewFromToolbar();
            CargoReviewForm.DoSelectorQuery(new[]
            {
                new SelectorQueryArguments(CargoReviewForm.Property.Site, CargoReviewForm.Operation.EqualTo, Site.OnSite),
                new SelectorQueryArguments(CargoReviewForm.Property.Mark, CargoReviewForm.Operation.StartsWith, "66327"),
            });
            CargoReviewForm.DoSearch();


            CargoReviewForm.OpenRequiredContextMenuForCreate(CargoReviewForm.QueryResultsContextMenu.CreateCustomDocument);
            var validateLicense = new CustomDocumentMaintenanceForm("Custom Document Maint TT1");
            validateLicense.ValidateDefaultValues(DocumentType, LicenseType, ErrorMessage);
            
        }


        void SetupAndLoadInitializeData(TestContext testContext)
        {

            fileOrder = 1;
            searchFor = "_66327_";

            // Delete Cargo OnShip
            CreateDataFileToLoad("DeleteCargoOnSite.xml",
      "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>METAL</cargoTypeDescr>\n            <id>66327-CUSTOMS1</id>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>BULK</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>IMPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n                 <mark>66327</mark>\n                <countryOfOriginCode>US</countryOfOriginCode>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnShip
            CreateDataFileToLoad("CreateCargoOnSite.xml",
      "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>METAL</cargoTypeDescr>\n            <id>66327-CUSTOMS1</id>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>BULK</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>IMPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n                 <mark>66327</mark>\n                <countryOfOriginCode>US</countryOfOriginCode>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
