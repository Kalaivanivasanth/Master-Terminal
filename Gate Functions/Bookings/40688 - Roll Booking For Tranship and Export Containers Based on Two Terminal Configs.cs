using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using System.Reflection;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Bookings
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40688 : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
   
        [TestCleanup]
        public new void TestCleanup()
        {
            SetTerminalConfigValues(@"1", @"0");

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>("USERCFGT");
        }


        [TestMethod]
        public void RollBookingForTranshipExportContainersBasedOnTwoTerminalConfigs()
        {
            MTNInitialize();
            
            // Iteration 1: Automatic Booking Cargo Updates -True | Automatic Booking Cargo Updates For Tranships Export Voyage - True
            // Expected Result -Voyage should be updated on Export cargo as well as on tranships
            RollBookingAndValidate(@"1", @"1", @"GRIG003", 
                true);

            // Iteration 2: Automatic Booking Cargo Updates -True | Automatic Booking Cargo Updates For Tranships Export Voyage - False
            // Expected Result -Voyage should be updated on Export cargo but not on tranships
            RollBookingAndValidate(@"0", @"0", @"GRIG002");

            // Iteration 3: Automatic Booking Cargo Updates -False | Automatic Booking Cargo Updates For Tranships Export Voyage - True
            // Expected Result -Voyage should be updated on Export cargo but not on tranships
            RollBookingAndValidate(@"0", @"1", @"GRIG003");

            // Iteration 4: Automatic Booking Cargo Updates -False | Automatic Booking Cargo Updates For Tranships Export Voyage - False
            // Expected Result -Voyage should be updated on Export cargo but not on tranships
            RollBookingAndValidate(@"1", @"0", @"GRIG002");

        }

        

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_40688_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite\n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnSite Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40688A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG002</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>D</messageMode>\n			<bookingRef>JLGBOOK40688A01</bookingRef>\n			<bookingId>JLGBOOK40688A01</bookingId>\n			<bookingItemSeq></bookingItemSeq>\n        </CargoOnSite>\n        <CargoOnSite Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40688A02</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG002</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>D</messageMode>\n			<bookingRef>JLGBOOK40688A01</bookingRef>\n			<bookingId>JLGBOOK40688A01</bookingId>\n			<bookingItemSeq></bookingItemSeq>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Delete Cargo On Ship
            CreateDataFileToLoad(@"DeleteCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnShip Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40688A03</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG001</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>AUSYD</destinationPort>\n            <locationId>118419</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>MT</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnShip>\n        <CargoOnShip Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40688A04</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG001</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>AUBNE</destinationPort>\n            <locationId>118219</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnShip>\n    </AllCargoOnShip>\n</JMTInternalCargoOnShip>");

            // Delete Booking
            CreateDataFileToLoad(@"DeleteBooking.xml",
                "<?xml version='1.0'?>\n<JMTInternalBooking>\n	<AllBookingHeader>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<BookingHeader Terminal='TT3'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n			<id>JLGBOOK40688A01</id>\n			<messageMode>D</messageMode>\n			<operatorCode>HSD</operatorCode>\n			<vesselName>GRIG</vesselName>\n			<voyageCode>GRIG002</voyageCode>\n				<AllBookingItem>\n					<BookingItem>\n						<weight>18830.0000</weight>\n						<number>5</number>\n						<cargoTypeDescr>ISO Container</cargoTypeDescr>\n						<commodity>APPL</commodity>\n						<fullOrMT>F</fullOrMT>\n						<id>JLGBOOK40688A01</id>\n						<isoType>2200</isoType>\n						<messageMode>D</messageMode>\n					</BookingItem>\n				</AllBookingItem>\n			</BookingHeader>\n	</AllBookingHeader>\n</JMTInternalBooking>\n");

            // Create Booking
            CreateDataFileToLoad(@"CreateBooking.xml",
                "<?xml version='1.0'?>\n<JMTInternalBooking>\n	<AllBookingHeader>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<BookingHeader Terminal='TT3'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n			<id>JLGBOOK40688A01</id>\n			<messageMode>A</messageMode>\n			<operatorCode>HSD</operatorCode>\n			<vesselName>GRIG</vesselName>\n			<voyageCode>GRIG002</voyageCode>\n				<AllBookingItem>\n					<BookingItem>\n						<weight>18830.0000</weight>\n						<number>5</number>\n						<cargoTypeDescr>ISO Container</cargoTypeDescr>\n						<commodity>APPL</commodity>\n						<fullOrMT>F</fullOrMT>\n						<id>JLGBOOK40688A01</id>\n						<isoType>2200</isoType>\n						<messageMode>A</messageMode>\n					</BookingItem>\n				</AllBookingItem>\n			</BookingHeader>\n	</AllBookingHeader>\n</JMTInternalBooking>\n");

            // Create Cargo On Ship
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnShip Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40688A03</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG001</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n            <locationId>118419</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n        <CargoOnShip Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40688A04</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG001</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n            <locationId>118219</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n    </AllCargoOnShip>\n</JMTInternalCargoOnShip>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite\n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnSite Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40688A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG002</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>A</messageMode>\n			<bookingRef>JLGBOOK40688A01</bookingRef>\n			<bookingId>JLGBOOK40688A01</bookingId>\n			<bookingItemSeq></bookingItemSeq>\n        </CargoOnSite>\n        <CargoOnSite Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40688A02</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG002</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>A</messageMode>\n			<bookingRef>JLGBOOK40688A01</bookingRef>\n			<bookingId>JLGBOOK40688A01</bookingId>\n			<bookingItemSeq></bookingItemSeq>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Tranship
            CreateDataFileToLoad(@"Tranship.xml",
                "<?xml version='1.0'?> <JMTInternalTranship xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalTranship.xsd'>\n	<AllTranship>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<Tranship Terminal='TT3'>\n			<weight/>\n			<temperature/>\n			<billOfLading/>\n			<bookingRef>JLGBOOK40688A01</bookingRef>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			 <commodity>APPL</commodity>\n			<containerId>JLG40688A03</containerId>\n			<description/>\n			<destinationPort/>\n			<dischargePort>AUSYD</dischargePort>\n			<exportVesselName>JOLLY GRIGIO</exportVesselName>\n			<exportVoyageCode>GRIG002</exportVoyageCode>\n			<fullOrMT/>\n			<id>JLG40688A03</id>\n			<isoType>2200</isoType>\n			<messageMode>A</messageMode>\n			<voyageCode>GRIG001</voyageCode>\n		</Tranship>\n		<Tranship Terminal='TT3'>\n			<weight/>\n			<temperature/>\n			<billOfLading/>\n			<bookingRef>JLGBOOK40688A01</bookingRef>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			 <commodity>APPL</commodity>\n			<containerId>JLG40688A04</containerId>\n			<description/>\n			<destinationPort/>\n			<dischargePort>AUSYD</dischargePort>\n			<exportVesselName>JOLLY GRIGIO</exportVesselName>\n			<exportVoyageCode>GRIG002</exportVoyageCode>\n			<fullOrMT/>\n			<id>JLG40688A04</id>\n			<isoType>2200</isoType>\n			<messageMode>A</messageMode>\n			<voyageCode>GRIG001</voyageCode>\n		</Tranship>\n	</AllTranship>\n</JMTInternalTranship>");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        private void RollBookingAndValidate(string automaticBookingCargoUpdates,
            string automaticBookingCargoUpdatesForTranshipExportVoyage, string voyageCode, bool firstTime = false)
        {

            var voyageCodeFull = $"{voyageCode} - JOLLY GRIGIO";
            
            // Step 2 - 3
            SetTerminalConfigValues(automaticBookingCargoUpdates, automaticBookingCargoUpdatesForTranshipExportVoyage);

            // Step 4 - 5
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking", forceReset: true);
            FormObjectBase.MainForm.OpenBookingFromToolbar();
            BookingForm bookingForm = new BookingForm(@"Booking TT3");

            bookingForm.GetSearcherTableDetails();
            MTNControlBase.SetValueInEditTable(bookingForm.tblSearcher, @"Booking", @"JLGBOOK40688A01");
            //bookingForm.btnSearch.DoClick(2000);
            bookingForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(bookingForm.tblBookings,
                // @"Booking^JLGBOOK40688A01", ClickType.ContextClick, rowHeight: 16, xOffset: 200, doAssert: false);
            bookingForm.TblBookings.FindClickRow(["Booking^JLGBOOK40688A01"], ClickType.ContextClick);            bookingForm.ContextMenuSelect(@"Roll Booking(s)...");

            RollBookingForm rollBookingForm = new RollBookingForm(@"Roll Booking TT3");
            //MTNControlBase.SetValue(rollBookingForm.cmbNewVoyage, voyageCode);
            rollBookingForm.cmbNewVoyage.SetValue(voyageCodeFull);
            //MTNControlBase.SetValue(rollBookingForm.txtNewBooking, @"JLGBOOK40688A01");
            rollBookingForm.txtNewBooking.SetValue(@"JLGBOOK40688A01");
            rollBookingForm.btnSave.DoClick();

            bookingForm.SetFocusToForm();
            bookingForm.CloseForm();

            if (firstTime)
            {
                warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Roll Booking TT3");
                string[] warningErrorToCheck = new string[]
                {
                    $"Code :82578. Booking/Pre-note differences: The Voyage for JLG40688A04 (GRIG001) does not match the Booking ({voyageCode}).",
                    $"Code :82578. Booking/Pre-note differences: The Voyage for JLG40688A03 (GRIG001) does not match the Booking ({voyageCode})."
                };
                warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
                warningErrorForm.btnSave.DoClick();
                // MTNControlBase.FindClickRowInTable(bookingForm.tblBookings,
                    // @"Booking^JLGBOOK40688A01~Items^5~Received^4~Released^0~Reserved^0~Pre-noted^0~Operator^HSD" +
                    // @"~Voyage^" + voyageCode + "~Discharge Port^NZAKL");
                bookingForm.TblBookings.FindClickRow(["Booking^JLGBOOK40688A01~Items^5~Received^4~Released^0~Reserved^0~Pre-noted^0~Operator^HSD" +
                    @"~Voyage^" + voyageCode + "~Discharge Port^NZAKL"]);
            }

            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT3");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG40688");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", Site.Anywhere
                /*, EditRowDataType.ComboBoxEdit, formCargoEnquiry: true*/);
           //cargoEnquiryForm.btnSearch.DoClick();
           cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, $@"ID^JLG40688A01~Voyage^{voyageCode}", rowHeight: 18);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, $@"ID^JLG40688A02~Voyage^{voyageCode}", rowHeight: 18);
            cargoEnquiryForm.tblData2.FindClickRow([
                $"ID^JLG40688A01~Voyage^{voyageCode}", $"ID^JLG40688A02~Voyage^{voyageCode}"
            ]);
            ValidateCargoA03A04(cargoEnquiryForm, @"ID^JLG40688A03",
                @"Item is On ship. Tranship x GRIG001 to GRIG003  to AUSYD");
            ValidateCargoA03A04(cargoEnquiryForm, @"ID^JLG40688A04",
                @"Item is On ship. Tranship x GRIG001 to GRIG003  to AUSYD");
            cargoEnquiryForm.CloseForm();
        }



        private void SetTerminalConfigValues(string automaticBookingCargoUpdates, 
            string automaticBookingCargoUpdatesForTranshipExportVoyage)
        {
            //FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.GetDetailsTab();
            //FormObjectBase.ClickButton(terminalConfigForm.btnEdit);
            //terminalConfigForm.btnEdit.DoClick();
            //terminalConfigForm.DoEdit();

            //MTNControlBase.FindTabOnForm(terminalConfigForm.tabDetails, @"Settings");
            terminalConfigForm.GetGenericTabAndTable(@"Settings");
            terminalConfigForm.DoEdit();
            MTNControlBase.SetValueInEditTable(terminalConfigForm.tblGeneric,
                @"Automatic Booking Cargo Updates",
                automaticBookingCargoUpdatesForTranshipExportVoyage, EditRowDataType.CheckBox);
            MTNControlBase.SetValueInEditTable(terminalConfigForm.tblGeneric,
                @"Automatic Booking Cargo Updates For Tranships Export Voyage",
                automaticBookingCargoUpdates, EditRowDataType.CheckBox);

            //FormObjectBase.ClickButton(terminalConfigForm.btnSave);
            //terminalConfigForm.btnSave.DoClick();
            terminalConfigForm.DoSave();
            terminalConfigForm.CloseForm();
        }


        private void ValidateCargoA03A04(CargoEnquiryForm cargoEnquiryForm, string cargoId, string expectedInfoDetails)
        {
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, cargoId, rowHeight: 18);
            cargoEnquiryForm.tblData2.FindClickRow([cargoId]);            
            if (cargoId.Contains(@"JLG40688A03"))
            {
                //cargoEnquiryForm.CargoEnquiryGeneralTab();
                //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, "Tranship");
                cargoEnquiryForm.GetGenericTabTableDetails(@"Tranship", @"4157");
            }

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Outbound Voyage", @"GRIG003");
            cargoEnquiryForm.GetInformationTable();
            var infoDetails =
                MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblInformation, @"Info").Replace(
                    "\r\n", string.Empty);
            Assert.IsTrue(infoDetails.Equals(expectedInfoDetails),
                @"TestCase040688 - Info does not match.  Expected: " + expectedInfoDetails + @"    Actual: " + infoDetails);
        }

    }


}
