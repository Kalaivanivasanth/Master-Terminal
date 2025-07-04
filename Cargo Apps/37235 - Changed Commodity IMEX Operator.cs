using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using MTNWebPages.PageObjects.Popups;
using System;
using DataObjects.LogInOutBO;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37235 : MobileAppsBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {

            // Setup data
            searchFor = @"_37235_";
            BaseClassInitialize_New(testContext);
          
        }

        [ClassCleanup]
        public new static void ClassCleanUp()
        {
            WebBase.ClassCleanUp();
        }

        [TestInitialize]
        public new void TestInitialize() {}
       
        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        [TestMethod]
        public void ChangeCommodityIMEXOperator()
        {
            MTNInitialize();
            
            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG37235A01");
            
            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoMaintenance();
            
            // Step 8
            MA_CargoMaintenancePage maintenancePage = new MA_CargoMaintenancePage(TestContext);

            string[,] fieldValueToSet = {
                { MA_CargoMaintenancePage.constCommodity,  @"IRON - Iron" },
                { MA_CargoMaintenancePage.constOperator,  @"COS HAKONE" },
                { MA_CargoMaintenancePage.constIMEX,  @"Import" }
            };
            maintenancePage.SetFields(fieldValueToSet);
            
            // Step 9
            maintenancePage.DoSave();

            Popup_OK_Cancel popupOKCancel = new Popup_OK_Cancel(TestContext);

            string[] messagesToCheck =
            {
                @"The Container Id (JLG37235A01) failed the validity checks and may be incorrect."
            };
            popupOKCancel.CheckMessage(Popup_OK.constFromPageCargoMaintenance, @"Warning(s)", messagesToCheck);
            popupOKCancel.DoOK();

            // Step 10
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();

            string[,] fieldValueToValidate = {
                { MA_CargoAppsDetailsPage.constCommodity,  @"Iron" },
                { MA_CargoAppsDetailsPage.constOperator,  @"COS" },
                { MA_CargoAppsDetailsPage.constIMEXStatus,  @"Import" }
            };
            detailsPage.ValidateDetails(fieldValueToValidate);
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37235</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37235A01</id>\n      <isoType>2200</isoType>\n      <locationId>MKBS01</locationId>\n      <operatorCode>COS</operatorCode>\n      <weight>2000.0000</weight>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37235</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37235A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>2000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
