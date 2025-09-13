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
    public class TestCase60354 : MTNBase
    {

        const string CargoGroupId = "CG60354";
        
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

            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestRoad, "60354Road2",
                ReleaseRequestAddForm.ReleaseType.Road);
            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestShip, "60354Ship2", 
                ReleaseRequestAddForm.ReleaseType.Ship);
            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestRail, "60354Rail2",
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
            ReleaseRequestAddForm.SetHeaderOrReserveDetails("1 - 7000 x Big bag of Sand (0 released )",
                new[] { new ReleaseRequestArguments(ReleaseRequestArguments.RowNames.TotalRequested, "10000") });
            ReleaseRequestAddForm.DoSaveStatic();

            WarningErrorForm.CheckErrorMessagesExist($"Errors for Release Request update {terminalId}",
                new[]
                {
                    $"Code :95927. The Total Requested value of 10000 can not be more than {CargoGroupId} assigned cargo Available Quantity of 7000(3000 Reserved. 0 Unavailable.).",
                    $"Code :96295. Item {CargoGroupId} has 3000 of 10000 Items already reserved, cannot release further 10000 Items"
                });

            ReleaseRequestAddForm.DoCancelStatic();

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo("Are you sure?");
            confirmationFormYesNo.ConfirmationFormYes();

            FormObjectBase.DeleteFormFromFormManager<ReleaseRequestAddForm>(); 
        }
    }

}
