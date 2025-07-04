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
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Seal_Checks;
using MTNWebPages.PageObjects.Popups;
using MTNWebSelenium.Classes;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Seal_Checks
{
    [TestClass, TestCategory(TestCategories.MTNWeb)]
    public class TestCase37859: MobileAppsBase
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


        private void MTNInitialize()
        {
            searchFor = @"_37859_";
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);

        }


        [TestMethod]
        public void PopulateAndClearSealChecks()
        {

            MTNInitialize();

            // Step 5 Click Cargo Apps tile  
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6 Enter Cargo ID and click Search icon
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG37859A01");

            // Step 7 Click Seal Checks tile  
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoSealChecks();

            // Step 8 Enter the Operator Seal = OPER, MAF / Customs Seal = CUST
            MA_CargoAppsSealChecksPage sealsPage = new MA_CargoAppsSealChecksPage(TestContext);
            string[,] sealsToSet = {
                { MA_CargoAppsSealChecksPage.constOperatorSeal,  @"OPER" },
                { MA_CargoAppsSealChecksPage.constMAFCustomsSeal,  @"CUST" }
            };
            sealsPage.SetSeals(sealsToSet);

            // Step 9 Click Save button
            sealsPage.DoSave();

            // Click Save on the Warnings pop up
            /*WarningErrorPopup warningErrorPopup = new WarningErrorPopup(TestContext);
            string[] warningErrorToCheck =
            {
               @"Code: 75016 The Container Id (JLG37859A01) failed the validity checks and may be incorrect."
            };
            warningErrorPopup.CheckMessage(WarningErrorPopup.constWarning, warningErrorToCheck);
            warningErrorPopup.DoSave();*/
            Popup_OK_Cancel popupOKCancel = new Popup_OK_Cancel(TestContext);
            string[] messagesToCheck =
            {
                "Code: 75016 The Container Id (JLG37859A01) failed the validity checks and may be incorrect."
            };
            popupOKCancel.CheckMessage(Popup_OK.constFromPageCargoMaintenance, "Warning(s)", messagesToCheck);
            popupOKCancel.DoOK();

            // Step 10 Go to MTN
            //MTNDesktop.SetFocusToMainWindow();
            LogInto<MTNLogInOutBO>();

            // Step 11 Open Cargo Enquiry form
            FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry");
            cargoEnquiryForm = new CargoEnquiryForm();

            // Step 12 Enter Cargo Id and click Find button
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG37859A01");

            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            // Step 13 Go to Status tab  
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4087");

            // Step 14 check values for Operator and MAF/Customs Seals
            string shipperSeal = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblStatus, @"Operator Seal");
            Assert.IsTrue(shipperSeal == @"OPER", @"Field: Operator Seal: " + shipperSeal + " doesn't equal: " + @"OPER");

            string MAFSeal = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblStatus, @"MAF/Customs Seal");
            Assert.IsTrue(MAFSeal == @"CUST", @"Field: MAF/Customs Seal: " + MAFSeal + " doesn't equal: " + @"CUST");

            // Step 15 Go back to Mobile Apps
            MTNWeb.webDriver.SwitchTo().Window(MTNWeb.webDriver.CurrentWindowHandle);
            
            // Step 16 Search for Cargo Id - JLG37859A01
            searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG37859A01");

            // Step 17 Click Seal Checks tils
            detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoSealChecks();

            // Step 18 Click 'X' button for Operator and MAF/Customs Seal 
            sealsPage = new MA_CargoAppsSealChecksPage(TestContext);
            string[] sealsToClear = {
                 MA_CargoAppsSealChecksPage.constOperatorSeal,
                 MA_CargoAppsSealChecksPage.constMAFCustomsSeal 
            };
            sealsPage.ClearSeals(sealsToClear);

            // Step 19 Click Save button
            sealsPage.DoSave();

            // Click Save on Warnings pop up
            /*warningErrorPopup = new WarningErrorPopup(TestContext);
            warningErrorPopup.CheckMessage(WarningErrorPopup.constWarning, warningErrorToCheck);
            warningErrorPopup.DoSave();*/
            popupOKCancel = new Popup_OK_Cancel(TestContext);
            popupOKCancel.DoOK();

            // Step 20 Back to MTN
            MTNDesktop.SetFocusToMainWindow();

            // Step 21 Open Cargo Enquiry
            //FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry");  //, forceReset : true);
            //cargoEnquiryForm = new CargoEnquiryForm();
            cargoEnquiryForm.SetFocusToForm();

            // Step 22 Enter Cargo Id and click Find button
            //MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG37859A01");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            // Step 23 Go to Status tab
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4087");

            // Step 24 check values for Operator and MAF/Customs Seals
            shipperSeal = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblStatus, @"Shippers Seal");
            Assert.IsTrue(shipperSeal == @"", @"Field: Shippers Seal: " + shipperSeal + " is not empty");

            MAFSeal = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblStatus, @"Shippers Seal");
            Assert.IsTrue(MAFSeal == @"", @"Field: MAF/Customs Seal: " + MAFSeal + " is not empty");
        }

        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37859</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37859A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSK</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1814.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37859</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37859A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSK</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1814.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
