using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase42202 : MTNBase
    {
        EDIOperationsForm ediOperationsForm;
        protected static string ediFile1 = "M_42202_COREOR.edi";
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_42202_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //Create CUSCAR EDI file
            CreateDataFile(ediFile1,
                "UNB+UNOC:2+LENAVI-GOA+ITSALSCT+181120:1054+000000012'\nUNH+DO134792+COREOR:D:08B:UN:SMDG20'\nBGM+129+MEDUCO308182+9'\nFTX+AAI+++0'\nRFF+RE:134792'\nTDT+20+AT847A+1++MSC:172:20+++9141780:146::MSC GIANNINA'\nLOC+11+ITSAL:139:6:Salerno+ITSALSC:ZZZ:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nNAD+MS+MSC:172'\nNAD+FW+IT07853020639:160++SCETTA P/C FORNAROLI '\nNAD+CB+IT07853020639:160++SCETTA P/C FORNAROLI '\nEQD+CN+JLG42202A01+4510+++5'\nDTM+36:20181130:203'\nMEA+AAE+G+KGM:25079'\nMEA+AAE+T+KGM:3750'\nFTX+MER+++20181130'\nEQD+CN+JLG42202A02+4510+++5'\nDTM+36:20181130:203'\nMEA+AAE+G+KGM:25328'\nMEA+AAE+T+KGM:3890'\nFTX+MER+++20181130'\nCNT+16:2'\nUNT+21+DO134792'\nUNH+DO134791+COREOR:D:08B:UN:SMDG20'\nBGM+129+MEDUCO305600+9'\nFTX+AAI+++0'\nRFF+RE:134791'\nTDT+20+AT847A+1++MSC:172:20+++9141780:146::MSC GIANNINA'\nLOC+11+ITSAL:139:6:Salerno+ITSALSC:ZZZ:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nNAD+MS+MSC:172'\nNAD+FW+IT07853020640:160++LAMBERTI P/C SCT '\nNAD+CB+IT07853020640:160++LAMBERTI P/C SCT '\nEQD+CN+JLG42202A03+4510+++5'\nDTM+36:20181130:203'\nMEA+AAE+G+KGM:19516'\nMEA+AAE+T+KGM:3700'\nFTX+MER+++20181130'\nCNT+16:1'\nUNT+16+DO134791'\nUNZ+3+000000012'\n\n");

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void CoreorFreightForwarderTranslation()
        {
            
            MTNInitialize();

            /*
            To test that when loading a COREOR EDI file the containers freight forwarder code updates correctly.
            */

            // 1. Open EDI Operations
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // 1b. Delete Existing EDI messages relating to this test.
            ediOperationsForm.DeleteEDIMessages(@"Operator Stops", @"42202");

            // 2. Load COPARN file
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);

            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile1);
            ediOperationsForm.ClickEDIDetailsTab();
            ediOperationsForm.GetTabTableGeneric(@"Operator Stops", @"4036");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^JLG42202A02~Cargo Type Descr^ISO Container~MT^F~Entry Number^134792~Vessel^MSC GIANNINA~Voyage^AT847A~Freight Forwarder Code^ABCNE", clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^JLG42202A01~Cargo Type Descr^ISO Container~MT^F~Entry Number^134792~Vessel^MSC GIANNINA~Voyage^AT847A~Freight Forwarder Code^ABCNE", clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^JLG42202A03~Cargo Type Descr^ISO Container~MT^F~Entry Number^134791~Vessel^MSC GIANNINA~Voyage^AT847A~Freight Forwarder Code^ABCNR", clickType: ClickType.None);

        }

    }

}
