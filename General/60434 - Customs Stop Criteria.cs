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
    public class TestCase60434 : MTNBase
    {
        private const string CargoId = "60434-CUSTOMS1";
        
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
        public void CustomsStopCriteria()
        {
            MTNInitialize();

            // Initial search and status check
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", "Metal", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", "On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            VerifyCustomsStatus("Customs");

            //Check the stop applied to cargo
            cargoEnquiryForm.GetStatusTable("4093");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblStatus, @"Stops (dbl click)", @"STOP_60434");

            // Edit the cargo and change the destination, then status check
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            cargoEnquiryForm.DoEdit();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Destination", @"AAA (US) Attalla", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSave();
            VerifyCustomsStatus("Free");

        }
        
        /// <summary>
        /// To search a cargo and check its customs status in cargo enquiry screen
        /// </summary>
        /// <param name="expectedStatus">status might be free, customs, cleared</param>
        private void VerifyCustomsStatus(string expectedStatus)
        {
                cargoEnquiryForm.SetFocusToForm();
                cargoEnquiryForm.tblData2.FindClickRow(new[]
                    { $"ID^60434-CUSTOMS1~Location ID^BULK~Total Quantity^1~Customs Status^{expectedStatus}" });
        }
        
        private void SetupAndLoadInitializeData(TestContext testContext)
        {
                fileOrder = 1;
                searchFor = "_60434_";

                // Delete Cargo OnShip
                CreateDataFileToLoad("DeleteCargoOnSite.xml",
                    "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>METAL</cargoTypeDescr>\n            <id>60434-CUSTOMS1</id>\n            <voyageCode>VOY_CUSTOMS</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>NZCHC</dischargePort>\n            <locationId>BULK</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>EXPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n                 <mark>60434</mark>\n                <countryOfOriginCode>US</countryOfOriginCode>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

                // Create Cargo OnShip
                CreateDataFileToLoad("CreateCargoOnSite.xml",
                    "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>METAL</cargoTypeDescr>\n            <id>60434-CUSTOMS1</id>\n            <voyageCode>VOY_CUSTOMS</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>NZCHC</dischargePort>\n            <locationId>BULK</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>EXPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n                 <mark>60434</mark>\n                <countryOfOriginCode>US</countryOfOriginCode>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

                // call Jade to load file(s)
                CallJadeToLoadFiles(testContext);
        }
        
    }
}
