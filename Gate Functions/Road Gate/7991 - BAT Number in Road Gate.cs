using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Gate_Functions.Security_Gate;
using MTNForms.FormObjects.Terminal_Functions;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase7991 : MTNBase
    {

        private const string TestCaseNumber = "7991";

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
            LogInto<MTNLogInOutBO>("TT1GATEUSR");
            CallJadeScriptToRun(TestContext, "resetData_" + TestCaseNumber);
        }
        

        [TestMethod]
        public void BATNumberInRoadGate()
        {

            MTNInitialize();


            // Step 1
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Admin");
            FormObjectBase.MainForm.OpenTerminalAdminFromToolbar();
            TerminalAdministrationForm terminalAdministrationForm = new TerminalAdministrationForm();
            terminalAdministrationForm.cmbTable.SetValue(@"RFID Tag");
            terminalAdministrationForm.GetGenericTabAndTable("Details");

            // Step 2
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date 
            /*MTNControlBase.FindClickRowInTable(terminalAdministrationForm.tblItems,
                @"Code^7991A0001~Description^RFID TAG FOR TEST 7991~Serial Number^7991A0001");*/
            terminalAdministrationForm.TblItems.FindClickRow(new[]
                { "Code^7991A0001~Description^RFID TAG FOR TEST 7991~Serial Number^7991A0001" });
            terminalAdministrationForm.DoEdit();

            // Step 3
            MTNControlBase.SetValueInEditTable(terminalAdministrationForm.TblGeneric.GetElement(), @"Is Available for Entry", @"1", EditRowDataType.CheckBox);
            terminalAdministrationForm.DoSave();

            // Step 4
            // Monday, 7 April 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Gate Functions|Security Gate", forceReset: true);
            FormObjectBase.MainForm.OpenSecurityGateFromToolbar();
            SecurityGateForm securityGateForm = new SecurityGateForm(vehicleId: @"7991");

            // Step 5
            securityGateForm.DoNew();
            SecurityGateNewVisitForm securityGateNewVisitForm = new SecurityGateNewVisitForm(@"New Vehicle Visit (Security Gate) " + 
                securityGateForm.GetTerminalName());

            // Step 6
            securityGateNewVisitForm.cmbBatNumber.SetValue(@"7991A0001");
            securityGateNewVisitForm.txtDriverName.SetValue(@"D7991");
            securityGateNewVisitForm.txtRegistration.SetValue(@"7991");

            // Step 7
            securityGateNewVisitForm.btnSave.DoClick();
            var visitNumber = securityGateForm.ReturnVisitNumberForBAT(@"7991^7991A0001^D7991");

            // Step 8
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Road Gate");
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm($"Road Gate {terminalId}");

            // Step 10
            roadGateForm.txtVisitNumber.SetValue(visitNumber);
            roadGateForm.btnVisitNumberSearch.DoClick();
            Assert.IsTrue(roadGateForm.txtRegistration.GetText() == "7991",
                $"TestCase7991 - Registration ({roadGateForm.txtRegistration.GetText()} does not match 7991");
            Assert.IsTrue(roadGateForm.txtDriverCode.GetText() == "D7991",
                $"TestCase7991 - Driver Code ({roadGateForm.txtDriverCode.GetText()} does not match D7991");
            
        }
        
    }

}
