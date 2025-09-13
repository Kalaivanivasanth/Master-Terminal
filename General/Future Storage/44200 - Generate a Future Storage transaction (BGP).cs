using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNUtilityClasses.Classes;
using System;
using System.Drawing;
using DataObjects.LogInOutBO;
using DateTimeExtensions;
using HardcodedData.SystemData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General.Future_Storage
{
    /// --------------------------------------------------------------------------------------------------
    /// Date                 Person          Reason for change
    /// ==========   ======          =================================
    /// 09/03/2022       navmh5          Sort out issue where not finding 'Additional Storage Days' to click
    /// --------------------------------------------------------------------------------------------------
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44200: MTNBase
    {
        
        BackgroundApplicationForm _backgroundApplicationForm;
        VoyageEnquiryForm _voyageEnquiryForm;
        VoyageDefinitionForm _voyageDefinitionForm;
        VoyageOperationsForm _voyageOperationsForm;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

        DateTime _currentDateTime = DateTime.Now;

         string _tableDetailsNotFound;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_44200_";
            userName = "44200";
            BaseClassInitialize_New(testContext);
        }
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup()
        {
            // Stop the CAMS(Server)
            _backgroundApplicationForm.SetFocusToForm();
            //FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            //_backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"FUTURE TIMER(Server)", @"Application Type^Future Timer Runner~",
                false);
            
            // Now wait until the Requests table is responsive 
            WaitUntilTableResponsive(_backgroundApplicationForm.TblRequests.GetElement());
            
            base.TestCleanup();
        }

        private void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            //MTNSignon(TestContext, userName);
            LogInto<MTNLogInOutBO>("44200");
        }

        [TestMethod]
        public void  GenerateFutureStorageTransaction()
        {
            MTNInitialize();
         
            FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry");
            cargoEnquiryForm = new CargoEnquiryForm();
         
            // set the required dates
            var anniversaryDate = new DateTime(_currentDateTime.Year, _currentDateTime.Month, 10);
            anniversaryDate = _currentDateTime.Day >= 11
                ? new DateTime(_currentDateTime.Year, _currentDateTime.Month, 11).AddMonths(1)
                : new DateTime(_currentDateTime.Year, _currentDateTime.Month, 11);
             var actionFutureDate = anniversaryDate.AddDays(1);

            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing");
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"FUTURE TIMER(Server)",
                @"Application Type^Future Timer Runner~Status^");
            //Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            
            // Now wait until the Requests table is responsive 
            WaitUntilTableResponsive(_backgroundApplicationForm.TblRequests.GetElement());

            // Step 4 - 6
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry", forceReset: true);
            _voyageEnquiryForm = new VoyageEnquiryForm(@"Voyage Enquiry " + terminalId);

            _voyageEnquiryForm.FindVoyageByVoyageCode(@"MSCK000006");
            // MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages, @"Code^" + @"MSCK000006", ClickType.DoubleClick);
            _voyageEnquiryForm.TblVoyages.FindClickRow(["Code^" + @"MSCK000006"], ClickType.DoubleClick);

            _voyageDefinitionForm = new VoyageDefinitionForm();
            _voyageDefinitionForm.ExchangeDetailsTab();
            //_voyageDefinitionForm.btnEditVoyage.DoClick();
            _voyageDefinitionForm.DoEdit();
            //var now = DateTime.Now;
            //var firstOfLastMonth = new DateTime(now.Year, now.Month - 1, 1);
            var firstOfLastMonth = DateTime.Now.LastDayOfTheMonth().AddMonths(-2).AddDays(1);
            Console.WriteLine($"firstOfLastMonth: {firstOfLastMonth}");
            //MTNControlBase.SendTextToTextbox(_voyageDefinitionForm.txtGeneralLandingDate, firstOfLastMonth.ToString(@"ddMMyyyy"));
            _voyageDefinitionForm.TxtGeneralLandingDate.SetValue(firstOfLastMonth.ToString(@"ddMMyyyy"));
            //_voyageDefinitionForm.btnSaveVoyage.DoClick();
            _voyageDefinitionForm.DoSave();

            // Step 7 - 9
            FormObjectBase.NavigationMenuSelection(@"Operations | Voyage Operations ");
            _voyageOperationsForm = new VoyageOperationsForm(@"Voyage Operations " + terminalId);
            ////_voyageOperationsForm.GetSearcherTab_ClassicMode();
            ////_voyageOperationsForm.GetSearcherTab();

            ////_voyageOperationsForm.chkLOLOBays.DoClick();
            //_voyageOperationsForm.SetValue(_voyageOperationsForm.cmbVoyage, @"MSCK000006");
            ////_voyageOperationsForm.cmbVoyage.SetValue(@"MSCK000006 MSC KATYA R.", doDownArrow: true);
            ////_voyageOperationsForm.chkShowOffsiteCargo.DoClick();
            ////_voyageOperationsForm.chkShowPrenotedCargo.DoClick();
            // _voyageOperationsForm.btnSelect.DoClick();
            ////_voyageOperationsForm.DoSelect();
            _voyageOperationsForm.DoSearchForVoyageGetDetails(new VoyageOperationsSearcherArguments
            {
                Voyage = "MSCK000006 MSC KATYA R.",
                LOLOBays = true,
                ShowOffsiteCargo = true,
                ShowPrenotedCargo = true
            });
            _voyageOperationsForm.GetMainDetails();

            // Step 10 In the Discharge To drop-down from the toolbar select FSA5 
            //_voyageOperationsForm.SetValue(_voyageOperationsForm.cmbDischargeTo, @"FSA5");
            _voyageOperationsForm.CmbDischargeTo.SetValue(@"FSA5", doDownArrow: true, searchSubStringTo:"FSA5".Length - 1); 

            // Step 11 On the top right, enter Ops Retrospective -Tick, Effective Date -1st of last month
            _voyageOperationsForm.GetOpsRetrospectiveControls();
            _voyageOperationsForm.ChkOpsRetrospective.DoClick();
            _voyageOperationsForm.GetOpsRetrospectiveControls();
            //MTNControlBase.SendTextToTextbox(_voyageOperationsForm.txtEffectiveDate, firstOfLastMonth.ToString(@"ddMMyyyy"));
            _voyageOperationsForm.TxtEffectiveDate.SetValue(firstOfLastMonth.ToString(@"ddMMyyyy"));
            // Step 12 Drag and drop JLG44200A01 from On Vessel table to On Terminal table 
            //MTNControlBase.FindClickRowInTable(_voyageOperationsForm.tblOnVessel,
            //  @"ID^JLG44200A01", ClickType.Click, rowHeight: 18);
            _voyageOperationsForm.TblOnVessel1.FindClickRow(new[] { "ID^JLG44200A01" }, ClickType.Click);

            Point startPoint = Mouse.Position;
            _voyageOperationsForm.TblOnTerminal1.GetElement().Click();
            Point endPoint = Mouse.Position;

            Mouse.Drag(startPoint, endPoint);

            // Step 13 Click OK button on Confirm Quantity to move
            ConfirmQuantityToMoveForm confirmQuantityToMoveForm = new ConfirmQuantityToMoveForm(formTitle: @"Confirm quantity to move");
            confirmQuantityToMoveForm.btnOK.DoClick();
            _voyageOperationsForm.CloseForm();

            // Step 14 Open System items | Background Process | Background Processing  
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();

            // Step 15 Click the Future Timer Runner controller and find the request with Cargo Id - JLG44200A01
            ////MTNControlBase.FindClickRowInTable(_backgroundApplicationForm.tblBackgroundProcesses,
            ////    @"Application Type^Future Timer Runner", ClickType.Click, rowHeight: 16);

            // Step 16 Check the Action Future Date and Anniversary Date
            //_backgroundApplicationForm.CheckFutureStorageBGP("FSA5", "JLG44200A01", actionFutureDate, anniversaryDate, out _tableDetailsNotFound);
            _backgroundApplicationForm.CheckFutureStorageBGP(new FutureStorageDetailsToValidateOrDeleteArgs
            {
                CargoId = "JLG44200A01",
                DetailsToValidateOrDelete = new []
                    {  $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate:dd/MM/yyyy}~Terminal^TT1~Cargo Id^JLG44200A01~Location^FSA5~Anniversary Date^{anniversaryDate:dd/MM/yyyy}" }
            }, out _tableDetailsNotFound);

            // Step 17 Open General Functions | Cargo Enquiry 
            //FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry", forceReset: true);
            //cargoEnquiryForm = new CargoEnquiryForm();

            // Step 18 Enter Cargo Type -STEEL, Cargo ID -JLG44200A01 and click the Search button:
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Steel, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG44200A01");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            // Step 19 Check the Future Storage Anniversary field 
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Future Storage Anniversary",
                anniversaryDate.ToString(@"dd/MM/yyyy"));

            // Step 20 Click the View Transactions button 
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            // Step 21 Check the Date of the most recent Future Storage transaction  
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG44200A01 TT1");
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Future Storage",
                 // ClickType.Click, rowHeight:17);
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(["Type^Future Storage"], ClickType.Click);
            string dateTime = MTNControlBase.ReturnValueFromTableCell(_cargoEnquiryTransactionForm.TblTransactions2.GetElement(),
                @"Type^Future Storage", @"Date", ClickType.None);

            Assert.IsTrue(dateTime.Equals(actionFutureDate.AddMonths(-1).ToString(@"dd/MM/yyyy") + @" 00:01"),
                @"Expected value is " + actionFutureDate.AddMonths(-1).ToString(@"dd/MM/yyyy") + @" 00:01" +
                @"Actual is " + dateTime);
            _cargoEnquiryTransactionForm.CloseForm();

            // Step 22 In Cargo Enquiry, double click in the Additional Storage Days field  
            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44200A01~Location ID^FSA5~Voyage^MSCK000006", ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG44200A01~Location ID^FSA5~Voyage^MSCK000006"], ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo | Edit Additional Storage Days...");

            // Step 23 Enter Additional Free Storage Days -2, Reason - Testing and click the OK button:
            AdditionalFreeStorageDaysForm additionalFreeStorageDaysForm = new AdditionalFreeStorageDaysForm(cargoEnquiryForm.GetTerminalName());

            //MTNControlBase.SetValue(additionalFreeStorageDaysForm.txtAdditionalDays, @"2");
            additionalFreeStorageDaysForm.txtAdditionalDays.SetValue(@"2");
            //MTNControlBase.SetValue(additionalFreeStorageDaysForm.txtReason, @"Testing");
            additionalFreeStorageDaysForm.txtReason.SetValue(@"Testing");
            additionalFreeStorageDaysForm.btnOK.DoClick();

            // Change by TFS 50883
            warningErrorForm = new WarningErrorForm(formTitle: @"Errors for Error TT1");
            string[] warningsToCheck = 
            {
                @"Code :91518. The number of additional days cannot be changed on JLG44200A01, because future storage billing has already commenced."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningsToCheck);
            warningErrorForm.btnCancel.DoClick();

            additionalFreeStorageDaysForm.SetFocusToForm();
            additionalFreeStorageDaysForm.btnCancel.DoClick();
            
            // Step 24 Check the Future Storage Anniversary  
            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Future Storage Anniversary", anniversaryDate.ToString(@"dd/MM/yyyy"));

            // Step 25 Open Background Processing form again and click Future Timer Runner controller
            /*FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();*/
            

            // Step 26 Find the request with Cargo Id - JLG44200A01 and check the Action Future Date and Anniversary Date
            _backgroundApplicationForm.SetFocusToForm();
           //// _backgroundApplicationForm.CheckFutureStorageBGP(@"FSA5", @"JLG44200A01", actionFutureDate, anniversaryDate, out _tableDetailsNotFound);
            _backgroundApplicationForm.CheckFutureStorageBGP(new FutureStorageDetailsToValidateOrDeleteArgs
            {
                CargoId = "JLG44200A01",
                DetailsToValidateOrDelete = new []
                    {  $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate:dd/MM/yyyy}~Terminal^TT1~Cargo Id^JLG44200A01~Location^FSA5~Anniversary Date^{anniversaryDate:dd/MM/yyyy}" }
            }, out _tableDetailsNotFound);

            // Step 27 Back in Cargo Enquiry, click the View Transactions button
            cargoEnquiryForm.SetFocusToForm();
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG44200A01 TT1");
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
               // @"Type^Received - Ship~User^44200", rowHeight:17);
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(["Type^Received - Ship~User^44200"]);
           // _cargoEnquiryTransactionForm.btnEdit.DoClick();
           _cargoEnquiryTransactionForm.DoEdit();

            // Step 29 Enter Date - Today's Date - 10 and click the Save button:
            CargoEnquiryTransactionMaintenanceForm cargoEnquiryTransactionMaintenanceForm =
                new CargoEnquiryTransactionMaintenanceForm(_cargoEnquiryTransactionForm.GetTerminalName());

            Keyboard.Press(VirtualKeyShort.TAB);

            DateTime date = DateTime.Today.Date;
            date = date.AddDays(-10);
            Keyboard.Type(date.ToString(@"ddMMyyyy"));
            cargoEnquiryTransactionMaintenanceForm.btnSave.DoClick();

            // Step 30 In Cargo enquiry, check the Future Storage Anniversary  
            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Future Storage Anniversary",
                anniversaryDate.ToString(@"dd/MM/yyyy"));

            // Step 31 Open Background Processing form again and click Future Timer Runner controller 
            //FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            //_backgroundApplicationForm = new BackgroundApplicationForm();

            // Step 32 Find the request with Cargo Id - JLG44200A01 and check the Action Future Date and Anniversary Date
            _backgroundApplicationForm.SetFocusToForm();
            _backgroundApplicationForm.CheckFutureStorageBGP(new FutureStorageDetailsToValidateOrDeleteArgs
            {
                CargoId = "JLG44200A01",
                DetailsToValidateOrDelete = new []
                {  $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate:dd/MM/yyyy}~Terminal^TT1~Cargo Id^JLG44200A01~Location^FSA5~Anniversary Date^{anniversaryDate:dd/MM/yyyy}" }
            }, out _tableDetailsNotFound);


        }

       

        /// <summary>
        /// Scroll to Additional Storage Days row and click
        /// NOTE:  Because this table has hidden fields there is no easy way to actually position it to the correct row
        ///             so the easiest that is scroll the table to the end (at the moment but this may change as more
        ///             fields get added to the bottom), count the number of fields up to Additional Storage Days and
        ///             change rows to equal this.  When it gets pushed off the top of the page the would guess
        ///             added a page up after the last end should work
        /// </summary>
        void ClickAdditionalStorageDays()
        {
            var table = cargoEnquiryForm.tblGeneralEdit;
            
            // Position to bottom of table
            var pointToClick = new Point(table.BoundingRectangle.X + 15, table.BoundingRectangle.Y + 7);
            Mouse.Click(pointToClick);
            MTNKeyboard.Press(VirtualKeyShort.END);
            pointToClick = new Point(table.BoundingRectangle.X + 15, table.BoundingRectangle.Y + 7);
            Mouse.Click(pointToClick);
            MTNKeyboard.Press(VirtualKeyShort.END);

            const int rowHeight = 18;
            const int rows = 12;
            pointToClick = new Point(cargoEnquiryForm.tblGeneralEdit.BoundingRectangle.Right - 100,
                cargoEnquiryForm.tblGeneralEdit.BoundingRectangle.Bottom - (rowHeight * rows) - (rowHeight / 2));
            Mouse.DoubleClick(pointToClick);
        }

        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n			<cargoTypeDescr>STEEL</cargoTypeDescr>\n            <product>PIPE</product>\n            <id>JLG44200A01</id>\n            <operatorCode>PAC</operatorCode>\n            <locationId>FSA5</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>MSCK000005</voyageCode>\n            <totalQuantity>300</totalQuantity>\n		   <messageMode>D</messageMode>\n        </CargoOnSite>\n          </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnShip
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n			<cargoTypeDescr>STEEL</cargoTypeDescr>\n            <product>PIPE</product>\n            <id>JLG44200A01</id>\n            <operatorCode>COS</operatorCode>\n            <locationId>MSCK000006</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>MSCK000006</voyageCode>\n            <totalQuantity>300</totalQuantity>\n		   <messageMode>A</messageMode>\n        </CargoOnShip>\n          </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }
}
