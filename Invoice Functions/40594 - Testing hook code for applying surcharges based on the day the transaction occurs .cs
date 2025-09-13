using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using System.Diagnostics;
using System.Drawing;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40594 : MTNBase
    {

        string[] _detailsToCheck;

        ConfirmationFormYesNo _confirmationFormYesNo;
        CargoEnquiryTransactionMaintenanceForm _cargoEnquiryTransactionMaintenanceForm;
        TransactionResyncForm _transactionResyncForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();            
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>( "MTN40594");
        }
       

        [TestMethod]
        public void TestHookCodeWithSurchargesBasedOnDayTxnOccurs()
        {
            MTNInitialize();
            
            var txnTabCount = 6;

            _detailsToCheck = new string[]
           {
                // setup the details we want to check 
                // removed / from the dates and : from the time because of copy/paste to Type change
                "09072018^1030^102.00^2% Shift Surcharge 1",
                "09072018^1632^104.00^4% Shift Surcharge 2",
                "10072018^0159^106.00^6% Shift Surcharge 3",
                "10072018^0730^108.00^8% Shift Surcharge 4",
                "10072018^1359^102.00^2% Shift Surcharge 1",
                "10072018^1400^104.00^4% Shift Surcharge 2",
                "11072018^0002^106.00^6% Shift Surcharge 3",
                "11072018^0200^108.00^8% Shift Surcharge 4",
                "11072018^1022^102.00^2% Shift Surcharge 1",
                "11072018^1959^104.00^4% Shift Surcharge 2",
                "11072018^2000^106.00^6% Shift Surcharge 3",
                "12072018^0759^108.00^8% Shift Surcharge 4",
                "12072018^0800^102.00^2% Shift Surcharge 1",
                "12072018^1521^104.00^4% Shift Surcharge 2",
                "13072018^0000^106.00^6% Shift Surcharge 3",
                "13072018^0500^108.00^8% Shift Surcharge 4",
                "13072018^0901^102.00^2% Shift Surcharge 1",
                "13072018^1533^104.00^4% Shift Surcharge 2",
                "14072018^0110^106.00^6% Shift Surcharge 3",
                "14072018^0330^108.00^8% Shift Surcharge 4",
                "14072018^1344^110.00^10% Surcharge Saturday Shift 1",
                "14072018^1632^112.00^12% Surcharge Saturday Shift 2",
                "15072018^0123^114.00^14% Surcharge Saturday Shift 3",
                "15072018^0622^116.00^16% Surcharge Saturday Shift 4",
                "15072018^1021^118.00^18% Surcharge Sunday Shift 1",
                "15072018^1532^120.00^20% Surcharge Sunday Shift 2",
                "16072018^0123^122.00^22% Surcharge Sunday Shift 3",
                "16072018^0723^124.00^24% Surcharge Sunday Shift 4",
                "17072018^1900^140.00^40% Surcharge Working Holiday",
                "17072018^2030^106.00^6% Shift Surcharge 3",
                "18072018^1900^130.00^30% Surcharge Non-working Holiday",
                "18072018^2030^106.00^6% Shift Surcharge 3"
          };


            // Step 1
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            // Step 2
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", "JLG40594A01");
            cargoEnquiryForm.DoSearch();

            // Step 3
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            cargoEnquiryForm.CloseForm();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            cargoEnquiryTransactionForm.GetChargesTab();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblCharges,
                // "Debtor^MK3~Status^Normal~Transaction^Received - Road~Type^Invoice Line~Narration^SNOREMARKS", ClickType.RightClick);
            cargoEnquiryTransactionForm.tblCharges1.FindClickRow(["Debtor^MK3~Status^Normal~Transaction^Received - Road~Type^Invoice Line~Narration^SNOREMARKS"], ClickType.RightClick);

            Keyboard.Press(VirtualKeyShort.DOWN);               // Show Invoice...
            Keyboard.Press(VirtualKeyShort.ENTER);

            // Step 5
            InvoiceLinesForm invoiceLinesForm = new InvoiceLinesForm();
            invoiceLinesForm.GetMainFormDetails();

            MTNControlBase.FindClickRowInTable(invoiceLinesForm.tblDetails,
                "Invoice Line Type^SNOREMARKS~Status^Normal~Item Details^JLG40594A01~Transaction^Received - Road~Debtor^MK3", ClickType.Click,
                countOffset: -1);

            var  reverseTable = true;

            // we don't actually have to go back to the start of the test and repeat steps
            // loop through and run the common steps
            foreach (var t in _detailsToCheck)
            {
                // get the parameters
                var parameters = t.Split('^');
                var date = parameters[0];
                string time = parameters[1];
                string rate = parameters[2];
                var narration = parameters[3];
                Trace.TraceInformation("date: {0}    time: {1}    rate: {2}    narration: {3}", date, time, rate, narration);
                
                Keyboard.TypeSimultaneously(VirtualKeyShort.SHIFT, VirtualKeyShort.F8);

                // Step 6
                _confirmationFormYesNo = new ConfirmationFormYesNo("Changing Invoice Line Status for 1 line(s) Void");
                _confirmationFormYesNo.btnYes.DoClick();

                // Step 8
                cargoEnquiryTransactionForm.SetFocusToForm();
                cargoEnquiryTransactionForm.GetTransactionTab($"Transactions ({txnTabCount})");
                cargoEnquiryTransactionForm.TblTransactions2.GetElement().Focus();

                SortTable(cargoEnquiryTransactionForm.TblTransactions2.GetElement(), 130, 10);

                //MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                //    @"Type^Received - Road~Charged^Yes~User^SUPERUSER~Details^Received");
                cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[] { "Type^Received - Road~Charged^Yes~User^SUPERUSER~Details^Received" });
                cargoEnquiryTransactionForm.DoEdit41324();
                
                _cargoEnquiryTransactionMaintenanceForm =
                    new CargoEnquiryTransactionMaintenanceForm(cargoEnquiryTransactionForm.GetTerminalName());

                Keyboard.Press(VirtualKeyShort.TAB);

                // Step 9
                MTNControlBase.SetValueInEditTable(_cargoEnquiryTransactionMaintenanceForm.tblDetails, "Date", date);
                MTNControlBase.SetValueInEditTable(_cargoEnquiryTransactionMaintenanceForm.tblDetails, "Time", time, doTab: false);

                _cargoEnquiryTransactionMaintenanceForm.btnSave.DoClick();

                // Step 10
                cargoEnquiryTransactionForm.SetFocusToForm();

                SortTable(cargoEnquiryTransactionForm.TblTransactions2.GetElement(), 130, 10);

               // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
               //     @"Type^Received - Road~Charged^Yes~User^SUPERUSER~Details^Received", rowHeight: 16);
               cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[] { "Type^Received - Road~Charged^Yes~User^SUPERUSER~Details^Received" });
                //Thread.Sleep(1000);
                Miscellaneous.MTNWaitForSeconds(1);

                cargoEnquiryTransactionForm.DoResync40542();

                // Step 11
                _transactionResyncForm = new TransactionResyncForm();
                _transactionResyncForm.btnSelectAll.DoClick();
                _transactionResyncForm.btnSave.DoClick();
                _transactionResyncForm.btnClose.DoClick();
                //Thread.Sleep(1000);
                Miscellaneous.MTNWaitForSeconds(1);

                // Step 12 - Don't need to do as we already have the form open

                // Step 13
                invoiceLinesForm.SetFocusToForm();
                Miscellaneous.MTNWaitForSeconds(1);
                
                Keyboard.Press(VirtualKeyShort.F5);
                Miscellaneous.MTNWaitForSeconds(1);

                if (reverseTable)
                {
                    Point point = new Point(invoiceLinesForm.tblDetails.BoundingRectangle.X + 5,
                        invoiceLinesForm.tblDetails.BoundingRectangle.Y + 5);
                    Mouse.DoubleClick(point);
                    Mouse.DoubleClick(point);
                    Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(200));
                    reverseTable = false;
                }

                /*Keyboard.Press(VirtualKeyShort.F5);
                Miscellaneous.MTNWaitForSeconds(1);*/

                Trace.TraceInformation(@"rate: {0}    narration: {1}", rate, narration);
                MTNControlBase.FindClickRowInTable(invoiceLinesForm.tblDetails,
                    $"Invoice Line Type^SNOREMARKS~Rate^{rate}~Status^Normal~Item Details^JLG40594A01~Transaction^Received - Road~Debtor^MK3~Line Remarks^" +
                    narration, ClickType.Click, countOffset: -1);
               
                // update the txnTabCount - needed as changes each time round
                txnTabCount++;
            }

        }

        private void SortTable(AutomationElement table, int xOffset, int yOffset)
        {
            Point point = new Point(table.BoundingRectangle.X + xOffset, table.BoundingRectangle.Y + yOffset);
            Mouse.DoubleClick(point);
            Mouse.DoubleClick(point);
        }

        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_40594_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40594A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>INV_VOY</voyageCode>\n            <operatorCode>MKOP</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40594A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>INV_VOY</voyageCode>\n            <operatorCode>MKOP</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }

}
