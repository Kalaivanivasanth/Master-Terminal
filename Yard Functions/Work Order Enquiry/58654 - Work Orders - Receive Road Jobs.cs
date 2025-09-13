using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.Gate_Functions.Bookings.BookingItemForm;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNForms.FormObjects.Yard_Functions.Work_Order;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Work_Order_Enquiry
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase58654 : MTNBase
    {
        WorkOrderEnquiryForm _workOrderEnquiryForm;
        NewWorkOrderForm _newWorkOrderForm;
        WorkOrderTasksForm _workOrderTasksForm;
        BookingItemsForm _bookingItemsForm;
        BookingItemFormCont _bookingItemFormCont;
        RoadGateDetailsReceiveForm _preNoteDetailsForm;
        ConfirmationFormOK _confirmationFormOk;
        ConfirmationFormYesNo _confirmationFormYesNo;
        WorkOrderOperationsForm _workOrderOperationsForm;

        private const string TestCaseNumber = @"58654";
        const string TerminalId = @"PEL";

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
            CallJadeScriptToRun(TestContext, @"reset_WorkOrders_ReceiveWO");
            SetupAndLoadInitializeData(TestContext);
        }

        /// <summary>
        /// To test the Receive Cargo Work Order functionality using Work Order Enquiry form
        /// </summary>
        [TestMethod]
        public void ReceiveRoadJobs()
        {
            MTNInitialize();

            // Open Work Order Enquiry
            FormObjectBase.MainForm.OpenWorkOrderEnquiryFromToolbar();
            _workOrderEnquiryForm = new WorkOrderEnquiryForm($"Work Order Enquiry {TerminalId}");

            // Click on New Cargo Work Order
            _workOrderEnquiryForm.DoNewCargoWorkOrder();

            _newWorkOrderForm = new NewWorkOrderForm($"Cargo Work Order New Screen {TerminalId}");
            var date = DateTime.Now;

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

            _newWorkOrderForm.btnAddBooking.DoClick();

            _bookingItemsForm = new BookingItemsForm($"Adding Booking  {TerminalId}");
            _bookingItemsForm.txtReference.SetValue($"BOOK{TestCaseNumber}A01");
            _bookingItemsForm.cmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _bookingItemsForm.cmbVoyage.SetValue(PEL.Voyage.VOYWORKORDERS, doDownArrow: true,
                searchSubStringTo: PEL.Voyage.VOYWORKORDERS.Length - 5);
            _bookingItemsForm.cmbDischargePort.SetValue(Port.BLUNZ, doDownArrow: true);
            _bookingItemsForm.btnAdd.DoClick();

            _bookingItemFormCont = new BookingItemFormCont($"Adding Booking Item for  {TerminalId}");
            _bookingItemFormCont.GetReleaseDetailsDetails();

            _bookingItemFormCont.cmbCargoType.SetValue(CargoType.ISOContainer, doDownArrow: true, searchSubStringTo: 6);
            _bookingItemFormCont.cmbISOType.SetValue(ISOType.ISO2200, doDownArrow: true, searchSubStringTo: ISOType.ISO2200.Length - 1);
            _bookingItemFormCont.cmbCommodity.SetValue(Commodity.GEN, doDownArrow: true);
            _bookingItemFormCont.txtBookingQuantity.SetValue(@"1");
            _bookingItemFormCont.mtBookingWeight.SetValueAndType("1000", "lbs");
            _bookingItemFormCont.btnOK.DoClick();
            _bookingItemsForm.SetFocusToForm();
            _bookingItemsForm.btnSave.DoClick();

            _newWorkOrderForm.GetTaskDetailsTab(@"Selected Cargo Items [0]");
            _newWorkOrderForm.btnCreatePrenote.DoClick();

            _preNoteDetailsForm = new RoadGateDetailsReceiveForm(formTitle: $"PreNote Full Container {TerminalId}");
            _preNoteDetailsForm.TxtCargoId.SetValue($"JLG{TestCaseNumber}A01");

            _preNoteDetailsForm.BtnSave.DoClick();

            WarningErrorForm.CompleteWarningErrorForm("Warnings for Pre-Notes",
                new[]
                {
                    "Code :75016. The Container Id (JLG58654A01) failed the validity checks and may be incorrect."
                });

            _confirmationFormOk = new ConfirmationFormOK("Pre-Note Added", automationIdOK: @"4");
            _confirmationFormOk.btnOK.DoClick();

            _newWorkOrderForm.SetFocusToForm();
            _newWorkOrderForm.GetTaskDetailsTab(@"Selected Cargo Items [0]");
            _newWorkOrderForm.btnAttachCargoFromBookingRelease.DoClick();

            _newWorkOrderForm.btnSave.DoClick();

            _workOrderEnquiryForm.txtCustomerReference.SetValue(@"RECEIVE");
            _workOrderEnquiryForm.lstStatus.BtnAllLeft.DoClick();

            _workOrderEnquiryForm.lstStatus.MoveItemsBetweenList(_workOrderEnquiryForm.lstStatus.LstLeft,
                new[] { "User Entry" });
            _workOrderEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                // $"Status^User Entry~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow([$"Status^User Entry~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Set Approved");

            var workOrderId = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
                $"Status^Approved~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}", @"Work Order #", ClickType.None);
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                // $"Status^Approved~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow([$"Status^Approved~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Action Task|Receive Cargo - Road Task #1");

            var pinNumber = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
               $"Status^Approved~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}", @"Pin", ClickType.None);

            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: $"Road Gate {TerminalId}", vehicleId: TestCaseNumber);
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            roadGateForm.txtNewItem.SetValue(pinNumber);
            Keyboard.Press(VirtualKeyShort.TAB);

            // Should open Receive Container form
            RoadGateDetailsReceiveForm receiveFullContainerForm = new RoadGateDetailsReceiveForm($"Receive Full Container {TerminalId}");

            // Click Save button Receive Container form
            receiveFullContainerForm.BtnSave.DoClick();

            // Click Save button on Warnings window
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out");

            // Click Save on Road Gate form
            roadGateForm.btnSave.DoClick();

            _workOrderEnquiryForm.SetFocusToForm();
            _workOrderEnquiryForm.DoWorkOrderOperations();

            //Allocate Jobs to gangs
            _workOrderOperationsForm = new WorkOrderOperationsForm($"Work Order Operations {TerminalId}");
            _workOrderOperationsForm.cmbOperationMode.SetValue(@"Road");
            _workOrderOperationsForm.chkJobSheet.DoClick(false);
            _workOrderOperationsForm.chkFilterCriteria.DoClick();
            _workOrderOperationsForm.GetFilterCriteria();
            _workOrderOperationsForm.cmbShiftFrom.SetValue(@" ");
            _workOrderOperationsForm.cmbShiftTo.SetValue(@" ");
            _workOrderOperationsForm.GetTabTableGeneric(@"Unallocated");

            //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric,
            //    @"Job Type^Receive Cargo - Road~Vehicle Id^" + TestCaseNumber + "~Work Order^" + workOrderId + "",
            //    ClickType.ContextClick, rowHeight: 16);
            _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
                [$"Job Type^Receive Cargo - Road~Vehicle Id^{TestCaseNumber}~Work Order^{workOrderId}"],
                ClickType.ContextClick);
            _workOrderOperationsForm.ContextMenuSelect(@"Allocate to job sheet...|ROADGANG");
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Allocate to job sheet...");
            _confirmationFormYesNo.btnYes.DoClick();

            _workOrderOperationsForm.GetTabTableGeneric(@"Allocated");
            //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric,
            //    @"Work Order^" + workOrderId + "~Vehicle Id^" + TestCaseNumber + "~Job Type^Receive Cargo - Road",
             //   ClickType.Click, rowHeight: 16);
            _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
                [$"Work Order^{workOrderId}~Vehicle Id^{TestCaseNumber}~Job Type^Receive Cargo - Road"]);

            _workOrderEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                // $"Status^Approved~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.Click);
            _workOrderEnquiryForm.TblData.FindClickRow([$"Status^Approved~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.Click);

            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();

            // Find the vehicle, complete road job and do Road Exit
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(RoadOperationsForm.FormTitle, new[] { "58654" });

            _workOrderEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                // $"Status^Complete~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow([$"Status^Complete~Work Order Description^Receive Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);

            _workOrderEnquiryForm.ContextMenuSelect(@"Set Archived");
        }

        #region SetUp and Load Data
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_58654_";
            
            //Delete Cargo Onsite
            CreateDataFileToLoad(@"DeleteCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='PEL'>\n         <TestCases>58654</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG58654A01</id>\n         <isoType>2200</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>BS01</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>VOYWORKORDERS</voyageCode>\n         <dischargePort>NZBLU</dischargePort>\n		 <messageMode>D</messageMode>\n      </CargoOnSite>\n       </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //File to Delete Booking
            CreateDataFileToLoad(@"Booking_Del.xml",
                "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <BookingHeader Terminal='PEL'>\n	<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>BOOK58654A01</id>\n	<DischargePort>NZBLU</DischargePort>\n	<voyageCode>VOYWORKORDERS</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>D</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>BOOK58654A01</id>\n		<IsoGroup>22G0</IsoGroup>\n		<number>2</number>\n		<totalWeight>20000</totalWeight>\n		<totalQuantity>1</totalQuantity>\n		<commodityDescription>GEN</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n		<messageMode>D</messageMode>	\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
        #endregion SetUp and Load Data
    }
}





