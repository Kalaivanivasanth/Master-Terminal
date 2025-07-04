using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using System;
using DataObjects.LogInOutBO;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Stops
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37735 : MobileAppsBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {

            // Setup data
            searchFor = @"_37735_";
            BaseClassInitialize_New(testContext);
            /*loadFileDeleteStartTime = DateTime.Now; // REMOVE AT YOUR OWN PERIL
            SetupAndLoadInitializeData(testContext);

            var url = testContext.GetRunSettingValue(@"URL_MobileApps");
            ClassInitialize(testContext, url);*/
        }

        [ClassCleanup]
        public new static void ClassCleanUp()
        {
            WebBase.ClassCleanUp();
        }

        [TestInitialize]
        public new void TestInitialize()
        {

            /*
            // Step 1 - Log in
            Login();

            base.TestInitialize();

            // Step 2 - Click Prenotes
            HomePage = new MA_TilesPage(TestContext);*/
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void ClearedStops()
        {

            MTNInitialize();
            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG37735A01");

            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();

            // Step 8
            string[,] fieldValueToValidate =
            {
               { MA_CargoAppsDetailsPage.constStops,  @"Customs Export (CEDO)" }
            };  
            detailsPage.ValidateDetails(fieldValueToValidate);

            // Step 9
            detailsPage.DoStops();

            // Step 10
            MA_CargoAppsStopsPage stopsPage = new MA_CargoAppsStopsPage(TestContext);
            stopsPage.SetUnsetStops(new [] { "Customs Export (CEDO)^TESTING" });

            // Step 11
            stopsPage.DoSave();
            
            // Step 12
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();

            // Step 13
             fieldValueToValidate = new string[,]
            {
               { MA_CargoAppsDetailsPage.constStops,  @"Customs Export (CEDO)" }
            };
            string detailsNotFound = detailsPage.ValidateDetails(fieldValueToValidate, false);
            Assert.IsTrue(detailsNotFound != null, @"TestCase37735 - The stop (Customs Export (CEDO)) has not been cleared.");
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG37735A01</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>2000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>GEN</commodity>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000002</voyageCode>\n         <isoType>2200</isoType>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG37735A01</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>2000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>GEN</commodity>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000002</voyageCode>\n         <isoType>2200</isoType>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
