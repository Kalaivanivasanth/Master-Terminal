using DataObjects;
using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Cash_Functions;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Invoice_Functions.Maintenance;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Navigation_Only
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class _54685___Invoice_Functions_and_Cash_Functions_Navigation : MTNBase
    {

        MTNLogInOutBO _loggedIn;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => _loggedIn = LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void VerifyInvoiceFunctionsForm()
        {
            MTNInitialize();

            // Invoice Functions
            
            ValidateForm<InvoiceGenerationForm>("Invoice Functions|Invoice Generation", "Invoice Generation TT1", "Invoice Generation Navigation Failed");
            ValidateForm<BusinessUnitMaintenanceForm>("Maintenance|Business Unit Maintenance", "Business Unit Maintenance TT1", "Business Unit Maintenance Navigation Failed");
            ValidateForm<DebtorFinancialPeriodsForm>("Debtor Financial Periods", "Debtor Financial Periods TT1", "Debtor Financial Periods Navigation Failed");

            ValidateForm<GeneralLedgerCodesForm>("General Ledger Codes", "GL Codes DFBU TT1", "General Ledger Codes Navigation Failed");
            ValidateForm<InvoiceLineChargeMappingForm>("Invoice Line Charge Mapping", "Invoice Line Charge Mapping TT1", "Invoice Line Charge Mapping Navigation Failed");
            ValidateForm<InvoiceLineConsolidationForm>("Invoice Line Consolidation", "Invoice Line Debtor DFBU TT1", "Invoice Line Consolidation Navigation Failed");
            ValidateForm<InvoiceLineGroupsForm>("Invoice Line Groups", "Invoice Line Group DFBU TT1", "Invoice Line Groups Navigation Failed");
            ValidateForm<MTNForms.FormObjects.Invoice_Functions.Maintenance.InvoiceLineMappingForm>("Invoice Line Mapping", "Invoice Line Mapping TT1", "Invoice Line Mapping Navigation Failed");
            ValidateForm<InvoiceLineTypesForm>("Invoice Line Types", "Invoice Line Types DFBU TT1", "Invoice Line Types Navigation Failed");
            ValidateForm<InvoiceSignOffTypeForm>("Invoice Sign-off Type", "Invoice Sign-off Types DFBU TT1", "Invoice Sing-off Type Navigation Failed");
            ValidateForm<InvoiceTypesForm>("Invoice Types", "Invoice Types DFBU TT1", "Invoice Types Navigation Failed");
            ValidateForm<RateTableForm>("Rate Table", "Rate Table TT1", "Rate Table Navigation Failed");
            ValidateForm<ServicePaymentTypeMappingForm>("Service Payment Type Mapping", "Service Payment Type Mapping TT1", "Service Payment Type Mapping Navigation Failed");
            ValidateForm<ServiceScheduleForm>("Service Schedule", "Service Schedule DFBU TT1", "Service Schedule Navigation Failed");
            ValidateForm<ServiceTypeForm>("Service Type", "Service Types TT1", "Service Type Navigation Failed");
            ValidateForm<MTNForms.FormObjects.Invoice_Functions.Maintenance.InvoiceConfigurationForm>("Invoice Configuration", "Invoice Configuration DFBU TT1", "Invoice Configuration Navigation Failed");
            ValidateForm<TaxCodeMaintenanceForm>("Tax Code Maintenance", "Tax Code Maintenance DFBU TT1", "Tax Code Maintenance Navigation Failed");

            //MTNButton logOff = GetLogOffButton();
            //logOff.DoClick();
            
            LogOffMTN();
                
            /*CashTestClassInitialize();
            MTNSignon(TestContext, userName, false);*/
            TestRunDO.GetInstance().SetForCashTestCase();
            LogInto(_loggedIn);
            
            ValidateForm<CashConsolidationForm>("Cash Functions|Cash Consolidation", "Cash Consolidation CASH", "Cash Consolidation Navigation Failed");
            ValidateForm<CashCreditForm>("Cash Credit", "Cash Credit DFBU CASH", "Cash Credit Navigation Failed");
            ValidateForm<CashDebtorBalancesForm>("Cash Debtor Balances", "Cash Debtor Balances", "Cash Debtor Balances Navigation Failed");
            ValidateForm<CashDebtorTransactionsForm>("Cash Debtor Transactions", "Cash Debtor - Transaction List CASH", "Cash Debtor Transactions Navigation Failed");
            //ValidateForm<DutyDocItemEnquiryForm>("Duty Doc Item Enquiry ", "Duty Documentation Item Enquiry CASH", "DutyDocItemEnquiryForm Navigation Failed");
            ValidateForm<DutyDocumentationEnquiryForm>("Duty Documentation Enquiry", "Duty Documentation Enquiry CASH", "Duty Documentation Enquiry Navigation Failed");

        }

    }
}
