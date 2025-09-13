using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Navigation_Only
{

    [TestClass, TestCategory(TestCategories.MTN)]
    public class _54677___EDI_And_General_Functions : MTNBase
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
           
            // EDI Functions
            ValidateForm<EDICAMSGroupsForm>("EDI Functions|EDI CAMS Groups", "Terminal Administration TT1");
            
            ValidateForm<EDIUserDefinedFileTypesForm>(
                "EDI User Defined File Types", "User-Defined File Types Maintenance TT1");
            
            ValidateForm<EDISenderRecipientForm>(
                "EDI Sender Recipient", "Terminal Administration TT1");
            
            ValidateForm<EDIMessageAdministrationForm>(
                "EDI Message Administration", "EDI Message Administration TT1",
                "EDI Message Administration Navigation failed");
            
            ValidateForm<EDIWebServiceViewerForm>("EDI Web Service Viewer", "EDI Web Service Viewer TT1",
                "EDI Web Service Viewer Navigation failed");
            
            // General Functions
            ValidateForm<CargoGroupAssignmentMaintenanceForm>(
                "General Functions|Cargo Group Assignment Maintenance", "Cargo Group Assignment TT1",
                "Cargo Group Assignment Maintenance Form Navigation failed", doReset: true);
            
            ValidateForm<CargoQuickViewForm>(
                "Cargo Quick View", "Cargo Quick View TT1",
                "Cargo Quick View Form Navigated Successfully");
            
            ValidateForm<CommercialEnquiryForm>(
                "Commercial Enquiry", "Commercial Enquiry TT1",
                "Cargo Group Assignment Maintenance Form Navigation failed");
            
            ValidateForm<DocketMaintenanceForm>(
                "Docket Maintenance", "Docket Maintenance TT1",
                "Docket Maintenance Form Navigation failed");
            
            ValidateForm<StockMovementsForm>(
                "Stock Movements", "Stock Movements TT1",
                "Stock Movements Form Navigation failed");
            
            ValidateForm<SnapshotsSummaryForm>(
                "Snapshot Summary", "Snap Shot Summary TT1",
                "Snapshot Summary Form Navigation failed");
            
            /*ValidateForm<SystemAlertsForm>(
                "System Alerts", "TT1",
                "System Alerts Form Navigation failed");*/
            
            ValidateForm<TransactionEnquiryForm>(
                "Transaction Enquiry", "Transaction Enquiry TT1",
                "Transaction Enquiry Form Navigation failed");
            
            ValidateForm<JobRequestViewForm>(
                "Job Request View", "Job Request TT1",
                "Job Request View Form Navigation failed");

        }
        
    }
}