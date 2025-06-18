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
    public class TestCase40416 : MTNBase
    {
        EDIOperationsForm ediOperationsForm;

        protected static string ediFile1 = "M_40416_COPARN.edi";
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_40416_";
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
                "UNA:+,'\nUNB+UNOA:1+COSCO+NZWLG+180508:0410+00667'\nUNG+COPARN+COSCO+NZWLG+180508:0410+00667+UN+D:95B'\nUNH+6670000001+COPARN:D:95B:UN'\nBGM+11+61754812602+5'\nRFF+BN:6175481260'\nTDT+20+40416A+1++MSL:172:166+++9628348:146::JOLLY BIANCO'\nRFF+VON:40416A'\nLOC+88+NZWLG:139:6'\nDTM+132:20180525:203'\nDTM+133:20180518:203'\nNAD+SH++360 LOGISTICS GROUP LTD (AKL)'\nNAD+CA+MSL:172:ZZZ'\nGID+1'\nEQD+CN++4510++2+5'\nRFF+BN:6175481260'\nRFF+SQ:1.000000'\nEQN+2'\nTMD+3++2'\nLOC+9+NZWLG:139:6'\nLOC+11+AUBNE:139:6'\nLOC+8+AUBNE:139:6'\nLOC+98+WLG11:139:6:CENTREPORT CONTAI'\nMEA+AAE+G+KGM'\nMEA+AAE+T+KGM:22000'\nMEA+AAE+EGW+KGM:22000'\nMEA+AAE++KGM:22000'\nMEA+AAE++KGM:22000'\nFTX+AAA+++CORN CHIPS'\nTDT+1'\nCNT+16:1'\nUNT+29+6670000001'\nUNE+1+00667'\nUNZ+1+00667'");

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void CoparnVoyageTranslation2()
        {

            MTNInitialize();
            
            /*
            To test that if a voyage does not have Lloyds Number or Call Sign, the voyage translation finds the correct voyage to apply changes through EDI file.
            Two Vessels - one with lloyds number and Call Sign, one without Llyods Number and Call sign - vessel definitions attached
            One voyage created for each of the two vessels - BISL000001 and BIAN000001 created 
            EDI Data Translations for each of the voyage, with same Foreign In Code and same Foreign Out Code - created Translation Type - Voyage for Operator - MSL, Terminal - TT1
            EDI file - used customer's edi file and edited the TDT+20 section to specify voyage and vessel used in this test 
            */

            // 1. Open EDI Operations
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // 1b. Delete Existing EDI messages relating to this test.
            ediOperationsForm.DeleteEDIMessages(@"Gate Document", @"40416", @"Loaded");

            // 2. Load COPARN file
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);

            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile1);
            ediOperationsForm.ClickEDIDetailsTab();
            ediOperationsForm.GetTabTableGeneric(@"Booking Header", @"4036");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Voyage^BIAN000001", clickType: ClickType.None, rowHeight: 16);

           
   
        }

    }

}
