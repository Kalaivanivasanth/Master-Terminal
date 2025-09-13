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
    public class TestCase60767 : MTNBase
    {

        readonly string[] _cargoGroupId =
        {
            "CG60767",
        };
        
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
                new SelectorQueryArguments(CargoReviewForm.Property.CargoGroupId, CargoReviewForm.Operation.StartsWith, _cargoGroupId[0]),
            });
            CargoReviewForm.DoSearch();

            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestRoad, "60767Road2",
                ReleaseRequestAddForm.ReleaseType.Road);
            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestShip, "60767Ship2",
                ReleaseRequestAddForm.ReleaseType.Ship);
            DoReleaseRequestProcess(CargoReviewForm.QueryResultsContextMenu.CreateReleaseRequestRail, "60767Rail2",
                ReleaseRequestAddForm.ReleaseType.Rail);
        }

        void DoReleaseRequestProcess(string menuOptionToSelect, string releaseNo, string releaseType)
        {
            string[] rowDetailsAndInstance =
            {
                $"Cargo Group Id^{_cargoGroupId[0]}~1",
            };

            CargoReviewForm.OpenRequiredContextMenuForCreate(menuOptionToSelect);

            ReleaseRequestAddForm.CheckCorrectData(new[]
            {
                new MTNGeneralArguments.FieldRowNameValueArguments() { FieldRowName = ReleaseRequestAddForm.RowNames.ReleaseType , FieldRowValue = releaseType},
            });
            ReleaseRequestAddForm.SetHeaderOrReserveDetails("<New request>",
                new[] { new ReleaseRequestArguments("Release No", releaseNo) });
            ReleaseRequestAddForm.DoSaveStatic();

            WarningErrorForm.CheckErrorMessagesExist("Errors for Release Request update TT1",
                new[]
                {
                    $"Code :96294. Item {_cargoGroupId[0]} has 892.100 MT of 98921.000 MT already reserved plus 7913.680 MT in current ownership changes, cannot release further 98028.900 MT"
                });

            ReleaseRequestAddForm.DoCancelStatic();

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo("Are you sure?");
            confirmationFormYesNo.ConfirmationFormYes();

            FormObjectBase.DeleteFormFromFormManager<ReleaseRequestAddForm>(); 
        }
    }

}
