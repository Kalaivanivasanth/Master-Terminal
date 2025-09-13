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
using MTNArguments.Classes;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Work_Order_Enquiry
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase65571 : MTNBase
    {
        WorkOrderEnquiryForm _workOrderEnquiryForm;
        NewWorkOrderForm _newWorkOrderForm;
        WorkOrderTasksForm _workOrderTasksForm;
        CargoSearchForm _cargoSearchForm;
        WorkOrderOperationsForm _workOrderOperationsForm;
        AddJobDetailsForm _addJobDetailsForm;
        CompleteJobForm _completeJobForm;
        WorkOrderGroupingMethodForm _workOrderGroupingMethodForm;

        private const string TestCaseNumber = @"65571";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";
        private const string TerminalId = @"EUP";
        private const string VoyageId = @"VOYWO001";

        CallAPI callAPI;
        GetSetArgumentsToCallAPI setArgumentsToCallAPI;
        string baseURL;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = $"_{TestCaseNumber}_";
            userName = "EUPUSER";
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
            baseURL = TestContext.GetRunSettingValue(@"BaseUrl");
            SetupAndLoadInitializeData();
            CallJadeScriptToRun(TestContext, @"resetData_63285");
            LogInto<MTNLogInOutBO>(userName);
        }
        /// <summary>
        /// This test case is to test the retrospective receive ship operations
        /// Discharge the cargo items using Complete Job form with and without the Cargo Grouping mode
        /// </summary>
        [TestMethod]
        public void RetrospectiveReceiveShipJobsGroupedByCargoGroup()
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
            _newWorkOrderForm.txtReference.SetValue(@"RECEIVESHIP");
            _newWorkOrderForm.btnAddTask.DoClick();

            //Create Receive Ship Cargo Work Order
            _workOrderTasksForm = new WorkOrderTasksForm($"Work Order Details {TerminalId}");
            MTNControlBase.FindClickRowInList(_workOrderTasksForm.lstTasks, @"Receive Cargo");

            _workOrderTasksForm.btnOK.DoClick();
            _newWorkOrderForm.SetFocusToForm();
            _newWorkOrderForm.GetTaskDetailsTab(@"Receive");
            _newWorkOrderForm.cmbTransportMode.SetValue(@"Ship", doDownArrow: true, searchSubStringTo: 3);
            _newWorkOrderForm.cmbVoyageReceiveCargo.SetValue(@"VOYWO001", doDownArrow: true, searchSubStringTo: 5, additionalWaitTimeout: 1000);
            _newWorkOrderForm.SetFocusToForm();
            _newWorkOrderForm.GetTaskDetailsTab(@"Receive");
            _newWorkOrderForm.cmbBillOfLading.SetValue(@"BOL65571", doDownArrow: true, searchSubStringTo: 5, additionalWaitTimeout: 1000);

            _newWorkOrderForm.GetTaskDetailsTab(@"Selected Cargo Items [0]");
            _newWorkOrderForm.btnAddCargoItems.DoClick();

            var cargoSearchArguments = new CargoSearchFormArguments
            {
                CargoSearchCriteria = new[] { new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = CargoSearchForm.ControlFieldName.Site, FieldRowValue = "On Ship" }, },
                CargoRowsToAdd = new[] { "ID^JLG65571A01", "ID^JLG65571A02", "ID^JLG65571A03", "ID^JLG65571A04", "ID^JLG65571A05", "ID^JLG65571B01", "ID^JLG65571B02", "ID^JLG65571B03", "ID^JLG65571B04", "ID^JLG65571B05", "ID^JLG65571C01", "ID^JLG65571C02", "ID^JLG65571C03", "ID^JLG65571C04", "ID^JLG65571C05", }
            }; CargoSearchForm.AddCargo("Cargo Search EUP", cargoSearchArguments);

            _newWorkOrderForm.btnSave.DoClick();

            _workOrderEnquiryForm.txtCustomerReference.SetValue(@"RECEIVESHIP");
            _workOrderEnquiryForm.lstStatus.BtnAllLeft.DoClick();
            string[] detailsToMove =
            {
               @"User Entry"
            };
            _workOrderEnquiryForm.lstStatus.MoveItemsBetweenList(_workOrderEnquiryForm.lstStatus.LstLeft, detailsToMove);
            _workOrderEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                // @"Status^User Entry~Work Order Description^Receive Cargo - Ship Task #1~Voyage^VOYWO001",
                // ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow(["Status^User Entry~Work Order Description^Receive Cargo - Ship Task #1~Voyage^VOYWO001"], ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow(new[] { "Status^User Entry~Work Order Description^Receive Cargo - Ship Task #1~Voyage^VOYWO001" }, ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Set Approved");

            _workOrderGroupingMethodForm = new WorkOrderGroupingMethodForm($"Select Work Order Grouping Method {TerminalId}");
            _workOrderGroupingMethodForm.btnApprove.DoClick();

            var workOrderId = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
                @"Status^Approved~Work Order Description^Receive Cargo - Ship Task #1~Voyage^VOYWO001", @"Work Order #", ClickType.None);

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
             //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric, $"Job Type^Receive Cargo - Ship~Work Order^{workOrderId}", ClickType.Click, rowHeight: 16);
            _workOrderOperationsForm.TabGeneric.TableWithHeader.FindClickRow([$"Job Type^Receive Cargo - Ship~Work Order^{workOrderId}"], ClickType.Click);

            var pastDate = DateTime.Now.AddDays(-1);
            var yesterdaysDate = pastDate.ToString("dd/MM/yyyy");
            var time = DateTime.Now.ToString("12:30");

            //Get the day of the week for yesterday
            DayOfWeek yesterdayDayOfWeek = pastDate.DayOfWeek;
            //Convert the DayOfWeek enum to string
            string yesterdaysDay = yesterdayDayOfWeek.ToString();
            string ShiftDate = yesterdaysDate + " " + yesterdaysDay;

            _workOrderOperationsForm.chkJobSheet.DoClick();
            _workOrderOperationsForm.GetJobSheetDetails();

            // Drag and drop the job to the job sheet
            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblJobSheet, $"Name^SHIPGANG", ClickType.Click, rowHeight: 16);
            _workOrderOperationsForm.TblJobSheet.FindClickRow(new[] { "Name^SHIPGANG" });
            var startPoint = Mouse.Position;

            _workOrderOperationsForm.SetFocusToForm();
            ///_workOrderOperationsForm.GetUnAllocatedtableDetails();

            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblUnAllocated, $"Job Type^Receive Cargo - Ship~Work Order^{workOrderId}", ClickType.Click, rowHeight: 16);
            _workOrderOperationsForm.TblUnAllocated.FindClickRow(new[]
                { $"Job Type^Receive Cargo - Ship~Work Order^{workOrderId}" });
            var endPoint = Mouse.Position;

            Mouse.Drag(startPoint, endPoint);

            _addJobDetailsForm = new AddJobDetailsForm(@"Add Job Details");
            _addJobDetailsForm.btnSave.DoClick();

            // On the Allocated tab select the Complete Job
            _workOrderOperationsForm.GetTabTableGeneric(@"Allocated");

            var jobId = MTNControlBase.ReturnValueFromTableCell(_workOrderOperationsForm.TabGeneric.TableWithHeader.GetElement(),
                $"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Unactioned", @"Job Id", ClickType.None);
             //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric, $"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Unactioned", ClickType.ContextClick, rowHeight: 16);
            _workOrderOperationsForm.TabGeneric.TableWithHeader.FindClickRow([$"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Unactioned"], ClickType.ContextClick);

            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            // Check that the cargo is grouped by Cargo Group and that the Cargo Groups are displayed in the list
            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");

            _completeJobForm.chkCargoGrouping.DoClick();
            _completeJobForm.btnClose.DoClick();
             //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric, $"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Unactioned", ClickType.ContextClick, rowHeight: 16);
            _workOrderOperationsForm.TabGeneric.TableWithHeader.FindClickRow([$"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Unactioned"], ClickType.ContextClick);

            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            // Check that the cargo is grouped by Cargo Group and that the Cargo Groups are displayed in the list
            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");

            _completeJobForm.GetOnShipOnSiteDetails();

            /*// Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65571A~Total Quantity^5", ClickType.Click, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65571B~Total Quantity^5", ClickType.Click, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^Unknown", ClickType.Click, rowHeight: 16);*/
            _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^GROUP65571A~Total Quantity^5", "ID^GROUP65571B~Total Quantity^5", "ID^Unknown" });

            // Discharge only 2 cargo items from the cargo group
            _completeJobForm.txtSelectCount.SetValue(@"2");
            Keyboard.Press(VirtualKeyShort.TAB);

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbTerminalArea.SetValue(@"BS1");

            _completeJobForm.btnLoadUnload.DoClick();
            Miscellaneous.WaitForSeconds(2);

            _completeJobForm.btnCollapseAll.DoClick();

            // Check that the Total Quantity for the Cargo Group is updated to 3
            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65571A~Total Quantity^3", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^GROUP65571A~Total Quantity^3" });

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbTerminalArea.SetValue(@"BS1");

            _completeJobForm.btnLoadUnload.DoClick();
            Miscellaneous.WaitForSeconds(2);

            // Check that the cargo group is no longer in the list
            /*// Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
            bool dataFound = MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65571A~Total Quantity^3", rowHeight: 16, doAssert: false);
            Assert.IsTrue(dataFound == false, "GROUP65571A should not be present in list");*/
            var dataFound = _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^GROUP65571A~Total Quantity^3" }, doAssert: false);
            Assert.IsTrue(!string.IsNullOrEmpty(dataFound), "GROUP65571A should not be present in list");

            _completeJobForm.btnClose.DoClick();

            // Change the Bulk Mode to false
            _workOrderOperationsForm.SetFocusToForm();
            _workOrderOperationsForm.GetDetailsForShipOperationMode();
            _workOrderOperationsForm.chkBulkMode.DoClick(false);

            _workOrderOperationsForm.GetTabTableGeneric(@"Allocated");
             //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric, $"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Part Complete", ClickType.ContextClick, rowHeight: 16);
            _workOrderOperationsForm.TabGeneric.TableWithHeader.FindClickRow([$"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Part Complete"], ClickType.ContextClick);

            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");
            _completeJobForm.chkHideCompleted.DoClick(false);

            // Check that the cargo group is displayed on the OnSite tab
           // _completeJobForm.GetOnSiteOnShipCompletedDetails();
            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSiteCompleted, @"ID^GROUP65571A~Total Quantity^5", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnShipOnSiteCompleted.FindClickRow(new[] { "ID^GROUP65571A~Total Quantity^5" });

            // Check that the whole cargo grouped can not be moved to the terminal area and an error message is displayed
            _completeJobForm.GetOnShipOnSiteDetails();
            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date  MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65571B~Total Quantity^5", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^GROUP65571B~Total Quantity^5" });

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbTerminalArea.SetValue(@"BS1");

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
             //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric, $"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Part Complete", ClickType.ContextClick, rowHeight: 16);
            _workOrderOperationsForm.TabGeneric.TableWithHeader.FindClickRow([$"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Part Complete"], ClickType.ContextClick);

            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            // Complete the job for the cargo group GROUP65571B
            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");
            _completeJobForm.GetOnShipOnSiteDetails();

            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^GROUP65571B~Total Quantity^5", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^GROUP65571B~Total Quantity^5" });

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbTerminalArea.SetValue(@"BS1");

            _completeJobForm.btnLoadUnload.DoClick();
            Miscellaneous.WaitForSeconds(2);

            // Check that the Unknown cargo group can not be moved in bulk and an error message is displayed
            // Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnShipOnSite, @"ID^Unknown", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnShipOnSite1.FindClickRow(new[] { "ID^Unknown" });

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbTerminalArea.SetValue(@"BS1");

            _completeJobForm.btnLoadUnload.DoClick();

           /* confirmationFormOK = new ConfirmationFormOK(@"Error", automationIdOK: @"4");
            confirmationFormOK.CheckMessageMatch(@"Please select Cargo Item(s)");
            confirmationFormOK.btnOK.DoClick();

            _completeJobForm.btnExpandAll.DoClick();

            _completeJobForm.txtSelectCount.SetValue(@"5");
            Keyboard.Press(VirtualKeyShort.TAB);

            _completeJobForm.GetToLocationDetails();
            _completeJobForm.cmbTerminalArea.SetValue(@"BS1");

            _completeJobForm.btnLoadUnload.DoClick();*/
            Miscellaneous.WaitForSeconds(2);

            _completeJobForm.btnClose.DoClick();
            _workOrderOperationsForm.SetFocusToForm();
             //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric, $"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Complete", ClickType.Click, rowHeight: 16);
            _workOrderOperationsForm.TabGeneric.TableWithHeader.FindClickRow([$"Work Order^{workOrderId}~Job Type^Receive Cargo - Ship~Job Status^Job Complete"], ClickType.Click);

            _workOrderOperationsForm.CloseForm();

            _workOrderEnquiryForm.SetFocusToForm();
            /*// Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                           @"Status^Complete~Work Order Description^Receive Cargo - Ship Task #1~Voyage^VOYWO001",
                           ClickType.Click);*/
            _workOrderEnquiryForm.TblData.FindClickRow(new[] { "Status^Complete~Work Order Description^Receive Cargo - Ship Task #1~Voyage^VOYWO001" });

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

            string cargoOnsiteDel = "<AllCargoOnSite>\r\n<Cargo>\r\n<ID>JLG65571A01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571A02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571A03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571A04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571A05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571B01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571B02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571B03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571B04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571B05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571C01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571C02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571C03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571C04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571C05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>BS1</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n</AllCargoOnSite>";
            string BOLDel = "<AllBOLHeader>\r\n<BOLHeader>\r\n<Action>D</Action>\r\n<DischargePort>NZBLU</DischargePort>\r\n<ID>BOL65571</ID>\r\n<Operator>MSL</Operator>\r\n<Voyage>VOYWO001</Voyage>\r\n<AllBOLDetails>\r\n<BOLDetails>\r\n<ID>BOL65571</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<Commodity></Commodity>\r\n<Weight>4000</Weight>\r\n<Quantity>15</Quantity>\r\n</BOLDetails>\r\n</AllBOLDetails>\r\n</BOLHeader>\r\n</AllBOLHeader>";
            string CargoGroupDel = "<AllCargoGroup>\r\n<CargoGroup>\r\n<ID>GROUP65571A</ID>\r\n<CargoTypeDescr>Key Mark</CargoTypeDescr>\r\n<MinWeight>80000</MinWeight>\r\n<MaxWeight>80000</MaxWeight>\r\n<Operator>MSL</Operator>\r\n<Action>D</Action>\r\n</CargoGroup>\r\n<CargoGroup>\r\n<ID>GROUP65571B</ID>\r\n<CargoTypeDescr>Key Mark</CargoTypeDescr>\r\n<MinWeight>80000</MinWeight>\r\n<MaxWeight>80000</MaxWeight>\r\n<Operator>MSL</Operator>\r\n<Action>D</Action>\r\n</CargoGroup>\r\n</AllCargoGroup>";
            string CargoGroupAdd = "<AllCargoGroup>\r\n<CargoGroup>\r\n<ID>GROUP65571A</ID>\r\n<CargoTypeDescr>Key Mark</CargoTypeDescr>\r\n<MinWeight>80000</MinWeight>\r\n<MaxWeight>80000</MaxWeight>\r\n<Operator>MSL</Operator>\r\n<Action>A</Action>\r\n</CargoGroup><CargoGroup>\r\n<ID>GROUP65571B</ID>\r\n<CargoTypeDescr>Key Mark</CargoTypeDescr>\r\n<MinWeight>80000</MinWeight>\r\n<MaxWeight>80000</MaxWeight>\r\n<Operator>MSL</Operator>\r\n<Action>A</Action>\r\n</CargoGroup>\r\n\r\n</AllCargoGroup>";
            string BOLCreate = "<AllBOLHeader>\r\n<BOLHeader>\r\n<DischargePort>NZBLU</DischargePort>\r\n<ID>BOL65571</ID>\r\n<Operator>MSL</Operator>\r\n<Voyage>VOYWO001</Voyage>\r\n<AllBOLDetails>\r\n<BOLDetails>\r\n<ID>BOL65571</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<Commodity></Commodity>\r\n<Weight>4000</Weight>\r\n<Quantity>15</Quantity>\r\n</BOLDetails>\r\n</AllBOLDetails>\r\n</BOLHeader>\r\n</AllBOLHeader>";
            string cargoOnshipAdd = "<AllCargoOnShip>\r\n<Cargo>\r\n<ID>JLG65571A01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65571A</CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571A02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65571A</CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571A03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65571A</CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571A04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65571A</CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571A05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65571A</CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571B01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65571B</CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571B02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65571B</CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571B03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65571B</CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571B04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65571B</CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571B05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId>GROUP65571B</CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571C01</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId></CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571C02</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId></CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571C03</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId></CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571C04</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId></CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n<Cargo>\r\n<ID>JLG65571C05</ID>\r\n<CargoTypeDescr>Paper</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>VOYWO001</Voyage>\r\n<Operator>MSL</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZBLU</DischargePort>\r\n<Location>VOYWO001</Location>\r\n<Commodity></Commodity>\r\n<LoadPort>NZCHC</LoadPort>\r\n<CargoSubtype>ROLLS</CargoSubtype>\r\n<Action>A</Action>\r\n<BOL>BOL65571</BOL>\r\n<TotalQuantity>1</TotalQuantity>\r\n<CargoGroupId></CargoGroupId>\r\n<MarkNumber>Mark65571</MarkNumber>\r\n</Cargo>\r\n</AllCargoOnShip>";

            // delete cargo onship
            AddDeleteData(cargoOnsiteDel, "CargoOnsiteXML");

            // delete BOL
            AddDeleteData(BOLDel, "UDFBOL");

            // delete cargo group
            AddDeleteData(CargoGroupDel, "CargoGroupXML");

            // create cargo group
            AddDeleteData(CargoGroupAdd, "CargoGroupXML");

            // create BOL
            AddDeleteData(BOLCreate, "UDFBOL");

            // create cargo onship
            AddDeleteData(cargoOnshipAdd, "CargoOnShipXML");
        }

        private void AddDeleteData(string data, string fileType)
        {
            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(data);
            string message = "{\r\n\r\n\"clientRef\": \"REF\",\r\n\r\n\"messageType\": \"" + fileType + "\",\r\n\r\n\"synchronous\": true,\r\n\r\n\"message\":\"" + System.Convert.ToBase64String(bytesToEncode) + "\"\r\n\r\n}";
            setArgumentsToCallAPI.MessageBody = message;
            var response = callAPI.DoCallAPI(setArgumentsToCallAPI);
            Assert.IsTrue(response.Result.IsSuccessStatusCode, "Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());
            Console.WriteLine("Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());
        }
        #endregion - Setup and Run Data Loads
    }
}





