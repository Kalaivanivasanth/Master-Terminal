using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Delays;
using System;
using MTNGlobal.EnumsStructs;
using DataObjects.LogInOutBO;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Delays
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37582 : MobileAppsBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        void MTNtInitialize()
        {
            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void AddedBreakdownDelay()
        {
            MTNtInitialize();

            var currentTimeStamp = DateTime.Now;
            
            // Step 2 - Need to add a bad weather delay as can't guarantee there is one there
            HomePage.ClickTile(HomePage.BtnDelays);

            MA_DelaysPage delaysPage = new MA_DelaysPage(TestContext);
            delaysPage.DoVoyage();

            MA_DelaysVoyagePage delayVoyagePage = new MA_DelaysVoyagePage(TestContext);
            delayVoyagePage.AddDelayToVoyage(@"MSCK000002");

            MA_MachineDelaysPage machineDelays = new MA_MachineDelaysPage(TestContext);
            machineDelays.DoAddNewDelay();

            MA_MachineDelayNewDetailsPage machineDelayDetailsPage = new MA_MachineDelayNewDetailsPage(TestContext);

            currentTimeStamp = currentTimeStamp.AddMinutes(60);
            string[,] fieldValueToSet =
            {
                { MA_MachineDelayNewDetailsPage.constDelayType, @"Breakdown" },
                { MA_MachineDelayNewDetailsPage.constMachine, @"Portainer Crane - CRN2" },
                {
                    MA_MachineDelayNewDetailsPage.constDelayEstimatedEnd,
                    currentTimeStamp.ToString(@"dd/MM/yyyy") + "|" + currentTimeStamp.ToString("HH:mm")
                }
            };
            machineDelayDetailsPage.SetFields(fieldValueToSet);
            var delayStartDateTime = machineDelayDetailsPage.GetEnteredDateTime(MA_MachineDelayNewDetailsPage.constDelayStart);
            var delayEstEndDateTime = machineDelayDetailsPage.GetEnteredDateTime(MA_MachineDelayNewDetailsPage.constDelayEstimatedEnd);
            machineDelayDetailsPage.DoSave();

            // Step 7
            machineDelays = new MA_MachineDelaysPage(TestContext);

            string[] fieldValueToCheck =  
            {
                @"CRN2 - Portainer Crane - CRN2",
                @"Breakdown",
                @"Start " + delayStartDateTime.ToString("dd/MM/yyyy h:mm tt").ToLower(),
                @"Est End " + delayEstEndDateTime.ToString("dd/MM/yyyy h:mm tt").ToLower()
            };

            machineDelays.FindDetailsOnPage(fieldValueToCheck);
          

        }

        

    }
}
