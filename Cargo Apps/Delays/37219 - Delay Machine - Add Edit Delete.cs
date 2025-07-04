using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Delays;
using System;
using DataObjects.LogInOutBO;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Delays
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37219 : MobileAppsBase
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
        public void DelayMachineAddEditDelete()
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
                { MA_MachineDelayNewDetailsPage.constDelayType, @"Damaged Container" },
                { MA_MachineDelayNewDetailsPage.constVoyage, @"MSCK000002 - MSC KATYA R." },
                {
                    MA_MachineDelayNewDetailsPage.constDelayEstimatedEnd,
                    currentTimeStamp.ToString(@"dd/MM/yyyy") + "|" + currentTimeStamp.ToString("HH:mm")
                }
            };
            machineDelayDetailsPage.SetFields(fieldValueToSet);
            var delayStartDateTime =
                machineDelayDetailsPage.GetEnteredDateTime(MA_MachineDelayNewDetailsPage.constDelayStart);
            var delayEstEndDateTime =
                machineDelayDetailsPage.GetEnteredDateTime(MA_MachineDelayNewDetailsPage.constDelayEstimatedEnd);
            machineDelayDetailsPage.DoSave();

            // Step 7
            machineDelays = new MA_MachineDelaysPage(TestContext);

            string[] fieldValueToCheck =
            {
                @"MSCK000002 - MSC KATYA R.",
                @"Damaged Container",
                @"Start " + delayStartDateTime.ToString("dd/MM/yyyy h:mm tt").ToLower(),
                @"Est End " + delayEstEndDateTime.ToString("dd/MM/yyyy h:mm tt").ToLower()
            };

            // Validate the remaining time
            machineDelays.ValidateRemainingTime(fieldValueToCheck);
            
            // Step 9
            machineDelays.ClickEdit(fieldValueToCheck);
            

            // Step 10
            MA_MachineDelayEditDetailsPage machineDelayEditDetailsPage = new MA_MachineDelayEditDetailsPage(TestContext);

            fieldValueToSet = new string[,]
            {
                { MA_MachineDelayEditDetailsPage.constDelayType, @"Bad Weather" },
            };
            machineDelayEditDetailsPage.SetFields(fieldValueToSet);
            
            machineDelayEditDetailsPage.DoSave();
            
            // Step 12
            machineDelays = new MA_MachineDelaysPage(TestContext);

            fieldValueToCheck = new string[]
            {
                @"MSCK000002 - MSC KATYA R.",
                @"Bad Weather",
                @"Start " + delayStartDateTime.ToString("dd/MM/yyyy h:mm tt").ToLower(),
                @"Est End " + delayEstEndDateTime.ToString("dd/MM/yyyy h:mm tt").ToLower()
            };

            // Step 9
            machineDelays.ClickEdit(fieldValueToCheck);

            machineDelayEditDetailsPage = new MA_MachineDelayEditDetailsPage(TestContext);
            machineDelayEditDetailsPage.DoDelete();
            machineDelayEditDetailsPage.DoConfirmDelete();

            machineDelays = new MA_MachineDelaysPage(TestContext);

            fieldValueToCheck = new string[]
            {
                @"MSCK000002 - MSC KATYA R.",
                @"Bad Weather",
                @"Start " + delayStartDateTime.ToString("dd/MM/yyyy h:mm tt").ToLower(),
                @"Est End " + delayEstEndDateTime.ToString("dd/MM/yyyy h:mm tt").ToLower()
            };

            // Step 9
            var returnedValue = machineDelays.FindDetailsOnPage(fieldValueToCheck, false);
            Assert.IsFalse(returnedValue != null, @"TestCase37219 - The following details were not deleted:\n" +
                    String.Concat(fieldValueToCheck));

        }

        

    }
}
