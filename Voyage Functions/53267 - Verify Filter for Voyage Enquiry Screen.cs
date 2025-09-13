using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase53267 : MTNBase
    {
        VoyageEnquiryForm _voyageEnquiry;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void VoyageEnquirySearch()
        {
            MTNInitialize();

            FormObjectBase.NavigationMenuSelection("Voyage Functions | Admin | Voyage Enquiry");

            CheckStatusBarTotalUsingSearchCriteria(new []
            {
                new SearchCriteriaStatusBarArguments { SearchCriteria = "PBDA", ExpectedTotal = 2 },
                new SearchCriteriaStatusBarArguments { SearchCriteria = "bis", ExpectedTotal = 7 },
                new SearchCriteriaStatusBarArguments { SearchCriteria = "AJMAN", ExpectedTotal = 4 },
                new SearchCriteriaStatusBarArguments { SearchCriteria = "e4", ExpectedTotal = 9 },
                new SearchCriteriaStatusBarArguments { SearchCriteria = "xxxzzzz", ExpectedTotal = 0 }
            });

            _voyageEnquiry.DoToolbarSearchClear();
            _voyageEnquiry.CheckValueAfterSearchClearIsCorrect();

        }

        void CheckStatusBarTotalUsingSearchCriteria(SearchCriteriaStatusBarArguments[] args)
        {
            _voyageEnquiry = new VoyageEnquiryForm();

            var mismatchFound = string.Empty;
            foreach (var arg in args)
            {
                _voyageEnquiry.SetToolbarSearchValue(arg.SearchCriteria)
                    .CheckStatusBarTotal("2001", arg.ExpectedTotal, ref mismatchFound, doAssert: false, arg.SearchCriteria);
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
