using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNUtilityClasses.Classes;
using System;
using MTNForms.FormObjects;
using DataObjects.LogInOutBO;
using DataObjects;
using FlaUI.Core.Input;
using MTNDesktopFlaUI.Classes;
using System.Collections.Generic;
using System.Diagnostics;
using FlaUI.Core.AutomationElements;
using System.Linq;
using HardcodedData.SystemData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace PERFAutomationTests.TestCases.Performance_Tests
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase60969 : MTNBase
    {
        static string _mark;
        string timings = null;
        string[] userSignons = 
            {
               "PRFSRCH1^95 EUP Columns",
               "MKSRCH2^All 385 Columns",
            };
        string[] userDetails;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _mark = millisecondsSince20000101.ToString();
            searchFor = @"_60969_";
            BaseClassInitialize_New(testContext);
        }

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize() 
        { 
            TestRunDO.GetInstance().SetKillBPGToFalse();
            
        }

        [TestMethod]
        public void CargoSearchPerformanceTest()
        {
            MTNInitialize();

            foreach (string user in userSignons)
            {
                userDetails = user.Split('^');
                LogInto<MTNLogInOutBO>(userDetails[0]);

                DoSearch(@"ABX");
                DoSearch(@"AA");
                DoSearch(@"A");

                if (userDetails[0] != @"PRFSRCH1") LogOffMTN();
            }

        }

        public void DoSearch(string cargoID)
        {
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            //cargoEnquiryForm = new CargoEnquiryForm();
            cargoEnquiryForm = new CargoEnquiryForm();

            // Enter Search criteria
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Blank, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", cargoID);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Active Voyages", @"MESDIAMZ");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnShip, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            
            // Start Timer and cargo search
            var startTime = DateTime.Now;
            cargoEnquiryForm.DoSearch();
            TimeSpan elapsedTime;

            // Check if search is complete and data is available in data table
            //Retry.WhileFalse(() => IsDataAvailable(), TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1), ignoreException: true);
            while (true)
            {
                elapsedTime = DateTime.Now - startTime;
                //Console.WriteLine($"elapsedTime: {elapsedTime}");
                if (elapsedTime.TotalMinutes > 15) break;

                try
                {
                    cargoEnquiryForm.DoSelectAll();
                    break;
                }
                catch (Exception e)
                {
                    //Console.WriteLine($"exception: {e}");
                }
            }

            elapsedTime = DateTime.Now - startTime;
            Wait.UntilResponsive(cargoEnquiryForm.tblData2.GetElement(), elapsedTime);
            
            timings = timings + "*************************************** \n" +
                "Columns selected: " + userDetails[1] + ", \n" +
                "Items retrieved: " + GetRowCount() + ", \n" +
                "Total items on voyage: 15000 \n" +
                "Time taken: " + elapsedTime.ToString(@"hh\:mm\:ss") + "\n *********************************************";
            Console.WriteLine(timings);

            // Close Cargo Enquiry form
            try
            {
                Wait.UntilResponsive(cargoEnquiryForm.GetForm());
                cargoEnquiryForm.CloseForm();
            }
            catch { }

        }

        public int GetRowCount()
        {
            int rowCount = -1;

            List<DataGridViewRow> rowsInTable =
                new List<DataGridViewRow>
                (from DataGridViewRow r in cargoEnquiryForm.tblData2.GetElement().AsDataGridView()?.Rows
                 select r);

            rowCount = rowsInTable.Count();

            return rowCount;
        }

    }
}
