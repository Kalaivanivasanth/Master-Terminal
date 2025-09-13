using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
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
    public class TestCase41060: MTNBase
    {
        TerminalConfigForm terminalConfigForm;
        RoadGateDetailsReceiveForm roadGateDetailsReceiveForm;

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
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void ValidatingForceUserEntryofAmountAtGateIn()
        {
            MTNInitialize();
            
            // Step 4 Open Terminal Ops | Terminal Config
            // Step 5 Click the Gate tab 
            // Step 6 Click the edit button in the toolbar and tick the "Force User entry of 'Amount' at Gate In for Bulk/Break-Bulk" check box 
            // Step 7 Click the Save button in the toolbar 
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Force User entry of 'Amount' at Gate In for Bulk/Break-Bulk", @"1", rowDataType: EditRowDataType.CheckBox);


            // Step 8 Open Gate Functions | Road Gate
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);


            // Step 9 Enter Registration - 41060, Carrier - American Auto Tpt, Gate - GATE, New Item -JLG41060A01 and press the tab key
            roadGateForm = new RoadGateForm(@"Road Gate TT1");
            roadGateForm.SetRegoCarrierGate("41060");
            roadGateForm.txtNewItem.SetValue(@"JLG41060A01", 100);
            MTNKeyboard.Press(VirtualKeyShort.TAB);


            // Step 10 Check that the Total Weight field has no value in it for the Cargo Type Break-Bulk Cargo
            roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive General Cargo TT1");
            string cargoType = roadGateDetailsReceiveForm.CmbCargoType.GetValue();
            Assert.IsTrue(cargoType.Equals(@"Break-Bulk Cargo"), @"Expected - Break-Bulk Cargo, Actual is " + cargoType);
            string weight = roadGateDetailsReceiveForm.MtTotalWeight.GetMeasurementValue();
            Assert.IsTrue(string.IsNullOrEmpty(weight) || weight == "0", $"Expected - no weight value or is 0, Actual is {weight}");

            // Step 11 Close the Receive General Cargo TT1 form 
            roadGateDetailsReceiveForm.CloseForm();

            // Step 12 Enter the New Item -JLG41060A02 and press the tab key :
            roadGateForm.txtNewItem.SetValue(@"JLG41060A02", 10);
            MTNKeyboard.Press(VirtualKeyShort.TAB);

            // Step 13 Check that the Total Weight field has value in it for the Cargo Type Motor Vehicle
            roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive General Cargo TT1");
            cargoType = roadGateDetailsReceiveForm.CmbCargoType.GetValue();
            Assert.IsTrue(cargoType.Equals(@"Motor Vehicle"), @"Expected - Motor Vehicle, Actual is " + cargoType);
            roadGateDetailsReceiveForm.MtTotalWeight.ValidateValueAndType("2204.624", "lbs");

            // Step 14 Close the Receive General Cargo TT1 form  
            roadGateDetailsReceiveForm.CloseForm();

            // Step 15 Go back to Terminal Ops | Terminal Config | Gate tab 
            terminalConfigForm.SetFocusToForm();

            // Step 16 Click the edit button in the toolbar and untick the "Force User entry of 'Amount' at Gate In for Bulk/Break-Bulk" check box  
            // Step 17 Click the Save button in the toolbar
            terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Force User entry of 'Amount' at Gate In for Bulk/Break-Bulk", @"0", rowDataType: EditRowDataType.CheckBox);


            // Step 18 Go back to Road Gate TT1 form and Enter Registration - 41060, Carrier - American Auto Tpt, Gate - GATE, New Item -JLG41060A01 and press the tab key
            roadGateForm.SetFocusToForm();
            roadGateForm.txtNewItem.SetValue(@"JLG41060A01", 10);
            MTNKeyboard.Press(VirtualKeyShort.TAB);

            // Step 19 Check that the Total Weight field has value in it for the Cargo Type Break-Bulk Cargo - Total Weight should be 2204.624 lbs  
            roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive General Cargo TT1"); 
            cargoType = roadGateDetailsReceiveForm.CmbCargoType.GetValue();
            Assert.IsTrue(cargoType.Equals(@"Break-Bulk Cargo"), @"Expected - Break-Bulk Cargo, Actual is " + cargoType);
            roadGateDetailsReceiveForm.MtTotalWeight.ValidateValueAndType("2204.624", "lbs");

            // Step 19 Close the Receive General Cargo TT1 form  
            roadGateDetailsReceiveForm.CloseForm();
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = "_41060_";
            fileOrder = 1;

            // delete prenote
            CreateDataFileToLoad(@"DeletePrenote.xml",
                "<?xml version='1.0'?>\n<JMTInternalPrenote>\n<AllPrenote>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n			<Prenote Terminal='TT1'>\n		\n	<cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n            <id>JLG41060A01</id>\n            <commodity>GENL</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000.0000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>D</messageMode>\n			</Prenote>\n			<Prenote Terminal='TT1'>\n	<cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG41060A02</id>\n            <commodity>MCAR</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000.0000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>D</messageMode>\n			</Prenote>\n		</AllPrenote>\n</JMTInternalPrenote>\n\n");


            // Create prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0'?>\n<JMTInternalPrenote>\n<AllPrenote>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n			<Prenote Terminal='TT1'>\n		\n	<cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n            <id>JLG41060A01</id>\n            <commodity>GENL</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000.0000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n			</Prenote>\n			<Prenote Terminal='TT1'>\n	<cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG41060A02</id>\n            <commodity>MCAR</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000.0000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n			</Prenote>\n		</AllPrenote>\n</JMTInternalPrenote>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);


        }
    }
}
