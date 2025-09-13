using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using FlaUI.Core.WindowsAPI;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.Gate_Functions.Bookings.BookingItemForm;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNWebPages.PageObjects.Mobile_Apps.Order_Picking;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Prenotes
{
    [TestClass, TestCategory(TestCategories.MTN)]

    public class TestCase40651 : MTNBase
    {
        private BookingForm _bookingForm;
        private BookingItemsForm _addBookingForm;
        private BookingItemFormCont _addBookingItemForm;
        private PreNoteForm _preNotesForm;
        private RoadGateDetailsReceiveForm _preNoteFullContForm;
        private PreNotePickerForm _preNotePickerForm;
        private RoadGatePickerForm _pickerForm;
        private AttachmentReceiveReleaseContainerForm _attachmentForm;
        private ConfirmationFormOK _messageBox;

        private const string TestCaseNumber = @"40651";
       
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }
        
        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);

            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void GateInPrenote()
        {
            MTNInitialize();

            // Open Gate Functions | Booking form | Click New button
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking");
            FormObjectBase.MainForm.OpenBookingFromToolbar();
            _bookingForm = new BookingForm(@"Booking TT1");
            //_bookingForm.btnAdd.DoClick(1000);
            _bookingForm.DoNew();

            // Enter Booking Ref, Operator, Voyage, Discharge Port and click Add button
            _addBookingForm = new BookingItemsForm(@"Adding Booking  TT1");
            _addBookingForm.txtReference.SetValue(@"40651BKNG");
            _addBookingForm.cmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _addBookingForm.cmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            _addBookingForm.cmbDischargePort.SetValue(Port.AKLNZ, doDownArrow: true);
            _addBookingForm.btnAdd.DoClick();

            // Enter Cargo Type, ISO Group, Commodity, Booking Quantity, booking Weight and click OK button
            _addBookingItemForm = new BookingItemFormCont(@"Adding Booking Item for  TT1");
            //_addBookingItemForm.cmbCargoType.SetValue(CargoType.ISOContainer, doDownArrow: true, searchSubStringTo: 10);
            _addBookingItemForm.cmbISOGroup.SetValue(@"22G0  20 Standard Dry", doDownArrow: true);
            _addBookingItemForm.cmbCommodity.SetValue(Commodity.GEN, doDownArrow: true, searchSubStringTo: 3);
            _addBookingItemForm.txtBookingQuantity.SetValue(@"20");
            _addBookingItemForm.mtBookingWeight.SetValueAndType("20000");
            _addBookingItemForm.btnOK.DoClick();

            // Click Add button
            _addBookingForm.btnAdd.DoClick();

            // Enter Cargo Type, Commodity, Booking Quantity, booking Weight and click OK button
            _addBookingItemForm = new BookingItemFormCont(@"Adding Booking Item for  TT1");
            _addBookingItemForm.cmbCargoType.SetValue(CargoType.BagOfSand, doDownArrow: true, searchSubStringTo:10);
            _addBookingItemForm.cmbCommodity.SetValue(Commodity.SANC, doDownArrow: true);
            _addBookingItemForm.txtBookingQuantity.SetValue(@"10");
            _addBookingItemForm.mtBookingWeight.SetValueAndType("10000");
            _addBookingItemForm.btnOK.DoClick(10);

            // Click the Save button
            _addBookingForm.btnSave.DoClick();

            // Open Gate Functions | Pre-Notes
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Pre-Notes", forceReset: true);
            FormObjectBase.MainForm.OpenPreNotesFromToolbar();
            
            // Click New button
            _preNotesForm = new PreNoteForm(@"Pre-Notes TT1");
            //_preNotesForm.btnAdd.DoClick();
            _preNotesForm.DoNew();

            // Enter Booking and press Tab key
            _preNoteFullContForm = new RoadGateDetailsReceiveForm(@"PreNote Full Container TT1");
            _preNoteFullContForm.TxtBooking.SetValue(@"40651BKNG");
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);

            // Select the Booking item with Cargo type - ISO Container and click OK button
            _preNotePickerForm = new PreNotePickerForm(@"Picker");
            //Keyboard.Press(VirtualKeyShort.DOWN);
            //MTNControlBase.FindClickRowInTable(_preNotePickerForm.tblPickerItems, @"Cargo Type^ISO Container", clickType: ClickType.Click);
            _preNotePickerForm.TblPickerItems.FindClickRow(new[] { "Cargo Type^ISO Container~Description^20 x 22G0" });
            _preNotePickerForm.btnOK.DoClick();

            // Enter ISO Type, Cargo ID and Total Weight
            _preNoteFullContForm.CmbIsoType.SetValue(ISOType.ISO220A, doDownArrow: true);
            _preNoteFullContForm.TxtCargoId.SetValue(@"JLG40651A01");
            _preNoteFullContForm.MtTotalWeight.SetValueAndType("200000");
            // Click the ellipsis button (...) to add attachment
            _preNoteFullContForm.BtnAttachments.DoClick();
            _attachmentForm = new AttachmentReceiveReleaseContainerForm(@"Attachment Details Container TT1");

            // Enter Booking Item, Cargo ID, Total weight and click Save button
            _attachmentForm.TxtBooking.SetValue("40651BKNG");
            _attachmentForm.CmbBookingItem.SetValue("10 x Bag of Sand BIGSAND");
            _attachmentForm.TxtCargoId.SetValue("JLG40651A02");
            _attachmentForm.MtTotalWeight.SetValueAndType("20000");
            _attachmentForm.BtnSave.DoClick();
           
            // Click Save button on PreNote Full Container form
            _preNoteFullContForm.BtnSave.DoClick();

            // Click Save button on Warnings window
            /*warningErrorForm = new WarningErrorForm(@"Warnings for Pre-Notes TT1");
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Pre-Notes");

            // Click OK button on message box
            _messageBox = new ConfirmationFormOK(@"Pre-Note Added", automationIdMessage: @"3", automationIdOK: @"4");
            _messageBox.btnOK.DoClick();

            // Open Gate Functions | Road Gate
            //FormObjectBase.NavigationMenuSelection("Road Gate");
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();

            // Enter Registration, Carrier, Gate, New Item and press Tab key
            roadGateForm = new RoadGateForm(@"Road Gate TT1");
            /*roadGateForm.txtRegistration.SetValue(@"40651", 100);
            roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            roadGateForm.cmbGate.SetValue(@"GATE");*/
            roadGateForm.SetRegoCarrierGate("40651");
            roadGateForm.txtNewItem.SetValue(@"40651BKNG", 100);
            //Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);

            // Select the Pre-Notified Item as shown in the screenshot and click the OK button
            _pickerForm = new RoadGatePickerForm(@"Picker");
            //MTNControlBase.FindClickRowInTable(_pickerForm.tblPickerItems, @"Type^Pre Notified Cargo", clickType: ClickType.Click);
            _pickerForm.TblPickerItems.FindClickRow(new[] { "Type^Pre Notified Cargo" });
            _pickerForm.btnOK.DoClick();

            // Should open Receive Container form
            RoadGateDetailsReceiveForm receiveFullContainerForm = new RoadGateDetailsReceiveForm(@"Receive Full Container TT1");

            // Click Save button Receive Container form
            //FormObjectBase.ClickButton(receiveFullContainerForm.btnSave);
            receiveFullContainerForm.BtnSave.DoClick();

            // Click Save button on Warnings window
            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            // Click Save on Road Gate form
            roadGateForm.btnSave.DoClick();

            // In Cargo Enquiry, check that the Children field has a child item JLG40651A02 
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG40651A01");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            string fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Children");
            string textToCheck = @"JLG40651A02";
            Assert.IsTrue(fieldValue == textToCheck, @"Field: Children has a value of " + fieldValue.ToUpper() + @" and should equal: " + textToCheck);

            // Click the Move It button in the toolbar
            //cargoEnquiryForm.btnMoveIt.DoClick();
            cargoEnquiryForm.DoMoveIt();

           
            // fields to set
            string[] fieldsToSet =
            {
                @"Move Mode^Queued Move",
                @"To Terminal Area^MKBS07"
                };

            // warning message(s) to check
            /*// Monday, 7 April 2025 navmh5 Can be removed 6 months after specified date 
            string[] warningErrorToCheck =
                {
                @"Code :82959. Item JLG40651A01 does not match Allocation E 44284 at location MKBS07"
                };*/

            cargoEnquiryForm.MoveCargoItemUsingMoveItForm(fieldsToSet,
                new[] { "Code :82959. Item JLG40651A01 does not match Allocation E 44284 at location MKBS07" });


            // Open Yard Functions | Road Operations
            //FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            
            // Find the vehicle 40651, complete road job and do Road Exit
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            MTNControlBase.FindClickRowInList(roadOperationsForm.lstVehicles, @"40651 (1/0) - ICA - Yard Interchange");

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^40651~Cargo ID^JLG40651A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Vehicle Id^40651~Cargo ID^JLG40651A01" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");

            /*warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Operations Move TT1");
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Operations Move");

            MTNControlBase.FindClickRowInList(roadOperationsForm.lstVehicles, @"40651 (1/0) - ICA - Yard Interchange");
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^40651~Cargo ID^JLG40651A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Vehicle Id^40651~Cargo ID^JLG40651A01" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^40651~Cargo ID^JLG40651A01", ClickType.None, doAssert: false);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Vehicle Id^40651~Cargo ID^JLG40651A01" }, ClickType.None, doAssert: false);
            roadOperationsForm.CloseForm();
        }


        

        public static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber + "_";

            //Create Data Files to load
            //File to Delete Booking
            CreateDataFileToLoad(@"Booking_Del.xml",
                "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <BookingHeader Terminal='TT1'>\n	<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>40651BKNG</id>\n	<DischargePort>NZAKL</DischargePort>\n	<voyageCode>MSCK000002</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>D</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>40651BKNG</id>\n		<IsoGroup>22G0</IsoGroup>\n		<number>2</number>\n		<totalWeight>20000</totalWeight>\n		<totalQuantity>20</totalQuantity>\n		<commodityDescription>GEN</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n		<messageMode>D</messageMode>	\n    </BookingItem>\n	<BookingItem>\n		<id>40651BKNG</id>	\n		<totalWeight>10000</totalWeight>\n		<commodityDescription>SANC</commodityDescription>\n		<cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n		<messageMode>D</messageMode>	\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n");

            //File to Delete CargoOnsite
            CreateDataFileToLoad(@"Cargo_Del.xml", "" +
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40651A01</id>\n            <operatorCode>MSC</operatorCode>\n            <locationId>MKBS07</locationId>\n            <weight>200000</weight>\n            <commodity>GEN</commodity>\n  <tareWeight>1000</tareWeight>\n          <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <isoType>220A</isoType>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }
}
