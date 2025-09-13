using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;
namespace MTNAutomationTests.TestCases.Master_Terminal.Navigation_Only
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class _54691___SystemItemsAndSystemOps : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void VerifyCanOpenForm()
        {
            MTNInitialize();

            // System Items | System Ops
            ValidateForm<CodeGroups>("System Ops|Code Groups", "Codes Maintenance", "Code Groups Navigation Failed");
            ValidateForm<Dest_to_Disch_Port>("Dest to Disch Port", "Dest to Discharge Port", "Dest to Discharge Port Navigation Failed");
            ValidateForm<Drawing_Attributes>("Drawing Attributes", "Drawing Attributes", "Drawing Attributes Navigation Failed");
            ValidateForm<Garbage_Collection>("Garbage Collection", "Garbage Collection", "Garbage Collection Navigation Failed");
            ValidateForm<Locales>("Locales", "Locale Maintenance", "Locales Navigation Failed");
            ValidateForm<Port_Code_Maintenance>("Port Code Maintenance", "Port Code Maintenance", "Port Code Maintenance Navigation Failed");
            ValidateForm<Release_History>("Release History", "Master Terminal Patch Upgrade Logs", "Release History Navigation Failed");
            ValidateForm<Query_Access_Sets>("Query Access Sets", "Query Access Sets", "Query Access Sets Navigation Failed");
            ValidateForm<System_Performance>("System Performance", "System Information", "System Performance Navigation Failed");
            ValidateForm<System_Calendar_View>("System Calendar View", "System Calendar View", "System Calendar View Navigation Failed");
            ValidateForm<Terminology_Maintenance>("Terminology Maintenance", "Terminology", "Terminology Maintenance Navigation Failed");
            ValidateForm<Text_Translation>("Text Translation", "Text Translation", "Text Translation Navigation Failed");
            ValidateForm<Trailer_Type_Maintenance>("Trailer Type Maintenance", "Trailer Type Maintenance", "Trailer Type Maintenance Navigation Failed");
            ValidateForm<User_Code_Hooks>("User Code Hooks", "User Code Hook Maintenance", "User Code Hooks Navigation Failed");
            ValidateForm<Voyage_Job_Priority>("Voyage Job Priority", "Job Priorities Master Terminal (System Wide Settings)", "Voyage Job Priority Navigation Failed");
            ValidateForm<Web_Configuration>("Web Configuration", "Web Configuration", "Web Configuration Navigation Failed");

        }

    }
}
