using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using MTNWebPages.PageObjects.Popups;
using System;
using MTNGlobal.EnumsStructs;
using DataObjects.LogInOutBO;
using MTNGlobal.Classes;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37242 : MobileAppsBase
    {
        

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
      

        
        
        void MTNInitialize()
        {
            searchFor = "_37242_";
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void ChangeWeightGetOverWeightError()
        {

            MTNInitialize();
            
            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG37242A01");
            
            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoMaintenance();
            
            // Step 8
            MA_CargoMaintenancePage maintenancePage = new MA_CargoMaintenancePage(TestContext);
            maintenancePage.SetFields(new [,] { { MA_CargoMaintenancePage.constTotalWeight,  @"151^MT" } });
     
            // Step 10
            maintenancePage.DoSave();
    
            // Step 11
            Popup_OK_Cancel popupOKCancel = new Popup_OK_Cancel(TestContext);

            string[] messagesToCheck = 
            {
                @"The Container Id (JLG37242A01) failed the validity checks and may be incorrect.",
                @"The weight for JLG37242A01 is greater than max lift weight of 150000. (330693.663lbs)"
            };

            popupOKCancel.CheckMessage(Popup_OK.constFromPageCargoMaintenance, @"Warning(s)", messagesToCheck);
            popupOKCancel.DoCancel();

            // Step 12
            maintenancePage.SetFields(new [,] { { MA_CargoMaintenancePage.constTotalWeight,  @"150^MT" } });
            
            // Step 9
            maintenancePage.DoSave();

            popupOKCancel = new Popup_OK_Cancel(TestContext);

            messagesToCheck = new string[]
            {
                @"The Container Id (JLG37242A01) failed the validity checks and may be incorrect."
            };
            popupOKCancel.CheckMessage(Popup_OK.constFromPageCargoMaintenance, @"Warning(s)", messagesToCheck);
            popupOKCancel.DoOK();

            // Step 10
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();

           detailsPage.ValidateDetails(new [,] { { MA_CargoAppsDetailsPage.constWeight,  @"330693.663 lbs" } });
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37242</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37242A01</id>\n      <isoType>2230</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>2000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>ICEC</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <temperature>-4</temperature>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37242</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37242A01</id>\n      <isoType>2230</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>2000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>ICEC</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <temperature>-4</temperature>\n      <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
