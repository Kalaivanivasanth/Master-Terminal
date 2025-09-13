using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.System_Items;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.System_Items.Harbor;
using MTNForms.FormObjects.System_Items.Invoicing;
using MTNForms.FormObjects.System_Items.Report_Maintenance;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Navigation_Only
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class _54689___VoyageFunctionsAndSystemItems : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}


        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void VerifyVoyageFunctionsForm()
        {
            MTNInitialize();

            ValidateForm<VoyageCargoCounterForm>("Voyage Functions|Admin|Voyage Cargo Counter", "Voyage Cargo Counter TT1", "Voyage Cargo Counter Navigation Failed");
            ValidateForm<VoyageTBAManualMatchForm>("Voyage TBA Manual Match", "Voyage TBA Manual Match TT1", "Voyage TBA Manual Match Navigation Failed");
            ValidateForm<ROROPlanningForm>("Planning|RORO  Planning", " ACTUAL Deck View by Discharge Port TT1", "RORO Planning Navigation Failed");
            ValidateForm<HoldJobOperationsForm>("Operations|Hold Job Operations", "Hold Job Operations TT1", "Hold Job Operations Navigation Failed");
            ValidateForm<ShipJobControllerForm>("Ship Job Controller", "Ship Job Controller TT1", "Ship Job Controller Navigation Failed");
            ValidateForm<VoyageSideProfileForm>("Voyage Side Profile", "Voyage Side Profile TT1", "Voyage Side Profile Navigation Failed");
            ValidateForm<BulkBreak_BulkMonitoringForm>("Bulk / Break-Bulk Monitoring", " ACTUAL Hold View, by Discharge Port TT1", "Bulk / Break-Bulk Monitoring Navigation Failed");
            ValidateForm<AutoSchedulerForm>("Background Process|Auto Scheduler", "Auto Scheduler Setup", "Auto Scheduler Navigation Failed");
            ValidateForm<BackupForm>("Backup", "Database Backup", "Backup Navigation Failed");

            /*
            // NAVMH5 21/10/2022 - Removing for the moment as have 2 menu items with the same name and we
            // don't seem to be catering for that correctly 
            FormObjectBase.NavigationMenuSelection("General Cargo|Cargo Subtypes");
            CargoSubtypesForm cargoSubtypesForm = new CargoSubtypesForm("Cargo Subtypes");
            cargoSubtypesForm.ValidateFormOpenedCorrectly("Cargo Subtypes Navigation Failed");
            cargoSubtypesForm.CloseForm();
            ValidateForm<CargoSubtypeGroupMaintForm>("Cargo Subtype Group Maint", "Cargo Subtype Groups", "Cargo Subtype Group Maint Navigation Failed");
            */

            ValidateForm<TidalDefinitionMaintenanceForm>("Harbour|Tidal Definition Maintenance", "Tidal Model Maintenance", "Tidal Definition Maintenance Navigation Failed");
            ValidateForm<CalculationTypesForm>("Invoicing|Calculation Types", "Calculation Types", "Calculation Types Navigation Failed");
            ValidateForm<DebtorRegisterForm>("Debtor Register", "Debtor Register", "Debtor Register Navigation Failed");
            ValidateForm<FinancialPeriodMaintenanceForm>("Financial Period Maintenance", "Financial Period Maintenance", "Financial Period Maintenance Navigation Failed");
            
            /*
             // navmh5 21/10/2022 - Removing as I can't get this to run correctly
             FormObjectBase.NavigationMenuSelection("Holiday Maintenance");
            HolidayMaintenanceForm holidayMaintenanceForm = new HolidayMaintenanceForm("Holiday Maintenance");
            holidayMaintenanceForm.ValidateFormOpenedCorrectly("Holiday Maintenance Navigation Failed");
            holidayMaintenanceForm.CloseForm();
            //ValidateForm<HolidayMaintenanceForm>("Holiday Maintenance ", "Holiday Maintenance", "Holiday Maintenance Navigation Failed");
            */
            
            ValidateForm<InvoiceProcessesForm>("Invoice Processes", "Invoice Process Maintenance", "Invoice Processes Navigation Failed");
            ValidateForm<InvoiceStatusesForm>("Invoice Statuses", "Invoice Status Maintenance", "Invoice Statuses Navigation Failed");
            ValidateForm<TermsOfTradeForm>("Terms Of Trade", "Invoice Terms of Trade", "Terms Of Trade Navigation Failed");
            ValidateForm<ReportAdminForm>("Report Maintenance|Report Admin", "JRW Admin", "Report Admin Navigation Failed");
            
            //System Ops
            ValidateForm<CodeGroupsForm>("System Ops|Code Groups", "Codes Maintenance", "Code Groups Navigation Failed");
            ValidateForm<DestToDischPortForm>("Dest to Disch Port", "Dest to Discharge Port", "Dest to Disch Port Navigation Failed");
            ValidateForm<DrawingAttributesForm>("Drawing Attributes", "Drawing Attributes", "Drawing Attributes Navigation Failed");
            ValidateForm<GarbageCollectionForm>("Garbage Collection", "Garbage Collection", "Garbage Collection Navigation Failed");
            ValidateForm<LocalesForm>("Locales", "Locale Maintenance", "Locales Navigation Failed");
            ValidateForm<PortCodeMaintenanceForm>("Port Code Maintenance", "Port Code Maintenance", "Port Code Maintenance Navigation Failed");
            ValidateForm<ReleaseHistoryForm>("Release History", "Master Terminal Patch Upgrade Logs", "Release History Navigation Failed");
            ValidateForm<QueryAccessSetsForm>("Query Access Sets", "Query Access Sets", "Query Access Sets Navigation Failed");
            ValidateForm<SystemPerformanceForm>("System Performance", "System Information", "System Performance Navigation Failed");
            ValidateForm<SystemCalendarViewForm>("System Calendar View", "System Calendar View", "System Calendar View Navigation Failed");
            ValidateForm<TerminologyMaintenanceForm>("Terminology Maintenance", "Terminology", "Terminology Maintenance Navigation Failed");
            ValidateForm<TextTranslationForm>("Text Translation", "Text Translation", "Text Translation Navigation Failed");
            ValidateForm<TrailerTypeMaintenanceForm>("Trailer Type Maintenance", "Trailer Type Maintenance", "Trailer Type Maintenance Navigation Failed");
            ValidateForm<UserCodeHooksForm>("User Code Hooks", "User Code Hook Maintenance", "User Code Hooks Navigation Failed");
            ValidateForm<VoyageJobPriorityForm>("Voyage Job Priority", "Job Priorities Master Terminal (System Wide Settings)", "Voyage Job Priority Navigation Failed");
            ValidateForm<WebConfigurationForm>("Web Configuration", "Web Configuration", "Web Configuration Navigation Failed");
            ValidateForm<SendMessageForm>("Send Message", "Send Message", "Send Message Navigation Failed");
            ValidateForm<UsersSignedOnForm>("Users Signed On", "Current Active Users", "Users Signed On Navigation Failed");
            //ValidateForm<ExternalWebServiceMonitorForm>("External Web Service Monitor", "External Web Services Monitor", "External Web Service Monitor Navigation Failed");

        }

    }
}
