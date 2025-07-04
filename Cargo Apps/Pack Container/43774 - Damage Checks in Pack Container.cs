using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Pack_Container;
using MTNWebPages.PageObjects.Popups;
using System;
using DataObjects.LogInOutBO;
using MTNGlobal.Classes;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Pack_Container
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase43774 : MobileAppsBase
    {

        const string TestCaseNumber = "43774";
        const string CargoId = "JLG" + TestCaseNumber + "A01";
        const string CargoId2 = "JLG" + TestCaseNumber + "A02";

        Popup_OK_Cancel _popupOKCancel;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        void MTNInitialize()
        {
            searchFor = @"_43774_";
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void DamageChecksInPackContainer()
        {
            MTNInitialize();

            // Step 5
            HomePage.ClickTile(HomePage.BtnPackReact);

            // Step 6
            MA_PackContainerSearchReactPage searchReactPage = new MA_PackContainerSearchReactPage(TestContext);
            searchReactPage.SearchForCargoId(CargoId);

            MA_PackContainerReactPage packContainerReactPage = new MA_PackContainerReactPage(TestContext);
            packContainerReactPage.ValidateHaveCorrectCargoId(CargoId);

            // Step 7
            searchReactPage.DoNext();
           
            _popupOKCancel = new Popup_OK_Cancel(TestContext);
            _popupOKCancel.CheckMessage(null, Popup_OK.ConstWarnings,
                new[] { Popup_OK.FailedValidityCheck.Replace("<cargoId>", CargoId) });
            _popupOKCancel.DoOK();
      
            // Step 8
            MA_PackContainerDetailsReactPage packContainerDetailsPage = new MA_PackContainerDetailsReactPage(TestContext);
            packContainerDetailsPage.FindCargo(CargoId2);

            // Step 9
            MA_PackContainerItemReactPage packContainerItemPage = new MA_PackContainerItemReactPage(TestContext);
            packContainerItemPage.ValidateCorrectItemToPack($"{CargoId2} Bottles of Beer, x 1");
            packContainerItemPage.DoPack();

            Popup_OK popupOK = new Popup_OK(TestContext);
            popupOK.CheckMessage(Popup_OK.ConstErrors,
                new[] { $"Cargo Check Damage for item {CargoId2} is required before continuing" });
            popupOK.DoOK();            

            // Step 10
            packContainerItemPage = new MA_PackContainerItemReactPage(TestContext);

            // Step 11
            packContainerItemPage.SetQuantityToPack("1");
            packContainerItemPage.DoChecks(new [,] { { MA_PackContainerItemReactPage.constDamage, "No" } } );

            // Step 12
            packContainerItemPage.DoPack();

            packContainerDetailsPage = new MA_PackContainerDetailsReactPage(TestContext);
            packContainerDetailsPage.DoClickTab(MA_PackContainerDetailsReactPage.constPacked);
            
            packContainerDetailsPage.ValidateCountSelectedTab("1");
            packContainerDetailsPage.ValidatePackedDetails(new [] { $"{CargoId2} Bottles of Beer x 1" });

        }

        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>43774</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG43774A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <commodity>GEN</commodity>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>43774</TestCases>\n      <cargoTypeDescr>Bottles of Beer</cargoTypeDescr>\n      <id>JLG43774A02</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS02</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>      \n	  <totalQuantity>1</totalQuantity>\n     	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>43774</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG43774A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>43774</TestCases>\n      <cargoTypeDescr>Bottles of Beer</cargoTypeDescr>\n      <id>JLG43774A02</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS02</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>      \n	  <totalQuantity>1</totalQuantity>\n     	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
