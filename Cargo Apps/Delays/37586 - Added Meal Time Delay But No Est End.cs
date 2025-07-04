using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Delays;
using MTNWebPages.PageObjects.Popups;
using System;
using MTNGlobal.EnumsStructs;
using DataObjects.LogInOutBO;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Delays
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37586 : MobileAppsBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        void MTNInitialize()
        {
            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void AddedMealTimeDelayNoEstEndTime()
        {
            MTNInitialize();

            var currentTimeStamp = DateTime.Now;
            
            // Step 2
            HomePage.ClickTile(HomePage.BtnDelays);

            // Step 3
            MA_DelaysPage delaysPage = new MA_DelaysPage(TestContext);

            // Step 4
            delaysPage.DoMachine();

            // Step 5
            MA_DelaysMachinePage delayMachinePage = new MA_DelaysMachinePage(TestContext);
            delayMachinePage.AddDelayToMachine(@"CRN1");

            // Step 6
            MA_MachineDelaysPage machineDelays = new MA_MachineDelaysPage(TestContext);
            machineDelays.DoAddNewDelay();

            MA_MachineDelayNewDetailsPage machineDelayDetailsPage = new MA_MachineDelayNewDetailsPage(TestContext);

            currentTimeStamp = currentTimeStamp.AddMinutes(60);
            string[,] fieldValueToSet = 
            {
                { MA_MachineDelayNewDetailsPage.constDelayType, @"Meal Time" },
                { MA_MachineDelayNewDetailsPage.constVoyage, @"MSCK000002 - MSC KATYA R." },
                { MA_MachineDelayNewDetailsPage.constDelayEstimatedEnd, "|" }
            };
            machineDelayDetailsPage.SetFields(fieldValueToSet);
            var delayStartDateTime = machineDelayDetailsPage.GetEnteredDateTime(MA_MachineDelayNewDetailsPage.constDelayStart);
            //var delayEstEndDateTime = machineDelayDetailsPage.GetEnteredDateTime(MA_MachineDelayNewDetailsPage.constDelayEstimatedEnd);
            machineDelayDetailsPage.DoSave();

            WarningErrorPopup warningErrorPopup = new WarningErrorPopup(TestContext);

            string[] warningErrorToCheck =
            {
               @"Code: 82425 The Estimated End Date is expected to be after the Start Date."
            };
            warningErrorPopup.CheckMessage(WarningErrorPopup.constError, warningErrorToCheck);
            warningErrorPopup.DoCancel();

        }

        

    }
}
