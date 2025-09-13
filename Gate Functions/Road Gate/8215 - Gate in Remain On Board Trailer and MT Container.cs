using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase8215 : MTNBase
    {
        private GateConfigurationForm _gateConfigurationForm;
        private RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;

        const string EDIFile1 = "M_8215_DeleteCargo.xml";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup()
        {

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Gate Configuration", forceReset: true);
            FormObjectBase.MainForm.OpenGateConfigurationFromToolbar();
            _gateConfigurationForm = new GateConfigurationForm(@"Gate Configuration " + terminalId);
            _gateConfigurationForm.cmbTrailerDefaultImex.SetValue(@"Storage");
            _gateConfigurationForm.btnSave.DoClick();
            base.TestCleanup();
        }
        
        void MTNInitialize()
        {
            searchFor ="_8215";
            
            CreateDataFile(EDIFile1,
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG8215A002</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>2722</weight>\n         <imexStatus>Import</imexStatus>\n         <commodity>MT</commodity>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000002</voyageCode>\n         <isoType>2200</isoType>\n		 <messageMode>D</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n");

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>("TT1GATEUSR");

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Gate Configuration");
            FormObjectBase.MainForm.OpenGateConfigurationFromToolbar();
            GateConfigurationForm gateConfigurationForm = new GateConfigurationForm($@"Gate Configuration {terminalId}");
            gateConfigurationForm.cmbTrailerDefaultImex.SetValue(@"Remain On Board");
            gateConfigurationForm.btnSave.DoClick();

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            EDIOperationsForm ediOperations = new EDIOperationsForm($@"EDI Operations {terminalId}");

            ediOperations.DeleteEDIMessages(EDIOperationsDataType.CargoOnSite, @"8215", ediStatus: EDIOperationsStatusType.Loaded);

            ediOperations.LoadEDIMessageFromFile(EDIFile1);
            ediOperations.ChangeEDIStatus(EDIFile1, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);

            ediOperations.CloseForm();
        }



        [TestMethod]
        public void GateInRemainOnBoard()
        {
            MTNInitialize();
            
            // 1. Open road gate form and enter vehicle visit details
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: $"Road Gate {terminalId}");
            roadGateForm.SetRegoCarrierGate("8215");
            roadGateForm.GetTrailerFields();
            roadGateForm.txtTrailerID.SetValue(@"JLG8215A001");
            roadGateForm.cmbTrailerType.SetValue(@"TRAILER_MK3001");
            roadGateForm.cmbTrailerOperator.SetValue("MSL	Messina Line", doDownArrow: true);
            roadGateForm.mtTareWeight.SetValueAndType("1000");
            roadGateForm.btnReceiveEmpty.DoClick();


            // 2. add details In road gate details form
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: $"Receive Empty Container {terminalId}");
            _roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG8215A002");
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("6000");
            _roadGateDetailsReceiveForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.MSL,  doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbDischargePort.SetValue(Port.LYTNZ, doDownArrow: true);
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            /*// Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date 
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
             warningErrorForm.btnSave.DoClick();*/
             WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out TT1");

            
            // 3. in roadgate get the visit number for later and save
            roadGateForm.SetFocusToForm();
            /*// Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Chassis~Detail^JLG8215A001; MSL; CT_MK3A004", rowHeight: 16);
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Chassis~Detail^JLG8215A001; MSL; CT_MK3A004", rowHeight: 16);
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Empty~Detail^JLG8215A002; MSL; 2200", rowHeight: 16);*/
            roadGateForm.TblGateItems.FindClickRow(new[]
            {
                "Type^Receive Chassis~Detail^JLG8215A001; MSL; CT_MK3A004",
                "Type^Release Chassis~Detail^JLG8215A001; MSL; CT_MK3A004",
                "Type^Receive Empty~Detail^JLG8215A002; MSL; 2200"
            });
            roadGateForm.btnSave.DoClick();

            // 4. in road ops,
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new[] { "8215" });
        }

    }

}
