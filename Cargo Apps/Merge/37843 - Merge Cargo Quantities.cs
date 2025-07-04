using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Merge_Cargo;
using MTNGlobal.EnumsStructs;
using DataObjects.LogInOutBO;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Merge
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37843 : MobileAppsBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        void MTNInitialize()
        {
            // Setup data
            searchFor = @"_37843_";
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void MergeCargoQuantities()
        {
            MTNInitialize();

            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG37843A01");
            
            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoMerge();

            MA_CargoAppsMergePage mergeCargoPage = new MA_CargoAppsMergePage(TestContext);
            mergeCargoPage.SearchForSpecificCargo(@"JLG37843");

           /* string[] cargoToMerge = 
            {
                @"JLG37843"
            };
            mergeCargoPage.SelectCargoToMerge(cargoToMerge);*/
            mergeCargoPage.DoMerge();

            // Step 12 - 13
            searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG37843A01");

            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            string[,] fieldValueToValidate =
            {
                { MA_CargoAppsDetailsPage.constQuantity,  @"500" }
            };
            detailsPage.ValidateDetails(fieldValueToValidate);

            // Step 14
            searchPage.SearchForCargoID(@"JLG37843");

            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.ValidateCargoIDType(@"JLG37843A01", @"Bag of Sand, SMSAND - Small bag of Sand, SANC, x 500");
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <TestCases>37843</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG37843A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>454.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>300</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>454.000</tareWeight>\n            <weight>907.000</weight>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n         <CargoOnSite Terminal='TT1'>\n            <TestCases>37843</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG37843</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>454.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>907.000</tareWeight>\n            <weight>1361.000</weight>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <TestCases>37843</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG37843A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>454.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>300</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>454.000</tareWeight>\n            <weight>907.000</weight>\n		   <messageMode>A</messageMode>\n        </CargoOnSite>\n         <CargoOnSite Terminal='TT1'>\n            <TestCases>37843</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG37843</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>454.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>907.000</tareWeight>\n            <weight>1361.000</weight>\n		 <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
