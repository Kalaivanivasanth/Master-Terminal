using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Delays;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Delays
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37580 : MobileAppsBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
      
        [ClassCleanup]
        public new static void ClassCleanUp() =>WebBase.ClassCleanUp();

        [TestInitialize]
        public new void TestInitialize() {}

        void MTNInitialize()
        {
            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void AddedBadWeatherDelay()
        {
            MTNInitialize();

            var currentTimeStamp = DateTime.Now;
            
            // Step 2
            HomePage.ClickTile(HomePage.BtnDelays);

            // Step 3
            MA_DelaysPage delaysPage = new MA_DelaysPage(TestContext);

            // Step 4
            delaysPage.DoVoyage();

            // Step 5
            MA_DelaysVoyagePage delayVoyagePage = new MA_DelaysVoyagePage(TestContext);
            delayVoyagePage.AddDelayToVoyage(TT1.Voyage.MSCK000002);

            // Step 6
            MA_MachineDelaysPage machineDelays = new MA_MachineDelaysPage(TestContext);
            machineDelays.DoAddNewDelay();

            MA_MachineDelayNewDetailsPage machineDelayDetailsPage = new MA_MachineDelayNewDetailsPage(TestContext);

            currentTimeStamp = currentTimeStamp.AddMinutes(60);
            string[,] fieldValueToSet =
            {
                { MA_MachineDelayNewDetailsPage.constDelayType, @"Bad Weather" },
                {
                    MA_MachineDelayNewDetailsPage.constDelayEstimatedEnd,
                    currentTimeStamp.ToString(@"dd/MM/yyyy") + "|" + currentTimeStamp.ToString("HH:mm")
                }
            };
            machineDelayDetailsPage.SetFields(fieldValueToSet);
            var delayStartDateTime = machineDelayDetailsPage.GetEnteredDateTime(MA_MachineDelayNewDetailsPage.constDelayStart);
            var delayEstEndDateTime = machineDelayDetailsPage.GetEnteredDateTime(MA_MachineDelayNewDetailsPage.constDelayEstimatedEnd);
            machineDelayDetailsPage.DoSave();

            string[]  fieldValueToCheck = 
            {
                $"{TT1.Voyage.MSCK000002} - MSC KATYA R.",
                "Bad Weather",
                "Start " + delayStartDateTime.ToString("dd/MM/yyyy h:mm tt").ToLower(),
                "Est End " + delayEstEndDateTime.ToString("dd/MM/yyyy h:mm tt").ToLower()
            };

            // Step 9
            var returnedValue = machineDelays.FindDetailsOnPage(fieldValueToCheck, false);
            Assert.IsFalse(returnedValue != null, @"TestCase37219 - The following details were not deleted:\n" +
                    string.Concat(fieldValueToCheck));

        }

        

    }
}
