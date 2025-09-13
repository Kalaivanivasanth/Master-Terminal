using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects.Gate_Functions.Vehicle_Bookings;
using System;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Vehicle_Bookings
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase64118 : MTNBase
    {
        private const string TestCaseNumber = @"64118";
        private const string TerminalId = @"EUP";

        VehicleBookingEnquiryForm _vehicleBookingsForm;
        AddingVehicleBookingSlotItemForm _addingVehicleBookingSlotItemForm;
        VehicleBookingSlotsMaintenanceForm _vehicleBookingSlotsMaintenanceForm;

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
        /// To test that when there are multiple reserve details a new check box 
        /// "Add All Items" is added to the VBS slot item form to select all the reserve details
        /// </summary>
        [TestMethod]
        public void VBSAddingEntireReleaseWithLineItems()
        {
            var startDate = DateTime.Now;
            var endDate = startDate.AddDays(1);

            MTNInitialize();

            // Open Vehicle Bookings Form and click on the New button from the toolbar
            _vehicleBookingsForm = MainFormToolbarVehicleBookings(TerminalId);
            _vehicleBookingsForm.DoNew();

            // Enter the required details on the VBS maintenance form and click on the Add button
            VehicleBookingSlotsMaintenanceForm.DoAddHeaderDetails("TS1", startDate, endDate, "AAUTO - American Auto Tpt");

            // On the Adding Vehicle Booking Slot Item form, select the Release Request and click OK and check that there is only one VBS slot item is added
            _addingVehicleBookingSlotItemForm = new AddingVehicleBookingSlotItemForm($"Adding Vehicle Booking Slot Item {TerminalId}");
            _addingVehicleBookingSlotItemForm.EnterReleaseRequestQuantityOrWeight($"{TestCaseNumber}RLSA", null, null);

            // Delete the VBS slot item 
            _vehicleBookingSlotsMaintenanceForm = new VehicleBookingSlotsMaintenanceForm($"Vehicle Booking Slots Maintenance - Advanced Process {TerminalId}");
            _vehicleBookingSlotsMaintenanceForm.DeleteVBSSlotItems(new[] { $"Bulk Release^{TestCaseNumber}RLSA" });
            // Monday, 17 February 2025 navmh5 _vehicleBookingSlotsMaintenanceForm.DeleteVBSSlotItems(new[] { $"Quantity^~Bulk Release^{TestCaseNumber}RLSA" });
            _vehicleBookingSlotsMaintenanceForm.DoAdd();

            // Add all the items from the release request by clicking on Add All Items checkbox and check that all the items are added
            _addingVehicleBookingSlotItemForm = new AddingVehicleBookingSlotItemForm($"Adding Vehicle Booking Slot Item {TerminalId}");
            _addingVehicleBookingSlotItemForm.AddAllItemsFromReleaseRequest($"{TestCaseNumber}RLSA");

            _vehicleBookingSlotsMaintenanceForm.ValidateVBSSlotItemsAndSave(new[] {
             $"Quantity^5~Bulk Release^{TestCaseNumber}RLSA~Reserve Details^5 x Rolls" ,
             $"Quantity^5~Bulk Release^{TestCaseNumber}RLSA~Reserve Details^5 x Bottles of Beer" ,
             $"Quantity^6~Bulk Release^{TestCaseNumber}RLSA~Reserve Details^6 x Big bag of Sand" });

            // Delete the Vehicle Booking
            _vehicleBookingsForm.SetFocusToForm();
            _vehicleBookingsForm.DeleteVehicleBooking(new[] { $"Cargo Release(s)^{TestCaseNumber}RLSA" });


        }

    }
}


