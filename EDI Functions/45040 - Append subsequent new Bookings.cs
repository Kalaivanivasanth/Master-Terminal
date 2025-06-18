using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using System.Web.UI.WebControls;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45040 : MTNBase
    {

        private TerminalConfigForm _terminalConfigForm;
        private EDIOperationsForm _ediOperationsForm;
        private EDIPartyEnquiryForm _ediPartyEnquiryForm;
        private EDIPartyMaintenanceForm _ediPartyMaintenanceForm;
        private BookingForm _bookingForm;

        private const string TestCaseNumber = @"45040";
        private const string BookingRef = @"BOOKING" + TestCaseNumber;

        protected static string ediFileStart = @"M_" + TestCaseNumber + "_COPARN_";
        protected static string ediFile1 = ediFileStart + @"01.edi";
        protected static string ediFile2 = ediFileStart + @"02.edi";
        protected static string ediFile3 = ediFileStart + @"03.edi";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
            BaseClassInitialize_New(testContext);
        }

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        private void MTNInitialize()
        {
             //MTNSignon(TestContext);
             LogInto<MTNLogInOutBO>();
            
             CreateDataFile(ediFile1,
               "UNA:+.'\nUNB+UNOA:2+HAPAG-LLOYD+ITSALSCT+190208:1004+65541'\nUNH+CANITSAL169533+COPARN:D:95B:UN:SMDG10'\nBGM+11:::GATE IN ORDER+BOOKING45040+9'\nFTX+OSI++L'\nRFF+BN:BOOKING45040'\nTDT+20+MSCK000002+1++MSC:172:166+++V2OF2:103::STAR COMET'\nRFF+VON:625504'\nLOC+9+ITSAL:139:6++SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nDTM+133:201902192200:203'\nDTM+132:201902220800:203'\nNAD+CF+MSC:160:166'\nGID+1'\nFTX+AAA+++FOAM PRODUCTS'\nSGP+JLG45040B01'\nEQD+CN+JLG45040B01+2200:102:5++2+5'\nRFF+BN:BOOKING45040'\nEQN+1'\nTMD+3++2'\nLOC+9+ITSAL:139:6+SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nLOC+11+NZAKL:139:6+:TER:ZZZ:MALTA FREEPORT TERMINAL LIMITED'\nLOC+8+YEADE:139:6'\nLOC+7+YEADE:139:6'\nMEA+AAE+T+KGM:3830'\nMEA+AAE+G+KGM:24000'\nTDT+10++9'\nLOC+5+ITSAL:139:6'\nDTM+133:201902081527:203'\nDTM+132:201902192200:203'\nNAD+CF+MSC:160:166'\nNAD+CU+763631:ZZZ++U. DEL CORONA & SCARDIGLI SRL++LIVORNO (LEGHORN)++57123'\nCNT+16:1'\nUNT+31+CANITSAL169533'\nUNH+CANITSAL169534+COPARN:D:95B:UN:SMDG10'\nBGM+11:::GATE IN ORDER+BOOKING45040+9'\nFTX+OSI++L'\nRFF+BN:BOOKING45040'\nTDT+20+MSCK000002+1++MSC:172:166+++V2OF2:103::STAR COMET'\nRFF+VON:625504'\nLOC+9+ITSAL:139:6++SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nDTM+133:201902192200:203'\nDTM+132:201902220800:203'\nNAD+CF+MSC:160:166'\nGID+1'\nFTX+AAA+++FOAM'\nSGP+JLG45040A01'\nEQD+CN+JLG45040A01+2200:102:5++2+5'\nRFF+BN:BOOKING45040'\nEQN+1'\nTMD+3++2'\nLOC+9+ITSAL:139:6+SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nLOC+11+NZAKL:139:6+:TER:ZZZ:MALTA FREEPORT TERMINAL LIMITED'\nLOC+8+YEADE:139:6'\nLOC+7+YEADE:139:6'\nMEA+AAE+T+KGM:3890'\nMEA+AAE+G+KGM:24000'\nTDT+10++9'\nLOC+5+ITSAL:139:6'\nDTM+133:201902081516:203'\nDTM+132:201902192200:203'\nNAD+CF+MSC:160:166'\nNAD+CU+763631:ZZZ++U. DEL CORONA & SCARDIGLI SRL++LIVORNO (LEGHORN)++57123'\nCNT+16:1'\nUNT+31+CANITSAL169534'\nUNZ+2+65541'");

            CreateDataFile(ediFile2,
                "UNA:+.'\nUNB+UNOA:2+HAPAG-LLOYD+ITSALSCT+190208:1004+65541'\nUNH+CANITSAL169533+COPARN:D:95B:UN:SMDG10'\nBGM+11:::GATE IN ORDER+BOOKING45040A+9'\nFTX+OSI++L'\nRFF+BN:BOOKING45040A'\nTDT+20+MSCK000002+1++MSC:172:166+++V2OF2:103::STAR COMET'\nRFF+VON:625504'\nLOC+9+ITSAL:139:6++SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nDTM+133:201902192200:203'\nDTM+132:201902220800:203'\nNAD+CF+MSC:160:166'\nGID+1'\nFTX+AAA+++FOAM PRODUCTS'\nSGP+JLG45040C01'\nEQD+CN+JLG45040C01+2200:102:5++2+5'\nRFF+BN:BOOKING45040A'\nEQN+1'\nTMD+3++2'\nLOC+9+ITSAL:139:6+SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nLOC+11+NZAKL:139:6+:TER:ZZZ:MALTA FREEPORT TERMINAL LIMITED'\nLOC+8+YEADE:139:6'\nLOC+7+YEADE:139:6'\nMEA+AAE+T+KGM:3830'\nMEA+AAE+G+KGM:24000'\nTDT+10++9'\nLOC+5+ITSAL:139:6'\nDTM+133:201902081527:203'\nDTM+132:201902192200:203'\nNAD+CF+MSC:160:166'\nNAD+CU+763631:ZZZ++U. DEL CORONA & SCARDIGLI SRL++LIVORNO (LEGHORN)++57123'\nCNT+16:1'\nUNT+31+CANITSAL169533'\nUNZ+2+65541'");

            CreateDataFile(ediFile3,
                "UNA:+.'\nUNB+UNOA:2+HAPAG-LLOYD+ITSALSCT+190208:1004+65541'\nUNH+CANITSAL169533+COPARN:D:95B:UN:SMDG10'\nBGM+11:::GATE IN ORDER+BOOKING45040+9'\nFTX+OSI++L'\nRFF+BN:BOOKING45040'\nTDT+20+MSCK000002+1++MSC:172:166+++V2OF2:103::STAR COMET'\nRFF+VON:625504'\nLOC+9+ITSAL:139:6++SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nDTM+133:201902192200:203'\nDTM+132:201902220800:203'\nNAD+CF+MSC:160:166'\nGID+1'\nFTX+AAA+++FOAM PRODUCTS'\nSGP+JLG45040B01'\nEQD+CN+JLG45040B01+2200:102:5++2+5'\nRFF+BN:BOOKING45040'\nEQN+1'\nTMD+3++2'\nLOC+9+ITSAL:139:6+SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nLOC+11+NZAKL:139:6+:TER:ZZZ:MALTA FREEPORT TERMINAL LIMITED'\nLOC+8+YEADE:139:6'\nLOC+7+YEADE:139:6'\nMEA+AAE+T+KGM:3830'\nMEA+AAE+G+KGM:24000'\nTDT+10++9'\nLOC+5+ITSAL:139:6'\nDTM+133:201902081527:203'\nDTM+132:201902192200:203'\nNAD+CF+MSC:160:166'\nNAD+CU+763631:ZZZ++U. DEL CORONA & SCARDIGLI SRL++LIVORNO (LEGHORN)++57123'\nCNT+16:1'\nUNT+31+CANITSAL169533'\nUNH+CANITSAL169534+COPARN:D:95B:UN:SMDG10'\nBGM+11:::GATE IN ORDER+BOOKING45040+9'\nFTX+OSI++L'\nRFF+BN:BOOKING45040'\nTDT+20+MSCK000002+1++MSC:172:166+++V2OF2:103::STAR COMET'\nRFF+VON:625504'\nLOC+9+ITSAL:139:6++SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nDTM+133:201902192200:203'\nDTM+132:201902220800:203'\nNAD+CF+MSC:160:166'\nGID+1'\nFTX+AAA+++FOAM'\nSGP+JLG45040A01'\nEQD+CN+JLG45040A01+2200:102:5++2+5'\nRFF+BN:BOOKING45040'\nEQN+1'\nTMD+3++2'\nLOC+9+ITSAL:139:6+SALERN:BER:ZZZ:SALERNO CONTAINER TERMINAL SPA'\nLOC+11+NZAKL:139:6+:TER:ZZZ:MALTA FREEPORT TERMINAL LIMITED'\nLOC+8+YEADE:139:6'\nLOC+7+YEADE:139:6'\nMEA+AAE+T+KGM:3890'\nMEA+AAE+G+KGM:24000'\nTDT+10++9'\nLOC+5+ITSAL:139:6'\nDTM+133:201902081516:203'\nDTM+132:201902192200:203'\nNAD+CF+MSC:160:166'\nNAD+CU+763631:ZZZ++U. DEL CORONA & SCARDIGLI SRL++LIVORNO (LEGHORN)++57123'\nCNT+16:1'\nUNT+31+CANITSAL169534'\nUNZ+2+65541'");

            
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops | Terminal Config");
            _terminalConfigForm = new TerminalConfigForm();
            _terminalConfigForm.SetTerminalConfiguration(@"EDI",
                @"Use EDI Booking Release Req rather than EDI Gate Document", @"0",
                rowDataType: EditRowDataType.CheckBox);
            _terminalConfigForm.CloseForm();

            // Step 4 - 6
            EDIPartySeAppendSubsequentNewBooking();

            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking", forceReset: true);
            _bookingForm = new BookingForm(@"Booking TT1");
            _bookingForm.DeleteBookings(@"BOOKING45040");


            // Load EDI Files
            SetupAndLoadInitializeData(TestContext);

            FormObjectBase.NavigationMenuSelection(@"EDI Functions | EDI Operations");  //, forceReset: true);
            //FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            _ediOperationsForm.DeleteEDIMessages(EDIOperationsDataType.GateDocument, ediFileStart, ediStatus: EDIOperationsStatusType.DBLoaded);
            _ediOperationsForm.DeleteEDIMessages(EDIOperationsDataType.GateDocument, ediFileStart, ediStatus: EDIOperationsStatusType.Loaded);
           

            LoadEDIFile(ediFile1);

        }


        [TestMethod]
        public void AppendSubsequentNewBookings()
        {
            MTNInitialize();

            // Step 9 - 11
            _bookingForm.SetFocusToForm();
            MTNControlBase.SetValueInEditTable(_bookingForm.tblSearcher, @"Booking", BookingRef);
            //_bookingForm.btnSearch.DoClick();
            _bookingForm.DoSearch();
            MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings,
                @"Booking^" + BookingRef + "~Items^2~Received^0~Released^0~Reserved^0~Pre-noted^2~Operator^HLL");
            _bookingForm.GetItemTableDetails();
            _bookingForm.CheckItemDetailsTableForText(
                @"Cargo Type: ISO Container; ISO Type: 2200  GENERAL; Items: 2; Received: 0; Items Received: JLG45040B01, JLG45040A01 (pre-noted)");

            // Step 12 - 13
            LoadEDIFile(ediFile2);

            // Step 14 - 16
            _bookingForm.SetFocusToForm();
            MTNControlBase.SetValueInEditTable(_bookingForm.tblSearcher, @"Booking", BookingRef);
            //_bookingForm.btnSearch.DoClick();
            _bookingForm.DoSearch();
           
            MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings,
                @"Booking^" + BookingRef + "~Items^2~Received^0~Released^0~Reserved^0~Pre-noted^2~Operator^HLL");
            _bookingForm.GetItemTableDetails();
            _bookingForm.CheckItemDetailsTableForText(
                @"Cargo Type: ISO Container; ISO Type: 2200  GENERAL; Items: 2; Received: 0; Items Received: JLG45040B01, JLG45040A01 (pre-noted)");
            MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings,
                @"Booking^" + BookingRef + "A~Items^1~Received^0~Released^0~Reserved^0~Pre-noted^1~Operator^HLL");
            _bookingForm.CheckItemDetailsTableForText(
                @"Cargo Type: ISO Container; ISO Type: 2200  GENERAL; Items: 1; Received: 0; Items Received: JLG45040C01 (pre-noted)");

            // Step 17 - 19
            EDIPartySeAppendSubsequentNewBooking(false);

            // Step 20 - 21 
            LoadEDIFile(ediFile3);

            // Step 22 - 26
            _bookingForm.SetFocusToForm();
            MTNControlBase.SetValueInEditTable(_bookingForm.tblSearcher, @"Booking", BookingRef);
            //_bookingForm.btnSearch.DoClick();
            _bookingForm.DoSearch();
            MTNControlBase.FindClickRowInTable(_bookingForm.tblBookings,
                @"Booking^" + BookingRef + "~Items^1~Received^0~Released^0~Reserved^0~Pre-noted^2~Operator^HLL");
            _bookingForm.GetItemTableDetails();
            _bookingForm.CheckItemDetailsTableForText(
                @"Cargo Type: ISO Container; ISO Type: 2200  GENERAL; Items: 1; Received: 0; Items Received: JLG45040B01, JLG45040A01 (pre-noted)");

            _bookingForm.SetFocusToForm();
            _bookingForm.DeleteBookings(@"BOOKING45040");
            
            // Step 27 - 33 : This is handled by resetConfigs

        }


        private void LoadEDIFile(string fileToLoad)
        {
            _ediOperationsForm.SetFocusToForm();
      
            //MTNControlBase.SetValueInEditTable(_ediOperationsForm.tblEDISearch, @"Data Type");
            MTNControlBase.SetValueInEditTable(_ediOperationsForm.tblEDISearch, @"Status");

            _ediOperationsForm.LoadEDIMessageFromFile(fileToLoad);
            //MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + fileToLoad,
            //    ClickType.ContextClick, rowHeight: 16);
            _ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + fileToLoad, ClickType.ContextClick);
            _ediOperationsForm.ContextMenuSelect(@"Load To DB");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
        }

        private void EDIPartySeAppendSubsequentNewBooking(bool set = true)
        {
            if (_ediPartyEnquiryForm == null)
            {
                FormObjectBase.NavigationMenuSelection(@" EDI Functions | EDI Party Enquiry", forceReset: true);
                //FormObjectBase.NavigationMenuSelection(@" EDI Functions | EDI Party Enquiry");
                _ediPartyEnquiryForm = new EDIPartyEnquiryForm(@"EDI Party Enquiry Form TT1");
            }
            else
            {
                _ediPartyEnquiryForm.SetFocusToForm();
            }

            MTNControlBase.FindClickRowInTable(_ediPartyEnquiryForm.tblDetails, @"Name^HLL [FOUT, FIN, LVE]", ClickType.DoubleClick,
                rowHeight: 16);

            _ediPartyMaintenanceForm = new EDIPartyMaintenanceForm(@"EDI Party Maintenance TT1");
            _ediPartyMaintenanceForm.chkAppendSubsequentNewBooking.DoClick(set);
            _ediPartyMaintenanceForm.btnSave.DoClick();
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Prenote
            CreateDataFileToLoad(@"DeletePrenote.xml",
                "<?xml version='1.0'?>\n	<JMTInternalPrenote>\n		<AllPrenote>\n			<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED / EDI_STATUS_DBLOADED_PARTIAL / EDI_STATUS_DBLOADED_PARTIAL_X</operationsToPerformStatuses>\n		<Prenote Terminal='TT1'>\n				<cargoTypeDescr>ISO Container</cargoTypeDescr>\n				<dischargePort>NZAKL</dischargePort>\n				<id>JLG45040A01</id>\n				<imexStatus>Export</imexStatus>\n				<isoType>2200</isoType>\n				<messageMode>D</messageMode>\n				<operatorCode>MSC</operatorCode>\n				<weight>6000</weight>\n				<voyageCode>MSCK000002</voyageCode>\n				<bookingRef>BOOKING45040</bookingRef>\n			</Prenote>\n			<Prenote Terminal='TT1'>\n				<cargoTypeDescr>ISO Container</cargoTypeDescr>\n				<dischargePort>NZAKL</dischargePort>\n				<id>JLG45040B01</id>\n				<imexStatus>Export</imexStatus>\n				<isoType>2200</isoType>\n				<messageMode>D</messageMode>\n				<operatorCode>MSC</operatorCode>\n				<weight>6000</weight>\n				<voyageCode>MSCK000002</voyageCode>\n				<bookingRef>BOOKING45040</bookingRef>\n			</Prenote>\n			<Prenote Terminal='TT1'>\n				<cargoTypeDescr>ISO Container</cargoTypeDescr>\n				<dischargePort>NZAKL</dischargePort>\n				<id>JLG45040C01</id>\n				<imexStatus>Export</imexStatus>\n				<isoType>2200</isoType>\n				<messageMode>D</messageMode>\n				<operatorCode>MSC</operatorCode>\n				<weight>6000</weight>\n				<voyageCode>MSCK000002</voyageCode>\n				<bookingRef>BOOKING45040A</bookingRef>\n			</Prenote>\n		</AllPrenote>\n	</JMTInternalPrenote>\n\n\n");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }


        #endregion - Setup and Run Data Loads


    }

}
