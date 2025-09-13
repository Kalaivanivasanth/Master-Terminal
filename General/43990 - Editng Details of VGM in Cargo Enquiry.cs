using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects.Message_Dialogs;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    /// --------------------------------------------------------------------------------------------------
    /// Date         Person          Reason for change
    /// ==========   ======          =================================
    /// 07/03/2022   navmh5          Initial creation
    /// --------------------------------------------------------------------------------------------------
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43990 : MTNBase
    {
        CargoEnquiryConfigureForm _cargoEnquiryConfigureForm;

        const string TestCaseNumber = @"43990";
        const string CargoId = "NAV" + TestCaseNumber + "A01";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
      
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            searchFor = @"_" + TestCaseNumber +"_";
            userName = $"USER{TestCaseNumber}";
            
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>(userName);
        }


        [TestMethod]
        public void ConfigureVGM()
        {
            MTNInitialize();
            
            // Step 2 - 4
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            
            // Set the rows to the Ship Ops tab
            DoConfigureFormChanges("Ship Ops");
            
            cargoEnquiryForm.SetFocusToForm();

            string[,] detailsToChange = new string[,]
            {
                {"Is Weight Certified", "1"},
                {"Weight Certifying Authority", "TEST TEAM"},
                {"Weight Certifying Person" ,"TESTER"}
            };
            SetValidateValuesInEditTable(detailsToChange);
            
            detailsToChange = new string[,]
            {
                {"Is Weight Certified", "0"},
                {"Weight Certifying Authority", ""},
                {"Weight Certifying Person" ,""}
            };
            SetValidateValuesInEditTable(detailsToChange);
            
            // Set the rows back to the General Tab
            DoConfigureFormChanges("General");
            
        }

        /// <summary>
        /// Move 'Weight Certifying Authority' / 'Weight Certifying Person' to the required Tab
        /// </summary>
        /// <param name="tabToSetRowsOn">Tab to set the feilds to</param>
        void DoConfigureFormChanges(string tabToSetRowsOn)
        {
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            cargoEnquiryForm.tblData2.GetElement().RightClick();
            cargoEnquiryForm.ContextMenuSelect(@"Configure...");

            _cargoEnquiryConfigureForm =
                new CargoEnquiryConfigureForm($"Cargo Enquiry Config for User {TestCaseNumber} {terminalId}");

            _cargoEnquiryConfigureForm.SelectDetailsInSheetsTable(tabToSetRowsOn);
            _cargoEnquiryConfigureForm.SelectWeightCertifyingRowsInAttributesTable();
            _cargoEnquiryConfigureForm.BtnSave.DoClick();
            _cargoEnquiryConfigureForm.BtnClose.DoClick();
        }

        /// <summary>
        /// Edit / Validate details in Edit Table
        /// </summary>
        /// <param name="detailsToChange">Details to edit / validate</param>
        void SetValidateValuesInEditTable(string[,] detailsToChange)
        {
            //cargoEnquiryForm.btnEdit.DoClick();
            cargoEnquiryForm.DoEdit();

            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, detailsToChange[0, 0],
                detailsToChange[0, 1], EditRowDataType.CheckBox);

            cargoEnquiryForm.CargoEnquiryShipOpsTab();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblShipOpsEdit, detailsToChange[1, 0],
                detailsToChange[1, 1]);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblShipOpsEdit, detailsToChange[2, 0],
                detailsToChange[2, 1]);
            //cargoEnquiryForm.btnSave.DoClick();
            cargoEnquiryForm.DoSave();

            warningErrorForm = new WarningErrorForm($"Warnings for Tracked Item Update {terminalId}");
            warningErrorForm.btnSave.DoClick();
            

            // Validate entered details
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblShipOpsEdit, detailsToChange[1,0],
                detailsToChange[1,1]);
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblShipOpsEdit, detailsToChange[2, 0],
         detailsToChange[2, 1]);

            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, detailsToChange[0, 0],
         detailsToChange[0, 1]);
        }

        /// <summary>
        /// Setup and Load initialization data via EDI Operations
        /// </summary>
        /// <param name="testContext">TestContext</param>
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>NAV43990A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>ACL</operatorCode>\n            <locationId>MKBS01</locationId>\n            <weight>24233.0000</weight>\n			<imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GEN</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");
            
            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>NAV43990A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>ACL</operatorCode>\n            <locationId>MKBS01</locationId>\n            <weight>24233.0000</weight>\n			<imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GEN</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
