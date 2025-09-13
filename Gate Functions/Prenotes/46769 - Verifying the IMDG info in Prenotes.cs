using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Bookings;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Prenotes
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase46769 : MTNBase
    {

        EDIOperationsForm _ediOperations;

        private DateTime _currentDateTime = DateTime.Now;

      
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        

        [TestCleanup]
        public new void TestCleanup()
        {
            // Step 25 - 26
            SetTerminalConfigValues(@"1", @"1");

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();

            // Step 3
            SetTerminalConfigValues(@"0", @"0");

            // load the edi file
            //FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            _ediOperations = new EDIOperationsForm();

            // Step 4 - 5
            searchFor = @"_46769_";
            loadFileDeleteStartTime = DateTime.Now; // REMOVE AT YOUR OWN PERIL
            SetupAndLoadInitializeData(TestContext);

            // Delete any Existing EDI messages relating to this test.
            _ediOperations.DeleteEDIMessages(@"Gate Document", @"46769", @"Loaded");

            // Load, find and click on loaded message to refresh data
            _ediOperations.LoadEDIMessageFromFile(@"2" + searchFor + "EDI_COPARN_ADD.edi");
            //MTNControlBase.FindClickRowInTable(ediOperations.tblEDIMessages, 
            //    @"Status^Loaded~File Name^2" + searchFor + "EDI_COPARN_ADD.edi", rowHeight: 16, xOffset: 200,
            //    clickType: ClickType.ContextClick);
            _ediOperations.TblMessages.FindClickRow([$"Status^Loaded~File Name^2{searchFor}EDI_COPARN_ADD.edi"],
                xOffset: 200, clickType: ClickType.ContextClick);
            _ediOperations.ContextMenuSelect(@"Load To DB");
        }

        [TestMethod]
        public void VerifyingIMDGinfoInPrenotes()
        {
            MTNInitialize();
            
            // Step 6
                           
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1500));   // needed as have to wait for file to be db loaded
                                                                           //ediOperations.ClickButton(ediOperations.btnSearch);
            //MTNControlBase.FindClickRowInTable(ediOperations.tblEDIMessages, 
            //    @"Status^DB Loaded~File Name^2_46769_A_EDI_COPARN_ADD.edi", rowHeight: 16, xOffset: 200);
            //_ediOperations.TblEDIMessages.FindClickRow(@"Status^DB Loaded~File Name^2_46769_A_EDI_COPARN_ADD.edi",  xOffset: 200);
            _ediOperations.TblMessages.FindClickRow(["Status^DB Loaded~File Name^2_46769_A_EDI_COPARN_ADD.edi"], xOffset: 200);
           
            //ediOperations.ShowPrenoteDataTable();
            //ediOperations.GetTabTableGeneric(@"Prenote", @"6187");  // 6166 // 6186
            _ediOperations.GetTabTableGeneric("Prenote");  // 6166 // 6186
            // MTNControlBase.FindClickRowInTable(_ediOperations.tblEDIDetails,
                // @"ID^JLG46769A01~IMEX Status^Export~Booking^JLGBOOK46769A01~Hazards^8 (1760)", ClickType.None);
            _ediOperations.TabGeneric.TableWithHeader.FindClickRow(["ID^JLG46769A01~IMEX Status^Export~Booking^JLGBOOK46769A01~Hazards^8 (1760)"], ClickType.None);
            // Step 7
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate ", forceReset:true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(vehicleId: @"46769");

            // Step 8
            roadGateForm.PopulateRegCarrierGateNewItemClickButton(@"46769", @"American Auto Tpt", @"GATE", @"JLG46769A01");
            RoadGateDetailsReceiveForm roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Full Container TT1");

            // Step 9
            //roadGateDetailsReceiveForm.GetCargoDetails();
            Assert.IsTrue(roadGateDetailsReceiveForm.TxtHazardDetails.GetText() .Equals(@"8 (1760)"), 
                @"TestCase46769 - Hazards Details do not match.  Actual: " + 
                roadGateDetailsReceiveForm.TxtHazardDetails.GetText() + " Expected: 8 (1760)");

            // Step 10 - 13
            roadGateDetailsReceiveForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            string[] warningErrorToCheck = new string[]
            {
                "Code :75016. The Container Id (JLG46769A01) failed the validity checks and may be incorrect.",
                "Code :75737. The operator MSK requires a consignor for item JLG46769A01."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorToCheck = new string[]
            {
                "Code :75737. The operator MSK requires a consignor for item JLG46769A01."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();
            
            // Step 14 - 16
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, 
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG46769A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, 
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
           //cargoEnquiryForm.btnSearch.DoClick();
           cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG46769A01", rowHeight: 18);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG46769A01"]);            cargoEnquiryForm.CargoEnquiryGeneralTab();
            var tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Hazard Details");
            Assert.IsTrue(tableValue.Equals("8 (1760)"),
                "TestCase46769 - Data mismatch on Hazard Details field: Expected=8 (1760); Actual=" + tableValue);

            // Step 17 - 20: Delete current entry - Road Operations
            //FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG46769A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] {"Cargo ID^JLG46769A01"}, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Delete Current Entry");
            ConfirmationFormOKwithText confirmationFormOKwithText = new ConfirmationFormOKwithText(@"Enter the Cancellation value:", controlType: ControlType.Pane);
            //confirmationFormOKwithText.SetValue(confirmationFormOKwithText.txtInput, @"Testing");
            confirmationFormOKwithText.txtInput.SetValue(@"Testing");
            confirmationFormOKwithText.btnOK.DoClick();
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Road Ops TT1");
            warningErrorForm.btnSave.DoClick();

            // Step 20 - 24: Delete booking
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Booking", forceReset: true);
            FormObjectBase.MainForm.OpenBookingFromToolbar();
            BookingForm bookingForm = new BookingForm(@"Booking TT1");
            bookingForm.DeleteBookings(@"JLGBOOK46769A01");
            bookingForm.CloseForm();

        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"PrenoteSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <Prenote Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG46769A01</id>\n            <imexStatus>Export</imexStatus>\n			<isoType>2200</isoType>\n             <weight>8000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSK</operatorCode>\n			<dischargePort>NZLYT</dischargePort>\n			<locationId>WHEAT</locationId>\n			<transportMode>Road</transportMode>\n			<messageMode>D</messageMode>\n        </Prenote>\n    </AllPrenote>\n</SystemXMLPrenote>\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);

            // Coparn Add file
            searchFor = @"_46769_A_";
            CreateDataFileToLoad(@"COPARN_ADD.edi",
               "UNB+UNOA:1+MSK:ZZ:SCAC+ITSALSCT+190910:1020+51240+++++MSK'\nUNH+5124000001+COPARN:D:95B:UN'\nBGM+11+20190910102012+5'\nRFF+BN:6066903530'\nTDT+20+BISLC0001+1++MSK:172:20+++BISLCC:103::MAERSK NESTON'\nLOC+88+ITSAL:139:6'\nLOC+9+ITSAL:139:6'\nDTM+133:201909132000:203'\nNAD+FW+10800009730+BDP INTERNATIONAL NV'\nNAD+CA+MSK:172:20'\nGID+1'\nFTX+AAA+++CHEMICAL PRODUCTS, NOS'\nDGS+IMD+8+1760+113:CEL+3+F-AS-B'\nFTX+AAD+++CORROSIVE LIQUID, N.O.S., PACKING GROUP III.:-'\nEQD+CN+JLG46769A01+2200:102:5+2+2+5'\nRFF+BN:JLGBOOK46769A01'\nRFF+SQ:P9CGUESH7ZU3B1'\nTMD+3++2'\nLOC+8+IDJKT:139:6'\nLOC+98+ITSAL:139:6'\nLOC+11+NZPOE:139:6'\nMEA+AAE+T+KGM:2210'\nMEA+AAE+VGM+KGM:8955,2'\nFTX+AAI+++CLOSING 2019-09-12 030000HRS.'\nTDT+30+937E+1++MSK:172:20+++OWIZ2:103::MAERSK MC-KINNEY MOLLER'\nLOC+11+SGSIN:139:6'\nCNT+16:1'\nUNT+27+5124000001'\nUNZ+1+51240'");

        }

        #endregion - Setup and Run Data Loads

        private void SetTerminalConfigValues(string bookingReleaseRatherThanEDIGateDocument, string useDetailsHazardousEntryScreen)
        {
            //FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.GetDetailsTab();
            //FormObjectBase.ClickButton(terminalConfigForm.btnEdit);
            //terminalConfigForm.btnEdit.DoClick();
            //terminalConfigForm.DoEdit();

            //MTNControlBase.FindTabOnForm(terminalConfigForm.tabDetails, @"Settings");
            terminalConfigForm.GetGenericTabAndTable(@"Settings");
            terminalConfigForm.DoEdit();
            MTNControlBase.SetValueInEditTable(terminalConfigForm.tblGeneric,
                @"Hazardous - Use Detailed Hazardous Entry Screen",
                useDetailsHazardousEntryScreen, EditRowDataType.CheckBox);

            //MTNControlBase.FindTabOnForm(terminalConfigForm.tabDetails, @"EDI", false);
            terminalConfigForm.GetEDITab();
            MTNControlBase.SetValueInEditTable(terminalConfigForm.tblEDI,
                @"Use EDI Booking Release Req rather than EDI Gate Document",
                bookingReleaseRatherThanEDIGateDocument, EditRowDataType.CheckBox);

            //FormObjectBase.ClickButton(terminalConfigForm.btnSave);
            //terminalConfigForm.btnSave.DoClick();
            terminalConfigForm.DoSave();
            terminalConfigForm.CloseForm();
        }

    }

}
