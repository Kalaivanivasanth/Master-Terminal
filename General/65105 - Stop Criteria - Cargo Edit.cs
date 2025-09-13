using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase65105 : MTNBase
    {
        private const string CargoId = "65105-CUSTOMS1";
        private const string MarkB = "65105_B"; 
        private const string MarkC = "65105_C";
     
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void StopCriteriaCargoEdit()
        {
            MTNInitialize();

            // Initial search and check stops
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", "Metal", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", "On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.GetStatusTable("4093");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus, @"Stops (dbl click)", @"STOP_65105_1, STOP_65105_3");

            // Edit the cargo and check stops
            EditCargoAndCheckStops(MarkB, @"STOP_65105_2, STOP_65105_3");
            EditCargoAndCheckStops(MarkC, @"STOP_65105_3");

            // Check transactions
            cargoEnquiryForm.DoViewTransactions();
            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for " + CargoId + " TT1");
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[]
            {
                "Type^Stop Set~Details^STOP_65105_1",
                "Type^Stop Set~Details^STOP_65105_3",
                "Type^Stop Cancelled~Details^STOP_65105_1",
                "Type^Stop Set~Details^STOP_65105_2",
                "Type^Stop Cancelled~Details^STOP_65105_2",

            });

        }

        /// <summary>
        /// Edit cargo and check stops
        /// </summary>
        /// <param name="mark">Mark value which needs to be changed</param>
        /// <param name="expectedStops">Stop should be shown for resective mark value</param>
        private void EditCargoAndCheckStops(string mark, string expectedStops)
        {
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            cargoEnquiryForm.DoEdit();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Mark", mark);
            cargoEnquiryForm.DoSave();
            cargoEnquiryForm.GetStatusTable("4093");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus, @"Stops (dbl click)", expectedStops);
        }

        private void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = "_65105_";

            // Delete Cargo OnShip
            CreateDataFileToLoad("DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>METAL</cargoTypeDescr>\n            <id>65105-CUSTOMS1</id>\n            <voyageCode>VOY_CUSTOMS</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>NZCHC</dischargePort>\n            <locationId>BULK</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>EXPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n                 <mark>65105_C</mark>\n                <countryOfOriginCode>US</countryOfOriginCode>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnShip
            CreateDataFileToLoad("CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>METAL</cargoTypeDescr>\n            <id>65105-CUSTOMS1</id>\n            <voyageCode>VOY_CUSTOMS</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>NZCHC</dischargePort>\n            <locationId>BULK</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>EXPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n                 <mark>65105_A</mark>\n                <countryOfOriginCode>US</countryOfOriginCode>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }
}
