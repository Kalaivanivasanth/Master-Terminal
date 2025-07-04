using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Pack_Container;
using MTNWebPages.PageObjects.Popups;
using DataObjects.LogInOutBO;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Pack_Container
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37396 : MobileAppsBase
    {
        
        Popup_OK_Cancel _popupOKCancel;

        const string TestCaseNumber = "37396";
        
        string CargoId = $"JLG{TestCaseNumber}A01";
        string CargoId2 = $"JLG{TestCaseNumber}A02";
        string CargoId3 = $"JLG{TestCaseNumber}A03";
        
        
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
        public void Pack2Pipes11BottlesBeerUnpack11BottleBeer()
        {
            MTNInitialize();

            // Step 4
            HomePage.ClickTile(HomePage.BtnPackReact);

            // Step 5
            MA_PackContainerSearchReactPage searchPage = new MA_PackContainerSearchReactPage(TestContext);
            searchPage.SearchForCargoId(CargoId);

            MA_PackContainerReactPage packContainerPage = new MA_PackContainerReactPage(TestContext);
            packContainerPage.ValidateHaveCorrectCargoId(CargoId);

            // Step 6
            searchPage.DoNext();

            _popupOKCancel = new Popup_OK_Cancel(TestContext);
            _popupOKCancel.CheckMessage(null, Popup_OK.ConstWarnings,
                new[] { Popup_OK.FailedValidityCheck.Replace("<cargoId>", CargoId) });
            _popupOKCancel.DoOK();
      
            // Step 7
            MA_PackContainerDetailsReactPage packContainerDetailsPage = new MA_PackContainerDetailsReactPage(TestContext);
            packContainerDetailsPage.FindCargo(CargoId3);
           
            // Step 8
            MA_PackContainerItemReactPage packContainerItemPage = new MA_PackContainerItemReactPage(TestContext);
            packContainerItemPage.ValidateCorrectItemToPack($"{CargoId3} STEEL, PIPE - Pipes, x 2");
            packContainerItemPage.SetQuantityToPack("2");
            
            packContainerItemPage.DoPack();

            // Step 10
            packContainerDetailsPage = new MA_PackContainerDetailsReactPage(TestContext);
            packContainerDetailsPage.FindCargo(CargoId2);

            // Step 11
            packContainerItemPage = new MA_PackContainerItemReactPage(TestContext);
            
            packContainerItemPage.ValidateCorrectItemToPack($"{CargoId2} Bottles of Beer, x 11");
            packContainerItemPage.SetQuantityToPack("11");  
            packContainerItemPage.DoPack();

            // Step 12
            packContainerDetailsPage = new MA_PackContainerDetailsReactPage(TestContext);
            packContainerDetailsPage.DoClickTab(MA_PackContainerDetailsReactPage.constPacked);

            packContainerDetailsPage.ValidateCountSelectedTab("13");
            packContainerDetailsPage.ValidatePackedDetails(new[]
                { $"{CargoId2} Bottles of Beer x 11", $"{CargoId3} STEEL x 2" });

            // Step 13
            packContainerDetailsPage.DoUnpack(CargoId2);
           
            MA_PackContainerUnpackReactPage unpackPage = new MA_PackContainerUnpackReactPage(TestContext);
            unpackPage.UnpackDetails("GCARA2 BEER", @"11");
            unpackPage.DoUnpack();

            // Step 15
            packContainerDetailsPage = new MA_PackContainerDetailsReactPage(TestContext);
            packContainerDetailsPage.DoClickTab(MA_PackContainerDetailsReactPage.constPacked);

            packContainerDetailsPage.ValidateCountSelectedTab("2");
            packContainerDetailsPage.ValidatePackedDetails(new [] { $"{CargoId3} STEEL x 2" });

            string returnedDetails =
                packContainerDetailsPage.ValidatePackedDetails(new[] { $"{CargoId2} Bottles of Beer x 11" }, false);
            Assert.IsTrue(returnedDetails != null, 
                $"TestCase37396 - The following details should not exist in the Packed table: {returnedDetails}");

            // Step 15 - 22 Rather than go to Master Terminal will do this by using Cargo Apps
            packContainerDetailsPage.ReturnToTilesPage();

            HomePage = new MA_TilesPage(TestContext);
            HomePage.ClickTile(HomePage.BtnCargoApps);

            MA_CargoAppsSearchPage cargoAppSearchPage = new MA_CargoAppsSearchPage(TestContext);
            cargoAppSearchPage.SearchForCargoID(CargoId2);

            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.ValidateCargoIDType(CargoId2, "Bottles of Beer, x 11");
            detailsPage.ValidateDetails(new [,] { { MA_CargoAppsDetailsPage.constLocation,  "GCARA2 BEER" } });

        }
        

        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n   \n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37396</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37396A01</id>\n      <isoType>220A</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>6000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37396</TestCases>\n      <cargoTypeDescr>Bottles of Beer</cargoTypeDescr>\n      <id>JLG37396A02</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>GCARA2 BEER</locationId>\n      <weight>8000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <totalQuantity>11</totalQuantity>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37396</TestCases>\n      <cargoTypeDescr>STEEL</cargoTypeDescr>\n      <product>PIPES</product>\n      <id>JLG37396A03</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>GCSTE1 PIPES </locationId>\n      <weight>8000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <totalQuantity>2</totalQuantity>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n   \n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37396</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37396A01</id>\n      <isoType>220A</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>6000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>A</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37396</TestCases>\n      <cargoTypeDescr>Bottles of Beer</cargoTypeDescr>\n      <id>JLG37396A02</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>GCARA1 BEER</locationId>\n      <weight>8000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <totalQuantity>11</totalQuantity>\n      <messageMode>A</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37396</TestCases>\n      <cargoTypeDescr>STEEL</cargoTypeDescr>\n      <product>PIPES</product>\n      <id>JLG37396A03</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>GCSTE1 PIPES </locationId>\n      <weight>8000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <totalQuantity>2</totalQuantity>\n      <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
