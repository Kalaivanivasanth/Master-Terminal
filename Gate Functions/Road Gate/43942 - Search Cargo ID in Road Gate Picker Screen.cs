using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43942 : MTNBase
    {

        RoadGatePickerForm _roadGatePickerForm;

        const string EDIFile1 = "M_43942_Booking_Delete.xml";
        const string EDIFile2 = "M_43942_Prenote_Delete.xml";
        const string EDIFile3 = "M_43942_Booking_Add.xml";
        const string EDIFile4 = "M_43942_Prenote_Add.xml";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            
            searchFor = @"_43942_";
            
             //create Booking file
            CreateDataFile(EDIFile1,
             "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n  <BookingHeader>\n	<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>JLGBOOK43942</id>\n	<dischargePort>NZAKL</dischargePort>\n	<voyageCode>MSCK000002</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>D</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK43942</id>\n		<isoType>2200</isoType>\n		<number>3</number>\n		<totalWeight>6000</totalWeight>\n		<commodityDescription>GEN</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n\n");

            CreateDataFile(EDIFile2,
            " <?xml version='1.0'?>\n<JMTInternalPrenote>\n<AllPrenote>\n<Prenote>\n	<bookingRef>JLGBOOK43942</bookingRef>\n	<cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG43942_01</id>\n            <isoType>2200</isoType>\n            <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>D</messageMode>\n			</Prenote>\n			<Prenote>\n				<bookingRef>JLGBOOK43942</bookingRef>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG43942_02</id>\n            <isoType>2200</isoType>\n            <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>D</messageMode>\n					</Prenote>\n				<Prenote>\n				<bookingRef>JLGBOOK43942</bookingRef>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG43942_03</id>\n            <isoType>2200</isoType>\n            <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>D</messageMode>\n</Prenote>\n</AllPrenote>\n</JMTInternalPrenote>\n\n");

            CreateDataFile(EDIFile3,
            "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n  <BookingHeader>\n	<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>JLGBOOK43942</id>\n	<dischargePort>NZAKL</dischargePort>\n	<voyageCode>MSCK000002</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>A</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK43942</id>\n		<isoType>2200</isoType>\n		<number>3</number>\n		<totalWeight>6000</totalWeight>\n		<commodityDescription>GEN</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n");

            CreateDataFile(EDIFile4,
            " <?xml version='1.0'?>\n<JMTInternalPrenote>\n<AllPrenote>\n<Prenote>\n	<bookingRef>JLGBOOK43942</bookingRef>\n	<cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG43942_01</id>\n            <isoType>2200</isoType>\n            <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n			</Prenote>\n			<Prenote>\n				<bookingRef>JLGBOOK43942</bookingRef>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG43942_02</id>\n            <isoType>2200</isoType>\n            <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n					</Prenote>\n				<Prenote>\n				<bookingRef>JLGBOOK43942</bookingRef>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG43942_03</id>\n            <isoType>2200</isoType>\n            <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n</Prenote>\n</AllPrenote>\n</JMTInternalPrenote>\n\n\n");

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void SearchCargoIdInRoadGatePickerScreen()
        {
            MTNInitialize();
            
            //Setup
            // Tuesday, 28 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            EDIOperationsForm ediOperationsForm = new EDIOperationsForm();
            ediOperationsForm.LoadFileToDB(EDIFile1, @"Booking Reference Multi Line", @"43492");
            ediOperationsForm.LoadFileToDB(EDIFile2, @"Prenote", @"43492");
            ediOperationsForm.LoadFileToDB(EDIFile3);
            ediOperationsForm.LoadFileToDB(EDIFile4);

            // Tuesday, 28 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Show Filter in Picker", @"1", rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();


            // 1. Open road gate form and enter vehicle visit details
            // Tuesday, 28 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"43942");
            roadGateForm.SetRegoCarrierGate("43942");
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK43942", 10, checkForMatch: false);

            _roadGatePickerForm = new RoadGatePickerForm(@"Picker");
            /*// Tuesday, 28 January 2025 navmh5 
            MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_01", clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_02", clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_03", clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^3 x 2200  GENERAL~Item Id^JLGBOOK43942", clickType: ClickType.None);*/
            _roadGatePickerForm.TblPickerItems.FindClickRow(
                new[]
                {
                    "Description^2200 GENERAL~Cargo Id^JLG43942_01", "Description^2200 GENERAL~Cargo Id^JLG43942_02",
                    "Description^2200 GENERAL~Cargo Id^JLG43942_03",
                    "Description^3 x 2200  GENERAL~Item Id^JLGBOOK43942"
                }, ClickType.None);

            Miscellaneous.WaitForSeconds(1);
            _roadGatePickerForm.txtFilterBy.SetValue("01");
            Miscellaneous.WaitForSeconds(1);
            // MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_01", clickType: ClickType.None);
            // var find1 = MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_02", doAssert: false, clickType: ClickType.None);
            // var find2 = MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_03", doAssert: false, clickType: ClickType.None);
            // var find3 = MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^3 x 2200  GENERAL~Item Id^JLGBOOK43942", doAssert: false, clickType: ClickType.None);
            _roadGatePickerForm.TblPickerItems.FindClickRow([
                "Description^2200 GENERAL~Cargo Id^JLG43942_01",
                "Description^2200 GENERAL~Cargo Id^JLG43942_02",
                "Description^2200 GENERAL~Cargo Id^JLG43942_03",
                "Description^3 x 2200  GENERAL~Item Id^JLGBOOK43942"
            ], clickType: ClickType.None);            
            var find1 = MTNControlBase.FindClickRowInTable(_roadGatePickerForm.TblPickerItems.GetElement(), @"Description^2200 GENERAL~Cargo Id^JLG43942_02", doAssert: false, clickType: ClickType.None);
            var find2 = MTNControlBase.FindClickRowInTable(_roadGatePickerForm.TblPickerItems.GetElement(), @"Description^2200 GENERAL~Cargo Id^JLG43942_03", doAssert: false, clickType: ClickType.None);
            var find3 = MTNControlBase.FindClickRowInTable(_roadGatePickerForm.TblPickerItems.GetElement(), @"Description^3 x 2200  GENERAL~Item Id^JLGBOOK43942", doAssert: false, clickType: ClickType.None);
            Assert.IsFalse((find1) && (find2) && (!find3), @"Too many Picker Rows Detected");
            _roadGatePickerForm.txtFilterBy.SetValue(string.Empty);
            Miscellaneous.WaitForSeconds(1);
            // MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_01", clickType: ClickType.None);
            // MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_02", clickType: ClickType.None);
            // MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_03", clickType: ClickType.None);
            // MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^3 x 2200  GENERAL~Cargo Id^", clickType: ClickType.None);
            _roadGatePickerForm.TblPickerItems.FindClickRow([
                "Description^2200 GENERAL~Cargo Id^JLG43942_01",
                "Description^2200 GENERAL~Cargo Id^JLG43942_02",
                "Description^2200 GENERAL~Cargo Id^JLG43942_03",
                "Description^3 x 2200  GENERAL~Cargo Id^"
            ], clickType: ClickType.None);
            Miscellaneous.WaitForSeconds(1);
            _roadGatePickerForm.txtFilterBy.SetValue("02");
            Miscellaneous.WaitForSeconds(1);
             // MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_02", clickType: ClickType.None);
            // find1 = MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_01", doAssert: false, clickType: ClickType.None);
            // find2 = MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_03", doAssert: false, clickType: ClickType.None);
            // find3 = MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^3 x 2200  GENERAL~Item Id^JLGBOOK43942", doAssert: false, clickType: ClickType.None);
             _roadGatePickerForm.TblPickerItems.FindClickRow([
                 "Description^2200 GENERAL~Cargo Id^JLG43942_02",
                 "Description^2200 GENERAL~Cargo Id^JLG43942_01",
                 "Description^2200 GENERAL~Cargo Id^JLG43942_03",
                 "Description^3 x 2200  GENERAL~Item Id^JLGBOOK43942"
             ], clickType: ClickType.None);
             Assert.IsFalse((find1) && (find2) && (!find3), @"Too many Picker Rows Detected");
            _roadGatePickerForm.txtFilterBy.SetValue(string.Empty);
            Miscellaneous.WaitForSeconds(1);
            // MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_01", clickType: ClickType.None);
            // MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_02", clickType: ClickType.None);
            // MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^2200 GENERAL~Cargo Id^JLG43942_03", clickType: ClickType.None);
            // MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Description^3 x 2200  GENERAL~Cargo Id^", clickType: ClickType.None);
            _roadGatePickerForm.TblPickerItems.FindClickRow([
                "Description^2200 GENERAL~Cargo Id^JLG43942_01",
                "Description^2200 GENERAL~Cargo Id^JLG43942_02",
                "Description^2200 GENERAL~Cargo Id^JLG43942_03",
                "Description^3 x 2200  GENERAL~Cargo Id^"
            ], clickType: ClickType.None);
            _roadGatePickerForm.btnCancel.DoClick();

            roadGateForm.SetFocusToForm();
            roadGateForm.CancelVehicleVisit(@"Test 43942");

        }

    }

}
