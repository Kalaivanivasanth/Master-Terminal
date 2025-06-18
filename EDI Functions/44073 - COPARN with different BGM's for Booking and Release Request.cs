using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44073 : MTNBase
    {
        EDIOperationsForm _ediOperationsForm;
        EDIPartyEnquiryForm _ediPartyEnquiryForm;
        EDIPartyMaintenanceForm _ediPartyMaintenanceForm;

        bool _itemFound;

        const string TestCaseNumber = @"44073";

        const string EdiFile = "M_"+ TestCaseNumber + "_COPARN.edi";
        const string EdiFile1 = "M_" + TestCaseNumber + "A_COPARN.edi";
        const string EdiFile2 = "M_" + TestCaseNumber + "B_COPARN.edi";
        const string EdiFile3 = "M_" + TestCaseNumber + "C_COPARN.edi";
        
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
            LogInto<MTNLogInOutBO>();

            CreateDataFile(EdiFile,
                "UNB+UNOA:2+GSL+ITSALSCT+180529:1011+123987+55660'\nUNH+1072711+COPARN:D:95B:UN:SMDG20'\nBGM+12+1072711+9'\nRFF+BN:JLG44073A01'\nTDT+20+0261+1++BCL:172:20+++V7AL8:103::RUTH BCLCHARD'\nNAD+CF+BCL'\nGID+1'\nNAD+CN+CARTON PACK'\nEQD+CN++45G1:102:5++2+4'\nRFF+BN:JLG44073A01'\nEQN+1'\nLOC+11+ILHFA'\nLOC+8+ILHFA'\nFTX+CFW+++GALLOZZI'\nFTX+SHP+++VIA ADELFIA ZI'\nFTX+SHP+++RUTIGLIANO BARI'\nFTX+SHP+++ x conto di CARTON PACK'\nFTX+CON+++CARTON PACK'\nFTX+AAA+++*'\nCNT+16:1'\nUNT+19+1072711'\nUNH+1072711+COPARN:D:95B:UN:SMDG20'\nBGM+11+1072711+2'\nRFF+BN:JLG44073A01'\nTDT+20+0261 +1++BCL:172:20+++V7AL8:103::RUTH BCLCHARD'\nNAD+CF+BCL'\nGID+1'\nNAD+CN+CARTON PACK'\nEQD+CN++45G1:102:5++2+5'\nRFF+BN:JLG44073A01'\nEQN+1'\nLOC+11+ILHFA'\nLOC+8+ILHFA'\nMEA+AAE+G+KGM:20400'\nFTX+CFW+++GALLOZZI'\nFTX+SHP+++CARTON PACK'\nFTX+SHP+++VIA ADELFIA ZI'\nFTX+SHP+++RUTIGLIANO BARI'\nFTX+CON+++CARTON PACK'\nFTX+AAA+++*'\nCNT+16:1'\nUNT+20+1072711'\nUNZ+2+123987'\n");

            CreateDataFile(EdiFile1,
                "UNB+UNOA:2+GSL+ITSALSCT+180529:1011+123987+55660'\nUNH+1072711+COPARN:D:95B:UN:SMDG20'\nBGM+12+1072711+9'\nRFF+BN:JLG44073A01'\nTDT+20+0261+1++BCL:172:20+++V7AL8:103::RUTH BCLCHARD'\nNAD+CF+BCL'\nGID+1'\nNAD+CN+CARTON PACK'\nEQD+CN++45G1:102:5++2+4'\nRFF+BN:JLG44073A01'\nEQN+1'\nLOC+11+ILHFA'\nLOC+8+ILHFA'\nFTX+CFW+++GALLOZZI'\nFTX+SHP+++VIA ADELFIA ZI'\nFTX+SHP+++RUTIGLIANO BARI'\nFTX+SHP+++ x conto di CARTON PACK'\nFTX+CON+++CARTON PACK'\nFTX+AAA+++*'\nCNT+16:1'\nUNT+19+1072711'\nUNH+1072711+COPARN:D:95B:UN:SMDG20'\nBGM+11+1072711+2'\nRFF+BN:JLG44073A01'\nTDT+20+0261 +1++BCL:172:20+++V7AL8:103::RUTH BCLCHARD'\nNAD+CF+BCL'\nGID+1'\nNAD+CN+CARTON PACK'\nEQD+CN++45G1:102:5++2+5'\nRFF+BN:JLG44073A01'\nEQN+1'\nLOC+11+ILHFA'\nLOC+8+ILHFA'\nMEA+AAE+G+KGM:20400'\nFTX+CFW+++GALLOZZI'\nFTX+SHP+++CARTON PACK'\nFTX+SHP+++VIA ADELFIA ZI'\nFTX+SHP+++RUTIGLIANO BARI'\nFTX+CON+++CARTON PACK'\nFTX+AAA+++*'\nCNT+16:1'\nUNT+20+1072711'\nUNZ+2+123987'\n");

            CreateDataFile(EdiFile2,
                "UNB+UNOA:2+GSL+ITSALSCT+180529:1011+123987+55660'\nUNH+1072711+COPARN:D:95B:UN:SMDG20'\nBGM+12+1072711+9'\nRFF+BN:JLG44073A01'\nTDT+20+0261+1++BCL:172:20+++V7AL8:103::RUTH BCLCHARD'\nNAD+CF+BCL'\nGID+1'\nNAD+CN+CARTON PACK'\nEQD+CN++45G1:102:5++2+4'\nRFF+BN:JLG44073A01'\nEQN+1'\nLOC+11+ILHFA'\nLOC+8+ILHFA'\nFTX+CFW+++GALLOZZI'\nFTX+SHP+++VIA ADELFIA ZI'\nFTX+SHP+++RUTIGLIANO BARI'\nFTX+SHP+++ x conto di CARTON PACK'\nFTX+CON+++CARTON PACK'\nFTX+AAA+++*'\nCNT+16:1'\nUNT+19+1072711'\nUNH+1072711+COPARN:D:95B:UN:SMDG20'\nBGM+11+1072711+2'\nRFF+BN:JLG44073A01'\nTDT+20+0261 +1++BCL:172:20+++V7AL8:103::RUTH BCLCHARD'\nNAD+CF+BCL'\nGID+1'\nNAD+CN+CARTON PACK'\nEQD+CN++45G1:102:5++2+5'\nRFF+BN:JLG44073A01'\nEQN+1'\nLOC+11+ILHFA'\nLOC+8+ILHFA'\nMEA+AAE+G+KGM:20400'\nFTX+CFW+++GALLOZZI'\nFTX+SHP+++CARTON PACK'\nFTX+SHP+++VIA ADELFIA ZI'\nFTX+SHP+++RUTIGLIANO BARI'\nFTX+CON+++CARTON PACK'\nFTX+AAA+++*'\nCNT+16:1'\nUNT+20+1072711'\nUNZ+2+123987'\n");

            CreateDataFile(EdiFile3,
                "UNB+UNOA:2+GSL+ITSALSCT+180529:1011+123987+55660'\nUNH+1072711+COPARN:D:95B:UN:SMDG20'\nBGM+12+1072711+9'\nRFF+BN:JLG44073A01'\nTDT+20+0261+1++BCL:172:20+++V7AL8:103::RUTH BCLCHARD'\nNAD+CF+BCL'\nGID+1'\nNAD+CN+CARTON PACK'\nEQD+CN++45G1:102:5++2+4'\nRFF+BN:JLG44073A01'\nEQN+1'\nLOC+11+ILHFA'\nLOC+8+ILHFA'\nFTX+CFW+++GALLOZZI'\nFTX+SHP+++VIA ADELFIA ZI'\nFTX+SHP+++RUTIGLIANO BARI'\nFTX+SHP+++ x conto di CARTON PACK'\nFTX+CON+++CARTON PACK'\nFTX+AAA+++*'\nCNT+16:1'\nUNT+19+1072711'\nUNH+1072711+COPARN:D:95B:UN:SMDG20'\nBGM+11+1072711+2'\nRFF+BN:JLG44073A01'\nTDT+20+0261 +1++BCL:172:20+++V7AL8:103::RUTH BCLCHARD'\nNAD+CF+BCL'\nGID+1'\nNAD+CN+CARTON PACK'\nEQD+CN++45G1:102:5++2+5'\nRFF+BN:JLG44073A01'\nEQN+1'\nLOC+11+ILHFA'\nLOC+8+ILHFA'\nMEA+AAE+G+KGM:20400'\nFTX+CFW+++GALLOZZI'\nFTX+SHP+++CARTON PACK'\nFTX+SHP+++VIA ADELFIA ZI'\nFTX+SHP+++RUTIGLIANO BARI'\nFTX+CON+++CARTON PACK'\nFTX+AAA+++*'\nCNT+16:1'\nUNT+20+1072711'\nUNZ+2+123987'\n");

            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);

        }


        [TestMethod]
        public void COPARNWDifferentBGMBookingRelease()
        {
            MTNInitialize();

            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // 1b. Delete Existing EDI messages relating to this test.
            _ediOperationsForm.DeleteEDIMessages(@"Gate Document", TestCaseNumber, @"Loaded");

            // 2. Load COPARN file
            LoadEDIFile(EdiFile);

            _ediOperationsForm.GetTabTableGeneric("Booking Header");
            MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails,
                @"Action^A~ID^JLG" + TestCaseNumber + "A01~Operator^BCL~Voyage^0261", clickType: ClickType.None,
                rowHeight: 16);

           // _ediOperationsForm.GetTabTableGeneric(@"Request Header", @"6197");
            _ediOperationsForm.GetTabTableGeneric("Request Header");
            MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails,
                @"Action^A~Release Request Number^JLG" + TestCaseNumber + "A01~Voyage^0261",
                clickType: ClickType.None, rowHeight: 16);

            // Step 10 - 13
            RemoveEDIPartyDetails(@"Item^BGM1.1~Value^11~Function^Booking");

            // Step 14 - 16
            LoadEDIFile(EdiFile1);

            //_ediOperationsForm.GetTabTableGeneric(@"Booking Header", @"4036");
            _ediOperationsForm.GetTabTableGeneric("Booking Header");
            _itemFound = MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails,
                @"Action^A~ID^JLG" + TestCaseNumber + "A01~Operator^BCL~Voyage^0261", clickType: ClickType.None,
                rowHeight: 16, doAssert: false);
            Assert.IsTrue(!_itemFound, @"Test Case 44073 - Should not have found a Booking line");

            //_ediOperationsForm.GetTabTableGeneric(@"Request Header", @"6197");
            _ediOperationsForm.GetTabTableGeneric("Request Header");
            MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails,
                @"Action^A~Release Request Number^JLG" + TestCaseNumber + "A01~Voyage^0261",
                clickType: ClickType.None, rowHeight: 16);

            // Step 17 - 19
            RemoveEDIPartyDetails(@"Item^BGM1.1~Value^12~Function^Release Request");

            // Step 20 - 22
            LoadEDIFile(EdiFile2);

            //_ediOperationsForm.GetTabTableGeneric(@"Booking Header", @"4036");
            _ediOperationsForm.GetTabTableGeneric("Booking Header");
            MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails,
                @"Action^A~ID^JLG" + TestCaseNumber + "A01~Operator^BCL~Voyage^0261", clickType: ClickType.None, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails,
                @"Action^A~ID^JLG" + TestCaseNumber + "A01~Operator^BCL~Voyage^0261", clickType: ClickType.None,
                rowHeight: 16, findInstance: 2);

            //_ediOperationsForm.GetTabTableGeneric(@"Request Header", @"6197");
            _ediOperationsForm.GetTabTableGeneric("Request Header");
            _itemFound = MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails,
                @"Action^A~Release Request Number^JLG" + TestCaseNumber + "A01~Voyage^0261",
                clickType: ClickType.None, rowHeight: 16, doAssert: false);

            Assert.IsTrue(!_itemFound, @"Test Case 44073 - Should not have found a Release Request line");

            // Step 23 - 24
            AddPartyMaintenanceAction(@"BGM1.1", @"11", @"Booking");

            // Step 25 - 27
            LoadEDIFile(EdiFile3);

            //_ediOperationsForm.GetTabTableGeneric(@"Booking Header", @"4036");
            _ediOperationsForm.GetTabTableGeneric("Booking Header");
            MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails,
                @"Action^A~ID^JLG" + TestCaseNumber + "A01~Operator^BCL~Voyage^0261", clickType: ClickType.None, rowHeight: 16);

            //_ediOperationsForm.GetTabTableGeneric(@"Request Header", @"6197");
            _ediOperationsForm.GetTabTableGeneric("Request Header");
            _itemFound = MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails,
                @"Action^A~Release Request Number^JLG" + TestCaseNumber + "A01~Voyage^0261",
                clickType: ClickType.None, rowHeight: 16, doAssert: false);

            Assert.IsTrue(!_itemFound, @"Test Case 44073 - Should not have found a Release Request line");

            // Step 28 - 29
            AddPartyMaintenanceAction(@"BGM1.1", @"12", @"Release Request");

            // Step 30 - Being done by resetTerminalConfig script

        }

        private void AddPartyMaintenanceAction(string item, string value, string function)
        {
            if (_ediPartyEnquiryForm == null)
            {
                FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Party Enquiry", forceReset: true);
                _ediPartyEnquiryForm = new EDIPartyEnquiryForm(@"EDI Party Enquiry Form TT1");
            }
            else
            {
                _ediPartyEnquiryForm.SetFocusToForm();
            }

            MTNControlBase.FindClickRowInTable(_ediPartyEnquiryForm.tblDetails, @"Name^44073", ClickType.DoubleClick,
                rowHeight: 16);

            _ediPartyMaintenanceForm = new EDIPartyMaintenanceForm(@"EDI Party Maintenance TT1");
            _ediPartyMaintenanceForm.btnCoparnAdd.DoClick();

            _ediPartyMaintenanceForm.GetCoparnActionTableCombobox();
            _ediPartyMaintenanceForm.cmbTableCell.SetValue(item);
            MTNKeyboard.Type(value);
            MTNKeyboard.Press(VirtualKeyShort.TAB);
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(1));
            MTNKeyboard.Type(function);
            MTNKeyboard.Press(VirtualKeyShort.TAB);
        
            _ediPartyMaintenanceForm.btnSave.DoClick();
        }

        private void RemoveEDIPartyDetails(string rowToDeleteText)
        {

            if (_ediPartyEnquiryForm == null)
            {
                FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Party Enquiry", forceReset: true);
                _ediPartyEnquiryForm = new EDIPartyEnquiryForm(@"EDI Party Enquiry Form TT1");
            }
            else
            {
                _ediPartyEnquiryForm.SetFocusToForm();
            }

            MTNControlBase.FindClickRowInTable(_ediPartyEnquiryForm.tblDetails, @"Name^44073", ClickType.DoubleClick,
                rowHeight: 16);

            _ediPartyMaintenanceForm = new EDIPartyMaintenanceForm(@"EDI Party Maintenance TT1");

            MTNControlBase.FindClickRowInTable(_ediPartyMaintenanceForm.tblActions, rowToDeleteText, clickType: ClickType.Click);
            _ediPartyMaintenanceForm.btnCoparnDelete.DoClick();

            _ediPartyMaintenanceForm.btnSave.DoClick();
        }

        // ReSharper disable once InconsistentNaming
        private void LoadEDIFile(string fileToLoad)
        {
            _ediOperationsForm.SetFocusToForm();

            _ediOperationsForm.LoadEDIMessageFromFile(fileToLoad);

            _ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + fileToLoad);
           _ediOperationsForm.ClickEDIDetailsTab();

        }

    }

}
