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
using MTNWebPages.PageObjects.Popups;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Tasks
{
    [TestClass, TestCategory(TestCategories.MTNWeb)]
    public class TestCase41085 : MobileAppsBase
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
            searchFor = @"_41085_";
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void CheckDamageToDoTask()
        {
            MTNInitialize();

            // Step 5 - 9: Will be done by data load

            // Step 10 - 11
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 12
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID("JLG41085A01");

            // Step 13
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();

            // Step 14
            string[,] fieldValueToValidate = {
                { MA_CargoAppsDetailsPage.constTasks,  "Damage" }
            };
            detailsPage.ValidateDetails(fieldValueToValidate);

            // Step 15
            detailsPage.DoCompleteTasks();

            // Step 16 - 18
            MA_CargoAppsCompleteTasksPage completeTasksPage = new MA_CargoAppsCompleteTasksPage(TestContext);
            completeTasksPage.SetUnsetTasksAndValues(new [] { "Check Damage" });
            completeTasksPage.DoSave();

            try
            {
                WarningErrorPopup warningErrorPopup = new WarningErrorPopup(TestContext);

                string[] warningErrorToCheck =
                {
                    "Code: 75016 The Container Id (JLG41085A01) failed the validity checks and may be incorrect."
                };
                warningErrorPopup.CheckMessage(WarningErrorPopup.constWarning, warningErrorToCheck);
                warningErrorPopup.DoSave();
            }
            catch {}

            completeTasksPage = new MA_CargoAppsCompleteTasksPage(TestContext);
            completeTasksPage.DoSave();

            // Step 19
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoExpander();

            // Step 14
            string detailsNotFound = detailsPage.ValidateExistsInExpander(new [] { "Damage" }, false);
            Assert.IsTrue(detailsNotFound.Contains("Damage"), "TestCase41085 - Damage To Do Task was NOT completed.");

            // Step 21 - 25
            LogInto<MTNLogInOutBO>();

            // Step 31 - 37
            FormObjectBase.NavigationMenuSelection("General Functions | Cargo Enquiry");
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", "ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", "JLG41085A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", "On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                // "Location ID^MKBS01~ID^JLG41085A01~Opr^COS~Discharge Port^NZAKL~Type^2200");
            cargoEnquiryForm.tblData2.FindClickRow(["Location ID^MKBS01~ID^JLG41085A01~Opr^COS~Discharge Port^NZAKL~Type^2200"]);

            cargoEnquiryForm.GetStatusTable("4087");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus, "Damaged", "No");

            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^Repaired.~Details^Todo Task Completed: Check Damage");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^Damaged.~Details^Todo Task Created:Check Damage");
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow([
                "Type^Repaired.~Details^Todo Task Completed: Check Damage",
                "Type^Damaged.~Details^Todo Task Created:Check Damage"
            ]);
;        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG41085A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>COS</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GEN</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n		    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG41085A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>COS</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GEN</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n		    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Add To-Do Task
            CreateDataFileToLoad(@"AddToDoTask.xml",
                "<?xml version='1.0'?> <JMTInternalToDoTask xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalToDoTask.xsd'>\n	<AllToDoTask>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<ToDoTask Terminal='TT1'>\n			<isComplete>false</isComplete>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<id>JLG41085A01</id>\n			<messageMode>A</messageMode>\n			<taskCode>Damage</taskCode>\n		</ToDoTask>\n	</AllToDoTask>\n</JMTInternalToDoTask>\n\n\n");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
