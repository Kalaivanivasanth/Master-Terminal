using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44032 : MTNBase
    {

        EDIOperationsForm ediOperationsForm;

        protected static string ediFile1 = "M_44032_CUSCAR.edi";
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_44032_";
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
                "UNB+UNOB:1+ICL+PENN+140408:1256+53669++++0++0'\nUNH+53669+CUSCAR:D:95B:UN:LOT10'\nBGM+785+UT_CAMIR+9'\nDTM+137:201404080000:102'\nRFF+SSX'\nNAD+MS+IILU:172:166'\nTDT+20+UT_CAMIR+1++IILU:172:166+++9306237:146::Independent Accord'\nLOC+60+NZPOE::139'\nDTM+132:201404150000:203'\nGIS+23'\nEQD+CN+ICUU2371952+22G1::5++3+5'\nMEA+AAE+G+KGM:19440'\nSEL+TBE495037'\nLOC+9+GBLIV::139'\nEQD+CN+ICUU2300394+22G1::5++3+5'\nMEA+AAE+G+KGM:10570'\nSEL+CMA-CGM366'\nLOC+9+GBLIV::139'\nCNI+8+MCBOL01'\nRFF+BM:MCBOL01'\nCNT+7:8090'\nCNT+8:50'\nLOC+9+GBLIV::139:LIVERPOOL GB'\nLOC+27+US::139:Tonawanda NY United States'\nGIS+23'\nTDT+30++1'\nNAD+CN++UOP Tonawanda / Tonawanda NY:175 E Park Drive--:Tonawanda:141510986:United States'\nNAD+CZ++UOP Tonawanda / Tonawanda NY:175 E Park Drive--:Tonawanda:141510986:United States'\nGID+1+50:SDR:::Steel Coils'\nFTX+AAA+++(AM # 36-12) CL 9,UN 3077,PKG III,ENVIRONMENTALLY HAZARDOUS SUBSTANCE,:SOLID, N.O.S.,CHEMICAL COMPOUND - COPPER METAL:POWDER'\nMEA+AAE+G+KGM:8090'\nSGP+ICUU2300394+50'\nDGS+IMD+9+3077'\nPCI+31'\nGID+1+16:IBC:::IBC'\nFTX+AAA+++TRIACETIN'\nMEA+AAE+G+KGM:16960'\nSGP+ICUU2371952+16'\nPCI+31'\nUNT+4135+53669'\nUNZ+1+53669'");

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void CuscarEdiLoadWithShipManifest()
        {
            
            MTNInitialize();

            // 1. Open EDI Operations
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // 1b. Delete Existing EDI messages relating to this test.
            ediOperationsForm.DeleteEDIMessages(@"Ship Manifest", @"44032", @"Loaded");

            // 2. Load CUSCAR file
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);

            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile1);

            ediOperationsForm.ClickEDIDetailsTab();
            //ediOperationsForm.GetTabTableGeneric(@"BOL Header", @"4036");
            ediOperationsForm.GetTabTableGeneric(@"BOL Header");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^MCBOL01", clickType: ClickType.None, rowHeight: 16);

           // ediOperationsForm.GetTabTableGeneric(@"BOL Details", @"6179", @"6178");
            ediOperationsForm.GetTabTableGeneric(@"BOL Details");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^MCBOL01~Cargo Type Descr^Motor Vehicle~Weight lbs^17835.412", clickType: ClickType.None, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^MCBOL01~Cargo Type Descr^Motor Vehicle~Weight lbs^37390.430", clickType: ClickType.None, rowHeight: 16);

            //ediOperationsForm.GetTabTableGeneric(@"Cargo On Ship", @"6188", @"6187");
            ediOperationsForm.GetTabTableGeneric(@"Cargo On Ship");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^UT_CAMIR/MCBOL01/001~Cargo Type Descr^Motor Vehicle", clickType: ClickType.None, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^ICUU2371952~Cargo Type Descr^ISO Container", clickType: ClickType.None, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^ICUU2300394~Cargo Type Descr^ISO Container", clickType: ClickType.None, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^UT_CAMIR/MCBOL01/002~Cargo Type Descr^Motor Vehicle", clickType: ClickType.None, rowHeight: 16);

            //ediOperationsForm.GetTabTableGeneric(@"Seal Details", @"6175", @"6174");
            ediOperationsForm.GetTabTableGeneric("Seal Details");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, "ID^ICUU2300394~Seal Number^CMA-CGM366",
                clickType: ClickType.None, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, "ID^ICUU2371952~Seal Number^TBE495037",
                clickType: ClickType.None, rowHeight: 16);

   
        }



    }

}
