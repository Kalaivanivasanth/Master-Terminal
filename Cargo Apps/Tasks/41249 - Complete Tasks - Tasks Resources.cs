using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNBaseClasses.BaseClasses.Web;
using MTNDesktopFlaUI.Classes;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Tasks;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Tasks
{
    [TestClass, TestCategory(TestCategories.MTNWeb)]
    public class TestCase41249 : MobileAppsBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
       
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
            MTNBase.MTNTestCleanup(TestContext);
            MTNBase.MTNCleanup();
        }

        void MTNInitialize()
        {
            // Setup data
            searchFor = @"_41249_";
            SetupAndLoadInitializeData(TestContext);
            
            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void CompleteTasksWithResources()
        {
            MTNInitialize();
            
            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID("JLG41249A01");

            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoAssignTasks();

            // Step 8
            MA_CargoAppsAssignTasksPage assignTasksPage = new MA_CargoAppsAssignTasksPage(TestContext);

            assignTasksPage.SetUnsetTasksAndValues(new [] { "Stick Label^1" });
            assignTasksPage.DoSave();
            
            // Step 9
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
           detailsPage.DoExpander();
            detailsPage.ValidateDetails(new [,] { { MA_CargoAppsDetailsPage.constTasks,  "Stick Label" } });

            // Step 10 - 11
            detailsPage.DoCompleteTasks();
            MA_CargoAppsCompleteTasksPage completeTasksPage = new MA_CargoAppsCompleteTasksPage(TestContext);
            completeTasksPage.SetUnsetTasksAndValues(new [] { "Stick Label" });

            // Step 12
            MA_CargoAppsTaskResourcesPage taskResourcesPage = new MA_CargoAppsTaskResourcesPage(TestContext);
            taskResourcesPage.DoPlus();

            DateTime fromTS = DateTime.Today;
            fromTS = fromTS.AddDays(-1);
            DateTime toTS = DateTime.Today;

            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(1));
            taskResourcesPage = new MA_CargoAppsTaskResourcesPage(TestContext);
            taskResourcesPage.AddResourcePopupFound();
            
            string[,] resourceDetails =  
            {
                { TaskAddResourcePopup.constResource, "Labour" },
                { TaskAddResourcePopup.constGang, "5" },
                { TaskAddResourcePopup.constFromTS, $"{fromTS:dd/MM/yyyy}^{fromTS:hh:mmtt}" },
                { TaskAddResourcePopup.constToTS, $"{toTS:dd/MM/yyyy}^{toTS:hh:mmtt}" },
                { TaskAddResourcePopup.constNoChange, "1" }
            };
            taskResourcesPage.SetResourceFields(resourceDetails);
            taskResourcesPage.DoResourceSave();
            
            taskResourcesPage = new MA_CargoAppsTaskResourcesPage(TestContext);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            taskResourcesPage.DoSave();

            completeTasksPage = new MA_CargoAppsCompleteTasksPage(TestContext);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            completeTasksPage.DoSave();

            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
           
            string detailsNotFound = detailsPage.ValidateExistsInExpander(new [] { "Tasks" }, false);
            Assert.IsTrue(detailsNotFound.Contains("Tasks"), "TestCase41249 - Task should NOT have been found");

            // Step 19 - 22
            LogInto<MTNLogInOutBO>();

            FormObjectBase.NavigationMenuSelection("General Functions | Cargo Enquiry");
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", "ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", "JLG41249A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", "On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // "Type^Todo Task Completed~Details^Todo Task Completed:  Stick Label  Resources used: 5.0 Gang [ Labour ]");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // "Type^Todo Task Added~Details^Todo Task Added: Stick Label");
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow([
                "Type^Todo Task Completed~Details^Todo Task Completed:  Stick Label  Resources used: 5.0 Gang [ Labour ]",
                "Type^Todo Task Added~Details^Todo Task Added: Stick Label"
            ]);
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG41249A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>MKBS01</locationId>\n            <weight>5000</weight>\n            <imexStatus>Import</imexStatus>\n            <commodity>MT</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <isoType>2200</isoType>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG41249A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>MKBS01</locationId>\n            <weight>5000</weight>\n            <imexStatus>Import</imexStatus>\n            <commodity>MT</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <isoType>2200</isoType>\n			<messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
