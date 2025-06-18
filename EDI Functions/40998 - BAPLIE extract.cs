using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNUtilityClasses.Classes;
using System;
using System.IO;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40998 : MTNBase
    {
        EDIOperationsForm ediOperationsForm;
        EDIExtractForm ediExtractForm;

        const string ediFile1 = "M_40998_BAPLIETEST.edi";
       

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            Miscellaneous.DeleteFile(string.Concat(dataDirectory, ediFile1));
            
            MTNSignon(TestContext);
        }


        [TestMethod]
        public void BaplieExtract()
        {
            MTNInitialize();
            
            // 1a. Open EDI Operations
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            //ediOperationsForm.btnExtractNew.DoClick();
            ediOperationsForm.DoExtractNew();

            ediExtractForm = new EDIExtractForm(@"EDI Extract TT1");
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Extract Type", @"Actual Bay Plan", rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Voyage", @"VOY_40998", rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Recipient", @"ABC	ABC CONTAINER LINE", rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Cargo Operator", @"MSL	Messina Line", rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Format", @"Baplie 2.2", rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"File Name (dbl click)", dataDirectory + @"M_40998_BAPLIETEST.edi");
            //ediExtractForm.btnSave.DoClick();
            ediExtractForm.DoSave();

            Wait.UntilInputIsProcessed(waitTimeout: TimeSpan.FromSeconds(5));
            string testMessage = File.ReadAllText(dataDirectory + ediFile1).Trim().Replace("\n", "").Replace("\r", "");
            string assertMessage1 = "UNB+UNOA:2+USJAX+ABC+";
            string assertMessage2 = "UNH+HIVOY_40998USJ+BAPLIE:D:95B:UN:SMDG22'";
            string assertMessage3 = "TDT+20+VOY_40998+++ABC:172:20+++HI:103:11:JOLLY DIAMANTE OFFICIAL'LOC+5+USJAX:139:6'LOC+61+:139:6'";
            string assertMessage4 = "LOC+147+0011082::5'GDS+APPL'FTX+AAA+++APPL'FTX+AAY++SHP+TEST TEAM:TESTER'FTX+AAY++SM1+1809201106:TEST TEAM,JADE LOGISTICS CHRISTCHURCH,,TEST.TEAM@JADELOGISTICS.COM:TESTER::'MEA+VGM++KGM:3628'LOC+9+USJAX'LOC+11+NZAKL'RFF+BM:1'EQD+CN+JLG40998A01+22G1++2+5'NAD+CA+MSL:172:20'UNT+20+HIVOY_40998USJ'";
            Assert.IsTrue(testMessage.Contains(assertMessage1), "The Baplie Extract file is not as expected");
            Assert.IsTrue(testMessage.Contains(assertMessage2), "The Baplie Extract file is not as expected");
            Assert.IsTrue(testMessage.Contains(assertMessage3), "The Baplie Extract file is not as expected");
            Assert.IsTrue(testMessage.Contains(assertMessage4), "The Baplie Extract file is not as expected");

        }

    

    }

}
