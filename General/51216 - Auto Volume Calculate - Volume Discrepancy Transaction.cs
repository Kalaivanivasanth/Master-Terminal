using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase51216 : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}
     
     
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();
            SetupAndLoadInitializeData(TestContext);
        }

        [TestMethod]
        public void AutoVolumeCaluculate_VolumeDiscrepancyTransaction()
        {
            MTNInitialize();

            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry");
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.BagOfSand, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG51216A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();

            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG51216A01", ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG51216A01" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo | Auto Volume Calculate");

            AutoVolumeCalculateForm autoVolumeCalculateForm = new AutoVolumeCalculateForm($"Auto Volume Calculate {terminalId}");
            autoVolumeCalculateForm.mtNewLength.SetValueAndType("150");
            autoVolumeCalculateForm.mtNewWidth.SetValueAndType("150");
            autoVolumeCalculateForm.mtNewHeight.SetValueAndType("150");
            autoVolumeCalculateForm.cmbReason.SetValue("Testing");
            autoVolumeCalculateForm.btnOK.DoClick();

            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.DoViewTransactions();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            // Monday, 3 February 2025 navmh5 Leaving for the moment as not finding details so need to check why
            cargoEnquiryTransactionForm.GetTransactionTab();
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, 
             // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date @"Type^Volume Discrepancy~Details^Old Length 100 cm, Change 50 cm, New Length 150 cmOld Width 100 cm, Change 50 cm, New Width 150 cmOld Height 100 cm, Change 50 cm, New Height 150 cmOld Weight 17196.070 lbs, Change 0 lbs, New Weight 17196.070 lbs");
           cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[]
           {
               "Type^Volume Discrepancy~Details^Old Length 100 cm, Change 50 cm, New Length 150 cm\r\nOld Width 100 cm, Change 50 cm, New Width 150 cm\r\nOld Height 100 cm, Change 50 cm, New Height 150 cm\r\nOld Weight 17196.070 lbs, Change 0 lbs, New Weight 17196.070 lbs"
           });

        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_51216_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <TestCases>51216</TestCases>\n         <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n     <product>SMSAND</product>\n    <id>JLG51216A01</id>\n                 <operatorCode>MSL</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>7800.0000</weight>\n         <imexStatus>Import</imexStatus>\n         <commodity>GEN</commodity>\n         <voyageCode>MSCK000002</voyageCode>\n         <dischargePort>NZAKL</dischargePort>\n	<quantity>8</quantity>\n	<totalQuantity>8</totalQuantity>\n  <height>100</height>\n   <length>100</length>\n   <width>100</width>\n   <messageMode>D</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <TestCases>51216</TestCases>\n         <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n     <product>SMSAND</product>\n     <id>JLG51216A01</id>\n                <operatorCode>MSL</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>7800.0000</weight>\n         <imexStatus>Import</imexStatus>\n         <commodity>GEN</commodity>\n         <voyageCode>MSCK000002</voyageCode>\n         <dischargePort>NZAKL</dischargePort>\n		<quantity>8</quantity>\n   <totalQuantity>8</totalQuantity>\n   <height>100</height>\n   <length>100</length>\n   <width>100</width>\n   <messageMode>A</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }
}
