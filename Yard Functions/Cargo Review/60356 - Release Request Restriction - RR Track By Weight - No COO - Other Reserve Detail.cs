using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNArguments.Classes;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Cargo_Review;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Cargo_Review
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase60356 : MTNBase
    {

        const string CargoGroupId = "CG60356";
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>("MTNCOO");


        [TestMethod]
        public void ReleaseRequestTrackedByQtyNoReserveDetailsWithCOO()
        {
            MTNInitialize();
            
            FormObjectBase.MainForm.OpenCargoReviewFromToolbar();
            CargoReviewForm.DoSelectorQuery(new[]
            {
                new SelectorQueryArguments(CargoReviewForm.Property.Site, CargoReviewForm.Operation.EqualTo, "On Site"),
                new SelectorQueryArguments(CargoReviewForm.Property.CargoGroupId, CargoReviewForm.Operation.StartsWith, CargoGroupId),
            });
            CargoReviewForm.DoSearch();

            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestRoad, "60356Road2",
                ReleaseRequestAddForm.ReleaseType.Road);
            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestShip, "60356Ship2",
                ReleaseRequestAddForm.ReleaseType.Ship);
            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestRail, "60356Rail2",
                ReleaseRequestAddForm.ReleaseType.Rail);
        }

        void DoReleaseRequestProcess(string menuOptionToSelect, string releaseNo, string releaseType)
        {
            CargoReviewForm.OpenRequiredContextMenuForCreate(menuOptionToSelect);

            ReleaseRequestAddForm.CheckCorrectData(new[]
            {
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = ReleaseRequestAddForm.RowNames.ReleaseType , FieldRowValue = releaseType},
            });
            ReleaseRequestAddForm.SetHeaderOrReserveDetails("<New request>",
                new[] { new ReleaseRequestArguments(ReleaseRequestArguments.RowNames.ReleaseNo, releaseNo) });
            ReleaseRequestAddForm.SetHeaderOrReserveDetails("1 - 6425.000 MT x Clinker (0 MT released )",
                new[]
                {
                    // Tuesday, 18 March 2025 navmh5 new ReleaseRequestArguments(ReleaseRequestArguments.RowNames.ByWeight, "1"),
                    new ReleaseRequestArguments(ReleaseRequestArguments.RowNames.WeightRequested, "6426.000")
                });
            ReleaseRequestAddForm.DoSaveStatic();

            WarningErrorForm.CheckErrorMessagesExist($"Errors for Release Request update {terminalId}",
                new[]
                {
                    $"Code :95928. The Weight Requested value of 6426.000 MT can not be more than Cargo Group {CargoGroupId} assigned cargo Available Weight of 6425.000 MT(100.000 MT Reserved. 0 MT Unavailable.).",
                    $"Code :96295. Item {CargoGroupId} has 100.000 MT of 6525.000 MT already reserved, cannot release further 6426.000 MT"
                });

            ReleaseRequestAddForm.DoCancelStatic();

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo("Are you sure?");
            confirmationFormYesNo.ConfirmationFormYes();

            FormObjectBase.DeleteFormFromFormManager<ReleaseRequestAddForm>(); 
        }
    }

}
