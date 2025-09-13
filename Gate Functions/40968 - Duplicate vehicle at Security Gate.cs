using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Security_Gate;
using MTNForms.FormObjects.Gate_Functions.Vehicle;
using MTNForms.FormObjects.Message_Dialogs;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40968 : MTNBase
    {
        SecurityGateForm _securityGateForm;
        SecurityGateNewVisitForm _securityGateNewVisitForm;
        VehicleCancelReasonForm _vehicleCancelReasonForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>("TT1GATEUSR");
            CallJadeScriptToRun(TestContext, @"resetData_40968");
        }


        [TestMethod]
        public void DuplicateVehicleAtSecurityGate()
        {
            MTNInitialize();
            
            // Note: to rerun, make sure all visit records are deleted.
            // cancel vehicle visit if already exists
            //VehicleVisitForm.CancelVehicleVisit(@"40968");

            // 1. Navigate to Security Gate
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Gate Functions|Security Gate");
            FormObjectBase.MainForm.OpenSecurityGateFromToolbar();
            _securityGateForm = new SecurityGateForm($"Security Gate Vehicle Visit Monitoring {terminalId}", vehicleId: @"40968");

            //2. Add new visit
            //_securityGateForm.btnNew.DoClick();
            _securityGateForm.DoNew();
            _securityGateNewVisitForm = new SecurityGateNewVisitForm($"New Vehicle Visit (Security Gate) {terminalId}");
            _securityGateNewVisitForm.cmbBatNumber.SetValue(@"40968A", additionalWaitTimeout: 2000);
            _securityGateNewVisitForm.txtRegistration.SetValue(@"40968");
            _securityGateNewVisitForm.btnSave.DoClick();

            //3. Add second visit with same registration
            _securityGateForm.DoNew();
            _securityGateNewVisitForm = new SecurityGateNewVisitForm($"New Vehicle Visit (Security Gate) {terminalId}");
            _securityGateNewVisitForm.cmbBatNumber.SetValue(@"40968B", additionalWaitTimeout: 2000);
            _securityGateNewVisitForm.txtRegistration.SetValue(@"40968");
            _securityGateNewVisitForm.btnSave.DoClick();

            //4. ensure error message is generated
            /*// Wednesday, 29 January 2025 navmh5 
            warningErrorForm = new WarningErrorForm(@"Errors for Vehicle Visit - Security Gate TT1");
            warningErrorForm.CheckWarningsErrorsExist(new [] { "Code :90915. Another vehicle with the same registration 40968 already exists at the security gate." });
            warningErrorForm.btnCancel.DoClick();*/
            WarningErrorForm.CheckErrorMessagesExist($"Errors for Vehicle Visit - Security Gate {terminalId}",
                new[]
                { "Code :90915. Another vehicle with the same registration 40968 already exists at the security gate." });

            //5. Cancel original visit
            _securityGateNewVisitForm.btnCancel.DoClick();
            // Wednesday, 29 January 2025 navmh5 MTNControlBase.FindClickRowInTable(_securityGateForm.tblVisits, "BAT^40968A", rowHeight: 16, xOffset: 150);
            _securityGateForm.TblVisits.FindClickRow(new [] { "BAT^40968A" });
            //_securityGateForm.btnCancel.DoClick();
            _securityGateForm.DoCancelVisit();
            _vehicleCancelReasonForm = new VehicleCancelReasonForm(@"Vehicle Cancel Reason TT1 ( BAT 40968A)");
            _vehicleCancelReasonForm.txtReason.SetValue(@"Test");
            _vehicleCancelReasonForm.btnOK.DoClick();
        }

    }

}
