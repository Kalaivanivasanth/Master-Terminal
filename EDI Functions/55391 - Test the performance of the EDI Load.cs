using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System.IO;
using System.Threading.Tasks;
using System;
using FlaUI.Core.Input;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    //[TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase55391 : MTNBase
    {

        private EDIOperationsForm _ediOperationsForm;
        private const string TestCaseNumber = @"55391";
        private const string filename1 = @"55391\D_55391A.csv";
        private const string filename2 = @"55391\M_55391A.csv";

        bool _timerFailed = false;
        bool _verifyFailed = false;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {

            searchFor = @"_" + TestCaseNumber + "_";

            // Start Master Terminal
            BaseClassInitialize(testContext);

            // Signon Master Terminal
            signonForm = new SignonPageObject();
            signonForm.Signon(testContext);
            signonForm.ClickSaveButton();

        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }


        [TestMethod]
        public void TestThePerformanceOfEdiLoad()
        {
           

            // 1. Open EDI Operations
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // 2. Delete any Existing EDI messages relating to this test.
            _ediOperationsForm.DeleteEDIMessages(@"Cargo On Site", TestCaseNumber, @"Loaded");

            // 3. Load the csv file from the directory to delete the cargo on site
            _ediOperationsForm.LoadEDIMessageFromFile55391(filename1, specifyType: true, fileType: @"55391_CargoOnSite");
            _ = TimingTest2();
           /* _ediOperationsForm.ChangeEDIStatus55391(filename1, @"Loaded", @"Verify");
            _ediOperationsForm.ChangeEDIStatus55391(filename1, @"Verified", @"Load To DB");

            // 4. Load the csv file from the directory to create the cargo on site
            _ediOperationsForm.LoadEDIMessageFromFile55391(filename2, specifyType: true, fileType: @"55391_CargoOnSite");
            _ediOperationsForm.ChangeEDIStatus55391(filename2, @"Loaded", @"Verify");
            _ediOperationsForm.ChangeEDIStatus55391(filename2, @"Verify warnings", @"Load To DB");*/
        }

        public async Task TimingTest2()
        {
            Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}*------- Starting TimingTest2 ------");
            Console.WriteLine($"Verify time < timer time");
            //var testStartTime = DateTime.Now;
            //Console.WriteLine($"TimingTest starting.....  {testStartTime}");

            Task[] tasksToRun = new Task[2];

            Task timerTask = Task.Factory.StartNew(() => TimerTask());
            tasksToRun[0] = timerTask;
            Task mainTask = Task.Factory.StartNew(() => VerifyFile());
            tasksToRun[1] = mainTask;


            await Task.WhenAny(tasksToRun);

            //var testEndTime = DateTime.Now;
            //Console.WriteLine($"TimingTest ending.....  {testEndTime}");
            //Console.WriteLine($"TimingTest: difference: {testEndTime.Subtract(testStartTime).TotalMinutes}");

            Console.WriteLine($"TimingTest2: _verifyFailed {_verifyFailed}");
            Console.WriteLine($"TimingTest2: _timerFailed {_timerFailed}");

            Console.WriteLine($"*------- Finishing TimingTest2 ------{Environment.NewLine}{Environment.NewLine}");
        }
        void VerifyFile()
        {
            Console.WriteLine("*------- Starting VerifyFile ------");
            Console.WriteLine($"VerifyFile: runTime: ");

            //var vfStartTime = DateTime.Now;
            //Console.WriteLine($"vfStartTime: {vfStartTime}");

            //Wait.UntilInputIsProcessed(TimeSpan.FromMinutes(runTime));
            _ediOperationsForm.ChangeEDIStatus55391(filename1, @"Loaded", @"Verify");
            //var vfEndTime = DateTime.Now;
            //Console.WriteLine($"ending.....  {vfEndTime}");
            //Console.WriteLine($"difference: {vfEndTime.Subtract(vfStartTime).TotalMinutes}");

            _verifyFailed = _timerFailed;

            Console.WriteLine($"VerifyFile: _timerFailed: {_timerFailed}");
            Console.WriteLine("*------- Finishing VerifyFile ------");

        }


        void TimerTask()
        {
            Console.WriteLine("*------- Starting TimerTask ------");
            Console.WriteLine($"TimerTask: runTime: 10");

            //DateTime startTime = DateTime.Now;
            //Console.WriteLine($"startTime: {startTime}");

            Wait.UntilInputIsProcessed(TimeSpan.FromMinutes(10));

            //DateTime endTime = DateTime.Now;
            //Console.WriteLine($"endTime: {endTime}");
            //Console.WriteLine($"difference: {endTime.Subtract(startTime).TotalMinutes}");

            _timerFailed = _verifyFailed = true;
            Console.WriteLine($"TimerTask: _timerFailed: {_timerFailed}");
            Console.WriteLine($"*------- Finishing TimerTask ------");
        }
    }

}
