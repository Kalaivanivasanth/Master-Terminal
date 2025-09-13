using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Work_Order;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using System.Diagnostics;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Gate_Functions;
using FlaUI.Core.Definitions;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Work_Order_Enquiry
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase63056 : MTNBase
    {
        WorkOrderEnquiryForm _workOrderEnquiryForm;
        NewWorkOrderForm _newWorkOrderForm;
        WorkOrderTasksForm _workOrderTasksForm;
        ConfirmationFormYesNo _confirmationFormYesNo;
        WorkOrderOperationsForm _workOrderOperationsForm;
        WorkPointJobSheetForm _workPointJobSheetForm;
        CompleteJobForm _completeJobForm;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;
        RoadGateDetailsReleaseForm _roadGateDetailsReleaseForm;
        RoadExitForm _roadExitForm;

        const string TestCaseNumber = @"63056";
        const string TerminalId = @"PEL";
        const string VoyageId = @"VOYWORKORDERS";

        private readonly string[] CargoId =
        {
            @"JLG" + TestCaseNumber + @"A01",
            @"JLG" + TestCaseNumber + @"A02",
        };

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>("WOUSER");
            CallJadeScriptToRun(TestContext, @"reset_WorkOrders_63056");
            SetupAndLoadInitializeData(TestContext);
        }

        /// <summary>
        /// To test Retrospective Operations Road Release
        /// </summary>
        [TestMethod]
        public void RetrospectiveOperationsRoadRelease()
        {
            MTNInitialize();

            var date = DateTime.Now;

            // Open Work Order Enquiry
            FormObjectBase.MainForm.OpenWorkOrderEnquiryFromToolbar();
            _workOrderEnquiryForm = new WorkOrderEnquiryForm($"Work Order Enquiry {TerminalId}");

            // Click on New Cargo Work Order
            _workOrderEnquiryForm.DoNewCargoWorkOrder();

            _newWorkOrderForm = new NewWorkOrderForm($"Cargo Work Order New Screen {TerminalId}");

            _newWorkOrderForm.txtReference.SetValue(@"RELEASE");
            _newWorkOrderForm.txtRequiredByDate.SetValue(date.ToString(@"ddMMyyyy"));
            _newWorkOrderForm.txtRequiredByTime.SetValue(date.ToString(@"HHmm"));
            _newWorkOrderForm.btnAddTask.DoClick();

            // Create Release Cargo Work Order
            _workOrderTasksForm = new WorkOrderTasksForm($"Work Order Details {TerminalId}");
            MTNControlBase.FindClickRowInList(_workOrderTasksForm.lstTasks, @"Release Cargo");

            _workOrderTasksForm.btnOK.DoClick();
            _newWorkOrderForm.SetFocusToForm();
            _newWorkOrderForm.GetTaskDetailsTab(@"Release");

            _newWorkOrderForm.cmbReleaseRequest.SetValue($"{TestCaseNumber}RLS");

            _newWorkOrderForm.btnSave.DoClick();

            var time = DateTime.Now;
            Trace.TraceInformation(@"time: {0}", time);

            var pastDate = date.AddDays(-1);
            Trace.TraceInformation(@"pastDate: {0}", pastDate);

            var futureDate = date.AddDays(+1);
            Trace.TraceInformation(@"futureDate: {0}", futureDate);

            // Get the day of the week for yesterday
            DayOfWeek yesterdayDayOfWeek = pastDate.DayOfWeek;

            // Convert the DayOfWeek enum to string
            string yesterdayDay = yesterdayDayOfWeek.ToString();

            string ShiftDate = pastDate.ToString(@"dd/MM/yyyy ") + yesterdayDay;

            _workOrderEnquiryForm.txtCustomerReference.SetValue(@"RELEASE");
            _workOrderEnquiryForm.lstStatus.BtnAllLeft.DoClick();

            _workOrderEnquiryForm.lstStatus.MoveItemsBetweenList(_workOrderEnquiryForm.lstStatus.LstLeft, new[] { "User Entry" });
            _workOrderEnquiryForm.DoSearch();

            // Approve and Action the task
            /*// Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                @"Status^User Entry~Work Order Description^Release Cargo - Road Task #1",
                ClickType.ContextClick);*/
            _workOrderEnquiryForm.TblData.FindClickRow(new[]
                { @"Status^User Entry~Work Order Description^Release Cargo - Road Task #1" }, ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Set Approved");

            var workOrderId = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
                @"Status^Approved~Work Order Description^Release Cargo - Road Task #1", @"Work Order #", ClickType.None);

            /*// Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                @"Status^Approved~Work Order Description^Release Cargo - Road Task #1",
                ClickType.ContextClick);*/
            _workOrderEnquiryForm.TblData.FindClickRow(new[]
                { @"Status^Approved~Work Order Description^Release Cargo - Road Task #1" }, ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Action Task|Release Cargo - Road Task #1");


            string pinNumber = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
              @"Status^Approved~Work Order Description^Release Cargo - Road Task #1", @"Pin", ClickType.None);

            //Use the Retrospective mode to gate in the vehicle
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: $"Road Gate {TerminalId}", vehicleId: TestCaseNumber);
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);

            // The Effective Date is set to yesterday's date
            roadGateForm.GetOpsRetrospectiveControls();
            roadGateForm.chkOpsRetrospective.DoClick();
            roadGateForm.GetOpsRetrospectiveControls();
            roadGateForm.txtEffectiveDate.SetValue(pastDate.ToString(@"ddMMyyyy"));
            roadGateForm.txtEffectiveTime.SetValue(time.ToString(@"HH:mm"));

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            roadGateForm.txtNewItem.SetValue(pinNumber);
            Keyboard.Press(VirtualKeyShort.TAB);

            //Release Full container form
            _roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm(formTitle: $"Release General Cargo  {TerminalId}");
            _roadGateDetailsReleaseForm.BtnSave.DoClick();

            // Click Save button on Warnings window
            warningErrorForm = new WarningErrorForm($"Warnings for Gate In/Out {TerminalId}");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.btnSave.DoClick();

            // Click Save button on Warnings window
            warningErrorForm = new WarningErrorForm($"Warnings for Gate In/Out {TerminalId}");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.CloseForm();

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
                @"Job Type^Release Cargo - Road~Vehicle Id^" + TestCaseNumber + "~Work Order^" + workOrderId + "",
                ClickType.ContextClick, rowHeight: 16);
            //_workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
            //[$"Job Type^Release Cargo - Road~Vehicle Id^{TestCaseNumber}~Work Order^{workOrderId}"], ClickType.ContextClick);
            _workOrderOperationsForm.ContextMenuSelect($"Allocate to job sheet...|ROAD{TestCaseNumber}");
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Allocate to job sheet...");
            _confirmationFormYesNo.btnYes.DoClick();

            //On the Allocated tab select the Complete Job
            _workOrderOperationsForm.GetTabTableGeneric(@"Allocated");
            MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.TabGeneric.GetElement(),
                @"Work Order^" + workOrderId + "~Vehicle Id^" + TestCaseNumber + "~Job Type^Release Cargo - Road",
                ClickType.ContextClick, rowHeight: 16);
            //_workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
            //[$"Work Order^{workOrderId}~Vehicle Id^{TestCaseNumber}~Job Type^Release Cargo - Road"], ClickType.ContextClick);
            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            //Complete the job by moving the tarcked item to BS01 location
            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");
            _completeJobForm.GetOnVehicleDetails();

            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnVehicle, @"ID^" + CargoId[0] + "", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnVehicle.FindClickRow(new[] { $"ID^{CargoId[0]}" });
            _completeJobForm.btnLoad.DoClick();

            ConfirmationFormOKwithText _confirmationFormOKwithText = new ConfirmationFormOKwithText(@"Confirm quantity to move", controlType: ControlType.Pane, automationIdOK: @"2003");
            _confirmationFormOKwithText.btnOK.DoClick();

            _completeJobForm.btnClose.DoClick();

            //Check if the status of the job is Job Part Complete
            _workOrderOperationsForm.SetFocusToForm();

            /////MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric, @"Work Order^" + workOrderId + "~Vehicle Id^" + TestCaseNumber + "~Job Type^Release Cargo - Road~Job Status^Job Part Complete", ClickType.Click, rowHeight: 16);
             MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.TabGeneric.GetElement(), @"Work Order^" + workOrderId + "~Vehicle Id^" + TestCaseNumber + "~Job Type^Release Cargo - Road~Job Status^Job Part Complete", ClickType.ContextClick, rowHeight: 16);
            //_workOrderOperationsForm.TabGeneric.TableWithHeader.FindClickRow(
            //[$"Work Order^{workOrderId}~Vehicle Id^{TestCaseNumber}~Job Type^Release Cargo - Road~Job Status^Job Part Complete"], ClickType.ContextClick);
            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            //Complete the job by moving the tarcked item to BS01 location
            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");
            _completeJobForm.GetOnVehicleDetails();

            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_completeJobForm.tblOnVehicle, @"ID^" + CargoId[1] + "", ClickType.Click, rowHeight: 16);
            _completeJobForm.TblOnVehicle.FindClickRow(new[] { $"ID^{CargoId[1]}" });
            _completeJobForm.btnLoad.DoClick();

            _confirmationFormOKwithText = new ConfirmationFormOKwithText(@"Confirm quantity to move", controlType: ControlType.Pane, automationIdOK: @"2003");
            _confirmationFormOKwithText.btnOK.DoClick();

            _completeJobForm.btnClose.DoClick();

            //Check if the status of the job is Job Complete
            _workOrderOperationsForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.TabGeneric.GetElement(), @"Work Order^" + workOrderId + "~Vehicle Id^" + TestCaseNumber + "~Job Type^Release Cargo - Road~Job Status^Job Complete", ClickType.Click, rowHeight: 16);
            //_workOrderOperationsForm.TabGeneric.TableWithHeader.FindClickRow(
            //[$"Work Order^{workOrderId}~Vehicle Id^{TestCaseNumber}~Job Type^Release Cargo - Road~Job Status^Job Complete"]);
            _workOrderOperationsForm.chkJobSheet.DoClick(false);

            _workOrderOperationsForm.CloseForm();

            _workOrderEnquiryForm.SetFocusToForm();

            //Check if the Work Order status is Complete
            /*// Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                $"Status^Complete~Work Order Description^Release Cargo - Road Task #1",
                ClickType.Click);*/
            _workOrderEnquiryForm.TblData.FindClickRow(new[]
                { @"Status^Complete~Work Order Description^Release Cargo - Road Task #1" });

            FormObjectBase.MainForm.OpenRoadExitFromToolbar();
            _roadExitForm = new RoadExitForm($"Road Exit {TerminalId}");

            _roadExitForm.DoOpsRetrospective();

            _roadExitForm.GetOpsRetrospectiveControls();
            _roadExitForm.txtEffectiveDate.SetValue(futureDate.ToString(@"ddMMyyyy"));
            _roadExitForm.txtEffectiveTime.SetValue(time.ToString(@"HH:mm"));

            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_roadExitForm.tblVehicleVisits, $"Vehicle ID^{TestCaseNumber}", ClickType.ContextClick, rowHeight: 16);
            _roadExitForm.TblVehicleVisits.FindClickRow(new[] { $"Vehicle ID^{TestCaseNumber}" }, ClickType.ContextClick);
            _roadExitForm.ContextMenuSelect(@"Process Road Exit");

            _roadExitForm.CloseForm();

            _workOrderEnquiryForm.SetFocusToForm();

            /*// Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                $"Status^Complete~Work Order Description^Release Cargo - Road Task #1",
                ClickType.ContextClick);*/
            _workOrderEnquiryForm.TblData.FindClickRow(new[]
                { "Status^Complete~Work Order Description^Release Cargo - Road Task #1" }, ClickType.ContextClick);

            _workOrderEnquiryForm.ContextMenuSelect(@"Set Archived");

            //Open Cargo Enquiry and check the transactions
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm($"Cargo Enquiry {TerminalId}");

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", " ", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", CargoId[0]);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"**Anywhere**",
            EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{CargoId[0]}" });

            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.MainToolbar, (int)CargoEnquiryForm.Toolbar.MainToolbar.ViewTransactions, "View Transactions");

            // Check the transactions to see if the retrospective date and time is on the Moved and Received Roads transactions
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>($"Transactions for {CargoId[0]} {TerminalId}");

            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[]
            {
                $"Type^Moved~Date^{futureDate:dd/MM/yyyy HH:}",
                $"Type^Released - Road~Date^{futureDate:dd/MM/yyyy HH:}",
                $"Type^Moved~Date^{pastDate:dd/MM/yyyy HH:}~Details^From BS02 to {TestCaseNumber} Road"
            });
        }

        #region Setup and Load Data
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = $"_{TestCaseNumber}_";

            //Delete Cargo Onsite
            CreateDataFileToLoad(@"DeleteCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n<AllCargoOnSite>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n<CargoOnSite Terminal='PEL'>\n<TestCases>" + TestCaseNumber + "</TestCases>\n<cargoTypeDescr>Bottles of Beer</cargoTypeDescr>\n<id>JLG" + TestCaseNumber + "A01</id>\n<totalQuantity>5</totalQuantity>\n<operatorCode>MSL</operatorCode>\n<locationId>BS02</locationId>\n<weight>5000.0000</weight>\n<imexStatus>Import</imexStatus>\n<dischargePort>NZBLU</dischargePort>\n<voyageCode>" + VoyageId + "</voyageCode>\n<messageMode>D</messageMode>\n</CargoOnSite>\n<CargoOnSite Terminal='PEL'>\n<TestCases>" + TestCaseNumber + "</TestCases>\n<cargoTypeDescr>Bottles of Beer</cargoTypeDescr>\n<id>JLG" + TestCaseNumber + "A02</id>\n<operatorCode>MSL</operatorCode>\n<locationId>BS02</locationId>\n<weight>5000.0000</weight>\n<imexStatus>Import</imexStatus>\n<totalQuantity>5</totalQuantity>\n<dischargePort>NZBLU</dischargePort>\n<voyageCode>" + VoyageId + "</voyageCode>\n<messageMode>D</messageMode>\n</CargoOnSite>\n</AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //Create Cargo Onsite
            CreateDataFileToLoad(@"CreateCargoOnsite.xml",
                  "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n<AllCargoOnSite>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n<CargoOnSite Terminal='PEL'>\n<TestCases>" + TestCaseNumber + "</TestCases>\n<cargoTypeDescr>Bottles of Beer</cargoTypeDescr>\n<id>JLG" + TestCaseNumber + "A01</id>\n<totalQuantity>5</totalQuantity>\n<operatorCode>MSL</operatorCode>\n<locationId>BS02</locationId>\n<weight>5000.0000</weight>\n<imexStatus>Import</imexStatus>\n<dischargePort>NZBLU</dischargePort>\n<voyageCode>" + VoyageId + "</voyageCode>\n<messageMode>A</messageMode>\n</CargoOnSite>\n<CargoOnSite Terminal='PEL'>\n<TestCases>" + TestCaseNumber + "</TestCases>\n<cargoTypeDescr>Bottles of Beer</cargoTypeDescr>\n<id>JLG" + TestCaseNumber + "A02</id>\n<operatorCode>MSL</operatorCode>\n<locationId>BS02</locationId>\n<weight>5000.0000</weight>\n<imexStatus>Import</imexStatus>\n<totalQuantity>5</totalQuantity>\n<dischargePort>NZBLU</dischargePort>\n<voyageCode>" + VoyageId + "</voyageCode>\n<messageMode>A</messageMode>\n</CargoOnSite>\n</AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            /*// Delete Release Request
            CreateDataFileToLoad(@"DeleteReleaseRequest.xml",
                "<?xml version='1.0' encoding='UTF-8'?>   \n   <JMTInternalRequestMultiLine>   \n   <AllRequestHeader>   \n   <operationsToPerform>Verify;Load To DB</operationsToPerform>   \n   <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>   \n   <RequestHeader Terminal='PEL'>   \n   <releaseByType>true</releaseByType>   \n   <messageMode>D</messageMode>   \n   <operatorCode>MSL</operatorCode>   \n   <voyageCode>" + VoyageId + "</voyageCode>   \n   <releaseRequestNumber>" + TestCaseNumber + "RLS</releaseRequestNumber>   \n   <releaseTypeStr>Road</releaseTypeStr>   \n   <statusBulkRelease>Complete</statusBulkRelease>   \n   <subTerminalCode>Depot</subTerminalCode>   \n   <carrierCode>CARRIER1</carrierCode>   \n   <AllRequestDetail>   \n   <RequestDetail>   \n   <quantity>10</quantity>   \n   <cargoTypeDescr>Bottles of Beer</cargoTypeDescr>   \n    <consignee>ABCNE</consignee>   \n   <releaseRequestNumber>" + TestCaseNumber + "RLS</releaseRequestNumber>   \n     <requestDetailID>001</requestDetailID>   \n   </RequestDetail>   \n   </AllRequestDetail>   \n   </RequestHeader>   \n   </AllRequestHeader>   \n   </JMTInternalRequestMultiLine>   \n      \n   ");
*/
            // Create Release Request
            CreateDataFileToLoad(@"CreateReleaseRequest.xml",
                "<?xml version='1.0' encoding='UTF-8'?>   \n   <JMTInternalRequestMultiLine>   \n   <AllRequestHeader>   \n   <operationsToPerform>Verify;Load To DB</operationsToPerform>   \n   <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>   \n   <RequestHeader Terminal='PEL'>   \n   <releaseByType>true</releaseByType>   \n   <messageMode>A</messageMode>   \n   <operatorCode>MSL</operatorCode>   \n   <voyageCode>" + VoyageId + "</voyageCode>   \n   <releaseRequestNumber>" + TestCaseNumber + "RLS</releaseRequestNumber>   \n   <releaseTypeStr>Road</releaseTypeStr>   \n   <statusBulkRelease>Active</statusBulkRelease>   \n   <subTerminalCode>Depot</subTerminalCode>   \n   <carrierCode>CARRIER1</carrierCode>   \n   <AllRequestDetail>   \n   <RequestDetail>   \n   <quantity>10</quantity>   \n   <cargoTypeDescr>Bottles of Beer</cargoTypeDescr>   \n     <consignee>ABCNE</consignee>   \n   <releaseRequestNumber>" + TestCaseNumber + "RLS</releaseRequestNumber>   \n     <requestDetailID>001</requestDetailID>   \n   </RequestDetail>   \n   </AllRequestDetail>   \n   </RequestHeader>   \n   </AllRequestHeader>   \n   </JMTInternalRequestMultiLine>   \n      \n   ");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
        #endregion Setup and Load Data
    }
}
