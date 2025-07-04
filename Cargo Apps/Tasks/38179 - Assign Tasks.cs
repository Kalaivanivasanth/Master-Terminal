using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Tasks;
using System;
using DataObjects.LogInOutBO;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Tasks
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase38179 : MobileAppsBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [ClassCleanup]
        public new static void ClassCleanUp() => WebBase.ClassCleanUp();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }
        

        [TestMethod]
        public void AssignTasks()
        {
            MTNInitialize();
            
            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG38179A01");

            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoAssignTasks();

            // Step 8
            MA_CargoAppsAssignTasksPage assignTasksPage = new MA_CargoAppsAssignTasksPage(TestContext);

            string[] tasksToSetUnsetValues =
            {
                @"Requires Pretripping^1^Pretripping remarks",
                @"Req Paint^1"
            };
            assignTasksPage.SetUnsetTasksAndValues(tasksToSetUnsetValues);
            assignTasksPage.DoBack();

            // Step 9 
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoAssignTasks();

            // Step 10
            assignTasksPage = new MA_CargoAppsAssignTasksPage(TestContext);
            assignTasksPage.DoFilterFor(@"Req");

            // Step 11
            assignTasksPage.DoClearFilter();
            //assignTasksPage.DoFilterReact();

            // Step 12
           tasksToSetUnsetValues = new []
            {
                @"Req Paint^1^Req Paint remarks"
            };
            assignTasksPage.SetUnsetTasksAndValues(tasksToSetUnsetValues);
            assignTasksPage.DoSave();

            // Step 13
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoAssignTasks();

            
            assignTasksPage = new MA_CargoAppsAssignTasksPage(TestContext);

            // * Unable to retrieve remarks but will leave commented out code for the moment
            string[] remarks = 
            {
                @"Req Paint^Req Paint remarks"
            };
            assignTasksPage.ValidateRemarks(remarks);
            assignTasksPage.ReturnToTilesPage();

            HomePage = new MA_TilesPage(TestContext);
            HomePage.ClickTile(HomePage.BtnCargoApps);

            searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG38179A01");
           
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();

            string[,] fieldValueToValidate = {
                { MA_CargoAppsDetailsPage.constTasks,  @"Req Paint" }
            };
            detailsPage.ValidateDetails(fieldValueToValidate);
            

        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_38179_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>   <AllCargoOnSite>     <operationsToPerform>Verify;Load To DB</operationsToPerform>     <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>     <CargoOnSite Terminal='TT1'>       <TestCases>38179</TestCases>       <cargoTypeDescr>ISO Container</cargoTypeDescr>       <id>JLG38179A01</id>       <isoType>2210</isoType>       <operatorCode>MSL</operatorCode>       <locationId>MKBS02</locationId>       <weight>5000.0000</weight>       <imexStatus>Export</imexStatus>       <commodity>GENL</commodity>       <dischargePort>NZAKL</dischargePort>       <voyageCode>MSCK000002</voyageCode>       <messageMode>D</messageMode>     </CargoOnSite>   </AllCargoOnSite> </JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>   <AllCargoOnSite>     <operationsToPerform>Verify;Load To DB</operationsToPerform>     <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>     <CargoOnSite Terminal='TT1'>       <TestCases>38179</TestCases>       <cargoTypeDescr>ISO Container</cargoTypeDescr>       <id>JLG38179A01</id>       <isoType>2210</isoType>       <operatorCode>MSL</operatorCode>       <locationId>MKBS02</locationId>       <weight>5000.0000</weight>       <imexStatus>Export</imexStatus>       <commodity>GENL</commodity>       <dischargePort>NZAKL</dischargePort>       <voyageCode>MSCK000002</voyageCode>       <messageMode>A</messageMode>     </CargoOnSite>   </AllCargoOnSite> </JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
