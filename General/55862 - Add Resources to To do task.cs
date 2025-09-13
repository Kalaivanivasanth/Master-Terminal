using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase55862: MTNBase
    {
        private const string TestCaseNumber = @"39585";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            searchFor = @"_" + TestCaseNumber + "_";
            
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void AddResourcesTodoTask()
        {
            MTNInitialize();

            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"MTN55862A01");
            cargoEnquiryForm.DoSearch();
            
            cargoEnquiryForm.tblData2.GetElement().RightClick();
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Add Tasks...");

            ToDoTaskForm toDoTaskForm = new ToDoTaskForm(@"MTN55862A01 TT1");
            toDoTaskForm.AddCompleteTask(@"Lashing", toDoTaskForm.btnSaveAndClose);
            ConfirmationFormOK confirmationFormOK = new ConfirmationFormOK(@"Tasks Added");
            confirmationFormOK.btnOK.DoClick();

            cargoEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^MTN55862A01", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^MTN55862A01"], clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Complete Tasks...");
            toDoTaskForm = new ToDoTaskForm(@"MTN55862A01 TT1");
            toDoTaskForm.AddCompleteTask(@"Lashing", toDoTaskForm.btnSaveAndClose);

            ToDoTaskResourceUsageForm toDoTaskResourceUsageForm = new ToDoTaskResourceUsageForm(@"Lashing MTN55862A01 - Resource Usage TT1");
            toDoTaskResourceUsageForm.btnAdd.DoClick();
            toDoTaskResourceUsageForm.cmbResourceType.SetValue(@"Straps");
            toDoTaskResourceUsageForm.txtUnit.SetValue(@"10");

            toDoTaskResourceUsageForm.btnUpdate.DoClick();
            toDoTaskResourceUsageForm.btnSave.DoClick();

            confirmationFormOK = new ConfirmationFormOK(@"Tasks Completed");
            confirmationFormOK.btnOK.DoClick();
            
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.MainToolbar, (int)CargoEnquiryForm.Toolbar.MainToolbar.ViewTransactions, "View Transactions");

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for MTN55862A01 TT1");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Lashed~Details^Todo Task Completed: Lashing  Resources used: 10 per metre [ Straps ]",
                 // ClickType.Click, rowHeight: 17);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Lashed~Details^Todo Task Completed: Lashing  Resources used: 10 per metre [ Straps ]",
                 // ClickType.RightClick, rowHeight: 17);
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow([
                "Type^Lashed~Details^Todo Task Completed: Lashing  Resources used: 10 per metre [ Straps ]",
                "Type^Lashed~Details^Todo Task Completed: Lashing  Resources used: 10 per metre [ Straps ]"
            ], ClickType.ContextClick);
            cargoEnquiryTransactionForm.ContextMenuSelect(@"Resources...");

            toDoTaskResourceUsageForm = new ToDoTaskResourceUsageForm(@"MTN55862A01 / Lashing - Resource Usage TT1");
           // toDoTaskResourceUsageForm.tblResources.FindClickRow(@"Resource^Straps");
            toDoTaskResourceUsageForm.TblResources.FindClickRow(["Resource^Straps"]);

            toDoTaskResourceUsageForm.cmbResourceType.SetValue(@"Straps");
            toDoTaskResourceUsageForm.txtUnit.SetValue(@"15");

            toDoTaskResourceUsageForm.btnUpdate.DoClick();
            toDoTaskResourceUsageForm.btnSave.DoClick();

            cargoEnquiryTransactionForm.DoRefresh();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Lashed~Details^Todo Task Completed: Lashing  Resources used: 15 per metre [ Straps ]",
                 // ClickType.Click, rowHeight: 17);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Lashed~Details^Todo Task Completed: Lashing  Resources used: 15 per metre [ Straps ]",
                 // ClickType.RightClick, rowHeight: 17);
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow([
                "Type^Lashed~Details^Todo Task Completed: Lashing  Resources used: 15 per metre [ Straps ]",
                "Type^Lashed~Details^Todo Task Completed: Lashing  Resources used: 15 per metre [ Straps ]"
            ], ClickType.Click);
            cargoEnquiryTransactionForm.ContextMenuSelect(@"View Audit", validateOnly: true);
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0'?> \n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>2200</isoType>\n		<dischargePort>NZAKL</dischargePort>\n		<id>MTN55862A01</id>\n		<locationId>MKBS01</locationId>\n		<messageMode>D</messageMode>\n		<imexStatus>Export</imexStatus>\n		<operatorCode>MSC</operatorCode>\n		<voyageCode>MSCK000010</voyageCode>\n		<weight>4500</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	</AllCargoOnSite>\n </JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0'?> \n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>2200</isoType>\n		<dischargePort>NZAKL</dischargePort>\n		<id>MTN55862A01</id>\n		<locationId>MKBS01</locationId>\n		<messageMode>A</messageMode>\n		<imexStatus>Export</imexStatus>\n		<operatorCode>MSC</operatorCode>\n		<voyageCode>MSCK000010</voyageCode>\n		<weight>4500</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	</AllCargoOnSite>\n </JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }
}
