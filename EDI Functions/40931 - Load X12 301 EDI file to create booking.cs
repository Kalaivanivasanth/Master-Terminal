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
    public class TestCase40931 : MTNBase
    {

        EDIOperationsForm ediOperationsForm;

        protected static string ediFile1 = "M_40931_BookingX12Y4.edi";
        protected static string ediFile2 = "M_40931_BookingX12R4.edi";
        protected static string ediFile3 = "M_40931_BookingX12LX.edi";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_40931_";
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
               "ISA*00*          *00*          *ZZ*CPRC           *ZZ*USCHT          *180706*1432*U*00401*000031959*0*P*>~\nGS*RO*CPRC*USCHT*20180706*1432*27110*X*004010~\nST*301*0001~\nB1*CPRC*CAT265152*20180705*U~\nY3*CAT265152*CPRC~\nY4*CAT265152****002*45R0~\nW09*CZ*-10*FA~\nN1*SH*MATOSANTOS COMMERCIAL CORP~\nN1*CN*MATOSANTOS COMMERCIAL CORP~\nR4*L*UN*USPHL*PHILADELPHIA*US~\nR4*D*UN*PRSJU*SAN JUAN*PR~\nR4*K*UN*PRSJU*SAN JUAN*PR~\nLX*1~\nL0*1***0*N***1*UNT~\nL5**GROCERY STORE MERCHANDIS~\nLX*2~\nL0*2***0*N***1*UNT~\nL5**GROCERY STORE MERCHANDIS~\nLX*3~\nL0*3***0*N***1*UNT~\nL5**GROCERY STORE MERCHANDIS~\nV1*4534530*455-3**8024S****L~\nSE*21*0001~\nGE*1*27110~\nIEA*1*000031959~\n");

            CreateDataFile(ediFile2,
               "ISA*00*          *00*          *ZZ*CPRC           *ZZ*USCHT          *180706*1432*U*00401*000031959*0*P*>~\nGS*RO*CPRC*USCHT*20180706*1432*27110*X*004010~\nST*301*0001~\nB1*MSL*CAT265152*20180705*U~\nY3*CAT265152*CPRC~\nY4*CAT265152****002*45R0~\nN1*SH*MATOSANTOS COMMERCIAL CORP~\nN1*CN*MATOSANTOS COMMERCIAL CORP~\nR4*L*UN*USPHL*PHILADELPHIA*US~\nR4*D*UN*PRSJU*SAN JUAN*PR~\nR4*K*UN*PRSJU*SAN JUAN*PR~\nW09*CZ*-10*FA~\nLX*1~\nL0*1***0*N***1*UNT~\nL5**ICEC~\nLX*2~\nL0*2***0*N***1*UNT~\nL5**ICEC~\nLX*3~\nL0*3***0*N***1*UNT~\nL5**ICEC~\nV1*4534530*455-3**MSCK000002****L~\nSE*21*0001~\nGE*1*27110~\nIEA*1*000031959~\n");

            CreateDataFile(ediFile3,
               "ISA*00*          *00*          *ZZ*CPRC           *ZZ*USCHT          *180706*1432*U*00401*000031959*0*P*>~\nGS*RO*CPRC*USCHT*20180706*1432*27110*X*004010~\nST*301*0001~\nB1*CPRC*CAT265152*20180705*U~\nY3*CAT265152*CPRC~\nY4*CAT265152****004*45R0~\nW09*CZ*-123*CE~\nN1*SH*MATOSANTOS COMMERCIAL CORP~\nN1*CN*MATOSANTOS COMMERCIAL CORP~\nR4*L*UN*USPHL*PHILADELPHIA*US~\nR4*D*UN*PRSJU*SAN JUAN*PR~\nR4*K*UN*PRSJU*SAN JUAN*PR~\nLX*1~\nW09*CZ*-001*CE~\nL0*1***0*N***1*UNT~\nL5**GROCERY STORE MERCHANDIS~\nLX*2~\nW09*CZ*-002*CE~\nL0*2***0*N***1*UNT~\nL5**GROCERY STORE MERCHANDIS~\nLX*3~\nW09*CZ*-003*CE~\nL0*3***0*N***1*UNT~\nL5**GROCERY STORE MERCHANDIS~\nLX*4~\nL0*4***0*N***1*UNT~\nL5**GROCERY STORE MERCHANDIS~\nV1*4534530*455-3**8024S****L~\nSE*21*0001~\nGE*1*27110~\nIEA*1*000031959~\n");

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void LoadX12301EdiFileForBooking()
        {
            MTNInitialize();

            // 1. Open EDI Operations
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // 1b. Delete any Existing EDI messages relating to this test.
            ediOperationsForm.DeleteEDIMessages(@"Gate", @"40931", @"Loaded");

            // 2. Load X12 file #1 Y4, find and click on loaded message to refresh data
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, rowHeight: 16, xOffset: 200);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile1, xOffset: 200);
            //ediOperationsForm.tabEDIDetails.Click();

            // 3. Go to Booking Item tab to assert data
            ediOperationsForm.ClickEDIDetailsTab();
            //ediOperationsForm.GetTabTableGeneric(@"Booking Item", @"6179");
            ediOperationsForm.GetTabTableGeneric(@"Booking Item");

            // 4. Assert Data
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Temperature^-23.3~ISO Type^45R0~ID^CAT265152",findInstance: 1, searchType: SearchType.Exact,rowHeight: 16, xOffset: 200);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Temperature^-23.3~ISO Type^45R0~ID^CAT265152",findInstance: 2, searchType: SearchType.Exact,rowHeight: 16, xOffset: 200);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Temperature^~ISO Type^~ID^CAT265152", searchType:SearchType.Exact, rowHeight: 16, xOffset: 200);

            // 2. Load X12 file #1 R4, find and click on loaded message to refresh data
            ediOperationsForm.LoadEDIMessageFromFile(ediFile2);
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile2, rowHeight: 16, xOffset: 200);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile2, xOffset: 200);
            //ediOperationsForm.tabEDIDetails.Click();

            // 3. Go to Booking Item tab to assert data
            ediOperationsForm.ClickEDIDetailsTab();
            //ediOperationsForm.GetTabTableGeneric(@"Booking Item", @"6179");
            ediOperationsForm.GetTabTableGeneric(@"Booking Item");

            // 4. Assert Data
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Temperature^-23.3~ISO Type^45R0~ID^CAT265152", searchType: SearchType.Exact, findInstance: 1, rowHeight: 16, xOffset: 200);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Temperature^-23.3~ISO Type^45R0~ID^CAT265152", searchType: SearchType.Exact,findInstance: 2, rowHeight: 16, xOffset: 200);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Temperature^-23.3~ISO Type^~ID^CAT265152", searchType: SearchType.Exact,rowHeight: 16, xOffset: 200);


            // 2. Load X12 file #1 Y4, find and click on loaded message to refresh data
            ediOperationsForm.LoadEDIMessageFromFile(ediFile3);
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile3, rowHeight: 16, xOffset: 200);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile3, xOffset: 200);
            //ediOperationsForm.tabEDIDetails.Click();

            // 3. Go to Booking Item tab to assert data
            ediOperationsForm.ClickEDIDetailsTab();
            //ediOperationsForm.GetTabTableGeneric(@"Booking Item", @"6179");
            ediOperationsForm.GetTabTableGeneric(@"Booking Item");

            // 4. Assert Data
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Temperature^-3~ISO Type^45R0~ID^CAT265152", searchType: SearchType.Exact, rowHeight: 16, xOffset: 200);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Temperature^-2~ISO Type^45R0~ID^CAT265152", searchType: SearchType.Exact, rowHeight: 16, xOffset: 200);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Temperature^-1~ISO Type^45R0~ID^CAT265152", searchType: SearchType.Exact, rowHeight: 16, xOffset: 200);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Temperature^-123~ISO Type^45R0~ID^CAT265152", searchType: SearchType.Exact, rowHeight: 16, xOffset: 200);





        }



    }

}
