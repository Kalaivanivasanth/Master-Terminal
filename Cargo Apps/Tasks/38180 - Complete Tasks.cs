using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Tasks;
using System;
using System.Net.Mime;
using DataObjects.LogInOutBO;
using MTNGlobal.EnumsStructs;
using MTNWebPages.PageObjects.Popups;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Tasks
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase38180 : MobileAppsBase
    {

        MA_CargoAppsCompleteTasksPage _completeTasksPage;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [ClassCleanup]
        public new static void ClassCleanUp() => WebBase.ClassCleanUp();

        void MTNInitialise()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void CompleteTasks()
        {

            MTNInitialise();
            
            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG38180A01");

            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoCompleteTasks();

            // Step 8
            _completeTasksPage = new MA_CargoAppsCompleteTasksPage(TestContext);
            _completeTasksPage.SetUnsetTasksAndValues(new [] { "Req Paint" });

            // Step 9
            _completeTasksPage.DoBack();

            // Step 10 
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoCompleteTasks();

            // Step 11 - 12
            _completeTasksPage = new MA_CargoAppsCompleteTasksPage(TestContext);
            _completeTasksPage.SetUnsetTasksAndValues(new[]
                { "Requires Pretripping^^^Pretripping has been completed!!!!" });
            
            // Extending to include all types that require additional details as at 21/04/2022
            

            // Check Damage
            DoDamageCheck();
            
            // Requires Weighing
            DoWeight();
            
            // Dimension Check
            DoDimensionCheck();

            // Seals1
            DoSeals();

            // Special Lift
            DoSpecialLifts();
            
            _completeTasksPage.DoSave();

            // Step 13 - 18 - Going to do it this way as don't need to go to Master Terminal to get the details
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();

            string[,] fieldValueToValidate = {
                { MA_CargoAppsDetailsPage.constTasks,  "Req Paint" }
            };
            detailsPage.ValidateDetails(fieldValueToValidate);
            
            
            
        }

        void DoDamageCheck()
        {
            _completeTasksPage = new MA_CargoAppsCompleteTasksPage(TestContext);
            _completeTasksPage.SetUnsetTasksAndValues(new[] { "Check Damage" });

            MA_CargoAppsCompleteTaskDamagePage
                completeTaskDamagePage = new MA_CargoAppsCompleteTaskDamagePage(TestContext);
            completeTaskDamagePage.DoSave();
        }

        void DoWeight()
        {
            _completeTasksPage = new MA_CargoAppsCompleteTasksPage(TestContext);
            _completeTasksPage.SetUnsetTasksAndValues(new[] { "Requires Weighing" });

            MA_CargoAppsCompleteTaskWeightPage
                completeTaskWeightPage = new MA_CargoAppsCompleteTaskWeightPage(TestContext);
            string[,] weightDetails =
            {
                { MA_CargoAppsCompleteTaskWeightPage.constTotalWeight, "6500" },
                { MA_CargoAppsCompleteTaskWeightPage.constIsWeightCertified, "1" },
                { MA_CargoAppsCompleteTaskWeightPage.constWeightCertifiedBy, "TEST TEAM" },
            };
            completeTaskWeightPage.SetFieldsOnPage(weightDetails);
            completeTaskWeightPage.DoSave();
            
            WarningErrorPopup warningErrorPopup = new WarningErrorPopup(TestContext);
            warningErrorPopup.DoOK();
            
        }

        void DoDimensionCheck()
        {
            _completeTasksPage = new MA_CargoAppsCompleteTasksPage(TestContext);
            _completeTasksPage.SetUnsetTasksAndValues(new[] { "Dimension Check" });

            MA_CargoAppsCompleteTaskDimensionCheckPage
                completeTaskDimensionCheckPage = new MA_CargoAppsCompleteTaskDimensionCheckPage(TestContext);
            string[] dimensionDetails =
            {
                "Front^12^",
                "Back^13^",
                "Left^14^",
                "Right^15^",
                "Height^16^",
            };
            completeTaskDimensionCheckPage.SetFieldValues(dimensionDetails);
            completeTaskDimensionCheckPage.DoSave();
            
            WarningErrorPopup warningErrorPopup = new WarningErrorPopup(TestContext);
            warningErrorPopup.DoOK();
        }

        void DoSeals()
        {
            _completeTasksPage = new MA_CargoAppsCompleteTasksPage(TestContext);
            _completeTasksPage.SetUnsetTasksAndValues(new[]
                { "Seals1" });

            MA_CargoAppsCompleteTaskSealsPage
                completeTaskSealsPage = new MA_CargoAppsCompleteTaskSealsPage(TestContext);
            completeTaskSealsPage.DoSeals();

            MA_CargoAppsCompleteTaskSealsDetailPage completeTaskSealsDetailsPage =
                new MA_CargoAppsCompleteTaskSealsDetailPage(TestContext);

            string[] sealDetails = 
            {
                $"{MA_CargoAppsCompleteTaskSealsDetailPage.constOperator}^38180Op",
                $"{MA_CargoAppsCompleteTaskSealsDetailPage.constMAFCustoms}^38180MAFCUS",
                $"{MA_CargoAppsCompleteTaskSealsDetailPage.constShippers}^38180Ship",
                $"{MA_CargoAppsCompleteTaskSealsDetailPage.constVent}^38180Vent",
            };
            completeTaskSealsDetailsPage.SetFieldValues(sealDetails);
            completeTaskSealsDetailsPage.DoSave();
            
            WarningErrorPopup warningErrorPopup = new WarningErrorPopup(TestContext);
            warningErrorPopup.DoOK();
            
            completeTaskSealsPage = new MA_CargoAppsCompleteTaskSealsPage(TestContext);
            completeTaskSealsPage.DoSave();
            
        }

        void DoSpecialLifts()
        {
            _completeTasksPage = new MA_CargoAppsCompleteTasksPage(TestContext);
            _completeTasksPage.SetUnsetTasksAndValues(new[]
                { "Spec Lift Check" });

            MA_CargoAppsCompleteTaskSpecialLiftPage appsCompleteTaskSpecialLiftPage =
                new MA_CargoAppsCompleteTaskSpecialLiftPage(TestContext);

            string[] specialLifts =
            {
                "Special Lift 2",
                "Special Lift 3"
            };
            appsCompleteTaskSpecialLiftPage.SelectSpecialLifts(specialLifts);
            appsCompleteTaskSpecialLiftPage.DoSave();
            
            WarningErrorPopup warningErrorPopup = new WarningErrorPopup(TestContext);
            warningErrorPopup.DoOK();
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_38180_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>   <AllCargoOnSite>     <operationsToPerform>Verify;Load To DB</operationsToPerform>     <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>     <CargoOnSite Terminal='TT1'>       <TestCases>38180</TestCases>       <cargoTypeDescr>ISO Container</cargoTypeDescr>       <id>JLG38180A01</id>       <isoType>2210</isoType>       <operatorCode>MSL</operatorCode>       <locationId>MKBS02</locationId>       <weight>5000.0000</weight>       <imexStatus>Export</imexStatus>       <commodity>GENL</commodity>       <dischargePort>NZAKL</dischargePort>       <voyageCode>MSCK000002</voyageCode> 	   <messageMode>D</messageMode>     </CargoOnSite>   </AllCargoOnSite> </JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>   <AllCargoOnSite>     <operationsToPerform>Verify;Load To DB</operationsToPerform>     <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>     <CargoOnSite Terminal='TT1'>       <TestCases>38180</TestCases>       <cargoTypeDescr>ISO Container</cargoTypeDescr>       <id>JLG38180A01</id>       <isoType>2210</isoType>       <operatorCode>MSL</operatorCode>       <locationId>MKBS02</locationId>       <weight>5000.0000</weight>       <imexStatus>Export</imexStatus>       <commodity>GENL</commodity>       <dischargePort>NZAKL</dischargePort>       <voyageCode>MSCK000002</voyageCode> 	   <messageMode>A</messageMode>     </CargoOnSite>   </AllCargoOnSite> </JMTInternalCargoOnSite>");

            // Add To Do Task
            CreateDataFileToLoad(@"AddToDoTask.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalToDoTask xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalToDoTask.xsd'>\n   <AllToDoTask>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <ToDoTask Terminal='TT1'>\n         <isComplete>false</isComplete>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG38180A01</id>\n         <messageMode>A</messageMode>\n         <taskCode>PTI</taskCode>\n      </ToDoTask>\n      <ToDoTask Terminal='TT1'>\n         <isComplete>false</isComplete>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG38180A01</id>\n         <messageMode>A</messageMode>\n         <taskCode>Req Paint</taskCode>\n      </ToDoTask>\n	  \n	  <ToDoTask Terminal='TT1'>\n         <isComplete>false</isComplete>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG38180A01</id>\n         <messageMode>A</messageMode>\n         <taskCode>SLIFT</taskCode>\n      </ToDoTask>\n	  <ToDoTask Terminal='TT1'>\n         <isComplete>false</isComplete>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG38180A01</id>\n         <messageMode>A</messageMode>\n         <taskCode>SEALS1</taskCode>\n      </ToDoTask>\n	  <ToDoTask Terminal='TT1'>\n         <isComplete>false</isComplete>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG38180A01</id>\n         <messageMode>A</messageMode>\n         <taskCode>DIMSCHK</taskCode>\n      </ToDoTask>\n	  <ToDoTask Terminal='TT1'>\n         <isComplete>false</isComplete>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG38180A01</id>\n         <messageMode>A</messageMode>\n         <taskCode>Weight</taskCode>\n      </ToDoTask>\n	  <ToDoTask Terminal='TT1'>\n         <isComplete>false</isComplete>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG38180A01</id>\n         <messageMode>A</messageMode>\n         <taskCode>Damage</taskCode>\n      </ToDoTask>\n   </AllToDoTask>\n</JMTInternalToDoTask>");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
