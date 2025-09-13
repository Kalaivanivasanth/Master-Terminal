using DataObjects.LogInOutBO;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Navigation_Only
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class _54688___TerminalOpsNavigation : MTNBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void VerifyTerminalOpsForm()
        {
            MTNInitialize();

            // Yard Functions
            ValidateForm<IMEXTransitionForm>("Terminal Ops|IMEX Transition", $"Cargo Receival IMEX Transition {terminalId}", "IMEX Transition Navigation Failed");
            
            // This need to be investigated before it is changed
            FormObjectBase.NavigationMenuSelection("Cargo Counter");   // In this case this is acceptable as this is a navigation test
            CargoCounterForm cargoCounterForm = new CargoCounterForm($"Cargo Counter {terminalId}");
            cargoCounterForm.DoCancel(TestContext);
            cargoCounterForm.ValidateFormOpenedCorrectly("Cargo Counter Navigation Failed");
            cargoCounterForm.CloseForm();

            ValidateForm<HazardSegregationForm>("Hazard Segregation", $"Hazard Segregation {terminalId}", "Hazard Segregation Navigation Failed");
            ValidateForm<HandHeldConfigForm>("Hand Held Config", $"Hand Held Configuration Form {terminalId}", "Hand Held Config Navigation Failed");
            ValidateForm<InterfaceDefinitionForm>("Interface Definition", $"Interface Definition {terminalId}", "Interface Definition Navigation Failed");
            ValidateForm<MachineRulesMaintenanceForm>("Machine Rules Maintenance", $"Machine Rules Maintenance {terminalId}", "Machine Rules Maintenance Navigation Failed");
            ValidateForm<SatelliteViewDefinitionForm>("Satellite View Definition", $"Satellite View Definition - Default {terminalId}", "Satellite View Definition Navigation Failed");
            ValidateForm<TerminalAreaAuditDetailsForm>("Terminal Area Audit Details", $"Terminal Area Audit Details {terminalId}", "Terminal Area Audit Details Navigation Failed");
            ValidateForm<GateDefinitionForm>("Gate Definition", $"Gate Definition {terminalId}", "Gate Definition Navigation Failed");
            ValidateForm<TerminalSpaceUtilisationForm>("Terminal Space Utilisation", $"Terminal Space Utilisation {terminalId}", "Terminal Space Utilisation Navigation Failed");
            ValidateForm<VehicleEnquiryForm>("Vehicle Enquiry", $"Vehicle Enquiry {terminalId}", "Vehicle Enquiry Navigation Failed");
            ValidateForm<VehicleRTConfigForm>("Vehicle App Configuration", $"Vehicle App Configuration {terminalId}", "Vehicle App Configuration Navigation Failed");
            ValidateForm<WorkerShiftMaintenanceForm>("Worker Shift Maintenance", $"Worker Shifts Maintenance {terminalId}", "Worker Shift Maintenance Navigation Failed");

        }

    }
}
