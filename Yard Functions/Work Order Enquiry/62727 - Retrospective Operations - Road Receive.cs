using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNForms.FormObjects.Yard_Functions.Work_Order;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using System.Diagnostics;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Work_Order_Enquiry
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase62727 : MTNBase
    {
        WorkOrderEnquiryForm _workOrderEnquiryForm;
        NewWorkOrderForm _newWorkOrderForm;
        WorkOrderTasksForm _workOrderTasksForm;
        ConfirmationFormYesNo _confirmationFormYesNo;
        WorkOrderOperationsForm _workOrderOperationsForm;
        WorkPointJobSheetForm _workPointJobSheetForm;
        CompleteJobForm _completeJobForm;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

        private const string TestCaseNumber = @"62727";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";
        private const string TerminalId = @"PEL";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            userName = "WOUSER";
            LogInto<MTNLogInOutBO>(userName);
            CallJadeScriptToRun(TestContext, @"reset_WorkOrders_62727");
            SetupAndLoadInitializeData(TestContext);
        }

        /// <summary>
        /// To test the Retrospective Operations Road Receive
        /// </summary>
        [TestMethod]
        public void RetrospectiveOperationsRoadReceive()
        {
            MTNInitialize();

            // Open Work Order Enquiry
            FormObjectBase.MainForm.OpenWorkOrderEnquiryFromToolbar();
            _workOrderEnquiryForm = new WorkOrderEnquiryForm($"Work Order Enquiry {TerminalId}");

            // Click on New Cargo Work Order
            _workOrderEnquiryForm.DoNewCargoWorkOrder();
            var date = DateTime.Now;
            _newWorkOrderForm = new NewWorkOrderForm($"Cargo Work Order New Screen {TerminalId}");

            _newWorkOrderForm.txtReference.SetValue(@"RECEIVE");
            _newWorkOrderForm.txtRequiredByDate.SetValue(date.ToString(@"ddMMyyyy"));
            _newWorkOrderForm.txtRequiredByTime.SetValue(date.ToString(@"HHmm"));
            _newWorkOrderForm.btnAddTask.DoClick();

            // Create Receive Cargo Work Order
            _workOrderTasksForm = new WorkOrderTasksForm($"Work Order Details {TerminalId}");
            MTNControlBase.FindClickRowInList(_workOrderTasksForm.lstTasks, @"Receive Cargo");

            _workOrderTasksForm.btnOK.DoClick();
            _newWorkOrderForm.SetFocusToForm();
            _newWorkOrderForm.GetTaskDetailsTab(@"Receive");

            _newWorkOrderForm.cmbBooking.SetValue($"JLGBOOK{TestCaseNumber}", doDownArrow: true, searchSubStringTo: 12, additionalWaitTimeout: 1000);

            _newWorkOrderForm.GetTaskDetailsTab(@"Selected Cargo Items [0]");
            _newWorkOrderForm.btnAttachCargoFromBookingRelease.DoClick();

            _newWorkOrderForm.btnSave.DoClick();

            var time = DateTime.Now;

            string formattedTime = time.ToString("HH:mm");
            Trace.TraceInformation(@"formattedTime: {0}", formattedTime);

            date = date.AddDays(-1);
            Trace.TraceInformation(@"date: {0}", date);

            string formattedDate = date.ToString("dd/MM/yyyy");
            Trace.TraceInformation(@"formattedDate: {0}", formattedDate);

            // Get the day of the week for yesterday
            DayOfWeek yesterdayDayOfWeek = date.DayOfWeek;

            // Convert the DayOfWeek enum to string
            string yesterdayDay = yesterdayDayOfWeek.ToString();

            string ShiftDate = date.ToString(@"dd/MM/yyyy ") + yesterdayDay;

            _workOrderEnquiryForm.txtCustomerReference.SetValue(@"RECEIVE");
            _workOrderEnquiryForm.lstStatus.BtnAllLeft.DoClick();
            string[] detailsToMove =
            {
                 @"User Entry"
              };
            _workOrderEnquiryForm.lstStatus.MoveItemsBetweenList(_workOrderEnquiryForm.lstStatus.LstLeft, detailsToMove);
            _workOrderEnquiryForm.DoSearch();

            // Approve and Action the task
            // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
           // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date      $"Status^User Entry~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
            // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date     ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow(
            [
                $"Status^User Entry~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"
            ], ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Set Approved");

            var workOrderId = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
                $"Status^Approved~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}", @"Work Order #", ClickType.None);

            // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
            // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date     $"Status^Approved~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
          // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date       ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow(
                new[]
                {
                    $"Status^Approved~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"
                }, ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Action Task|Receive Cargo - Road Task #1");

            string pinNumber = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
              $"Status^Approved~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}", @"Pin", ClickType.None);

            //Use the Retrospective mode to gate in the vehicle
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: $"Road Gate {TerminalId}", vehicleId: TestCaseNumber);
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);

            // The Effective Date is set to yesterday's date
            roadGateForm.GetOpsRetrospectiveControls();
            roadGateForm.chkOpsRetrospective.DoClick();
            roadGateForm.GetOpsRetrospectiveControls();
            roadGateForm.txtEffectiveDate.SetValue(date.ToString(@"ddMMyyyy"));
            roadGateForm.txtEffectiveTime.SetValue(time.ToString(@"HH:mm"));

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            roadGateForm.txtNewItem.SetValue(pinNumber);
            Keyboard.Press(VirtualKeyShort.TAB);

            // Should open Receive Container form
            RoadGateDetailsReceiveForm receiveFullContainerForm = new RoadGateDetailsReceiveForm($"Receive Full Container {TerminalId}");

            // Click Save button Receive Container form
            receiveFullContainerForm.BtnSave.DoClick();

            // Click Save button on Warnings window
            warningErrorForm = new WarningErrorForm($"Warnings for Gate In/Out {TerminalId}");
            warningErrorForm.btnSave.DoClick();

            // Click Save on Road Gate form
            roadGateForm.btnSave.DoClick();

            _workOrderEnquiryForm.SetFocusToForm();
            _workOrderEnquiryForm.DoWorkOrderOperations();

            //Create a Job sheet using yesterdays shift
            _workOrderOperationsForm = new WorkOrderOperationsForm($"Work Order Operations {TerminalId}");
            _workOrderOperationsForm.cmbOperationMode.SetValue(@"Road");
            _workOrderOperationsForm.chkJobSheet.DoClick();
            _workOrderOperationsForm.chkFilterCriteria.DoClick();
            _workOrderOperationsForm.GetFilterCriteria();
            _workOrderOperationsForm.cmbShiftFrom.SetValue(@" ");
            _workOrderOperationsForm.cmbShiftTo.SetValue(@" ");
            _workOrderOperationsForm.btnSearch.DoClick();

            _workOrderOperationsForm.GetJobSheetDetails();
            _workOrderOperationsForm.TblJobSheet.GetElement().RightClick();
            _workOrderOperationsForm.ContextMenuSelect(@"Add Job Sheet...");

            _workPointJobSheetForm = new WorkPointJobSheetForm($"Work Point Job Sheet {TerminalId}");
            _workPointJobSheetForm.cmbWorkPointType.SetValue(@"ROAD	Road", doDownArrow: true);
            _workPointJobSheetForm.cmbWorkPoint.SetValue(@"ROADWP	Road Work Point", doDownArrow: true);
            _workPointJobSheetForm.cmbShift.SetValue(ShiftDate, doDownArrow: true, downArrowSearchType: SearchType.Contains, searchSubStringTo: 10, additionalWaitTimeout: 1000, doTab: false);
            Keyboard.Press(VirtualKeyShort.DOWN);
            Keyboard.Press(VirtualKeyShort.TAB);

            _workPointJobSheetForm.txtExternalName.SetValue($"ROAD{TestCaseNumber}");

            _workPointJobSheetForm.btnOK.DoClick();

            //Allocate the job to gangs
            _workOrderOperationsForm.SetFocusToForm();
            _workOrderOperationsForm.chkJobSheet.DoClick(false);
            _workOrderOperationsForm.GetTabTableGeneric(@"Unallocated");
            MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.TabGeneric.GetElement(),
                @"Job Type^Receive Cargo - Road~Vehicle Id^" + TestCaseNumber + "~Work Order^" + workOrderId + "",
                ClickType.ContextClick, rowHeight: 16);
            /*_workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
                [$"Job Type^Receive Cargo - Road~Vehicle Id^{TestCaseNumber}~Work Order^{workOrderId}"],
                ClickType.ContextClick);*/
            _workOrderOperationsForm.ContextMenuSelect($"Allocate to job sheet...|ROAD{TestCaseNumber}");
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Allocate to job sheet...");
            _confirmationFormYesNo.btnYes.DoClick();

            //On the Allocated tab select the Complete Job
            _workOrderOperationsForm.GetTabTableGeneric(@"Allocated");
            MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.TabGeneric.GetElement(),
               @"Work Order^" + workOrderId + "~Vehicle Id^" + TestCaseNumber + "~Job Type^Receive Cargo - Road",
               ClickType.ContextClick, rowHeight: 16);
           /* _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
                [$"Work Order^{workOrderId}~Vehicle Id^{TestCaseNumber}~Job Type^Receive Cargo - Road"],
                ClickType.ContextClick);*/

            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            //Complete the job by moving the tarcked item to BS01 location
            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");
            _completeJobForm.GetOnVehicleDetails();

            // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnVehicle, @"ID^" + CargoId + "", ClickType.Click, rowHeight: 15);
            _completeJobForm.TblOnVehicle.FindClickRow(new[] { $"ID^{CargoId}" });
            _completeJobForm.GetToLocationDetails();

            _completeJobForm.cmbTerminalArea.SetValue(@"BS01");

            _completeJobForm.btnLoadUnload.DoClick();
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Move");
            _confirmationFormYesNo.btnYes.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: $"Warnings for Move {TerminalId}");
            warningErrorForm.btnSave.DoClick();

            _completeJobForm.btnClose.DoClick();

            //Check if the status of the job is Job Complete
            _workOrderOperationsForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.TabGeneric.GetElement(),
                @"Work Order^" + workOrderId + "~Vehicle Id^" + TestCaseNumber +
                "~Job Type^Receive Cargo - Road~Job Status^Job Complete", ClickType.Click, rowHeight: 16);
           /* _workOrderOperationsForm.TabGeneric.TableWithHeader.FindClickRow(
                [$"Work Order^{workOrderId}~Vehicle Id^{TestCaseNumber}~Job Type^Receive Cargo - Road~Job Status^Job Complete"]);*/

            _workOrderOperationsForm.chkJobSheet.DoClick(false);

            _workOrderOperationsForm.CloseForm();

            _workOrderEnquiryForm.SetFocusToForm();

            //Check if the Work Order status is Complete
            // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
             // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date    $"Status^Complete~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
             // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date    ClickType.Click);
            _workOrderEnquiryForm.TblData.FindClickRow(new[]
            {
                $"Status^Complete~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"
            });

            // Open Yard Functions | Road Operations
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();

            // Find the vehicle, complete road job and do Road Exit
            roadOperationsForm = new RoadOperationsForm($"Road Operations {TerminalId}");
            MTNControlBase.FindClickRowInList(roadOperationsForm.lstVehicles, $"{TestCaseNumber} (1/0) - ICA - Yard Interchange");

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, $"Vehicle Id^{TestCaseNumber}~Cargo ID^{CargoId}", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { $"Vehicle Id^{TestCaseNumber}~Cargo ID^{CargoId}" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, $"Vehicle Id^{TestCaseNumber}~Cargo ID^{CargoId}", ClickType.None, doAssert: false);
            roadOperationsForm.TblYard2.FindClickRow(new[] { $"Vehicle Id^{TestCaseNumber}~Cargo ID^{CargoId}" }, ClickType.None, doAssert: false);
            roadOperationsForm.CloseForm();

            _workOrderEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                // $"Status^Complete~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow([$"Status^Complete~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);

            _workOrderEnquiryForm.ContextMenuSelect(@"Set Archived");

            //Open Cargo Enquiry and check the transactions
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", CargoId);
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{CargoId}" });

            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.MainToolbar, (int)CargoEnquiryForm.Toolbar.MainToolbar.ViewTransactions, "View Transactions");

            // Check the transactions to see if the retrospective date and time is on the Moved and Received Roads transactions
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>($"Transactions for {CargoId} {TerminalId}");
            /*// Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
                $"Type^Moved~Date^{formattedDate} {formattedTime}~Details^From {TestCaseNumber} Road to BS01", ClickType.Click);
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
                $"Type^Moved~Date^{formattedDate} {formattedTime}~Details^From Nowhere to {TestCaseNumber} Road", ClickType.Click);
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
                $"Type^Received - Road~Date^{formattedDate} {formattedTime}", ClickType.Click);*/
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[]
            {
                $"Type^Moved~Date^{formattedDate} {formattedTime}~Details^From {TestCaseNumber} Road to BS01",
                $"Type^Moved~Date^{formattedDate} {formattedTime}~Details^From Nowhere to {TestCaseNumber} Road",
                $"Type^Received - Road~Date^{formattedDate} {formattedTime}"
            });
        }

        #region - Setup and Run Data Loads
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = $"_{TestCaseNumber}_";

            // Delete cargo onsite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='" + TerminalId + "'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<messageMode>D</messageMode>\n            <id>" + CargoId + "</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>BS01</locationId>\n            <weight>5000</weight>\n            <imexStatus>Import</imexStatus>\n            <commodity>GEN</commodity>\n            <dischargePort>NZBLU</dischargePort>\n            <voyageCode>VOYWORKORDERS</voyageCode>\n            <isoType>2210</isoType>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0'?>\n<JMTInternalPrenote>\n<AllPrenote>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n<Prenote Terminal='" + TerminalId + "'>\n	<bookingRef>JLGBOOK" + TestCaseNumber + "</bookingRef>  \n <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>" + CargoId + "</id>\n            <isoType>2210</isoType>\n            <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n            <voyageCode>VOYWORKORDERS</voyageCode>\n            <operatorCode>MSL</operatorCode>\n			<dischargePort>NZBLU</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n			</Prenote>\n			</AllPrenote>\n</JMTInternalPrenote>\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
        #endregion - Setup and Run Data Loads
    }
}
