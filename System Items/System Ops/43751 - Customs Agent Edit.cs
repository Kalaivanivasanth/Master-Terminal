using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43751 : MTNBase
    {

        AgentMaintenanceForm _agentMaintenanceForm;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();
            
            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_43751");
        }


        [TestMethod]
        public void EditCustomsAgent()
        {
            MTNInitialize();
            
            // Navigate to Agent Debtor Maintenace
            FormObjectBase.NavigationMenuSelection(@"System Ops|Agent Debtor Maintenance");

            // 1. Add new Customs Agent
            _agentMaintenanceForm = new AgentMaintenanceForm(@"Agent Maintenance");
            //MTNControlBase.SetValue(agentMaintenanceForm.cmbAgentType, @"Customs Agent");
            _agentMaintenanceForm.cmbAgentType.SetValue(@"Customs Agent");
            //agentMaintenanceForm.btnNew.DoClick();
            _agentMaintenanceForm.DoNew();
            MTNControlBase.SetValueInEditTable(_agentMaintenanceForm.tblDetails, @"Code", @"43751");
            MTNControlBase.SetValueInEditTable(_agentMaintenanceForm.tblDetails, @"Description", @"TESTING 43751");
            MTNControlBase.SetValueInEditTable(_agentMaintenanceForm.tblDetails, @"Debtor", @"TEST43751");
            //agentMaintenanceForm.btnSave.DoClick();
            _agentMaintenanceForm.DoSave();
            _agentMaintenanceForm.btnClose.DoClick();

            // 2. Edit the Customs Agent
            FormObjectBase.NavigationMenuSelection(@"System Ops|Agent Debtor Maintenance", forceReset: true);
            _agentMaintenanceForm = new AgentMaintenanceForm(@"Agent Maintenance");
            //MTNControlBase.SetValue(agentMaintenanceForm.cmbAgentType, @"Customs Agent");
            _agentMaintenanceForm.cmbAgentType.SetValue(@"Customs Agent");
            //MTNControlBase.SetValue(agentMaintenanceForm.cmbTypeSelection, @"TESTING 43751");
            _agentMaintenanceForm.cmbTypeSelection.SetValue(@"TESTING 43751");

            //agentMaintenanceForm.btnEdit.DoClick();
            _agentMaintenanceForm.DoEdit();
            MTNControlBase.SetValueInEditTable(_agentMaintenanceForm.tblDetails, @"Address", @"123 MTN");
            //agentMaintenanceForm.btnSave.DoClick();
            _agentMaintenanceForm.DoSave();
            _agentMaintenanceForm.btnClose.DoClick();

            // 3. Delete the Customs Agent - note didn't work unless I created new form object
            FormObjectBase.NavigationMenuSelection(@"System Ops|Agent Debtor Maintenance", forceReset: true);
            _agentMaintenanceForm = new AgentMaintenanceForm(@"Agent Maintenance");
            //MTNControlBase.SetValue(agentMaintenanceForm.cmbAgentType, @"Customs Agent");
            _agentMaintenanceForm.cmbAgentType.SetValue(@"Customs Agent");
            //MTNControlBase.SetValue(agentMaintenanceForm.cmbTypeSelection, @"TESTING 43751");
            _agentMaintenanceForm.cmbTypeSelection.SetValue(@"TESTING 43751");
            //agentMaintenanceForm.btnDelete.DoClick();
            _agentMaintenanceForm.DoDelete();

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Delete");
            confirmationFormYesNo.btnYes.DoClick();
            _agentMaintenanceForm.btnClose.DoClick();


        }

        

    }

}
