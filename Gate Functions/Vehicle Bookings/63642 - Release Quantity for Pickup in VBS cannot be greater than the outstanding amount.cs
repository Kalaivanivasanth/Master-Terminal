using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects.Gate_Functions.Vehicle_Bookings;
using System;
using MTNForms.FormObjects.Message_Dialogs;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Vehicle_Bookings
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase63642 : MTNBase
    {
        private const string TestCaseNumber = @"63642";
        private const string TerminalId = @"EUP";

        VehicleBookingEnquiryForm _vehicleBookingsForm;
        AddingVehicleBookingSlotItemForm _addingVehicleBookingSlotItemForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            userName = "EUPUSER";
            LogInto<MTNLogInOutBO>(userName);
        }

        /// <summary>
        /// Validations should occur on the VBS Slot for a Pickup, 
        /// if the VBS Quantity/ Weight greater than the Outstanding Release line quantity/weight.
        /// </summary>
        [TestMethod]
        public void ReleaseQuantityForAPickupVBSQuantityWeightCannotBeGreaterThanTheOutstandingReleaseLineQuantityWeight()
        {
            var startDate = DateTime.Now;
            var endDate = startDate.AddDays(1);

            MTNInitialize();

            // Open Vehicle Bookings Form and click on the New button from the toolbar
            _vehicleBookingsForm = MainFormToolbarVehicleBookings(TerminalId);
            _vehicleBookingsForm.DoNew();

            // Enter the required details on the VBS maintenance form and click on the Add button
            VehicleBookingSlotsMaintenanceForm.DoAddHeaderDetails("TS1", startDate, endDate, "AAUTO - American Auto Tpt");   

            // On the Adding Vehicle Booking Slot Item form, select the Release Request and enter the Quantity greater than the ReserveDetail Quantity
            _addingVehicleBookingSlotItemForm = new AddingVehicleBookingSlotItemForm($"Adding Vehicle Booking Slot Item {TerminalId}");           
            _addingVehicleBookingSlotItemForm.EnterReleaseRequestQuantityOrWeight($"{TestCaseNumber}RLSA", "150", null);

            // Click on the OK button and verify the error message
            ConfirmationFormOK.ValidateMessageClickOK(new ConfirmationFormOKArguments
            { FormTitle = "Error", Message = "Error Code :96060. Quantity should be less than ReserveDetail Quantity(100). Already Items added for Quantity(0).", MessageAutomationId = "3", OKAutomationId = "4" });

            // On the Adding Vehicle Booking Slot Item form, select the Release Request and enter the Weight greater than the ReserveDetail Weight
            _addingVehicleBookingSlotItemForm.SetFocusToForm();
            _addingVehicleBookingSlotItemForm.EnterReleaseRequestQuantityOrWeight($"{TestCaseNumber}RLSB", null, "6000");

            // Verify the error message
            // Click on the OK button and verify the error message
            ConfirmationFormOK.ValidateMessageClickOK(new ConfirmationFormOKArguments
                {
                    FormTitle = "Error",
                    Message = "Error Code :96791. The VBS Max Weight of '5999.996 lbs' for Release Request: '63642RLSB' for releasing '5000.000 lbs' of 'Paper' cargo cannot exceed the release line weight of '5000.000 lbs'",
                    MessageAutomationId = "3", OKAutomationId = "4"
                });

            _addingVehicleBookingSlotItemForm.SetFocusToForm();
            _addingVehicleBookingSlotItemForm.DoClose();
        }

    }
}


