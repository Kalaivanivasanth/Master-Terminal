using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43946 : MTNBase
    {
        EDIOperationsForm ediOperationsForm;
       
        protected static string ediFile1 = "M_43946_COPARN.edi";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_43946_";
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
                "UNB+UNOA:1+MAEU:ZZ:SCAC+ITSALSCT+181221:1517+36057+++++MAE'\nUNH+3605700001+COPARN:D:95B:UN'\nBGM+11+20181221151726+5'\nRFF+BN:967254988'\nTDT+20+904W+1++MSK:172:20+++9V3818:103::MAERSK PATRS'\nLOC+88+ITSAL:139:6'\nLOC+9+ITSAL:139:6'\nDTM+133:201901211400:203'\nNAD+CZ+118239879+ALICOM IMPORT EXPORT SRL'\nNAD+FW+11800258618+MARFREIGHT SRL'\nNAD+CA+MSK:172:20'\nGID+1'\nFTX+AAA+++GENL, NOS, NON-FROZEN'\nEQD+CN++2200:102:5+2+2+5'\nRFF+BN:967254988'\nEQN+5'\nTMD+3++2'\nDTM+181:201812260800:203'\nLOC+8+QADOH:139:6'\nLOC+98+ITSAL:139:6'\nLOC+11+ESALG:139:6'\nMEA+AAE+G+KGM:28600'\nMEA+AAE+T+KGM:4004'\nMEA+AAE+EGW+KGM:32604'\nFTX+AAI+++5X40 HC DRY |CNTRS IDONEI PER ALIMENTI|'\nFTX+SIN++VIP'\nTDT+30+1903+1++MAE:172:20+++WMPP:103::MAERSK PITTSBURGH'\nLOC+11+OMSLL:139:6'\nCNT+16:1'\nUNT+29+3605700001'\nUNZ+1+36057'");

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void EnforceVoyageCallSignTranlsations()
        {
            MTNInitialize();
            
            /*
            To test the following scenarios:
            1. If the Terminal Config > EDI >Enforce Voyage Call Sign/Lloyds Number For Translation is checked
            When the Coparn file has a callsign/Lloyd's number that doesn't match the callsign/Lloyd's number for any valid voyage-code-translated voyages, the voyage code should not be translated 
            when the file is loaded. It should throw the verification error 
            2. If the Terminal Config > EDI > Enforce Voyage Call Sign/Lloyds Number For Translation is unchecked
            When Loaded the Coparn file which has the Callsign/Lloyds number that doesn't match with the callsign/Lloyd's number for any valid voyage-code-translated voyages. It doesn't show any verification error and it translates the voyage code.
            */

            // set terminal config
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"EDI", @"Enforce Voyage Call Sign/Lloyds Number For Translation", @"1", doReverse: true,rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();

            // delete booking if it exists
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking", forceReset: true);
            BookingForm bookingForm = new BookingForm(@"Booking TT1");
            bookingForm.DeleteBookings(@"967254988");
            bookingForm.CloseForm();


            // 1. Open EDI Operations and clear any existing files
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperationsForm.DeleteEDIMessages(@"Gate Document", @"43946",@"Loaded");
            ediOperationsForm.DeleteEDIMessages(@"Booking Reference Multi Line", @"43946",@"Loaded");

            // 2. Load COPARN file
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile1);
            //ediOperationsForm.tabEDIDetails.Click();

            //3. check voyage translation on booking header results in 904W
            /*MTNControlBase.FindTabOnForm(ediOperationsForm.tabEDIDetails, @"Booking Header");
            ediOperationsForm.ShowEDIDataTable(@"4036");*/
            ediOperationsForm.ClickEDIDetailsTab();
            ediOperationsForm.GetTabTableGeneric(@"Booking Header", @"4036");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Voyage^904W", clickType: ClickType.None, rowHeight: 16);

            //4. Verify and check that the voyage code is in error
            ediOperationsForm.ChangeEDIStatus(@"43946", "Loaded", "Verify");
            ediOperationsForm.ShowErrorWarningMessages();
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIErrorWarningMessages,@"ID^967254988~Type^Error~Property^voyageCode~Error Message^Code :85962. Voyage '904W' does not exist for terminal 'TT1'",clickType: ClickType.None);

            //5. switch terminal config
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"EDI", @"Enforce Voyage Call Sign/Lloyds Number For Translation", @"0", doReverse: true, rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();

            //6. load same edi file and check translation results in PETR_43946. Load the booking to DB
            ediOperationsForm.SetFocusToForm();
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile1);
            /*FormObjectBase.ClickButton(ediOperationsForm.tabEDIDetails);
            MTNControlBase.FindTabOnForm(ediOperationsForm.tabEDIDetails, @"Booking Header");
            ediOperationsForm.ShowEDIDataTable(@"4036");*/
            ediOperationsForm.ClickEDIDetailsTab();
            ediOperationsForm.GetTabTableGeneric(@"Booking Header", @"4036");
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Voyage^PETR_43946", clickType: ClickType.None, rowHeight: 16);
            ediOperationsForm.ChangeEDIStatus(@"43946", "Loaded", "Verify");
            ediOperationsForm.ChangeEDIStatus(@"43946", "Verify warnings", "Load To DB");

            //7. Check the booking was loaded correctly.
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking", forceReset: true);
            bookingForm = new BookingForm(@"Booking TT1");
            bookingForm.GetSearcherTableDetails();
            MTNControlBase.SetValueInEditTable(bookingForm.tblSearcher, @"Booking", @"967254988");
            //bookingForm.btnSearch.DoClick(2000);
            bookingForm.DoSearch();
            MTNControlBase.FindClickRowInTable(bookingForm.tblBookings, @"Booking^967254988", rowHeight: 16);
            
        }

    }

}
