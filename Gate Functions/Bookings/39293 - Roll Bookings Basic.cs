using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNArguments.Classes;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Bookings
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39293 : MTNBase
    {

        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;
        BookingItemsForm _bookingFormItemsForm;
        BookingForm _bookingForm;
        RollBookingForm _rollBookingForm;
        ConfirmationFormOK _confirmationFormOK;
       
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void RollBookingsBasic()
        {
            MTNInitialize();
            
            //1. Go to road gate and enter truck information
           FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"39293");
            roadGateForm.SetRegoCarrierGate("39293");
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK39293A01");
            
            //2. Add 2 cargo IDs and complete road gate
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive Full Container TT1");
            _roadGateDetailsReceiveForm.AddMultipleCargoIds(@"JLG39293A01",@"2");
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("15000");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();
            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();
            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();

            //3. In the booking for check the 2 have been received
            FormObjectBase.MainForm.OpenBookingFromToolbar();
            _bookingForm = new BookingForm(@"Booking TT1");
            MTNControlBase.SetValueInEditTable(_bookingForm.tblSearcher, @"Booking", @"JLGBOOK39293");
            _bookingForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings, @"Booking^JLGBOOK39293~Received^2");
            _bookingForm.TblBookings.FindClickRow(["Booking^JLGBOOK39293~Received^2"]);            _bookingForm.DoEdit();

            //4. roll the booking to a different voyage
            _bookingFormItemsForm = new BookingItemsForm(@"Editing Booking JLGBOOK39293A01 TT1");
            _bookingFormItemsForm.btnRoll.DoClick();
            _rollBookingForm = new RollBookingForm(@"Roll Booking TT1");
            _rollBookingForm.cmbNewVoyage.SetValue(@"MESDAI200001 - Jolly Diamante 2 OFFICIAL");
            _rollBookingForm.txtNewBooking.SetValue(@"JLGBOOK39293A01A");
            _rollBookingForm.cmbDischargePort.SetValue(Port.NSNNZ, doDownArrow:true);
            _rollBookingForm.btnSave.DoClick();
            _bookingForm.CloseForm();

            //5. Check cargo enquiry/cargo transactions to ensure the booking has been rolled as expected
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG39293A");
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39293A01");
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG39293A01"]);            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, new [] {
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Voyage", FieldRowValue ="MESDAI200001" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Discharge Port", FieldRowValue ="NZNSN" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Booking", FieldRowValue ="JLGBOOK39293A01A" },
            });
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39293A01", ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG39293A01"], ClickType.ContextClick);            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG39293A01 TT1");
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Roll Booking for Cargo~Details^Booking Reference JLGBOOK39293A01 => JLGBOOK39293A01A Discharge Port NZLYT => NZNSN Voyage MSCK000002 = > MESDAI200001");
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(["Type^Roll Booking for Cargo~Details^Booking Reference JLGBOOK39293A01 => JLGBOOK39293A01A Discharge Port NZLYT => NZNSN Voyage MSCK000002 = > MESDAI200001"]);           
            cargoEnquiryForm.SetFocusToForm();
            
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG39293A02" });
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, new [] {
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Voyage", FieldRowValue ="MESDAI200001" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Discharge Port", FieldRowValue ="NZNSN" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Booking", FieldRowValue ="JLGBOOK39293A01A" },
            });
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG39293A02" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG39293A02 TT1");
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^Roll Booking for Cargo~Details^Booking Reference JLGBOOK39293A01 => JLGBOOK39293A01A Discharge Port NZLYT => NZNSN Voyage MSCK000002 = > MESDAI200001");
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(["Type^Roll Booking for Cargo~Details^Booking Reference JLGBOOK39293A01 => JLGBOOK39293A01A Discharge Port NZLYT => NZNSN Voyage MSCK000002 = > MESDAI200001"]);

            //6. Try to gate in using the old booking number
            roadGateForm.SetFocusToForm();
            roadGateForm.SetRegoCarrierGate("39293A");
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK39293A01", 10);
            _confirmationFormOK = new ConfirmationFormOK(@"Rolled Booking");
            _confirmationFormOK.CheckMessageMatch(@"This booking (JLGBOOK39293A01) has been rolled to booking JLGBOOK39293A01A.");
            _confirmationFormOK.btnOK.DoClick();

            //7. then gate in using the new booking number, adding 2 additional container
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK39293A01A", 10);
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive Full Container TT1");
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG39293A03");
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("12000");
            _roadGateDetailsReceiveForm.BtnSaveNext.DoClick();
            
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out");
            
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG39293A04");
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("13000");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out");
            
            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();

            //8. go to booking for and ensure all 4 containers have been received under the rolled booking
            FormObjectBase.MainForm.OpenBookingFromToolbar();
            _bookingForm = new BookingForm(@"Booking TT1");
            MTNControlBase.SetValueInEditTable(_bookingForm.tblSearcher, @"Booking", @"JLGBOOK39293");
            _bookingForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings, @"Booking^JLGBOOK39293~Received^4");
            _bookingForm.TblBookings.FindClickRow(["Booking^JLGBOOK39293~Received^4"]);            
            //9. Check cargo enquiry details for the 2 new containers
            cargoEnquiryForm.SetFocusToForm();
            
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG39293A");
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG39293A01", "ID^JLG39293A02", "ID^JLG39293A03", "ID^JLG39293A04" });
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, new [] {
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Voyage", FieldRowValue ="MESDAI200001" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Discharge Port", FieldRowValue ="NZNSN" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Booking", FieldRowValue ="JLGBOOK39293A01A" },
            });
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39293A03");
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG39293A03" });
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, new [] {
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Voyage", FieldRowValue ="MESDAI200001" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Discharge Port", FieldRowValue ="NZNSN" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Booking", FieldRowValue ="JLGBOOK39293A01A" },
            });

            //10. Complete vehicle visit/cargo via road ops
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new [] { "39293A" });
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new [] { "39293" });

        }



        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_39293_";

            // Delete Booking
            CreateDataFileToLoad(@"DeleteBooking.xml",
                "<?xml version='1.0'?>\n<JMTInternalBookingRef>\n    <AllBookingReferenceSingleLine>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <BookingReferenceSingleLine Terminal='TT1'>\n            <dischargePort>NZLYT</dischargePort>\n            <id>JLGBOOK39293A01A</id>\n            <messageMode>D</messageMode>\n            <operatorCode>MSL</operatorCode>\n            <vesselName>MESDAI2</vesselName>\n            <voyageCode>MESDAI200001</voyageCode>\n            <number>4</number>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <commodity>GEN</commodity>\n            <commodityDescription>General</commodityDescription>\n            <fullOrMT>F</fullOrMT>\n            <isoType>2200</isoType>\n        </BookingReferenceSingleLine>\n    </AllBookingReferenceSingleLine>\n</JMTInternalBookingRef>\n");

            // Delete Cargo Onsite
            CreateDataFileToLoad(@"DeleteCargo.xml",
                "<?xml version='1.0'?> <JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39293A01</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>NZNSN</dischargePort>\n      <voyageCode>MeSDAI200001</voyageCode>\n      <isoType>2200</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39293A02</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>NZNSN</dischargePort>\n      <voyageCode>MeSDAI200001</voyageCode>\n      <isoType>2200</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39293A03</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>NZNSN</dischargePort>\n      <voyageCode>MeSDAI200001</voyageCode>\n      <isoType>2200</isoType>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	    <messageMode>D</messageMode>\n      <id>JLG39293A04</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>NZNSN</dischargePort>\n      <voyageCode>MeSDAI200001</voyageCode>\n      <isoType>2200</isoType>\n    </CargoOnSite>\n	\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Add Booking
            CreateDataFileToLoad(@"CreateBooking.xml",
                "<?xml version='1.0'?>\n<JMTInternalBookingRef>\n    <AllBookingReferenceSingleLine>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <BookingReferenceSingleLine Terminal='TT1'>\n            <dischargePort>NZLYT</dischargePort>\n            <id>JLGBOOK39293A01</id>\n            <messageMode>A</messageMode>\n            <operatorCode>MSL</operatorCode>\n            <vesselName>MSCK</vesselName>\n            <voyageCode>MSCK000002</voyageCode>\n            <number>4</number>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <commodity>GEN</commodity>\n            <commodityDescription>General</commodityDescription>\n            <fullOrMT>F</fullOrMT>\n            <isoType>2200</isoType>\n            <messageMode>A</messageMode>\n        </BookingReferenceSingleLine>\n    </AllBookingReferenceSingleLine>\n</JMTInternalBookingRef>\n");


            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
