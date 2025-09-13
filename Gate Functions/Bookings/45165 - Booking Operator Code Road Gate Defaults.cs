using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.Gate_Functions.Bookings.BookingItemForm;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using System.Diagnostics;
using DataObjects.LogInOutBO;
using FlaUI.Core.WindowsAPI;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Bookings
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45165 : MTNBase
    {

        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        ConfirmationFormYesNo _confirmationFormYesNo;

        string[] _msgToCheck;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //MTNSignon(TestContext, userName);
            LogInto<MTNLogInOutBO>();

            SetupAndLoadInitializeData(TestContext);
        }


        [TestMethod]
        public void BookingOperatorCodeRoadGateDefaults()
        {
            MTNInitialize();

            // Step 6 - 7
            RoadGateSetVehicleSection(@"45165");
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK45165", 10);

            // Step 8
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Full Container TT1");
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG45165A02");
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("8000");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            // Step 9
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");

            /*_msgToCheck = new string[] 
            {
                @"Code :75016. The Container Id (JLG45165A02) failed the validity checks and may be incorrect."
            };*/
            warningErrorForm.CheckWarningsErrorsExist(new [] { "Code :75016. The Container Id (JLG45165A02) failed the validity checks and may be incorrect." });
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Full~Detail^JLG45165A02; MSC; 2210~Booking/Release^JLGBOOK45165");
            roadGateForm.TblGateItems.FindClickRow(["Type^Receive Full~Detail^JLG45165A02; MSC; 2210~Booking/Release^JLGBOOK45165"]);
            // Step 10
            roadGateForm.btnSave.DoClick();
           
            // Step 11
            //FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");

            // Step 12
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^45165A~Cargo ID^JLG45165A02~Commodity^GEN", ClickType.DoubleClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { @"Vehicle Id^45165A~Cargo ID^JLG45165A02~Commodity^GEN" }, ClickType.DoubleClick);
            CargoEnquiryDirectForm cargoEnquiryDirectForm = new CargoEnquiryDirectForm();

            // Step 1
            //cargoEnquiryDirectForm.btnEdit.DoClick();
            cargoEnquiryDirectForm.DoEdit();
            MTNControlBase.SetValueInEditTable(cargoEnquiryDirectForm.tblGeneral, @"Operator", Operator.COS,
                EditRowDataType.ComboBoxEdit, waitTime: 200, fromCargoEnquiry: true, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryDirectForm.tblGeneral, @"Tare Weight", @"3500");
            Keyboard.Press(VirtualKeyShort.TAB);
            Keyboard.Press(VirtualKeyShort.TAB);
            Keyboard.Press(VirtualKeyShort.TAB);
            //cargoEnquiryDirectForm.btnSave.DoClick();
            cargoEnquiryDirectForm.DoSave();
            
            /*warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Tracked Item Update TT1");

            _msgToCheck = new string[]
            {
                @"Code :75016. The Container Id (JLG45165A02) failed the validity checks and may be incorrect."
            };
            warningErrorForm.CheckWarningsErrorsExist(_msgToCheck);
            cargoEnquiryDirectForm.CloseForm();*/
            WarningErrorForm.CheckErrorMessagesExist("Warnings for Tracked Item Update TT1",
                new[]
                {
                    "Code :75016. The Container Id (JLG45165A02) failed the validity checks and may be incorrect."
                }, false);

            // Step 14
            /*roadOperationsForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^45165A~Cargo ID^JLG45165A02~Commodity^GEN", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");

            // Step 15
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^45165A~Cargo ID^JLG45165A02~Commodity^GEN", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");*/
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new[] { "45165A" });

            // Step 17
            RoadGateSetVehicleSection();
            roadGateForm.btnReleaseFull.DoClick();

            // Step 18
            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm(formTitle: @"Release Full Container  TT1");
            roadGateDetailsReleaseForm.TxtCargoId.SetValue("JLG45165A02");
            roadGateDetailsReleaseForm.TxtDeliveryReleaseNo.SetValue(@"123456");
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            /*warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");

            _msgToCheck = new string[]
            {
                @"Code :75738. The operator COS requires a consignee for item JLG45165A02.",
                @"Code :79688. Cargo JLG45165A02's availability status of Unavailable for Release does not match the request's availability status of Available for release. Has Stops."
            };
            warningErrorForm.CheckWarningsErrorsExist(_msgToCheck);
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CheckErrorMessagesExist("Warnings for Gate In/Out TT1",
                new[]
                {
                    "Code :75738. The operator COS requires a consignee for item JLG45165A02.",
                    "Code :79688. Cargo JLG45165A02's availability status of Unavailable for Release does not match the request's availability status of Available for release. Has Stops."
                }, false);

            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Full~Detail^JLG45165A02; COS; 2210");
            roadGateForm.TblGateItems.FindClickRow(["Type^Release Full~Detail^JLG45165A02; COS; 2210"]);
            // Step 20
            roadGateForm.btnSave.DoClick();

            // Step 21 - 22
            /*roadOperationsForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^45165A~Cargo ID^JLG45165A02~Commodity^GEN", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");

            // Step 23
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^45165A~Cargo ID^JLG45165A02~Commodity^GEN", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");*/
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new[] { "45165A" });

            // Step 24 - 25
          
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetFocusToForm();
            terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Prefill Prenote Container from previous visit at Road Gate", @"1", rowDataType: EditRowDataType.CheckBox);
          
            // Step 26 - 27
            RoadGateSetVehicleSection();
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK45165", 10);

            // Step 28//
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Full Container TT1");
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG45165A02");

            // Step 29
            _roadGateDetailsReceiveForm.MtTareWeight.ValidateValueAndType("3500.000", "lbs");
            Assert.IsTrue(_roadGateDetailsReceiveForm.CmbOperator.GetValue() == "COS\tCOS HAKONE",
                @"TestCase45165 - Operator did not default to COS");
            
            // Step 30
            _roadGateDetailsReceiveForm.BtnCancel.DoClick();

            // Step 31
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"WARNING: Unsaved Changes.");
            _confirmationFormYesNo.CheckMessageMatch(@"You are currently editing an object that has not been saved. Do you want to abandon your changes ?");
            _confirmationFormYesNo.btnYes.DoClick();

            terminalConfigForm.SetFocusToForm();
            terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Prefill Prenote Container from Booking", @"1", rowDataType: EditRowDataType.CheckBox);

            // Step 32 - 35
            RoadGateSetVehicleSection();
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK45165", 10);

            // Step 36
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Full Container TT1");
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG45165A02");

            // Step 37
            Trace.TraceInformation(@"ISO Type: {0}    Operator: {1}", _roadGateDetailsReceiveForm.CmbIsoType.GetValue(),
               _roadGateDetailsReceiveForm.CmbOperator.GetValue());
            Assert.IsTrue(_roadGateDetailsReceiveForm.CmbIsoType.GetValue() == "2210\tGeneral purpose, upper vents",
               @"TestCase45165 - ISO Type did not default to 2210");
            _roadGateDetailsReceiveForm.MtTareWeight.ValidateValueAndType("1999.594", "lbs");
            Assert.IsTrue(_roadGateDetailsReceiveForm.CmbOperator.GetValue() == "MSC\tMediterranean Shipping  Company",
                @"TestCase45165 - Operator did not default to MSC");
            _roadGateDetailsReceiveForm.BtnCancel.DoClick();
            
            // Step 38 - 39
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"WARNING: Unsaved Changes.");
            _confirmationFormYesNo.CheckMessageMatch(@"You are currently editing an object that has not been saved. Do you want to abandon your changes ?");
            _confirmationFormYesNo.btnYes.DoClick();
            
            // Step 40
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking", forceReset: true);
            BookingForm bookingForm = new BookingForm(@"Booking " + terminalId);

            // Step 41
            bookingForm.GetSearcherTableDetails();
            MTNControlBase.SetValueInEditTable(bookingForm.tblSearcher, @"Booking", @"JLGBOOK45165");
            //bookingForm.btnSearch.DoClick();
            bookingForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(bookingForm.tblBookings,
                // @"Booking^JLGBOOK45165~ISO Type^2210~Status^Active~Operator^MSC~Voyage^MSCK000002");
            bookingForm.TblBookings.FindClickRow(["Booking^JLGBOOK45165~ISO Type^2210~Status^Active~Operator^MSC~Voyage^MSCK000002"]);            //bookingForm.btnEdit.DoClick();
            bookingForm.DoEdit();
            BookingItemsForm bookingItemsForm = new BookingItemsForm();
            // MTNControlBase.FindClickRowInTable(bookingItemsForm.tblItems,
                // @"Items^2~Received^0~Released^0~Cargo Type^ISO Container~ISO Type^2210~Commodity^GEN", ClickType.DoubleClick);
            bookingItemsForm.TblItems.FindClickRow(["Items^2~Received^0~Released^0~Cargo Type^ISO Container~ISO Type^2210~Commodity^GEN"], ClickType.DoubleClick);
            // Step 44
            BookingItemFormCont bookingItemFormCont = new BookingItemFormCont(@"Editing Booking Item for JLGBOOK45165 TT1");
            //MTNControlBase.SetValue(bookingItemFormCont.cmbISOGroup, @"22G0");
            bookingItemFormCont.cmbISOGroup.SetValue(@"22G0  20 Standard Dry", doDownArrow: true);
            bookingItemFormCont.btnOK.DoClick();

            bookingItemsForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(bookingItemsForm.tblItems,
                // @"Items^2~Received^0~Released^0~Cargo Type^ISO Container~ISO Type^~Commodity^GEN~ISO Group^22G0", ClickType.None);
            bookingItemsForm.TblItems.FindClickRow(["Items^2~Received^0~Released^0~Cargo Type^ISO Container~ISO Type^~Commodity^GEN~ISO Group^22G0"], ClickType.None);
            // Step 45
            bookingItemsForm.btnSave.DoClick();

            // Step 46 - 47
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1");
            roadGateForm.SetFocusToForm();
            roadGateForm.SetRegoCarrierGate("45165B");
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK45165", 10);

            // Step 48
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Full Container TT1");
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG45165A02");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            // Step 49
            Assert.IsTrue(_roadGateDetailsReceiveForm.CmbIsoType.GetValue() == "2210\tGeneral purpose, upper vents",
               @"TestCase45165 - ISO Type did not default to 2210");
            _roadGateDetailsReceiveForm.MtTareWeight.ValidateValueAndType("3500.000", "lbs");
            Assert.IsTrue(_roadGateDetailsReceiveForm.CmbOperator.GetValue() == "MSC\tMediterranean Shipping  Company",
                @"TestCase45165 - Operator did not default to MSC");
            _roadGateDetailsReceiveForm.BtnCancel.DoClick();

            // Step 50 - 51
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"WARNING: Unsaved Changes.");
            _confirmationFormYesNo.CheckMessageMatch(@"You are currently editing an object that has not been saved. Do you want to abandon your changes ?");
            _confirmationFormYesNo.btnYes.DoClick();

            // need to do this order get the message box up and cleanup crashes
            roadGateForm.btnCancel.DoClick();

            ConfirmationFormOKwithText confirmationFormOKwithText = new ConfirmationFormOKwithText(@"Vehicle Cancel Reason " + roadGateForm.GetTerminalName(),
                    ControlType.Pane, automationIdInput: @"2004", automationIdOK: @"2002", automationIdMessage:@"2003");
            confirmationFormOKwithText.txtInput.SetValue(@"Test step complete");
            confirmationFormOKwithText.btnOK.DoClick();

            roadGateForm.CloseForm();

        }

        private void RoadGateSetVehicleSection(string vehicleId = null)
        {
            if (roadGateForm == null)
            {
                FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
                roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: vehicleId);
            }

            roadGateForm.SetFocusToForm();
            roadGateForm.SetRegoCarrierGate("45165A");
        }




        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_45165_";

            // Delete Booking
            CreateDataFileToLoad(@"DeleteBooking.xml",
                "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n	<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n  <BookingHeader Terminal='TT1'>\n	<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>JLGBOOK45165</id>\n	<dischargePort>NZAKL</dischargePort>\n	<voyageCode>MSCK000002</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>D</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK45165</id>\n		<isoType>2210</isoType>\n		<number>2</number>\n		<weight>2000</weight>\n		<commodityDescription>GEN</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n		<messageMode>D</messageMode>	\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n\n");

            // Delete Cargo
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>45165</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG45165A02</id>\n      <isoType>2210</isoType>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS08</locationId>\n      <weight>8000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");


            // Create Booking
            CreateDataFileToLoad(@"CreateBooking.xml",
                "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n	<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n  <BookingHeader Terminal='TT1'>\n	<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>JLGBOOK45165</id>\n	<dischargePort>NZAKL</dischargePort>\n	<voyageCode>MSCK000002</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>A</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK45165</id>\n		<isoType>2210</isoType>\n		<number>2</number>\n		<weight>2000</weight>\n		<commodityDescription>GEN</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
