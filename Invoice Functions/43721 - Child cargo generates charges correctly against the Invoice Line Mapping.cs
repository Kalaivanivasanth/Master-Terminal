

using System;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43721 : MTNBase
    {
       
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup() =>base.TestCleanup();            

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();

            FormObjectBase.NavigationMenuSelection(@"Invoice Functions|Maintenance|Transaction Map");
            var transactionMapForm = new InvoiceTransactionMapForm(@"Transaction Map DFBU TT1");

            transactionMapForm.chkCargo.DoClick();
            transactionMapForm.chkEvent.DoClick(false);
            transactionMapForm.chkVoyage.DoClick(false);
            transactionMapForm.chkSnapshotSummary.DoClick(false);
            transactionMapForm.chkCargoAllocation.DoClick(false);
            transactionMapForm.chkBillOfLading.DoClick(false);
            //transactionMapForm.btnSearch.DoClick();
            transactionMapForm.DoSearch();

            transactionMapForm.lstTransactionMaps.FindItemInList(@"Cargo|Test43721");
            transactionMapForm.lstTransactionMaps.DoRightClick();

            transactionMapForm.ContextMenuSelect(@"Set To Active");
        }
       

        [TestMethod]
        public void ChildCargoGeneratesChargesCorrectlyAgainstInvoiceLineMapping()
        {
            MTNInitialize();

            // Step 4
            FormObjectBase.MainForm.OpenVoyageOperationsFromToolbar();
            VoyageOperationsForm voyageOperationsForm = new VoyageOperationsForm();
            voyageOperationsForm.GetSearcherTab_ClassicMode();
            voyageOperationsForm.GetSearcherTab();

            // Step 5
            voyageOperationsForm.ChkHolds.DoClick();

            voyageOperationsForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);

            voyageOperationsForm.DoSelect();
            voyageOperationsForm.GetMainDetails();

            // Step 6
            Mouse.RightClick();
            voyageOperationsForm.ContextMenuSelect(@"Cargo|Create New Cargo On Vessel...");

            var roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Add Cargo TT1");
           // roadGateDetailsReceiveForm.();

            // Step 7
            roadGateDetailsReceiveForm.CmbCargoType.SetValue(CargoType.ISOContainer, doDownArrow: true, searchSubStringTo: CargoType.ISOContainer.Length - 5, additionalWaitTimeout: 2000);
            roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO220A, doDownArrow: true);
            roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG43721A01", 20);
            roadGateDetailsReceiveForm.CmbCommodity.SetValue(Commodity.GEN, doDownArrow: true, searchSubStringTo: Commodity.GEN.Length - 1);
            roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("7000");
            roadGateDetailsReceiveForm.TxtLocation.SetValue(@"MSCK000002");
            roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            
            roadGateDetailsReceiveForm.BtnAttachments.DoClick();

            // Step 8
            var attachmentReceiveReleaseCargoForm = new AttachmentReceiveReleaseCargoForm(@"Add Cargo TT1");
            attachmentReceiveReleaseCargoForm.CmbCargoType.SetValue(CargoType.GeneralCargo, doDownArrow: true,
                searchSubStringTo: CargoType.GeneralCargo.Length - 5, waitTime: 20);
            attachmentReceiveReleaseCargoForm.TxtCargoId.SetValue(@"JLG43721B01");
            attachmentReceiveReleaseCargoForm.BtnSave.DoClick();

                // Step 9
            roadGateDetailsReceiveForm.SetFocusToForm();
            roadGateDetailsReceiveForm.BtnSave.DoClick();

            // Step 10
            /*// Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date 
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Pre-Notes TT1");
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Pre-Notes TT1");

            // Step 11
            voyageOperationsForm.SetFocusToForm();
            voyageOperationsForm.CmbDischargeTo.SetValue(TT1.TerminalArea.MKBS01, doDownArrow: true, searchSubStringTo: TT1.TerminalArea.MKBS01.Length - 1);

            // Step 12
            /*// Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(voyageOperationsForm.TblOnVessel,
                @"ID^JLG43721A01~Location^MSCK000002", ClickType.ContextClick, rowHeight: 16);*/
            voyageOperationsForm.TblOnVessel1.FindClickRow(
                new[] { "ID^JLG43721A01~Location^MSCK000002" }, ClickType.ContextClick);
            voyageOperationsForm.ContextMenuSelect(@"Actual Discharge | Actual Discharge Selected");

            // Step 13
            try
            {
                // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date warningErrorForm = new WarningErrorForm(@"Warnings for Discharge Cargo TT1");
                // Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date warningErrorForm.btnSave.DoClick();
                WarningErrorForm.CompleteWarningErrorForm("Warnings for Discharge Cargo TT1");
            }
            catch (Exception) { /* ignored */ }

            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.GeneralCargo, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG43721B01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.DoViewTransactions();

            // Step 17
            CargoEnquiryTransactionForm cargoEnquiryTransactionForm =
                FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG43721B01 TT1");
            cargoEnquiryTransactionForm.GetChargesTab();

            /*// Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblCharges,
                @"Debtor^Mediterranean Shipping Co~Manual^false~Status^Normal~Transaction^Received - Ship~Type^Invoice Line~Narration^43721_FAC General Cargo on MAFI");
            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblCharges,
                @"Debtor^RDT~Manual^false~Status^Normal~Transaction^Received - Ship~Type^Invoice Line~Narration^43721_R&D General Cargo");*/
            cargoEnquiryTransactionForm.tblCharges1.FindClickRow(new[]
            {
                "Debtor^Mediterranean Shipping Co~Manual^false~Status^Normal~Transaction^Received - Ship~Type^Invoice Line~Narration^43721_FAC General Cargo on MAFI",
                "Debtor^RDT~Manual^false~Status^Normal~Transaction^Received - Ship~Type^Invoice Line~Narration^43721_R&D General Cargo"
            });
        }

        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n   \n    <CargoOnSite Terminal='TT1'>\n      <TestCases>43721</TestCases>\n      <cargoTypeDescr>GENERAL CARGO</cargoTypeDescr>\n      <id>JLG43721A01</id>\n      <isoType>220A</isoType>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>6000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }

}
