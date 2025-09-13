using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.Gate_Functions.Bookings.BookingItemForm;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;


namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41434 : MTNBase
    {

        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        BookingForm _bookingForm;
        BookingItemsForm _bookingItemsForm;
        BookingItemFormCont _bookingItemFormCont;
        ReleaseRequestForm _releaseRequestForm;
        RoadGatePickerForm _roadGatePickerForm;
        RoadGateDetailsReleaseForm _roadGateDetailsReleaseForm;

        const string EDIFile1 = "M_41434_DeleteBooking.xml";
        const string EDIFile2 = "M_41434_CreateBooking.xml";
        const string EDIFile3 = "M_41434_DeleteCargo.xml";

        private static readonly string MarkDetails = @"41434" + millisecondsSince20000101.ToString();

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
            // Delete bookings
            CreateDataFile(EDIFile1,
                "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n		<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n  <BookingHeader Terminal='TT1'>\n	<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>JLGBOOK41434A01</id>\n	<dischargePort>NZAKL</dischargePort>\n	<voyageCode>MSCK000002</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>D</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK41434A01</id>\n		<isoType>2200</isoType>\n		<number>1</number>\n		<weight>1000</weight>\n		<commodityDescription>MT</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n    </BookingItem>\n	</AllBookingItem>\n		</BookingHeader>\n	<BookingHeader>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>JLGBOOK41434B01</id>\n	<dischargePort>NZAKL</dischargePort>\n	<voyageCode>MSCK000002</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>D</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK41434B01</id>\n		<isoType>2200</isoType>\n		<number>1</number>\n		<weight>1000</weight>\n		<commodityDescription>GEN</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n");

            // Create booking
            CreateDataFile(EDIFile2,
                "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n		<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n  <BookingHeader Terminal='TT1'>\n	<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>JLGBOOK41434A01</id>\n	<dischargePort>NZAKL</dischargePort>\n	<voyageCode>MSCK000002</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>A</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK41434A01</id>\n		<isoType>2200</isoType>\n		<number>1</number>\n		<weight>1000</weight>\n		<commodityDescription>MT</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n");

            // Delete Cargo
            CreateDataFile(EDIFile3,
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>41434</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG41434A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Storage</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();
            
            //FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            EDIOperationsForm ediOperations = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperations.DeleteEDIMessages(EDIOperationsDataType.BookingReferenceMultiLine, @"41434", ediStatus: EDIOperationsStatusType.Loaded);
            ediOperations.LoadEDIMessageFromFile(EDIFile1);
            ediOperations.ChangeEDIStatus(EDIFile1, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);
            ediOperations.LoadEDIMessageFromFile(EDIFile2); 
            ediOperations.ChangeEDIStatus(EDIFile2, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);
            ediOperations.LoadEDIMessageFromFile(EDIFile3);
            ediOperations.ChangeEDIStatus(EDIFile3, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);
            ediOperations.CloseForm();
        }


        [TestMethod]
        public void EmptyContainerPoolManagement()
        {
            MTNInitialize();
            
            var now = DateTimeOffset.UtcNow;
            var unixTimeMilliseconds = now.ToUnixTimeMilliseconds();
            var uniqueID = $"41434{unixTimeMilliseconds}";

            // 1. Open road gate form and enter vehicle visit details
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"41434");

            
            roadGateForm.SetRegoCarrierGate("41434");
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK41434A01", 10);
            // 2. In road gate details form, press click and accept warning 
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Empty Container TT1");
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG41434A01");
            _roadGateDetailsReceiveForm.CmbImex.SetValue(IMEX.Storage, doDownArrow: true);
            _roadGateDetailsReceiveForm.BtnSave.DoClick();
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();
            roadGateForm.btnSave.DoClick();
            roadGateForm.CloseForm();

            /*FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^41434", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^41434", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");
            roadOperationsForm.CloseForm();*/
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new[] {"41434"});
  
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer,EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG41434A01");
            cargoEnquiryForm.DoSearch();

            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG41434A01");
            cargoEnquiryForm.tblData2.FindClickRow(new[] {@"ID^JLG41434A01"});
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            
            cargoEnquiryForm.DoEdit();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Mark", MarkDetails, doReverse: true);
            cargoEnquiryForm.DoSave();

            /*warningErrorForm = new WarningErrorForm(@"Warnings for Tracked Item Update TT1");
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Tracked Item Update");

            var fieldData = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Booking");
            Assert.IsTrue(fieldData == @"JLGBOOK41434A01", @"Field: Booking: " + fieldData + " doesn't equal: " + @"JLGBOOK41434A01");
            cargoEnquiryForm.CloseForm();
            

            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking", forceReset: true);
            FormObjectBase.MainForm.OpenBookingFromToolbar();
            _bookingForm = new BookingForm(@"Booking TT1");
            _bookingForm.DoNew();

             _bookingItemsForm = new BookingItemsForm(@"Adding Booking  TT1");
            _bookingItemsForm.txtReference.SetValue("JLGBOOK41434B01");
            _bookingItemsForm.cmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _bookingItemsForm.cmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true); //, 10);
            _bookingItemsForm.cmbDischargePort.SetValue(Port.AKLNZ, doDownArrow: true);
            _bookingItemsForm.cmbDestinationPort.SetValue(Port.ADOJP, doDownArrow: true);
            _bookingItemsForm.chkAdditionalRequirements.DoClick();
            _bookingItemsForm.showAdditionalRequirements();
            _bookingItemsForm.chkReleaseEmpties.DoClick();
            _bookingItemsForm.btnAdd.DoClick();

            _bookingItemFormCont = new BookingItemFormCont(@"Adding Booking Item for  TT1");
            _bookingItemFormCont.GetReleaseDetailsDetails();

            _bookingItemFormCont.cmbCargoType.SetValue(CargoType.ISOContainer, doDownArrow: true, searchSubStringTo: 6);
            _bookingItemFormCont.cmbISOType.SetValue(ISOType.ISO2200, doDownArrow: true);
            _bookingItemFormCont.cmbCommodity.SetValue(Commodity.GEN, doDownArrow: true);
            _bookingItemFormCont.txtBookingQuantity.SetValue(@"1");
            _bookingItemFormCont.mtBookingWeight.SetValueAndType("1000", "lbs");
            _bookingItemFormCont.txtMaxToRelease.SetValue(@"1");
            _bookingItemFormCont.btnOK.DoClick();
            _bookingItemsForm.SetFocusToForm();
            _bookingItemsForm.btnSave.DoClick();
            
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Release Requests", forceReset: true);
            FormObjectBase.MainForm.OpenReleaseRequestFromToolbar();
            _releaseRequestForm = new ReleaseRequestForm(formTitle: @"Release Requests TT1");
            _releaseRequestForm.cmbView.SetValue(@"Active");
            _releaseRequestForm.cmbType.SetValue(@"Road");
            MTNControlBase.SetValueInEditTable(_releaseRequestForm.tblSearchTable, @"Release No", @"JLGBOOK41434B01");

            _releaseRequestForm.DoSearch();

            string tableData1 = MTNControlBase.GetValueInEditTable(_releaseRequestForm.tblReleaseRequestDetails, @"Release No"); 
            Assert.IsTrue(tableData1 == "JLGBOOK41434B01", "Release request number is " + tableData1 + " and expected to be JLGBOOK41434B01");
            string tableData2 = MTNControlBase.GetValueInEditTable(_releaseRequestForm.tblReleaseRequestDetails, @"Booking"); 
            Assert.IsTrue(tableData2 == "JLGBOOK41434B01", "Booking number is " +  tableData2 + " and expected to be JLGBOOK41434B01");
            _releaseRequestForm.CloseForm();
           
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            //maybe need to change this was roadGate2
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1");
            roadGateForm.SetRegoCarrierGate("41434");
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK41434B01", 10);

            _roadGatePickerForm = new RoadGatePickerForm(formTitle: @"Picker");
            // MTNControlBase.FindClickRowInTable(_roadGatePickerForm.tblPickerItems, @"Type^Release~Item Id^JLGBOOK41434B01", rowHeight: 16);
            _roadGatePickerForm.TblPickerItems.FindClickRow(["Type^Release~Item Id^JLGBOOK41434B01"]);            _roadGatePickerForm.btnOK.DoClick();

            _roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm(formTitle: @"Release Empty Container  TT1");
            _roadGateDetailsReleaseForm.TxtCargoId.SetValue(@"JLG41434A01");
            _roadGateDetailsReleaseForm.TxtRemarks.SetValue(uniqueID);
            _roadGateDetailsReleaseForm.BtnSave.DoClick();

            /*warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out");
            
            roadGateForm.btnSave.DoClick();
            roadGateForm.CloseForm();

            /*FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);

            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^41434", ClickType.Click, rowHeight: 16);

            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^41434", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^41434", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");
            roadOperationsForm.CloseForm();*/
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new[] { "41434" });

            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG41434A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", Site.OffSite, 
                EditRowDataType.ComboBoxEdit,  fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Mark", MarkDetails);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"Booking^JLGBOOK41434A01~ID^JLG41434A01~Remarks^" + uniqueID);
            cargoEnquiryForm.tblData2.FindClickRow(new[]
                { $"Booking^JLGBOOK41434A01~ID^JLG41434A01~Remarks^{uniqueID}" });
        }

    }

}
