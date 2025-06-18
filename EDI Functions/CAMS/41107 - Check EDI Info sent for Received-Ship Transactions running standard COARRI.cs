using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNUtilityClasses.Classes;
using System;
using System.Diagnostics;
using System.Drawing;
using HardcodedData.SystemData;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions.CAMS
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41107 : MTNBase
    {
        private CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

        private BackgroundApplicationForm _backgroundApplicationForm;
        private EDICAMProtocolForm _ediCAMProtocolForm;

        private DateTime _startTS;


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_41107_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            // Stop the CAMS(Server)
            //_backgroundApplicationForm.SetFocusToForm();
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^",
                false);

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void CheckEDIInfoSentForReceivedShipTxnSTANDARDCOARRI()
        {
            
            MTNInitialize();

            _startTS = DateTime.Now;

            // Step 4 - 5
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing");
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^");
           

            // Step 7 - 12
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Operations|Voyage Operations", forceReset: true);
            VoyageOperationsForm voyageOperationsForm = new VoyageOperationsForm();
            voyageOperationsForm.GetSearcherTab_ClassicMode();
            voyageOperationsForm.GetSearcherTab();

            voyageOperationsForm.ChkLoloBays.DoClick();
            //voyageOperationsForm.SetValue(voyageOperationsForm.cmbVoyage, @"MSKA2000001");
            voyageOperationsForm.CmbVoyage.SetValue(Voyage.MSKA2000001, doDownArrow: true);
            //voyageOperationsForm.btnSelect.DoClick();
            voyageOperationsForm.DoSelect();
            voyageOperationsForm.GetMainDetails();

            Point pointToClick = new Point(voyageOperationsForm.TblOnVessel.BoundingRectangle.X + 5,
                voyageOperationsForm.TblOnVessel.BoundingRectangle.Y + 15);
            Mouse.RightClick();

            voyageOperationsForm.ContextMenuSelect(@"Cargo|Create New Cargo On Vessel...");

            RoadGateDetailsReceiveForm roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Add Cargo TT1");
            //roadGateDetailsReceiveForm.GetCargoDetails();

            //roadGateDetailsReceiveForm.SetValue(roadGateDetailsReceiveForm.cmbCargoType, @"ISO CONTAINER");
            roadGateDetailsReceiveForm.CmbCargoType.SetValue(CargoType.ISOContainer, doDownArrow: true, searchSubStringTo: CargoType.ISOContainer.Length - 1);
            //roadGateDetailsReceiveForm.ShowContainerDetails();
            //roadGateDetailsReceiveForm.ShowCommodity();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(100));
            //roadGateDetailsReceiveForm.SetValue(roadGateDetailsReceiveForm.cmbIsoType, @"2200");
            roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            //roadGateDetailsReceiveForm.SetValue(roadGateDetailsReceiveForm.txtCargoId, @"JLG41107A01");
            roadGateDetailsReceiveForm.TxtCargoId.SetValue("JLG41107A01");
            //roadGateDetailsReceiveForm.SetValue(roadGateDetailsReceiveForm.cmbCommodity, @"GEN");
            roadGateDetailsReceiveForm.CmbCommodity.SetValue(Commodity.GEN, doDownArrow: true);
            //roadGateDetailsReceiveForm.SetValue(roadGateDetailsReceiveForm.txtTotalWeight, @"1000");
            roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("1000");
            //roadGateDetailsReceiveForm.SetValue(roadGateDetailsReceiveForm.cmbOperator, @"MSL");
            roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.MSL,  doDownArrow: true);
            //roadGateDetailsReceiveForm.btnSave.DoClick();
            roadGateDetailsReceiveForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(@"Warnings for Pre-Notes TT1");
            warningErrorForm.btnSave.DoClick();

            MTNControlBase.FindClickRowInTable(voyageOperationsForm.TblOnVessel,
                 @"ID^JLG41107A01~Location^MSKA2000001~Total Quantity^1~Cargo Type^ISO Container", ClickType.ContextClick, rowHeight: 16);
           
            voyageOperationsForm.ContextMenuSelect(@"Tranship...");

            // Step 14 
            TranshipForm transhipForm = new TranshipForm(@"JLG41107A01 TT1");

            transhipForm.cmbOutboundVoyage.SetValue(@"MSCK000002	MSC KATYA R.", doDownArrow: true);
            transhipForm.cmbDischargePort.SetValue(Port.AKLNZ);
            transhipForm.cmbDestinationPort.SetValue(Port.ADOJP);
            transhipForm.cmbTranshipTo.SetValue(Port.ADOJP);
            transhipForm.btnOK.DoClick();

            voyageOperationsForm.SetFocusToForm();
           

            // Step 15 - 17
            //voyageOperationsForm.SetValue(voyageOperationsForm.cmbDischargeTo, @"MKBS02");
            voyageOperationsForm.CmbDischargeTo.SetValue(@"MKBS02");
            voyageOperationsForm.TblOnVessel1.FindClickRow(
                new[] { "ID^JLG41107A01~Location^MSKA2000001~Total Quantity^1~Cargo Type^ISO Container~IMEX Status^Tranship" }, 
                ClickType.ContextClick);

            voyageOperationsForm.ContextMenuSelect(@"Actual Discharge | Actual Discharge Selected");

            warningErrorForm = new WarningErrorForm(@"Warnings for Discharge Cargo TT1");
            warningErrorForm.btnSave.DoClick();

            // Step 18
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Admin", forceReset: true);
            TerminalAdministrationForm terminalAdministrationForm = new TerminalAdministrationForm();
            //MTNControlBase.SetValue(terminalAdministrationForm.cmbTable, @"Cam Protocol");
            terminalAdministrationForm.cmbTable.SetValue(@"Cam Protocol");

            var endTS = DateTime.Now;
            endTS = endTS.AddMinutes(2);

            _ediCAMProtocolForm = new EDICAMProtocolForm(@"Terminal Administration TT1");
            //MTNControlBase.FindClickRowInTable(_ediCAMProtocolForm.tblProtocols,
            //    @"Protocol Description^STANDARD COARRI SMDG162~Name^Test40571", xOffset: 40);
            _ediCAMProtocolForm.TblProtocols.FindClickRow(
                new[] { "Protocol Description^STANDARD COARRI SMDG162~Name^Test40571" }, xOffset: 40);

            
            _ediCAMProtocolForm.GetDetailsTabAndDetails();
            _ediCAMProtocolForm.GetScheduledTimesTabAndDetails(findTab: true);

            Trace.TraceInformation(_startTS.ToString());
            Trace.TraceInformation("ddMMyyyy: {0}", _startTS.ToString("ddMMyyyy"));
            Trace.TraceInformation("HHmm: {0}", _startTS.ToString("HHmm"));
            Trace.TraceInformation(endTS.ToString());
            Trace.TraceInformation("ddMMyyyy: {0}", endTS.ToString("ddMMyyyy"));
            Trace.TraceInformation("HHmm: {0}", endTS.ToString("HHmm"));


            //MTNControlBase.SetValue(_ediCAMProtocolForm.txtStartDate, _startTS.ToString(@"ddMMyyyy"),
            //  additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtStartDate.SetValue(_startTS.ToString(@"ddMMyyyy"),
                additionalWaitTimeout: 100);
           // MTNControlBase.SetValue(_ediCAMProtocolForm.txtStartTime, _startTS.ToString(@"HHmm"),
             //   additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtStartTime.SetValue(_startTS.ToString(@"HHmm"),
                additionalWaitTimeout: 100);
            //MTNControlBase.SetValue(_ediCAMProtocolForm.txtEndDate, endTS.ToString(@"ddMMyyyy"),
              //  additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtEndDate.SetValue(endTS.ToString(@"ddMMyyyy"),
                additionalWaitTimeout: 100);
            //MTNControlBase.SetValue(_ediCAMProtocolForm.txtEndTime, endTS.ToString(@"HHmm"),
              //  additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtEndTime.SetValue(endTS.ToString(@"HHmm"),
                additionalWaitTimeout: 100);
            _ediCAMProtocolForm.btnRunAdHoc.DoClick();

            // Adhoc Protocol run popup 1
            ConfirmationFormYesNo _confirmationFormYesNo = new ConfirmationFormYesNo(@"Adhoc Protocol Run");
            _confirmationFormYesNo.btnYes.DoClick();

            // Adhoc Protocol run popup 2
            ConfirmationFormOK _confirmationFormOk = new ConfirmationFormOK(@"Adhoc Protocol Run", automationIdMessage: @"3",
                automationIdOK: @"4");
            _confirmationFormOk.btnOK.DoClick();

            // Step 25 - 28
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG41107A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            _cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(formTitle: 
                "Transactions for JLG41107A01 (Tranship Outbound) TT1");
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, 
                @"Type^EDI Info Sent~Details^CAMS Details double-click to view details...", ClickType.DoubleClick);

            LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm();
            loggingDetailsForm.FindStringInTable(@"Transaction: Received - Ship");
            loggingDetailsForm.FindStringInTable(@"TDT+30+MSCK000002+1++MSK:172:20+++9227302:146::MSC KATYA R.");
            loggingDetailsForm.btnCanel.DoClick();


        }



        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG41107A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS02</locationId>\n            <weight>1000.0000</weight>\n            <imexStatus>Tranship</imexStatus>\n            <commodity>GEN</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads



    }

}
