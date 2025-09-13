using System;
using System.Linq;
using DataObjects;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.Controls.Lists;
using MTNForms.Controls.Table;
using MTNForms.FormObjects;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Misc
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase65525 : MTNBase
    {
        
        string _notFoundDetailsToValidate;
        
        EDIOperationsForm _ediOperationsForm;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            TestRunDO.GetInstance().SetDoResetConfigsToFalse();
            LogInto<MTNLogInOutBO>("MTNCOLORG");
        }


        [TestMethod]
        public void ColumnOrganiserTest()
        {
            MTNInitialize();

            var expectedFailedDetails = new[]
            {
                "    Cargo Enquiry - Available - Validate - ft3 - Total Volume ft3", "    Cargo Enquiry - Available - Validate - ft3 - Cargo Volume ft3",
                "    Cargo Enquiry - Available - Validate - ft3 - Cargo VolumeA ft3", "    Cargo Enquiry - Select And Order - Validate - Planned - Planned Location",
                "    Cargo Enquiry - Select And Order - Validate - Planned - Location ID", "    Cargo Enquiry - Select And Order - Validate - Planned - Location IDA",
                "Cargo Enquiry - Select And Order - Clear & Validate - CargoA Type", "Cargo Enquiry - Select And Order - Clear & Validate - CargoA Type",
                "    EDI Operations - EDI Details - Available - Validate - b - Total Weight lbs", "    EDI Operations - EDI Details - Available - Validate - b - Over Length Back cm",
                "    EDI Operations - EDI Details - Available - Validate - b - Over LengthA Back cm", "    EDI Operations - EDI Details - Available - Validate - Port - Opt Discharge Ports",
                "    EDI Operations - EDI Details - Available - Validate - Port - Opt Discharge Port", "    EDI Operations - EDI Details - Select And Order - Validate - Port - Commodity Descr",
                "    EDI Operations - EDI Details - Select And Order - Validate - Port - Commodity", "    EDI Operations - EDI Details - Select And Order - Validate - Port - CommodityA",
                "    Terminal Administration - Machine - Available - Validate - lb - Tare Weight lbs", "    Terminal Administration - Machine - Available - Validate - lb - Maximum Lift Weight Overall lbs",
                "    Terminal Administration - Machine - Available - Validate - lb - Maximum Lift Weight OverallA lbs", "    Terminal Administration - Machine - Select And Order - Validate - Can - Machine Type",
                "    Terminal Administration - Machine - Select And Order - Validate - Can - Description", "    Terminal Administration - Machine - Select And Order - Validate - Can - DescriptionA",
                "Terminal Administration - Machine - Select And Order - Clear & Validate - Machine Typea", "Terminal Administration - Machine - Select And Order - Clear & Validate - Machine Typea",
                "    Release Requests - Available - Validate - inst - Instruction Description", "    Release Requests - Available - Validate - inst - Instruction Code",
                "    Release Requests - Available - Validate - inst - Instruction CodeA",
                "    Release Requests - Select And Order - Validate - rem - Customs Assistance",
                "    Release Requests - Select And Order - Validate - rem - Customs Agent", "    Release Requests - Select And Order - Validate - rem - Customs AgentA",
                "Release Requests - Select And Order - Clear & Validate - Heavy DutyA", "Release Requests - Select And Order - Clear & Validate - Heavy DutyA",
                "    Booking - Available - Validate - code - Instruction Code", "    Booking - Available - Validate - code - HS Code",
                "    Booking - Available - Validate - code - HS CodeA", "    Booking - Available - Validate - multiple - Cargo Subtype Description",
                "    Booking - Available - Validate - multiple - Cargo Type", "    Booking - Select And Order - Validate - m3 - Tranship Voyage",
                "    Booking - Select And Order - Validate - m3 - Tranship", "    Booking - Select And Order - Validate - m3 - TranshipA",
                "Booking - Select And Order - Clear & Validate - StockA", "Booking - Select And Order - Clear & Validate - StockA"

            };

             _notFoundDetailsToValidate += CargoEnquiryColumnOrganiserTest();
             _notFoundDetailsToValidate += EDIOperationsColumnOrganiserTest();
             _notFoundDetailsToValidate += TerminalAdminColumnOrganiserTest();
             _notFoundDetailsToValidate += ReleaseRequestColumnOrganiserTest();
             _notFoundDetailsToValidate += BookingColumnOrganiserTest();
             
            var notFoundArray = _notFoundDetailsToValidate.Split(new[] { "\r\n" }, StringSplitOptions.None);
            var notFound = expectedFailedDetails.Except(notFoundArray).Aggregate("", (current, item) => current + item + "\r\n");
            
             Assert.IsTrue(string.IsNullOrEmpty(notFound), $"TestCase65525 - The following were not found in the list or where extra in the list or Move Up / Down were enabled / disabled incorrectly:\r\n{notFound}");
            
        }

        string CargoEnquiryColumnOrganiserTest()
        {
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria,
                CargoEnquiryForm.SearchFields.CargoType, CargoEnquiryForm.CargoTypes.ISOContainer,
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);

            ClickContextMenuColumnOrganizerForRequestedTable(cargoEnquiryForm.tblData2);
            var notFoundDetailsToValidate = DoColumnOrganiser("Cargo Enquiry", string.Empty, new [] 
                { 
                    new FilterArguments { FilterOn = "ft3", ExpectedFilteredDetails = new[] { "Cargo Volume ft3", "Container Volume ft3", "Total Volume ft3" } },    // Pass
                    new FilterArguments { FilterOn = "ft3", ExpectedFilteredDetails = new[] { "Cargo Volume ft3", "Container Volume ft3" } },    // Failed - Extra in the list - Total Volume ft3
                    new FilterArguments { FilterOn = "ft3", ExpectedFilteredDetails = new[] { "Cargo VolumeA ft3", "Container Volume ft3", "Total Volume ft3" } },    // Failed - Cargo VolumeA ft3 not found in the list and Cargo Volume ft3 is extra in the list
                }, new []
                { new FilterArguments { FilterOn = "Planned", ExpectedFilteredDetails = new[] { "Cargo Planned Onboard", "Planned State", "Site (Planned)" } },    // Pass
                }, new []
                {
                    "Cargo Onboard", "Colour", "Customs References", "Has Valid ID", "Onboard Reefer", "Port Of Origin", "Stevedore", "Trailer ID"  // Pass
                }, new[]
                { 
                    new FilterArguments { FilterOn = "Planned", ExpectedFilteredDetails = new[] { "Cargo Planned Onboard", "Planned State", "Site (Planned)" } },    // Pass
                    new FilterArguments { FilterOn = "Bill", ExpectedFilteredDetails = new[] { "All Possible Waybill Releases", "Bill Of Lading", "House Bill of Lading"} },    // Pass
                    new FilterArguments { FilterOn = "Train", ExpectedFilteredDetails = new[] { "Assigned Train", "Receive Train", "Release Train"} },    // Pass
                },
                new [] 
                { 
                    new FilterArguments { FilterOn = "Location", ExpectedFilteredDetails = new[] { "Location ID", "Queued Location", "Planned Location" } },    // Pass
                    new FilterArguments { FilterOn = "Location", ExpectedFilteredDetails = new[] { "Location ID", "Queued Location" } },    // Failed - Extra in the list - Planned Location
                    new FilterArguments { FilterOn = "Location", ExpectedFilteredDetails = new[] { "Location IDA", "Queued Location", "Planned Location" } },    // Failed - Location IDA not found in the list and Location ID is extra in the list
                }, new []
                { new FilterArguments { FilterOn = "Carr", ExpectedFilteredDetails = new[] { "Receive Sub Carrier", "Release Sub Carrier" } },    // Pass
                }, new []
                {
                    "ID", "Children", "Parent Id", "Location ID", "Queued Location", "CargoA Type", "Voyage", "To Do"  // Fail - CargoA Type
                }, new[]
                { 
                    new FilterArguments { FilterOn = "Grade", ExpectedFilteredDetails = new[] { "Availability Grade" } },    // Pass
                    new FilterArguments { FilterOn = "ID", ExpectedFilteredDetails = new[] { "ID", "Parent Id", "Location ID"} },    // Pass
                    new FilterArguments { FilterOn = "DO", ExpectedFilteredDetails = new[] { "To Do Tasks (dbl click)", "To Do"} },    // Pass
                });
            return notFoundDetailsToValidate;
        }
        
        string EDIOperationsColumnOrganiserTest()
        {
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            _ediOperationsForm = new EDIOperationsForm("EDI Operations TT1");

            MTNControlBase.SetValueInEditTable(_ediOperationsForm.tblEDISearch, "Data Type", "Bay Plan", EditRowDataType.ComboBoxEdit);
            _ediOperationsForm.DoSearch();

            _ediOperationsForm.GetTabTableGeneric("Bay Plan");
            _ediOperationsForm.TblMessages.FindClickRow(new[] { "Status^Verify errors~Voyage^HHRV10001" });

            ClickContextMenuColumnOrganizerForRequestedTable(_ediOperationsForm.TabGeneric.TableWithHeader);
            var notFoundDetailsToValidate = DoColumnOrganiser("EDI Operations", " - EDI Details", new [] 
                { 
                    new FilterArguments { FilterOn = "b", ExpectedFilteredDetails = new[] { "Over Length Back cm", "Over Length Back ft", "Total Weight lbs" } },    // Pass
                    new FilterArguments { FilterOn = "b", ExpectedFilteredDetails = new[] { "Over Length Back cm", "Over Length Back ft",  } },    // Failed - Extra in the list - Total Weight lbs
                    new FilterArguments { FilterOn = "b", ExpectedFilteredDetails = new[]  {"Over LengthA Back cm", "Over Length Back ft", "Total Weight lbs" } },    // Failed - Over LengthA Back cm not found in the list and Over Length Back cm is extra in the list
                }, new []
                { new FilterArguments { FilterOn = "Port", ExpectedFilteredDetails = new[] { "Discharge Port", "Load Port", "Opt Discharge Port", "Tranship To Port" } },    // Pass
                }, new [] { "Company", "Destination", "Humidity", "Location", "MT", "Operator", "Remarks", "Voyage" // Pass
                }, new[]
                { 
                    new FilterArguments { FilterOn = "Vo", ExpectedFilteredDetails = new[] { "Volume ft3", "Volume m3", "Voyage" } },    // Pass
                    new FilterArguments { FilterOn = "des", ExpectedFilteredDetails = new[] { "Description", "Destination"} },    // Pass
                    new FilterArguments { FilterOn = "Height", ExpectedFilteredDetails = new[] { "Height cm", "Height ft", "Over Height cm", "Over Height ft"} },    // Pass
                },
                new [] 
                { 
                    new FilterArguments { FilterOn = "m", ExpectedFilteredDetails = new[] { "Temperature", "Commodity", "Commodity Descr" } },    // Pass
                    new FilterArguments { FilterOn = "m", ExpectedFilteredDetails = new[] { "Temperature", "Commodity" } },    // Failed - Extra in the list - Commodity Descr
                    new FilterArguments { FilterOn = "m", ExpectedFilteredDetails = new[] { "Temperature", "CommodityA", "Commodity Descr" } },    // Failed -CommodityA not found in the list and Commodity is extra in the list
                }, new []
                { new FilterArguments { FilterOn = "desc", ExpectedFilteredDetails = new[] { "Commodity Descr", "Cargo Type Descr" } },    // Pass
                }, new [] { "ID", "CO2", "BOL", "HasChanged", "ISO Type"  // Pass
                }, new[]
                { 
                    new FilterArguments { FilterOn = "IS", ExpectedFilteredDetails = new[] { "ISO Type" } },    // Pass
                    new FilterArguments { FilterOn = "g", ExpectedFilteredDetails = new[] { "Cargo Type Descr", "HasChanged"} },    // Pass
                    new FilterArguments { FilterOn = "BOL", ExpectedFilteredDetails = new[] { "BOL"} },    // Pass
                });
            return notFoundDetailsToValidate;
        }
        
        string TerminalAdminColumnOrganiserTest()
        {
            FormObjectBase.MainForm.OpenTerminalAdminFromToolbar();
            var terminalAdministrationForm = new TerminalAdministrationForm();

            Miscellaneous.WaitForSeconds(1);
            terminalAdministrationForm.cmbTable.SetValue("Machine");
            Miscellaneous.WaitForSeconds(1);

            ClickContextMenuColumnOrganizerForRequestedTable(terminalAdministrationForm.TblItems);
            var notFoundDetailsToValidate = DoColumnOrganiser("Terminal Administration", " - Machine", new [] 
                { 
                    new FilterArguments { FilterOn = "lb", ExpectedFilteredDetails = new[] { "Maximum Lift Weight Overall lbs", "Tare Weight lbs" } },    // Pass
                    new FilterArguments { FilterOn = "lb", ExpectedFilteredDetails = new[] { "Maximum Lift Weight Overall lbs" } },    // Failed - Extra in the list - Tare Weight lbs
                    new FilterArguments { FilterOn = "lb", ExpectedFilteredDetails = new[]  {"Maximum Lift Weight OverallA lbs", "Tare Weight lbs"} },    // Failed - Maximum Lift Weight OverallA lbs not found in the list and Maximum Lift Weight OverallA lbs is extra in the list
                }, new []
                { new FilterArguments { FilterOn = "Can", ExpectedFilteredDetails = new[] { "Can Lift 30-foot Containers", "Can Lift 40-foot Containers", "Can Lift 45-foot Containers", "Can Lift 53-foot Containers" } },    // Pass
                }, new [] { "Base Job Number", "External Reference", "Machine Lift Type", "Owner" // Pass
                }, new[]
                { 
                    new FilterArguments { FilterOn = "Last", ExpectedFilteredDetails = new[] { "Last Known Location", "Last Known Location Timestamp" } },    // Pass
                    new FilterArguments { FilterOn = "ow", ExpectedFilteredDetails = new[] { "Last Known Location", "Last Known Location Timestamp", "Owner"} },    // Pass
                    new FilterArguments { FilterOn = "Reference", ExpectedFilteredDetails = new[] { "External Reference"} },    // Pass
                },
                new [] 
                { 
                    new FilterArguments { FilterOn = "n", ExpectedFilteredDetails = new[] { "Not Available For Entry", "Description", "Machine Type" } },    // Pass
                    new FilterArguments { FilterOn = "n", ExpectedFilteredDetails = new[] { "Not Available For Entry", "Description", } },    // Failed - Extra in the list - Machine Type
                    new FilterArguments { FilterOn = "n", ExpectedFilteredDetails = new[] { "Not Available For Entry", "DescriptionA", "Machine Type" } },    // Failed -DescriptionA not found in the list and Description is extra in the list
                }, new []
                { new FilterArguments { FilterOn = "id", ExpectedFilteredDetails = new[] { "Id", "Radio Telemetry Device Id" } },    // Pass
                }, new [] { "Id", "Description", "Machine Typea", "Colour"  // Fail - Machine Typea
                }, new[]
                { 
                    new FilterArguments { FilterOn = "Not", ExpectedFilteredDetails = new[] { "Not Available For Entry" } },    // Pass
                    new FilterArguments { FilterOn = "ou", ExpectedFilteredDetails = new[] { "Colour"} },    // Pass
                    new FilterArguments { FilterOn = "available", ExpectedFilteredDetails = new[] { "Not Available For Entry", "Is Available"} },    // Pass
                });
            return notFoundDetailsToValidate;
        }
         
        string ReleaseRequestColumnOrganiserTest()
        {
            FormObjectBase.MainForm.OpenReleaseRequestFromToolbar();
            var releaseRequestForm = new ReleaseRequestForm("Release Requests TT1");

            ClickContextMenuColumnOrganizerForRequestedTable(releaseRequestForm.TblReleaseRequests);
            var notFoundDetailsToValidate = DoColumnOrganiser("Release Requests", string.Empty, new [] 
                { 
                    new FilterArguments { FilterOn = "inst", ExpectedFilteredDetails = new[] { "Instruction Code", "Instruction Description", "Instruction External", "Instruction Internal" } },    // Pass
                    new FilterArguments { FilterOn = "inst", ExpectedFilteredDetails = new[] { "Instruction Code", "Instruction External", "Instruction Internal" } },    // Failed - Extra in the list - Instruction Description
                    new FilterArguments { FilterOn = "inst", ExpectedFilteredDetails = new[]  { "Instruction CodeA", "Instruction Description", "Instruction External", "Instruction Internal" } },    // Failed - Instruction CodeA not found in the list and Instruction Code is extra in the list
                }, new []
                { new FilterArguments { FilterOn = "rem", ExpectedFilteredDetails = new[] { "Remarks Additional 1", "Remarks Additional 2" } },    // Pass
                }, new [] { "Cargo Group", "Customer Reference" // Pass
                }, new[]
                { 
                    new FilterArguments { FilterOn = "Group", ExpectedFilteredDetails = new[] { "Cargo Group" } },    // Pass
                    new FilterArguments { FilterOn = "ernal", ExpectedFilteredDetails = new[] { "Instruction External", "Instruction Internal" } },    // Pass
                    new FilterArguments { FilterOn = "ter", ExpectedFilteredDetails = new[] { "Instruction External", "Instruction Internal" } },    // Pass
                },
                new [] 
                { 
                    new FilterArguments { FilterOn = "cu", ExpectedFilteredDetails = new[] { "Customs Agent", "Customs Assistance", "Can be Hi or Low Cube" } },    // Pass
                    new FilterArguments { FilterOn = "cu", ExpectedFilteredDetails = new[] { "Customs Agent",  "Can be Hi or Low Cube", } },    // Failed - Extra in the list - Customs Assistance
                    new FilterArguments { FilterOn = "cu", ExpectedFilteredDetails = new[] { "Customs AgentA", "Customs Assistance", "Can be Hi or Low Cube" } },    // Failed - Customs AgentA not found in the list and Customs Agent is extra in the list
                }, new []
                { new FilterArguments { FilterOn = "rea", ExpectedFilteredDetails = new[] { "Creation Date/Time", "Delivery Reason" } },    // Pass
                }, new [] { "Line Number", "Status", "Item", "Load Port", "Train Id", "Heavy DutyA"  // Fail - Heavy DutyA
                }, new[]
                { 
                    new FilterArguments { FilterOn = "voy", ExpectedFilteredDetails = new[] { "Voyage", "Voyage Inward", "Voyage Arrival", "Consignor Voyage" } },    // Pass
                    new FilterArguments { FilterOn = "Port", ExpectedFilteredDetails = new[] { "Discharge Port", "Load Port" } },    // Pass
                    new FilterArguments { FilterOn = "Book", ExpectedFilteredDetails = new[] { "Booking", "Booking Confirmation Number", "Vehicle Booking Slots" } },    // Pass
                });
            return notFoundDetailsToValidate;
        }
        
        string BookingColumnOrganiserTest()
        {
            FormObjectBase.MainForm.OpenBookingFromToolbar();
            var bookingForm = new BookingForm("Booking TT1");

            ClickContextMenuColumnOrganizerForRequestedTable(bookingForm.TblBookings, true);
            var notFoundDetailsToValidate = DoColumnOrganiser("Booking", string.Empty, new [] 
                { 
                    new FilterArguments { FilterOn = "code", ExpectedFilteredDetails = new[] { "HS Code", "HS Code Description", "Instruction Code" } },    // Pass
                    new FilterArguments { FilterOn = "code", ExpectedFilteredDetails = new[] { "HS Code", "HS Code Description" } },    // Failed - Extra in the list - Instruction Code
                    new FilterArguments { FilterOn = "code", ExpectedFilteredDetails = new[] { "HS CodeA", "HS Code Description", "Instruction Code" } },    // Failed - HS CodeA lbs not found in the list and HS Code is extra in the list
                }, new []
                { new FilterArguments { FilterOn = "m3", ExpectedFilteredDetails = new[] { "Ventilation m3/hr", "Volume m3" } },    // Pass
                }, new [] { "Cargo Type", "Curtains In",  "ISO Group", "Rolled From", "Special Lift" // Pass
                }, new[]
                { 
                    new FilterArguments { FilterOn = "Car", ExpectedFilteredDetails = new[] { "Carriage Temperature (deg C)", "Carriage Temperature (deg F)", "Carrier Reference" } },    // Pass
                    new FilterArguments { FilterOn = "rolle", ExpectedFilteredDetails = new[] { "Controlled Atmosphere", "Rolled From", "Rolled To"} },    // Pass
                    new FilterArguments { FilterOn = "HS", ExpectedFilteredDetails = new[] { "HS Code", "HS Code Description"} },    // Pass
                },
                new [] 
                { 
                    new FilterArguments { FilterOn = "Tranship", ExpectedFilteredDetails = new[] { "Tranship to Port", "Tranship", "Tranship Voyage" } },    // Pass
                    new FilterArguments { FilterOn = "Tranship", ExpectedFilteredDetails = new[] { "Tranship to Port", "Tranship", } },    // Failed - Extra in the list - Tranship Voyage
                    new FilterArguments { FilterOn = "Tranship", ExpectedFilteredDetails = new[] { "Tranship to Port", "TranshipA", "Tranship Voyage" } },    // Failed -TranshipA not found in the list and Tranship is extra in the list
                }, new []
                { new FilterArguments { FilterOn = "on", ExpectedFilteredDetails = new[] { "Creation Date", "Destination", "Consignor" } },    // Pass
                }, new [] { "Operator", "Stevedore", "StockA", "Vessel", "Pack As"  // Fail - StockA
                }, new[]
                { 
                    new FilterArguments { FilterOn = "Commod", ExpectedFilteredDetails = new[] { "Commodity", "Commodity Group" } },    // Pass
                    new FilterArguments { FilterOn = "Book", ExpectedFilteredDetails = new[] { "Booking", "Booking Remarks", "No. Linked Stock Booking Items"} },    // Pass
                    new FilterArguments { FilterOn = "Port", ExpectedFilteredDetails = new[] { "Discharge Port", "Tranship to Port"} },    // Pass
                });
            return notFoundDetailsToValidate;
        }

        string DoColumnOrganiser(string fromForm, string details, FilterArguments[] validateAvailableSingleFilter, FilterArguments[] validateAvailableStringFilter2, string[] clearAvailableSingleFilter, FilterArguments[] validateAvailableMultiFilter,
            FilterArguments[] validateSelectAndOrderSingleFilter, FilterArguments[] validateSelectAndOrderStringFilter2, string[] clearSelectAndOrderSingleFilter, params FilterArguments[] validateSelectAndOrderMultiFilter)
        {
            var columnOrganiserForm = new ColumnOrganiserForm($"Column Organiser {fromForm} TT1");
            columnOrganiserForm.lstSelect
                .ValidateAvailableFilteredDetails(validateAvailableSingleFilter, $"{fromForm}{details} - Available - Validate - {validateAvailableSingleFilter[0].FilterOn}", out var availableFilteredDetailsNotFound, false)
                .DoAvailableClearAndValidateDetails(clearAvailableSingleFilter, $"{fromForm}{details} - Available - Clear & Validate", out var clearDetailsNotFound, false)
                .ValidateAvailableFilteredDetails(validateAvailableStringFilter2, $"{fromForm}{details} - Available - Validate - {validateAvailableStringFilter2[0].FilterOn}", out var availableFilteredDetailsNotFound1, false)
                .DoAvailableClearAndValidateDetails(clearAvailableSingleFilter, $"{fromForm}{details} - Available - Clear & Validate", out var clearDetailsNotFound1, false)
                .ValidateAvailableFilteredDetails(validateAvailableMultiFilter, $"{fromForm}{details} - Available - Validate - multiple", out var availableFilteredDetailsNotFound2, false)
                .ValidateSelectAndOrderFilteredDetails(validateSelectAndOrderSingleFilter, $"{fromForm}{details} - Select And Order - Validate - {validateAvailableStringFilter2[0].FilterOn}", out var selectedAndOrderFilteredDetailsNotFound, false)
                .CheckUnableToClickUpOrDownArrow(validateSelectAndOrderSingleFilter[0].ExpectedFilteredDetails[1], out var ableToClickUpOrDownArrow)
                .DoSelectAndOrderClearAndValidateDetails(clearSelectAndOrderSingleFilter, $"{fromForm}{details} - Select And Order - Clear & Validate", out var clearDetailsNotFound2, false)
                .CheckAbleToClickUpOrDownArrow(validateSelectAndOrderSingleFilter[0].ExpectedFilteredDetails[1], out var unableToClickUpOrDownArrow)
                .ValidateSelectAndOrderFilteredDetails(validateSelectAndOrderStringFilter2, $"{fromForm}{details} - Select And Order - Validate - {validateAvailableStringFilter2[0].FilterOn}", out var selectedAndOrderFilteredDetailsNotFound1, false)
                .DoSelectAndOrderClearAndValidateDetails(clearSelectAndOrderSingleFilter, $"{fromForm}{details} - Select And Order - Clear & Validate", out var clearDetailsNotFound3, false) 
                .ValidateSelectAndOrderFilteredDetails(validateSelectAndOrderMultiFilter, $"{fromForm}{details} - Select And Order - Validate - multiple", out var selectedAndOrderFilteredDetailsNotFound2, false)
                .DoCancel();


            var notFoundDetailsToValidate = availableFilteredDetailsNotFound + clearDetailsNotFound +
                                            availableFilteredDetailsNotFound1 + clearDetailsNotFound1 +
                                            availableFilteredDetailsNotFound2 + selectedAndOrderFilteredDetailsNotFound +
                                            clearDetailsNotFound2 + selectedAndOrderFilteredDetailsNotFound1 + clearDetailsNotFound3 +
                                            selectedAndOrderFilteredDetailsNotFound2 + unableToClickUpOrDownArrow + ableToClickUpOrDownArrow;
            
            Console.WriteLine("notFoundDetailsToValidate: " + notFoundDetailsToValidate);
            return notFoundDetailsToValidate;
        }


        void ClickContextMenuColumnOrganizerForRequestedTable(MTNDataTableHHeader table, bool alternateSpelling = false)
        {
            Mouse.RightClick(table.GetElement().BoundingRectangle.Center());
            
            var columnOrganiserText = "Column Organiser";
            if (!alternateSpelling)
                columnOrganiserText += "...";
            
            table.contextMenu.MenuSelect(columnOrganiserText);
        }
    }
    
}
