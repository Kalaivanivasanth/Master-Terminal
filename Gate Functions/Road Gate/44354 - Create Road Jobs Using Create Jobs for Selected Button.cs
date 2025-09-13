using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44354 : MTNBase
    {

        RoadGatePickerForm _roadGatePickerForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            searchFor = @"_44354_";
            
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void CreateJobsForSelected()
        {
            MTNInitialize();
            
           // 1. Open road gate form and enter vehicle visit details
           FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
           roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"44354");
           //MTNControlBase.SetValue(roadGateForm.txtRegistration, @"44354");
            //roadGateForm.txtRegistration.SetValue(@"44354");
            //MTNControlBase.SetValue(roadGateForm.cmbCarrier, @"American Auto Tpt");
            //roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            //MTNControlBase.SetValue(roadGateForm.cmbGate, @"GATE");
            //roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.SetRegoCarrierGate("44354");
            //MTNControlBase.SetValue(roadGateForm.txtNewItem, @"JLGBOOK44354");
            roadGateForm.txtNewItem.SetValue(@"JLGBOOK44354", 10);

            //Keyboard.Press(VirtualKeyShort.TAB);

            //2. from road gate picket select all and create jobs for all selected
            _roadGatePickerForm = new RoadGatePickerForm("Picker");
            _roadGatePickerForm.btnSelectAll.DoClick();
            _roadGatePickerForm.btnCreateJobsForSelected.DoClick();
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            string[] warningErrorToCheck = new string[]
             {
               "Code :75016. The Container Id (JLG44354A01) failed the validity checks and may be incorrect.",
               "Code :75016. The Container Id (JLG44354A02) failed the validity checks and may be incorrect."
             };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();
            _roadGatePickerForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, "Type^Receive Full~Detail^JLG44354A01; MSC; 2200~Booking/Release^JLGBOOK44354", rowHeight: 16);
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, "Type^Receive Full~Detail^JLG44354A02; MSC; 2200~Booking/Release^JLGBOOK44354", rowHeight: 16);
            roadGateForm.TblGateItems.FindClickRow([
                "Type^Receive Full~Detail^JLG44354A01; MSC; 2200~Booking/Release^JLGBOOK44354",
                "Type^Receive Full~Detail^JLG44354A02; MSC; 2200~Booking/Release^JLGBOOK44354"
            ]);
            roadGateForm.btnSave.DoClick();

            //4. Go to road operations, move cargo onsite and gate out
            /*FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^44354~Cargo ID^JLG44354A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^44354~Cargo ID^JLG44354A02", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^44354~Cargo ID^JLG44354A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");*/
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new[] { "44354" });

        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            //fileOrder = 1;

             //Cargo on Site Delete
            CreateDataFileToLoad(@"DeleteCargo.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		  <TestCases>44354</TestCases>\n		  <cargoTypeDescr>ISO Container</cargoTypeDescr>\n		  <id>JLG44354A01</id>\n		  <isoType>2200</isoType>\n		  <operatorCode>MSC</operatorCode>\n		  <locationId>MKBS01</locationId>\n		  <commodity>APPL</commodity>\n		  <dischargePort>NZBLU</dischargePort>\n		  <voyageCode>MSCK000002</voyageCode>\n		  <messageMode>D</messageMode>\n    </CargoOnSite>\n	<CargoOnSite Terminal='TT1'>\n		  <TestCases>44354</TestCases>\n		  <cargoTypeDescr>ISO Container</cargoTypeDescr>\n		  <id>JLG44354A02</id>\n		  <isoType>2200</isoType>\n		  <operatorCode>MSC</operatorCode>\n		  <locationId>MKBS01</locationId>\n		  <commodity>APPL</commodity>\n		  <dischargePort>NZBLU</dischargePort>\n		  <voyageCode>MSCK000002</voyageCode>\n		  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");


            //Delete Bookling
            CreateDataFileToLoad(@"DeleteBooking.xml",
               "<?xml version='1.0'?>\n<JMTInternalBooking>\n	<AllBookingHeader>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n	<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<BookingHeader Terminal='TT1'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZBLU</dischargePort>\n			<id>JLGBOOK44354</id>\n			<messageMode>D</messageMode>\n			<operatorCode>MSC</operatorCode>\n			<voyageCode>MSCK000002</voyageCode>\n				<AllBookingItem>\n					<BookingItem>\n						<weight>18830.0000</weight>\n						<number>2</number>\n						<cargoTypeDescr>ISO Container</cargoTypeDescr>\n						<commodity>APPL</commodity>\n						<fullOrMT>F</fullOrMT>\n						<id>JLGBOOK44354</id>\n						<isoType>2200</isoType>\n						<messageMode>D</messageMode>\n					</BookingItem>\n				</AllBookingItem>\n			</BookingHeader>\n	</AllBookingHeader>\n</JMTInternalBooking>\n");


            // Add Booking
            CreateDataFileToLoad(@"CreateBooking.xml",
                "<?xml version='1.0'?>\n<JMTInternalBooking>\n	<AllBookingHeader>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n	<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<BookingHeader Terminal='TT1'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZBLU</dischargePort>\n			<id>JLGBOOK44354</id>\n			<messageMode>A</messageMode>\n			<operatorCode>MSC</operatorCode>\n			<voyageCode>MSCK000002</voyageCode>\n				<AllBookingItem>\n					<BookingItem>\n						<weight>18830.0000</weight>\n						<number>2</number>\n						<cargoTypeDescr>ISO Container</cargoTypeDescr>\n						<commodity>APPL</commodity>\n						<fullOrMT>F</fullOrMT>\n						<id>JLGBOOK44354</id>\n						<isoType>2200</isoType>\n						<messageMode>A</messageMode>\n					</BookingItem>\n				</AllBookingItem>\n			</BookingHeader>\n	</AllBookingHeader>\n</JMTInternalBooking>\n");

            //Add Prenote
            CreateDataFileToLoad(@"AddPrenote.xml",
               "<?xml version='1.0'?>\n	<JMTInternalPrenote>\n		<AllPrenote>\n			<operationsToPerform>Verify;Load To DB</operationsToPerform>\n    		<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n			<Prenote Terminal='TT1'>\n				<cargoTypeDescr>ISO Container</cargoTypeDescr>\n				<dischargePort>NZBLU</dischargePort>\n				<id>JLG44354A01</id>\n				<imexStatus>Export</imexStatus>\n				<isoType>2200</isoType>\n				<operatorCode>MSC</operatorCode>\n				<weight>6000</weight>\n				<voyageCode>MSCK000002</voyageCode>\n				<commodity>APPL</commodity>\n				<bookingRef>JLGBOOK44354</bookingRef>\n				<messageMode>A</messageMode>\n			</Prenote>\n			<Prenote Terminal='TT1'>\n				<cargoTypeDescr>ISO Container</cargoTypeDescr>\n				<dischargePort>NZBLU</dischargePort>\n				<id>JLG44354A02</id>\n				<imexStatus>Export</imexStatus>\n				<isoType>2200</isoType>\n				<operatorCode>MSC</operatorCode>\n				<weight>6000</weight>\n				<voyageCode>MSCK000002</voyageCode>\n				<commodity>APPL</commodity>\n				<bookingRef>JLGBOOK44354</bookingRef>\n				<messageMode>A</messageMode>\n			</Prenote>\n		</AllPrenote>\n	</JMTInternalPrenote>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
