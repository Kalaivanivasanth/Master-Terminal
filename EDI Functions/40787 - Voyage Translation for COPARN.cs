using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40787 : MTNBase
    {

        EDIOperationsForm ediOperationsForm;
        
        protected static string ediFile1 = "M_40787_COPARN.edi";
    

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_40787_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //create COPARN file
            CreateDataFile(ediFile1,
                "UNA:+.'\nUNB+UNOA:2+HAPAG-LLOYD+ITSALSCT+180904:0743+54176'\nUNH+CANITSAL711081+COPARN:D:95B:UN:SMDG10'\nBGM+11:::GATE IN ORDER+82004167-004+9'\nFTX+OSI++L'\nRFF+BN:82004167-004'\nTDT+20+40787+1++HLC:172:166+++PBDA:103::Bermuda islander'\nRFF+VON:218970'\nLOC+9+ITSAL:139:6++SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nDTM+133:201810060800:203'\nDTM+132:201810221900:203'\nNAD+CF+HLC:160:166'\nGID+1'\nFTX+AAA+++HIGH PERFORMANCE STE'\nSGP+TCLU6194637'\nEQD+CN+TCLU6194637+22G1:102:5++2+5'\nRFF+BN:82004167-0004'\nEQN+1'\nTMD+3++2'\nLOC+9+ITSAL:139:6+SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nLOC+11+NZAKL:139:6+:TER:ZZZ:MGT - POM SECTION 62 - RACINE'\nLOC+8+CACAL:139:6'\nLOC+7+CACAL:139:6'\nMEA+AAE+T+KGM:2200'\nMEA+AAE+G+KGM:4000'\nTDT+10+015N+1+13++++NSW:103::NO SAIL WEEK'\nLOC+5+GRPIR:139:6'\nDTM+133:201809230900:203'\nDTM+132:201809292300:203'\nNAD+CF+HLC:160:166'\nNAD+CU+219959:ZZZ++CARGO PARTNERS++NOVI SAD++21000'\nCNT+16:1'\nUNT+31+CANITSAL711081'\nUNZ+1+54176'");

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void COPARNTranslation()
        {
            
            MTNInitialize();

            // set configuration as required by test
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"EDI", @"Use EDI Booking Release Req rather than EDI Gate Document", @"0", rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();

            // 1. Open EDI Operations and create the cargo on-site - first delete then create 
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);

            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperationsForm.DeleteEDIMessages(@"Gate Document", @"40787",ediStatus: @"Loaded");
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Loaded", @"Verify");

            /*ediOperationsForm.tabEDIDetails.Click();
            MTNControlBase.FindTabOnForm(ediOperationsForm.tabEDIDetails, @"Booking Header");
            ediOperationsForm.ShowEDIDataTable(@"4036");*/
            ediOperationsForm.ClickEDIDetailsTab();
            ediOperationsForm.GetTabTableGeneric(@"Booking Header", @"4036");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^82004167-0004~Operator^HLL~Voyage^VOY_40787~Destination^CACAL~Discharge Port^NZAKL~Vessel^Bermuda islander", clickType: ClickType.None, searchType: SearchType.Exact);

            /*MTNControlBase.FindTabOnForm(ediOperationsForm.tabEDIDetails, @"Prenote");
            ediOperationsForm.ShowEDIDataTable(@"6166");*/
            //ediOperationsForm.GetTabTableGeneric(@"Prenote", @"6188");
            ediOperationsForm.GetTabTableGeneric(@"Prenote");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ISO Type^22G1~Cargo Type Descr^ISO Container~ID^TCLU6194637~Transport Mode^Road~IMEX Status^Export~Operator^HLL~Discharge Port^NZAKL~Voyage^VOY_40787~Cmdty^GEN~Booking^82004167-0004~Destination^CACAL", clickType: ClickType.None, searchType: SearchType.Exact);

        }




    }

}
