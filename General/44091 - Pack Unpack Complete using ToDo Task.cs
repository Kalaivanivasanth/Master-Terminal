using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44091 : MTNBase
    {

        TerminalConfigForm _terminalConfigForm;
        ToDoTaskForm _toDoTaskForm;
        ConfirmationFormOK _confirmationFormOk;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

        const string EDIFile1 = "M_44091_CargoOnsiteDelete.xml";
        const string EDIFile2 = "M_44091_CargoOnsiteAdd.xml";
        const string EDIFile3 = "M_44091_ToDoTaskAdd.xml";
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize()  {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        void MTNInitialize()
        {
            searchFor = @"_44091_";
            
            CreateDataFile(EDIFile1,
               "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		  <TestCases>44091</TestCases>\n		  <cargoTypeDescr>ISO Container</cargoTypeDescr>\n		  <id>JLG44091A01</id>\n		  <isoType>2200</isoType>\n		  <operatorCode>MSC</operatorCode>\n		  <locationId>MKBS01</locationId>\n		  <weight>6000</weight>\n		  <imexStatus>Import</imexStatus>\n		  <commodity>MT</commodity>\n		  <dischargePort>USJAX</dischargePort>\n		  <voyageCode>MSCK000002</voyageCode>\n		  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            CreateDataFile(EDIFile2,
              "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		  <TestCases>44091</TestCases>\n		  <cargoTypeDescr>ISO Container</cargoTypeDescr>\n		  <id>JLG44091A01</id>\n		  <isoType>2200</isoType>\n		  <operatorCode>MSC</operatorCode>\n		  <locationId>MKBS01</locationId>\n		  <weight>6000</weight>\n		  <imexStatus>Import</imexStatus>\n		  <commodity>MT</commodity>\n		  <dischargePort>USJAX</dischargePort>\n		  <voyageCode>MSCK000002</voyageCode>\n		  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            CreateDataFile(EDIFile3,
              "<?xml version='1.0'?> <JMTInternalToDoTask xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalToDoTask.xsd'>\n	<AllToDoTask>\n		<ToDoTask>\n			<isComplete>false</isComplete>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<id>JLG44091A01</id>\n			<messageMode>A</messageMode>\n			<taskCode>Pack</taskCode>\n		</ToDoTask>\n	</AllToDoTask>\n</JMTInternalToDoTask>\n\n");

            LogInto<MTNLogInOutBO>();

        }


        [TestMethod]
        public void PackUnpackToDoTask()
        {
            
            MTNInitialize();

            // Thursday, 30 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            EDIOperationsForm ediOperations = new EDIOperationsForm(@"EDI Operations TT1");
            
            ediOperations.DeleteEDIMessages(EDIOperationsDataType.CargoOnSite, @"44091", ediStatus: EDIOperationsStatusType.Loaded);
            ediOperations.DeleteEDIMessages(EDIOperationsDataType.ToDoTask, @"44091", ediStatus: EDIOperationsStatusType.Loaded);
            
            ediOperations.LoadEDIMessageFromFile(EDIFile1);
            ediOperations.ChangeEDIStatus(EDIFile1, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);
            
            ediOperations.LoadEDIMessageFromFile(EDIFile2);
            ediOperations.ChangeEDIStatus(EDIFile2, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);
            
            ediOperations.LoadEDIMessageFromFile(EDIFile3);
            ediOperations.ChangeEDIStatus(EDIFile3, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);
            
            ediOperations.CloseForm();

            // Thursday, 30 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            _terminalConfigForm = new TerminalConfigForm();
            _terminalConfigForm.SetTerminalConfiguration(@"Settings",
                @"Bulk Pack/Unpack Complete Transaction Container Processing", @"1",
                rowDataType: EditRowDataType.CheckBox);
            _terminalConfigForm.CloseForm();

            //1. Find cargo JLG44091A01 in cargo enquiry
            // Thursday, 30 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG44091A01");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
   
           // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44091A01");
           cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG44091A01" });
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            //2. Check Status and Imex status fields are as expected
            string strStatus = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit,@"Status"); 
            Assert.IsTrue(strStatus == "MT", @"Status expected to be MT, actual value is " + strStatus);
            
            string strIMEXStatus = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"IMEX Status");
            Assert.IsTrue(strIMEXStatus == "Import", @"IMEX Status expected to be Import, actual value is " + strIMEXStatus);

            //3. Complete the Pack ToDo Task 
            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44091A01", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG44091A01" }, clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Complete Tasks...");

            _toDoTaskForm = new ToDoTaskForm(@"JLG44091A01 TT1");
            _toDoTaskForm.AddCompleteTask(@"PACK_44091", _toDoTaskForm.btnSaveAndClose);

            _confirmationFormOk = new ConfirmationFormOK(@"Tasks Completed");
            _confirmationFormOk.btnOK.DoClick();

            //4. check the Pack complete transaction has been created
            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44091A01", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG44091A01" }, clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");

            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Pack Complete~Details^Todo Task Completed: PACK_44091");
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(["Type^Pack Complete~Details^Todo Task Completed: PACK_44091"]);
            _cargoEnquiryTransactionForm.CloseForm();

            //5. Check Status and Imex status fields are as expected
            strStatus = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Status");
            Assert.IsTrue(strStatus == "FCL", @"Status expected to be FCL, actual value is " + strStatus);

            strIMEXStatus = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"IMEX Status");
            Assert.IsTrue(strIMEXStatus == "Export", @"IMEX Status expected to be Export, actual value is " + strIMEXStatus);

            // Thursday, 30 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44091A01", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG44091A01" }, clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Add Tasks...");

            //6. Add Unpack complete and check transactions
            _toDoTaskForm = new ToDoTaskForm(@"JLG44091A01 TT1");
            _toDoTaskForm.AddCompleteTask(@"UNPACK_44091", _toDoTaskForm.btnSaveAndClose);

            _confirmationFormOk = new ConfirmationFormOK(@"Tasks Added");
            _confirmationFormOk.btnOK.DoClick();

            // Thursday, 30 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44091A01", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG44091A01" }, clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");

            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Unpack Complete~Details^Todo Task Created:UNPACK_44091");
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(["Type^Unpack Complete~Details^Todo Task Created:UNPACK_44091"]);
            _cargoEnquiryTransactionForm.CloseForm();

            //7. Check Status and Imex status fields are as expected
            strStatus = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Status");
            Assert.IsTrue(strStatus == "MT", @"Status expected to be MT, actual value is " + strStatus);

            strIMEXStatus = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"IMEX Status");
            Assert.IsTrue(strIMEXStatus == "Storage", @"IMEX Status expected to be Storage, actual value is " + strIMEXStatus);


        }



    }

}
