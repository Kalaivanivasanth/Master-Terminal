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
using MTNWebPages.PageObjects.Popups;
using System;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Repair;
using DataObjects.LogInOutBO;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Damage;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Repair
{
    /// --------------------------------------------------------------------------------------------------
    /// Date         Person          Reason for change
    /// ==========   ======          =================================
    /// 19/05/2022   navmp4          Cargo Repair App functionality
    /// --------------------------------------------------------------------------------------------------
    [TestClass, TestCategory(TestCategories.MTNWeb)]
    public class TestCase53643 : MobileAppsBase
    {

        AddDamagePopup_React _addDamagePopup;
        MA_CargoAppsDetailsPage _detailsPage;
        MA_CargoDamageRepairPage _cargoDamageRepairPage;


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
         => BaseClassInitialize_New(testContext);




        [ClassCleanup]
        public new static void ClassCleanUp()
        {
            WebBase.ClassCleanUp();
        }



        [TestCleanup]
        public new void TestCleanup()
        {
            MTNBase.MTNTestCleanup(TestContext);
            MTNBase.MTNCleanup();
            base.TestCleanup();
        }

        private void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);


            // MTNLogin();

            //MobileAppsLogin();
            LogInto<MobileAppsLogInOutBO>("MTNOLDDAM");
            HomePage = new MA_TilesPage(TestContext);

        }

        [TestMethod]
        public void RepairApp()
        {
            MTNInitialize();

            // Click on the Cargo Apps tile
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Search for the Cargo 
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG53643A01");

            // Click on Damage tile
            _detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            _detailsPage.DoDamage();

            MA_CargoAppsDamageReportPage damageReportPage = new MA_CargoAppsDamageReportPage(TestContext);
            damageReportPage.DoPlus();

            // Add Damage details
            _addDamagePopup = new AddDamagePopup_React(TestContext);
            string[,] damageDetails =
            {
                { AddDamagePopup_React.constDamage, @"RUST" },
                { AddDamagePopup_React.constPosition, @"DOOR" },
                { AddDamagePopup_React.constQuantity, @"2" },
                { AddDamagePopup_React.constSeverity, @"3" }
            };
            _addDamagePopup.SetFields(damageDetails);
            _addDamagePopup.DoAdd();
            _addDamagePopup.DoSave();
            _addDamagePopup.CheckDamageDetails(@"RUST", @"DOOR");

            damageReportPage.DoPlus();

            damageDetails = new string[,]
            {
                { AddDamagePopup_React.constDamage, @"DENT" },
                { AddDamagePopup_React.constPosition, @"Back" },
                { AddDamagePopup_React.constQuantity, @"3" },
                { AddDamagePopup_React.constSeverity, @"4" }
            };
            _addDamagePopup.SetFields(damageDetails);
            _addDamagePopup.DoAdd();
            _addDamagePopup.DoSave();
            _addDamagePopup.CheckDamageDetails(@"DENT", @"Back");

            damageReportPage.DoPlus();
            damageDetails = new string[,]
            {
                { AddDamagePopup_React.constDamage, @"BENT" },
                { AddDamagePopup_React.constPosition, @"Bottom" },
                { AddDamagePopup_React.constQuantity, @"4" },
                { AddDamagePopup_React.constSeverity, @"5" }
            };
            _addDamagePopup.SetFields(damageDetails);
            _addDamagePopup.DoAdd();
            _addDamagePopup.DoSave();
            _addDamagePopup.CheckDamageDetails(@"BENT", @"Bottom");
            _addDamagePopup.DoBack();

            //Click on Repair tile
            _detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            _detailsPage.DoRepair();

            _cargoDamageRepairPage = new MA_CargoDamageRepairPage(TestContext);

            // Set Damage "Denty to Repaired and Cancel it
            _cargoDamageRepairPage.SetToRepaired(@"Denty", @"Back");
            _cargoDamageRepairPage.DoCancel();

            // Cancel the warning and Save it
            _cargoDamageRepairPage.DoCancelWarning();
            _cargoDamageRepairPage.DoSave();

            // Click on Repair tile again
            _detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            _detailsPage.DoRepair();

            // Click All button to select all the damages
            _cargoDamageRepairPage.DoAll();

            // Cancel and Confirm the warning message
            _cargoDamageRepairPage.DoCancel();
            _cargoDamageRepairPage.DoConfirm();

            // Click on Repair tile again
            _detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            _detailsPage.DoRepair();

            // Click on All button and Save
            _cargoDamageRepairPage.DoAll();
            _cargoDamageRepairPage.DoSave();

            // Set Focus to MTN
            LogInto<MTNLogInOutBO>("MTNOLDDAM");
            MTNDesktop.SetFocusToMainWindow();

            // Open Cargo Enquiry
            // FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry");
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            // Search for the cargo
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG53643A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.GetStatusTable(@"4087");

            // Damage Detail (dbl click) field should be empty
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus,
                @"Damage Detail (dbl click)", @"");

            // Check the transactions of the cargo item
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();


            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new string[]
{
    @"Type^Repaired.",
    @"Type^Repair Details~Details^Bottom BENT(5) (x4)",
    @"Type^Repair Details~Details^DOOR RUST(3) (x2)",
    @"Type^Repair Details~Details^Back DENT(4) (x3)"
});

           

            //cargoenquirytransactionform cargoenquirytransactionform = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            //mtncontrolbase.findclickrowintable(cargoenquirytransactionform.tbltransactions,
            //    @"type^repaired.");
            //mtncontrolbase.findclickrowintable(cargoenquirytransactionform.tbltransactions,
            //    @"type^repair details~details^bottom bent(5) (x4)");
            //mtncontrolbase.findclickrowintable(cargoenquirytransactionform.tbltransactions,
            //    @"type^repair details~details^door rust(3) (x2)");
            //mtncontrolbase.findclickrowintable(cargoenquirytransactionform.tbltransactions,
            //    @"type^repair details~details^back dent(4) (x3)");

        }

       

   


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_53643_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>53643</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG53643A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>53643</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG53643A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>A</messageMode>\n    </CargoOnSite>\n      </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
