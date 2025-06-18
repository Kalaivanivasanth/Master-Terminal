using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using System.Reflection;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40863 : MTNBase
    {

        EDIOperationsForm ediOperationsForm;
        TerminalConfigForm terminalConfigForm;

        protected static string ediFile1 = "M_40863_BOOKING.xml";
        protected static string ediFile2 = "M_40863_GATEDOC.edi";


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_40863_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            //reset configuration to what it is on base system
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"EDI", @"Use EDI Booking Release Req rather than EDI Gate Document", @"0", rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //create Booking file
            CreateDataFile(ediFile1,
              "<?xml version='1.0'?>\n<JMTInternalBooking>\n	<AllBookingHeader>\n		<BookingHeader>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZBLU</dischargePort>\n			<id>JLGBOOK40863A01</id>\n			<messageMode>A</messageMode>\n			<operatorCode>HSD</operatorCode>\n			<voyageCode>MSCK000002</voyageCode>\n				<AllBookingItem>\n					<BookingItem>\n						<weight>18830.0000</weight>\n						<number>5</number>\n						<cargoTypeDescr>ISO Container</cargoTypeDescr>\n						<commodity>APPL</commodity>\n						<fullOrMT>F</fullOrMT>\n						<id>JLGBOOK40863A01</id>\n						<isoType>2200</isoType>\n						<messageMode>A</messageMode>\n					</BookingItem>\n				</AllBookingItem>\n			</BookingHeader>\n	</AllBookingHeader>\n</JMTInternalBooking>\n");

            //create Gate Doc Booking edi file
            CreateDataFile(ediFile2,
              "UNB+UNOA:2+AUSSYD999+POETERM+180813:2215+18730'\nUNH+18730+COPARN:D:95B:UN'\nBGM+126+18617+5'\nRFF+BN:8DUD007381'\nTDT+20+MSCK000002+1++HSD:172:20+++9190755:146::LEDA MAERSK'\nLOC+9+NZPOE:139:6:PORT CHALMERS'\nLOC+8+PHMNL:139:6:MANILA'\nDTM+137:201808140615:203'\nNAD+CA+HSD:172:20'\nGID+1'\nEQD+CN++4510+++5'\nRFF+SQ:000018617001'\nEQN+1'\nLOC+11+NZAKL:139:6'\nMEA+AAE+G+KGM:24700'\nFTX+HAN++CCN'\nFTX+AAA+++APPL(NOS)'\nNAD+CZ++CHAMPION FREIGHT (N.Z.) LIMITED'\nCNT+1:1'\nUNT+19+18730'\nUNZ+1+18730'");

           MTNSignon(TestContext);

            // set configuration as required by test
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"EDI", @"Use EDI Booking Release Req rather than EDI Gate Document", @"1", rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();
        }


        [TestMethod]
        public void BookingInvalidOperator()
        {
            MTNInitialize();

            // 1. Open EDI Operations and load the booking reference multi line
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperationsForm.DeleteEDIMessages(@"Booking Reference Multi Line", @"40863", ediStatus: @"Loaded");
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Loaded", @"Verify");
            ediOperationsForm.ShowErrorWarningMessages();
            Wait.UntilResponsive(ediOperationsForm.tblEDIErrorWarningMessages);
            /// check the error message in this mode
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIErrorWarningMessages, @"ID^See error text.~Type^Error~Error Message^Code :75021. Operator HSD is not valid for voyage MSCK000002.", clickType: ClickType.None, searchType: SearchType.Exact);

            // 2. Set terminal configuration to ensure we're loading a gate document
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"EDI", @"Use EDI Booking Release Req rather than EDI Gate Document", @"0", rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();

            // 3. Load Gate Document edi
            ediOperationsForm.SetFocusToForm();
            ediOperationsForm.DeleteEDIMessages(@"Gate Document", @"40863", ediStatus: @"Loaded");
            ediOperationsForm.LoadEDIMessageFromFile(ediFile2);
            ediOperationsForm.ChangeEDIStatus(ediFile2, @"Loaded", @"Verify");

            ediOperationsForm.tblEDIErrorWarningMessages.DoubleClick();
            Wait.UntilResponsive(ediOperationsForm.tblEDIErrorWarningMessages);
            // check the error message in this mode
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIErrorWarningMessages, @"ID^See error text.~Type^Error~Error Message^Code :75021. Operator HSD is not valid for voyage MSCK000002.", clickType: ClickType.None, searchType: SearchType.Exact);
            ediOperationsForm.CloseForm();
        }




    }

}
