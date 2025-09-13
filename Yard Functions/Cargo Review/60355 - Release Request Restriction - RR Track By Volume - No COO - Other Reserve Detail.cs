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
    public class TestCase60355 : MTNBase
    {

        const string CargoGroupId = "CG60355";
        
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

            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestRoad, "60355Road2", 
                ReleaseRequestAddForm.ReleaseType.Road);
            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestShip, "60355Ship2", 
                ReleaseRequestAddForm.ReleaseType.Ship);
            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestRail, "60355Rail2", 
                ReleaseRequestAddForm.ReleaseType.Rail);
        }

        void DoReleaseRequestProcess(string menuOptionToSelect, string releaseNo, string releaseType)
        {
            CargoReviewForm.OpenRequiredContextMenuForCreate(menuOptionToSelect);

            ReleaseRequestAddForm.CheckCorrectData(new[]
            {
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = ReleaseRequestAddForm.RowNames.ReleaseType, FieldRowValue = releaseType},
            });
            ReleaseRequestAddForm.SetHeaderOrReserveDetails("<New request>",
                new[] { new ReleaseRequestArguments(ReleaseRequestArguments.RowNames.ReleaseNo, releaseNo) });
            ReleaseRequestAddForm.SetHeaderOrReserveDetails("1 - 589900.0000 m3 x Bauxite Generic 2 (0 m3 released )",
                new[]
                {
                    new ReleaseRequestArguments(ReleaseRequestArguments.RowNames.VolumeRequested, "599900.0000"),
                    new ReleaseRequestArguments(ReleaseRequestArguments.RowNames.Volumem3, "599900.0000")
                });
            ReleaseRequestAddForm.DoSaveStatic();

            WarningErrorForm.CheckErrorMessagesExist($"Errors for Release Request update {terminalId}",
                new[]
                {
                    $"Code :96353. The Volume Requested value of 599900.0000 m3 can not be more than Cargo Group {CargoGroupId} assigned cargo Available Volume of 589900.0000 m3(10000.0000 m3 Reserved. 0 m3 Unavailable.).",
                    $"Code :96295. Item {CargoGroupId} has 10000.0000 m3 of 599900.0000 m3 already reserved, cannot release further 599900.0000 m3"
                });

            ReleaseRequestAddForm.DoCancelStatic();

            var confirmationFormYesNo = new ConfirmationFormYesNo("Are you sure?");
            confirmationFormYesNo.ConfirmationFormYes();

            FormObjectBase.DeleteFormFromFormManager<ReleaseRequestAddForm>(); 
        }
    }

}
