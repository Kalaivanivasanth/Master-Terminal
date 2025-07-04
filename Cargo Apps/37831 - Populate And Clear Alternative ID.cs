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
    public class TestCase37831 : MobileAppsBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [ClassCleanup]
        public new static void ClassCleanUp() => WebBase.ClassCleanUp();

        [TestInitialize]
        public new void TestInitialize() {}

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }
        

        [TestMethod]
        public void AlternativeID()
        {

            MTNInitialize();
            
            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID("JLG37831A01");
            
            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoAltID();
            
            // Step 8
            MA_CargoAppsAltIdPage altIDPage = new MA_CargoAppsAltIdPage(TestContext);
            altIDPage.SetAlternativeID("TESTING");

            // additional testing
            altIDPage.DoClearText();

            altIDPage = new MA_CargoAppsAltIdPage(TestContext);
            altIDPage.ValidateAlternativeId();

            altIDPage = new MA_CargoAppsAltIdPage(TestContext);
            altIDPage.SetAlternativeID("TESTING");

            // Step 9
            altIDPage.DoSave();

            WarningErrorPopup warningErrorPopup = new WarningErrorPopup(TestContext);
            warningErrorPopup.DoOK();

            // Step 10
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();

            // Step 11
            detailsPage.ValidateDetails(new [,] { { MA_CargoAppsDetailsPage.constAlternativeID,  "TESTING" } });

            // Step 12
            detailsPage.DoAltID();

            // Step 13
            altIDPage = new MA_CargoAppsAltIdPage(TestContext);
            altIDPage.DoClearText();

            // Step 14
            altIDPage.DoSave();

            warningErrorPopup = new WarningErrorPopup(TestContext);
            warningErrorPopup.DoOK();

            // Step 15
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();

            string[] checkExpanderFor =
            {
                "Alternative ID"
            };
            string detailsNotFound = detailsPage.ValidateExistsInExpander(checkExpanderFor, false);
            Assert.IsTrue(detailsNotFound.Contains("Alternative ID"), "TestCase37831 - Alternative ID still exists.");
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_37813_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37831</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37831A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSK</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1361.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <availabilityGrade>0</availabilityGrade>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37831</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37831A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSK</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1361.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <availabilityGrade>0</availabilityGrade>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
