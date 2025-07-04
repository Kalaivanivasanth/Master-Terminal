using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNBaseClasses.BaseClasses.Web;
using MTNDesktopFlaUI.Classes;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using MTNWebPages.PageObjects.Popups;
using System;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using HardcodedData.SystemData;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Damage;
using System.Threading;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Damage
{
    /// --------------------------------------------------------------------------------------------------
    /// Date         Person          Reason for change
    /// ==========   ======          =================================
    /// 25/02/2022   navmh5          Change to use new React Framework
    /// --------------------------------------------------------------------------------------------------
    [TestClass, TestCategory(TestCategories.MTNWeb)]
    public class TestCase38166 : MobileAppsBase
    {
        Popup_OK _popupOKPage;
        AddDamagePopup_React _addDamagePopup;

        string[] _messagesToCheck;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

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
           

            /*MTNLogin();

            MobileAppsLogin();*/
            LogInto<MobileAppsLogInOutBO>("MTNOLDDAM");
            HomePage = new MA_TilesPage(TestContext);

        }



        [TestMethod]
        public void Damage()
        {
            MTNInitialize();

            // Step 5
            HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6
            MA_CargoAppsSearchPage searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.SearchForCargoID(@"JLG38166");
            Miscellaneous.WaitForSeconds(2);
            searchPage.SelectCargo(@"JLG38166A02");
           
            // Step 7 - 8
            MA_CargoAppsDetailsPage detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            detailsPage.DoDamage();

            MA_CargoAppsDamageReportPage damageReportPage = new MA_CargoAppsDamageReportPage(TestContext);
            damageReportPage.DoPlus();


            // Step 9 - Add Damage details
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
            //_addDamagePopup.DoAccept();

            // Step 10 - Add Image and save
            //_addDamagePopup.DoAddImages(@"38166 - Damage - Step 10.png");
            //_addDamagePopup.DoSave();

            // <06/03/2025> <NAVRS11> Can be removed 6 months after specified date

            // We can Directly click on Edit - Added as per the New Damage Flow
            // Step 11
            //detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            //detailsPage.DoDamage();
            // Step 12 - Edit Damage Details
            //_addDamagePopup = new AddDamagePopup_React(TestContext);
            // Check the damage details

            // Step 11 - Edit Damage Details
            _addDamagePopup.CheckDamageDetails(@"RUST", @"DOOR");
            _addDamagePopup.DoEdit();

            damageDetails = new [,]
            {
                { AddDamagePopup_React.constDamage, @"DENT" },
                { AddDamagePopup_React.constPosition, @"Bottom" },
                { AddDamagePopup_React.constQuantity, @"4" },
                { AddDamagePopup_React.constSeverity, @"4" }
            };
            _addDamagePopup.SetFields(damageDetails);
            _addDamagePopup.DoAdd();

            Thread.Sleep(1000);

            // As per the New Damage Flow - We need to click on the Save button to get the warning message
            _addDamagePopup.DoSave();
            Thread.Sleep(1000);
            _popupOKPage = new Popup_OK(TestContext);
            _messagesToCheck = new []
            {
               @"Code :82156 There was originally damage recorded on the Door side of the cargo item (Rust). Are you sure you want to overwrite this?"
            };

            _popupOKPage.CheckMessage(null, @"Warning(s)", _messagesToCheck);
            _popupOKPage.DoOK();

            // Step 13 Check the damage details
            _addDamagePopup.CheckDamageDetails(@"DENT", @"Bottom");
            _addDamagePopup.DoEdit();

            // <06/03/2025> <NAVRS11> Can be removed 6 months after specified date

            // Step 14 - Clear the details
            //_addDamagePopup.DoClear();

            // _addDamagePopup.DoEdit();
            // Step 15 - Delete the damage details
            _addDamagePopup.DoDelete();

            _messagesToCheck = new string[]
            {
               @"Are you sure you want to delete this?"
            };

            _popupOKPage.CheckMessage(null, @"Damage", _messagesToCheck);
            _popupOKPage.DoOK();
            Thread.Sleep(1000);

            _addDamagePopup.DoSave();

            _messagesToCheck = new []
            {
               @"Code :82156 There was originally damage recorded on the Bottom side of the cargo item (Denty). Are you sure you want to overwrite this?"
            };

            _popupOKPage.CheckMessage(null, @"Warning(s)", _messagesToCheck);
            _popupOKPage.DoOK();
           

            Thread.Sleep(1000); 

            damageReportPage.DoPlus();


            // Step 15 - 

            damageDetails = new[,]
          {
                { AddDamagePopup_React.constDamage, @"RUST" },
                { AddDamagePopup_React.constPosition, @"DOOR" },
                { AddDamagePopup_React.constQuantity, @"3" },
                { AddDamagePopup_React.constSeverity, @"2" }
            };
            _addDamagePopup.SetFields(damageDetails);
            _addDamagePopup.DoAdd();
            _addDamagePopup.DoSave();

            _addDamagePopup.CheckDamageDetails(@"RUST", @"DOOR");


            damageReportPage.DoPlus();


            // Step 15 - 18
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            damageDetails = new [,]
            {
                { AddDamagePopup_React.constDamage, @"DAM" },
            };
            CheckErrorOccursWhenRequiredFieldsNotFilledIn(damageDetails);
            
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            damageDetails = new [,]
            {
                { AddDamagePopup_React.constPosition, @"Back" },
            };
            CheckErrorOccursWhenRequiredFieldsNotFilledIn(damageDetails);
           
            damageDetails = new [,]
            {
                { AddDamagePopup_React.constDamage, @"DAM" },
                { AddDamagePopup_React.constPosition, @"Right" },
                { AddDamagePopup_React.constSeverity, @"1" },
                { AddDamagePopup_React.constQuantity, @"6" },
            };
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            _addDamagePopup = new AddDamagePopup_React(TestContext);
            _addDamagePopup.SetFields(damageDetails);
            _addDamagePopup.DoAdd();
            _addDamagePopup.DoSave();


            damageDetails = new [,]
            {
                { AddDamagePopup_React.ConstDamageClassName, @"RUST" },
                { AddDamagePopup_React.ConstPositionClassName, @"Door" },
           };
            _addDamagePopup.CheckDamageDetails(damageDetails);
            
            damageDetails = new [,]
            {
                { AddDamagePopup_React.ConstDamageClassName, @"DAM" },
                { AddDamagePopup_React.ConstPositionClassName, @"Right" },
                { AddDamagePopup_React.ConstSeverityClassName, @"1" },
                { AddDamagePopup_React.ConstQuantityClassName, @"6" },
            };
            _addDamagePopup.CheckDamageDetails(damageDetails);
            _addDamagePopup.DoEdit();
            
            damageDetails = new [,]
            {
                { AddDamagePopup_React.constSeverity, @"4" },
                { AddDamagePopup_React.constQuantity, @"8" },
            };
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            _addDamagePopup = new AddDamagePopup_React(TestContext);
            _addDamagePopup.SetFields(damageDetails);
            _addDamagePopup.DoAdd();
            
            damageDetails = new [,]
            {
                { AddDamagePopup_React.ConstDamageClassName, @"RUST" },
                { AddDamagePopup_React.ConstPositionClassName, @"Door" },
                { AddDamagePopup_React.ConstSeverityClassName, @"2" },
                { AddDamagePopup_React.ConstQuantityClassName, @"3" },
            };
            _addDamagePopup.CheckDamageDetails(damageDetails);
           
            damageDetails = new [,]
            {
                { AddDamagePopup_React.ConstDamageClassName, @"DAM" },
                { AddDamagePopup_React.ConstPositionClassName, @"Right" },
                { AddDamagePopup_React.ConstSeverityClassName, @"4" },
                { AddDamagePopup_React.ConstQuantityClassName, @"8" },
            };
            _addDamagePopup.CheckDamageDetails(damageDetails);
            _addDamagePopup.DoEdit();
            
            _addDamagePopup.DoDelete();
            _popupOKPage = new Popup_OK(TestContext);
            _messagesToCheck = new []
            {
                "Are you sure you want to delete this?"
            };

            _popupOKPage.CheckMessage(null, "Damage", _messagesToCheck);
            _popupOKPage.DoCancel();
            
            _addDamagePopup.DoDelete();
            _popupOKPage = new Popup_OK(TestContext);
            _messagesToCheck = new []
            {
                "Are you sure you want to delete this?"
            };

            _popupOKPage.CheckMessage(null, "Damage", _messagesToCheck);
            _popupOKPage.DoOK();
            Thread.Sleep(1000);

            _addDamagePopup.DoSave();

            _messagesToCheck = new []
            {
                @"Code :82156 There was originally damage recorded on the Bottom side of the cargo item (Damage). Are you sure you want to overwrite this?"
            };

            _popupOKPage.CheckMessage(null, @"Warning(s)", _messagesToCheck);
            _popupOKPage.DoOK();
            _addDamagePopup.DoSave();
            _addDamagePopup.DoBack();

            // <06/03/2025> <NAVRS11> Can be removed 6 months after specified date

            //_addDamagePopup.CheckDamageDetails(damageDetails);

            //damageDetails = new[,]
            //{
            //    { AddDamagePopup_React.ConstDamageClassName, @"DAM" },
            //    { AddDamagePopup_React.ConstPositionClassName, @"Right" },
            //    { AddDamagePopup_React.ConstSeverityClassName, @"4" },
            //    { AddDamagePopup_React.ConstQuantityClassName, @"8" },
            //};
            //_addDamagePopup.CheckDamageDetails(damageDetails);
            //var detailsNotFound = _addDamagePopup.CheckDamageDetails(damageDetails, true);

            //Assert.IsTrue(
            //   Miscellaneous.RemoveCarriageReturnLineFeed(detailsNotFound).Equals(
            //        "damage-detail-tile-damage, DAMdamage-detail-tile-position, Rightdamage-detail-tile-severity, 4damage-detail-tile-quantity, 8"),
            //   "Should not have found a damage row matching 'DAM^Right^8^4'");

            //_addDamagePopup.DoBack();

            //searchPage = new MA_CargoAppsSearchPage(TestContext);
            //searchPage.SearchForCargoID(@"JLG38166");
            //searchPage.SelectCargo(@"JLG38166A02");

            //detailsPage = new MA_CargoAppsDetailsPage(TestContext);
            //detailsPage.DoDamage();

            //_addDamagePopup = new AddDamagePopup_React(TestContext);
            //damageDetails = new[,]
            //{
            //    { AddDamagePopup_React.ConstDamageClassName, @"DAM" },
            //    { AddDamagePopup_React.ConstPositionClassName, @"Right" },
            //    { AddDamagePopup_React.ConstSeverityClassName, @"4" },
            //    { AddDamagePopup_React.ConstQuantityClassName, @"8" },
            //};
            //detailsNotFound = _addDamagePopup.CheckDamageDetails(damageDetails, true);

            //Assert.IsTrue(
            //    Miscellaneous.RemoveCarriageReturnLineFeed(detailsNotFound).Equals(
            //        "damage-detail-tile-damage, DAMdamage-detail-tile-position, Rightdamage-detail-tile-severity, 4damage-detail-tile-quantity, 8"),
            //    "Should not have found a damage row matching 'DAM^Right^8^4'");

            // Step 29 - 30
            LogInto<MTNLogInOutBO>("MTNOLDDAM");
            //LogInto<MTNLogInOutBO>();
            //MTNLogin();

            // Step 31 - 37
            //FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry");
            //cargoEnquiryForm = new CargoEnquiryForm();
            //MTNBase.MainFormToolbarCargoEnquiry1(ref cargoEnquiryForm);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            var cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG38166A02");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            //cargoEnquiryForm.btnPhotos.DoClick();
            //cargoEnquiryForm.DoPhotos();
           //CargoEnquiryPhotosForm cargoPhotosForm = new CargoEnquiryPhotosForm(@"Cargo Photos TT1");
            //MTNControlBase.ValidateValueInEditTable(cargoPhotosForm.tblDetails, @"Description", @"38166 - Damage - Step 10.png");

            cargoEnquiryForm.SetFocusToForm();
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();

            string[] rowsToFind =
{
               "Type^Cams Audit~Details^Damage Location - transaction deleted",
               "Type^Damage Location~Details^Right DAM(4)( x8) @ MKBS01",
               "Type^*** DELETED *** Damage Location~Details^Transaction deleted by MTNOLDDAM.  Reason given: User deleted the damage entry Door RUST(2)( x3)",
               // Removed the Edited Transaction as it containes Date time and it will not be same for all the runs
               //"Type^Edited~Details^allDamageInstancesByPos Door RUST(3)(x2) => Bottom DENT(4)(x4)",
               "Type^Cams Audit~Details^Damage Location - transaction deleted",
              //"Type^Edited~Details^Attached photo: 38166 - Damage - Step 10.png",
              "Type^*** DELETED *** Damage Location~Details^Transaction deleted by MTNOLDDAM.  Reason given: User deleted the damage entry Bottom DENT(4)( x4)",
              "Type^Damaged~Details^Right DAM(4)( x8)"
};

            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(rowsToFind);

            // Step 38
            cargoEnquiryForm.SetFocusToForm();
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4087");

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus, 
                @"Damage Detail (dbl click)", @"Right has Damage (4)");

        }

        void CheckErrorOccursWhenRequiredFieldsNotFilledIn(string[,] damageDetails)
        {
            _addDamagePopup = new AddDamagePopup_React(TestContext);
            _addDamagePopup.SetFields(damageDetails);
            _addDamagePopup.DoAdd();
            _addDamagePopup.CheckErrorValidationAppeared("Error", "Some required fields have not been filled in");
            _addDamagePopup.DoClear();
        }


        

       


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_38166_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>38166</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG38166A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>38166</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG38166A02</id>\n      <isoType>2210</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n       <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>38166</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG38166A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>A</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>38166</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG38166A02</id>\n      <isoType>2210</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1133.981</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n       <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
