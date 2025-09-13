using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Invoicing;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.Invoicing
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase50527 : MTNBase
    {
        private DebtorTypesForm _debtorTypesForm;
        
        [ClassInitialize]
        public new static void ClassInitialize(TestContext context) => BaseClassInitialize_New(context);
        
        [TestInitialize]
        public new void TestInitialize() {}


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize() => LogInto<MTNLogInOutBO>();   


        [TestMethod]
        public void CheckDefaultDebtorTypesExist()
        {
            MTNInitialize();

            FormObjectBase.NavigationMenuSelection(@"Invoicing|Debtor Types");

            _debtorTypesForm = new DebtorTypesForm();

            string[] debtorTypes =
            {
                "Code^SDT0~Invoice Line Type^Empty Pool Party",
                "Code^SDT1~Invoice Line Type^Cargo Operator",
                "Code^SDT2~Invoice Line Type^Consignee",
                "Code^SDT3~Invoice Line Type^Consignor",
                "Code^SDT4~Invoice Line Type^Ship Operator",
                "Code^SDT5~Invoice Line Type^Business Unit",
                "Code^SDT6~Invoice Line Type^Customs Agent",
                "Code^SDT7~Invoice Line Type^Freight Forwarder",
                "Code^SDT8~Invoice Line Type^Event Type",
                "Code^SDT9~Invoice Line Type^Carrier",
                "Code^SDT10~Invoice Line Type^Stevedore for Voyage",
                "Code^SDT11~Invoice Line Type^BOL Operator",
                "Code^SDT12~Invoice Line Type^BOL Item First Consignee",
                "Code^SDT13~Invoice Line Type^BOL Item First Consignor",
                "Code^SDT14~Invoice Line Type^BOL Voyage Operator",
                "Code^SDT15~Invoice Line Type^Stevedore for Receive",
                "Code^SDT16~Invoice Line Type^Stevedore for Release",
                "Code^SDT17~Invoice Line Type^Shipping Line",
                "Code^SDT18~Invoice Line Type^Ownership Change Old Operator",
                "Code^SDT19~Invoice Line Type^Ownership Change New Operator",
            };
            int countOfLinesFound = CheckForDebtorTypeLines(debtorTypes);

            int numberOfLinesExpected = _debtorTypesForm.GetNumberOfSDTCodes();

            Assert.IsTrue(countOfLinesFound == numberOfLinesExpected,
                $"TestCase50257 - Incorrect number of Debtor Type Lines found.  Expecting: {numberOfLinesExpected} Actual: {countOfLinesFound}");
        }

        
        private int CheckForDebtorTypeLines(string[] debtorTypeLines)
        {
            // Need to rewrite this
            var lineCount = 0;

            foreach (var debtorTypeLine in debtorTypeLines)
            {
                _debtorTypesForm.TblDebtorTypes.FindClickRow(new [] { debtorTypeLine }, ClickType.None);
                lineCount++;
            }

            return lineCount;

        }

        

    }

}
