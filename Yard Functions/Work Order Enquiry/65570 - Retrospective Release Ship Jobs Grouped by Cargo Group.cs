using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Yard_Functions.Work_Order;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using FlaUI.Core.Tools;
using HardcodedData.TerminalData;
using MTNForms.FormObjects.Message_Dialogs;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Work_Order_Enquiry
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase65570 : MTNBase
    {
        WorkOrderEnquiryForm _workOrderEnquiryForm;
        NewWorkOrderForm _newWorkOrderForm;
        WorkOrderTasksForm _workOrderTasksForm;
        WorkOrderOperationsForm _workOrderOperationsForm;
        AddJobDetailsForm _addJobDetailsForm;
        CompleteJobForm _completeJobForm;
        WorkOrderGroupingMethodForm _workOrderGroupingMethodForm;

        private const string TestCaseNumber = @"65570";
        private const string TerminalId = @"EUP";

        CallAPI callAPI;
        GetSetArgumentsToCallAPI setArgumentsToCallAPI;
        string baseURL;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = $"_{TestCaseNumber}_";
            BaseClassInitialize_New(testContext);
        }

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        private void MTNInitialize()
        {
            userName = "EUPUSER";
            CallJadeScriptToRun(TestContext, @"resetData_65570");
            baseURL = TestContext.GetRunSettingValue(@"BaseUrl");
            SetupAndLoadInitializeData();
            LogInto<MTNLogInOutBO>(userName);
        }
        
        /// <summary>
        /// This test case is to test the retrospective release ship operations
        /// Load the cargo items using Complete Job form with and without the Cargo Grouping mode
        /// </summary>
        [TestMethod]
        public void RetrospectiveReleaseShipJobsGroupedByCargoGroup()
        {

            MTNInitialize();

            //Open Work Order Enquiry
            FormObjectBase.MainForm.OpenWorkOrderEnquiryFromToolbar();
            _workOrderEnquiryForm = new WorkOrderEnquiryForm($"Work Order Enquiry {TerminalId}");

            //Click on New Cargo Work Order
            _workOrderEnquiryForm.DoNewCargoWorkOrder();

            _newWorkOrderForm = new NewWorkOrderForm($"Cargo Work Order New Screen {TerminalId}");
            var date = DateTime.Now;

            _newWorkOrderForm.txtRequiredByDate.SetValue(date.ToString(@"ddMMyyyy"));
            _newWorkOrderForm.txtRequiredByTime.SetValue(date.ToString(@"HHmm"));
            _newWorkOrderForm.txtReference.SetValue(@"RELEASESHIP");
            _newWorkOrderForm.btnAddTask.DoClick();

            //Create Release Ship Cargo Work Order
            _workOrderTasksForm = new WorkOrderTasksForm($"Work Order Details {TerminalId}");
            MTNControlBase.FindClickRowInList(_workOrderTasksForm.lstTasks, @"Release Cargo");

            _workOrderTasksForm.btnOK.DoClick();
            _newWorkOrderForm.SetFocusToForm();
            _newWorkOrderForm.GetTaskDetailsTab(@"Release");
            _newWorkOrderForm.cmbTransportMode.SetValue(@"Ship", doDownArrow: true, searchSubStringTo: 3);
            _newWorkOrderForm.cmbVoyageReceiveCargo.SetValue(@"VOYWO001", doDownArrow: true, searchSubStringTo: 5, additionalWaitTimeout: 1000);
            _newWorkOrderForm.SetFocusToForm();
            _newWorkOrderForm.GetTaskDetailsTab(@"Release");
            _newWorkOrderForm.cmbReleaseRequest.SetValue($"{TestCaseNumber}RLS", doDownArrow: true, searchSubStringTo: 5, additionalWaitTimeout: 1000);

            _newWorkOrderForm.GetTaskDetailsTab(@"Selected Cargo Items [0]");
            _newWorkOrderForm.btnAttachCargoFromBookingRelease.DoClick();
            _newWorkOrderForm.btnSave.DoClick();

            _workOrderEnquiryForm.txtCustomerReference.SetValue(@"RELEASESHIP");
            _workOrderEnquiryForm.lstStatus.BtnAllLeft.DoClick();
            /*
             * // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
             string[] detailsToMove =
            {
               @"User Entry"
            };*/
            _workOrderEnquiryForm.lstStatus.MoveItemsBetweenList(_workOrderEnquiryForm.lstStatus.LstLeft, new[] { @"User Entry" });
            _workOrderEnquiryForm.DoSearch();

            //Approve and Action the task
            /*// Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                @"Status^User Entry~Work Order Description^Release Cargo - Ship Task #1~Voyage^VOYWO001",
                ClickType.ContextClick);*/
            _workOrderEnquiryForm.TblData.FindClickRow(new[] { "Status^User Entry~Work Order Description^Release Cargo - Ship Task #1~Voyage^VOYWO001" }, ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Set Approved");

            _workOrderGroupingMethodForm = new WorkOrderGroupingMethodForm($"Select Work Order Grouping Method {TerminalId}");
            _workOrderGroupingMethodForm.btnApprove.DoClick();

            var workOrderId = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
                @"Status^Approved~Work Order Description^Release Cargo - Ship Task #1~Voyage^VOYWO001", @"Work Order #", ClickType.None);

            //Open Work Order Operations in the Ship mode
            _workOrderEnquiryForm.DoWorkOrderOperations();

            _workOrderOperationsForm = new WorkOrderOperationsForm($"Work Order Operations {TerminalId}");

            _workOrderOperationsForm.SetFocusToForm();

            Retry.WhileException(() =>
            {
                _workOrderOperationsForm.cmbOperationMode.SetValue(@"Ship");
            }, TimeSpan.FromSeconds(2), null, true);

            _workOrderOperationsForm.chkFilterCriteria.DoClick();
            _workOrderOperationsForm.GetFilterCriteria();
            _workOrderOperationsForm.cmbShiftFrom.SetValue(@" ");
            _workOrderOperationsForm.cmbShiftTo.SetValue(@" ");
            _workOrderOperationsForm.GetDetailsForShipOperationMode();
            _workOrderOperationsForm.chkBulkMode.DoClick();
            _workOrderOperationsForm.chkJobSheet.DoClick(false);
            _workOrderOperationsForm.cmbVoyage.SetValue(EUP.Voyage.VOYWO001, doDownArrow: true);
            _workOrderOperationsForm.btnSearch.DoClick();

            //Find the job and update the retrospective date and time
            Retry.WhileException(() =>
            {
                _workOrderOperationsForm.GetTabTableGeneric(@"Unallocated");
            }, TimeSpan.FromSeconds(2), null, true);

           // MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric, $"Job Type^Release Cargo - Ship~Work Order^{workOrderId}", ClickType.Click, rowHeight: 16);
            _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
            [$"Job Type^Release Cargo - Ship~Work Order^{workOrderId}"]);

            var pastDate = DateTime.Now.AddDays(-1);
            var yesterdaysDate = pastDate.ToString("dd/MM/yyyy");
            var time = DateTime.Now.ToString("12:30");

            //Get the day of the week for yesterday
            DayOfWeek yesterdayDayOfWeek = pastDate.DayOfWeek;
            //Convert the DayOfWeek enum to string
            string yesterdaysDay = yesterdayDayOfWeek.ToString();
            string ShiftDate = yesterdaysDate + " " + yesterdaysDay;

            _workOrderOperationsForm.SetFocusToForm();
            _workOrderOperationsForm.chkJobSheet.DoClick();
            _workOrderOperationsForm.GetJobSheetDetails();
            // MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblJobSheet, $"Name^SHIPGANG", ClickType.Click, rowHeight: 16);
            _workOrderOperationsForm.TblJobSheet.FindClickRow([$"Name^SHIPGANG"], ClickType.Click);
            // Tuesday, 18 March 2025 navmh5 Can be removed 6 months after specified date _workOrderOperationsForm.TblJobSheet.FindClickRow(new[] { "Name^SHIPGANG" });
            var startPoint = Mouse.Position;

            _workOrderOperationsForm.SetFocusToForm();
            //_workOrderOperationsForm.GetUnAllocatedtableDetails();

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblUnAllocated, $"Job Type^Release Cargo - Ship~Work Order^{workOrderId}", ClickType.Click, rowHeight: 16);
            _workOrderOperationsForm.TblUnAllocated.FindClickRow(new[]
                { $"Job Type^Release Cargo - Ship~Work Order^{workOrderId}" });
            var endPoint = Mouse.Position;

            Mouse.Drag(startPoint, endPoint);

            _addJobDetailsForm = new AddJobDetailsForm(@"Add Job Details");
            _addJobDetailsForm.btnSave.DoClick();

            // On the Allocated tab select the Complete Job
            _workOrderOperationsForm.GetTabTableGeneric(@"Allocated");

            var jobId = MTNControlBase.ReturnValueFromTableCell(_workOrderOperationsForm.TabGeneric.TableWithHeader.GetElement(),
                $"Work Order^{workOrderId}~Job Type^Release Cargo - Ship~Job Status^Job Unactioned", @"Job Id", ClickType.None);

            //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric, $"Work Order^{workOrderId}~Job Type^Release Cargo - Ship~Job Status^Job Unactioned", ClickType.ContextClick, rowHeight: 16);
            _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
            [$"Work Order^{workOrderId}~Job Type^Release Cargo - Ship~Job Status^Job Unactioned"], ClickType.ContextClick);

            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            // Check that the cargo is grouped by Cargo Group and that the Cargo Groups are displayed in the list
            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");

            _completeJobForm.chkCargoGrouping.DoClick();
            _completeJobForm.btnClose.DoClick();

            //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric,
             //   $"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Unactioned",
             //   ClickType.ContextClick, rowHeight: 16);
            _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
            [$"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Unactioned"], ClickType.ContextClick);

            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            // Check that the cargo is grouped by Cargo Group and that the Cargo Groups are displayed in the list
            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");

            _completeJobForm.GetOnShipOnSiteDetails();

            /*// Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65570A~Total Quantity^5", ClickType.Click, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65570B~Total Quantity^5", ClickType.Click, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^Unknown", ClickType.Click, rowHeight: 16);*/
            _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^GROUP65570A~Total Quantity^5", "ID^GROUP65570B~Total Quantity^5", "ID^Unknown" });

            // Discharge only 2 cargo items from the cargo group
            _completeJobForm.txtSelectCount.SetValue(@"2");
            Keyboard.Press(VirtualKeyShort.TAB);

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbVoyageLocation.SetValue(@"VOYWO001");

            _completeJobForm.btnLoadUnload.DoClick();
            Miscellaneous.WaitForSeconds(2);

            _completeJobForm.btnCollapseAll.DoClick();
            Miscellaneous.WaitForSeconds(2);

            // Check that the Total Quantity for the Cargo Group is updated to 3
            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65570A~Total Quantity^3", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^GROUP65570A~Total Quantity^3" });

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbVoyageLocation.SetValue(@"VOYWO001");

            _completeJobForm.btnLoadUnload.DoClick();
            Miscellaneous.WaitForSeconds(2);

            // Check that the cargo group is no longer in the list
            /*// Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
            bool dataFound = MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65570A~Total Quantity^3", rowHeight: 16, doAssert: false);
            Assert.IsTrue(dataFound == false, "GROUP65570A should not be present in list");*/
           var  dataFound = _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^GROUP65570A~Total Quantity^3" }, doAssert: false);
           Assert.IsTrue(!string.IsNullOrEmpty(dataFound), "GROUP65570A should not be present in list");

            _completeJobForm.btnClose.DoClick();

            // Change the Bulk Mode to false
            _workOrderOperationsForm.SetFocusToForm();
            _workOrderOperationsForm.GetDetailsForShipOperationMode();
            _workOrderOperationsForm.chkBulkMode.DoClick(false);

            _workOrderOperationsForm.GetTabTableGeneric(@"Allocated");

            // Expecting this status to be Job Part Complete, should change after the bug fix 66258
            //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric,
            //    $"Work Order^{workOrderId}~Job Type^Release Cargo - Ship~Job Status^Job Part Complete",
            //    ClickType.ContextClick, rowHeight: 16);
            _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
            [$"Work Order^{workOrderId}~Job Type^Release Cargo - Ship~Job Status^Job Part Complete"], ClickType.ContextClick);

            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");

            // Check that the cargo group is displayed on the OnSite tab - Bug Raised on this issue 66258
            _completeJobForm.chkHideCompleted.DoClick(false);

            // Check that the cargo group is displayed on the OnSite tab
           /// _completeJobForm.GetOnSiteOnShipCompletedDetails();
            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSiteCompleted, @"ID^GROUP65570A~Total Quantity^5", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnShipOnSiteCompleted.FindClickRow(new[] { "ID^GROUP65570A~Total Quantity^5" });

            // Check that the whole cargo grouped can not be moved to the terminal area and an error message is displayed
            _completeJobForm.GetOnShipOnSiteDetails();
            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65570B~Total Quantity^5", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^GROUP65570B~Total Quantity^5" });

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbVoyageLocation.SetValue(@"VOYWO001");

            _completeJobForm.btnLoadUnload.DoClick();
            ConfirmationFormOK confirmationFormOK = new ConfirmationFormOK(@"Error", automationIdOK: @"4");
            confirmationFormOK.CheckMessageMatch(@"Please select Cargo Item(s)");
            confirmationFormOK.btnOK.DoClick();

            _completeJobForm.btnClose.DoClick();

            // Change the Bulk Mode to true 
            _workOrderOperationsForm.SetFocusToForm();
            _workOrderOperationsForm.GetDetailsForShipOperationMode();
            _workOrderOperationsForm.chkBulkMode.DoClick(true);
            _workOrderOperationsForm.GetTabTableGeneric(@"Allocated");

            //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric,
            //    $"Work Order^{workOrderId}~Job Type^Release Cargo - Ship~Job Status^Job Part Complete",
            //    ClickType.ContextClick, rowHeight: 16);
            _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
            [$"Work Order^{workOrderId}~Job Type^Release Cargo - Ship~Job Status^Job Part Complete"], ClickType.ContextClick);

            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            // Complete the job for the cargo group GROUP65570B
            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");
            _completeJobForm.GetOnShipOnSiteDetails();

            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65570B~Total Quantity^5", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^GROUP65570B~Total Quantity^5" });

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbVoyageLocation.SetValue(@"VOYWO001");

            _completeJobForm.btnLoadUnload.DoClick();
            Miscellaneous.WaitForSeconds(2);

            // Check that the Unknown cargo group can not be moved in bulk and an error message is displayed
            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^Unknown", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^Unknown" });

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbVoyageLocation.SetValue(@"VOYWO001");

            _completeJobForm.btnLoadUnload.DoClick();

            /*confirmationFormOK = new ConfirmationFormOK(@"Error", automationIdOK: @"4");
            confirmationFormOK.CheckMessageMatch(@"Please select Cargo Item(s)");
            confirmationFormOK.btnOK.DoClick();

            _completeJobForm.btnExpandAll.DoClick();

            _completeJobForm.txtSelectCount.SetValue(@"5");
            Keyboard.Press(VirtualKeyShort.TAB);

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbVoyageLocation.SetValue(@"VOYWO001");

            _completeJobForm.btnLoadUnload.DoClick();*/
            Miscellaneous.WaitForSeconds(2);

            _completeJobForm.btnClose.DoClick();
            _workOrderOperationsForm.SetFocusToForm();

            // Check the work order is complete and job status is Job Complete
           // MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric,
           //     $"Work Order^{workOrderId}~Job Type^Release Cargo - Ship~Job Status^Job Complete", ClickType.Click,
           //     rowHeight: 16);
            _workOrderOperationsForm.TabGeneric.TableWithHeader.FindClickRow(new[] { "Work Order^" + workOrderId + "~Job Type^Release Cargo - Ship~Job Status^Job Complete" });

            _workOrderOperationsForm.CloseForm();

            _workOrderEnquiryForm.SetFocusToForm();
            /*/ Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                           @"Status^Complete~Work Order Description^Release Cargo - Ship Task #1~Voyage^VOYWO001",
                           ClickType.Click);*/
            _workOrderEnquiryForm.TblData.FindClickRow(new[] { "Status^Complete~Work Order Description^Release Cargo - Ship Task #1~Voyage^VOYWO001" });
            _workOrderEnquiryForm.CloseForm();
        }

        #region - Setup and Run Data Loads
        private void SetupAndLoadInitializeData()
        {
            callAPI = new CallAPI();
            setArgumentsToCallAPI = new GetSetArgumentsToCallAPI();
            setArgumentsToCallAPI.RequestURL = baseURL + "SendEDI?MasterTerminalAPI";
            setArgumentsToCallAPI.MediaType = "application/json";
            setArgumentsToCallAPI.UserName = userName;
            setArgumentsToCallAPI.Password = "Password1*";
            setArgumentsToCallAPI.TerminalId = "EUP";
            setArgumentsToCallAPI.Authorization = "Bearer";

            string cargoOnshipDel = "<AllCargoOnShip>\r\n<Cargo>\r\n<ID>JLG65570A01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65570A</CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570A02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65570A</CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570A03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65570A</CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570A04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65570A</CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570A05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65570A</CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570B01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65570B</CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570B02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65570B</CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570B03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65570B</CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570B04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65570B</CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570B05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65570B</CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570C01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId></CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570C02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId></CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570C03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId></CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570C04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId></CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570C05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>D</Action>\r\n<BOL></BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId></CargoGroupId>\r\n<MarkNumber></MarkNumber>\r\n</Cargo>\r\n</AllCargoOnShip>";
            //string releaseRequestDel = "<AllRequestHeader>\r\n<RH>\r\n<Action>D</Action>\r\n<ReleasebyType>false</ReleasebyType>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<ReleaseType>Ship</ReleaseType>\r\n<Status>Complete</Status>\r\n<Operator>MSL</Operator>\r\n</RH>\r\n</AllRequestHeader>";
            string cargoGroupDel = "<AllCargoGroup>\r\n<CargoGroup>\r\n<ID>GROUP65570A</ID>\r\n<CargoTypeDescr>Key Mark</CargoTypeDescr>\r\n<MinWeight>80000</MinWeight>\r\n<MaxWeight>80000</MaxWeight>\r\n<Operator>MSL</Operator>\r\n<Action>D</Action>\r\n</CargoGroup>\r\n<CargoGroup>\r\n<ID>GROUP65570B</ID>\r\n<CargoTypeDescr>Key Mark</CargoTypeDescr>\r\n<MinWeight>80000</MinWeight>\r\n<MaxWeight>80000</MaxWeight>\r\n<Operator>MSL</Operator>\r\n<Action>D</Action>\r\n</CargoGroup>\r\n</AllCargoGroup>";
            string cargoGroupAdd = "<AllCargoGroup>\r\n<CargoGroup>\r\n<ID>GROUP65570A</ID>\r\n<CargoTypeDescr>Key Mark</CargoTypeDescr>\r\n<MinWeight>80000</MinWeight>\r\n<MaxWeight>80000</MaxWeight>\r\n<Operator>MSL</Operator>\r\n<Action>A</Action>\r\n</CargoGroup><CargoGroup>\r\n<ID>GROUP65570B</ID>\r\n<CargoTypeDescr>Key Mark</CargoTypeDescr>\r\n<MinWeight>80000</MinWeight>\r\n<MaxWeight>80000</MaxWeight>\r\n<Operator>MSL</Operator>\r\n<Action>A</Action>\r\n</CargoGroup>\r\n\r\n</AllCargoGroup>";
            string cargoOnsiteAdd = "<AllCargoOnSite>\r\n<Cargo>\r\n<ID>JLG65570A01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<CargoGroupId>GROUP65570A</CargoGroupId>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570A02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<CargoGroupId>GROUP65570A</CargoGroupId>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570A03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<CargoGroupId>GROUP65570A</CargoGroupId>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570A04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<CargoGroupId>GROUP65570A</CargoGroupId>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570A05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<CargoGroupId>GROUP65570A</CargoGroupId>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570B01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<CargoGroupId>GROUP65570B</CargoGroupId>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570B02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<CargoGroupId>GROUP65570B</CargoGroupId>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570B03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<CargoGroupId>GROUP65570B</CargoGroupId>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570B04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<CargoGroupId>GROUP65570B</CargoGroupId>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570B05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<CargoGroupId>GROUP65570B</CargoGroupId>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570C01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570C02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570C03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570C04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65570C05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort><CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n</Cargo>\r\n</AllCargoOnSite>";
            string releaseRequestAdd = "<AllRequestHeader>\r\n<RH>\r\n<Action>A</Action>\r\n<ReleasebyType>false</ReleasebyType>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<ReleaseType>Ship</ReleaseType>\r\n<Status>Active</Status>\r\n<Operator>MSL</Operator>\r\n<AllRequestDetail>\r\n<RD>\r\n<ID>JLG65570A01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>001</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570A02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>002</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570A03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>003</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570A04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>004</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570A05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>005</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570B01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>006</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570B02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>007</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570B03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>008</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570B04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>009</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570B05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>010</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570C01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>011</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570C02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>012</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570C03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>013</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570C04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>014</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n<RD>\r\n<ID>JLG65570C05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<ReleaseRequestNumber>65570RLS</ReleaseRequestNumber>\r\n<RequestDetailID>015</RequestDetailID>\r\n<Quantity>1</Quantity>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n</RD>\r\n</AllRequestDetail>\r\n</RH>\r\n</AllRequestHeader>\r\n";
            
            // delete cargo onship
            AddDeleteData(cargoOnshipDel, "CargoOnShipXML");

            // delete BOL
            //AddDeleteData(releaseRequestDel, "ReleaseRequestXML");

            // delete cargo group
            AddDeleteData(cargoGroupDel, "CargoGroupXML");

            // create cargo group
            AddDeleteData(cargoGroupAdd, "CargoGroupXML");

            // create cargo onsite
             AddDeleteData(cargoOnsiteAdd, "CargoOnSiteXML");

            // create BOL
            AddDeleteData(releaseRequestAdd, "ReleaseRequestXML");
        }

        private void AddDeleteData(string data, string fileType)
        {
            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(data);
            string message = "{\r\n\r\n\"clientRef\": \"REF\",\r\n\r\n\"messageType\": \"" + fileType + "\",\r\n\r\n\"synchronous\": true,\r\n\r\n\"message\":\"" + System.Convert.ToBase64String(bytesToEncode) + "\"\r\n\r\n}";
            setArgumentsToCallAPI.MessageBody = message;
            var response = callAPI.DoCallAPI(setArgumentsToCallAPI);
            Assert.IsTrue(response.Result.IsSuccessStatusCode, "Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());
            Console.WriteLine("Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());
            Miscellaneous.WaitForSeconds(5);
        }
        #endregion - Setup and Run Data Loads
    }
}






