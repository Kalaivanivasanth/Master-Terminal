using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase65239 : MTNBase
    {
        RoadGatePickerForm roadGatePickerForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void RoadGatePickerRailCargo()
        {
            MTNInitialize();

            // Open road gate form and enter vehicle visit details
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            RoadGateForm roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1");
            roadGateForm.SetRegoCarrierGate("65239");
            roadGateForm.txtNewItem.SetValue(@"TEST65239");

            // Initialize the RoadGatePickerForm
            roadGatePickerForm = new RoadGatePickerForm(@"Picker");

            // Enable "Show Rail Prenotes" and click rows
            roadGatePickerForm.ManagePrenotesAndClickRows(
                enablePrenotes: true,
                rowDescriptions: new[]
                {
                    @"Description^1 x Rolls~Operator^ABOC~Cargo Id^BKG101",
                    @"Description^1 x Rolls~Operator^ABOC~Cargo Id^BKG102",
                    @"Description^1 x Rolls~Operator^ABOC~Cargo Id^BKG103",
                    @"Description^1 x Rolls~Operator^ABOC~Cargo Id^BKG104"

                }
                
            );

            // Disable "Show Rail Prenotes" and click rows
            roadGatePickerForm.ManagePrenotesAndClickRows(
                enablePrenotes: false,
                rowDescriptions: new[]
                {
                  @"Description^1 x Rolls~Operator^ABOC~Cargo Id^BKG101",
                  @"Description^1 x Rolls~Operator^ABOC~Cargo Id^BKG102"
                }
               
            );

            // Cancel picker form
            roadGatePickerForm.btnCancel.DoClick();

            // Return to road gate form and cancel the vehicle visit
            roadGateForm.SetFocusToForm();
            roadGateForm.CancelVehicleVisit(@"Test65239");
        }
    }
}
