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
    public class TestCase37743 : MobileAppsBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            // Setup data
            searchFor = @"_37743_";
            /*loadFileDeleteStartTime = DateTime.Now; // REMOVE AT YOUR OWN PERIL
            SetupAndLoadInitializeData(testContext);

            var url = testContext.GetRunSettingValue(@"URL_MobileApps");
            ClassInitialize(testContext, url);*/
            BaseClassInitialize_New(testContext);
        }

        [ClassCleanup]
        public new static void ClassCleanUp()
        {
            WebBase.ClassCleanUp();
        }

        [TestInitialize]
        public new void TestInitialize()
        {

            /*// Step 1 - Log in
            Login();

            base.TestInitialize();

            // Step 2 - 
            HomePage = new MA_TilesPage(TestContext);*/
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();
            
            HomePage = new MA_TilesPage(TestContext);
        }
        

        [TestMethod]
        public void SetMultipleStopsIncludeCEDOAndClearStops()
        {

            MTNInitialize();
            
            // Step 2
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 3
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG37743A01");

            // Step 4
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoStops();

            // Step 5
            MA_CargoAppsStopsPage stopsPage = new MA_CargoAppsStopsPage(TestContext);

            string[] stopsToSetUnset =
            {
                @"STOP_39019",
                @"Customs Export (CEDO)",
                
            };
            stopsPage.SetUnsetStops(stopsToSetUnset);

            // Step 6
            stopsPage.DoSave();
            
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();
            
            detailsPage.ValidateDetails(new[,]
                { { MA_CargoAppsDetailsPage.constStops, @"STOP_39019, Customs Export (CEDO)" } });
            
            // Step 7
            detailsPage.DoStops();
            
            // Step 8
            stopsToSetUnset = new string[]
            {
                @"Customs Export (CEDO)^TESTING"
            };
            stopsPage.SetUnsetStops(stopsToSetUnset);

            // Step 9
            stopsPage.DoSave();

            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();
          
            detailsPage.ValidateDetails(new [,] { { MA_CargoAppsDetailsPage.constStops,  @"STOP_39019" } });
        }



        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Stops
            CreateDataFileToLoad(@"DeleteStops.xml",
                "<?xml version='1.0'?> <JMTInternalStop xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalStop.xsd'>\n	<AllCargoUpdateRequest>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<CargoUpdateRequest Terminal='TT1'>\n			<directDelivery/>\n			<clearanceExpiryDate/>\n			<updateType/>\n			<billOfLading/>\n			<cargoRemarks/>\n			<cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n			<deliveryReleaseNumber>37743</deliveryReleaseNumber>\n			<description/>\n			<fullOrMT/>\n			<gradeCode/>\n			<id>JLG37743A01</id>\n			<messageMode>A</messageMode>\n			<stopAction>D</stopAction>\n			<stopCode>5</stopCode>\n			<subTerminalCode/>\n			<toDoAction/>\n			<toDoRemarks/>\n			<toDoTaskShort/>\n			<voyageCode/>\n		</CargoUpdateRequest>\n		<CargoUpdateRequest Terminal='TT1'>\n			<directDelivery/>\n			<clearanceExpiryDate/>\n			<updateType/>\n			<billOfLading/>\n			<cargoRemarks/>\n			<cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n			<deliveryReleaseNumber></deliveryReleaseNumber>\n			<description/>\n			<fullOrMT/>\n			<gradeCode/>\n			<id>JLG37743A01</id>\n			<messageMode>A</messageMode>\n			<stopAction>D</stopAction>\n			<stopCode>1</stopCode>\n			<subTerminalCode/>\n			<toDoAction/>\n			<toDoRemarks/>\n			<toDoTaskShort/>\n			<voyageCode/>\n		</CargoUpdateRequest>\n	</AllCargoUpdateRequest>\n</JMTInternalStop>\n");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }
          
}
