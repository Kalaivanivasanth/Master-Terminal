using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNArguments.Classes;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects.Yard_Functions.Cargo_Review;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.Controls;
using MTNGlobal.EnumsStructs;


namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Cargo_Review
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase65277 : MTNBase
    {
        private ReleaseRequestAddForm _releaseRequestAddForm;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>("MTNCOO");
        }

        [TestMethod]
        public void CargoReviewSortingMethod()
        {
            MTNInitialize();

            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Yard Functions|Cargo Review");
            FormObjectBase.MainForm.OpenCargoReviewFromToolbar();
            CargoReviewForm.DoSelectorQuery(new[]
            {
                new SelectorQueryArguments(CargoReviewForm.Property.Site, CargoReviewForm.Operation.EqualTo, "On Site"),
                new SelectorQueryArguments("Cargo Group Id", "starts with", "65277G", SelectorQueryArguments.LineAction.Add),
            });

            var cargoReviewForm = new CargoReviewForm("Cargo Review TT1");
            cargoReviewForm.ValidateDataTableRowDetails(new[] { "Cargo Group Id^65277G1~Sorting Method^FIFO", "Cargo Group Id^65277G2~Sorting Method^FIFO", "Cargo Group Id^65277G3~Sorting Method^LIFO" });
            CargoReviewForm.OpenRequiredContextMenuForCreate(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestRoad);
            _releaseRequestAddForm = new ReleaseRequestAddForm(@"New request... TT1");
            MTNControlBase.FindClickRowInList(_releaseRequestAddForm.tblReleaseRequestList, @"1 - 2 x Steel Coil (0 released )");
            ReleaseRequestAddForm.CheckCorrectData(new[]
            {
                new MTNGeneralArguments.FieldRowNameValueArguments() { FieldRowName = ReleaseRequestAddForm.RowNames.ExtraCriteria, FieldRowValue = "Sorting Method=FIFO"},
             
            });
        }
    }
}