using FlaUI.Core.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Prenotes
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43924 : MTNBase
    {
        private TerminalConfigForm _terminalConfigForm;
        private RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        private RoadGatePickerForm _roadGatePickerForm;
        private ConfirmationFormOKwithText _confirmationFormOKWithText;
        private CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;
        private BackgroundApplicationForm _backgroundApplicationForm;
        private EDICAMProtocolForm _ediCAMProtocolForm;
        private ConfirmationFormYesNo _confirmationFormYesNo;
        private EDIProtocolTaskHistoryForm _ediProtocolTaskHistoryForm;

        private const string TestCaseNumber = @"43924";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup()
        {
            //_backgroundApplicationForm.SetFocusToForm();
            //FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            FormObjectBase.MainForm.OpenBackgroundProcessingFromToolbar();
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^",
                false);

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            
            loadFileDeleteStartTime = DateTime.Now;
            
            CallJadeScriptToRun(TestContext, $"resetData_{TestCaseNumber}");
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();
            
            //FormObjectBase.NavigationMenuSelection(@"Terminal Ops | Terminal Config");
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            _terminalConfigForm = new TerminalConfigForm();

            _terminalConfigForm.GetGenericTabAndTable(@"Defaults");
            //_terminalConfigForm.btnEdit.DoClick();
            _terminalConfigForm.DoEdit();
            MTNControlBase.SetValueInEditTable(_terminalConfigForm.tblGeneric,
                @"Allow Duplicate Prenote Ids for Cargo Types Allowing Dups", @"0", EditRowDataType.CheckBox, doReverse: true);
            //_terminalConfigForm.btnSave.DoClick();
            _terminalConfigForm.DoSave();
            
        }


        [TestMethod] public void InboundDeleteProcessAllowDuplicatePrenoteFalse()
        {
            
            MTNInitialize();

            //FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            FormObjectBase.MainForm.OpenBackgroundProcessingFromToolbar();
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^");

            // Step 8 - 14
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(@"Road Gate TT1", vehicleId: TestCaseNumber);

            roadGateForm.SetFocusToForm();
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);
            roadGateForm.txtNewItem.SetValue($"JLG{TestCaseNumber}", 100);

            _roadGatePickerForm = new RoadGatePickerForm("Picker");
            _roadGatePickerForm.btnSelectAll.DoClick();
            _roadGatePickerForm.btnOK.DoClick();

             _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive General Cargo TT1");
             _roadGateDetailsReceiveForm.BtnSave.DoClick();

           // warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
           //  warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();

            //warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            // warningErrorForm.btnSave.DoClick();

            // Step 15 - 21
            //FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^" + CargoId, ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { $"Cargo ID^{CargoId}"}, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Delete Current Entry");

            _confirmationFormOKWithText = new ConfirmationFormOKwithText(@"Enter the Cancellation value:", controlType: ControlType.Pane);
            //_confirmationFormOKWithText.SetValue(_confirmationFormOKWithText.txtInput, @"Test");
            _confirmationFormOKWithText.txtInput.SetValue(@"Test");
            _confirmationFormOKWithText.btnOK.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Road Ops TT1");
             warningErrorForm.btnSave.DoClick();

            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(@"Road Operations TT1", new [] {TestCaseNumber });

            // Step 22 - 25
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.BreakBulkCargo,
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"Pre-Notified",
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId + "~Voyage^MSCK000002~State^Prenotified");
           cargoEnquiryForm.tblData2.FindClickRow(["ID^" + CargoId + "~Voyage^MSCK000002~State^Prenotified"]);            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

                _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^*** DELETED *** Sub-Term Depot Receive (Road)~Details^Transaction deleted by USERDWAT.  No reason given");
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^*** DELETED *** Received - Road~Details^Transaction deleted by USERDWAT.  No reason given");
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow([
                "Type^*** DELETED *** Sub-Term Depot Receive (Road)~Details^Transaction deleted by USERDWAT.  No reason given",
                "Type^*** DELETED *** Received - Road~Details^Transaction deleted by USERDWAT.  No reason given"
            ]);   
            _cargoEnquiryTransactionForm.CloseForm();

            // Step 26 - 34
            //FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Admin", forceReset: true);
            FormObjectBase.MainForm.OpenTerminalAdminFromToolbar();
            TerminalAdministrationForm terminalAdministrationForm = new TerminalAdministrationForm();
            //MTNControlBase.SetValue(terminalAdministrationForm.cmbTable, @"Cam Protocol");
            terminalAdministrationForm.cmbTable.SetValue(@"Cam Protocol");

            loadFileDeleteStartTime = DateTime.Now;
            var endTS = loadFileDeleteStartTime.AddMinutes(2);
            loadFileDeleteStartTime = loadFileDeleteStartTime.AddMinutes(-2);

            _ediCAMProtocolForm = new EDICAMProtocolForm(@"Terminal Administration TT1");
            /*MTNControlBase.FindClickRowInTable(_ediCAMProtocolForm.tblProtocols,
                @"Protocol Description^User Defined Event JRW-Report~Name^Test43771", xOffset: 40);*/
            _ediCAMProtocolForm.TblProtocols.FindClickRow([@"Protocol Description^User Defined Event JRW-Report", @"Name^Test43771"
            ], xOffset: 40);

            _ediCAMProtocolForm.GetDetailsTabAndDetails();
            _ediCAMProtocolForm.GetScheduledTimesTabAndDetails(findTab: true);

            _ediCAMProtocolForm.txtStartDate.SetValue(loadFileDeleteStartTime.ToString(@"ddMMyyyy"),
                additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtStartTime.SetValue(loadFileDeleteStartTime.ToString(@"HHmm"),
                additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtEndDate.SetValue(endTS.ToString(@"ddMMyyyy"),
                additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtEndTime.SetValue(endTS.ToString(@"HHmm"),
                additionalWaitTimeout: 100);
            _ediCAMProtocolForm.btnRunAdHoc.DoClick();

            // Adhoc Protocol run popup 1
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Adhoc Protocol Run");
            _confirmationFormYesNo.btnYes.DoClick();

            // Adhoc Protocol run popup 2
            ConfirmationFormOK _confirmationFormOk = new ConfirmationFormOK(@"Adhoc Protocol Run", automationIdMessage: @"3",
                automationIdOK: @"4");
            _confirmationFormOk.btnOK.DoClick();

            _ediCAMProtocolForm.SetFocusToForm();
            _ediCAMProtocolForm.GetProtocolTaskHistoryTabAndDetails(@"Protocol Task History [1]");

            //MTNControlBase.FindClickRowInTable(_ediCAMProtocolForm.tblProtocolTaskHistory, @"Task Status^Complete", ClickType.DoubleClick);
            _ediCAMProtocolForm.TblProtocolTaskHistory.FindClickRow(new[] { @"Task Status^Complete" }, ClickType.DoubleClick);

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Warning");
            _confirmationFormYesNo.btnYes.DoClick();

            _ediProtocolTaskHistoryForm = new EDIProtocolTaskHistoryForm(TestContext);
           _ediProtocolTaskHistoryForm.ValidateDetailsExist(["TT1;Deleted;", $";;;{CargoId};"], out var detailsNotFound, doAssert: true);

        }

        private static void CheckFileDetails(List<string> fileLines, string detailToCheck)
        {
            var matchFound = fileLines.Any(t => t.Trim().Contains(detailToCheck));

            Assert.IsTrue(matchFound,
                @"TestCase43924 - The following details where not found in the file:\n" + detailToCheck);
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber + "_";

            // Delete Prenote
           // CreateDataFileToLoad(@"DeletePrenote.xml",
           //     "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED;EDI_STATUS_DBLOADED_PARTIAL;EDI_STATUS_DBLOADED_PARTIAL_X</operationsToPerformStatuses>\n        <Prenote Terminal='TT1'>\n            <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n            <id>JLG43924A01</id>\n             <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>D</messageMode>\n        </Prenote>\n    </AllPrenote>\n</SystemXMLPrenote>\n\n\n\n");

            // Delete Cargo On Site
            //CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
            //    "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n         <id>JLG43924A02</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS02</locationId>\n         <weight>2000</weight>\n         <imexStatus>Import</imexStatus>\n         <voyageCode>MSCK000002</voyageCode>\n         <totalQuantity>1</totalQuantity>\n             <messageMode>D</messageMode>\n		 </CargoOnSite>\n		 	<CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n         <id>JLG43924A03</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS02</locationId>\n         <weight>2000</weight>\n         <imexStatus>Import</imexStatus>\n         <voyageCode>MSCK000002</voyageCode>\n         <totalQuantity>1</totalQuantity>\n          <messageMode>D</messageMode>\n		 </CargoOnSite>\n		 	<CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n         <id>JLG43924A04</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS02</locationId>\n         <weight>2000</weight>\n         <imexStatus>Import</imexStatus>\n         <voyageCode>MSCK000002</voyageCode>\n         <totalQuantity>1</totalQuantity>\n               <messageMode>D</messageMode>\n		 </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");

            // Create Prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <Prenote Terminal='TT1'>\n            <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n            <id>JLG43924A01</id>\n             <commodity>GENL</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS02</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n		   <Prenote Terminal='TT1'>\n            <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n            <id>JLG43924A02</id>\n             <commodity>GENL</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS02</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n		   <Prenote Terminal='TT1'>\n            <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n            <id>JLG43924A03</id>\n             <commodity>GENL</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS02</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n		   <Prenote Terminal='TT1'>\n            <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n            <id>JLG43924A04</id>\n             <commodity>GENL</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS02</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n    </AllPrenote>\n</SystemXMLPrenote>\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
           

        }


    }

}
