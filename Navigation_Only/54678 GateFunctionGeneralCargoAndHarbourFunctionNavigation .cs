using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.General_Cargo;
using MTNForms.FormObjects.Harbour_Functions;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Navigation_Only
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class _54678_GateFunctionGeneralCargoAndHarbourFunctionNavigation_ : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();    //MTNSignon(TestContext);

        [TestMethod]
        public void VerifyCanOpenForm()
        {
            MTNInitialize();

            // Gate Functions
            ValidateForm<BreakBulkDeliveryOrdersForm>("Gate Functions|Break Bulk Delivery Orders",
                "Break Bulk Delivery Orders TT1", "Break Bulk Delivery Orders Navigation failed");
            
            ValidateForm<CargoLabelPrintRequestForm>("Cargo Label Print Request", "Cargo Label Print Request TT1",
                "Cargo Label Print Request Navigation failed");

            ValidateForm<CargoNotificationRequestForm>("Cargo Notification Request", "Cargo Notification Request TT1",
                "Cargo Notification Request Navigation failed");

            ValidateForm<CargoReleaseExportsForm>("Cargo Release Exports", "Cargo Release Exports TT1",
                "Cargo Release Exports Navigation failed");

            ValidateForm<DockReleaseForm>("Dock Release", "Dock Release TT1", "Dock Release Navigation failed");

            ValidateForm<EasyGateEnquiryForm>("Easy Gate Enquiry", "EasyGate Enquiry TT1",
                "Easy Gate Enquiry Navigation failed");

            //General Cargo Functions
            ValidateForm<CargoLoadoutForm>("General Cargo|Cargo Loadout", "General Cargo Loadout TT1",
                "Cargo Loadout Navigation failed", doReset: true);

            ValidateForm<GeneralCargoAreasForm>("General Cargo Areas", "Terminal Areas TT1",
                "Cargo Areas Navigation failed");

            ValidateForm<RejectItemForm>("Reject Item", "Reject Break Bulk TT1", "Reject Item Navigation failed");

            ValidateForm<StockOrdersForm>("Stock Orders", "Stock Orders TT1", "Stock Orders Navigation failed");

            ValidateForm<TransferForm>("Transfer", "Transfer Items TT1", "Transfer Navigation failed");

            //Harbour Functions

            ValidateForm<AuditTrailForm>("Harbour Functions|Audit Trail", "HMS Audit Trail Enquiry TT1",
                "Harbour Functions Navigation failed", doReset: true);

            ValidateForm<SearchAndSelectForm>("Search  Select", "HMS Voyage Search  Select TT1 HMSAVON TT1",
                "Search Select Navigation failed");

            ValidateForm<TidalModelInterfaceForm>("Tidal Model Interface", "Tidal Model",
                "Tidal Model Navigation failed");

        }

    }
}
