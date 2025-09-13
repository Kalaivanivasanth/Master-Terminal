using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase65121 : MTNBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void AllowDuplicateNamesInRoadGateEntry()
        {
            MTNInitialize();

            // Open Gate Functions | Road Gate
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");

            roadGateForm = new RoadGateForm("Road Gate TT1");

            // Terminal Config Known Vehicle and Driver Required - Off
            roadGateForm.txtRegistration.SetValue(@"RSTEST");

            roadGateForm.txtDriverName.SetValue(@"TEST63908");

            roadGateForm.cmbDriverCodeCmb.SetValue("TEST01", doDownArrow: true);
            Assert.AreEqual("TEST01", roadGateForm.cmbDriverCodeCmb.GetValue(), "The value in cmbDriverCodeCmb is not as expected.");

            roadGateForm.cmbDriverCodeCmb.SetValue("TEST02", doDownArrow: true);
            Assert.AreEqual("TEST02", roadGateForm.cmbDriverCodeCmb.GetValue(), "The value in cmbDriverCodeCmb is not as expected.");


            roadGateForm.btnCancel.DoClick();

            roadGateForm.CloseForm();



            //Terminal Config Known Vehicle and Driver Required - On

            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            var terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Known Vehicle and Driver Required",@"1" , rowDataType: EditRowDataType.CheckBox);

            terminalConfigForm.CloseForm();

            //  Open Gate Functions | Road Gate
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            //roadGateForm.SetFocusToForm();
            roadGateForm = new RoadGateForm("Road Gate TT1");

            roadGateForm.cmb1.SetValue("RSTEST");

            roadGateForm.cmbDriverCodeCmb1.SetValue("TEST01 TEST63908", doDownArrow: true);
            Assert.AreEqual("TEST01 TEST63908", roadGateForm.cmbDriverCodeCmb1.GetValue(), "The value in cmbDriverCodeCmb1 is not as expected.");

            roadGateForm.cmbDriverCodeCmb1.SetValue("TEST02 TEST63908", doDownArrow: true);
            Assert.AreEqual("TEST02 TEST63908", roadGateForm.cmbDriverCodeCmb1.GetValue(), "The value in cmbDriverCodeCmb1 is not as expected.");


            roadGateForm.btnCancel.DoClick();

            roadGateForm.CloseForm();
        }
    }

}
