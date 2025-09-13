using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Yard_Functions.Work_Order;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using FlaUI.Core.Input;
using System;
using FlaUI.Core.WindowsAPI;
using MTNForms.FormObjects.Message_Dialogs;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Work_Order_Enquiry
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase64267 : MTNBase
    {
        WorkOrderEnquiryForm _workOrderEnquiryForm;
        WorkOrderOperationsForm _workOrderOperationsForm;

        private const string TestCaseNumber = @"64267";
        const string TerminalId = @"PEL";
        const string UserName = @"WOUSER";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"reset_WorkOrders_64267");
            LogInto<MTNLogInOutBO>(UserName);
        }

        /// <summary>
        /// To test that the user is able to un-approve the already actioned work order and delete the work order jobs.
        /// </summary>
        [TestMethod]
        public void SetUnapproveTheActionedWorkOrder()
        {
            MTNInitialize();

            // Open Work Order Enquiry
            FormObjectBase.MainForm.OpenWorkOrderEnquiryFromToolbar();
            _workOrderEnquiryForm = new WorkOrderEnquiryForm($"Work Order Enquiry {TerminalId}");

            _workOrderEnquiryForm.txtCustomerReference.SetValue(TestCaseNumber);
            _workOrderEnquiryForm.lstStatus.BtnAllLeft.DoClick();
            _workOrderEnquiryForm.lstStatus.MoveItemsBetweenList(_workOrderEnquiryForm.lstStatus.LstLeft, new [] { @"User Entry" });
            
            _workOrderEnquiryForm.GetGateSearchTab();
            _workOrderEnquiryForm.txtRequiredDateFrom.SetValue("");
            _workOrderEnquiryForm.txtRequestedByDateFrom.SetValue("");

            _workOrderEnquiryForm.DoSearch();
            // Approve and action the task
            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date     @"Status^User Entry~Work Order Description^Receive Cargo - Road Task #1",
            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date     ClickType.ContextClick);
            FindRowDoContextMenu("Status^User Entry~Work Order Description^Receive Cargo - Road Task #1", @"Set Approved");

            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date  MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date     @"Status^Approved~Work Order Description^Receive Cargo - Road Task #1",
             // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date    ClickType.ContextClick);
            FindRowDoContextMenu(@"Status^Approved~Work Order Description^Receive Cargo - Road Task #1", "Action Task|Receive Cargo - Road Task #1");

            var workOrderId = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
                @"Status^Approved~Work Order Description^Receive Cargo - Road Task #1", @"Work Order #", ClickType.None);

            // Into superuser mode
            PutIntoSuperUserMode();

            // Find the Work Order job
            _workOrderEnquiryForm.DoWorkOrderOperations();

            _workOrderOperationsForm = new WorkOrderOperationsForm($"Work Order Operations {TerminalId}");
            _workOrderOperationsForm.cmbOperationMode.SetValue(@"Road");
            _workOrderOperationsForm.chkJobSheet.DoClick(false);
            _workOrderOperationsForm.chkFilterCriteria.DoClick();
            _workOrderOperationsForm.GetFilterCriteria();
            _workOrderOperationsForm.cmbShiftFrom.SetValue(@" ");
            _workOrderOperationsForm.cmbShiftTo.SetValue(@" ");
            _workOrderOperationsForm.btnSearch.DoClick();
            _workOrderOperationsForm.GetTabTableGeneric(@"Unallocated");

            //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric, @"Job Type^Receive Cargo - Road~Work Order^" + workOrderId + "", ClickType.Click, rowHeight: 16);
            _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
            [$"Job Type^Receive Cargo - Road~Work Order^{workOrderId}"]);

            _workOrderEnquiryForm.SetFocusToForm();

            // Unapprove the actioned task
            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date  MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
              // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date @"Status^Approved~Work Order Description^Receive Cargo - Road Task #1",
             // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date  ClickType.ContextClick);
            FindRowDoContextMenu(@"Status^Approved~Work Order Description^Receive Cargo - Road Task #1", @"Set Un-Approve");

            // Check if the status is changed to User Entry
            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date    @"Status^User Entry~Work Order Description^Receive Cargo - Road Task #1",
             // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date   ClickType.Click);
            _workOrderEnquiryForm.TblData.FindClickRow(new[] { "Status^User Entry~Work Order Description^Receive Cargo - Road Task #1" });

            _workOrderOperationsForm.SetFocusToForm();

            // Check if the work order is removed from the Unallocated tab
            _workOrderOperationsForm.GetTabTableGeneric(@"Unallocated");

            //var isRowPresent = MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric,
            //    $"Job Type^Receive Cargo - Road~Work Order^{workOrderId}", ClickType.None, rowHeight: 16,
            //    doAssert: false);
           // Assert.IsTrue(!isRowPresent, @"Test Case 64267 - Should not have found a work order job");
            var isRowPresent = _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
                new[] { $"Job Type^Receive Cargo - Road~Work Order^{workOrderId}" }, ClickType.None, doAssert: false);
            Assert.IsTrue(!string.IsNullOrEmpty(isRowPresent), "Test Case 64267 - Should not have found a work order job");

            _workOrderEnquiryForm.SetFocusToForm();

            // Approve and action the task
            /*// Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                @"Status^User Entry~Work Order Description^Receive Cargo - Road Task #1",
                ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Set Approved");*/
            FindRowDoContextMenu("Status^User Entry~Work Order Description^Receive Cargo - Road Task #1", "Set Approved");

            /*// Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
               @"Status^Approved~Work Order Description^Receive Cargo - Road Task #1",
               ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Action Task|Receive Cargo - Road Task #1");*/
            FindRowDoContextMenu("Status^Approved~Work Order Description^Receive Cargo - Road Task #1",
                "Action Task|Receive Cargo - Road Task #1");

            string pinNumber = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
               @"Status^Approved~Work Order Description^Receive Cargo - Road Task #1", @"Pin", ClickType.None);
            

            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: $"Road Gate {TerminalId}", vehicleId: TestCaseNumber);
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            roadGateForm.txtNewItem.SetValue(pinNumber);
            Keyboard.Press(VirtualKeyShort.TAB);

            // Should open Receive Container form
            RoadGateDetailsReceiveForm receiveGeneralCargoForm = new RoadGateDetailsReceiveForm($"Receive General Cargo {TerminalId}");

            // Click Save button Receive Container form
            receiveGeneralCargoForm.BtnSave.DoClick();

            // Click Save on Road Gate form
            roadGateForm.btnSave.DoClick();

            _workOrderOperationsForm.SetFocusToForm();

            _workOrderOperationsForm.GetTabTableGeneric(@"Unallocated");

            //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric,
            //    @"Job Type^Receive Cargo - Road~Vehicle Id^" + TestCaseNumber + "~Work Order^" + workOrderId + "",
             //   ClickType.Click, rowHeight: 16);
            _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
                new[] { $"Job Type^Receive Cargo - Road~Vehicle Id^{TestCaseNumber}~Work Order^{workOrderId}" });

            _workOrderEnquiryForm.SetFocusToForm();

            FindRowDoContextMenu(@"Status^Approved~Work Order Description^Receive Cargo - Road Task #1", @"Set Un-Approve");

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Un-Approve");
            confirmationFormYesNo.btnYes.DoClick();

            _workOrderOperationsForm.SetFocusToForm();

            // Check if the work order is removed from the Unallocated tab
            _workOrderOperationsForm.GetTabTableGeneric(@"Unallocated");

            bool isRowPresent1 = MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.TabGeneric.TableWithHeader.GetElement(),
                @"Job Type^Receive Cargo - Road~Vehicle Id^" + TestCaseNumber + "~Work Order^" + workOrderId + "",
                ClickType.Click, rowHeight: 16, doAssert: false);
            Assert.IsTrue(!isRowPresent1, @"Test Case 64267 - Should not have found a work order job");
        }

        void FindRowDoContextMenu(string rowToFind, string contextMenuToSelect)
        {
            _workOrderEnquiryForm.TblData.FindClickRow(new [] { rowToFind }, ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(contextMenuToSelect);
        }
    }
}






