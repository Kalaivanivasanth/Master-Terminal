using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Yard_Functions;
using MTNForms.FormObjects.Yard_Functions.Cargo_Storage_Areas;
using MTNForms.FormObjects.Yard_Functions.Rail_Activities;
using MTNForms.FormObjects.Yard_Functions.Reefer;
using MTNForms.FormObjects.Yard_Functions.Yard_Activities;
using MTNForms.FormObjects.Yard_Functions.Yard_Maintenance;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Navigation_Only
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class _54690___Yard_Functions_Navigator : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void VerifyYardFunctionsForm()
        {
            MTNInitialize();

            // Yard Functions
            ValidateForm<ROROAreasForm>("Yard Functions|Cargo Storage Areas|RORO Areas", "RoRo Lane Areas TT1", "RORO Areas Navigation Failed");
            ValidateForm<RailPlanningForm>("Rail Activities|Rail Planning", "ACTUAL, TT1", "Rail Planning Navigation Failed");
            ValidateForm<RailLoadListForm>("Rail Load List", "Rail Load List - NON PLANNED", "Rail Load List Navigation Failed");
            ValidateForm<RailConfigurationForm>("Rail Configuration", "Rail Configuration TT1", "Rail Configuration Navigation Failed");
            ValidateForm<RefConMonitorForm>("Reefer Activities|RefCon Monitor", "RefCon Monitor TT1", "Rail Planning Navigation Failed");
            ValidateForm<LocationActivityEnquiryForm>("Yard Activities|Location Activity Enquiry", "Location Activity Enquiry TT1", "Location Activity Navigation Failed");
            ValidateForm<LocationInventoryEnquiryForm>("Location Inventory Enquiry", "Location Inventory Enquiry TT1", "Location Inventory Navigation Failed");
            ValidateForm<LocationMaintenanceForm>("Yard Maintenance|Location Maintenance", "Location Maintenance TT1", "Location Maintenance Navigation Failed");
            ValidateForm<RTQueueMaintenanceForm>("RT Queue Maintenance", "Radio Telemetry Queue Maintenance TT1", "Radio Queue Maintenance Navigation Failed");
            ValidateForm<ToDoTaskListForm>("To Do Task List", "To Do Tasks TT1", "To Do Task List Navigation Failed");
            ValidateForm<FaultEnquiryForm>("Fault Enquiry", "Fault Enquiry TT1", "Fault Enquiry Navigation Failed");
            ValidateForm<FaultResponseEnquiryForm>("Fault Response Enquiry", "Fault Response Enquiry TT1", "Fault Response Enquiry Navigation Failed");
            ValidateForm<InterStorageTransportForm>("Inter Storage Transport", "Inter Storage Transport TT1", "Inter Storage Transport Navigation Failed");
            ValidateForm<MachineDelaysForm>("Machine Delays", "Delays Enquiry TT1", "Machine Delays Navigation Failed");
            ValidateForm<MTPoolingEnquiryForm>("MT Pooling Enquiry", "Empty Container Availability Enquiry TT1", "MT Pooling Enquiry Navigation Failed");
            ValidateForm<MoveForm>("Move", "Move TT1", "Move Navigation Failed");
            ValidateForm<RoadOperationsDualForm>("Road Operations Dual", "Road Operations TT1", "Road Operations Dual Navigation Failed");
            ValidateForm<VoyageDischargeSummaryForm>("Voyage Discharge Summary", "Voyage Discharge Summary TT1", "Voyage Discharge Summary Navigation Failed");
            ValidateForm<WeighVehicleForm>("Weigh Vehicle", "Weigh Vehicle Form TT1", "Weight Vehicle Navigation Failed");
            ValidateForm<WorkManagerForm>("Work Manager", "Work Manager TT1", "Work Manager Navigation Failed");

        }

    }
    }
