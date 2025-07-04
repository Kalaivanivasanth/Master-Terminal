using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNBaseClasses.BaseClasses.Web;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps
{
    [TestClass, TestCategory(TestCategories.MTNWeb)]
    public class TestCase38177 : MobileAppsBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
      
        [ClassCleanup]
        public new static void ClassCleanUp() => WebBase.ClassCleanUp();

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
            MTNBase.MTNTestCleanup(TestContext);
            MTNBase.MTNCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void RaiseAlert()
        {
            MTNInitialize();

            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG38177A01");
            
            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoRaiseAlert();

            // Step 8
            MA_CargoAppsRaiseAlertPage raiseAlertPage = new MA_CargoAppsRaiseAlertPage(TestContext);
            raiseAlertPage.SetAlertType(@"Door AJAR");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            raiseAlertPage = new MA_CargoAppsRaiseAlertPage(TestContext);
            raiseAlertPage.SetAlertComment(@"Someone has left the door ajar and there is a really funk smell coming from the container");
            raiseAlertPage.DoBack();

            // Step 9
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoRaiseAlert();
            
            raiseAlertPage.SetAlertType(@"Door AJAR");

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            raiseAlertPage = new MA_CargoAppsRaiseAlertPage(TestContext);
            raiseAlertPage.SetAlertComment(@"Someone has left the door ajar and there is a really funk smell coming from the container");
            raiseAlertPage.DoSend();


            // Step 11 - 14
            //SignonMTN(TestContext);
            LogInto<MTNLogInOutBO>();

            // Step 31 - 37
            FormObjectBase.NavigationMenuSelection(@"General Functions | System Alerts");
            SystemAlertsForm systemAlertsForm = new SystemAlertsForm();
            // MTNControlBase.FindClickRowInTable(systemAlertsForm.tblDetails,
                // @"Detail^Sent from Mobile App, USERDWAT: JLG38177A01 : Door AJAR", 
                // ClickType.DoubleClick, SearchType.Contains);
            systemAlertsForm.TblDetails1.FindClickRow(["Detail^Sent from Mobile App, USERDWAT: JLG38177A01 : Door AJAR"], ClickType.DoubleClick, SearchType.Contains);

            SystemAlertsDetailsForm systemAlertsDetailsForm = new SystemAlertsDetailsForm();

            string[] alertsDetailsToValidate =
            {
                @"Sent from Mobile App, USERDWAT: JLG38177A01 : Door AJAR
Someone has left the door ajar and there is a really funk smell coming from the container"
            };
            systemAlertsDetailsForm.ValidateAlertDetails(alertsDetailsToValidate);

        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_38177_";
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>38177</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG38177A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>38177</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG38177A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
