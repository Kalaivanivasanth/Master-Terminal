using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNUtilityClasses.Classes;
using System;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using FlaUI.Core.AutomationElements;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using DataGridViewRow = FlaUI.Core.AutomationElements.DataGridViewRow;

// Date		   Who		    Reason
//17/01/2022  NAVMP4     Initial creation

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions.CAMS
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40571 : MTNBase
    {
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;
        OrderOfWorkForm _orderOfWorkForm;
        VoyageJobOperationsForm _voyageJobOperationsForm;
        BackgroundApplicationForm _backgroundApplicationForm;

        DateTime _startTS = DateTime.Now;

        const string TestCaseNumber = @"40571";
        const string CargoId = @"JLG" + TestCaseNumber + @"A01";
        const string VoyageId = @"VOY_40571";


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup()
        {
            // Stop the CAMS(Server)
            //_backgroundApplicationForm?.SetFocusToForm();
            //FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            FormObjectBase.MainForm.OpenBackgroundProcessingFromToolbar();
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm?.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^",
                false);

            base.TestCleanup();
        }
        
        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();

            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);          
           
        }


        [TestMethod]
        public void EDIDataTranslationsVoyageTranslationsForATranshipCargoItem()
        {
            
            //Step 1 - LOLO colour coding, Job load to operations are now done by resetData_40571
            MTNInitialize();

            //Step 2 - Open Order of Work form and get the Job Number
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|Order Of Work");
            _orderOfWorkForm = new OrderOfWorkForm(@"Order of Work TT1");

            _orderOfWorkForm.cmbVoyage.SetValue(Voyage.VOY_40571);
            _orderOfWorkForm.DoFind();

            _orderOfWorkForm.CreateOOWAllJobsTable();

            List<DataGridViewRow> rowInTable =
                new List<DataGridViewRow>
                (_orderOfWorkForm.tblOOWAllJobs.AsDataGridView()
                    .Rows
                .Where(r => r.Cells.Any() && r.Cells[1].Name.Contains(@"11  OD") &&
                       r.Cells[2].Name.Contains(@"Discharge") && r.Cells[3].Name.Contains(@"CRN1")));

            Assert.IsTrue(rowInTable.Count > 0,
                @"TestCase40571 - Can not find required OOW details: Aboard^11 OD~Type^Discharge~Crane^CRN1");

            var jobNumber = Miscellaneous.ReturnTextFromTableString(rowInTable[0].Cells[0].Name);

            // Step 3 - On the Job Operations, Queue the Job and Move It
            FormObjectBase.NavigationMenuSelection(@"Operations|Job Operations", forceReset: false);
            _voyageJobOperationsForm = new VoyageJobOperationsForm(@"Job Operations TT1");

            //MTNControlBase.SendTextToCombobox(_voyageJobOperationsForm.cmbVoyage, @"VOY_40571 JOLLY GRIGIO");
            _voyageJobOperationsForm.cmbVoyage.SetValue(Voyage.VOY_40571);
            _voyageJobOperationsForm.lstVoyageJobs.FindItemInList(@"2036", @"VOY_40571|CRN1|Import  " + jobNumber);

            _voyageJobOperationsForm.GetJobDetails();

            MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                @"Location Id^118219~Seq.^1~Cargo ID^JLG40571A01~Total Quantity^1",
                clickType: ClickType.ContextClick, rowHeight: 16);
            _voyageJobOperationsForm.ContextMenuSelect(@"Queue | Queue Selected");

            //_voyageJobOperationsForm.btnMoveIt.DoClick();
            _voyageJobOperationsForm.DoMoveIt();

            // Step 4 - Start the CAMS Protocol
            //FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            FormObjectBase.MainForm.OpenBackgroundProcessingFromToolbar();
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^");
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(1));

            // Step 5 - Adhoc run the EDI Cams Protocol
            FormObjectBase.NavigationMenuSelection(@"EDI Functions | EDI CAMS Protocols", forceReset: true);
            EDICAMProtocolForm ediCAMProtocolForm = new EDICAMProtocolForm();
            MTNControlBase.FindClickRowInTable(ediCAMProtocolForm.tblProtocols,
                 @"Protocol Description^STANDARD COARRI SMDG162~Name^Test40571", xOffset: 40);

            var endDateTime = DateTime.Now.AddMinutes(2);

            ediCAMProtocolForm.GetDetailsTabAndDetails();
            ediCAMProtocolForm.RunAdhocReport(_startTS.ToString(@"ddMMyyyy"),
                _startTS.ToString(@"HHmm"), endDateTime.ToString(@"ddMMyyyy"),
                endDateTime.ToString(@"HHmm"), getTab: true);

            // Step 6 - Open Cargo Enquiry and View the Transactions
            //FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
           
            // Step 7 Check the EDI Info Sent
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId, rowHeight: 18);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            _cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for " + CargoId + " (Tranship Outbound) TT1");

            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^EDI Info Sent",
                ClickType.DoubleClick);

            // Step 8 Check logging details
            LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm(@"Logging Details");

            string[] linesToCheck =
            {
                 @"UNA:+.?",
                 @"TDT+20+004+1",
                 @"RFF+VON:004",
                 @"TDT+30+006"
             };

            loggingDetailsForm.FindStringsInTable(linesToCheck);
            loggingDetailsForm.DoCancel();
            
            _cargoEnquiryTransactionForm.SetFocusToForm();
            _cargoEnquiryTransactionForm.CloseForm();

        }

        


    }

}

