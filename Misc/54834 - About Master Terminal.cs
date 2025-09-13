using DataObjects;
using DataObjects.AboutFormPageBO;
using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Misc
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase54834 : MTNBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestCleanup]
        public new void TestCleanup() => MTNTestCleanup(TestContext);
        
        [ClassCleanup]
        public new static void ClassCleanup() => MTNCleanup();

        private void MTNInitialize()
        {
            TestRunDO.GetInstance().SetDoResetConfigsToFalse();
            TestRunDO.GetInstance().SetDoNavigatorCloseAllToFalse();
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void CheckAboutForm()
        {
            MTNInitialize();

            AboutFormPageBO aboutFormPage = new MTNAboutFormPageBO();
            aboutFormPage.ValidateVersionDate();

        }
       
    }
}
