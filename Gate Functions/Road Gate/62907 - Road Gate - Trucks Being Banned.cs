using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase62907 : MTNBase
    {
        GateConfigurationForm _gateConfigurationForm = null;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Gate Configuration");
            _gateConfigurationForm = new GateConfigurationForm(@"Gate Configuration " + terminalId);
            _gateConfigurationForm.chkShowVehicleState.DoClick();
            _gateConfigurationForm.btnSave.DoClick();

        }

        [TestMethod]
        public void RoadGateTrucksBeingBanned()
        {
            MTNInitialize();
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            roadGateForm = new RoadGateForm(vehicleId: @"62907-Truck");
            roadGateForm.SetRegoCarrierGate("62907-TRUCK", carrier: "CARRIER1");
            roadGateForm.txtState.SetValue(@"TEST");
            roadGateForm.btnReleaseCargo.DoClick();
            
            //Ensure error message is generated
            WarningErrorForm.CheckErrorMessagesExist("Errors for Gate In/Out TT1",
                ["Code :79906. Vehicle (62907-TRUCK TEST) is not available for entry."]);
            
            roadGateForm.SetFocusToForm();
            roadGateForm.btnCancel.DoClick();
            
            
            
            
        }
    }
}
