using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45041 : MTNBase
    {

        EDIOperationsForm ediOperationsForm;
       
        const string EdiFile1 = "M_45041_Translations01.edi";


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_45041_";
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            MTNSignon(TestContext);
            
            //create Booking file
            CreateDataFile(EdiFile1,
                "UNA:+.'\nUNB+UNOA:1+PIL+NZWLGT01+20190320:0943+201903200943'\nUNH+201903200943+COPARN:D:00B:UN'\nBGM+104+WLG900024900+9'\nDTM+7:201903200943:203'\nRFF+BN:WLG900024900'\nTDT+20+MSCK000002+1+++++9628324:146:11:KOTA LIHAT'\nRFF+VON:MSCK000002'\nLOC+9+NZWLG:139:6'\nLOC+11+SGSIN:139:6'\nNAD+CA+BALT'\nNAD+OY++OPEN COUNTRY DAIRY LTD.'\nEQD+CN++2200:102:5+1+2+5'\nRFF+SQ:1'\nEQN+1'\nLOC+9+NZWLG:139:6'\nLOC+11+SGSIN:139:6'\nLOC+65+SGSIN:139:6'\nFTX+AAA+++DAIRY PRODUCE; MILK AND CREAM, CONCENTRATED OR CONTAINING ADDED SUGAR OR OTHER SWEETENING MATTER, IN POWDER, GRANULES OR OTHER SOLID FORMS, OF A FAT CONTENT NOT EXCEEDING 1.5% (BY WEIGHT)'\nUNT+18+201903200943'\nUNZ+30+201903200943'");

        }
        

        [TestMethod]
        public void EDIFileLoadwithGCIDs()
        {
            MTNInitialize();

            // set configuration as required by test
            FormObjectBase.NavigationMenuSelection(@"System Ops|System Config");
            SystemConfigForm systemConfigForm = new SystemConfigForm(@"Configuration");
            systemConfigForm.SetTerminalConfiguration(@"Defaults", @"Enable EDI Xlation over 30 characters", @"1", rowDataType: EditRowDataType.CheckBox);
            systemConfigForm.CloseForm();

            // 1. Open EDI Operations and load gate doc
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperationsForm.DeleteEDIMessages(@"Gate Document", @"45041", ediStatus: @"Loaded");
            ediOperationsForm.LoadEDIMessageFromFile(EdiFile1);

            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, clickType: ClickType.Click, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + EdiFile1, clickType: ClickType.Click);
            ediOperationsForm.ClickEDIDetailsTab();
            //ediOperationsForm.GetTabTableGeneric(@"Booking Item", @"6179", @"6178");
            ediOperationsForm.GetTabTableGeneric("Booking Item");

            //2. Ensure commodity translates as expected
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Cmdty^APPL", ClickType.None,searchType: SearchType.Exact,rowHeight: 16);

            //3. reset configuration to what it is on base system
            FormObjectBase.NavigationMenuSelection(@"System Ops|System Config", forceReset: true);
            systemConfigForm = new SystemConfigForm(@"Configuration");
            systemConfigForm.SetTerminalConfiguration(@"Defaults", @"Enable EDI Xlation over 30 characters", @"0", rowDataType: EditRowDataType.CheckBox);
            systemConfigForm.CloseForm();

            //4. Ensure commodity translates as expected
            ediOperationsForm.SetFocusToForm();
            ediOperationsForm.DeleteEDIMessages(@"Gate Document", @"45041", ediStatus: @"Loaded");
            ediOperationsForm.LoadEDIMessageFromFile(EdiFile1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + EdiFile1, clickType: ClickType.Click);
            ediOperationsForm.ClickEDIDetailsTab();
            //ediOperationsForm.GetTabTableGeneric(@"Booking Item", @"6179");
            ediOperationsForm.GetTabTableGeneric("Booking Item");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Cmdty^DAIR", ClickType.None, searchType: SearchType.Exact, rowHeight: 16);


        }




    }

}
