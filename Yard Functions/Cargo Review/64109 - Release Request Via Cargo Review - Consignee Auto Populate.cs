using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNArguments.Classes;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.Yard_Functions.Cargo_Review;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;


namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Cargo_Review
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase64109 : MTNBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
           LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void ReleaseRequestConsigneeAutoPopulate()
        {
            MTNInitialize();

            FormObjectBase.NavigationMenuSelection("Yard Functions|Cargo Review");
            CargoReviewForm.DoSelectorQuery(new[]
            {
                new SelectorQueryArguments(CargoReviewForm.Property.Site, CargoReviewForm.Operation.EqualTo, "On Site"),
                new SelectorQueryArguments(CargoReviewForm.Property.CargoGroupId, CargoReviewForm.Operation.StartsWith, "64109TESTG"),
            });
            CargoReviewForm.DoSearch();


            CargoReviewForm.OpenRequiredContextMenuForCreate(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestRoad);
            ReleaseRequestAddForm.CheckCorrectData(new[]
            {
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = ReleaseRequestAddForm.RowNames.Consignee, FieldRowValue = "JFH"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = ReleaseRequestAddForm.RowNames.Consignor, FieldRowValue = "HINO"},
            });
        }

              
    }

}
