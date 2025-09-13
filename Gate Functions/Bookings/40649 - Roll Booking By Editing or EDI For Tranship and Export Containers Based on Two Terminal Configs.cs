using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNBaseClasses.BaseClasses;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Bookings
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40649 : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
      
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>("USERCFGT");
        }


        [TestMethod]
        public void RollBookingByEditingOrEDIForTranshipExportContainersBasedOnTwoTerminalConfigs()
        {
            MTNInitialize();
            
            // Iteration 1: Automatic Booking Cargo Updates -True | Automatic Booking Cargo Updates For Tranships Export Voyage - True
            // Expected Result - Voyage should be updated on Export cargo as well as on tranships
            RollBookingByEditingAndEDI(@"1", @"1",
                @"GRIG003", @"Item is On ship. Tranship x GRIG001 to GRIG003  to AUSYD",
                @"GRIG003", @"GRIG002", @"Item is On ship. Tranship x GRIG001 to GRIG002  to AUSYD");
           
            // Iteration 2: Automatic Booking Cargo Updates -False | Automatic Booking Cargo Updates For Tranships Export Voyage - False
            // Expected Result - Voyage should not be updated on Export cargo or on tranships
            RollBookingByEditingAndEDI(@"0", @"0",
                @"GRIG002", @"Item is On ship. Tranship x GRIG001 to GRIG002  to AUSYD",
                @"GRIG002", @"GRIG002", @"Item is On ship. Tranship x GRIG001 to GRIG002  to AUSYD");
          
            // Iteration 3: Automatic Booking Cargo Updates -False | Automatic Booking Cargo Updates For Tranships Export Voyage - True
            // Expected Result - Voyage should not be updated on Export cargo or on tranships
           RollBookingByEditingAndEDI(@"0", @"1",
                @"GRIG002", @"Item is On ship. Tranship x GRIG001 to GRIG002  to AUSYD",
                @"GRIG002", @"GRIG002", @"Item is On ship. Tranship x GRIG001 to GRIG002  to AUSYD");
      
            // Iteration 4: Automatic Booking Cargo Updates -True | Automatic Booking Cargo Updates For Tranships Export Voyage - False
            // Expected Result - Voyage should be updated on Export cargo but not on tranships
            RollBookingByEditingAndEDI(@"1", @"0",
                @"GRIG003", @"Item is On ship. Tranship x GRIG001 to GRIG002  to AUSYD",
                @"GRIG002", @"GRIG002", @"Item is On ship. Tranship x GRIG001 to GRIG002  to AUSYD");

        }


        private void RollBookingByEditingAndEDI(string automaticBookingCargoUpdates,
            string automaticBookingCargoUpdatesForTranshipExportVoyage, string voyage2, string info1,
            string outBoundVoyage, string voyage1, string info2)
        {
            // Step 2 - 3
            SetTerminalConfigValues(automaticBookingCargoUpdates, automaticBookingCargoUpdatesForTranshipExportVoyage);

            // Step 4 - 5
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking", forceReset: true);
            FormObjectBase.MainForm.OpenBookingFromToolbar();
            BookingForm bookingForm = new BookingForm(@"Booking TT3");

            bookingForm.GetSearcherTableDetails();
            MTNControlBase.SetValueInEditTable(bookingForm.tblSearcher, @"Booking", @"JLGBOOK40649A01");
            //bookingForm.btnSearch.DoClick(2000);
            bookingForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(bookingForm.tblBookings,
                // @"Booking^JLGBOOK40649A01", rowHeight: 16, xOffset: 200, doAssert: false);
            bookingForm.TblBookings.FindClickRow(["Booking^JLGBOOK40649A01"]);
            bookingForm.DoEdit();

            BookingItemsForm bookingItemsForm = new BookingItemsForm(@"Editing Booking JLGBOOK40649A01 TT3");
            //MTNControlBase.SetValue(bookingItemsForm.cmbVoyage, @"GRIG003");
            bookingItemsForm.cmbVoyage.SetValue(TT3.Voyage.GRIG003, doDownArrow: true);
            bookingItemsForm.btnSave.DoClick();
            // MTNControlBase.FindClickRowInTable(bookingForm.tblBookings,
                // @"Booking^JLGBOOK40649A01~Items^5~Received^2~Released^0~Reserved^0~Pre-noted^0~Operator^HSD" +
                // @"~Voyage^GRIG003~Discharge Port^NZBLU");
            bookingForm.TblBookings.FindClickRow(["Booking^JLGBOOK40649A01~Items^5~Received^2~Released^0~Reserved^0~Pre-noted^0~Operator^HSD" +
                @"~Voyage^GRIG003~Discharge Port^NZBLU"]);
            bookingForm.CloseForm();

            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT3");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG40649");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", Site.Anywhere,
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40649A01~Voyage^" +  voyage2,
            // rowHeight: 18);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID ^JLG40649A02~Voyage^" + voyage2,
            // rowHeight: 18);
            cargoEnquiryForm.tblData2.FindClickRow([
                $"ID^JLG40649A01~Voyage^{voyage2}",
                $"ID^JLG40649A02~Voyage^{voyage2}"
            ]);
            CheckA03A04(cargoEnquiryForm, @"ID^JLG40649A03", info1, outBoundVoyage);
            CheckA03A04(cargoEnquiryForm, @"ID^JLG40649A04", info1, outBoundVoyage);
            cargoEnquiryForm.CloseForm();

            SetupAndLoadInitializeData2(TestContext);

            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT3");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG40649");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"**Anywhere**",
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40649A01~Voyage^" + voyage1,
            // rowHeight: 18);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40649A02~Voyage^" + voyage1,
            // rowHeight: 18);
            cargoEnquiryForm.tblData2.FindClickRow([
                $"ID^JLG40649A01~Voyage^{voyage1}",
                $"ID^JLG40649A02~Voyage^{voyage1}"
            ]);
            CheckA03A04(cargoEnquiryForm, @"ID^JLG40649A03", info2, voyage1);
            CheckA03A04(cargoEnquiryForm, @"ID^JLG40649A04", info2, voyage1);
            cargoEnquiryForm.CloseForm();
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_40649_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite\n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnSite Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40649A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG002</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>D</messageMode>\n			<bookingRef>JLGBOOK40649A01</bookingRef>\n			<bookingId>JLGBOOK40649A01</bookingId>\n			<bookingItemSeq></bookingItemSeq>\n        </CargoOnSite>\n        <CargoOnSite Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40649A02</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG002</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>D</messageMode>\n			<bookingRef>JLGBOOK40649A01</bookingRef>\n			<bookingId>JLGBOOK40649A01</bookingId>\n			<bookingItemSeq></bookingItemSeq>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");
            
            // Delete Cargo On Ship
            CreateDataFileToLoad(@"DeleteCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnShip Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40649A03</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG001</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>AUSYD</destinationPort>\n            <locationId>118419</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>MT</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnShip>\n        <CargoOnShip Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40649A04</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG001</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>AUBNE</destinationPort>\n            <locationId>118219</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnShip>\n    </AllCargoOnShip>\n</JMTInternalCargoOnShip>");

            // Delete Booking
            CreateDataFileToLoad(@"DeleteBooking.xml",
                "<?xml version='1.0'?>\n<JMTInternalBooking>\n	<AllBookingHeader>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<BookingHeader Terminal='TT3'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZBLU</dischargePort>\n			<id>JLGBOOK40649A01</id>\n			<messageMode>D</messageMode>\n			<operatorCode>HSD</operatorCode>\n			<vesselName>GRIG</vesselName>\n			<voyageCode>GRIG002</voyageCode>\n				<AllBookingItem>\n					<BookingItem>\n						<weight>18830.0000</weight>\n						<number>5</number>\n						<cargoTypeDescr>ISO Container</cargoTypeDescr>\n						<commodity>APPL</commodity>\n						<fullOrMT>F</fullOrMT>\n						<id>JLGBOOK40649A01</id>\n						<isoType>2200</isoType>\n						<messageMode>D</messageMode>\n					</BookingItem>\n				</AllBookingItem>\n			</BookingHeader>\n	</AllBookingHeader>\n</JMTInternalBooking>\n");

            // Create Booking
            CreateDataFileToLoad(@"CreateBooking.xml",
                "<?xml version='1.0'?>\n<JMTInternalBooking>\n	<AllBookingHeader>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<BookingHeader Terminal='TT3'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZBLU</dischargePort>\n			<id>JLGBOOK40649A01</id>\n			<messageMode>A</messageMode>\n			<operatorCode>HSD</operatorCode>\n			<vesselName>GRIG</vesselName>\n			<voyageCode>GRIG002</voyageCode>\n				<AllBookingItem>\n					<BookingItem>\n						<weight>18830.0000</weight>\n						<number>5</number>\n						<cargoTypeDescr>ISO Container</cargoTypeDescr>\n						<commodity>APPL</commodity>\n						<fullOrMT>F</fullOrMT>\n						<id>JLGBOOK40649A01</id>\n						<isoType>2200</isoType>\n						<messageMode>A</messageMode>\n					</BookingItem>\n				</AllBookingItem>\n			</BookingHeader>\n	</AllBookingHeader>\n</JMTInternalBooking>\n");

            // Create Cargo On Ship
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnShip Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40649A03</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG001</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n            <locationId>118419</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n        <CargoOnShip Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40649A04</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG001</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n            <locationId>118219</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n    </AllCargoOnShip>\n</JMTInternalCargoOnShip>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite\n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnSite Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40649A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG002</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>A</messageMode>\n			<bookingRef>JLGBOOK40649A01</bookingRef>\n			<bookingId>JLGBOOK40649A01</bookingId>\n			<bookingItemSeq></bookingItemSeq>\n        </CargoOnSite>\n        <CargoOnSite Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40649A02</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG002</voyageCode>\n            <operatorCode>HSD</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <destinationPort>NZBLU</destinationPort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>A</messageMode>\n			<bookingRef>JLGBOOK40649A01</bookingRef>\n			<bookingId>JLGBOOK40649A01</bookingId>\n			<bookingItemSeq></bookingItemSeq>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Tranship
            CreateDataFileToLoad(@"Tranship.xml",
                "<?xml version='1.0'?> <JMTInternalTranship xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalTranship.xsd'>\n	<AllTranship>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<Tranship Terminal='TT3'>\n			<weight/>\n			<temperature/>\n			<billOfLading/>\n			<bookingRef>JLGBOOK40649A01</bookingRef>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			 <commodity>APPL</commodity>\n			<containerId>JLG40649A03</containerId>\n			<description/>\n			<destinationPort/>\n			<dischargePort>AUSYD</dischargePort>\n			<exportVesselName>JOLLY GRIGIO</exportVesselName>\n			<exportVoyageCode>GRIG002</exportVoyageCode>\n			<fullOrMT/>\n			<id>JLG40649A03</id>\n			<isoType>2200</isoType>\n			<messageMode>A</messageMode>\n			<voyageCode>GRIG001</voyageCode>\n		</Tranship>\n		<Tranship Terminal='TT3'>\n			<weight/>\n			<temperature/>\n			<billOfLading/>\n			<bookingRef>JLGBOOK40649A01</bookingRef>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			 <commodity>APPL</commodity>\n			<containerId>JLG40649A04</containerId>\n			<description/>\n			<destinationPort/>\n			<dischargePort>AUSYD</dischargePort>\n			<exportVesselName>JOLLY GRIGIO</exportVesselName>\n			<exportVoyageCode>GRIG002</exportVoyageCode>\n			<fullOrMT/>\n			<id>JLG40649A04</id>\n			<isoType>2200</isoType>\n			<messageMode>A</messageMode>\n			<voyageCode>GRIG001</voyageCode>\n		</Tranship>\n	</AllTranship>\n</JMTInternalTranship>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        private static void SetupAndLoadInitializeData2(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = "_40649_A_";

            // Booking Update
            CreateDataFileToLoad(@"BookingUpdate.xml",
                "<?xml version='1.0'?>\n<JMTInternalBooking>\n	<AllBookingHeader>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<BookingHeader Terminal='TT3'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZBLU</dischargePort>\n			<id>JLGBOOK40649A01</id>\n			<messageMode>U</messageMode>\n			<operatorCode>HSD</operatorCode>\n			<vesselName>GRIG</vesselName>\n			<voyageCode>GRIG002</voyageCode>\n				<AllBookingItem>\n					<BookingItem>\n						<weight>18830.0000</weight>\n						<number>5</number>\n						<cargoTypeDescr>ISO Container</cargoTypeDescr>\n						<commodity>APPL</commodity>\n						<fullOrMT>F</fullOrMT>\n						<id>JLGBOOK40649A01</id>\n						<isoType>2200</isoType>\n						<messageMode>A</messageMode>\n					</BookingItem>\n				</AllBookingItem>\n			</BookingHeader>\n	</AllBookingHeader>\n</JMTInternalBooking>\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }


        private void CheckA03A04(CargoEnquiryForm cargoEnquiryForm, string detailsToSearchFor, string info, string outBoundVoyage)
        {
            Console.WriteLine($"CheckA03A04 - detailsToSearchFor: {detailsToSearchFor} info: {info}  outBoundVoyage: {outBoundVoyage}");
            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, detailsToSearchFor);
            //cargoEnquiryForm.TblData.FindClickRow(detailsToSearchFor);
            cargoEnquiryForm.tblData2.FindClickRow(new string[] { detailsToSearchFor });

            if (detailsToSearchFor.Equals(@"ID^JLG40649A03"))
            {
                //cargoEnquiryForm.CargoEnquiryGeneralTab();
                //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, "Tranship");
                cargoEnquiryForm.GetGenericTabTableDetails(@"Tranship", @"4157");
            }
       
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Outbound Voyage",
                outBoundVoyage);
            cargoEnquiryForm.GetInformationTable();
            var infoDetails =
                MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblInformation, @"Info").Replace(
                    "\r", String.Empty).Replace("\n", String.Empty);
            Assert.IsTrue(infoDetails.Replace(" ", String.Empty).Equals(info.Replace(" ", String.Empty)),
                $@"TestCase40689 - Info does not match.  Expected: {info}    Actual: {infoDetails}");

        }



        private void SetTerminalConfigValues(string automaticBookingCargoUpdates, 
            string automaticBookingCargoUpdatesForTranshipExportVoyage)
        {
            //FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            //terminalConfigForm.GetDetailsTab();
            terminalConfigForm.GetGenericTabAndTable(@"Settings");
            //FormObjectBase.ClickButton(terminalConfigForm.btnEdit);
            //terminalConfigForm.btnEdit.DoClick();
            terminalConfigForm.DoEdit();

            //MTNControlBase.FindTabOnForm(terminalConfigForm.tabDetails, @"Settings");
            terminalConfigForm.GetGenericTabAndTable(@"Settings");
            MTNControlBase.SetValueInEditTable(terminalConfigForm.tblGeneric,
                @"Automatic Booking Cargo Updates",
                automaticBookingCargoUpdates, EditRowDataType.CheckBox);
            MTNControlBase.SetValueInEditTable(terminalConfigForm.tblGeneric,
                @"Automatic Booking Cargo Updates For Tranships Export Voyage",
                automaticBookingCargoUpdatesForTranshipExportVoyage, EditRowDataType.CheckBox);

            //FormObjectBase.ClickButton(terminalConfigForm.btnSave);
            //terminalConfigForm.btnSave.DoClick();
            terminalConfigForm.DoSave();
            terminalConfigForm.CloseForm();
        }


       

    }


}
