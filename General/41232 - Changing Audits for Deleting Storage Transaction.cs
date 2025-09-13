using System.ServiceModel.Security;
using DataObjects.LogInOutBO;
using FlaUI.Core.Definitions;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41232 : MTNBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            searchFor = @"_41232_";
            
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void ChangingAuditsForDeletedStorageTransactions()
        {
            MTNInitialize();

            // Step 5
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG41232A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);

            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                // @"Location ID^MKBS07~ID^JLG41232A01~Cargo Type^ISO Container~Voyage^MSCK000002~IMEX Status^Export~Type^2200");
            cargoEnquiryForm.tblData2.FindClickRow(["Location ID^MKBS07~ID^JLG41232A01~Cargo Type^ISO Container~Voyage^MSCK000002~IMEX Status^Export~Type^2200"]);

            // Step 6
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^Received - Road", ClickType.ContextClick);
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(["Type^Received - Road"], ClickType.ContextClick);
            cargoEnquiryTransactionForm.ContextMenuSelect(@"Delete");

            // Step 8
            ConfirmationFormOK confirmationFormOK = new ConfirmationFormOK(@"Confirm Delete");
            confirmationFormOK.CheckMessageMatch(@"Are you sure you want to delete Transaction 'Received - Road'?");
            confirmationFormOK.btnOK.DoClick();

            // Step 9
            ConfirmationFormOKwithText confirmationFormOKwithText = new ConfirmationFormOKwithText(@"Enter the Reason value:", ControlType.Pane,@"4005");
            //confirmationFormOKwithText.SetValue(confirmationFormOKwithText.txtInput, @"Testing Reason");
            confirmationFormOKwithText.txtInput.SetValue(@"Testing Reason");
            confirmationFormOKwithText.btnOK.DoClick();

            // Step 10
            cargoEnquiryTransactionForm.SetFocusToForm();
            //MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
            //    @"Type^*** DELETED *** Received - Road~User^SUPERUSER~Details^Transaction deleted by USERDWAT.  Reason given: Testing Reason");
           // cargoEnquiryTransactionForm.tblTransactions1.FindClickRow(
            //"Type^*** DELETED *** Received - Road~User^SUPERUSER~Details^Transaction deleted by USERDWAT.  Reason given: Testing Reason");
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow([
                "Type^*** DELETED *** Received - Road~User^SUPERUSER~Details^Transaction deleted by USERDWAT.  Reason given: Testing Reason"
            ]);
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG41232A01</id>\n         <operatorCode>MSL</operatorCode>\n         <weight>5000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>GEN</commodity>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000002</voyageCode>\n         <isoType>2200</isoType>\n		 <locationId>MKBS07</locationId>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG41232A01</id>\n         <operatorCode>MSL</operatorCode>\n         <weight>5000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>GEN</commodity>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000002</voyageCode>\n         <isoType>2200</isoType>\n		 <locationId>MKBS07</locationId>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }
  

}
