using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Split_Cargo;
using System;
using DataObjects.LogInOutBO;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Cargo_Split
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37836 : MobileAppsBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

       [TestCleanup]
        public void TestCleanup()
        {
            base.TestCleanup();
            MTNBase.MTNTestCleanup(TestContext);
            MTNBase.MTNCleanup();
        }

        private void MTNInitialize()
        {
            // Setup data
            searchFor = @"_37836_";
            SetupAndLoadInitializeData(TestContext);

            // Log in
            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void SplitCargo()
        {
            MTNInitialize();

            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG37836A01");
            
            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoSplit();

            // Step 8 - 10
            MA_CASplitCargoPage splitCargoPage = new MA_CASplitCargoPage(TestContext);
            splitCargoPage.CreateOneCargoItem();
           
            string[,] fieldsToSet =
            {
                { MA_CASplitCargoPage.constOperator, @"ABC - ABC CONTAINER LINE" },
                { MA_CASplitCargoPage.constNewQuantity, @"50" },
                { MA_CASplitCargoPage.constNewId, @"JLG37836TEST" }
            };
            splitCargoPage.SetFields(fieldsToSet);

            splitCargoPage.DoClear();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            splitCargoPage.DoBack();

            // Additional test section to check that the X in the New ID field is working 
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoSplit();

            splitCargoPage = new MA_CASplitCargoPage(TestContext);
            splitCargoPage.CreateOneCargoItem();
            splitCargoPage.SetFields(fieldsToSet);

            // Additional test section to test whether +/- work
            splitCargoPage.DoDecrement();
            splitCargoPage.ValidateQuantity("49");
            splitCargoPage.DoIncrement();
            splitCargoPage.ValidateQuantity("50");

            splitCargoPage = new MA_CASplitCargoPage(TestContext);
            splitCargoPage.DoSave();

            // Step 11 - 13
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            string[,] fieldValueToValidate = 
            {
                { MA_CargoAppsDetailsPage.constQuantity,  @"150" }
            };
            detailsPage.ValidateDetails(fieldValueToValidate);
            detailsPage.DoBack();

            // Step 14
            searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG37836TEST");

            // Step 15 - 16
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            fieldValueToValidate = new string[,] 
            {
                { MA_CargoAppsDetailsPage.constQuantity,  @"50" },
                { MA_CargoAppsDetailsPage.constOperator,  @"ABC" }
            };
            detailsPage.ValidateDetails(fieldValueToValidate);
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		\n        <CargoOnSite Terminal='TT1'>\n            <TestCases>37836</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG37836A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			   <CargoOnSite Terminal='TT1'>\n            <TestCases>37836</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG37836TEST</id>\n            <operatorCode>ABC</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>1750</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>50</totalQuantity>\n            <commodity>SANC</commodity>\n			<messageMode>D</messageMode>\n				  </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <TestCases>37836</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG37836A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>907.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>200</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>2268</tareWeight>\n            <weight>3175.00</weight>\n			<messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads
    }
}
