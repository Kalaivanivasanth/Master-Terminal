using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNUtilityClasses.Classes;
using System;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40648 : MTNBase
    {
        VoyageEnquiryForm _voyageEnquiryForm;
        EDIOperationsForm _ediOperationsForm;
        VoyageDefinitionForm _voyageDefinitionForm;
        ConfirmationFormYesNo _confirmationFormYesNo;
        
        protected static string EDIFile1 = "M_40648_VoyageAdd.csv";
        protected static string EDIFile2 = "M_40648_VoyageModify.csv";
        protected static string EDIFile3 = "M_40648_VoyageUpdate.csv";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_40648_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            // Setup data
            CreateDataFile(EDIFile1,
                "control column,Action,Operator,Scheduled arrival (datetime),scheduled departure (Datetime),Shipping Line Code,Tidal Class Arrival,Tidal Class Departure,Trade Description,Valid Operators,Vessel Code, Vessel,Voyage\nV,A,HLL,01-01-2018-13:00,31-01-2018-13:00,MSK,A,A,New Zealand East Coast,HLL,SARE,SANTA REGULA,40648\nP,NZAKL,NZAKL NZPOE,,,,,,,,,,\nP,NZBLU,NZNPE,,,,,,,,,,\n");
            CreateDataFile(EDIFile2,
                "control column,Action,Operator,Scheduled arrival (datetime),scheduled departure (Datetime),Shipping Line Code,Tidal Class Arrival,Tidal Class Departure,Trade Description,Valid Operators,Vessel Code, Vessel,Voyage\nV,M,HLL,01-02-2018-13:00,28-02-2018-13:00,MSK,A,A,New Zealand East Coast,HLL,SARE,SANTA REGULA,40648\nP,NZAKL,NZAKL NZPOE,,,,,,,,,,\nP,NZBLU,NZNPE,,,,,,,,,,\n");
            CreateDataFile(EDIFile3,
                "control column,Action,Operator,Scheduled arrival (datetime),scheduled departure (Datetime),Shipping Line Code,Tidal Class Arrival,Tidal Class Departure,Trade Description,Valid Operators,Vessel Code, Vessel,Voyage\nV,U,HLL,01-03-2018-13:00,30-03-2018-13:00,MSK,A,A,New Zealand East Coast,HLL,SARE,SANTA REGULA,40648\nP,NZAKL,NZAKL NZPOE,,,,,,,,,,\nP,NZBLU,NZNPE,,,,,,,,,,\n");

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void UpdateVoyageUserDefined()
        {
            
            MTNInitialize();

            // 1. Delete existing voyage
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry");
            _voyageEnquiryForm = new VoyageEnquiryForm(@"Voyage Enquiry TT1");
            _voyageEnquiryForm.DeleteVoyageByVoyageCode(@"40648");
   
            // 2. Open EDI Operations
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // 3. Delete any Existing EDI messages relating to this test.
            _ediOperationsForm.DeleteEDIMessages(@"Voyage", @"40648", @"Loaded");

            // 4. Load User Defined EDI (Add)
            _ediOperationsForm.LoadEDIMessageFromFile(EDIFile1,specifyType: true, fileType: @"40648_ADP");
            _ediOperationsForm.ChangeEDIStatus(EDIFile1, @"Loaded", @"Verify");
            _ediOperationsForm.ChangeEDIStatus(EDIFile1, @"Verified", @"Load To DB");


            //5. open voyage enquiry and validate data load + add voyage remarks
            _voyageEnquiryForm.SetFocusToForm();
            _voyageEnquiryForm.FindVoyageByVoyageCode(@"40648");
            MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages, @"Code^40648", ClickType.DoubleClick);

            _voyageDefinitionForm = new VoyageDefinitionForm(@"SANTA REGULA - (40648) Voyage Definition TT1");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtArrivalDate, @"Arrival Date", @"01/01/2018");
            _voyageDefinitionForm.txtArrivalDate.ValidateText("01/01/2018");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtArrivalTime, @"Arrival Time", @"13:00");
            _voyageDefinitionForm.txtArrivalTime.ValidateText("13:00");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtDepartureDate, @"Departure Date", @"31/01/2018");
            _voyageDefinitionForm.txtDepartureDate.ValidateText( @"31/01/2018");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtDepartureTime, @"Departure Time", @"13:00");
            _voyageDefinitionForm.txtDepartureTime.ValidateText("13:00");
            //MTNControlBase.FindTabOnForm(voyageDefinitionForm.tabVoyageDetails, @"Remarks");
            _voyageDefinitionForm.RemarksTab();
            //voyageDefinitionForm.btnEditVoyage.DoClick();
            _voyageDefinitionForm.DoEdit();
            //MTNControlBase.SetValue(voyageDefinitionForm.txtGeneralRemarks, @"Testing 40648 - General Remarks");
            _voyageDefinitionForm.txtGeneralRemarks.SetValue(@"Testing 40648 - General Remarks");
            //voyageDefinitionForm.btnSaveVoyage.DoClick();
            _voyageDefinitionForm.DoSave();
            _voyageDefinitionForm.CloseForm();


            //6. Load User Defined EDI (Modify) and ensure remarks are untouched
            _ediOperationsForm.SetFocusToForm();
            _ediOperationsForm.LoadEDIMessageFromFile(EDIFile2, specifyType: true, fileType: @"40648_ADP");
            _ediOperationsForm.ChangeEDIStatus(EDIFile2, @"Loaded", @"Verify");
            _ediOperationsForm.ChangeEDIStatus(EDIFile2, @"Verified", @"Load To DB");

            //voyageEnquiry.SetFocusToForm();

            _voyageEnquiryForm.SetFocusToForm();
            _voyageEnquiryForm.FindVoyageByVoyageCode(@"40648");
            MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages, @"Code^40648", ClickType.DoubleClick);
            _voyageDefinitionForm = new VoyageDefinitionForm(@"SANTA REGULA - (40648) Voyage Definition TT1");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtArrivalDate, @"Arrival Date", @"01/02/2018");
            _voyageDefinitionForm.txtArrivalDate.ValidateText("01/02/2018");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtArrivalTime, @"Arrival Time", @"13:00");
            _voyageDefinitionForm.txtArrivalTime.ValidateText("13:00");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtDepartureDate, @"Departure Date", @"28/02/2018");
            _voyageDefinitionForm.txtDepartureDate.ValidateText("28/02/2018");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtDepartureTime, @"Departure Time", @"13:00");
            _voyageDefinitionForm.txtDepartureTime.ValidateText("13:00");
            //MTNControlBase.FindTabOnForm(voyageDefinitionForm.tabVoyageDetails, @"Remarks");
            _voyageDefinitionForm.RemarksTab();
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtGeneralRemarks, @"General Remarks", @"Testing 40648 - General Remarks");
            _voyageDefinitionForm.txtGeneralRemarks.ValidateText("Testing 40648 - General Remarks");
            _voyageDefinitionForm.CloseForm();

            //7. Load User Defined EDI (Update) and ensure remarks are removed
            _ediOperationsForm.SetFocusToForm();
            _ediOperationsForm.LoadEDIMessageFromFile(EDIFile3, specifyType: true, fileType: @"40648_ADP");
            _ediOperationsForm.ChangeEDIStatus(EDIFile3, @"Loaded", @"Verify");
            _ediOperationsForm.ChangeEDIStatus(EDIFile3, @"Verified", @"Load To DB");

            //voyageEnquiry.SetFocusToForm();
            _voyageEnquiryForm.SetFocusToForm();
            _voyageEnquiryForm.FindVoyageByVoyageCode(@"40648");
            MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages, @"Code^40648", ClickType.DoubleClick);
            _voyageDefinitionForm = new VoyageDefinitionForm(@"SANTA REGULA - (40648) Voyage Definition TT1");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtArrivalDate, @"Arrival Date", @"01/03/2018");
            _voyageDefinitionForm.txtArrivalDate.ValidateText("01/03/2018");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtArrivalTime, @"Arrival Time", @"13:00");
            _voyageDefinitionForm.txtArrivalTime.ValidateText("13:00");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtDepartureDate, @"Departure Date", @"30/03/2018");
            _voyageDefinitionForm.txtDepartureDate.ValidateText("30/03/2018");
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtDepartureTime, @"Departure Time", @"13:00");
            _voyageDefinitionForm.txtDepartureTime.ValidateText("13:00");
            //MTNControlBase.FindTabOnForm(voyageDefinitionForm.tabVoyageDetails, @"Remarks");
            _voyageDefinitionForm.RemarksTab();
            //MTNControlBase.ValidateElementText(voyageDefinitionForm.txtGeneralRemarks, @"General Remarks", @"");
            _voyageDefinitionForm.txtGeneralRemarks.ValidateText("");
            _voyageDefinitionForm.CloseForm();

            //8. Delete voyage
            _voyageEnquiryForm.btnDeleteVoyage.DoClick();
            //voyageEnquiryForm.DoDeleteVoyage();
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Voyage Enquiry TT1");
            _confirmationFormYesNo.btnYes.DoClick();
            warningErrorForm = new WarningErrorForm(@"Warnings for Voyage Enquiry TT1");
            warningErrorForm.btnSave.DoClick();
            Wait.UntilResponsive(_voyageEnquiryForm.tblVoyages);
            Wait.UntilInputIsProcessed(waitTimeout: TimeSpan.FromSeconds(2));
  
        }

    }

}
