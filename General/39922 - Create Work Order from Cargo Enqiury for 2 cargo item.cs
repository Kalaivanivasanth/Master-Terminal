using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Yard_Functions.Work_Order;
using MTNForms.FormObjects.Yard_Functions.Yard_Operations;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNUtilityClasses.Classes;
using ClickType = MTNGlobal.EnumsStructs.ClickType;
using MTNForms.FormObjects.Message_Dialogs;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39922 : MTNBase
    {
        const string TestCaseNumber = @"39922";
        const string CargoId1 = "JLG" + TestCaseNumber + "A01";
        const string CargoId2 = "JLG" + TestCaseNumber + "A02";
        const string TerminalId = "PEL";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            userName = "WOUSER";
            CallJadeScriptToRun(TestContext, $"resetData_{TestCaseNumber}");
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>(userName);
        }

        /// <summary>
        /// To test that a Work Order can be created from the Cargo Enquiry form for multiple cargo items.
        /// </summary>
        [TestMethod]
        public void CreateWorkOrderFromCargoEnquiryFor2CargoItems()
        {
            MTNInitialize();

            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", $"JLG{TestCaseNumber}");
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{CargoId1}", $"ID^{CargoId2}" }, ClickType.ContextClick, multiSelect: true);
            cargoEnquiryForm.tblData2.contextMenu.MenuSelect("Cargo|Create Work Order");

            WorkOrderTasksForm workOrderTasksForm = new WorkOrderTasksForm($"Work Order Details {TerminalId}");
            MTNControlBase.FindClickRowInList(workOrderTasksForm.lstTasks, @"Move");

            workOrderTasksForm.btnOK.DoClick();
            NewWorkOrderForm newWorkOrderForm = new NewWorkOrderForm($"Cargo Work Order New Screen {TerminalId}");

            var date = DateTime.Now;
            newWorkOrderForm.txtReference.SetValue(TestCaseNumber);
            newWorkOrderForm.txtRequiredByDate.SetValue(date.ToString(@"ddMMyyyy"));
            newWorkOrderForm.txtRequiredByTime.SetValue(date.ToString(@"HHmm"));

            newWorkOrderForm.GetTaskDetailsTab(@"Move");
            newWorkOrderForm.cmbToTerminalAreaType.SetValue(@"Block Stack", doDownArrow: true, searchSubStringTo: 5);
            newWorkOrderForm.cmbToTerminalArea.SetValue(@"BS02", doDownArrow: true, downArrowSearchType: SearchType.StartsWith, searchSubStringTo: 3);
            newWorkOrderForm.btnSave.DoClick();

            FormObjectBase.MainForm.OpenWorkOrderEnquiryFromToolbar();
            WorkOrderEnquiryForm workOrderEnquiryForm = new WorkOrderEnquiryForm($"Work Order Enquiry {TerminalId}");

            workOrderEnquiryForm.txtCustomerReference.SetValue(TestCaseNumber);
            workOrderEnquiryForm.lstStatus.BtnAllLeft.DoClick();
            workOrderEnquiryForm.lstStatus.MoveItemsBetweenList(workOrderEnquiryForm.lstStatus.LstLeft, new[] { "User Entry" });
            workOrderEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(workOrderEnquiryForm.tblData,
                // $"Status^User Entry~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            workOrderEnquiryForm.TblData.FindClickRow([$"Status^User Entry~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);
            workOrderEnquiryForm.ContextMenuSelect(@"Set Approved");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            var workOrderId = MTNControlBase.ReturnValueFromTableCell(workOrderEnquiryForm.TblData.GetElement(),
                $"Status^Approved~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}", @"Work Order #", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(workOrderEnquiryForm.tblData,
                // $"Status^Approved~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            workOrderEnquiryForm.TblData.FindClickRow([$"Status^Approved~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);

            workOrderEnquiryForm.ContextMenuSelect(@"Action Task | Move Task #1");

            FormObjectBase.MainForm.OpenYardOperationsFromToolbar();
            YardOperationsForm yardOperationsForm = new YardOperationsForm($"Yard Operations {TerminalId}");
            yardOperationsForm.ShowSearchTabDetails();
            yardOperationsForm.lstTasks.BtnAllLeft.DoClick();

            yardOperationsForm.lstTasks.MoveItemsBetweenList(yardOperationsForm.lstTasks.LstLeft, new[] { "Move" });
            yardOperationsForm.DoSearch();

            yardOperationsForm.ShowToolBarDetails();
            MTNControlBase.SetValue(yardOperationsForm.optWorkList);
            yardOperationsForm.ShowWorkListDetails();

            YardOpsAllocateCargoAndMove(yardOperationsForm, workOrderId, CargoId1, @"4200");
            YardOpsAllocateCargoAndMove(yardOperationsForm, workOrderId, CargoId2, @"2200");

            workOrderEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(workOrderEnquiryForm.tblData,
               // $"Work Order #^{workOrderId}~Status^Complete~Work Order Description^Move Task #1", ClickType.ContextClick);
            workOrderEnquiryForm.TblData.FindClickRow([$"Work Order #^{workOrderId}~Status^Complete~Work Order Description^Move Task #1"], ClickType.ContextClick);

            workOrderEnquiryForm.ContextMenuSelect(@"Delete Work Order");

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Delete");
            confirmationFormYesNo.btnYes.DoClick();
        }

        /// <summary>
        /// Opens Yard Operations form and move the selected cargo to the queued location
        /// </summary>
        /// <param name="yardOperationsForm"></param>
        /// <param name="workOrderId"></param>
        /// <param name="cargoId"></param>
        /// <param name="isoType"></param>
       void YardOpsAllocateCargoAndMove(YardOperationsForm yardOperationsForm, string workOrderId, string cargoId, string isoType)
        {
            // MTNControlBase.FindClickRowInTable(yardOperationsForm.tblDetails,
                // $"Cargo ID^{cargoId}~Task Type^Move~Job ID^{workOrderId}-A~Location Id^BS01~Operator^MSC~ISO Type^{isoType}",
                // ClickType.ContextClick);
            yardOperationsForm.TblDetails.FindClickRow([$"Cargo ID^{cargoId}~Task Type^Move~Job ID^{workOrderId}-A~Location Id^BS01~Operator^MSC~ISO Type^{isoType}"], ClickType.ContextClick);
            yardOperationsForm.ContextMenuSelect(@"Move It|Move Selected");

            WarningErrorForm.CheckErrorMessagesExist($"Warnings for Operations Move {TerminalId}", null, false);
        }

        #region SetUp and Load Data
        void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber +"_";

            // Delete Cargo OnSite
            // Monday, 17 March 2025 navmh5 Can be removed 6 months after specified date CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
            // Monday, 17 March 2025 navmh5 Can be removed 6 months after specified date     "<?xml version='1.0'?> <JMTInternalCargoOnSite>\n<AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='PEL'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>4200</isoType>\n		<dischargePort>NZAKL</dischargePort>\n		<id>JLG39922A01</id>\n		<messageMode>D</messageMode>\n		<imexStatus>Export</imexStatus>\n		<operatorCode>MSC</operatorCode>\n		<voyageCode>VOYWORKORDERS</voyageCode>\n		<weight>4500</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	<CargoOnSite Terminal='PEL'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>2200</isoType>\n		<dischargePort>NZAKL</dischargePort>\n		<id>JLG39922A02</id>\n		<messageMode>D</messageMode>\n		<imexStatus>Export</imexStatus>\n		<operatorCode>MSC</operatorCode>\n		<voyageCode>VOYWORKORDERS</voyageCode>\n		<weight>6000</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	</AllCargoOnSite>\n </JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0'?> <JMTInternalCargoOnSite>\n<AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='PEL'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>4200</isoType>\n		<dischargePort>NZAKL</dischargePort>\n		<id>JLG39922A01</id>\n		<locationId>BS01</locationId>\n		<messageMode>A</messageMode>\n		<imexStatus>Export</imexStatus>\n		<operatorCode>MSC</operatorCode>\n		<voyageCode>VOYWORKORDERS</voyageCode>\n		<weight>4500</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	<CargoOnSite Terminal='PEL'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>2200</isoType>\n		<dischargePort>NZAKL</dischargePort>\n		<id>JLG39922A02</id>\n		<locationId>BS01</locationId>\n		<messageMode>A</messageMode>\n		<imexStatus>Export</imexStatus>\n		<operatorCode>MSC</operatorCode>\n		<voyageCode>VOYWORKORDERS</voyageCode>\n		<weight>6000</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	</AllCargoOnSite>\n </JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
        #endregion
    }
}
