using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Bookings
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41268 : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

      
        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize() => LogInto<MTNLogInOutBO>("USERCFGT");
    

        [TestMethod]
        public void RollBookingForOnboardTranshipContainer()
        {
            
            MTNInitialize();
            
            // Step 2 - 3
            SetTerminalConfigValues(@"1", @"1");

            // Step 4 - 5
            loadFileDeleteStartTime = DateTime.Now; // REMOVE AT YOUR OWN PERIL
            SetupAndLoadInitializeData(TestContext);

            // Step 6
            //FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Operations|Voyage Operations");
            FormObjectBase.MainForm.OpenVoyageOperationsFromToolbar();
            VoyageOperationsForm voyageOperationsForm = new VoyageOperationsForm();
            voyageOperationsForm.GetSearcherTab_ClassicMode();
            voyageOperationsForm.GetSearcherTab();

            // Step 7 - 8
            voyageOperationsForm.ChkLoloBays.DoClick();
            //voyageOperationsForm.SetValue(voyageOperationsForm.cmbVoyage, @"GRIG001");
            voyageOperationsForm.CmbVoyage.SetValue(TT3.Voyage.GRIG001, doDownArrow: true);
            //voyageOperationsForm.btnSelect.DoClick();
            voyageOperationsForm.DoSelect();
            voyageOperationsForm.GetMainDetails();
            // MTNControlBase.FindClickRowInTable(voyageOperationsForm.TblOnVessel,
                // @"Location^GRIG001~Total Quantity^1~ID^JLG41268A01~Cargo Type^ISO Container", 
                // ClickType.ContextClick, rowHeight: 16);
            voyageOperationsForm.TblOnVessel1.FindClickRow(["Location^GRIG001~Total Quantity^1~ID^JLG41268A01~Cargo Type^ISO Container"], ClickType.ContextClick);            /*voyageOperationsForm.tblOnVessel1.FindClickRow(
                new[] { @"Location^GRIG001~Total Quantity^1~ID^JLG41268A01~Cargo Type^ISO Container" }, 
                    ClickType.ContextClick);*/

            voyageOperationsForm.ContextMenuSelect(@"Tranship...", waitTimeOut:2000);
            TranshipForm transhipForm = new TranshipForm(@"JLG41268A01 TT3");

            transhipForm.cmbOutboundVoyage.SetValue(@"GRIG002	JOLLY GRIGIO", doDownArrow: true);
            transhipForm.cmbDischargePort.SetValue(Port.AKLNZ, doDownArrow: true);
            transhipForm.cmbDestinationPort.SetValue(Port.AALBE, doDownArrow: true);
            transhipForm.cmbTranshipTo.SetValue(Port.ADOJP, doDownArrow: true);
            transhipForm.txtOutboundBooking.SetValue(@"JLGBOOK41268");
            transhipForm.btnOK.DoClick();

            voyageOperationsForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(voyageOperationsForm.TblOnVessel,
                // @"Location^GRIG001~Total Quantity^1~ID^JLG41268A01~Cargo Type^ISO Container~IMEX Status^Tranship",
                // ClickType.None, rowHeight: 16);
            voyageOperationsForm.TblOnVessel1.FindClickRow(["Location^GRIG001~Total Quantity^1~ID^JLG41268A01~Cargo Type^ISO Container~IMEX Status^Tranship"], ClickType.None);
            // Step 11 - 13
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT3");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG41268A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", Site.OnShip/*,
                EditRowDataType.ComboBoxEdit, formCargoEnquiry: true*/);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

                //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG41268A01", rowHeight: 18);
            cargoEnquiryForm.tblData2.FindClickRow(new[] { @"ID^JLG41268A01" });

            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, "Tranship");
            cargoEnquiryForm.GetGenericTabTableDetails(@"Tranship", @"4157");

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Outbound Voyage", 
                TT3.Voyage.GRIG002);
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Inbound Voyage Code", 
                TT3.Voyage.GRIG001);

            // Step 14 - 19
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking", forceReset: true);
            FormObjectBase.MainForm.OpenBookingFromToolbar();
            BookingForm bookingForm = new BookingForm(@"Booking TT3");

            bookingForm.GetSearcherTableDetails();
            MTNControlBase.SetValueInEditTable(bookingForm.tblSearcher, @"Booking", @"JLGBOOK41268");
            //bookingForm.btnSearch.DoClick(2000);
            bookingForm.DoSearch();

            bookingForm.CheckOnlyOneBooking(new[] { "JLGBOOK41268" });
            // MTNControlBase.FindClickRowInTable(bookingForm.tblBookings,
                // @"Booking^JLGBOOK41268", ClickType.DoubleClick, rowHeight: 16, xOffset: 200, doAssert: false);
            bookingForm.TblBookings.FindClickRow(["Booking^JLGBOOK41268"], ClickType.DoubleClick);
            BookingItemsForm bookingItemsForm = new BookingItemsForm(@"Editing Booking JLGBOOK41268 TT3");
            bookingItemsForm.btnRoll.DoClick();

            RollBookingForm rollBookingForm = new RollBookingForm(@"Roll Booking TT3");
            //MTNControlBase.SetValue(rollBookingForm.cmbNewVoyage, @"GRIG003");
            rollBookingForm.cmbNewVoyage.SetValue(@"GRIG003 - JOLLY GRIGIO");
            rollBookingForm.btnSave.DoClick();

            bookingForm.SetFocusToForm();
            bookingForm.CloseForm();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Roll Booking TT3");
            string[] warningErrorToCheck = new string[]
            {
                "Code :82578. Booking/Pre-note differences: The Voyage for JLG41268A01 (GRIG001) does not match the Booking (GRIG003). The Destination port for JLG41268A01 (BEAAL) does not match the Booking ()."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();
            // MTNControlBase.FindClickRowInTable(bookingForm.tblBookings,
                // @"Booking^JLGBOOK41268~Items^2~Received^1~Released^0~Reserved^0~Pre-noted^0~Operator^MSC" +
                // @"~Voyage^GRIG003~Discharge Port^NZAKL");
            bookingForm.TblBookings.FindClickRow(["Booking^JLGBOOK41268~Items^2~Received^1~Released^0~Reserved^0~Pre-noted^0~Operator^MSC" +
                @"~Voyage^GRIG003~Discharge Port^NZAKL"]);
           // Step 20 - 21
           cargoEnquiryForm.SetFocusToForm();
           MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Outbound Voyage",
               TT3.Voyage.GRIG003);

            // Step 22
           //cargoEnquiryForm.btnViewTransaction.DoClick();
           cargoEnquiryForm.DoViewTransactions();

           CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
           // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
               // @"Type^Roll Booking for Cargo~User^USERCFGT~Details^Booking Reference  => JLGBOOK41268 Voyage GRIG001 = > GRIG003");
           cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(["Type^Roll Booking for Cargo~User^USERCFGT~Details^Booking Reference  => JLGBOOK41268 Voyage GRIG001 = > GRIG003"]);           cargoEnquiryTransactionForm.CloseForm();

        }



        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_41268_";
            
            // Delete Cargo On Ship
            CreateDataFileToLoad(@"DeleteCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<isoType>2200</isoType>\n            <id>JLG41268A01</id>\n            <voyageCode>GRIG001</voyageCode>\n            <operatorCode>MSC</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <weight>45</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GEN</commodity>\n			 <messageMode>D</messageMode>\n        </CargoOnShip>\n          </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n\n");

            // Delete Booking
            CreateDataFileToLoad(@"DeleteBooking.xml",
                "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n  <BookingHeader Terminal='TT3'>\n	<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>JLGBOOK41268</id>\n	<dischargePort>NZAKL</dischargePort>\n	<voyageCode>GRIG002</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>D</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK41268</id>\n		<isoType>2200</isoType>\n		<number>2</number>\n		<totalWeight>6000</totalWeight>\n		<commodityDescription>GEN</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n");

            // Create Booking
            CreateDataFileToLoad(@"CreateBooking.xml",
                "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n  <BookingHeader Terminal='TT3'>\n	<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>JLGBOOK41268</id>\n	<dischargePort>NZAKL</dischargePort>\n	<voyageCode>GRIG002</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>A</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK41268</id>\n		<isoType>2200</isoType>\n		<number>2</number>\n		<totalWeight>6000</totalWeight>\n		<commodityDescription>GEN</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n\n");

            // Create Cargo On Ship
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT3'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<isoType>2200</isoType>\n            <id>JLG41268A01</id>\n            <voyageCode>GRIG001</voyageCode>\n            <operatorCode>MSC</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <weight>45</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GEN</commodity>\n			 <messageMode>A</messageMode>\n        </CargoOnShip>\n          </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }


        private void SetTerminalConfigValues(string automaticBookingCargoUpdates, 
            string automaticBookingCargoUpdatesForTranshipExportVoyage)
        {
            //FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            //terminalConfigForm.GetDetailsTab();
            //FormObjectBase.ClickButton(terminalConfigForm.btnEdit);
            //terminalConfigForm.btnEdit.DoClick();
            //terminalConfigForm.DoEdit();

            //MTNControlBase.FindTabOnForm(terminalConfigForm.tabDetails, @"Settings");
            terminalConfigForm.GetGenericTabAndTable(@"Settings");
            terminalConfigForm.DoEdit();
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
