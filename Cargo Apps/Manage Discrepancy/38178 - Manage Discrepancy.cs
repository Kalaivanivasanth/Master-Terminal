using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNBaseClasses.BaseClasses.Web;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Manage_Discrepancy
{
    [TestClass, TestCategory(TestCategories.MTNWeb)]
    public class TestCase38178 : MobileAppsBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [ClassCleanup]
        public new static void ClassCleanUp() => WebBase.ClassCleanUp();

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
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }
        


        [TestMethod]
        public void ManageDiscrepancy()
        {

            MTNInitialize();
            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG38178A03");

            // Step 7
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoManageDiscrepancy();

            // Step 8
            MA_CargoAppsManageDiscrepancyPage manageDiscrepancyPage = new MA_CargoAppsManageDiscrepancyPage(TestContext);

            string[,] discrepancyToSet =
            {
                { MA_CargoAppsManageDiscrepancyPage.constNewQuantity, @"90" },
                //{ MA_CargoAppsManageDiscrepancyPage.constWeightMeasurement, @"lbs" },
                { MA_CargoAppsManageDiscrepancyPage.constNewWeight, @"1200.00" },
                { MA_CargoAppsManageDiscrepancyPage.constReason, @"Someone miss counted" }
            };
            manageDiscrepancyPage.SetFields(discrepancyToSet);
            manageDiscrepancyPage.DoBack();

            // Step 9
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            string[,] fieldValueToValidate =
            {
               { MA_CargoAppsDetailsPage.constQuantity,  @"100" }
            };
            detailsPage.ValidateDetails(fieldValueToValidate);
            detailsPage.DoManageDiscrepancy();

            // Step 10
            manageDiscrepancyPage = new MA_CargoAppsManageDiscrepancyPage(TestContext);

            discrepancyToSet = new [,]
            {
                { MA_CargoAppsManageDiscrepancyPage.constNewQuantity, @"90" },
                //{ MA_CargoAppsManageDiscrepancyPage.constWeightMeasurement, @"lbs" },
                { MA_CargoAppsManageDiscrepancyPage.constNewWeight, @"1200.00" },
                { MA_CargoAppsManageDiscrepancyPage.constReason, @"Someone miss counted" }
            };
            manageDiscrepancyPage.SetFields(discrepancyToSet);

            // Additional testing
            manageDiscrepancyPage.IncreaseDecreaseQuantity(false);
            manageDiscrepancyPage.IncreaseDecreaseQuantity(false);
            manageDiscrepancyPage.ValidateNewQuantity(@"88");
            manageDiscrepancyPage.IncreaseDecreaseQuantity();
            manageDiscrepancyPage.IncreaseDecreaseQuantity();
            manageDiscrepancyPage.ValidateNewQuantity(@"90");

            manageDiscrepancyPage.DoClearReason();

            discrepancyToSet = new [,]
            {
               { MA_CargoAppsManageDiscrepancyPage.constReason, @"Someone miss counted" }
            };
            manageDiscrepancyPage.SetFields(discrepancyToSet);

            manageDiscrepancyPage.DoSave();

            // Step 11
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            fieldValueToValidate = new [,]
            {
               { MA_CargoAppsDetailsPage.constQuantity,  @"90" }
            };
            detailsPage.ValidateDetails(fieldValueToValidate);
            detailsPage.DoManageDiscrepancy();

            // Step 12
            discrepancyToSet = new [,]
           {
                { MA_CargoAppsManageDiscrepancyPage.constNewQuantity, @"100" },
                //{ MA_CargoAppsManageDiscrepancyPage.constWeightMeasurement, @"lbs" },
                { MA_CargoAppsManageDiscrepancyPage.constNewWeight, @"544.00" },
                { MA_CargoAppsManageDiscrepancyPage.constReason, @"We've found the missing 10" }
           };
            manageDiscrepancyPage.SetFields(discrepancyToSet);
            manageDiscrepancyPage.DoSave();

            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            fieldValueToValidate = new [,]
            {
               { MA_CargoAppsDetailsPage.constQuantity,  @"100" }
            };
            detailsPage.ValidateDetails(fieldValueToValidate);

            // Step 13 - 18 
            //MTNDesktop.SetFocusToMainWindow();
            LogInto<MTNLogInOutBO>();

            FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry");
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Blank, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG38178");
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                // @"ID^JLG38178A03");
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG38178A03"]);
            cargoEnquiryForm.DoViewTransactions();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^Discrepancy~Details^Old Total Quantity 90, Change 10, New Total Quantity 100Old Total Weight 1200.000 lbs, Change -656.000 lbs, New Total Weight 544.000 lbsReason : We've found the missing 10",
                // ClickType.Click);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^Discrepancy~Details^Old Total Quantity 100, Change -10, New Total Quantity 90Old Total Weight 2204.624 lbs, Change -1004.624 lbs, New Total Weight 1200.000 lbsReason : Someone miss counted", 
                // ClickType.Click);
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow([
                "Type^Discrepancy~Details^Old Total Quantity 90, Change 10, New Total Quantity 100\r\nOld Total Weight 1200.000 lbs, Change -656.000 lbs, New Total Weight 544.000 lbs\r\nReason : We've found the missing 10",
                "Type^Discrepancy~Details^Old Total Quantity 100, Change -10, New Total Quantity 90\r\nOld Total Weight 2204.624 lbs, Change -1004.624 lbs, New Total Weight 1200.000 lbs\r\nReason : Someone miss counted"
            ], ClickType.None);
;        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = "_38178_";
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>   <AllCargoOnSite>     <operationsToPerform>Verify;Load To DB</operationsToPerform>     <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>     <CargoOnSite Terminal='TT1'>       <TestCases>38178</TestCases>       <cargoTypeDescr>Bag of Sand</cargoTypeDescr>       <product>SMSAND</product>       <id>JLG38178A03</id>       <operatorCode>MSL</operatorCode>       <locationId>GCARA1 SMALL_SAND</locationId>       <weight>1000.0000</weight>       <imexStatus>Export</imexStatus>       <dischargePort>NZNPE</dischargePort>       <voyageCode>MSCK000002</voyageCode>       <totalQuantity>100</totalQuantity>       <commodity>SANC</commodity>       <messageMode>D</messageMode>     </CargoOnSite>   </AllCargoOnSite> </JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>   <AllCargoOnSite>     <operationsToPerform>Verify;Load To DB</operationsToPerform>     <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>     <CargoOnSite Terminal='TT1'>       <TestCases>38178</TestCases>       <cargoTypeDescr>Bag of Sand</cargoTypeDescr>       <product>SMSAND</product>       <id>JLG38178A03</id>       <operatorCode>MSL</operatorCode>       <locationId>GCARA1 SMALL_SAND</locationId>       <weight>1000.0000</weight>       <imexStatus>Export</imexStatus>       <dischargePort>NZNPE</dischargePort>       <voyageCode>MSCK000002</voyageCode>       <totalQuantity>100</totalQuantity>       <commodity>SANC</commodity>       <messageMode>A</messageMode>     </CargoOnSite>   </AllCargoOnSite> </JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
