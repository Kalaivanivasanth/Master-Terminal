using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using System.Diagnostics;
using MTNForms.Controls;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43972 : MTNBase
    {

        private CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;
        private TerminalConfigForm _terminalConfigForm;
        private EDIOperationsForm _ediOperationsForm;

        private const string TestCaseNumber = @"43972";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";

        protected static string ediFile1 = "M_" + TestCaseNumber + "_Add_COPARN.edi";
        protected static string ediFile2 = "M_" + TestCaseNumber + "_Update_COPARN.edi";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
           
            MTNSignon(TestContext);
            
            CreateDataFile(ediFile1,
                "UNA:+.'\nUNB+UNOC:3+LENAVI-GOA:14+ITGOAAL:14+181003:0921+8153'\nUNH+4+COPARN:D:00B:UN:SMDG20'\nBGM+11+43972+9'\nRFF+BN:43972'\nTDT+20+MSCK000002+1++MSC:172:20+++D5PF2:103::MSC ASLI'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nDTM+133:201810060400:203'\nNAD+CF+MSC'\nNAD+MS+MSC'\nNAD+CB+ANGELO PROVERA SRL:160:ZZZ'\nEQD+CN+JLG43972A01+2200:102:5++2+5'\nEQN+1:2'\nDTM+132:201810051200:203'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nLOC+11+NZAKL:139:6:GIOIA TAURO+IT:162:5:ITALY'\nLOC+88+ITCRM:139:6:Carmagnola+IT:162:5:Italy'\nLOC+7+NZBLU:139:6:NOVOROSSIYSK+RU:162:5:RUSSIAN FEDERATION'\nMEA+AAE+G+KGM:25000'\nFTX+AAA+++GEN'\nCNT+16:1'\nUNT+20+4'\nUNZ+4+8153'\n");

            CreateDataFile(ediFile2,
                "UNA:+.'\nUNB+UNOC:3+LENAVI-GOA:14+ITGOAAL:14+181003:1531+8183'\nUNH+1+COPARN:D:00B:UN:SMDG20'\nBGM+11+43972+5'\nRFF+BN:43972'\nTDT+20+MSCK000002+1++MSC:172:20+++D5PF2:103::MSC ASLI'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nDTM+133:201810052000:203'\nNAD+CF+MSC'\nNAD+MS+MSC'\nNAD+CB+ANGELO PROVERA SRL:160:ZZZ'\nEQD+CN+JLG43972A01+2200:102:5++2+5'\nRFF+VGR:VGMG0012954000'\nEQN+1:2'\nDTM+132:201810050400:203'\nDTM+798:201810031300:203'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nLOC+11+NZAKL:139:6:GIOIA TAURO+IT:162:5:ITALY'\nLOC+88+ITCRM:139:6:Carmagnola+IT:162:5:Italy'\nLOC+7+NZBLU:139:6:NOVOROSSIYSK+RU:162:5:RUSSIAN FEDERATION'\nMEA+AAE+VGM+KGM:20000'\nFTX+AAA+++GEN'\nFTX+ABS++SM1:ZZZ:SMD'\nNAD+AM+++TESTER'\nNAD+SPC+++TEST41318'\nCNT+16:1'\nUNT+25+1'\n");


            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            _terminalConfigForm = new TerminalConfigForm();
            _terminalConfigForm.SetTerminalConfiguration(@"EDI",
                @"Use EDI Booking Release Req rather than EDI Gate Document", @"0",
                rowDataType: EditRowDataType.CheckBox);
            _terminalConfigForm.CloseForm();

            // Load EDI Files
            SetupAndLoadInitializeData(TestContext);

            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            _ediOperationsForm.DeleteEDIMessages(@"Gate Document", TestCaseNumber, ediStatus: @"Loaded");
            _ediOperationsForm.DeleteEDIMessages(@"Gate Document", TestCaseNumber, ediStatus: @"DB Loaded");

            MTNControlBase.SetValueInEditTable(_ediOperationsForm.tblEDISearch, @"Data Type", @"Gate Document");
            MTNControlBase.SetValueInEditTable(_ediOperationsForm.tblEDISearch, @"Status", @"Loaded");

            _ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            Trace.TraceInformation("43972 - Find and click 'Load To DB' add file");
            //MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIMessages,
            //    @"Status^Loaded~File Name^M_" + TestCaseNumber + @"_Add_COPARN", ClickType.ContextClick, SearchType.Contains);
            _ediOperationsForm.TblEDIMessages.FindClickRow(
                @"Status^Loaded~File Name^M_" + TestCaseNumber + @"_Add_COPARN", ClickType.ContextClick,
                SearchType.Contains);
            _ediOperationsForm.ContextMenuSelect(@"Load To DB");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            SetupAndLoadInitializeData2(TestContext);
        }

   
        [TestMethod]
        public void COPARNDeletesStopFields()
        {

            MTNInitialize();
            
            // Step 6 - 10 : Since we already have tests which do these steps it is now done as part of the data load

            // Step 11 - 12
            _ediOperationsForm.SetFocusToForm();
            _ediOperationsForm.LoadEDIMessageFromFile(ediFile2);
            Trace.TraceInformation("43972 - Find and click 'Load To DB' update file");
            //MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIMessages,
            //    @"Status^Loaded~File Name^M_" + TestCaseNumber + @"_Update_COPARN", ClickType.ContextClick,
            //    SearchType.Contains);
            _ediOperationsForm.TblEDIMessages.FindClickRow(
                @"Status^Loaded~File Name^M_" + TestCaseNumber + @"_Update_COPARN", ClickType.ContextClick);
            _ediOperationsForm.ContextMenuSelect(@"Load To DB");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            // Step 13 - 16
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"Pre-Notified", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId, rowHeight: 18);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            _cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for " + CargoId + " TT1");
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
                @"Type^Stop Cleared~Details^Stop Cleared: Customs Export (CEDO).");
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
                @"Type^Stop Set~Details^Stop Set: Customs Export (CEDO).");

            // Step 17 - 18 : This is handled by resetConfigs

        }

        
        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Prenote
            CreateDataFileToLoad(@"DeletePrenote.xml",
                "<?xml version='1.0'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED / EDI_STATUS_DBLOADED_PARTIAL / EDI_STATUS_DBLOADED_PARTIAL_X</operationsToPerformStatuses>\n		<Prenote Terminal='TT1'>\n			<messageMode>D</messageMode>\n            <id>JLG43972A01</id>\n            <operatorCode>MSC</operatorCode>\n            <weight>1000</weight>\n            <imexStatus>Export</imexStatus>\n            <commodity>GEN</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <isoType>2200</isoType>\n		</Prenote>\n			</AllPrenote>\n</SystemXMLPrenote>\n\n");

            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        private static void SetupAndLoadInitializeData2(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber + "A_";

            // Delete Stops
            CreateDataFileToLoad(@"DeleteStops.xml",
                "<?xml version='1.0'?> <JMTInternalStop xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalStop.xsd'>\n	<AllCargoUpdateRequest>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<CargoUpdateRequest Terminal='TT1'>\n			<directDelivery/>\n			<clearanceExpiryDate/>\n			<updateType/>\n			<billOfLading/>\n			<cargoRemarks/>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<deliveryReleaseNumber>1</deliveryReleaseNumber>\n			<description/>\n			<fullOrMT/>\n			<gradeCode/>\n			<id>JLG43972A01</id>\n			<messageMode>A</messageMode>\n			<stopAction>D</stopAction>\n			<stopCode>5</stopCode>\n			<subTerminalCode/>\n			<toDoAction/>\n			<toDoRemarks/>\n			<toDoTaskShort/>\n			<voyageCode/>\n		</CargoUpdateRequest>\n		<CargoUpdateRequest Terminal='TT1'>\n			<directDelivery/>\n			<clearanceExpiryDate/>\n			<updateType/>\n			<billOfLading/>\n			<cargoRemarks/>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<deliveryReleaseNumber/>\n			<description/>\n			<fullOrMT/>\n			<gradeCode/>\n			<id>JLG43972A01</id>\n			<messageMode>A</messageMode>\n			<stopAction>D</stopAction>\n			<stopCode>1</stopCode>\n			<subTerminalCode/>\n			<toDoAction/>\n			<toDoRemarks/>\n			<toDoTaskShort/>\n			<voyageCode/>\n		</CargoUpdateRequest>\n	</AllCargoUpdateRequest>\n</JMTInternalStop>\n");


            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }

}
