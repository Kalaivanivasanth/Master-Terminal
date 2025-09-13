using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Logs;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Navigation_Only
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class _54687___LogFunctionsNavigation : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
       
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();   //MTNSignon(TestContext);

        [TestMethod]
        public void VerifyCanOpenForm()
        {
            MTNInitialize();

            // LogVoyageBerthMaintenanceForm
            ValidateForm<LogVoyageBerthMaintenanceForm> ("Log Functions|Log Voyage Berth Maintenance", "Berth Maintenance TT1", "Berth Maintenance Navigation");

            //CheckScalingForm
            ValidateForm<CheckScalingForm>("Check Scaling", "Check Scaling TT1");

            //DisassembleForm
            ValidateForm<DisassemblyForm>("Disassembly", "Disassembly TT1");

            //DisassembleForm
            ValidateForm<LogAreasForm>("Log Areas", "Terminal Areas TT1");

            //LogRowMaintenanceForm
            ValidateForm<LogRowMaintenanceForm>("Log Row Maintenance", "Log Row Maintenance TT1");

            //LoadOutForm
            ValidateForm<LoadOutForm>("Log Voyage Functions|Load Out", "Load Out Entry TT1", "Log Voyage Functions Load Out Navigation");

            //LogVoyageFunctionsForm
            ValidateForm<LogVoyageFunctionsForm> ("Log Voyage Operations", "Log Voyage Operations TT1", "Log Voyage Operations Navigation");

            //NoNumberReconciliationForm
            ValidateForm<NoNumberReconciliationForm> ("No Number Reconciliation", "No-Number Reconciliation TT1", "No Number Reconciliation Navigation");

            //OffLoadForm
            ValidateForm<OffLoadForm>("OffLoad", "Offload TT1", "Off Load Navigation");

            //PencilTrimmingForm
            ValidateForm<PencilTrimmingForm> ("Pencil Trimming", "Pencil Trimming TT1", "Pencil Trimming Navigation");

            //TransitTransferForm
            ValidateForm<TransitTransferForm> ("Transit Transfer", "Transit Transfer TT1", "Transit Transfer Navigation");

            //LoadOutReconciliationForm
            ValidateForm<LoadOutReconciliationForm>("Load Out Reconciliation", "Load Out Reconciliation TT1", "Log Voyage Functions Load Out Reconciliation Navigation");

            //RegradingForm
            ValidateForm<RegradingForm>("Regrading", "Regrading TT1");

            //ReinstateRejectsForm
            ValidateForm<ReinstateRejectsForm> ("Reinstate Rejects", "Reinstate Rejects TT1");

            //LogRejectionsForm
            ValidateForm<LogRejectionsForm>("Log Rejections", "Rejection TT1");

            //ReticketingForm
            ValidateForm<ReticketingForm>("Reticketing", "Reticketing TT1");

            //TransfersForm
            ValidateForm<TransfersForm>("Transfers", "Transfer TT1");


        }

    }
}
