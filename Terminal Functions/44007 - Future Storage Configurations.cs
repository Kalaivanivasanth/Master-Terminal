using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Terminal_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44007 : MTNBase
    {

        StorageConfigurationForm _storageConfigurationForm;
        EditStorageDetailsForm _editStorageDetailsForm;
        FreeStorageLoggingForm _freeStorageLoggingForm;

        const string EDIFile1 = "M_44007_CargoOnSiteDelete.xml";
        const string EDIFile2 = "M_44007_CargoOnSiteAdd.xml";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup()
        {
            SetStorageConfigurationFields();

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            CreateDataFile(EDIFile1,
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44007</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44007A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            CreateDataFile(EDIFile2,
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44007</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44007A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();

            // Load the cargo on site
            //FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            EDIOperationsForm ediOperations = new EDIOperationsForm(@"EDI Operations TT1");

            ediOperations.DeleteEDIMessages(EDIOperationsDataType.CargoOnSite, @"44077",
                ediStatus: EDIOperationsStatusType.Loaded);

            ediOperations.LoadEDIMessageFromFile(EDIFile1);
            ediOperations.ChangeEDIStatus(EDIFile1, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);

            ediOperations.LoadEDIMessageFromFile(EDIFile2);
            ediOperations.ChangeEDIStatus(EDIFile2, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);

            ediOperations.CloseForm();
        }


        [TestMethod]
        public void ReevaluateFutureStorage()
        {
            MTNInitialize();
            
            //1 Go to Storage configuration and set IsReefer to yes - setting all just in case.
            SetStorageConfigurationFields(true);
         
            //2. Goto cargo enquiry and re-evaluate free storage days on cargo JLG44007A01
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG44007A01");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44007A01",clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG44007A01"], clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Re-evaluate Free Storage Days");

            //3. check the logging for expected output from configuration change
            _freeStorageLoggingForm = new FreeStorageLoggingForm();
            _freeStorageLoggingForm.FindEntryInLog(
                @"Storage Detail - [3070] Cargo Type: CONT; Is Over Dimensional: N/A; Operator (Item): MSL; Is Hazardous: N/A; Is Hazardous Limited Quantity: N/A; Is Hazardous Or Limited Quantity: N/A; Is Hazardous Not Limited: N/A; Is Damaged: N/A; Is Empty: N/A; Is Reefer: Yes"); // 1535
            _freeStorageLoggingForm.FindEntryInLog(@"No match on Is Reefer");

            _freeStorageLoggingForm.CloseForm();


        }

        void SetStorageConfigurationFields(bool reeferYes = false)
        {
            /*// Wednesday, 30 April 2025 navmh5 Can be removed 6 months after specified date 
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Storage Configuration", forceReset: true);
            _storageConfigurationForm = new StorageConfigurationForm();*/
            _storageConfigurationForm = FormObjectBase.NavigateToAndCreateForm<StorageConfigurationForm>();
            // Thursday, 30 January 2025 navmh5 MTNControlBase.FindClickRowInTable(_storageConfigurationForm.TblStorageConfig, @"Description^TC44007",
            // Thursday, 30 January 2025 navmh5     rowHeight: 16);
            _storageConfigurationForm.TblStorageConfig.FindClickRow(new [] { "Description^TC44007" });
            _storageConfigurationForm.DoEdit();

            // reset the configuration
            //_editStorageDetailsForm = new EditStorageDetailsForm();
            _editStorageDetailsForm = FormObjectBase.CreateForm<EditStorageDetailsForm>();
            _editStorageDetailsForm.TxtCargoType.SetValue("CONT");
            _editStorageDetailsForm.TxtIMEXStatus.SetValue(string.Empty);
            _editStorageDetailsForm.TxtOperatorItem.SetValue("MSL");

            _editStorageDetailsForm.RdoIsDamagedNA.Click();
            _editStorageDetailsForm.RdoIsEmptyNA.Click();
            _editStorageDetailsForm.RdoIsHazardousNA.Click();
            _editStorageDetailsForm.RdoIsHazardousLimitedNA.Click();
            _editStorageDetailsForm.RdoIsHazardousNotLimitedNA.Click();
            _editStorageDetailsForm.RdoIsHazardousOrLimitedNA.Click();
            _editStorageDetailsForm.RdoIsOverDimensionalNA.Click();

            if (reeferYes)
                _editStorageDetailsForm.RdoIsReeferYes.Click();
            else
                _editStorageDetailsForm.RdoIsReeferNA.Click();

            _editStorageDetailsForm.btnOK.DoClick();
            _storageConfigurationForm.CloseForm();
        }
    }

}
