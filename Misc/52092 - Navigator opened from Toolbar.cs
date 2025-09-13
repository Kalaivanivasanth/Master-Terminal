using DataObjects;
using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Misc;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Misc
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase52092 : MTNBase
    {
        private const string TestCaseNumber = @"52092";
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
            BaseClassInitialize_New(testContext);
            //userName = "MTN52092";
        }

        [TestInitialize]
        public new void TestInitialize() {}
      

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        private void MTNInitialize()
        {
            //MTNSignon(TestContext, userName);
            LogInto<MTNLogInOutBO>("MTN52092");

            navigatorForm = new NavigatorForm();
        }
        

        [TestMethod]
        public void CheckCanReopenClosedNavigator()
        {
            MTNInitialize();

            navigatorForm.CloseFromContextMenu(false);

            // reopen navigator
            //_mainForm.ShowNavigatorMenu();
            //MTNBase.ShowNavigatorMenu();
            FormObjectBase.MainForm.ShowNavigatorMenuFromToolbar();

            navigatorForm = new NavigatorForm();
        }

        

       
    }
}
