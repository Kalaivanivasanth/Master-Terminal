using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions.X12
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43931 : MTNBase
    {

        private BackgroundApplicationForm _backgroundApplicationForm;
        private EDICAMProtocolForm _ediCAMProtocolForm;
        private ConfirmationFormYesNo _confirmationFormYesNo;
        private ConfirmationFormOK _confirmationFormOk;
        private CargoEnquiryForm _cargoEnquiryForm;
        private CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

       
        [TestCleanup]
        public new void TestCleanup()
        {

         

            base.TestCleanup();
        }
        
        private void MTNTestInitialize()
        {
            MTNSignon(TestContext);

            // Start the CAMS(Server)
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing");
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"CAMS");
        }

        
        [TestMethod]
        public void Ship()
        {

            MTNTestInitialize();

            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_43931");

            // Open EDI Functions | Cam Protocol
            FormObjectBase.NavigationMenuSelection(@"EDI Functions | EDI CAMS Protocols", forceReset: true);
            _ediCAMProtocolForm = new EDICAMProtocolForm();

            MTNControlBase.FindClickRowInTable(_ediCAMProtocolForm.tblProtocols,
                @"Protocol Description^X12 322 - Operations Activity~Name^X12 322 EDI Messages", xOffset: 40);
            _ediCAMProtocolForm.GetDetailsTabAndDetails();

            // removed / from dates and : from time due to copy/paste to Typing change
            // Cargo ID JLG43931013
            _ediCAMProtocolForm.SetRelaxedIdChecking(TestContext, false);
            RunAdhocReport(@"02042019", @"0000", @"02042019", @"0100", getTab: true); // Cargo ID JLG43931013
            RunAdhocReport(@"02042019", @"0100", @"02042019", @"0200");    // Cargo ID JLG4393101
            RunAdhocReport(@"02042019", @"0200", @"02042019", @"0300");   // Cargo ID JLG43931
            RunAdhocReport(@"02042019", @"0300", @"02042019", @"0400");   // Cargo ID JLGG4393101

            _ediCAMProtocolForm.SetRelaxedIdChecking(TestContext, true);
            RunAdhocReport(@"02042019", @"0000", @"02042019", @"0100", getTab: true); // Cargo ID JLG43931013
            RunAdhocReport(@"02042019", @"0100", @"02042019", @"0200");    // Cargo ID JLG4393101
            RunAdhocReport(@"02042019", @"0200", @"02042019", @"0300");   // Cargo ID JLG43931
            RunAdhocReport(@"02042019", @"0300", @"02042019", @"0400");   // Cargo ID JLGG4393101

            // Search for the cargo
            SearchOnCargoEnquiry(@"JLGG439, JLG43931");

            // Cargo ID = JLG43931013
            string[] linesToCheck1 =
            {
                @"Transaction: Released - Ship",
                @"N7**JLG4393101*2500*G*400******CN*ALL***02000*A*K**HH***FLTP*ALL*~",
                @"W2**JLG4393101**CN*L***********~"
            };

            string[] linesToCheck2 =
            {
                @"Transaction: Received - Ship",
                @"N7**JLG4393101*2500*G*400******CN*ALL***02000*A*K**HH***FLTP*ALL*~",
                @"W2**JLG4393101**CN*L***********~"
            };

            string[] linesToCheck3 =
            {
                @"Transaction: Released - Ship",
                @"N7*JLG*4393101*2500*G*400******CN*ALL***02000*A*K*3*HH***FLTP*ALL*~",
                @"W2*JLG*4393101**CN*L********3***~"
            };

            string[] linesToCheck4 =
            {
                @"Transaction: Received - Ship",
                @"N7*JLG*4393101*2500*G*400******CN*ALL***02000*A*K*3*HH***FLTP*ALL*~",
                @"W2*JLG*4393101**CN*L********3***~"
            };

            ValidateEDIInfoIsCorrect(@"Location ID^MKBS01~ID^JLG43931013~Type^FLTP",
                linesToCheck1, linesToCheck2, linesToCheck3, linesToCheck4);

            // Cargo ID = JLG4393101
            linesToCheck1 = new string[]
            {
                @"Transaction: Released - Ship",
                @"N7**JLG4393101*2500*G*400******CN*ALL***02000*A*K**HH***FLTP*ALL*~",
                @"W2**JLG4393101**CN*L***********~"
            };

            linesToCheck2 = new string[]
            {
                @"Transaction: Received - Ship",
                @"N7**JLG4393101*2500*G*400******CN*ALL***02000*A*K**HH***FLTP*ALL*~",
                @"W2**JLG4393101**CN*L***********~"
            };

            linesToCheck3 = new string[]
            {
                @"Transaction: Released - Ship",
                @"N7***2500*G*400******CN*ALL***02000*A*K**HH***FLTP*ALL*~",
                @"W2****CN*L***********~"
            };

            linesToCheck4 = new string[]
            {
                @"Transaction: Received - Ship",
                @"N7***2500*G*400******CN*ALL***02000*A*K**HH***FLTP*ALL*~",
                @"W2****CN*L***********~"
            };

            ValidateEDIInfoIsCorrect(@"Location ID^MKBS01~ID^JLG4393101~Type^FLTP",
                linesToCheck1, linesToCheck2, linesToCheck3, linesToCheck4);

            // Cargo ID = JLG43931
            linesToCheck1 = new string[]
            {
                @"Transaction: Released - Ship",
                @"N7**JLG43931*2500*G*400******CN*ALL***02000*A*K**HH***TRAI*ALL*~",
                @"W2**JLG43931**CN*L***********~"
            };

            linesToCheck2 = new string[]
            {
                @"Transaction: Received - Ship",
                @"N7**JLG43931*2500*G*400******CN*ALL***02000*A*K**HH***TRAI*ALL*~",
                @"W2**CNW43931**CN*L***********~"
            };

            linesToCheck3 = new string[]
            {
                @"Transaction: Released - Ship",
                @"N7***2500*G*400******CN*ALL***02000*A*K**HH***TRAI*ALL*~",
                @"W2****CN*L***********~"
            };

            linesToCheck4 = new string[]
            {
                @"Transaction: Received - Ship",
                @"N7***2500*G*400******CN*ALL***02000*A*K**HH***TRAI*ALL*~",
                @"W2****CN*L***********~"
            };

            ValidateEDIInfoIsCorrect(@"Location ID^MKBS01~ID^JLG43931~Type^TRAI",
                linesToCheck1, linesToCheck2, linesToCheck3, linesToCheck4);

            // Cargo ID = JLGG4393101
            linesToCheck1 = new string[]
            {
                @"Transaction: Released - Ship",
                @"N7*JLGG*439310*2500*G*400******CN*ALL***02000*A*K*1*HH***2200*ALL*~",
                @"W2*JLGG*439310**CN*L********1***~"
            };

            linesToCheck2 = new string[]
            {
                @"Transaction: Received - Ship",
                @"N7*JLGG*439310*2500*G*400******CN*ALL***02000*A*K*1*HH***2200*ALL*~",
                @"W2*JLGG*439310**CN*L********1***~"
            };

            linesToCheck3 = new string[]
            {
                @"Transaction: Released - Ship",
                @"N7*JLGG*439310*2500*G*400******CN*ALL***02000*A*K*1*HH***2200*ALL*~",
                @"W2*JLGG*439310**CN*L********1***~"
            };

            linesToCheck4 = new string[]
            {
                @"Transaction: Received - Ship",
                @"N7*JLGG*439310*2500*G*400******CN*ALL***02000*A*K*1*HH***2200*ALL*~",
                @"W2*JLGG*439310**CN*L********1***~"
            };

            ValidateEDIInfoIsCorrect(@"Location ID^MKBS01~ID^JLGG4393101~Type^2200",
                linesToCheck1, linesToCheck2, linesToCheck3, linesToCheck4);

        }

        

        #region - Methods

        private void CheckLogDetailsExist(string[] linesToCheck, int instance)
        {
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^EDI Info Sent",
                ClickType.DoubleClick, findInstance: instance);

            // Check logging details
            LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm(@"Logging Details");

            loggingDetailsForm.FindStringsInTable(linesToCheck);
            loggingDetailsForm.DoCancel();
        }

        private void SearchOnCargoEnquiry(string cargoToSearchFor)
        {
            // check the details in the transactions on the Cargo Enquiry
            if (_cargoEnquiryForm == null)
            {
                FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry", forceReset: true);
                _cargoEnquiryForm = new CargoEnquiryForm();
            }
            else
            {
                _cargoEnquiryForm.SetFocusToForm();
            }

            MTNControlBase.SetValueInEditTable(_cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", cargoToSearchFor);
            MTNControlBase.SetValueInEditTable(_cargoEnquiryForm.tblSearchCriteria, @"Site (Current)",
                CargoEnquiryForm.SiteType.Anywhere, EditRowDataType.ComboBoxEdit);
            //_cargoEnquiryForm.btnSearch.DoClick();
            _cargoEnquiryForm.DoSearch();

        }

        private void DisplayTransactionsForCargoItem(string rowToFind)
        {
            MTNControlBase.FindClickRowInTable(_cargoEnquiryForm.tblData, rowToFind, ClickType.ContextClick, SearchType.Exact);
            //_cargoEnquiryForm.btnViewTransaction.DoClick();
            _cargoEnquiryForm.DoViewTransactions();

            _cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm();

        }


        private void RunAdhocReport(string startDate, string startTime, string endDate, string endTime, bool getTab = false)
        {
            // Go to Scheduled Times tab and run adhoc with specified times
            _ediCAMProtocolForm.GetDetailsTabAndDetails();
            _ediCAMProtocolForm.GetScheduledTimesTabAndDetails(findTab: true);
            //  MTNControlBase.SetValue(_ediCAMProtocolForm.txtStartDate, startDate, additionalWaitTimeout:100);
            _ediCAMProtocolForm.txtStartDate.SetValue(startDate, additionalWaitTimeout: 100);
           // MTNControlBase.SetValue(_ediCAMProtocolForm.txtStartTime, startTime, additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtStartTime.SetValue(startTime, additionalWaitTimeout: 100);
           // MTNControlBase.SetValue(_ediCAMProtocolForm.txtEndDate, endDate, additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtEndDate.SetValue(endDate, additionalWaitTimeout: 100);
            //MTNControlBase.SetValue(_ediCAMProtocolForm.txtEndTime, endTime, additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtEndTime.SetValue(endTime, additionalWaitTimeout: 100);
            _ediCAMProtocolForm.btnRunAdHoc.DoClick();


            // Adhoc Protocol run popup 1
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Adhoc Protocol Run");
            _confirmationFormYesNo.CheckMessageMatch(@"Are you sure you want to process the CAM Protocol X12 322 EDI Messages ?");
            _confirmationFormYesNo.btnYes.DoClick();

            // Adhoc Protocol run popup 2
            _confirmationFormOk = new ConfirmationFormOK(@"Adhoc Protocol Run", automationIdMessage: @"3",
                automationIdOK: @"4");
            _confirmationFormOk.btnOK.DoClick();
        }

        private void ValidateEDIInfoIsCorrect(string forCargoLine, string[] linesToCheck1,
            string[] linesToCheck2, string[] linesToCheck3, string[] linesToCheck4)
        {
            _cargoEnquiryForm.SetFocusToForm();

            // Display the transaction
            DisplayTransactionsForCargoItem(forCargoLine);

            // View transactions
            CheckLogDetailsExist(linesToCheck1, 1);
            CheckLogDetailsExist(linesToCheck2, 2);
            CheckLogDetailsExist(linesToCheck3, 3);
            CheckLogDetailsExist(linesToCheck4, 4);

            //_cargoEnquiryTransactionForm.btnCancel.DoClick();
            _cargoEnquiryTransactionForm.DoCancel();
        }

        #endregion - View Transactions

    }

}
