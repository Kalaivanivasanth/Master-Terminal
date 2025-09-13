using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Vehicle_Booking;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]

    public class TestCase49902 : MTNBase
    {
        
        [ClassInitialize] 
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}
      
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();
            CallJadeScriptToRun(TestContext, "resetData_49902");
        }

        [TestMethod]
        public void VehicleBookingAddEditDelete()
        {
            MTNInitialize();
            
            // Open Yard Functions | Vehicle Bookings
            FormObjectBase.NavigationMenuSelection(@"Gate Functions | Vehicle Bookings");

            // Click Search button 
            VehicleBookingsForm vehicleBookingsForm = new VehicleBookingsForm();

            // Enter Include - UnBooked, Booked - Unused, Used, Carrier - **Any**,  Driver - **Any**, Vehicle - **Any** and click the Close button
            vehicleBookingsForm.ShowSearcher();


            // In the drop down, select by Day
            var defaultDate = DateTime.Now.AddDays(1).ToString(@"ddMMyyyy");
            vehicleBookingsForm.cmbDayMonth.SetValue(@"by Day");
            vehicleBookingsForm.GetDateTextbox();
            vehicleBookingsForm.txtDate.SetValue(defaultDate);
            // Friday, 14 March 2025 navmh5 Can be removed 6 months after specified date vehicleBookingsForm.txtDate.SetValue(DateTime.Now.AddDays(1).ToString(@"ddMMyyyy"));
            // Right click anywhere in the results table and from the context menu, select Add Booking Slot... 
            Mouse.MoveTo(vehicleBookingsForm.tblShowSlots.BoundingRectangle.Center());
            Mouse.RightClick();
            vehicleBookingsForm.ContextMenuSelect(@"Add Booking Slot...");

            // Enter Start Date -Today's date, Start Time -15:00, End Date -Today's date, End Time -14:00, Carrier - Auto - AAUTO  American Auto Tpt, Delivering - Checked
            VehicleBookingsMaintForm vehicleBookingMaintForm = new VehicleBookingsMaintForm(@"Vehicle Booking Time Slot Maintenance TT1");
            // Friday, 14 March 2025 navmh5 Can be removed 6 months after specified date vehicleBookingMaintForm.txtStartDate.SetValue(DateTime.Now.AddDays(1).ToString(@"ddMMyyyy"));
            vehicleBookingMaintForm.txtStartDate.SetValue(defaultDate);
            vehicleBookingMaintForm.txtStartTime.Focus();
            Keyboard.Type("15");
            vehicleBookingMaintForm.txtEndDate.SetValue(defaultDate);
            // Friday, 14 March 2025 navmh5 Can be removed 6 months after specified date vehicleBookingMaintForm.txtEndDate.SetValue(DateTime.Now.AddDays(1).ToString(@"ddMMyyyy"));
            vehicleBookingMaintForm.txtEndTime.Focus();
            Keyboard.Type(@"16");
            vehicleBookingMaintForm.cmbCarrier.SetValue(Carrier.CARRIER1, doDownArrow: true, downArrowSearchType: SearchType.StartsWith, searchSubStringTo: Carrier.CARRIER1.Length - 4);
            vehicleBookingMaintForm.chkDelivery.DoClick();
            vehicleBookingMaintForm.btnSave.DoClick();

            // Select the entry, right click and select Maintain Booking Slot...
            MTNControlBase.FindClickRowInTableVHeader(vehicleBookingsForm.tblShowSlots, @"15:00", ClickType.ContextClick);
            vehicleBookingsForm.ContextMenuSelect(@"Maintain Booking Slot...");

            // Enter the following data and click the Save button
            VehicleBookingsMaintForm vehicleBookingMaintForm1 = new VehicleBookingsMaintForm(@"Vehicle Booking Time Slot Maintenance TT1");
            vehicleBookingMaintForm1.cmbCarrier.SetValue("AAUTO", doDownArrow: true, downArrowSearchType: SearchType.StartsWith, searchSubStringTo: 4);
            vehicleBookingMaintForm1.btnSave.DoClick();

            // Select the entry, right click and select Delete Booking Slot(s)
            MTNControlBase.FindClickRowInTableVHeader(vehicleBookingsForm.tblShowSlots, @"15:00", ClickType.ContextClick);
            vehicleBookingsForm.ContextMenuSelect(@"Delete Booking Slot(s)");
            ConfirmationFormYesNo confirmYesNo = new ConfirmationFormYesNo(@"Continue ?", ControlType.Window, @"2", @"3", @"4");

            // Click the Yes button 
            confirmYesNo.btnYes.DoClick();
        }
    }
}
