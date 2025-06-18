using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNUtilityClasses.Classes;
using System;
using HardcodedData.SystemData;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions.CAMS
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44041 : MTNBase
    {
        private CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

        private BackgroundApplicationForm _backgroundApplicationForm;
      
        private DateTime _startTS;
     
        private const string _testCaseNumber = @"44041";
        private const string _cargoId = @"JLG" + _testCaseNumber + @"A01";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + _testCaseNumber + "_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            // Stop the CAMS(Server)
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm?.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^",
                false);

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void TranshipLoadPortInSMDG213COARRI()
        {
            MTNInitialize();

            _startTS = DateTime.Now;

            // Step 6
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Operations|Voyage Operations");
            VoyageOperationsForm voyageOperationsForm = new VoyageOperationsForm();
            voyageOperationsForm.GetSearcherTab_ClassicMode();
            voyageOperationsForm.GetSearcherTab();

            // Step 7 - 8
            voyageOperationsForm.ChkLoloBays.DoClick();
            //voyageOperationsForm.SetValue(voyageOperationsForm.cmbVoyage, @"MSCK000002");
            voyageOperationsForm.CmbVoyage.SetValue(Voyage.MSCK000002, doDownArrow: true);
            //voyageOperationsForm.btnSelect.DoClick();
            voyageOperationsForm.DoSelect();
            voyageOperationsForm.GetMainDetails();

            // Step 9 - 10
            MTNControlBase.FindClickRowInTable(voyageOperationsForm.TblOnVessel,
                @"ID^" + _cargoId + "~Location^MSCK000002~Total Quantity^1~Cargo Type^ISO Container",
                ClickType.ContextClick, rowHeight: 16);
            /*voyageOperationsForm.tblOnVessel1.FindClickRow(
                new[] { $"ID^{_cargoId}~Location^MSCK000002~Total Quantity^1~Cargo Type^ISO Container" },
                ClickType.ContextClick);*/

            voyageOperationsForm.ContextMenuSelect(@"Tranship...", waitTimeOut: 2000);
            TranshipForm transhipForm = new TranshipForm(_cargoId + @" TT1");

            //MTNControlBase.SetValue(transhipForm.cmbOutboundVoyage, @"GRIG001");
            transhipForm.cmbOutboundVoyage.SetValue(@"GRIG001	JOLLY GRIGIO", doDownArrow: true);
            //MTNControlBase.SetValue(transhipForm.cmbDischargePort, @"AKL (NZ) Auckland");
            transhipForm.cmbDischargePort.SetValue(Port.AKLNZ);
            //MTNControlBase.SetValue(transhipForm.cmbDestinationPort, @"NPE (NZ) Napier");
            transhipForm.cmbDestinationPort.SetValue(Port.NPENZ);
            transhipForm.btnOK.DoClick();

            voyageOperationsForm.SetFocusToForm();

            //MTNControlBase.SendTextToCombobox(voyageOperationsForm.cmbDischargeTo, @"MKBS03");
            voyageOperationsForm.CmbDischargeTo.SetValue(@"MKBS03");

            MTNControlBase.FindClickRowInTable(voyageOperationsForm.TblOnVessel,
                @"ID^" + _cargoId + "~Location^MSCK000002~Total Quantity^1~Cargo Type^ISO Container~IMEX Status^Tranship",
                ClickType.ContextClick, rowHeight: 16);
            /*voyageOperationsForm.tblOnVessel1.FindClickRow(
                new[] { $"ID^{_cargoId}~Location^MSCK000002~Total Quantity^1~Cargo Type^ISO Container~IMEX Status^Tranship" },
                    ClickType.ContextClick);*/

            voyageOperationsForm.ContextMenuSelect(@"Actual Discharge | Actual Discharge Selected");

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Discharge Cargo TT1");
            warningErrorForm.btnSave.DoClick();

            // Step 13 - 19
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            FormObjectBase.NavigationMenuSelection(@"EDI Functions | EDI CAMS Protocols", forceReset: true);
            EDICAMProtocolForm ediCAMProtocolForm = new EDICAMProtocolForm();
            MTNControlBase.FindClickRowInTable(ediCAMProtocolForm.tblProtocols,
                 @"Protocol Description^STANDARD COARRI SMDG213~Name^TEST44041", xOffset: 40);

            var endDateTime = DateTime.Now.AddMinutes(1);

            ediCAMProtocolForm.GetDetailsTabAndDetails();
            ediCAMProtocolForm.RunAdhocReport(_startTS.ToString(@"ddMMyyyy"),
                _startTS.ToString(@"HHmm"), endDateTime.ToString(@"ddMMyyyy"),
                endDateTime.ToString(@"HHmm"), getTab: true);

            // Step 20 - 25
            FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", _cargoId);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + _cargoId, rowHeight: 18);
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Ship Ops");
            cargoEnquiryForm.CargoEnquiryShipOpsTab();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblShipOpsEdit, @"Load Port", @"USJAX");
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Tranship");
            cargoEnquiryForm.CargoEnquiryTranshipTab();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblTranshipEdit,
                @"Inbound Voyage Load Port", @"AUADL");

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + _cargoId, rowHeight: 18);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            _cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for " + _cargoId + " (Tranship Outbound) TT1");

             MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^EDI Info Sent",
                 ClickType.DoubleClick);

             // Check logging details
             LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm(@"Logging Details");

             string[] linesToCheck =
             {
                 @"UNA:+.?",
                 @"UNB+UNOA:2+TEST44041+",
                 @"LOC+11+USJAX:139:6",
                 @"LOC+7+NZNPE:139:6",
                 @"LOC+11+NZAKL:139:6",
                 @"LOC+9+AUADL:139:6"
             };

             loggingDetailsForm.FindStringsInTable(linesToCheck);
             loggingDetailsForm.DoCancel();

        }



        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<isoType>2200</isoType>\n            <id>JLG44041A01</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n			<weight>8000</weight>\n			<imexStatus>Import</imexStatus>\n		  <commodity>APPL</commodity>\n			 <messageMode>D</messageMode>\n        </CargoOnSite>\n          </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<isoType>2200</isoType>\n            <id>JLG44041A01</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n			<loadPort>AUADL</loadPort>\n			<weight>8000</weight>\n			<imexStatus>Import</imexStatus>\n		  <commodity>APPL</commodity>\n			 <messageMode>A</messageMode>\n        </CargoOnShip>\n          </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads



    }

}
