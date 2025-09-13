using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using System.Diagnostics;
using DataObjects.LogInOutBO;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39776 : MTNBase
    {
        ToDoTaskForm _toDoTaskForm;
        ConfirmationFormOK _confirmationFormOK;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;
        InvoiceLinesForm _invoiceLinesForm;
        CalendarViewForm _calendarViewForm;
        CalendarMaintenanceForm _calendarMaintenanceForm;
        CalendarDateMaintenanceForm _calendarDateMaintenanceForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void ToTestRatesAreAppliedCorrectlyBasedOnNormalorOvertimeHours()

        {
            MTNInitialize();
            
            // Step 4 Open General Functions | Cargo Enquiry
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            // Step 5 enter Cargo ID - JLG39776A01 and click the Search button 
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG39776A01");
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG39776A01" }, ClickType.ContextClick);

            // Step 6 Right click and select Cargo | Add Tasks.... 
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Add Tasks...");

            // Step 7 Check the checkbox for task Req Paint
            // Step 8 click the Save & Close button
            _toDoTaskForm = new ToDoTaskForm(@"JLG39776A01 TT1");
            _toDoTaskForm.AddCompleteTask(@"Req Paint", _toDoTaskForm.btnSaveAndClose);

            // Step 9 Click the OK button 
            _confirmationFormOK = new ConfirmationFormOK(@"Tasks Added");
            _confirmationFormOK.btnOK.DoClick();

            // Step 10 Click View Transactions button in the toolbar 
            cargoEnquiryForm.DoViewTransactions();

            // Step 11 Click the Charges tab 
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>("Transactions for JLG39776A01 TT1");
            _cargoEnquiryTransactionForm.GetChargesTab(@"Charges (2)");


            // Step 12 Select the item, right click and select Show Invoice...
            var dateTime = MTNControlBase.ReturnValueFromTableCell(_cargoEnquiryTransactionForm.tblCharges1.GetElement(),
                @"Debtor^MK3~Qty^1~Status^Normal~Type^Invoice Line~Narration^Test39776~Amount^100.00", @"Date",
                ClickType.None);

            DateTime dateTimeNoted = DateTime.Parse(Miscellaneous.ReturnTextFromTableString(dateTime));
            Trace.TraceInformation(@"Date/Time noted is " + dateTimeNoted);
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblCharges,
                // @"Debtor^MK3~Qty^1~Status^Normal~Type^Invoice Line~Narration^Test39776~Amount^100.00",
                // ClickType.ContextClick);
            _cargoEnquiryTransactionForm.tblCharges1.FindClickRow(["Debtor^MK3~Qty^1~Status^Normal~Type^Invoice Line~Narration^Test39776~Amount^100.00"], ClickType.ContextClick);

            _cargoEnquiryTransactionForm.ContextMenuSelect(@"Show Invoice...");

            // Step 13 Check the Number column and select the Invoice Item with the highest number  
            _invoiceLinesForm = new InvoiceLinesForm();
            _invoiceLinesForm.SetFocusToForm();
            _invoiceLinesForm.GetMainFormDetails();
            MTNControlBase.FindClickRowInTable(_invoiceLinesForm.tblDetails,
                @"Invoice Line Type^Test39776~Total Excl Tax^100.00~Rate^100.00~Status^Normal~Item Details^JLG39776A01~Transaction^Todo Task Added~Debtor^MK3",
                ClickType.Click, countOffset: -1);


            // Step 14 Open Terminal Ops | Calendar View  
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Calendar View", forceReset: true);

            // Step 15 Click the Calendar Maint button in the toolbar 
            _calendarViewForm = new CalendarViewForm(@"Calendar View TT1");
            _calendarViewForm.BtnCalendarMaint.DoClick();

            // Step 16 In the Calendar drop down, select Test - 39776  
            _calendarMaintenanceForm = new CalendarMaintenanceForm(@"Calendar Maintenance TT1");
            _calendarMaintenanceForm.cmbCalendar.SetValue(@"Test - 39776");

            // Step 17 Click the New button at the bottom of the form 
            _calendarMaintenanceForm.btnNewDateEntry.DoClick();


            // Step 18 Enter Date - Today's Date, Start Time -00:00, End Time - < Time Noted previously>, Description - Normal Hours and click the Save button
            _calendarDateMaintenanceForm = new CalendarDateMaintenanceForm(@"Calendar Date Maintenance TT1");
            _calendarDateMaintenanceForm.txtDate.SetValue(dateTimeNoted.ToString(@"ddMMyyyy"));
            MTNControlBase.SetTime(_calendarDateMaintenanceForm.txtEndTime, dateTimeNoted.ToString(@"HH:mm"), 500);
            // Wednesday, 9 April 2025 navmh5 _calendarDateMaintenanceForm.txtEndTime.SetValue(dateTimeNoted.ToString("HH:mm"), 500);
            _calendarDateMaintenanceForm.txtDescription.SetValue(@"Normal Hours");
            
            _calendarDateMaintenanceForm.btnSave.DoClick();

            // Step 19 Go back to the Invoice Items for Invoice form, right click and select Re-evaluate Selected Items 
            _invoiceLinesForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(_invoiceLinesForm.tblDetails,
                @"Invoice Line Type^Test39776~Total Excl Tax^100.00~Rate^100.00~Status^Normal~Item Details^JLG39776A01~Transaction^Todo Task Added~Debtor^MK3", ClickType.ContextClick, countOffset: -1);

            _invoiceLinesForm.ContextMenuSelect(@"Re-evaluate Selected Items");


            // Step 20 Should show a Confirm Re-evaluation dialog - Click the Yes button
            ConfirmationFormYesNo confirmReEvaluation = new ConfirmationFormYesNo(@"Confirm Re-evaluation");
            confirmReEvaluation.btnYes.DoClick();

            // Should show logging as in the attached screenshot - NORMAL hours rate 
            LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm();
            loggingDetailsForm.FindStringInTable(@"NORMAL hours rate");

            // Step 21 Close the logging window and go back to the Calendar Maintenance form 
            loggingDetailsForm.btnCanel.DoClick();
            _calendarMaintenanceForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_calendarMaintenanceForm.tblDateEntries,
                // @"Description^Normal Hours", ClickType.Click);
            _calendarMaintenanceForm.TblDateEntries.FindClickRow(["Description^Normal Hours"], ClickType.Click);
            _calendarMaintenanceForm.btnUpdateDateEntry.DoClick();

            // Step 23 Change the End Time to (End Time - 1 minute) e.g. if End Time is 14:19, change it to 14:18 
            _calendarDateMaintenanceForm = new CalendarDateMaintenanceForm(@"Calendar Date Maintenance TT1");
            DateTime timeNotedMinus1 = dateTimeNoted.AddMinutes(-1);
            Trace.TraceInformation(@"timeNotedMinus1 = " + timeNotedMinus1);
           MTNControlBase.SetTime(_calendarDateMaintenanceForm.txtEndTime, timeNotedMinus1.ToString(@"HH:mm"), 500);

            // Step 24 Click the Save button
            _calendarDateMaintenanceForm.btnSave.DoClick();

            // Step 25 Go back to the Invoice Items for Invoice form, right click and select Re-evaluate Selected Items   
            _invoiceLinesForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(_invoiceLinesForm.tblDetails,
                @"Invoice Line Type^Test39776~Total Excl Tax^100.00~Rate^100.00~Status^Normal~Item Details^JLG39776A01~Transaction^Todo Task Added~Debtor^MK3", ClickType.ContextClick, countOffset: -1);

            _invoiceLinesForm.ContextMenuSelect(@"Re-evaluate Selected Items");


            // Step 26 Should show a Confirm Re-evaluation dialog - Click the Yes button
            confirmReEvaluation = new ConfirmationFormYesNo(@"Confirm Re-evaluation");
            confirmReEvaluation.btnYes.DoClick();

            // Should show logging as in the attached screenshot - NORMAL hours rate 
            loggingDetailsForm = new LoggingDetailsForm();
            loggingDetailsForm.FindStringInTable(@"OVERTIME hours rate");

            // Step 27 Close the logging window and go back to the Calendar Maintenance form 
            loggingDetailsForm.btnCanel.DoClick();
            _calendarMaintenanceForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_calendarMaintenanceForm.tblDateEntries,
                // @"Description^Normal Hours", ClickType.Click);
            _calendarMaintenanceForm.TblDateEntries.FindClickRow(["Description^Normal Hours"], ClickType.Click);
            _calendarMaintenanceForm.btnUpdateDateEntry.DoClick();

            // Step 29 Change the End Time to (End Time + 2 minute) e.g. if End Time is 14:18, change it to 14:20  
            _calendarDateMaintenanceForm = new CalendarDateMaintenanceForm(@"Calendar Date Maintenance TT1");
            DateTime timeNotedPlus2 = dateTimeNoted.AddMinutes(1);
            Trace.TraceInformation(@"timeNotedPlus2 = " + timeNotedPlus2);
             MTNControlBase.SetTime(_calendarDateMaintenanceForm.txtEndTime, timeNotedPlus2.ToString(@"HH:mm"), 500);
             // Wednesday, 9 April 2025 navmh5 _calendarDateMaintenanceForm.txtEndTime.SetValue(timeNotedPlus2.ToString(@"HH:mm"), 500);

            // Step 30 Click the Save button
            _calendarDateMaintenanceForm.btnSave.DoClick();

            // Step 31 Go back to the Invoice Items for Invoice form, right click and select Re-evaluate Selected Items 
            _invoiceLinesForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(_invoiceLinesForm.tblDetails,
                @"Invoice Line Type^Test39776~Total Excl Tax^200.00~Rate^200.00~Status^Normal~Item Details^JLG39776A01~Transaction^Todo Task Added~Debtor^MK3", ClickType.ContextClick, countOffset: -1);

            _invoiceLinesForm.ContextMenuSelect(@"Re-evaluate Selected Items");

            // Step 32 Click the Yes button
            confirmReEvaluation = new ConfirmationFormYesNo(@"Confirm Re-evaluation");
            confirmReEvaluation.btnYes.DoClick();

            loggingDetailsForm = new LoggingDetailsForm();
            loggingDetailsForm.FindStringInTable(@"NORMAL hours rate");
            loggingDetailsForm.btnCanel.DoClick();

            // Step 33 Go back to the Calendar Maintenance form   
            _calendarMaintenanceForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_calendarMaintenanceForm.tblDateEntries,
                // @"Description^Normal Hours", ClickType.Click);
            _calendarMaintenanceForm.TblDateEntries.FindClickRow(["Description^Normal Hours"], ClickType.Click);
            _calendarMaintenanceForm.btnRemoveDateEntry.DoClick();

            // Step 35 Click the Yes button
            confirmReEvaluation = new ConfirmationFormYesNo(@"Calendar");
            confirmReEvaluation.btnYes.DoClick();
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_39776_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>4200</isoType>\n		<dischargePort>NZAKL</dischargePort>\n		<id>JLG39776A01</id>\n		<locationId>MKBS01</locationId>\n		<messageMode>D</messageMode>\n		<imexStatus>Export</imexStatus>\n		<operatorCode>MKOP</operatorCode>\n		<voyageCode>MSCK000002</voyageCode>\n		<weight>4500</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	</AllCargoOnSite>\n </JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>4200</isoType>\n		<dischargePort>NZAKL</dischargePort>\n		<id>JLG39776A01</id>\n		<locationId>MKBS01</locationId>\n		<messageMode>A</messageMode>\n		<imexStatus>Export</imexStatus>\n		<operatorCode>MKOP</operatorCode>\n		<voyageCode>MSCK000002</voyageCode>\n		<weight>4500</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	</AllCargoOnSite>\n </JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
