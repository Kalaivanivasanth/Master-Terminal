using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.FormObjects;
using MTNUtilityClasses.Classes;
using MTNForms.Controls;
using MTNGlobal.EnumsStructs;


namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops.System_Admin
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase66361 : MTNBase
    {
        private SystemAdminForm _systemAdminForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        private void MTNInitialise()
        {
            CallJadeScriptToRun(TestContext, @"resetData_66361");
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void DeliveryPartyAddUpdateDeleteUsingEDI()
        {
            MTNInitialise();

            // Add Delivery Party
            var dataToCreate = "D, A, TEST_1, TEST_DP1, TT1|PNL\r\nA, CHC, AU, 8 Cannes Rd, Hamilton County, Near Station, 3098, North Carolina, TEST33694A, Deliver ASAP, TEST_1, TestDescription1, N";
            DeliveryParty(dataToCreate);

            // Check Delivery Party
            FormObjectBase.MainForm.OpenSystemAdminFromToolbar();
            _systemAdminForm = new SystemAdminForm(@"System Administration");
            _systemAdminForm.cmbTable.SetValue(@"Delivery Party");

            _systemAdminForm.TblAdministrationItemsRH16A.FindClickRow(new[] { "Code^TEST_1" });

            // Update Delivery Party
            var dataToUpdate = "D, U, TEST_1, TEST_DP1, TT1|PNL\r\nA, CHC, AU, 8 Bailey Rd, BC County, Near Pump, 3066, North West, TEST33694B, Deliver ASAP, TEST_1, TestDescription2, Y";
            DeliveryParty(dataToUpdate);

            // Check Delivery Party
            _systemAdminForm.TblAdministrationItemsLH18.FindClickRow(new[] { "Code^TEST_1" });

            _systemAdminForm.GetGenericTabAndTable(@"Address", "4476");
             // Assert.IsTrue(MTNControlBase.FindClickRowInTable(_systemAdminForm.tblGeneric, @"id^TEST33694B~Description^TestDescription2~Default^Yes"));
             _systemAdminForm.TabGeneric.TableWithHeader.FindClickRow(["id^TEST33694B~Description^TestDescription2~Default^Yes"]);
            //_systemAdminForm.TblGeneric.FindClickRow(["id^TEST33694B~Description^TestDescription2~Default^Yes"]);

            // Delete Delivery Party
            var dataToDelete = "D, D, TEST_1, TEST_DP1, TT1|PNL\r\nA, CHC, AU, 8 Cannes Rd, Hamilton County, Near Station, 3098, North Carolina, TEST33694B, Deliver ASAP, TEST_1, TestDescription, Y";
            DeliveryParty(dataToDelete);
                
            // Check Delivery Party
            // Wednesday, 23 April 2025 navmh5 Can be removed 6 months after specified date Assert.IsNotNull(_systemAdminForm.TblAdministrationItemsLH18.FindClickRow(new[] { "Code^TEST_1" }));
            var rowNotFound = _systemAdminForm.TblAdministrationItemsLH18.FindClickRow(["Code^TEST_1"], doAssert: false);
            Assert.IsTrue(!string.IsNullOrEmpty(rowNotFound), "!string.IsNullOrEmpty(rowNotFound)");

        }

        void DeliveryParty(string data)
        {
            CallAPI callAPI = new CallAPI();
            string baseURL = TestContext.GetRunSettingValue(@"BaseUrl");
            callAPI.AddDeleteData(data, "Test33694", new GetSetArgumentsToCallAPI
            {
                RequestURL = baseURL + "SendEDI?MasterTerminalAPI",
            });
            
            
            
            
        }
    }
}
