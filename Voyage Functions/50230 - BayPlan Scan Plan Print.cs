using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Print_Preview;
using MTNUtilityClasses.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase50230 : MTNBase
    {

        const string TestCaseNumber = "50230";
        const string SubDirectory = TestCaseNumber;
        private string[] _detailsToMove;
        List<string> _fileNamesToCompare = new List<string>();

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        void MTNInitialize()
        {
            searchFor = "_" + TestCaseNumber + "_";
            Miscellaneous.SetDefaultPrinter(DefaultPrinterName);
            LogInto<MTNLogInOutBO>();
            GenerateImagesOnly = TestContext.GetRunSettingValue(@"RunType").Equals("Manual") &&
                                 Convert.ToBoolean(TestContext.GetRunSettingValue(@"GenerateNewImagesOnly"));
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            Miscellaneous.SetDefaultPrinter(defaultPrinter);
            base.TestCleanup();
        }


        [TestMethod]
        public void ValidateBayPlanScanPlanPrinting()
        {
            MTNInitialize();

            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|LOLO Planning");
            LOLOPlanningForm loloPlanning = new LOLOPlanningForm();

            loloPlanning.btnPrintScanPlan.DoClick();

            ScanPlanReportForm scanPlanReportForm = new ScanPlanReportForm(@"Scan Plan Report TT1");

            // Set the stuff that doesn't change
            //MTNControlBase.SendTextToCombobox(scanPlanReportForm.cmbVoyage, @"PTN010001 - PONL TARANAKI");
            scanPlanReportForm.cmbVoyage.SetValue(TT1.Voyage.PTN010001);
            scanPlanReportForm.chkPrintPreview.DoClick();
            scanPlanReportForm.chkPrintCargoList.DoClick(false);

            scanPlanReportForm.GetBaysList();

            _detailsToMove = new []
            {
                @"Bay 01",
                @"Bay 03",
                @"Bay 33",
                @"Bay 35",
                @"Bay 37",
                @"Bay 39",
                @"Bay 41",
                @"Bay 43",
                @"Bay 45",
                @"Bay 47",
                @"Bay 49"
            };
            scanPlanReportForm.lstBays.MoveItemsBetweenList(scanPlanReportForm.lstBays.LstLeft, _detailsToMove);

            // BPB 1; L 1st; SC DP; SC A; ROB G;  Bays 01, 03, 33, 35, 37, 39, 41, 43, 45, 47, 49
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 12 pages", @"1", 12,
                scanPlanReportForm.optOne, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // BPB 2; L 1st; SC DP; SC A; ROB G;  Bays 01, 03, 33, 35, 37, 39, 41, 43, 45, 47, 49
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 7 pages", @"2", 7,
                scanPlanReportForm.optTwo, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // BPB 3; L 1st; SC DP; SC A; ROB G;  Bays 01, 03, 33, 35, 37, 39, 41, 43, 45, 47, 49
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 5 pages", @"3", 5,
                scanPlanReportForm.optThree, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // BPB 2x3; L 1st; SC DP; SC A; ROB G;  Bays 01, 03, 33, 35, 37, 39, 41, 43, 45, 47, 49
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 3 pages", @"4", 3,
                scanPlanReportForm.opt2X3, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);
            
            // BPB 3x4; L 1st; SC DP; SC A; ROB G;  Bays 01, 03, 33, 35, 37, 39, 41, 43, 45, 47, 49
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 3 pages", @"5", 2,
                scanPlanReportForm.opt3X4, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            _detailsToMove = new []
            {
                @"Bay 33",
                @"Bay 35",
                @"Bay 37",
                @"Bay 39",
                @"Bay 41",
                @"Bay 43",
                @"Bay 45",
                @"Bay 47",
                @"Bay 49"
            };
            scanPlanReportForm.lstBays.MoveItemsBetweenList(scanPlanReportForm.lstBays.LstRight, _detailsToMove, false);

            // BPB 1; L 1st; SC N; SC A; ROB G;  Bays 01, 03
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 3 pages", @"6", 3,
                scanPlanReportForm.optOne, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optNone, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // BPB 1; L 1st; SC O; SC A; ROB G;  Bays 01, 03
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 3 pages", @"7", 3,
                scanPlanReportForm.optOne, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optOperator, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // BPB 1; L 1st; SC C; SC A; ROB G;  Bays 01, 03
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 3 pages", @"8", 3,
                scanPlanReportForm.optOne, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optCommodity, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // BPB 1; L 1st; SC I; SC A; ROB G;  Bays 01, 03
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 3 pages", @"9", 3,
                scanPlanReportForm.optOne, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optISOType, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // BPB 1; L 1st; SC LP; SC A; ROB G;  Bays 01, 03
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 3 pages", @"10", 3,
                scanPlanReportForm.optOne, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optLoad, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // easiest way to do this is to close the form and reopen
            scanPlanReportForm.btnCancel.DoClick();

            loloPlanning.SetFocusToForm();
            loloPlanning.btnPrintScanPlan.DoClick();

            scanPlanReportForm = new ScanPlanReportForm(@"Scan Plan Report TT1");

            // Set the stuff that doesn't change
            //MTNControlBase.SendTextToCombobox(scanPlanReportForm.cmbVoyage, @"PTN010001 - PONL TARANAKI");
            scanPlanReportForm.cmbVoyage.SetValue(TT1.Voyage.PTN010001);
            scanPlanReportForm.chkPrintPreview.DoClick();

            scanPlanReportForm.GetBaysList();

            _detailsToMove = new []
            {
                @"Bay 01",
                @"Bay 03",
                @"Bay 43",
                @"Bay 45"
            };
            scanPlanReportForm.lstBays.MoveItemsBetweenList(scanPlanReportForm.lstBays.LstLeft, _detailsToMove);

            // BPB 1; L 1st; SC DP; SC P; ROB G;  Bays 01, 03, 43, 45
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 5 pages", @"11", 5,
                scanPlanReportForm.optOne, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optProposed, scanPlanReportForm.optGrey);

            // BPB 1; L 1st; SC DP; SC L; ROB G;  Bays 01, 03, 43, 45
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 5 pages", @"12", 5,
                scanPlanReportForm.optOne, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optLoad, scanPlanReportForm.optGrey);

            // BPB 1; L 1st; SC DP; SC D; ROB G;  Bays 01, 03, 43, 45
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 5 pages", @"13", 5,
                scanPlanReportForm.optOne, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optDischarge, scanPlanReportForm.optGrey);

            // BPB 1; L 1st; SC DP; SC DL; ROB G;  Bays 01, 03, 43, 45
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 5 pages", @"14", 5,
                scanPlanReportForm.optOne, scanPlanReportForm.optFirstPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optDischargeAndLoad, scanPlanReportForm.optGrey);

            // BPB 1; L E; SC DP; SC A; ROB G;  Bays 01, 03, 43, 45
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 4 pages", @"15", 4,
                scanPlanReportForm.optOne, scanPlanReportForm.optEveryPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // BPB 2; L E; SC DP; SC A; ROB G;  Bays 01, 03, 43, 45
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 2 pages", @"16", 2,
                scanPlanReportForm.optTwo, scanPlanReportForm.optEveryPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // BPB 3; L E; SC DP; SC A; ROB G;  Bays 01, 03, 43, 45
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 2 pages", @"17", 2,
                scanPlanReportForm.optThree, scanPlanReportForm.optEveryPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // BPB 2x3; L E; SC DP; SC A; ROB G;  Bays 01, 03, 43, 45
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 1 pages", @"18", 1,
                scanPlanReportForm.opt2X3, scanPlanReportForm.optEveryPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // BPB 3x4; L E; SC DP; SC A; ROB G;  Bays 01, 03, 43, 45
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 1 pages", @"19", 1,
                scanPlanReportForm.opt3X4, scanPlanReportForm.optEveryPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // easiest way to do this is to close the form and reopen
            scanPlanReportForm.btnCancel.DoClick();

            loloPlanning.SetFocusToForm();
            loloPlanning.btnPrintScanPlan.DoClick();

            scanPlanReportForm = new ScanPlanReportForm(@"Scan Plan Report TT1");

            // Set the stuff that doesn't change
            //MTNControlBase.SendTextToCombobox(scanPlanReportForm.cmbVoyage, @"PTN010001 - PONL TARANAKI");
            scanPlanReportForm.cmbVoyage.SetValue(TT1.Voyage.PTN010001);
            scanPlanReportForm.chkPrintPreview.DoClick();
            scanPlanReportForm.chkPrintCargoList.DoClick();

            scanPlanReportForm.GetBaysList();

            _detailsToMove = new []
            {
                @"Bay 01",
                @"Bay 03",
                @"Bay 43",
                @"Bay 45",
                @"Bay 47",
                @"Bay 49",
            };
            scanPlanReportForm.lstBays.MoveItemsBetweenList(scanPlanReportForm.lstBays.LstLeft, _detailsToMove);

            // BPB 3x4; L E; SC DP; SC A; ROB W;  Bays 01, 03, 43, 45, 47, 49
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 7 pages", @"20", 7,
                scanPlanReportForm.optOne, scanPlanReportForm.optEveryPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optWhite);


            // BPB 3x4; L E; SC DP; SC A; ROB W;  Bays 01, 03, 43, 45, 47, 49  PCL
            scanPlanReportForm.chkPrintCargoList.DoClick();
            SetValuesAndPrintPages(scanPlanReportForm, @"Preview of printed page 1 of 10 pages", @"21", 7,
                scanPlanReportForm.optOne, scanPlanReportForm.optEveryPage,
                scanPlanReportForm.optDischargePort, scanPlanReportForm.optActual, scanPlanReportForm.optGrey);

            // Need to do to report at some stage once we work out how to get past date / time issue

            scanPlanReportForm.btnCancel.DoClick();
            
            CompareImages();
            
        }

        private void SetValuesAndPrintPages(ScanPlanReportForm scanPlanReportForm, string reportName, string testNumber, int numberOfPages,
            AutomationElement bpbOption, AutomationElement legendOption, AutomationElement slotColourOption, AutomationElement slotContentOption, 
            AutomationElement robOption)
        {
            MTNControlBase.SetValue(bpbOption);
            MTNControlBase.SetValue(legendOption);
            MTNControlBase.SetValue(slotColourOption);
            MTNControlBase.SetValue(slotContentOption);
            MTNControlBase.SetValue(robOption);
            
            Miscellaneous.CaptureElementAsImage(TestContext, scanPlanReportForm.GetForm(), $"ScanPlanReportForm_Print_{testNumber}.png",
                subDirectory: SubDirectory, generateToCompareDirectory: GenerateImagesOnly);
            
            scanPlanReportForm.btnPrint.DoClick();

            
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(10));
            PrintPreviewJadeForm printPreviewJadeForm = new PrintPreviewJadeForm(reportName);

            printPreviewJadeForm.btnReduce.DoClick();

            for (int page = 1; page <= numberOfPages; page++)
            {
                string fileName = @"50230_Test" + testNumber + "_Page" + page + ".png";
                _fileNamesToCompare.Add(fileName);
                Miscellaneous.CaptureElementAsImage(TestContext, printPreviewJadeForm.rptPane, fileName,
                    subDirectory: SubDirectory, generateToCompareDirectory: GenerateImagesOnly);

                /*if (!TestContext.GetRunSettingValue(@"GetNewImages").Equals("true"))
                {
                    CompareImages(fileName + ".png");
                }*/

                printPreviewJadeForm.btnNextPage.DoClick();

            }

            printPreviewJadeForm.btnCancel.DoClick();
          

            scanPlanReportForm.SetFocusToForm();
        }


        private void CompareImages()
        {
          
            string compareDirectory = Miscellaneous.GetCreateSaveCompareDirectoryName(TestContext, false,  SubDirectory);
            string savedDirectory = Miscellaneous.GetCreateSaveCompareDirectoryName(TestContext, subDirectory: SubDirectory);
            
            //Trace.TraceInformation("fileName / Path - Actual: {0}    expected: {1}", (savedDirectory + actualFilename),
            //    (compareDirectory + expectedFilename));

            string erroneousImages = null;
            foreach (var fileName in _fileNamesToCompare)
            {
                Trace.TraceInformation($"fileName: {fileName}");
                Image png = Image.FromFile(savedDirectory + fileName);
                Bitmap actual = new Bitmap(png);

                png = Image.FromFile(compareDirectory + fileName);
                Bitmap expected = new Bitmap(png);

                var equal = CompareBitmapsSlow(actual, expected);

                if (!equal)
                {
                    erroneousImages += fileName + Environment.NewLine;
                }


            }

            Assert.IsTrue(string.IsNullOrEmpty(erroneousImages),
                $"The following images don't match:{Environment.NewLine}{erroneousImages}");
         
        }

        private bool CompareBitmapsSlow(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return false;
            if (object.Equals(bmp1, bmp2))
                return true;
           

            bool imagesSame = true;
      
            for (int column = 0; column < bmp1.Width; column++)
            {
                if (column > 961)
                {
                    break;
                }
                
                for (int row = 0; row < bmp1.Height; row++)
                {
                    if (row > 734)
                    {
                        break;
                    }

                    if (!bmp1.GetPixel(column, row).Equals(bmp2.GetPixel(column, row)))
                    {
                        Trace.TraceInformation("difference: fileName: {0}    x: {1}    y: {2}", "", column, row);
                        imagesSame = false;
                    }
                }
            }

            return imagesSame;
        }
    }

}
