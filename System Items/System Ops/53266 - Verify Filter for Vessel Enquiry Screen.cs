using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase53266 : MTNBase
    {

        VesselEnquiryForm _vesselEnquiry;
               
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void VesselEnquirySearch()
        {
            MTNInitialize();

            FormObjectBase.NavigationMenuSelection("System Ops | Vessel Enquiry");
            
            CheckStatusBarTotalUsingSearchCriteria(new SearchCriteriaStatusBarArguments[]
            {
                new SearchCriteriaStatusBarArguments { SearchCriteria = "PBDA", ExpectedTotal = 1 },
                new SearchCriteriaStatusBarArguments { SearchCriteria = "bis", ExpectedTotal = 6 },
                new SearchCriteriaStatusBarArguments { SearchCriteria = "TYF", ExpectedTotal = 2 },
                new SearchCriteriaStatusBarArguments { SearchCriteria = "3e", ExpectedTotal = 4 },
                new SearchCriteriaStatusBarArguments { SearchCriteria = "NZ", ExpectedTotal = 7 },
                new SearchCriteriaStatusBarArguments { SearchCriteria = "xxxzzzz", ExpectedTotal = 0 }
            });
            
            // Click the Toolbar Search Criteria Clear Button
            _vesselEnquiry.DoToolbarSearchClear();
            _vesselEnquiry.CheckValueAfterSearchClearIsCorrect();

        }

        void CheckStatusBarTotalUsingSearchCriteria(SearchCriteriaStatusBarArguments[] args)
        {
            _vesselEnquiry = new VesselEnquiryForm();

            var mismatchFound = string.Empty;
            foreach (var arg in args)
            {
                _vesselEnquiry.SetToolbarSearchValue(arg.SearchCriteria)
                    .CheckStatusBarTotal(arg.ExpectedTotal, ref mismatchFound, doAssert: false);
            }
            
            Assert.IsTrue(string.IsNullOrEmpty(mismatchFound), $"The following search / status bar count mismatches found:\r\n{mismatchFound}");
        }
        
        class SearchCriteriaStatusBarArguments
        {
            public string SearchCriteria { get; set; }
            public int ExpectedTotal { get; set; }
        }
        
        
    }

}
