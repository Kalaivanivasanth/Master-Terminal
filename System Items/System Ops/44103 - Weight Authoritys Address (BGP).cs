using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using DataGridViewRow = FlaUI.Core.AutomationElements.DataGridViewRow;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44103 : MTNBase
    {

        const string TestCaseNumber = @"44103";
        const string CargoId = @"JLG" + TestCaseNumber + "A01";

        BackgroundApplicationForm _backgroundApplicationForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup()
        {

            // Stop the CAMS(Server)
            // Wednesday, 29 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            FormObjectBase.MainForm.OpenBackgroundProcessingFromToolbar();
           _backgroundApplicationForm = new BackgroundApplicationForm();
           _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^",
                    false);

            base.TestCleanup();
        }

        private void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();

            // Wednesday, 29 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Use AUS Customs Authority validation", @"1",
                rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();
        }


        [TestMethod]
        public void WeightAuthoritysAddress()
        {
            MTNInitialize();

            // Step 6 - 10
            // Wednesday, 29 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            // Wednesday, 29 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId, rowHeight: 18);
            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{CargoId}" });
            cargoEnquiryForm.DoViewTransactions();
            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = 
                FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for " + CargoId + " TT1");

            List<DataGridViewRow> rowsInTable =
                new List<DataGridViewRow>
                    // Wednesday, 29 January 2025 navmh5  (cargoEnquiryTransactionForm.tblTransactions.AsDataGridView()
                (cargoEnquiryTransactionForm.TblTransactions2.GetElement().AsDataGridView()
                    .Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells.Any() && r.Cells[0].Name.Contains("Created")));

           DateTime createdDateTime =
                DateTime.Parse(Miscellaneous.ReturnTextFromTableString(rowsInTable[0].Cells[1].Name));
            
            cargoEnquiryTransactionForm.CloseForm();

            // Steps 11 - 13 
            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.GetStatusTable(@"4087");

            MTNControlBase.FindClickRowInTableVHeader(cargoEnquiryForm.tblStatus, @"Stops (dbl click)", clickType: ClickType.DoubleClick);

            StopsForm stopsForm = new StopsForm();
            // MTNControlBase.FindClickRowInTable(stopsForm.tblStops, @"Stop^STOP_39019", xOffset: 130);
            // MTNControlBase.FindClickRowInTable(stopsForm.tblStops, @"Stop^Delivery Order", xOffset: 130);
            stopsForm.TblStops.FindClickRow([
                "Stop^STOP_39019",
                "Stop^Delivery Order"
            ], xOffset: 130);
;            stopsForm.btnSaveAndClose.DoClick();

            // Step 14 - 17
            cargoEnquiryForm.SetFocusToForm();
            // Wednesday, 29 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId, clickType: ClickType.ContextClick,
            // Wednesday, 29 January 2025 navmh5     rowHeight: 18 );
            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{CargoId}" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Add Tasks...");

            ToDoTaskForm toDoTaskForm = new ToDoTaskForm(CargoId + @" TT1");
            toDoTaskForm.AddCompleteTask(@"Requires Weighing", toDoTaskForm.btnSaveAndClose);

            ConfirmationFormOK confirmationFormOK = new ConfirmationFormOK(@"Tasks Added");
            confirmationFormOK.btnOK.DoClick();

            cargoEnquiryForm.SetFocusToForm();
            // Wednesday, 29 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId, clickType: ClickType.ContextClick,
            // Wednesday, 29 January 2025 navmh5     rowHeight: 18);
            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{CargoId}" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Complete Tasks...");

            toDoTaskForm = new ToDoTaskForm(CargoId + @" TT1");
            toDoTaskForm.AddCompleteTask(@"Requires Weighing", toDoTaskForm.btnSaveAndClose);

            CargoWeighForm cargoWeighForm = new CargoWeighForm(@"Complete Weigh Task for " + CargoId + " TT1");
            cargoWeighForm.WeightMeasurementFocus();
            cargoWeighForm.WeightFocus();
            cargoWeighForm.mtWeight.SetValueAndType("10000", "kgs");
            cargoWeighForm.chkIsWeightCertified.DoClick();
            cargoWeighForm.cmbWeightCertifyingAuthority.SetValue(@"WEIGHING TC 44103");
            cargoWeighForm.cmbWeightCertifyingPerson.SetValue(@"TC44103");

            cargoWeighForm.btnOK.DoClick();

            confirmationFormOK = new ConfirmationFormOK(@"Tasks Completed");
            confirmationFormOK.btnOK.DoClick();

            // Step 22 - 23.  Have set this as default 

            // Step 24 - 25
            // Wednesday, 29 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset:true);
            FormObjectBase.MainForm.OpenBackgroundProcessingFromToolbar();
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^");
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(5));

            // Step 26 - 33
            FormObjectBase.NavigationMenuSelection(@"EDI Functions | EDI CAMS Protocols", forceReset: true);
            EDICAMProtocolForm ediCAMProtocolForm = new EDICAMProtocolForm();

            // Wednesday, 29 January 2025 navmh5 MTNControlBase.FindClickRowInTable(ediCAMProtocolForm.tblProtocols,
            // Wednesday, 29 January 2025 navmh5     @"Protocol Description^UN/EDIFACT Vermas~Name^TC44103", xOffset: 40);
            ediCAMProtocolForm.TblProtocols.FindClickRow(new[] { "Protocol Description^UN/EDIFACT Vermas~Name^TC44103" }, xOffset: 40);

            var endDateTime = DateTime.Now.AddMinutes(1);
            
            ediCAMProtocolForm.GetDetailsTabAndDetails();
            ediCAMProtocolForm.RunAdhocReport(createdDateTime.ToString(@"ddMMyyyy"),
                createdDateTime.ToString(@"HHmm"), endDateTime.ToString(@"ddMMyyyy"),
                endDateTime.ToString(@"HHmm"), getTab: true);

            // Step 36 - 39
            cargoEnquiryForm.SetFocusToForm();
            // Wednesday, 29 January 2025 navmh5  MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId, rowHeight: 18);
            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{CargoId}" });
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for " + CargoId + " TT1");

            // Wednesday, 29 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^EDI Info Sent",
            // Wednesday, 29 January 2025 navmh5      ClickType.DoubleClick);
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[] { "Type^EDI Info Sent" }, ClickType.DoubleClick);

            // Check logging details
            /*// Tuesday, 8 April 2025 navmh5 Can be removed 6 months after specified date 
            LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm(@"Logging Details");

            string[] linesToCheck =
            {
                @"CTA+RP+:TC44103",
                @"COM+,,AUSTRALIA:MA"
            };

            loggingDetailsForm.FindStringsInTable(linesToCheck);
            loggingDetailsForm.DoCancel();*/
            LoggingDetailsForm.ValidateLogDetails(new[] { "CTA+RP+:TC44103", "COM+,,AUSTRALIA:MA" });

        }

        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber + "_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44103</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44103A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44103</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44103A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }

}
