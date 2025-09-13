using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNGlobal.EnumsStructs;


namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions.Operations

{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase63859 : MTNBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();



        [TestMethod]
        public void VoyageOpsQuickFilter()
        {
            MTNInitialize();

            FormObjectBase.NavigationMenuSelection("Voyage Functions|Operations|Voyage Operations Live");

            var voyageOperationsLiveForm = new VoyageOperationsLiveForm("Voyage Operations Live TT1");

            voyageOperationsLiveForm.cmbVoyage.SetValue(TT1.Voyage.AUTOVOY01, doDownArrow: true, searchSubStringTo: 5);



            voyageOperationsLiveForm.FilterCheck(voyageOperationsLiveForm.DoImports,
     voyageOperationsLiveForm.UndoImports,
    new[] { @"IMEX Status^Import~ID^AUTCARGO01" });

            voyageOperationsLiveForm.FilterCheck(
                voyageOperationsLiveForm.DoExports,
                voyageOperationsLiveForm.UndoExports,
                new[] { @"IMEX Status^Export~ID^AUTCARGO02",
                @"IMEX Status^Export~ID^AUTCARGO03" });

            voyageOperationsLiveForm.FilterCheck(voyageOperationsLiveForm.DoRTL,
                voyageOperationsLiveForm.UndoRTL,
               new[] { @"IMEX Status^Export~ID^AUTCARGO02",
                @"IMEX Status^Export~ID^AUTCARGO03" });

            voyageOperationsLiveForm.FilterCheck(voyageOperationsLiveForm.DoHigh,
                voyageOperationsLiveForm.UndoHigh,
                new[]  { @"IMEX Status^Import~ID^AUTCARGO01",
                @"IMEX Status^Export~ID^AUTCARGO02" });

        }
    }
}
