using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45109 : MTNBase
    {

        private TerminalConfigForm _terminalConfigForm;
       
        private const string TestCaseNumber = @"45109";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
        }
        
        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            MTNSignon(TestContext);

            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            _terminalConfigForm = new TerminalConfigForm();
            _terminalConfigForm.SetTerminalConfiguration(@"EDI",
                @"Use EDI Booking Release Req rather than EDI Gate Document", @"0",
                rowDataType: EditRowDataType.CheckBox);
            _terminalConfigForm.CloseForm();
        }


        [TestMethod]
        public void COPARNWithVGMNoWeightCertifyingPersonAuthority()
        {
            MTNInitialize();

            // STep 6 - 10
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);

            // Enter Registration, Carrier, Gate, New Item and press Tab key
            roadGateForm = new RoadGateForm(@"Road Gate TT1", vehicleId: TestCaseNumber);
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);
            roadGateForm.txtNewItem.SetValue(CargoId);

            RoadGateDetailsReceiveForm receiveFullContainerForm = new RoadGateDetailsReceiveForm(@"Receive Full Reefer TT1");
            receiveFullContainerForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();

            // Step 11 - 13
            ValidateWeightCertifyingAuthorityPerson();

            // Step 14 - 15
            SetupAndLoadInitializeData2(TestContext);

            // Step 16 - 18
            ValidateWeightCertifyingAuthorityPerson();

            // Step 19 - 21
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(@"Road Operations TT1", new[] {TestCaseNumber});

        }

        private void ValidateWeightCertifyingAuthorityPerson()
        {
            if (cargoEnquiryForm == null)
            {
                FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
                cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            }
            else
            {
                cargoEnquiryForm.SetFocusToForm();
            }

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Weight Certifying Authority", @"TEST TEAM");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Weight Certifying Person", @"TESTER");
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>45109</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45109A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n     <commodity>GEN</commodity>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n\n	\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n");

            // Delete Booking
            CreateDataFileToLoad(@"DeleteBooking.xml",
                "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n  <BookingHeader Terminal='TT1'>\n	<cargoTypeDescr>ISO Container</cargoTypeDescr>\n	<id>JLGBOOK45109A01</id>\n	<dischargePort>NZBLU</dischargePort>\n	<voyageCode>MSCK000002</voyageCode>\n	<operatorCode>ONEY</operatorCode>\n	<messageMode>D</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK45109A01</id>\n		<isoType>2230</isoType>\n		<number>1</number>\n		<weight>1000</weight>\n		<commodityDescription>REEF</commodityDescription>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n");

            // Create Booking
            CreateDataFileToLoad(@"CreateBooking.xml",
                "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n  <BookingHeader Terminal='TT1'>\n	<cargoTypeDescr>ISO Container</cargoTypeDescr>\n	<id>JLGBOOK45109A01</id>\n	<dischargePort>NZBLU</dischargePort>\n	<voyageCode>MSCK000002</voyageCode>\n	<operatorCode>ONEY</operatorCode>\n	<messageMode>A</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK45109A01</id>\n		<isoType>2230</isoType>\n		<number>1</number>\n		<weight>1000</weight>\n		<commodityDescription>REEF</commodityDescription>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<temperature>12</temperature>\n		\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n");

            // Create Prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0'?>\n	<JMTInternalPrenote>\n		<AllPrenote>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n			<Prenote Terminal='TT1'>\n				<cargoTypeDescr>ISO Container</cargoTypeDescr>\n				<dischargePort>NZBLU</dischargePort>\n				<id>JLG45109A01</id>\n				<imexStatus>Export</imexStatus>\n				<isoType>2230</isoType>\n				<messageMode>A</messageMode>\n				<operatorCode>ONEY</operatorCode>\n				<weight>6000</weight>\n				<isWeightCertified>Yes</isWeightCertified>\n				<weightCertifiedName>TESTER</weightCertifiedName>\n				<weightCertifiedReference>TEST TEAM</weightCertifiedReference>\n				<voyageCode>MSCK000002</voyageCode>\n				<bookingRef>JLGBOOK45109A01</bookingRef>\n			<commodityDescription>REEF</commodityDescription>\n			<temperature>12</temperature>\n			</Prenote>\n		</AllPrenote>\n	</JMTInternalPrenote>\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        private static void SetupAndLoadInitializeData2(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber + "A_";

            // Update Coparn
            CreateDataFileToLoad(@"UpdateCoparn.xml",
                "UNA:+.'\nUNB+UNOA:2+ONEY+ITSCT+190531:1510+9952'\nUNH+8636+COPARN:D:95B:UN:ITG12'\nBGM+11+BKG190531241596+5'\nRFF+ACW:JLGBOOK45109A01'\nTDT+20+MSCK000002+1++ONE:172:20+++V7UO5:103::MSC KATYA R.'\nRFF+VON:MSCK000002'\nDTM+132:201906020500:203'\nDTM+133:201906030700:203'\nNAD+MS+ONE:160:20'\nNAD+CF+ONE:160:20'\nEQD+CN+JLG45109A01+2230:102:5++2+5'\nRFF+BN:JLGBOOK45109A01:N52170'\nEQN+1'\nTMD+++2'\nLOC+9+ITSAL:139:6:SALERNO+VTE:TER:ZZZ'\nLOC+11+NZBLU:139:6:DAMIETTA  EGYPT'\nLOC+8+ITSAL:139:6+VTE:TER:ZZZ'\nLOC+163+KRPUS:139:6+PNC:TER:ZZZ'\nMEA+AAE+VGM+KGM:28040'\nTMP+2+015:CEL'\nSEL+062013'\nFTX+AAA+++Reefer'\nFTX+ABS++SM1:ZZZ:SMD'\nTDT+10++3'\nCNT+16:1'\nUNT+25+8636'\nUNZ+1+9952'");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }

}
