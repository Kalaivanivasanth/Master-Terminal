using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Split_Cargo;
using System;
using DataObjects.LogInOutBO;
using MTNGlobal.Classes;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Cargo_Split
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase50795 : MobileAppsBase
    {

        private const string TestCaseNumber = @"50795";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
      

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
     
        void MTNInitialize()
        {
            searchFor = @"_" + TestCaseNumber + "_";
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();

            // Step 2 - Click Prenotes
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void SplitCargoMultiple()
        {
            MTNInitialize();
            
            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(CargoId);
            
            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoSplit();

            // Step 8 - 10
            MA_CASplitCargoPage splitCargoPage = new MA_CASplitCargoPage(TestContext);
           
            string[,] fieldsToSet =
            {
                { MA_CASplitCargoPage.constOptMultipleCargo, @"1" },
                { MA_CASplitCargoPage.constOperator, @"ABC - ABC CONTAINER LINE" },
                { MA_CASplitCargoPage.constNewQuantity, @"10"},
                { MA_CASplitCargoPage.constNewId, @"JLG50795B" },
                { MA_CASplitCargoPage.constFirstIDSuffix, @"01"}
            };
            splitCargoPage.SetFields(fieldsToSet);

            splitCargoPage.DoClear(false);
            splitCargoPage.ValidateFirstIDSuffix();
            
            fieldsToSet = new string[,]
            {
                { MA_CASplitCargoPage.constFirstIDSuffix, @"01"}
            };
            splitCargoPage.SetFields(fieldsToSet);
            
            splitCargoPage.DoSave();
          
            searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG50795B");

            string[] cargoToCheck =
            {
                @"JLG50795B01",
                @"JLG50795B02",
                @"JLG50795B03",
                @"JLG50795B04",
                @"JLG50795B05",
                @"JLG50795B06",
                @"JLG50795B07",
                @"JLG50795B08",
                @"JLG50795B09",
                @"JLG50795B10"
            };
            searchPage.ValidateCargoReturned(cargoToCheck, true);
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			  <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795B01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			  <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795B02</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			  <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795B03</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			  <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795B04</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			  <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795B05</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			  <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795B06</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			  <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795B07</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			  <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795B08</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			  <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795B09</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			  <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795B10</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			 \n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <TestCases>50795</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG50795A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
