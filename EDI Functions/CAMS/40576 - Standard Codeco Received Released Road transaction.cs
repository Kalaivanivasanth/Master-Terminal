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
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using HardcodedData.SystemData;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions.CAMS
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40756 : MTNBase
    {
        
        private BackgroundApplicationForm _backgroundApplicationForm;
        private CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;
       
        private const string TestCaseNumber = @"40576";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";

        private static readonly string MarkDetails = TestCaseNumber + millisecondsSince20000101.ToString();

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            // Stop the CAMS(Server)
            //_backgroundApplicationForm?.SetFocusToForm();
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm?.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^",
                false);

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            MTNSignon(TestContext);
            
            CallJadeScriptToRun(TestContext, $@"resetData_{TestCaseNumber}");
        }


        [TestMethod]
        public void RunStandardCodecoForReceivedReleasedRoadTransactionsCheckEDIInfoSent()
        {
            MTNInitialize();

            // Step 2 - 6
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: TestCaseNumber);

            /*roadGateForm.cmbCarrier.SetValue("American Auto Tpt");
            roadGateForm.txtRegistration.SetValue(TestCaseNumber);
            roadGateForm.cmbGate.SetValue("GATE");*/
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);
            roadGateForm.btnReceiveEmpty.DoClick();

            RoadGateDetailsReceiveForm roadGateDetailsForm =
                new RoadGateDetailsReceiveForm(formTitle: @"Receive Empty Container TT1");
            //roadGateDetailsForm.ShowContainerDetails();

            roadGateDetailsForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            roadGateDetailsForm.TxtCargoId.SetValue(CargoId);
            roadGateDetailsForm.MtTotalWeight.SetValueAndType("2000");
            roadGateDetailsForm.CmbImex.SetValue(IMEX.Storage, additionalWaitTimeout: 2000, doDownArrow: true);
            roadGateDetailsForm.CmbOperator.SetValue(Operator.ABOC, doDownArrow: true);

            //roadGateDetailsForm.GetCargoDetails();
            roadGateDetailsForm.TxtOperatorSeal.SetValue("OPSEAL001");
            roadGateDetailsForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Empty~Detail^" + CargoId + "; ABOC; 2200");

            roadGateForm.btnSave.DoClick();

            // Step 7 - 10
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(@"Road Operations TT1", new[] {TestCaseNumber});

            // Step 11 - 13
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.SearchToolbar,
                (int)CargoEnquiryForm.Toolbar.SearcherToolbar.Search, "Search");

            //cargoEnquiryForm.btnEdit.DoClick();
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.MainToolbar,
                (int)CargoEnquiryForm.Toolbar.MainToolbar.Edit, "Edit");
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Mark", MarkDetails);
            cargoEnquiryForm.GetGenericTabTableDetails(@"Status", @"4087");

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Shippers Seal", @"Seal-40756", EditRowDataType.Table);
            //cargoEnquiryForm.btnSave.DoClick();
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.MainToolbar,
                (int)CargoEnquiryForm.Toolbar.MainToolbar.Save, "Save");

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Tracked Item Update TT1");
            warningErrorForm.btnSave.DoClick();

            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Shippers Seal", @"Seal-40756");

            // Step 14 - 17
            roadGateForm.SetFocusToForm();

            /*roadGateForm.cmbCarrier.SetValue("American Auto Tpt");
            roadGateForm.txtRegistration.SetValue(TestCaseNumber);
            roadGateForm.cmbGate.SetValue("GATE");*/
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);
            roadGateForm.btnReleaseEmpty.DoClick();

            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm();
            roadGateDetailsReleaseForm.TxtCargoId.SetValue(CargoId);
            roadGateDetailsReleaseForm.TxtDeliveryReleaseNo.SetValue(TestCaseNumber);
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            roadGateForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Release Empty~Detail^" + CargoId + "; ABOC; 2200");

            roadGateForm.btnSave.DoClick();

            // Step 18 - 21
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(@"Road Operations TT1", new [] {TestCaseNumber});

            // Steps 22 - 30
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            FormObjectBase.NavigationMenuSelection(@"EDI Functions | EDI CAMS Protocols", forceReset: true);
            EDICAMProtocolForm ediCAMProtocolForm = new EDICAMProtocolForm();

            MTNControlBase.FindClickRowInTable(ediCAMProtocolForm.tblProtocols,
                @"Protocol Description^STANDARD CODECO SMDG162~Name^Test40756", xOffset: 40);

            var endDateTime = DateTime.Now.AddMinutes(1);

            ediCAMProtocolForm.GetDetailsTabAndDetails();
            ediCAMProtocolForm.RunAdhocReport(loadFileDeleteStartTime.ToString(@"ddMMyyyy"),
                loadFileDeleteStartTime.ToString(@"HHmm"), endDateTime.ToString(@"ddMMyyyy"),
                endDateTime.ToString(@"HHmm"), getTab: true);

            // Step 31 - 35
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"Off Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Mark", MarkDetails);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.SearchToolbar,
                (int)CargoEnquiryForm.Toolbar.SearcherToolbar.Search, "Search");

            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.MainToolbar,
                (int)CargoEnquiryForm.Toolbar.MainToolbar.ViewTransactions, "View Transactions");
            _cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for " + CargoId + " TT1");

            string[] linesToCheck =
            {
                @"Transaction: Released - Road",
                @"EQD+CN+" + CargoId + @"+2200:102:5+++4'",
                @"SEL+OPSEAL001+CA'",
                @"SEL+Seal_40756+SH'"
            };
            CheckEDIINFOValid(linesToCheck);

            linesToCheck = new string[]
            {
                @"Transaction: Received - Road",
                @"EQD+CN+" + CargoId + @"+2200:102:5+++4'",
                @"SEL+OPSEAL001+CA'"
            };
            CheckEDIINFOValid(linesToCheck, 2);
            
            _cargoEnquiryTransactionForm.CloseForm();
        }

        private void CheckEDIINFOValid(string[] linesToCheck, int instance = 1)
        {
            _cargoEnquiryTransactionForm.SetFocusToForm();

            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^EDI Info Sent",
                ClickType.DoubleClick, findInstance: instance);

            // Check logging details
            LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm(@"Logging Details");

            loggingDetailsForm.FindStringsInTable(linesToCheck);
            loggingDetailsForm.DoCancel();
        }

        
    }

}
