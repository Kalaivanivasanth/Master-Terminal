using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.General.Future_Storage
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44183 : MTNBase
    {

        BackgroundApplicationForm _backgroundApplicationForm;

        DateTime _currentDateTime = DateTime.Now;

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
        public void FutureStorageAnniversaryBGPFutureTimerActionDateChangesOnAdditionalStorageDays()
        {
            MTNInitialize();

            // Step 3
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions | Operations | Voyage Operations ");
            VoyageOperationsForm voyageOperationsForm = new VoyageOperationsForm();
            /*voyageOperationsForm.GetSearcherTab_ClassicMode();
            voyageOperationsForm.GetSearcherTab();

            // Step 4
            voyageOperationsForm.chkLOLOBays.DoClick();
            voyageOperationsForm.CmbVoyage.SetValue(Voyage.MSCK000002, doDownArrow: true);
            voyageOperationsForm.DoSelect();*/
            voyageOperationsForm.DoSearchForVoyageGetDetails(new VoyageOperationsSearcherArguments { Voyage = "MSCK000002 MSC KATYA R.", LOLOBays = true});
            voyageOperationsForm.GetMainDetails();

            // Step 5
            voyageOperationsForm.CmbDischargeTo.SetValue(@"FSA3");
            // MTNControlBase.FindClickRowInTable(voyageOperationsForm.TblOnVessel,
                // @"ID^JLG44183A01~Location^MSCK000002~Total Quantity^1~Cargo Type^ISO Container", ClickType.ContextClick, rowHeight:16);
            voyageOperationsForm.TblOnVessel1.FindClickRow(["ID^JLG44183A01~Location^MSCK000002~Total Quantity^1~Cargo Type^ISO Container"], ClickType.ContextClick);
        
            // Step 7
            voyageOperationsForm.ContextMenuSelect(@"Actual Discharge | Actual Discharge Selected");

            // Step 8
            FormObjectBase.NavigationMenuSelection(@"Background Process | Background Processing", forceReset:true);
            /*_backgroundApplicationForm = new BackgroundApplicationForm();
            
            var actionFutureDate = _currentDateTime.AddDays(11);
            var anniversaryDate = _currentDateTime.AddDays(10);

            _backgroundApplicationForm
                .EnableFutureTimerEnquiry()
                .DoFutureTimerEnquiry()
                .ValidateFutureTimerEnquiryTableDetails(new FutureTimerEnquiryArguments
                    {
                        SearchCriteria = new[]
                        {
                            new GetSetFieldsOnFormArguments
                                { FieldName = FutureTimerEnquiryForm.FieldNames.CargoId, FieldValue = "JLG43689A01" },
                        },
                        TableDetailsToValidate = new[]
                        {
                            "Request Type^Future Storage Process~Action Future Date^" + actionFutureDate.ToString(@"dd/MM/yyyy") + 
                            "~Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^" + anniversaryDate.ToString(@"dd/MM/yyyy"),
                        }
                    },
                    out var tableDetailsNotFound);*/
            ValidateFutureTimerRowDetails(11, 10, "JLG44183A01",
                "Request Type^Future Storage Process~Action Future Date^<ActionFutureDate>" +
                "~Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^<AnniversaryDate>");

            //FormObjectBase.NavigationMenuSelection("General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            ////MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", " ", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", "JLG44183A01");
            cargoEnquiryForm.DoSearch();
            ////MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, "ID^JLG44183A01");

            // Step 12 - 13
            var freeStorageExpiryDate = _currentDateTime.AddDays(10);
            var futureStorageAnniversary = _currentDateTime.AddDays(10);
            
            cargoEnquiryForm.tblData2.FindClickRow(
                new[]
                {
                    "Location ID^FSA3~ID^JLG44183A01~Cargo Type^ISO Container~Free Storage Expiry Date^" +
                    freeStorageExpiryDate.ToString(@"dd/MM/yyyy") + "~Type^2200~Future Storage Anniversary^" +
                    futureStorageAnniversary.ToString(@"dd/MM/yyyy")
                }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo | Edit Additional Storage Days...");
            AdditionalFreeStorageDaysForm additionalFreeStorageDaysForm = new AdditionalFreeStorageDaysForm(cargoEnquiryForm.GetTerminalName());

            // Step 14
            additionalFreeStorageDaysForm.txtAdditionalDays.SetValue(@"2");
            additionalFreeStorageDaysForm.txtReason.SetValue(@"Testing");
            additionalFreeStorageDaysForm.btnOK.DoClick();

            // Step 15
            freeStorageExpiryDate = _currentDateTime.AddDays(12);
            futureStorageAnniversary = _currentDateTime.AddDays(12);
            /*MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Location ID^FSA3~ID^JLG44183A01~Cargo Type^ISO Container~Free Storage Expiry Date^" + freeStorageExpiryDate.ToString(@"dd/MM/yyyy") +
                @"~Type^2200~Future Storage Anniversary^" + futureStorageAnniversary.ToString(@"dd/MM/yyyy"));*/
            cargoEnquiryForm.tblData2.FindClickRow(
                new[]
                { "Location ID^FSA3~ID^JLG44183A01~Cargo Type^ISO Container~Free Storage Expiry Date^" + freeStorageExpiryDate.ToString(@"dd/MM/yyyy") +
                  @"~Type^2200~Future Storage Anniversary^" + futureStorageAnniversary.ToString(@"dd/MM/yyyy") });

            // Step 16
            //_backgroundApplicationForm.SetFocusToForm();
            ValidateFutureTimerRowDetails(13, 12, "JLG44183A01",
                "Request Type^Future Storage Process~Action Future Date^<ActionFutureDate>" +
                "~Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^<AnniversaryDate>");
            /*Keyboard.Press(VirtualKeyShort.F5);
            _backgroundApplicationForm.tblRequests.Focus();
            actionFutureDate = _currentDateTime.AddDays(13);
            anniversaryDate = _currentDateTime.AddDays(12);
            /*MTNControlBase.FindClickRowInTable(_backgroundApplicationForm.tblRequests,
                @"Request Type^Future Storage Process~Action Future Date^" + actionFutureDate.ToString(@"dd/MM/yyyy") + @"~" +
                @"Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^" + anniversaryDate.ToString(@"dd/MM/yyyy"),rowHeight: 16);#1#
            _backgroundApplicationForm.TblRequests.FindClickRow(
                new[]
                {
                    "Request Type^Future Storage Process~Action Future Date^" + actionFutureDate.ToString(@"dd/MM/yyyy") + @"~" +
                    @"Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^" + anniversaryDate.ToString(@"dd/MM/yyyy")
                });*/

            // Step 17
            cargoEnquiryForm.SetFocusToForm();
            /*MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Location ID^FSA3~ID^JLG44183A01~Cargo Type^ISO Container~Free Storage Expiry Date^" + freeStorageExpiryDate.ToString(@"dd/MM/yyyy") +
                @"~Type^2200~Future Storage Anniversary^" + futureStorageAnniversary.ToString(@"dd/MM/yyyy"));*/
            cargoEnquiryForm.tblData2.FindClickRow(
                new[]
                { "Location ID^FSA3~ID^JLG44183A01~Cargo Type^ISO Container~Free Storage Expiry Date^" + freeStorageExpiryDate.ToString(@"dd/MM/yyyy") +
                  @"~Type^2200~Future Storage Anniversary^" + futureStorageAnniversary.ToString(@"dd/MM/yyyy") }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo | Edit Additional Storage Days...");
            additionalFreeStorageDaysForm = new AdditionalFreeStorageDaysForm(cargoEnquiryForm.GetTerminalName());

            // Step 14
            //MTNControlBase.SetValue(additionalFreeStorageDaysForm.txtAdditionalDays, @"2");
            additionalFreeStorageDaysForm.txtAdditionalDays.SetValue("3");
            //MTNControlBase.SetValue(additionalFreeStorageDaysForm.txtReason, @"Testing");
            additionalFreeStorageDaysForm.txtReason.SetValue("Testing - 3 Days");
            additionalFreeStorageDaysForm.btnOK.DoClick();
            //cargoEnquiryForm.btnEdit.DoClick();
            /*cargoEnquiryForm.DoEdit();

            // Step 18
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Additional Storage Days" , @"3");
            //cargoEnquiryForm.btnSave.DoClick();
            cargoEnquiryForm.DoSave();*/

            /*warningErrorForm = new WarningErrorForm("Warnings for Tracked Item Update " + cargoEnquiryForm.GetTerminalName());

            string[] warningErrorToCheck =
            {
                "Code :75016. The Container Id (JLG44183A01) failed the validity checks and may be incorrect."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();
            WarningErrorForm.CheckErrorMessagesExist(
                $"Warnings for Tracked Item Update {cargoEnquiryForm.GetTerminalName()}",
                new[] { "Code :75016. The Container Id (JLG44183A01) failed the validity checks and may be incorrect." }, false);*/
            
            // Step 19
            freeStorageExpiryDate = _currentDateTime.AddDays(13);
            futureStorageAnniversary = _currentDateTime.AddDays(13);
            /*MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Location ID^FSA3~ID^JLG44183A01~Cargo Type^ISO Container~Free Storage Expiry Date^" + freeStorageExpiryDate.ToString(@"dd/MM/yyyy") +
                @"~Type^2200~Future Storage Anniversary^" + futureStorageAnniversary.ToString(@"dd/MM/yyyy"));*/
            cargoEnquiryForm.tblData2.FindClickRow(new[]
            {
                "Location ID^FSA3~ID^JLG44183A01~Cargo Type^ISO Container~Free Storage Expiry Date^" +
                freeStorageExpiryDate.ToString(@"dd/MM/yyyy") +
                @"~Type^2200~Future Storage Anniversary^" + futureStorageAnniversary.ToString(@"dd/MM/yyyy")
            });

            // Step 20
            _backgroundApplicationForm.SetFocusToForm();
            ValidateFutureTimerRowDetails(14, 13, "JLG44183A01",
                "Request Type^Future Storage Process~Action Future Date^<ActionFutureDate>" +
                "~Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^<AnniversaryDate>" );
            /*Keyboard.Press(VirtualKeyShort.F5);
            _backgroundApplicationForm.tblRequests.Focus();
            actionFutureDate = _currentDateTime.AddDays(14);
            anniversaryDate = _currentDateTime.AddDays(13);
            /*MTNControlBase.FindClickRowInTable(_backgroundApplicationForm.tblRequests,
                @"Request Type^Future Storage Process~Action Future Date^" + actionFutureDate.ToString(@"dd/MM/yyyy") + @"~" +
                @"Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^" + anniversaryDate.ToString(@"dd/MM/yyyy"), rowHeight: 16);#1#
            _backgroundApplicationForm.TblRequests.FindClickRow(new[]
            {
                "Request Type^Future Storage Process~Action Future Date^" + actionFutureDate.ToString(@"dd/MM/yyyy") +
                @"~" + @"Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^" +
                anniversaryDate.ToString(@"dd/MM/yyyy")
            });*/

            // Step 21
            cargoEnquiryForm.SetFocusToForm();
            freeStorageExpiryDate = _currentDateTime.AddDays(13);
            futureStorageAnniversary = _currentDateTime.AddDays(13);
            /*MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Location ID^FSA3~ID^JLG44183A01~Cargo Type^ISO Container~Free Storage Expiry Date^" + freeStorageExpiryDate.ToString(@"dd/MM/yyyy") +
                @"~Type^2200~Future Storage Anniversary^" + futureStorageAnniversary.ToString(@"dd/MM/yyyy"));*/
            cargoEnquiryForm.tblData2.FindClickRow(new[]
            {
                "Location ID^FSA3~ID^JLG44183A01~Cargo Type^ISO Container~Free Storage Expiry Date^" +
                freeStorageExpiryDate.ToString(@"dd/MM/yyyy") + @"~Type^2200~Future Storage Anniversary^" +
                futureStorageAnniversary.ToString(@"dd/MM/yyyy")
            });
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            // Step 22
            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            /*MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                @"Type^Received - Ship~Details^Received ()  From MSCK000002 to FSA3~User^USERDWAT", rowHeight: 17);*/
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[]
            {
                "Type^Received - Ship~Details^Received ()  From MSCK000002 to FSA3~User^USERDWAT"
            });
            //cargoEnquiryTransactionForm.btnEdit.DoClick();
            cargoEnquiryTransactionForm.DoEdit();

            CargoEnquiryTransactionMaintenanceForm cargoEnquiryTransactionMaintenanceForm =
                    new CargoEnquiryTransactionMaintenanceForm(cargoEnquiryTransactionForm.GetTerminalName());
            
            // Step 23
            Keyboard.Press(VirtualKeyShort.TAB);

            var date = _currentDateTime.AddDays(-10);
            MTNControlBase.SetValueInEditTable(cargoEnquiryTransactionMaintenanceForm.tblDetails, @"Date", date.ToString(@"ddMMyyyy"));

            cargoEnquiryTransactionMaintenanceForm.btnSave.DoClick();

            // Step 24
            cargoEnquiryForm.SetFocusToForm();
            Keyboard.Press(VirtualKeyShort.F5);
            freeStorageExpiryDate = _currentDateTime.AddDays(3);
            futureStorageAnniversary = _currentDateTime.AddDays(3);
            /*MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData,
                @"Location ID^FSA3~ID^JLG44183A01~Cargo Type^ISO Container~Free Storage Expiry Date^" + freeStorageExpiryDate.ToString(@"dd/MM/yyyy") +
                @"~Type^2200~Future Storage Anniversary^" + futureStorageAnniversary.ToString(@"dd/MM/yyyy"));*/
            cargoEnquiryForm.tblData2.FindClickRow(new[]
            {
                "Location ID^FSA3~ID^JLG44183A01~Cargo Type^ISO Container~Free Storage Expiry Date^" +
                freeStorageExpiryDate.ToString(@"dd/MM/yyyy") + @"~Type^2200~Future Storage Anniversary^" +
                futureStorageAnniversary.ToString(@"dd/MM/yyyy")
            });

            // Step 25
            _backgroundApplicationForm.SetFocusToForm();
            ValidateFutureTimerRowDetails(4, 3, "JLG44183A01",
                "Request Type^Future Storage Process~Action Future Date^<ActionFutureDate>" +
                "~Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^<AnniversaryDate>");
            /*Keyboard.Press(VirtualKeyShort.F5);
            _backgroundApplicationForm.tblRequests.Focus();
            actionFutureDate = _currentDateTime.AddDays(4);
            anniversaryDate = _currentDateTime.AddDays(3);
            /*MTNControlBase.FindClickRowInTable(_backgroundApplicationForm.tblRequests,
                @"Request Type^Future Storage Process~Action Future Date^" + actionFutureDate.ToString(@"dd/MM/yyyy") + @"~" +
                @"Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^" + anniversaryDate.ToString(@"dd/MM/yyyy"),rowHeight:16);#1#
            _backgroundApplicationForm.TblRequests.FindClickRow(new[]
            {
                "Request Type^Future Storage Process~Action Future Date^" + actionFutureDate.ToString(@"dd/MM/yyyy") +
                @"~" + @"Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^" +
                anniversaryDate.ToString(@"dd/MM/yyyy")
            });*/

            // Step 26
            if (_backgroundApplicationForm != null)
            {
                _backgroundApplicationForm.SetFocusToForm();
                _backgroundApplicationForm
                    .EnableFutureTimerEnquiry()
                    .DoFutureTimerEnquiry()
                    .DeleteRowsInFutureTimerTable(new FutureTimerEnquiryArguments
                        {
                            SearchCriteria = new[]
                            {
                                new GetSetFieldsOnFormArguments 
                                { FieldName = FutureTimerEnquiryForm.FieldNames.CargoId, FieldValue = "JLG44183A01" },
                            },
                            TableDetailsToValidateOrDelete = new[]
                            {
                                $"Request Type^Future Storage Process~Action Future Date^{_currentDateTime.AddDays(4):dd/MM/yyyy}~Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^{_currentDateTime.AddDays(3):dd/MM/yyyy}"
                            },

                        },
                        out var tableDetailsNotFound5);
                /*Keyboard.Press(VirtualKeyShort.F5);
                _backgroundApplicationForm.tblRequests.Focus();
                actionFutureDate = _currentDateTime.AddDays(4);
                anniversaryDate = _currentDateTime.AddDays(3);
                /*MTNControlBase.FindClickRowInTable(_backgroundApplicationForm.tblRequests,
                    @"Request Type^Future Storage Process~Action Future Date^" + actionFutureDate.ToString(@"dd/MM/yyyy") + @"~" +
                    @"Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^" + anniversaryDate.ToString(@"dd/MM/yyyy"), ClickType.ContextClick, rowHeight: 16);#1#
                _backgroundApplicationForm.TblRequests.FindClickRow(
                    new[]
                    {
                        "Request Type^Future Storage Process~Action Future Date^" + actionFutureDate.ToString(@"dd/MM/yyyy") +
                        @"~" + @"Terminal^TT1~Cargo Id^JLG44183A01~Location^FSA3~Anniversary Date^" +
                        anniversaryDate.ToString(@"dd/MM/yyyy")
                    }, ClickType.ContextClick);*/
                //_backgroundApplicationForm.ContextMenuSelect(@"Clear Request");
            }

            warningErrorForm = new WarningErrorForm("Warnings for Deletion");
            warningErrorForm.btnSave.DoClick();

        }

        string ValidateFutureTimerRowDetails(int actionFutureDateDays, double anniversaryDateDays, string cargoId, string detailsToValidate)
        {
            if (_backgroundApplicationForm == null)
                 _backgroundApplicationForm = new BackgroundApplicationForm();
            else
                _backgroundApplicationForm.SetFocusToForm();
            
            var actionFutureDate = _currentDateTime.AddDays(actionFutureDateDays);
            var anniversaryDate = _currentDateTime.AddDays(anniversaryDateDays);
            
            detailsToValidate = detailsToValidate
                .Replace("<ActionFutureDate>", actionFutureDate.ToString(@"dd/MM/yyyy"))
                .Replace("<AnniversaryDate>", anniversaryDate.ToString(@"dd/MM/yyyy"));

            _backgroundApplicationForm
                .EnableFutureTimerEnquiry()
                .DoFutureTimerEnquiry()
                .ValidateFutureTimerEnquiryTableDetails(new FutureTimerEnquiryArguments
                    {
                        SearchCriteria = new[]
                        {
                            new GetSetFieldsOnFormArguments
                                { FieldName = FutureTimerEnquiryForm.FieldNames.CargoId, FieldValue = cargoId },
                        },
                        TableDetailsToValidateOrDelete = new[] { detailsToValidate }
                    },
                    out var tableDetailsNotFound);
            return tableDetailsNotFound;
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_44183_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		  <TestCases>44183</TestCases>\n		  <cargoTypeDescr>ISO Container</cargoTypeDescr>\n		  <id>JLG44183A01</id>\n		  <isoType>2200</isoType>\n		  <operatorCode>PAC</operatorCode>\n		  <locationId>FSA3</locationId>\n		  <weight>6000</weight>\n		  <imexStatus>Import</imexStatus>\n		  <dischargePort>USJAX</dischargePort>\n		  <voyageCode>MSCK000002</voyageCode>\n		  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnShip
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<isoType>2200</isoType>\n            <id>JLG44183A01</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>PAC</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n			<imex>Import</imex>\n			<weight>6000</weight>\n			 <messageMode>A</messageMode>\n        </CargoOnShip>\n          </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }

}
