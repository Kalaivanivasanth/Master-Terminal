using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using System;
using MTNGlobal.EnumsStructs;
using DataObjects.LogInOutBO;
using MTNGlobal.Classes;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase40829 : MobileAppsBase
    {
        private const string TestCaseNumber = @"40829";
        private readonly string[] CargoId =
        {
            @"JLG" + TestCaseNumber + @"A01",
            @"JLG" + TestCaseNumber + @"A02"
        };

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        void MTNInitialize()
        {
            searchFor = $"_{TestCaseNumber}_";
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void CargoSearchBasic()
        {
            
            MTNInitialize();

            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(CargoId[0]);
           
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.ValidateCargoIDType($"{CargoId[0]}", @"ISO Container, 2200 - GENERAL, GEN");

            // Step 7
            detailsPage.DoClearSearch();

            // Step 8
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.SearchForCargo(CargoId[1]);

            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.ValidateCargoIDType($"{CargoId[1]}", @"ISO Container, 2200 - GENERAL, GEN");
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>    <AllCargoOnSite>       <operationsToPerform>Verify;Load To DB</operationsToPerform>       <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>       <CargoOnSite Terminal='TT1'>          <cargoTypeDescr>ISO Container</cargoTypeDescr>          <id>JLG40829A01</id>          <operatorCode>MSC</operatorCode>          <weight>2000</weight>          <imexStatus>Export</imexStatus>          <commodity>GEN</commodity>          <dischargePort>NZAKL</dischargePort>          <voyageCode>MSCK000002</voyageCode>          <isoType>2200</isoType> 		 <locationId>MKBS01</locationId>          <messageMode>D</messageMode>       </CargoOnSite> 	        <CargoOnSite Terminal='TT1'>          <cargoTypeDescr>ISO Container</cargoTypeDescr>            <id>JLG40829A02</id>          <operatorCode>MSC</operatorCode>          <weight>2000</weight>          <imexStatus>Export</imexStatus>          <commodity>GEN</commodity>          <dischargePort>NZAKL</dischargePort>          <voyageCode>MSCK000002</voyageCode>          <isoType>2200</isoType> 		 <locationId>MKBS01</locationId>          <messageMode>D</messageMode> 		       </CargoOnSite>    </AllCargoOnSite> </JMTInternalCargoOnSite>  ");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>    <AllCargoOnSite>       <operationsToPerform>Verify;Load To DB</operationsToPerform>       <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>       <CargoOnSite Terminal='TT1'>          <cargoTypeDescr>ISO Container</cargoTypeDescr>          <id>JLG40829A01</id>          <operatorCode>MSC</operatorCode>          <weight>2000</weight>          <imexStatus>Export</imexStatus>          <commodity>GEN</commodity>          <dischargePort>NZAKL</dischargePort>          <voyageCode>MSCK000002</voyageCode>          <isoType>2200</isoType> 		 <locationId>MKBS01</locationId>          <messageMode>A</messageMode>       </CargoOnSite> 	        <CargoOnSite Terminal='TT1'>          <cargoTypeDescr>ISO Container</cargoTypeDescr>            <id>JLG40829A02</id>          <operatorCode>MSC</operatorCode>          <weight>2000</weight>          <imexStatus>Export</imexStatus>          <commodity>GEN</commodity>          <dischargePort>NZAKL</dischargePort>          <voyageCode>MSCK000002</voyageCode>          <isoType>2200</isoType> 		 <locationId>MKBS01</locationId>          <messageMode>A</messageMode> 		       </CargoOnSite>    </AllCargoOnSite> </JMTInternalCargoOnSite>  ");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
