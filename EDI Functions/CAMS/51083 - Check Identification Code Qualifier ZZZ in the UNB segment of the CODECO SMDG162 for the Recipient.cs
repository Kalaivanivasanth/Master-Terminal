using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
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
using MTNForms.Controls;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions.CAMS
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase51083 : MTNBase
    {
        private BackgroundApplicationForm _backgroundApplicationForm;
        private CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;
       
        private const string TestCaseNumber = @"51083";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";

  
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

        void MTNInitialize() => MTNSignon(TestContext);


        [TestMethod]
        public void CheckIdentificationCodeQualifierZZZinUNBsegmentoftheCODECOfortheRecipient()
        {

            MTNInitialize();
            
            // Step 2 - 6
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: TestCaseNumber);
          
            //roadGateForm.SetValue(roadGateForm.txtRegistration, TestCaseNumber);
            roadGateForm.txtRegistration.SetValue(TestCaseNumber);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            //roadGateForm.SetValue(roadGateForm.txtCarrier, @"American Auto Tpt");
            roadGateForm.cmbCarrier.SetValue("American Auto Tpt");
            //roadGateForm.SetValue(roadGateForm.cmbGate, @"GATE");
            roadGateForm.cmbGate.SetValue("GATE");
            roadGateForm.btnReceiveEmpty.DoClick();

            RoadGateDetailsReceiveForm roadGateDetailsForm =
                new RoadGateDetailsReceiveForm(formTitle: @"Receive Empty Container TT1");
            //roadGateDetailsForm.ShowContainerDetails();

            //roadGateDetailsForm.SetValue(roadGateDetailsForm.cmbIsoType, @"2200");
            roadGateDetailsForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            //roadGateDetailsForm.SetValue(roadGateDetailsForm.txtCargoId, CargoId);
            roadGateDetailsForm.TxtCargoId.SetValue(CargoId);
            //roadGateDetailsForm.SetValue(roadGateDetailsForm.txtTotalWeight, @"2000");
            roadGateDetailsForm.MtTotalWeight.SetValueAndType("2000");
            //roadGateDetailsForm.SetValue(roadGateDetailsForm.txtImex, @"S", additionalWaitTimeout: 2000);
            roadGateDetailsForm.CmbImex.SetValue(IMEX.Storage, additionalWaitTimeout: 2000, doDownArrow: true);
            //roadGateDetailsForm.SetValue(roadGateDetailsForm.cmbOperator, @"ABOC");
            roadGateDetailsForm.CmbOperator.SetValue(Operator.ABOC, doDownArrow: true);

            //FormObjectBase.ClickButton(roadGateDetailsForm.btnSave);
            roadGateDetailsForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Empty~Detail^" + CargoId + "; ABOC; 2200");

            roadGateForm.btnSave.DoClick();

            // Step 7 - 10
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(@"Road Operations TT1", new [] {TestCaseNumber });

         

            // Step 11 - 16
            roadGateForm.SetFocusToForm();

            //roadGateForm.SetValue(roadGateForm.txtRegistration, TestCaseNumber);
            roadGateForm.txtRegistration.SetValue(TestCaseNumber);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            //roadGateForm.SetValue(roadGateForm.txtCarrier, @"American Auto Tpt");
            roadGateForm.cmbCarrier.SetValue("American Auto Tpt");
            //roadGateForm.SetValue(roadGateForm.cmbGate, @"GATE");
            roadGateForm.cmbGate.SetValue("GATE");


            roadGateForm.btnReleaseEmpty.DoClick();

            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm();
            //roadGateDetailsReleaseForm.SetValue(roadGateDetailsReleaseForm.txtCargoId, CargoId);
            roadGateDetailsReleaseForm.TxtCargoId.SetValue(CargoId);
            //roadGateDetailsReleaseForm.SetValue(roadGateDetailsReleaseForm.txtDeliveryReleaseNo, TestCaseNumber);
            roadGateDetailsReleaseForm.TxtDeliveryReleaseNo.SetValue(TestCaseNumber);
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            roadGateForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Release Empty~Detail^" + CargoId + "; ABOC; 2200");

            roadGateForm.btnSave.DoClick();

            // Step 17 - 20
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(@"Road Operations TT1", new [] {TestCaseNumber });

            // Steps 21 - 30
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

            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            _cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for " + CargoId + " TT1");

            string[] linesToCheck =
            {
               @"UNB+UNOA:2+TEST40756:ZZZ+GPHA:ZZZ+210430:2203+0'"
            };
            CheckEDIINFOValid(linesToCheck);
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
