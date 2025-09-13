using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.Gate_Functions.Bookings.BookingItemForm;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using System.Drawing;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Bookings
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39312 : MTNBase
    {

        private RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        private BookingForm _bookingForm;
   
        private string[] _warningErrorToCheck;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"resetData_39312");
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void SplitBookingWithPrenotedItem()
        {
            MTNInitialize();
            
            // Step 4
            // Monday, 3 February 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Pre-Notes", forceReset: true);
            FormObjectBase.MainForm.OpenPreNotesFromToolbar();
            var preNoteForm = new PreNoteForm(formTitle: @"Pre-Notes TT1");

            preNoteForm.cmbTransportMode.SetValue("Road");

            MTNControlBase.SetValueInEditTable(preNoteForm.tblPreNoteSearch, @"Cargo Id", @"JLG39312");
            preNoteForm.DoSearch();

            // Monday, 3 February 2025 navmh5 
           bool matchFound =
                MTNControlBase.FindClickRowInTable(preNoteForm.TblPreNotes.GetElement(), @"ID^JLG39312", ClickType.None, doAssert: false);
            Assert.IsFalse(matchFound, @"TestCase39132 - Prenote should not exist");
            /////var matchFound = preNoteForm.TblPreNotes.FindClickRow(new[] { "ID^JLG39312" }, ClickType.None, doAssert: false);
            ////Assert.IsTrue(!string.IsNullOrEmpty(matchFound), "TestCase39132 - Prenote should not exist");
            
            // Step 5 - 7
            preNoteForm.DoNew();
            var preNoteDetailsForm = new RoadGateDetailsReceiveForm(formTitle: @"PreNote Full Container TT1");
       
            preNoteDetailsForm.TxtBooking.SetValue(@"JLGBOOK39312A01");
            PreNotePickerForm preNotePickerForm = new PreNotePickerForm(@"Picker");
            // MTNControlBase.FindClickRowInTable(preNotePickerForm.tblPickerItems, @"Description^3 x 220A  General With Tare Weight~Cargo Type^ISO Container", 
                // clickType: ClickType.DoubleClick);
            preNotePickerForm.TblPickerItems.FindClickRow(["Description^3 x 220A  General With Tare Weight~Cargo Type^ISO Container"], clickType: ClickType.DoubleClick);            /////preNotePickerForm.TblPickerItems.FindClickRow(
            /////    new[] { "Description^3 x 220A  General With Tare Weight~Cargo Type^ISO Container" },
            /////    ClickType.DoubleClick);
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(1));
            Wait.UntilResponsive(preNoteDetailsForm.TxtCargoId.GetElement());
            preNoteDetailsForm.TxtCargoId.SetValue(@"JLG39312A01");
            preNoteDetailsForm.BtnSave.DoClick();

            /*// Monday, 3 February 2025 navmh5 
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Pre-Notes TT1");
            warningErrorForm.CheckWarningsErrorsExist(new [] { "Code :75016. The Container Id (JLG39312A01) failed the validity checks and may be incorrect." });
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Pre-Notes TT1",
                new[]
                { "Code :75016. The Container Id (JLG39312A01) failed the validity checks and may be incorrect." });

            ConfirmationFormOK messageBox = new ConfirmationFormOK(@"Pre-Note Added", automationIdMessage: @"3", 
                automationIdOK: @"4");
            string pinNo = messageBox.ReturnPIN();
            messageBox.btnOK.DoClick();

            // Step 8 - 11
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"39312");

            roadGateForm.SetRegoCarrierGate("39312");
            roadGateForm.txtNewItem.SetValue(pinNo);
            
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Empty Container TT1");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, 
                // @"Type^Receive Empty~Detail^JLG39312A01; MSC; 220A~Booking/Release^JLGBOOK39312A01");
            roadGateForm.TblGateItems.FindClickRow(["Type^Receive Empty~Detail^JLG39312A01; MSC; 220A~Booking/Release^JLGBOOK39312A01"]);            /////roadGateForm.TblGateItems.FindClickRow(
            /////    new[] { "Type^Receive Empty~Detail^JLG39312A01; MSC; 220A~Booking/Release^JLGBOOK39312A01" });
            roadGateForm.btnSave.DoClick();
            roadGateForm.CloseForm();

            // Step 12
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            // Monday, 3 February 2025 navmh5 
            //MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG39312A01", clickType: ClickType.Click, 
            //    rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Cargo ID^JLG39312A01" });
            roadOperationsForm.DoMoveIt();

            // Step 13
            FormObjectBase.MainForm.OpenBookingFromToolbar();
            _bookingForm = new BookingForm(@"Booking TT1");
            MTNControlBase.SetValueInEditTable(_bookingForm.tblSearcher, @"Booking", @"JLGBOOK39312");
            _bookingForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings, @"Booking^JLGBOOK39312");
            _bookingForm.TblBookings.FindClickRow(["Booking^JLGBOOK39312"]);            /////_bookingForm.TblBookings.FindClickRow(new[] { "Booking^JLGBOOK39312" });
            _bookingForm.GetItemTableDetails();
            _bookingForm.CheckItemDetailsTableForText(@"Cargo Type: ISO Container; ISO Type: 220A  General With Tare Weight; Commodity: MT; Items: 3; Received: 1; Items Received: JLG39312A01");

            // Step 15
            _bookingForm.DoNew();

            var bookingItemsForm = new BookingItemsForm();
            bookingItemsForm.txtReference.SetValue(@"JLGBOOK39312A01A");
            bookingItemsForm.cmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            bookingItemsForm.cmbVoyage.SetValue(TT1.Voyage.MESDAI200001, doDownArrow: true, searchSubStringTo: TT1.Voyage.MESDAI200001.Length - 5);
            bookingItemsForm.cmbDischargePort.SetValue(Port.NSNNZ, doDownArrow: true);
            bookingItemsForm.btnAdd.DoClick();

            // Step 16
            BookingItemForm bookingItemForm = new BookingItemForm(@"Adding Booking Item for  TT1");
            bookingItemForm.GetISOContainerDetails();
            bookingItemForm.cmbCargoType.SetValue(CargoType.ISOContainer, doDownArrow: true);
            bookingItemForm.cmbISOType.SetValue(ISOType.ISO220A, doDownArrow: true);
            bookingItemForm.cmbCommodity.SetValue(Commodity.MT, doDownArrow: true/*, searchSubStringTo: 2*/);
            bookingItemForm.txtBookingQuantity.SetValue(@"3");
            bookingItemForm.mtBookingWeight.SetValueAndType("12500", "lbs");
            bookingItemForm.btnOK.DoClick();
            
            // Step 17
            bookingItemsForm.SetFocusToForm();
            
            bookingItemsForm.btnSave.DoClick();

            _bookingForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings,
                // @"Booking^JLGBOOK39312A01A~ISO Type^220A~Items^3~Received^0~Released^0~Reserved^0~Pre-noted^0");
            // MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings,
                // @"Booking^JLGBOOK39312A01~ISO Type^2230, 220A~Items^8~Received^1~Released^0~Reserved^0~Pre-noted^0");
            _bookingForm.TblBookings.FindClickRow([
                "Booking^JLGBOOK39312A01A~ISO Type^220A~Items^3~Received^0~Released^0~Reserved^0~Pre-noted^0",
                "Booking^JLGBOOK39312A01~ISO Type^2230, 220A~Items^8~Received^1~Released^0~Reserved^0~Pre-noted^0"
            ]);
            /////_bookingForm.TblBookings.FindClickRow(new[]
            /////    {
            /////        "Booking^JLGBOOK39312A01A~ISO Type^220A~Items^3~Received^0~Released^0~Reserved^0~Pre-noted^0",
            /////        "Booking^JLGBOOK39312A01~ISO Type^2230, 220A~Items^8~Received^1~Released^0~Reserved^0~Pre-noted^0"
            /////    });
          
            
            // Step 18
            _bookingForm.DoEdit();

            // Step 19
            bookingItemsForm = new BookingItemsForm();
            bookingItemsForm.btnSplit.DoClick();

            SplitBookingForm splitBookingForm = new SplitBookingForm(@"Split Booking TT1");
            splitBookingForm.cmbNewVoyage.SetValue(@"MESDAI200001 - Jolly Diamante 2 OFFICIAL", doDownArrow: true, searchSubStringTo: 12);
            splitBookingForm.cmbNewBooking.SetValue(@"JLGBOOK39312A01A");
            splitBookingForm.txtRemarks.SetValue(@"Splitting Booking to JLGBOOK39312A01A");
            MTNControlBase.SetUnsetValueCheckboxTable(splitBookingForm.tblSelectToMove, new [,] { { "JLG39312A01", "1" } });
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            splitBookingForm.btnSave.DoClick();

            // Step 20
            bookingItemsForm.SetFocusToForm();
            bookingItemsForm.btnCancel.DoClick();

            // Step 21 - 22
            _bookingForm.SetFocusToForm();
            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings,
            // Monday, 3 February 2025 navmh5     @"Booking^JLGBOOK39312A01~ISO Type^2230, 220A~Items^7~Received^0~Released^0~Reserved^0~Pre-noted^0");
            _bookingForm.TblBookings.FindClickRow(new[]
                { "Booking^JLGBOOK39312A01~ISO Type^2230, 220A~Items^7~Received^0~Released^0~Reserved^0~Pre-noted^0" });
            _bookingForm.CheckItemDetailsTableForText(@"Cargo Type: ISO Container; ISO Type: 220A  General With Tare Weight; Commodity: MT; Items: 2; Received: 0");
            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings,
            // Monday, 3 February 2025 navmh5     @"Booking^JLGBOOK39312A01A~ISO Type^220A~Items^4~Received^1~Released^0~Reserved^0~Pre-noted^0");
            _bookingForm.TblBookings.FindClickRow(new[]
                { "Booking^JLGBOOK39312A01A~ISO Type^220A~Items^4~Received^1~Released^0~Reserved^0~Pre-noted^0" });
            _bookingForm.CheckItemDetailsTableForText(@"Cargo Type: ISO Container; ISO Type: 220A  General With Tare Weight; ISO Group: 22G0; Commodity: MT; Items: 4; Received: 1; Items Received: JLG39312A01");
            

            // Step 23
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            FormObjectBase.NavigationMenuSelection(@"System Ops|Event Enquiry", forceReset: true);
            EventEnquiryForm eventEnquiryForm = new EventEnquiryForm(@"System Event Enquiry");
            eventEnquiryForm.GetSearcherDetails();

            // Step 24 - 25
            eventEnquiryForm.GetSearcherDateRange();
            eventEnquiryForm.txtDateFrom.SetValue(DateTime.Now.ToString(@"ddMMyyyy"));
            eventEnquiryForm.GetSearcherEventType();

            MTNControlBase.SetUnsetValueCheckboxTable(eventEnquiryForm.tblEventTypes, new [,] { { "Split Booking   (System defined)", @"1" }  });
            eventEnquiryForm.DoSearch();

            //eventEnquiryForm.GetMainTable();

            eventEnquiryForm.TblMain.GetElement().Focus();
            Point point= new Point(eventEnquiryForm.TblMain.GetElement().BoundingRectangle.X + 50, 
                eventEnquiryForm.TblMain.GetElement().BoundingRectangle.Y + 5);
            Mouse.DoubleClick(point);

            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(eventEnquiryForm.tblMain,
            // Monday, 3 February 2025 navmh5     @"Event Type^Split Booking~User^USERDWAT~Error Text^Split FROM booking JLGBOOK39312A01 on voyage MSCK000002, discharge port NZBLU TO booking JLGBOOK39312A01A on voyage MESDAI200001, discharge port NZNSN (Splitting Booking to JLGBOOK39312A01A)");
            eventEnquiryForm.TblMain.FindClickRow(new[]
                { "Event Type^Split Booking~User^USERDWAT~Error Text^Split FROM booking JLGBOOK39312A01 on voyage MSCK000002, discharge port NZBLU TO booking JLGBOOK39312A01A on voyage MESDAI200001, discharge port NZNSN (Splitting Booking to JLGBOOK39312A01A)" });
        
            // Step 26 - 31
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"39312A");

            roadGateForm.SetRegoCarrierGate("39312A");
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK39312A01", 10);
            var roadGatePickerForm = new RoadGatePickerForm(@"Picker");
            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(roadGatePickerForm.tblPickerItems, @"Description^x 220A  General With Tare Weight~Cargo Type^ISO Container",
            // Monday, 3 February 2025 navmh5     clickType: ClickType.DoubleClick);
            roadGatePickerForm.TblPickerItems.FindClickRow(
                new[] { "Description^x 220A  General With Tare Weight~Cargo Type^ISO Container" },
                ClickType.DoubleClick);

            // Step 28
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive Empty Container TT1");
            _roadGateDetailsReceiveForm.AddMultipleCargoIds(@"JLG39312A02", @"2");

            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("15000");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            /*
            // Monday, 3 February 2025 navmh5 
            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.CheckWarningsErrorsExist(new [] { "Code :75016. The Container Id (JLG39312A02) failed the validity checks and may be incorrect." });
             warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out TT1",
                new[]
                {
                    "Code :75016. The Container Id (JLG39312A02) failed the validity checks and may be incorrect."
                });

            // Step 32 - 37
            roadGateForm.SetFocusToForm();
            /*// Monday, 3 February 2025 navmh5 
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Empty~Detail^JLG39312A02; MSC; 220A~Booking/Release^JLGBOOK39312A01");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Empty~Detail^JLG39312A03; MSC; 220A~Booking/Release^JLGBOOK39312A01");*/
            roadGateForm.TblGateItems.FindClickRow(
                new[]
                {
                    "Type^Receive Empty~Detail^JLG39312A02; MSC; 220A~Booking/Release^JLGBOOK39312A01",
                    "Type^Receive Empty~Detail^JLG39312A03; MSC; 220A~Booking/Release^JLGBOOK39312A01"
                });

            roadGateForm.txtNewItem.SetValue(@"JLGBOOK39312A01", 10);
            roadGatePickerForm = new RoadGatePickerForm(@"Picker");
            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(roadGatePickerForm.tblPickerItems, @"Description^ x 2230  Powered Reefer~Cargo Type^ISO Container",
            // Monday, 3 February 2025 navmh5     clickType: ClickType.DoubleClick);
            roadGatePickerForm.TblPickerItems.FindClickRow(
                new[] { "Description^ x 2230  Powered Reefer~Cargo Type^ISO Container" },
                ClickType.DoubleClick);

            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive Full Reefer TT1");
            _roadGateDetailsReceiveForm.AddMultipleCargoIds(@"JLG39312A04", @"5");

            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("13500");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            _warningErrorToCheck = new string[]
            {
                "Code :75016. The Container Id (JLG39312A04) failed the validity checks and may be incorrect.",
                "Code :75353. Warning: The Operator MSC requires a humidity setting."
            };
            warningErrorForm.CheckWarningsErrorsExist(_warningErrorToCheck);
             warningErrorForm.btnSave.DoClick();

            // Step 38 - 41
            roadGateForm.SetFocusToForm();
            /*// Monday, 3 February 2025 navmh5 
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Full~Detail^JLG39312A04; MSC; 2230~Booking/Release^JLGBOOK39312A01");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Full~Detail^JLG39312A05; MSC; 2230~Booking/Release^JLGBOOK39312A01");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Full~Detail^JLG39312A06; MSC; 2230~Booking/Release^JLGBOOK39312A01");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Full~Detail^JLG39312A07; MSC; 2230~Booking/Release^JLGBOOK39312A01");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Full~Detail^JLG39312A08; MSC; 2230~Booking/Release^JLGBOOK39312A01");*/
            roadGateForm.TblGateItems.FindClickRow(new[]
                {
                    "Type^Receive Full~Detail^JLG39312A04; MSC; 2230~Booking/Release^JLGBOOK39312A01",
                    "Type^Receive Full~Detail^JLG39312A05; MSC; 2230~Booking/Release^JLGBOOK39312A01",
                    "Type^Receive Full~Detail^JLG39312A06; MSC; 2230~Booking/Release^JLGBOOK39312A01",
                    "Type^Receive Full~Detail^JLG39312A07; MSC; 2230~Booking/Release^JLGBOOK39312A01",
                    "Type^Receive Full~Detail^JLG39312A08; MSC; 2230~Booking/Release^JLGBOOK39312A01"
                });
            
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK39312A01A", 10);

            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive Empty Container TT1");
            _roadGateDetailsReceiveForm.AddMultipleCargoIds("JLG39312A09\nJLG39312A10\nJLG39312A11");
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("9500");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            /*// Monday, 3 February 2025 navmh5 
            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            _warningErrorToCheck = new string[]
            { 
                "Code :75016. The Container Id (JLG39312A09) failed the validity checks and may be incorrect."
            };
            warningErrorForm.CheckWarningsErrorsExist(_warningErrorToCheck);
             warningErrorForm.btnSave.DoClick();*/
             WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out TT1",
                new[]
                { "Code :75016. The Container Id (JLG39312A09) failed the validity checks and may be incorrect." });

            // Step 42
            roadGateForm.SetFocusToForm();
            /*// Monday, 3 February 2025 navmh5 
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Empty~Detail^JLG39312A09; MSC; 220A~Booking/Release^JLGBOOK39312A01A");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Empty~Detail^JLG39312A09; MSC; 220A~Booking/Release^JLGBOOK39312A01A");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Empty~Detail^JLG39312A09; MSC; 220A~Booking/Release^JLGBOOK39312A01A");*/
            roadGateForm.TblGateItems.FindClickRow(new[]
                {
                    "Type^Receive Empty~Detail^JLG39312A09; MSC; 220A~Booking/Release^JLGBOOK39312A01A",
                    "Type^Receive Empty~Detail^JLG39312A10; MSC; 220A~Booking/Release^JLGBOOK39312A01A",
                    "Type^Receive Empty~Detail^JLG39312A11; MSC; 220A~Booking/Release^JLGBOOK39312A01A"
                });

            roadGateForm.btnSave.DoClick();

            // Step 43
            _bookingForm = new BookingForm(@"Booking TT1");
            MTNControlBase.SetValueInEditTable(_bookingForm.tblSearcher, @"Booking", @"JLGBOOK39312");
            //_bookingForm.btnSearch.DoClick();
            _bookingForm.DoSearch();

            // Step 44
            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings,
            // Monday, 3 February 2025 navmh5     @"Booking^JLGBOOK39312A01~ISO Type^2230, 220A~Items^7~Received^7~Released^0~Reserved^0~Pre-noted^0");
            _bookingForm.TblBookings.FindClickRow(new[]
                { "Booking^JLGBOOK39312A01~ISO Type^2230, 220A~Items^7~Received^7~Released^0~Reserved^0~Pre-noted^0" });
            _bookingForm.GetItemTableDetails();
            _bookingForm.CheckItemDetailsTableForText(@"Cargo Type: ISO Container; ISO Type: 2230  Powered Reefer; Commodity: ICEC; Items: 5; Carriage Temperature (deg C): -1.0; Carriage Temperature (deg F): +30.2; Received: 5; Items Received: JLG39312A08, JLG39312A07, JLG39312A06, JLG39312A05, JLG39312A04");
            _bookingForm.CheckItemDetailsTableForText(@"Cargo Type: ISO Container; ISO Type: 220A  General With Tare Weight; Commodity: MT; Items: 2; Received: 2; Items Received: JLG39312A03, JLG39312A02");

            // Step 45
            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings,
            // Monday, 3 February 2025 navmh5     @"Booking^JLGBOOK39312A01A~ISO Type^220A~Items^4~Received^4~Released^0~Reserved^0~Pre-noted^0");
            _bookingForm.TblBookings.FindClickRow(new[]
                { "Booking^JLGBOOK39312A01A~ISO Type^220A~Items^4~Received^4~Released^0~Reserved^0~Pre-noted^0" });
            _bookingForm.CheckItemDetailsTableForText(@"Cargo Type: ISO Container; ISO Type: 220A  General With Tare Weight; ISO Group: 22G0; Commodity: MT; Items: 4; Received: 4; Items Received: JLG39312A11, JLG39312A10, JLG39312A09, JLG39312A01");
           
            // Step 46
            //FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            this.roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            roadOperationsForm.MoveQueuedAllCargo(new[] { "39312A" });
            roadOperationsForm.CloseForm();

            // Step 47 - 49
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            /*// Tuesday, 22 April 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG39312A");
            cargoEnquiryForm.DoSearch();

            // Step 48
            /#1#/ Monday, 3 February 2025 navmh5 
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Booking^JLGBOOK39312A01A~ID^JLG39312A01~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001",ClickType.None);
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Booking^JLGBOOK39312A01A~ID^JLG39312A09~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001", ClickType.None);
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Booking^JLGBOOK39312A01A~ID^JLG39312A10~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001", ClickType.None);
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Booking^JLGBOOK39312A01A~ID^JLG39312A11~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001", ClickType.None);#1#
            cargoEnquiryForm.tblData2.FindClickRow(new[]
                {
                    "Booking^JLGBOOK39312A01A~ID^JLG39312A01~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001",
                    "Booking^JLGBOOK39312A01A~ID^JLG39312A09~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001",
                    "Booking^JLGBOOK39312A01A~ID^JLG39312A10~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001",
                    "Booking^JLGBOOK39312A01A~ID^JLG39312A11~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001"
                }, clickType: ClickType.None);

            // Step 49
            cargoEnquiryForm.DoSearch();
            /#1#/ Monday, 3 February 2025 navmh5 
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Booking^JLGBOOK39312A01~ID^JLG39312A02~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002", ClickType.None);
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Booking^JLGBOOK39312A01~ID^JLG39312A03~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002", ClickType.None);
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Booking^JLGBOOK39312A01~ID^JLG39312A04~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002", ClickType.None);
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Booking^JLGBOOK39312A01~ID^JLG39312A05~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002", ClickType.None);
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Booking^JLGBOOK39312A01~ID^JLG39312A06~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002", ClickType.None);
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Booking^JLGBOOK39312A01~ID^JLG39312A07~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002", ClickType.None);
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Booking^JLGBOOK39312A01~ID^JLG39312A08~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002", ClickType.None);#1#
            cargoEnquiryForm.tblData2.FindClickRow(new[]
                {
                    "Booking^JLGBOOK39312A01~ID^JLG39312A02~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                    "Booking^JLGBOOK39312A01~ID^JLG39312A03~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                    "Booking^JLGBOOK39312A01~ID^JLG39312A04~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                    "Booking^JLGBOOK39312A01~ID^JLG39312A05~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                    "Booking^JLGBOOK39312A01~ID^JLG39312A06~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                    "Booking^JLGBOOK39312A01~ID^JLG39312A07~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                    "Booking^JLGBOOK39312A01~ID^JLG39312A08~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002"
                }, clickType: ClickType.None);*/
            
            ValidateRowsExistCargoEnquiry("JLG39312A01,JLG39312A09,JLG39312A10,JLG39312A11", [
                "Booking^JLGBOOK39312A01A~ID^JLG39312A01~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001",
                "Booking^JLGBOOK39312A01A~ID^JLG39312A09~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001",
                "Booking^JLGBOOK39312A01A~ID^JLG39312A10~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001",
                "Booking^JLGBOOK39312A01A~ID^JLG39312A11~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001"]);
            ValidateRowsExistCargoEnquiry("JLG39312A02, JLG39312A03,JLG39312A04,JLG39312A05,JLG39312A06,JLG39312A07,JLG39312A08", [
                "Booking^JLGBOOK39312A01~ID^JLG39312A02~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                "Booking^JLGBOOK39312A01~ID^JLG39312A03~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                "Booking^JLGBOOK39312A01~ID^JLG39312A04~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                "Booking^JLGBOOK39312A01~ID^JLG39312A05~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                "Booking^JLGBOOK39312A01~ID^JLG39312A06~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                "Booking^JLGBOOK39312A01~ID^JLG39312A07~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002",
                "Booking^JLGBOOK39312A01~ID^JLG39312A08~Opr^MSC~Discharge Port^NZBLU~Voyage^MSCK000002"]);
            
            // Step 50
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG39312A01");
            cargoEnquiryForm.DoSearch();
            
            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
            // Monday, 3 February 2025 navmh5     @"Booking^JLGBOOK39312A01A~ID^JLG39312A01~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001");
            cargoEnquiryForm.tblData2.FindClickRow(new[]
                { "Booking^JLGBOOK39312A01A~ID^JLG39312A01~Opr^MSC~Discharge Port^NZNSN~Voyage^MESDAI200001" });
            cargoEnquiryForm.DoViewTransactions();

            // Step 51
            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG39312A01 TT1");
            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
            // Monday, 3 February 2025 navmh5     @"Type^Split Booking for Cargo~Details^Booking Reference JLGBOOK39312A01 => JLGBOOK39312A01A Discharge Port NZBLU => NZNSN Voyage MSCK000002 = > MESDAI200001 (Splitting Booking to JLGBOOK39312A01A)");
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[]
                { "Type^Split Booking for Cargo~Details^Booking Reference JLGBOOK39312A01 => JLGBOOK39312A01A Discharge Port NZBLU => NZNSN Voyage MSCK000002 = > MESDAI200001 (Splitting Booking to JLGBOOK39312A01A)" });
            cargoEnquiryTransactionForm.CloseForm();
            
            cargoEnquiryForm.SetFocusToForm2();
            cargoEnquiryForm.CloseForm();   

            // Step 52 - 53
            RoadExitForm.RoadExitVehicles(new [] { "39312", "39312A" });
            
        }

        /// <summary>
        /// Validate Rows Exist in Cargo Enquiry
        /// Doing it this way as currently scroll issues. When Jade changes happen, this will be rewritten
        /// </summary>
        /// <param name="cargoIds">Cargo Id(s) to search for</param>
        /// <param name="rowDetails">Row details to validate</param>
        void ValidateRowsExistCargoEnquiry(string cargoIds, string[] rowDetails)
        {
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, CargoEnquiryForm.SearchFields.CargoID, cargoIds);
            cargoEnquiryForm.DoSearch();
            
            cargoEnquiryForm.tblData2.FindClickRow(rowDetails, clickType: ClickType.None);
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_39312_";

            // Delete Booking
            CreateDataFileToLoad(@"DeleteBooking.xml",
                "<?xml version='1.0'?>\n<JMTInternalBooking>\n	<AllBookingHeader>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<BookingHeader Terminal='TT1'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZBLU</dischargePort>\n			<id>JLGBOOK39312A01</id>\n			<messageMode>D</messageMode>\n			<operatorCode>MSC</operatorCode>\n			<vesselName>MSCK</vesselName>\n			<voyageCode>MSCK000002</voyageCode>\n		</BookingHeader>\n		<BookingHeader Terminal='TT1'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZBLU</dischargePort>\n			<id>JLGBOOK39312A01A</id>\n			<messageMode>D</messageMode>\n			<operatorCode>MSC</operatorCode>\n			<vesselName>DIAM2</vesselName>\n			<voyageCode>MESDAI200001</voyageCode>\n		</BookingHeader>\n	</AllBookingHeader>\n</JMTInternalBooking>\n");

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0'?> <JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39312A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZBLU</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>220A</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39312A02</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZBLU</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>220A</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39312A03</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZBLU</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>220A</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39312A04</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZBLU</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>220A</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39312A05</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZBLU</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>220A</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39312A06</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZBLU</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>220A</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39312A07</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZBLU</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>220A</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39312A08</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZBLU</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>220A</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39312A09</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZBLU</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>220A</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39312A10</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZBLU</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>220A</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39312A11</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZBLU</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>220A</isoType>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Booking
            CreateDataFileToLoad(@"CreateBooking.xml",
                "<?xml version='1.0'?>\n<JMTInternalBooking>\n	<AllBookingHeader>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<BookingHeader Terminal='TT1'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZBLU</dischargePort>\n			<id>JLGBOOK39312A01</id>\n			<messageMode>A</messageMode>\n			<operatorCode>MSC</operatorCode>\n			<vesselName>MSCK</vesselName>\n			<voyageCode>MSCK000002</voyageCode>\n				<AllBookingItem>\n					<BookingItem>\n						<weight>18830.0000</weight>\n						<number>5</number>\n						<temperature>-1</temperature>\n						<cargoTypeDescr>ISO Container</cargoTypeDescr>\n						<commodity>ICEC</commodity>\n						<fullOrMT>F</fullOrMT>\n						<id>JLGBOOK39312A01</id>\n						<isoType>2230</isoType>\n						<messageMode>A</messageMode>\n					</BookingItem>\n					<BookingItem>\n						<weight>25840.0000</weight>\n						<number>3</number>\n						<cargoTypeDescr>ISO Container</cargoTypeDescr>\n						<commodity>MT</commodity>\n						<fullOrMT>F</fullOrMT>\n						<id>JLGBOOK39312A01</id>\n						<isoType>220A</isoType>\n						<messageMode>A</messageMode>\n					</BookingItem>\n				</AllBookingItem>\n			</BookingHeader>\n	</AllBookingHeader>\n</JMTInternalBooking>\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
