using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Terminal_Functions
{
    // [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44415 : MTNBase
    {
        
        protected static string ediFile1 = "M_44415_CargoOnSiteDelete.xml";
        protected static string ediFile2 = "M_44415_CargoOnSiteAdd.xml";
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup()
        {
   
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Road Gate Cargo Check Option", @"None", rowDataType: EditRowDataType.ComboBox);
            terminalConfigForm.CloseForm();

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //delete cargo on site
            CreateDataFile(ediFile1,
              "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44415</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>pipe</product>\n      <id>JLG44415A01</id>\n	  <operatorCode>CSA</operatorCode>\n	  <totalQuantity>20</totalQuantity>\n      <locationId>MKBS01</locationId>\n      <weight>50000.0000</weight>\n      <commodity>GENL</commodity>\n	  <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n	</AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            CreateDataFile(ediFile2,
             "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44415</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>pipe</product>\n      <id>JLG44415A01</id>\n	  <operatorCode>CSA</operatorCode>\n	  <totalQuantity>20</totalQuantity>\n      <locationId>MKBS01</locationId>\n      <weight>50000.0000</weight>\n      <commodity>GENL</commodity>\n	  <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            LogInto<MTNLogInOutBO>();

            //1 Go to Storage configuration and set IsReefer to yes - setting all just in case.
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Gate",@"Road Gate Cargo Check Option",@"Check Cargo Option 1", rowDataType: EditRowDataType.ComboBox, doReverse: true);
            //terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Road Gate Cargo Check Criteria", @"", rowDataType: EditRowDataType.DoubleClick, doReverse: true);
            terminalConfigForm.CloseForm();

            // Load the cargo on site
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            EDIOperationsForm ediOperations = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperations.DeleteEDIMessages(@"Cargo On Site", @"44415", ediStatus: @"Loaded");
            ediOperations.LoadEDIMessageFromFile(ediFile1);
            ediOperations.ChangeEDIStatus(ediFile1, @"Loaded", @"Load To DB");
            ediOperations.LoadEDIMessageFromFile(ediFile2);
            ediOperations.ChangeEDIStatus(ediFile2, @"Loaded", @"Load To DB");
            ediOperations.CloseForm();
        }


        [TestMethod]
        public void CargoAvailabilityCheck()
        {
            MTNInitialize();
 
            //1. Goto cargo enquiry and re-evaluate free storage days on cargo JLG44415A01
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Steel, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG44415A01");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            //cargoEnquiryForm.GetGenericTabTableDetails(@"Status", @"4093");
            cargoEnquiryForm.GetStatusTable(@"4093");
            //cargoEnquiryForm.CargoEnquiryStatusTab();

            MTNControlBase.FindClickRowInTableVHeader(cargoEnquiryForm.tblStatus, @"Stops (dbl click)", clickType: ClickType.DoubleClick);

            StopsForm stopsForm = new StopsForm();
            // MTNControlBase.FindClickRowInTable(stopsForm.tblStops,@"Stop^Delivery Order",xOffset: 130);
            stopsForm.TblStops.FindClickRow(["Stop^Delivery Order"], xOffset: 130);
            stopsForm.btnSaveAndClose.DoClick();
            
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            roadGateForm = new RoadGateForm(vehicleId: @"44415");

            //MTNControlBase.SetValue(roadGateForm.txtRegistration, @"44415");
            roadGateForm.txtRegistration.SetValue(@"44415");
            //MTNControlBase.SetValue(roadGateForm.cmbCarrier, @"American Auto Tpt");
            roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            roadGateForm.btnReleaseCargo.DoClick();

        }



    }

}
