using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using HardcodedData.SystemData;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40797 : MTNBase
    {
        RoadGateDetailsReceiveForm receiveFullContainerForm;
        AttachmentReceiveReleaseCargoForm attachmentReceiveReleaseCargoForm;
        CargoEnquiryTransactionForm cargoEnquiryTransactionForm;
        EDIOperationsForm ediOperationsForm;

        protected static string ediFile1 = "M_40797_stop_clearance.edi";
        protected static string ediFile2 = "M_40797_vgm_update.edi";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_40797_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            CreateDataFile(ediFile1,
                ";ISO Container;JLG40797A01;2;D;00081947 N 17/08/18;;;");
            CreateDataFile(ediFile2,
                "U;JLG40797A01;2722;ITT S.P.A.;ITT S.P.A.;");

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void LoadingVGMediFileNotReapplyStopsClearedFromChildCargo()
        {
            
            MTNInitialize();
            
            // Step 3 Open Gate Functions | Road Gate
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");

            // Step 4 Enter Registration, Carrier, Gate and click Receive Full button
            roadGateForm = new RoadGateForm(@"Road Gate TT1");
            //roadGateForm.txtRegistration.SetValue(@"40797");
            //roadGateForm.cmbCarrier.SetValue(@"American Auto Tpt");
            //roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.SetRegoCarrierGate("40797");
            roadGateForm.btnReceiveFull.DoClick();


            // Step 5 Enter ISO Type, Cargo ID, Commodity, Total Weght, IMEX, Voyage, Operator, Discharge Port
            receiveFullContainerForm = new RoadGateDetailsReceiveForm(@"Receive Full Container TT1");
            receiveFullContainerForm.CmbIsoType.SetValue(ISOType.ISO220A, doDownArrow: true);
            receiveFullContainerForm.TxtCargoId.SetValue("JLG40797A01");
            receiveFullContainerForm.CmbCommodity.SetValue(Commodity.GEN, doDownArrow: true);
            receiveFullContainerForm.MtTotalWeight.SetValueAndType("6000");
            receiveFullContainerForm.CmbImex.SetValue(IMEX.Export, doDownArrow: true); //, 1000);
            receiveFullContainerForm.CmbVoyage.SetValue(Voyage.MSCK000002, doDownArrow: true);
            receiveFullContainerForm.CmbOperator.SetValue(Operator.AER,  doDownArrow: true); //, 100);
            receiveFullContainerForm.CmbDischargePort.SetValue(Port.LYTNZ, doDownArrow: true);

            // Step 6 Click on the ellipses button next to Attachment(s) field 
            receiveFullContainerForm.BtnAttachments.DoClick();

            // Step 7 Enter Cargo Type -Break - Bulk Cargo, Cargo ID -JLG40797A02, Commodity - GENL, Total Weight -2000 lbs, Operator - AER and click the Save-Next button
            
            attachmentReceiveReleaseCargoForm = new AttachmentReceiveReleaseCargoForm(@"Attachment Details Container TT1");
            attachmentReceiveReleaseCargoForm.CmbCargoType.SetValue(CargoType.BreakBulkCargo, additionalWaitTimeout: 500, searchSubStringTo: 5, doDownArrow: true);
            attachmentReceiveReleaseCargoForm.TxtCargoId.SetValue(@"JLG40797A02");
            attachmentReceiveReleaseCargoForm.CmbCommodity.SetValue(Commodity.GENL, doDownArrow: true);
            attachmentReceiveReleaseCargoForm.MtTotalWeight.SetValueAndType("2000", "lbs");
            attachmentReceiveReleaseCargoForm.CmbOperator.SetValue(Operator.AER,  doDownArrow: true);
            attachmentReceiveReleaseCargoForm.BtnSaveNext.DoClick();

            // Step 8 Enter Cargo Type -Break - Bulk Cargo, Cargo ID -JLG40797A03, Commodity - GENL, Total Weight -2000 lbs, Operator - AER and click the Save button 
            attachmentReceiveReleaseCargoForm.CmbCargoType.SetValue(CargoType.BreakBulkCargo, additionalWaitTimeout: 500, searchSubStringTo: 5, doDownArrow: true);
            attachmentReceiveReleaseCargoForm.TxtCargoId.SetValue(@"JLG40797A03");
            attachmentReceiveReleaseCargoForm.CmbCommodity.SetValue(Commodity.GENL, doDownArrow: true);
            attachmentReceiveReleaseCargoForm.MtTotalWeight.SetValueAndType("2000", "lbs");
            attachmentReceiveReleaseCargoForm.CmbOperator.SetValue(Operator.AER,  doDownArrow: true);
            attachmentReceiveReleaseCargoForm.BtnSave.DoClick();

            // Step 9 Click the Save button
            receiveFullContainerForm.SetFocusToForm();
            receiveFullContainerForm.BtnSave.DoClick();

            // Step 10 Click the Save button
            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            // Step 11 Click the Save button
            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();
            
            // Step 12 Open Yard Functions | Road Operations 
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);

            // Step 13 Select the Vehicle 40797
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            MTNControlBase.FindClickRowInList(roadOperationsForm.lstVehicles, @"40797 (1/0) - ICA - Yard Interchange");

            // Step 14 Select Receipt JLG40797A01 in the yard and click the Move It button  
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^40797~Cargo ID^JLG40797A01", ClickType.Click, rowHeight: 16);
            //roadOperationsForm.btnMoveIt.DoClick();
            roadOperationsForm.DoToolbarClick(roadOperationsForm.MainToolbar, (int)RoadOperationsForm.Toolbar.MainToolbar.MoveIt, "Move It");

            // Step 15 Select the vehicle 40797 right click and select Process Road Exit 
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^40797", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");
            roadOperationsForm.CloseForm();


            // Step 16 Open General Functions | Cargo Enquiry  
            FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm();

            // Step 17 Enter Cargo Type - Blank, Cargo ID -JLG40797 and click the Search button:
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @" ", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG40797");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.SearchToolbar, (int)CargoEnquiryForm.Toolbar.SearcherToolbar.Search, "Search");

            // Step 18 For JLG40797A01, click the View Transactions button and check that it has a transaction Stop Set: STOP_40797
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40797A01", ClickType.Click);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.ViewTransactions();

            cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for JLG40797A01 TT1");

            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Stop Set~Details^Stop Set: STOP_40797",
                 ClickType.Click, rowHeight: 17);
            cargoEnquiryTransactionForm.CloseForm();

            // Step 19 For JLG40797A02, click the View Transactions button and check that it has a transaction Stop Set: STOP_40797
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40797A02", ClickType.Click);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.ViewTransactions();

            cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for JLG40797A02 TT1");

            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Stop Set~Details^Stop Set: STOP_40797",
                 ClickType.Click, rowHeight: 17);
            cargoEnquiryTransactionForm.CloseForm();

            // Step 20 For JLG40797A03, click the View Transactions button and check that it has a transaction Stop Set: STOP_40797
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40797A03", ClickType.Click);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.ViewTransactions();

            cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for JLG40797A03 TT1");

            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Stop Set~Details^Stop Set: STOP_40797",
                 ClickType.Click, rowHeight: 17);
            cargoEnquiryTransactionForm.CloseForm();
            
            // Step 21 - 22
            FormObjectBase.NavigationMenuSelection(@"EDI Functions | EDI Operations", forceReset: true);
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperationsForm.DeleteEDIMessages(@"Cargo Update Request", @"40797", @"Loaded");
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1, specifyType: true, fileType: @"40797_Stop");
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Loaded", @"Verify");
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Verified", @"Load To DB");
            
            // Step 23 In Cargo Enquiry, for JLG40797A01, 
            // -click the Status Tab and check the Stops(dbl click) field and Operator Export Release Number field
            //- click the View Transactions button and check that it has a transaction
            
            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40797A01", ClickType.Click);

            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4087");

            string strActualValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblStatus, @"Stops (dbl click)");
            string strExpectedValue = @"STOP_40797";
            Assert.IsFalse(strActualValue.Contains(strExpectedValue), @" Stop STOP_40797 should not be set" + ", Actual = " + strActualValue);

            // Step 24 In Cargo Enquiry, for JLG40797A02, 
            // -click the Status Tab and check the Stops(dbl click) field and Operator Export Release Number field
            //- click the View Transactions button and check that it has a transaction
            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40797A02", ClickType.Click);

            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4093");

            strActualValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblStatus, @"Stops (dbl click)");
            Assert.IsFalse(strActualValue.Contains(strExpectedValue), @" Stop STOP_40797 should not be set" + ", Actual = " + strActualValue);

            // Step 25 In Cargo Enquiry, for JLG40797A03, 
            // -click the Status Tab and check the Stops(dbl click) field and Operator Export Release Number field
            //- click the View Transactions button and check that it has a transaction
            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40797A03", ClickType.Click);

            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4093");

            strActualValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblStatus, @"Stops (dbl click)");
            Assert.IsFalse(strActualValue.Contains(strExpectedValue), @" Stop STOP_40797 should not be set" + ", Actual = " + strActualValue);


            // Step 26 - 30
            ediOperationsForm.SetFocusToForm();
            ediOperationsForm.LoadEDIMessageFromFile(ediFile2, specifyType: true, fileType: @"40797_Vgm");
            ediOperationsForm.ChangeEDIStatus(ediFile2, @"Loaded", @"Verify");
            ediOperationsForm.ChangeEDIStatus(ediFile2, @"Verify warnings", @"Load To DB");
            
            // Step 31
            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40797A01", ClickType.Click);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.ViewTransactions();

            cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for JLG40797A01 TT1");

            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, 
                @"Type^Edited~Details^isWeightCertified No => Yes: totalWeight 6000.000 lbs => 6000.988 lbsweightCertifiedName  => ITT S.P.A.: weightCertifiedReference  => ITT S.P.A.",
                ClickType.Click, rowHeight: 17);
            cargoEnquiryTransactionForm.CloseForm();

            // Step 32
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40797A02", ClickType.Click);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.ViewTransactions();

            cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for JLG40797A02 TT1");

            Assert.IsFalse(MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Stop Set~Details^Stop Set: STOP_40797", 
                 ClickType.Click, SearchType.Exact, findInstance: 2, rowHeight: 17, doAssert: false));
            cargoEnquiryTransactionForm.CloseForm();

            // Step 33
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40797A03", ClickType.Click);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.ViewTransactions();

            cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for JLG40797A03 TT1");

            Assert.IsFalse(MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Stop Set~Details^Stop Set: STOP_40797",
                 ClickType.Click, SearchType.Exact, findInstance: 2, rowHeight: 17, doAssert: false));
            cargoEnquiryTransactionForm.CloseForm();
        }

        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite\n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40797A01</id>\n            <isoType>220A</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>AER</operatorCode>\n            <dischargePort>NZLYT</dischargePort>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GEN</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n            <id>JLG40797A02</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>AER</operatorCode>\n            <dischargePort>NZBLU</dischargePort>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GENL</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n            <id>JLG40797A03</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>AER</operatorCode>\n            <dischargePort>NZBLU</dischargePort>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GENL</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }
}
