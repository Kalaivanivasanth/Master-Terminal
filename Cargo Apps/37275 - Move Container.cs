using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNGlobal.EnumsStructs;
using MTNWebPages.PageObjects.Popups;
using OpenQA.Selenium.DevTools.V121.Storage;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps
{
    /// --------------------------------------------------------------------------------------------------
    /// Date         Person          Reason for change
    /// ==========   ======          =================================
    /// 17/03/2022   navmh5          Changes for React framework change (50698)
    /// --------------------------------------------------------------------------------------------------
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37275 : MobileAppsBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize()  {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        void MTNInitialize()
        {
            searchFor = @"_37275_";
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }


    [TestMethod]
        public void MoveContainer()
        {
            MTNInitialize();
            
            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG37275A01");
            
            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoMoveReact();

            // Step 8A - Additional step to check what happens on error
            MA_CargoAppsMoveCargoPage movePage = new MA_CargoAppsMoveCargoPage(TestContext);
            movePage.SetLocationMoveToNextStep(@"ZZZ999");

            Popup_OK popupOk = new Popup_OK(TestContext);
            string[] messagesToCheck =
            {
                "Code: 83378 Location ZZZ999 can not be found."
            };
            popupOk.CheckMessage(null, @"Error(s)", messagesToCheck);
            popupOk.DoOK();
            
            movePage = new MA_CargoAppsMoveCargoPage(TestContext);
            movePage.DoBack();
            
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoMoveReact();
            
            movePage = new MA_CargoAppsMoveCargoPage(TestContext);
            movePage.SetLocationMoveToNextStep(TT1.TerminalArea.MKBS01);

            MA_CargoAppsMoveCargoCompletePage completePage = new MA_CargoAppsMoveCargoCompletePage(TestContext);
            completePage.DoComplete();

            // Step 9
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);

            /*string[,] fieldValueToValidate = {
               { MA_CargoAppsDetailsPage.constLocation,  TT1.TerminalArea.MKBS01 }
            };*/
            detailsPage.ValidateDetails(new [,] { { MA_CargoAppsDetailsPage.constLocation,  TT1.TerminalArea.MKBS01 } });
            
            // Test Complete button and already set to move location
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoMoveReact();
            
            movePage = new MA_CargoAppsMoveCargoPage(TestContext);
            movePage.SetLocationMoveToNextStep(TT1.TerminalArea.MKBS01);

            completePage = new MA_CargoAppsMoveCargoCompletePage(TestContext);
            completePage.DoComplete();
            
            popupOk = new Popup_OK(TestContext);
            messagesToCheck = new []
            {
                "Code: 70124 The specified Cargo (JLG37275A01) is already in this position (MKBS01)."
            };
            popupOk.CheckMessage(null, @"Error(s)", messagesToCheck);
            popupOk.DoOK();
           
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37275</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37275A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	   <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37275</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37275A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKGB01 000501</locationId>\n      <weight>5000</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	   <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
