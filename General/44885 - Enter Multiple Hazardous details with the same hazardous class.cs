using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44885 : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_44885_";
            BaseClassInitialize_New(testContext);
        }
        
        [TestInitialize]
        public new void TestInitialize() {}


        [TestCleanup]
        public new void TestCleanup()
        {
            // navmh5 27/10/2023 - Removed as in the resetTerminalConfigs script
            //Reset Configuration
            /*FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Settings",
                @"Hazardous - Use Detailed Hazardous Entry Screen", @"1",
                rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();*/

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();

            //Set Configuration
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Settings",
                @"Hazardous - Use Detailed Hazardous Entry Screen", @"0",
                rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();
        }


        [TestMethod]
        public void EnterMultipleHazardousDetailsWiththesameHazardousClass()
        {
            MTNInitialize();
            
            //Step 4
            /*FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.BagOfSand, EditRowDataType.ComboBoxEdit, formCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG44885A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, formCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44885A01", rowHeight: 18);

            //Step 5
            //cargoEnquiryForm.btnEdit.DoClick();
            cargoEnquiryForm.DoEdit();
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //cargoEnquiryForm.tabGeneral.Focus();

            //cargoEnquiryForm.GetGenericTabTableDetails(@"General", @"4042");
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            //Step 6
            MTNControlBase.FindClickRowInTableVHeader(cargoEnquiryForm.tblGeneralEdit, @"Hazard Details",
                clickType: ClickType.DoubleClick, tableRowHeight: 20);

            //Step 7
            HazardousDetailsForm hazardousDetailsForm = new HazardousDetailsForm(@"Hazardous Details TT1");
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbHazardClass, @"8");
            hazardousDetailsForm.cmbHazardClass.SetValue("8	CORROSIVE", doDownArrow: true);
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.txtUNDGCodes, @"3264");
            hazardousDetailsForm.txtUNDGCodes.SetValue(@"3264");
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbPackingGroup, @"III");
            hazardousDetailsForm.cmbPackingGroup.SetValue(@"III");
            hazardousDetailsForm.btnAdd.DoClick();

            //Step 8
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbHazardClass, @"8");
            hazardousDetailsForm.cmbHazardClass.SetValue("8	CORROSIVE", doDownArrow: true);
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.txtUNDGCodes, @"1823");
            hazardousDetailsForm.txtUNDGCodes.SetValue(@"1823");
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbPackingGroup, @"III");
            hazardousDetailsForm.cmbPackingGroup.SetValue(@"III");
            hazardousDetailsForm.btnAdd.DoClick();

            //Step 9
            MTNControlBase.FindClickRowInTable(hazardousDetailsForm.tblHazardDetails,
            @"Hazard Class^8~UN Number^3264~Packing Group^III");
            MTNControlBase.FindClickRowInTable(hazardousDetailsForm.tblHazardDetails,
             @"Hazard Class^8~UN Number^1823~Packing Group^III");

            hazardousDetailsForm.btnOK.DoClick();

            cargoEnquiryForm.SetFocusToForm();
            //cargoEnquiryForm.btnSave.DoClick();
            cargoEnquiryForm.DoSave();
            //cargoEnquiryForm.tabGeneral.Focus();
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            //Step 10
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Is Hazardous", @"Yes");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Hazard Details",
                @"8 (3264) III; 8 (1823) III");

            //Step 11
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking", forceReset: true);
            BookingForm bookingForm = new BookingForm(@"Booking TT1");

            //bookingForm.btnAdd.DoClick();
            bookingForm.DoNew();

            //Step 12
            BookingItemsForm bookingItemsForm = new BookingItemsForm();
            //bookingItemsForm.SetValue(bookingItemsForm.txtReference, @"JLGBOOK44885A01");
            bookingItemsForm.txtReference.SetValue(@"JLGBOOK44885A01");
            //bookingItemsForm.SetValue(bookingItemsForm.cmbOperator, @"MSC");
            bookingItemsForm.cmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            //bookingItemsForm.SetValue(bookingItemsForm.cmbVoyage, @"MSCK000002");
            bookingItemsForm.cmbVoyage.SetValue(Voyage.MSCK000002, doDownArrow: true);
            //bookingItemsForm.SetValue(bookingItemsForm.cmbDischargePort, @"AKL (NZ) Auckland");
            bookingItemsForm.cmbDischargePort.SetValue(DischargePort.AKLNZ, doDownArrow: true);
            bookingItemsForm.btnHazardDetails.DoClick();

            //Step 13
            hazardousDetailsForm = new HazardousDetailsForm(@"Hazardous Details TT1");
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbHazardClass, @"8");
            hazardousDetailsForm.cmbHazardClass.SetValue("8	CORROSIVE", doDownArrow: true);
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.txtUNDGCodes, @"3264");
            hazardousDetailsForm.txtUNDGCodes.SetValue(@"3264");
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbPackingGroup, @"III");
            hazardousDetailsForm.cmbPackingGroup.SetValue(@"III");
            hazardousDetailsForm.btnAdd.DoClick();

            //Step 14
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbHazardClass, @"8");
            hazardousDetailsForm.cmbHazardClass.SetValue("8	CORROSIVE", doDownArrow: true);
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.txtUNDGCodes, @"1823");
            hazardousDetailsForm.txtUNDGCodes.SetValue(@"1823");
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbPackingGroup, @"III");
            hazardousDetailsForm.cmbPackingGroup.SetValue(@"III");
            hazardousDetailsForm.btnAdd.DoClick();

            //Step 15
            MTNControlBase.FindClickRowInTable(hazardousDetailsForm.tblHazardDetails,
            @"Hazard Class^8~UN Number^3264~Packing Group^III");
            MTNControlBase.FindClickRowInTable(hazardousDetailsForm.tblHazardDetails,
             @"Hazard Class^8~UN Number^1823~Packing Group^III");

            hazardousDetailsForm.btnOK.DoClick();

            bookingItemsForm.btnAdd.DoClick();

            // Step 16
            BookingItemForm bookingItemForm = new BookingItemForm(@"Adding Booking Item for  TT1");
            bookingItemForm.GetISOContainerDetails();
            //bookingItemForm.SetValue(bookingItemForm.cmbCargoType, @"ISO Container");
            bookingItemForm.cmbCargoType.SetValue(CargoType.ISOContainer, doDownArrow: true, searchSubStringTo: 10);
            //bookingItemForm.SetValue(bookingItemForm.cmbISOType, @"2200");
            bookingItemForm.cmbISOType.SetValue(ISOTypes.ISO2200, doDownArrow: true);
            //bookingItemForm.SetValue(bookingItemForm.cmbCommodity, @"GEN");
            bookingItemForm.cmbCommodity.SetValue(Commodity.GEN, doDownArrow: true);
            //bookingItemForm.SetValue(bookingItemForm.txtBookingQuantity, @"2");
            bookingItemForm.txtBookingQuantity.SetValue(@"2");
            //MTNControlBase.SetWeightValues(bookingItemForm.txtBookingWeight, @"1000", @"lbs");
            bookingItemForm.mtBookingWeight.SetValueAndType("1000", "lbs");
            bookingItemForm.btnOK.DoClick();

            // Step 17
            bookingItemsForm.SetFocusToForm();
            bookingItemsForm.btnSave.DoClick();*/

            //Step 18
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Pre-Notes", forceReset: true);
            PreNoteForm preNoteForm = new PreNoteForm(formTitle: @"Pre-Notes TT1");

            //Step 19
            //preNoteForm.btnAdd.DoClick();
            preNoteForm.DoNew();
            RoadGateDetailsReceiveForm preNoteDetailsForm = new RoadGateDetailsReceiveForm(formTitle: @"PreNote Full Container TT1");
            //preNoteDetailsForm.ShowContainerDetails();
           // preNoteDetailsForm.GetCargoDetails();

            //preNoteDetailsForm.SetValue(preNoteDetailsForm.cmbIsoType, @"2200");
            preNoteDetailsForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            //preNoteDetailsForm.SetValue(preNoteDetailsForm.txtCargoId, @"JLG44885A01");
            preNoteDetailsForm.TxtCargoId.SetValue(@"JLG44885A01");
            //preNoteDetailsForm.SetValue(preNoteDetailsForm.cmbCommodity, @"GEN");
            preNoteDetailsForm.CmbCommodity.SetValue(Commodity.GEN, doDownArrow: true);
            //preNoteDetailsForm.SetValue(preNoteDetailsForm.txtTotalWeight, @"1000");
            preNoteDetailsForm.MtTotalWeight.SetValueAndType("1000");


            //preNoteDetailsForm.SetValue(preNoteDetailsForm.txtImex, @"E", additionalWaitTimeout: 2000);
            preNoteDetailsForm.CmbImex.SetValue(IMEX.Export, additionalWaitTimeout: 2000, doDownArrow: true);
            //preNoteDetailsForm.SetValue(preNoteDetailsForm.cmbVoyage, @"MSCK000002");
            preNoteDetailsForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            //preNoteDetailsForm.SetValue(preNoteDetailsForm.cmbOperator, @"MSC");
            preNoteDetailsForm.CmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            //preNoteDetailsForm.SetValue(preNoteDetailsForm.cmbDischargePort, @"AKL (NZ)");
            preNoteDetailsForm.CmbDischargePort.SetValue(Port.LYTNZ, doDownArrow: true);
            //FormObjectBase.ClickButton(preNoteDetailsForm.btnHazardDetails);
            preNoteDetailsForm.BtnHazardDetails.DoClick();

            //Step 20
            HazardousDetailsForm hazardousDetailsForm = new HazardousDetailsForm(@"Hazardous Details TT1");
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbHazardClass, @"8");
            hazardousDetailsForm.cmbHazardClass.SetValue("8	CORROSIVE", doDownArrow: true);
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.txtUNDGCodes, @"3264");
            hazardousDetailsForm.txtUNDGCodes.SetValue(@"3264");
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbPackingGroup, @"III");
            hazardousDetailsForm.cmbPackingGroup.SetValue(@"III");
            hazardousDetailsForm.btnAdd.DoClick();

            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbHazardClass, @"8");
            hazardousDetailsForm.cmbHazardClass.SetValue("8	CORROSIVE", doDownArrow: true);
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.txtUNDGCodes, @"1823");
            hazardousDetailsForm.txtUNDGCodes.SetValue(@"1823");
            //hazardousDetailsForm.SetValue(hazardousDetailsForm.cmbPackingGroup, @"III");
            hazardousDetailsForm.cmbPackingGroup.SetValue(@"III");
            hazardousDetailsForm.btnAdd.DoClick();
            // MTNControlBase.FindClickRowInTable(hazardousDetailsForm.tblHazardDetails,
            // @"Hazard Class^8~UN Number^3264~Packing Group^III");
            // MTNControlBase.FindClickRowInTable(hazardousDetailsForm.tblHazardDetails,
             // @"Hazard Class^8~UN Number^1823~Packing Group^III");
            hazardousDetailsForm.TblHazardDetails.FindClickRow([
                "Hazard Class^8~UN Number^3264~Packing Group^III",
                "Hazard Class^8~UN Number^1823~Packing Group^III"
            ]);
            hazardousDetailsForm.btnOK.DoClick();

            //FormObjectBase.ClickButton(preNoteDetailsForm.btnSave);
            preNoteDetailsForm.BtnSave.DoClick();

            //Step 22
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Pre-Notes TT1");
            string[] warningErrorToCheckPreNote = new string[]
             {
               "Code :75016. The Container Id (JLG44885A01) failed the validity checks and may be incorrect.",
               //"Code :75780. Booking is unknown or invalid.",
               "Code :83114. No PIN will be generated for hazardous or over dimension cargo item JLG44885A01."
             };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheckPreNote);
            warningErrorForm.btnSave.DoClick();


        }



        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44885</TestCases>\n      <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n	  <product>SMSAND</product>\n      <id>JLG44885A01</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>GCARA1 SMALL_SAND</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Export</imexStatus>\n      <dischargePort>NZNPE</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	       <totalQuantity>100</totalQuantity>\n      <commodity>SANC</commodity>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n	   </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                  "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44885</TestCases>\n      <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n	  <product>SMSAND</product>\n      <id>JLG44885A01</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>GCARA1 SMALL_SAND</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Export</imexStatus>\n      <dischargePort>NZNPE</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	       <totalQuantity>100</totalQuantity>\n      <commodity>SANC</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");


            // Delete Booking
            CreateDataFileToLoad(@"DeleteBooking.xml",
                "<?xml version='1.0'?> \n<JMTInternalBooking>\n<AllBookingHeader>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n	<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n  <BookingHeader Terminal='TT1'>\n	<cargoTypeDescr>CONT</cargoTypeDescr>\n	<id>JLGBOOK44885A01</id>\n	<dischargePort>NZAKL</dischargePort>\n	<voyageCode>MSCK000002</voyageCode>\n	<operatorCode>MSC</operatorCode>\n	<messageMode>D</messageMode>	\n	<AllBookingItem>\n    <BookingItem>\n		<id>JLGBOOK44885A01</id>\n		<isoType>2200</isoType>\n		<number>1</number>\n		<weight>1000</weight>\n		<commodityDescription>GEN</commodityDescription>\n		<cargoTypeDescr>CONT</cargoTypeDescr>\n		<messageMode>D</messageMode>	\n    </BookingItem>\n	</AllBookingItem>\n	</BookingHeader>\n  </AllBookingHeader>        \n</JMTInternalBooking>\n\n\n");

            // Create prenote
            CreateDataFileToLoad(@"DeletePrenote.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <Prenote Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n     <isoType>2200</isoType>\n       <id>JLG44885A01</id>\n             <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>6000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZBLU</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>D</messageMode>\n        </Prenote>\n    </AllPrenote>\n</SystemXMLPrenote>\n\n");





            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
