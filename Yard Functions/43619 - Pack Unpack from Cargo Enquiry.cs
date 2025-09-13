using FlaUI.Core.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions.Pack_Unpack;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43619 : MTNBase
    {

        private ConfirmationPackUnpack _confirmationPackUnpack;
        private ConfirmationFormYesNo _confirmationFormYesNo;
        private CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;
        private TransactionMaintenanceForm _transactionMaintenanceForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() { base.TestCleanup(); }

        void MTNInitialize()
        {
            searchFor = @"_43619_";
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void PackUnpackCargoEnquiry()
        {
            MTNInitialize();

            //1. set terminal configuration
            /*string[,] configurationSettings = {
                {@"Settings", @"Bulk Pack/Unpack Complete Transaction Container Processing",@"1", "Checkbox","Up"}
            };
            SetTerminalConfiguration(configurationSettings);*/
            

            //2. navigate to cargo Enquiry
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            //3. Search for cargo
            /*string[,] searchCriteria = {
                {@"Cargo Type", @"",@"ComboBoxEdit",""},
                {@"Cargo ID",@"JLG43619",@"Text",""}
            };
            cargoEnquiryForm.SearchForCargo(searchCriteria);*/
            
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.Blank, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG43619");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43619A01", ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG43619A01"], ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG43619A01 TT1");
            //_cargoEnquiryTransactionForm.btnAdd.DoClick();
            _cargoEnquiryTransactionForm.DoAdd();

            _transactionMaintenanceForm = new TransactionMaintenanceForm(@"Transaction Maintenance TT1");
            _transactionMaintenanceForm.SetTransactionType(@"Pack Complete");
            //MTNControlBase.SetValueInEditTable(_transactionMaintenanceForm.tblEdit, @"Transaction Type",
            //    "Pack Complete", EditRowDataType.ComboBox, doDownArrow: true, searchSubStringTo: 3); 
            return;
            _transactionMaintenanceForm.btnSave.DoClick();
            
            //7. Check the container details
            cargoEnquiryForm.SetFocusToForm();
            DoCheck(cargoEnquiryForm, @"FCL", @"Export");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43619A02",ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG43619A02"], ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Unpack Cargo");
            _confirmationPackUnpack = new ConfirmationPackUnpack(@"Unpack Cargo JLG43619A02");
            //MTNControlBase.SetValue(_confirmationPackUnpack.txtPackContainer, TT1.TerminalArea.MKBS01);
            _confirmationPackUnpack.txtPackContainer.SetValue(TT1.TerminalArea.MKBS01);
            _confirmationPackUnpack.btnOk.DoClick();

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Unpack Complete",ControlType.Window,automationIdMessage: @"2",automationIdYes:@"3",automationIdNo:@"4");
            _confirmationFormYesNo.btnNo.DoClick();

            //9. Check the cargo enquiry details on the container
            cargoEnquiryForm.SetFocusToForm();
            DoCheck(cargoEnquiryForm, @"FCL", @"Export");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43619A01", ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG43619A01"], ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG43619A01 TT1");
            //_cargoEnquiryTransactionForm.btnAdd.DoClick();
            _cargoEnquiryTransactionForm.DoAdd();

            _transactionMaintenanceForm = new TransactionMaintenanceForm(@"Transaction Maintenance TT1");
            _transactionMaintenanceForm.SetTransactionType(@"Unpack Complete");
            //MTNControlBase.SetValueInEditTable(_transactionMaintenanceForm.tblEdit, @"Transaction Type",
            //    "Unpack Complete", EditRowDataType.ComboBox);
            _transactionMaintenanceForm.btnSave.DoClick();

            //11. check the cargo enquiry details
            cargoEnquiryForm.SetFocusToForm();
            DoCheck(cargoEnquiryForm, @"MT", @"Storage");

           
        }

        private static void DoCheck(CargoEnquiryForm form,string expectedStatus, string expectedImex)
        {
            // MTNControlBase.FindClickRowInTable(form.tblData, @"ID^JLG43619A01");
            //form.TblData.FindClickRow(["ID^JLG43619A01"]);
            form.tblData2.FindClickRow(["ID^JLG43619A01"]);
            form.CargoEnquiryGeneralTab();
            var tableValue = MTNControlBase.GetValueInEditTable(form.tblGeneralEdit, @"Status");
            Assert.IsTrue(tableValue == expectedStatus, "Data mismatch on Status field: Expected=Yes; Actual=" + tableValue);
            tableValue = MTNControlBase.GetValueInEditTable(form.tblGeneralEdit, @"IMEX Status");
            Assert.IsTrue(tableValue == expectedImex, "Data mismatch on Status field: Expected=Yes; Actual=" + tableValue);



        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {

            //Delete Cargo Onsite
            CreateDataFileToLoad(@"DeleteCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		  <TestCases>43619</TestCases>\n		  <cargoTypeDescr>ISO Container</cargoTypeDescr>\n		  <id>JLG43619A01</id>\n		  <isoType>220A</isoType>\n		  <operatorCode>MSK</operatorCode>\n		  <locationId>MKBS01</locationId>\n		  <weight>1814.0000</weight>\n		  <imexStatus>Import</imexStatus>\n		  <commodity>GEN</commodity>\n		  <dischargePort>NZAKL</dischargePort>\n		  <voyageCode>MSCK000002</voyageCode>\n		  <messageMode>D</messageMode>\n    </CargoOnSite>\n	<CargoOnSite Terminal='TT1'>\n            <TestCases>43619</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG43619A02</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>MKBS01</locationId>\n            <cargoWeight>454.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>300</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>454.000</tareWeight>\n            <weight>907.000</weight>\n		   <messageMode>D</messageMode>\n        </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //Create Cargo Onsite
            CreateDataFileToLoad(@"CreateCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		  <TestCases>43619</TestCases>\n		  <cargoTypeDescr>ISO Container</cargoTypeDescr>\n		  <id>JLG43619A01</id>\n		  <isoType>220A</isoType>\n		  <operatorCode>MSK</operatorCode>\n		  <locationId>MKBS01</locationId>\n		  <weight>1814.0000</weight>\n		  <imexStatus>Import</imexStatus>\n		  <commodity>GEN</commodity>\n		  <dischargePort>NZAKL</dischargePort>\n		  <voyageCode>MSCK000002</voyageCode>\n		  <messageMode>A</messageMode>\n    </CargoOnSite>\n	<CargoOnSite Terminal='TT1'>\n            <TestCases>43619</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG43619A02</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>454.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>300</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>454.000</tareWeight>\n            <weight>907.000</weight>\n		   <messageMode>A</messageMode>\n        </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");


            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);


        }




    }

}
