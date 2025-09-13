using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.Gate_Functions.Bookings.BookingItemForm;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Gate_Functions.Vehicle;
using MTNForms.FormObjects.Message_Dialogs;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase37631 : MTNBase
    {

        private ConfirmationFormYesNo _confirmationFormYesNo;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void CargoTypeDefaultCommmodities()
        {
            MTNInitialize();

            // Step 1 - 2
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking");
            FormObjectBase.MainForm.OpenBookingFromToolbar();
            var bookingForm = new BookingForm(@"Booking TT1");
            //bookingForm.btnAdd.DoClick();
            bookingForm.DoNew();

            var bookingFormItemsForm = new BookingItemsForm();

            bookingFormItemsForm.txtReference.SetValue(@"JLGBOOK37631A001");
            bookingFormItemsForm.cmbOperator.SetValue(Operator.MSL,  doDownArrow: true);
            bookingFormItemsForm.cmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            bookingFormItemsForm.cmbDischargePort.SetValue(Port.LYTNZ);

            bookingFormItemsForm.btnAdd.DoClick();

            var bookingItemForm = new BookingItemForm(@"Adding Booking Item for  TT1");
            
            // Step 3 - 17
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, CargoType.TimberFor37631A, @"TIMBTimber");
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, CargoType.TimberFor37631B, @"GENLGeneral",
                @"TIMB37631BPROD1  TIMB37631B Product 1", true);
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, CargoType.TimberFor37631C, @"TIMBTimber",
                @"TIMB37631CPROD1  TIMB37631C Product 1", true);
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, CargoType.TimberFor37631D, @"TIMBTimber",
                @"TIMB37631DPROD1  TIMB37631D Product 1", true);
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, string.Empty, @"GENLGeneral",
                @"TIMB37631DPROD2  TIMB37631D Product 2");
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, string.Empty, @"TIMBTimber",
                @"TIMB37631DPROD1  TIMB37631D Product 1");
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, CargoType.TimberFor37631E, String.Empty);
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, CargoType.TimberFor37631FNCTDC1, @"GENLGeneral",
                @"TIMB37631FPROD1  TIMB37631F Product 1", true);
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, CargoType.TimberFor37631GNCTDC, String.Empty,
                @"TIMB37631GPROD1  TIMB37631G Product 1", true);
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, CargoType.TimberFor37631HNCTDC, String.Empty,
                @"TIMB37631HPROD1  TIMB37631H Product 1", true);
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, string.Empty, @"CUSTCustom Wood",
                @"TIMB37631HPROD2  TIMB37631H Product 2");
            SetValidateCargoTypeAndCommodityBooking(bookingItemForm, string.Empty, String.Empty,
                @"TIMB37631HPROD1  TIMB37631H Product 1");
            bookingItemForm.btnCancel.DoClick();

            bookingFormItemsForm.SetFocusToForm();
            bookingFormItemsForm.btnCancel.DoClick();

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"WARNING: Unsaved Changes.");
            _confirmationFormYesNo.CheckMessageMatch(@"You are currently editing an object that has not been saved. Do you want to abandon your changes ?");
            _confirmationFormYesNo.btnYes.DoClick();

            bookingForm.SetFocusToForm();
            bookingForm.CloseForm();

            // Step 18 - 51
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            //FormObjectBase.NavigationMenuSelection(@"Road Gate");
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"37631");
            roadGateForm.SetRegoCarrierGate("37631");

            SetValueCargoTypeAndCommodityRoadGate(CargoType.TimberFor37631A , @"TIMB Timber");
            SetValueCargoTypeAndCommodityRoadGate(CargoType.TimberFor37631B , @"TIMB Timber");
            SetValueCargoTypeAndCommodityRoadGate(CargoType.TimberFor37631B , @"GENL General",
                @"TIMB37631BPROD1 TIMB37631B Product 1");
            SetValueCargoTypeAndCommodityRoadGate(CargoType.TimberFor37631C , string.Empty,
                @"TIMB37631CPROD1 TIMB37631C Product 1");
            SetValueCargoTypeAndCommodityRoadGate(CargoType.TimberFor37631D , @"GENL General",
                @"TIMB37631DPROD2 TIMB37631D Product 2", string.Empty, @"TIMB37631DPROD1 TIMB37631D Product 1");
             SetValueCargoTypeAndCommodityRoadGate(CargoType.TimberFor37631E , @"TIMB Timber");
            SetValueCargoTypeAndCommodityRoadGate(CargoType.TimberFor37631FNCTDC1, @"GENL General",
                @"TIMB37631FPROD1 TIMB37631F Product 1");
            SetValueCargoTypeAndCommodityRoadGate(CargoType.TimberFor37631GNCTDC, string.Empty,
                @"TIMB37631GPROD1 TIMB37631G Product 1");
            SetValueCargoTypeAndCommodityRoadGate(CargoType.TimberFor37631HNCTDC, @"CUST Custom Wood",
                @"TIMB37631HPROD2 TIMB37631H Product 2", string.Empty, @"TIMB37631HPROD1 TIMB37631H Product 1");

            // Step 52 - 53
            roadGateForm.SetFocusToForm();
            roadGateForm.btnCancel.DoClick();

            VehicleCancelReasonForm vehicleCancelReasonForm = new VehicleCancelReasonForm(@"Vehicle Cancel Reason TT1");
            vehicleCancelReasonForm.txtReason.SetValue(@"Finished with vehicle 37631");
            vehicleCancelReasonForm.btnOK.DoClick();
        }

        private void SetValueCargoTypeAndCommodityRoadGate(string cargoType, string commodity, 
            string subType = null, string commodity2 = null, string subType2 = null)
        {
            roadGateForm.btnReceiveCargo.DoClick();

            RoadGateDetailsReceiveForm roadGateDetailsReceiveForm =
                new RoadGateDetailsReceiveForm(@"Receive General Cargo TT1");

            if (!string.IsNullOrEmpty(cargoType))
            {
                roadGateDetailsReceiveForm.CmbCargoType.SetValue(cargoType, doDownArrow: true, searchSubStringTo: 10/*cargoType.Length - 1*/);
                Keyboard.Press(VirtualKeyShort.TAB);
            }

            if (!string.IsNullOrEmpty(subType))
            {
                roadGateDetailsReceiveForm.CmbCargoSubtype.SetValue(subType, additionalWaitTimeout: 500, doDownArrow: true);
                Miscellaneous.WaitForSeconds(2);
            }
            
            if (!string.IsNullOrEmpty(subType2))
            {
                roadGateDetailsReceiveForm.CmbCargoSubtype.SetValue(subType2, additionalWaitTimeout: 500, doDownArrow: true);
                Miscellaneous.WaitForSeconds(2);
                roadGateDetailsReceiveForm.CmbCommodity.ValidateText(commodity2);
                

                if (subType2.Equals(@"TIMB37631HPROD1 TIMB37631H Product 1"))
                {
                    roadGateDetailsReceiveForm.CmbCargoSubtype.SetValue(subType, additionalWaitTimeout: 500, doDownArrow: true);
                    Miscellaneous.WaitForSeconds(4);
                    roadGateDetailsReceiveForm.CmbCommodity.ValidateText(commodity);
                }
            }
            roadGateDetailsReceiveForm.BtnCancel.DoClick();

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"WARNING: Unsaved Changes.");
            _confirmationFormYesNo.CheckMessageMatch(
                @"You are currently editing an object that has not been saved. Do you want to abandon your changes ?");
            _confirmationFormYesNo.btnYes.DoClick();
        }

        private void SetValidateCargoTypeAndCommodityBooking(BookingItemForm bookingItemForm, string cargoType, string commodity,
            string subType = null, bool checkSubType = false)
        {
            if (!string.IsNullOrEmpty(cargoType))
            {
                bookingItemForm.cmbCargoType.SetValue(cargoType, doDownArrow: true, searchSubStringTo:6);
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
                bookingItemForm.GetCommodityDetails();
            }

            if (string.IsNullOrEmpty(subType)) return;
            
            bookingItemForm.GetSubTypeDetails();
            if (!checkSubType)
                bookingItemForm.cmbSubType.SetValue(subType);
            else
                bookingItemForm.cmbSubType.ValidateText(subType);
        }
    }

}
