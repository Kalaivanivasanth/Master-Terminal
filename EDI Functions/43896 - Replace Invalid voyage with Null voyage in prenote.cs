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
    public class TestCase43896 : MTNBase
    {

        EDIOperationsForm ediOperationsForm;

        const string EDIFile1 = "M_43896_PrenoteNullVoyage.edi";
        const string EDIFile2 = "M_43896_PrenoteInvalidVoyage.edi";
        const string EDIFile3 = "M_43896_PrenoteXlationVoyage.edi";
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_43896_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //null voyage 
            CreateDataFile(EDIFile1,
                "A,TR493703,FISHP,MSKU9411944,Import,9723349,,845_,,MSC,GENL,4200,10400,,,,CLXROAD,2/12/2018,MBX,ROAD,'FAP, FISHER AND PAYKEL',F\n");

            //invalid voyage 
            CreateDataFile(EDIFile2,
                "A,TR493703,FISHP,MSKU9411944,Import,9723349,,845_,,MSC,GENL,4200,10400,,,,CLXROAD,2/12/2018,MBX,ROAD,'FAP, FISHER AND PAYKEL',F\n");


            //translation voyage 
            CreateDataFile(EDIFile3,
                "A,TR493703,FISHP,MSKU9411944,Import,9723349,,845_,,MSL,GENL,4200,10400,,,,CLXROAD,2/12/2018,MBX,ROAD,'FAP, FISHER AND PAYKEL',F\n");

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void PreNoteReplaceInvalidVoyage()
        {
            MTNInitialize();
            
            // 1. Open EDI Operations and delete any loaded messages relating to this test
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperationsForm.DeleteEDIMessages(@"Prenote", @"43896",ediStatus: @"Loaded");

            // 2. message 1 - null voyage when invalid
            ediOperationsForm.LoadEDIMessageFromFile(EDIFile1,specifyType: true,fileType: @"PlatoPrenote");
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + EDIFile1);
            ediOperationsForm.ClickEDIDetailsTab();
            ediOperationsForm.GetTabTableGeneric(@"Prenote", @"4036");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^MSKU9411944~Transport Mode^ROAD~Operator^MSC~Voyage^********************", clickType: ClickType.None, searchType: SearchType.Exact);

            // 2. message 2 - invalid voyage
            ediOperationsForm.LoadEDIMessageFromFile(EDIFile2, specifyType: true, fileType: @"PlatoPrenoteDup");
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile2, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + EDIFile2);
            ediOperationsForm.ClickEDIDetailsTab();
            ediOperationsForm.GetTabTableGeneric(@"Prenote", @"4036");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^MSKU9411944~Transport Mode^ROAD~Operator^MSC~Voyage^845_", clickType: ClickType.None, searchType: SearchType.Exact);
            ediOperationsForm.ChangeEDIStatus(EDIFile2, @"Loaded", @"Verify");
            ediOperationsForm.ShowErrorWarningMessages();
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIErrorWarningMessages, @"ID^MSKU9411944~Property^voyageCode~Error Message^Code :70102. An invalid Voyage '845_' was specified.", clickType: ClickType.None);

            // 3. message 3 - translation
            ediOperationsForm.LoadEDIMessageFromFile(EDIFile3, specifyType: true, fileType: @"PlatoPrenoteXlate");
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile3, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + EDIFile3);
            /*ediOperationsForm.tabEDIDetails.Click();
            MTNControlBase.FindTabOnForm(ediOperationsForm.tabEDIDetails, @"Prenote");
            ediOperationsForm.ShowEDIDataTable(@"4036");*/
            ediOperationsForm.ClickEDIDetailsTab();
            ediOperationsForm.GetTabTableGeneric(@"Prenote", @"4036");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^MSKU9411944~Transport Mode^ROAD~Operator^MSL~Voyage^GRIG001IMPORT", clickType: ClickType.None, searchType: SearchType.Exact);

        }

    }

}
