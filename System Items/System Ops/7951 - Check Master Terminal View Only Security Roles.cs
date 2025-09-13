using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.System_Items.System_Ops;
using System.Collections.Generic;
using System.Linq;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase7951 : MTNBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>("USERSECROL");


        [TestMethod]
        public void CheckMTNViewOnlySecurityRoles()
        {
            MTNInitialize();

            var combinedList = new List<string>();

            // Step 1
            // 27/01/2025 navmh5 FormObjectBase.NavigationMenuSelection(@"System Ops|Security Roles");
            FormObjectBase.MainForm.OpenSecurityRolesFromToolbar();
            var securityRolesForm = new SecurityRolesForm();

            // Step 4

            // System Role additional permissions to check
            string[,] additionalChecks = 
            {
                 { "Administration", "Hand Held Apps Logon As" },
                 { "Administration", "Send Message" },
                 { "Administration", "System Performance" },
                 { "Administration", "To Do Task shown on PreNoteForm" },
                 { "Administration", "Vehicle RT Logon As" },
                 { "E-Commerce", "Send Email (Web Service)" },
                 { "Log", "Logs AS400 File Load (Port only)" },
                 { "ReportWriter", "Report Writer Configuration Read-Only Access (Port only)" },
                 { "ReportWriter", "Report Writer Designer Read-Only Access (Port only)" }
            };

            // ignore these items
            string[,] itemsToIgnore = { };

            // System Role tabs to check
            string[] tabsToCheck =   
            {
               "System",
               "Component",
               "Administration",
               "Invoicing",
               "E-Commerce",
               "Yard",
               "ReportWriter",
               "Log",
               "HMS",
               "General",
               "Ship",
               "GeneralCargo"
            };

            securityRolesForm.CheckAllViewPermissionsSelected("System Roles", "View Only User - Sys", tabsToCheck,
                additionalChecks, itemsToIgnore, ref combinedList);

            // Step 6

            // System Role additional permissions to check
            additionalChecks = new [,]
            {
                { "General", "Always Show if Stops Present" },
                { "General", "Transaction Enquiry" },
                { "E-Commerce", "EDI CAMS Groups" },
                { "E-Commerce", "EDI CAMS Protocols" },
                { "Administration", "Selector Query Entry" },
                { "Administration", "Selector Query Run" },
                { "Administration", "Transaction Enquiry" },
                { "Yard", "Voyage Discharge Summary" },
                { "Yard", "To Do Tasks" },
                { "Ship", "Voyage Cargo Counter" },
                { "Invoicing", "Access Multi Company" },
                { "Invoicing", "Export Invoices" },
                { "Invoicing", "Export invoices to file" },
                { "Invoicing", "Generate Snapshot Summary Invoices" },
                { "Invoicing", "Invoice Lines Re-evaluate Invoice Lines" },
                { "Invoicing", "Cash Credit" },
                { "Invoicing", "Cash Debtor Balances" },
                { "Invoicing", "Cash Transaction Enquiry" },
                { "Invoicing", "Export invoices" },
                { "Invoicing", "Invoice Line Mapping" },
                { "Invoicing", "Rate Audit Report" },
                { "Gate", "Cargo Release Imports Generate Pin Number (Port only)" },
                { "Gate", "Cargo Release Request Generate Pin Number (Port only)" },
                { "Gate", "Cargo Release Imports Forwarder Update" },
                { "Gate", "Duty Documentation Item Enquiry" },
                { "Gate", "Pre Note Receive Cargo" },
                { "Mobile App", "Cargo Identifier" },
                { "Mobile App", "Location Check" },
                { "Mobile App", "Mobile Cargo Enquiry" },
                { "Mobile App", "Mobile Location Summary" }
            };

            // ignore these items
            itemsToIgnore = new [,]
            {
                { "General", "Cargo Quick View Maint" },
                { "Yard", "RORO Areas View Maint" },
                { "Yard", "Satellite View Definition Maint" },
                { "Yard", "Satellite View Maint" }
            };

            // System Role tabs to check
            tabsToCheck = new []  {
               "General",
               "E-Commerce",
               "Administration",
               "Yard",
               "Ship",
               "Invoicing",
               "Component",
               "Gate",
               "Log",
               "GeneralCargo",
               "XML",
               "Mobile App", 
               "Web Services"
            };

            securityRolesForm.CheckAllViewPermissionsSelected("Terminal Roles", "View Only User - Term", tabsToCheck,
                additionalChecks, itemsToIgnore, ref combinedList);

            Assert.IsTrue(!combinedList.Any(),
                $"TestCase7951 failed with the following Security Role issue:\r\n{string.Join("\r\n", combinedList)}");
        }

        

    }

}
