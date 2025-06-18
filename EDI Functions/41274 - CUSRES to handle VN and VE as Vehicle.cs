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
    public class TestCase41274 : MTNBase
    {
        EDIOperationsForm ediOperationsForm;

        const string EDIFile1 = "M_41274_CUSRES_VN.edi";
        const string EDIFile2 = "M_41274_CUSRES_VE.edi";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_41274_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //Create CUSRES EDI file with VN
            CreateDataFile(EDIFile1,
                "UNA:+.'\nUNB+UNOA:2+CUSMOD:ZZZ+00114886K:ZZZ+180913:0028+24933'\nUNH+48677+CUSRES:D:96B:UN+S00007980'\nBGM+932+51099347:01'\nFTX+DIN+++5 LOOSE PACKAGE(S) OR ITEM(S)'\nTDT+20+3A+1+++++:::GRAND PEARL'\nLOC+11+NZWLG'\nGIS+819:120:143'\nNAD+AL+40181547D:ZZZ:143+BUYING SOLUTIONS LIMITED'\nNAD+CB+40433457D:ZZZ:143+JACANNA HOLDINGS LIMITED'\nDOC+964+2'\nPAC+5++VE'\nRFF+BM:ATI118249A00030'\nUNT+12+48677'\nUNZ+1+24933'");

            //Create CUSRES EDI file with VE
            CreateDataFile(EDIFile2,
                "UNA:+.'\nUNB+UNOA:2+CUSMOD:ZZZ+00114886K:ZZZ+180913:0028+24933'\nUNH+48677+CUSRES:D:96B:UN+S00007980'\nBGM+932+51099347:01'\nFTX+DIN+++5 LOOSE PACKAGE(S) OR ITEM(S)'\nTDT+20+3A+1+++++:::GRAND PEARL'\nLOC+11+NZWLG'\nGIS+819:120:143'\nNAD+AL+40181547D:ZZZ:143+BUYING SOLUTIONS LIMITED'\nNAD+CB+40433457D:ZZZ:143+JACANNA HOLDINGS LIMITED'\nDOC+964+2'\nPAC+5++VE'\nRFF+BM:ATI118249A00030'\nUNT+12+48677'\nUNZ+1+24933'");

            MTNSignon(TestContext);

        }


        [TestMethod]
        public void CusresToHandleVnAndVe()
        {
            MTNInitialize();
            
            // 1a. Open EDI Operations
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // 1b. Delete Existing EDI messages relating to this test.
            ediOperationsForm.DeleteEDIMessages(EDIOperationsDataType.CustomStops, @"41274");

            // 2. Load CUSRES VN and check that the cargo type on the stop is recognised as a Motor Vehicle
            ediOperationsForm.LoadEDIMessageFromFile(EDIFile1);
            /*MTNControlBase.FindTabOnForm(ediOperationsForm.tabEDIDetails, @"Custom Stops");
            ediOperationsForm.ShowEDIDataTable(@"4036");*/
            ediOperationsForm.GetTabTableGeneric(@"Custom Stops", @"4036");
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + EDIFile1, xOffset: 200);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^00000001~Entry Number^51099347/01~BOL^ATI118249A00030~Clearance^819~Cargo Type Descr^Motor Vehicle", clickType: ClickType.None, rowHeight: 16);
            ediOperationsForm.CloseForm();


            // 3. Load CUSRES VE and check that the cargo type on the stop is recognised as a Motor Vehicle
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperationsForm.LoadEDIMessageFromFile(EDIFile2);
            /*MTNControlBase.FindTabOnForm(ediOperationsForm.tabEDIDetails, @"Custom Stops");
            ediOperationsForm.ShowEDIDataTable(@"4036");*/
            ediOperationsForm.GetTabTableGeneric(@"Custom Stops", @"4036");

            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile2, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + EDIFile2, xOffset: 200);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"ID^00000001~Entry Number^51099347/01~BOL^ATI118249A00030~Clearance^819~Cargo Type Descr^Motor Vehicle", clickType: ClickType.None, rowHeight: 16);


        }

 



    }

}
