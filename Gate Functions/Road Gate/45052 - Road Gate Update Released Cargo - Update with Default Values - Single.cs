using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45052 : MTNBase
    {
        TerminalConfigForm _terminalConfigForm ;

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

            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            _terminalConfigForm = new TerminalConfigForm();
            _terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Road Gate Update Released Cargo", @"Update With Default Values - Single", 
                rowDataType: EditRowDataType.ComboBox);
            _terminalConfigForm.CloseForm();
        }


        [TestMethod]
        public void RoadGateUpdateReleasedCargoWithDefaultValuesSingle()
        {
            MTNInitialize();
            
            // Step 7
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1");

            // Step 8
            roadGateForm.SetRegoCarrierGate("45052");
            roadGateForm.txtNewItem.SetValue(@"JLG45052ROAD001", 10);

            // Step 9  
            RoadGatePickerForm picker = new RoadGatePickerForm(formTitle: @"Picker");
            picker.btnOK.DoClick();
            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm("Release Full Container  TT1");

            // Step 10 - 11
            roadGateDetailsReleaseForm.TxtCargoId.Click();
            Keyboard.Press(VirtualKeyShort.TAB);
            roadGateDetailsReleaseForm.CheckCargoTableForTestCase45052();
                        
            // Step 12
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            // Step 13
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            string[] warningErrorToCheck = 
             {
                   "Code :79688. Cargo JLG45052A03's availability status of Unavailable for Release does not match the request's availability status of Available for release"
             };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();

            // Step 14
            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // @"Type^Release Full~Detail^JLG45052A03; MSC; 2200~Booking/Release^JLG45052ROAD001");
            roadGateForm.TblGateItems.FindClickRow(["Type^Release Full~Detail^JLG45052A03; MSC; 2200~Booking/Release^JLG45052ROAD001"]);            roadGateForm.btnSave.DoClick();

            // Step 15 - 17
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new [] { "45052" });

            // Step 18
            // Thursday, 30 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Release Requests", forceReset: true);
            FormObjectBase.MainForm.OpenReleaseRequestFromToolbar();
            ReleaseRequestForm releaseRequestForm = new ReleaseRequestForm();

            // Step 19
            releaseRequestForm.cmbView.SetValue(@"Active");
            releaseRequestForm.cmbType.SetValue(@"Road");
            MTNControlBase.SetValueInEditTable(releaseRequestForm.tblSearchTable, @"Release No", @"JLG45052ROAD001");
            releaseRequestForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(releaseRequestForm.tblReleaseRequests,
                // @"Status^Active~Release No^JLG45052ROAD001", ClickType.ContextClick);
            releaseRequestForm.TblReleaseRequests.FindClickRow(["Status^Active~Release No^JLG45052ROAD001"], ClickType.ContextClick);            releaseRequestForm.ContextMenuSelect(@"Set Release Status");
            ConfirmationFormOKwithText confirmationFormOKwithText = new ConfirmationFormOKwithText(@"Edit Status TT1", FlaUI.Core.Definitions.ControlType.Pane,
                automationIdInput:@"6016", automationIdMessage: @"4003");

            // Step 21
            confirmationFormOKwithText.txtInput.SetValue(@"Complete");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            confirmationFormOKwithText.btnOK.DoClick();

            // Step 22
            releaseRequestForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(releaseRequestForm.tblReleaseRequests,
                // @"Status^Complete~Release No^JLG45052ROAD001");
            releaseRequestForm.TblReleaseRequests.FindClickRow(["Status^Complete~Release No^JLG45052ROAD001"]);            releaseRequestForm.DoDelete();
            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Deletion");
            confirmationFormYesNo.btnYes.DoClick();

        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_45052_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>45052</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45052A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 010002</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <consignee>ABCNE</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>45052</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45052A02</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 010003</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <consignee>CCS</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n	  \n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>45052</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45052A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 010002</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <consignee>ABCNE</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>45052</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45052A02</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 010003</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <consignee>CCS</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>45052</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45052A03</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 010004</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <consignee>CSKO</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");

            // Add Release Request
            CreateDataFileToLoad(@"CreateReleaseRequest.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalRequestMultiLine>\n   <AllRequestHeader>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <RequestHeader Terminal='TT1'>\n         <releaseByType>false</releaseByType>\n         <messageMode>A</messageMode>\n         <operatorCode>MSL</operatorCode>\n         <voyageCode>MSCK000002</voyageCode>\n         <releaseRequestNumber>JLG45052ROAD001</releaseRequestNumber>\n         <releaseTypeStr>Road</releaseTypeStr>\n         <statusBulkRelease>Active</statusBulkRelease>\n         <subTerminalCode>Depot</subTerminalCode>\n         <carrierCode>CARRIER1</carrierCode>\n         <AllRequestDetail>\n            <RequestDetail>\n               <quantity>1</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			     <isoType>2200</isoType>\n				 <consignee>ABCNE</consignee>\n               <releaseRequestNumber>JLG45052ROAD001</releaseRequestNumber>\n               <id>JLG45052A01</id>\n               <requestDetailID>001</requestDetailID>\n            </RequestDetail>\n            <RequestDetail>\n               <quantity>1</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			     <isoType>2200</isoType>\n				  <consignee>CCS</consignee>\n               <releaseRequestNumber>JLG45052ROAD001</releaseRequestNumber>\n               <id>JLG45052A02</id>\n               <requestDetailID>002</requestDetailID>\n            </RequestDetail>\n			<RequestDetail>\n               <quantity>1</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			   <isoType>2200</isoType>\n			    <consignee>CSKO</consignee>\n               <releaseRequestNumber>JLG45052ROAD001</releaseRequestNumber>\n               <id>JLG45052A03</id>\n               <requestDetailID>002</requestDetailID>\n            </RequestDetail>\n         </AllRequestDetail>\n      </RequestHeader>\n   </AllRequestHeader>\n</JMTInternalRequestMultiLine>\n\n\n");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
