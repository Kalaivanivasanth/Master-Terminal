using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41243 : MTNBase
    {

        ToDoTaskForm _toDoTaskForm;
        ConfirmationFormOK _confirmationFormOK;
        CargoWeighForm _cargoWeighForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}


        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            searchFor = @"_41243_";
            
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void CompleteWeighTaskOnCargo()
        {
            MTNInitialize();
            
            string fieldValue = null;
            // 1. find cargo JLG41243A01
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.BagOfSand, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG41243A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG41243A01",clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG41243A01"], clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Add Tasks...");

            _toDoTaskForm = new ToDoTaskForm(@"JLG41243A01 TT1");
            _toDoTaskForm.AddCompleteTask(@"Requires Weighing", _toDoTaskForm.btnSaveAndClose);
            _confirmationFormOK = new ConfirmationFormOK(@"Tasks Added");
            _confirmationFormOK.btnOK.DoClick();
 
            // 3. context menu navigate to complete the weigh to do task
            cargoEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG41243A01", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG41243A01"], clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Complete Tasks...");
            _toDoTaskForm = new ToDoTaskForm(@"JLG41243A01 TT1");
            _toDoTaskForm.AddCompleteTask(@"Requires Weighing", _toDoTaskForm.btnSave);

       
            // 4. check the weigh form has the correct data in it and then update the cargo weight to be 500
            _cargoWeighForm = new CargoWeighForm(@"Complete Weigh Task for JLG41243A01 TT1");
            //cargoWeighForm.tblWeight.Click();
            _cargoWeighForm.WeightFocus();
            //MTNControlBase.ValidateElementText(cargoWeighForm.txtWeight, "Weight", "2204.624");
            //cargoWeighForm.txtWeight.ValidateText("2204.624");*/
            _cargoWeighForm.mtWeight.ValidateValueAndType("2204.624");
            //cargoWeighForm.tblTareWeight.Click();
            _cargoWeighForm.TareWeightFocus();
            //MTNControlBase.ValidateElementText(cargoWeighForm.txtTareWeight, "Tare Weight", "1322.775");
            _cargoWeighForm.mtTareWeight.ValidateValueAndType("1322.775");
            //cargoWeighForm.tblCargoWeight.Click();
            _cargoWeighForm.CargoWeightFocus();
            //MTNControlBase.SetValue(cargoWeighForm.txtCargoWeight, @"500");
            _cargoWeighForm.mtCargoWeight.SetValueAndType("500");
            _cargoWeighForm.btnOK.DoClick();

            // 5. complete the weigh to do task
            _confirmationFormOK = new ConfirmationFormOK(@"Tasks Completed");
            _confirmationFormOK.btnOK.DoClick();
            //FormObjectBase.ClickButton(toDoTaskForm.btnClose);
            _toDoTaskForm.btnClose.DoClick();

            cargoEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG41243A01");
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG41243A01"]);
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            // validate the weight from the to do task in cargo enquiry - doesn't really matter where the cursor clicks
            fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Tare Weight");
            string textToCheck = @"1322.775 lbs; 0.591 LT; 0.600 MT; 0.661 ST; 600 kg";
            Assert.IsTrue(fieldValue == textToCheck, @"Field: Tare Weight has a value of " + fieldValue.ToUpper() + @" and should equal: " + textToCheck);
            
            fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Cargo Weight"); 
            textToCheck = @"500.000 lbs; 0.223 LT; 0.227 MT; 0.250 ST; 227 kg";
            Assert.IsTrue(fieldValue == textToCheck, @"Field: Cargo Weight has a value of " + fieldValue.ToUpper() + @" and should equal: " + textToCheck);

            fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Total Weight");
            textToCheck = @"1822.775 lbs; 0.814 LT; 0.827 MT; 0.911 ST; 827 kg";
            Assert.IsTrue(fieldValue == textToCheck, @"Field: Total Weight has a value of " + fieldValue.ToUpper() + @" and should equal: " + textToCheck);
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            //fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> \n <JMTInternalCargoOnSite \n xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'> \n <AllCargoOnSite> \n    <operationsToPerform>Verify;Load To DB</operationsToPerform> \n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses> \n    <CargoOnSite Terminal='TT1'> \n      <TestCases>41243</TestCases> \n      <cargoTypeDescr>Bag of Sand</cargoTypeDescr> \n      <product>SMSAND</product> \n      <id>JLG41243A01</id> \n      <operatorCode>MSL</operatorCode> \n      <locationId>GCARA1 SMALL_SAND</locationId> \n      <weight>1000.0000</weight> \n	  <tareWeight>600.0000</tareWeight> \n      <imexStatus>Export</imexStatus> \n      <dischargePort>NZNPE</dischargePort> \n      <voyageCode>MSCK000002</voyageCode> \n      <totalQuantity>100</totalQuantity> \n      <commodity>SANC</commodity> \n	  <messageMode>D</messageMode> \n    </CargoOnSite> \n  </AllCargoOnSite> \n</JMTInternalCargoOnSite> \n\n");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> \n <JMTInternalCargoOnSite \n xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'> \n <AllCargoOnSite> \n    <operationsToPerform>Verify;Load To DB</operationsToPerform> \n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses> \n    <CargoOnSite Terminal='TT1'> \n      <TestCases>41243</TestCases> \n      <cargoTypeDescr>Bag of Sand</cargoTypeDescr> \n      <product>SMSAND</product> \n      <id>JLG41243A01</id> \n      <operatorCode>MSL</operatorCode> \n      <locationId>GCARA1 SMALL_SAND</locationId> \n      <weight>1000.0000</weight> \n     <tareWeight>600.0000</tareWeight> \n      <imexStatus>Export</imexStatus> \n       <dischargePort>NZNPE</dischargePort> \n     <voyageCode>MSCK000002</voyageCode> \n      <totalQuantity>100</totalQuantity> \n     <commodity>SANC</commodity> \n     <messageMode>A</messageMode> \n    </CargoOnSite> \n  </AllCargoOnSite> \n</JMTInternalCargoOnSite> \n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }


    }

}
