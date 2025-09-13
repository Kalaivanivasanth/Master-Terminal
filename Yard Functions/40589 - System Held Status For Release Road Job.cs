using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNForms.FormObjects.Yard_Functions.Cargo_Storage_Areas;
using System.Drawing;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects.Yard_Functions.Radio_Telemetry_Queue__RTQ_;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40589 : MTNBase
    {

         RoadGateDetailsReceiveForm _receiveFullContainerForm;
         RoadGateDetailsReleaseForm _roadGateDetailsReleaseForm;
         RadioTelemetryQueueForm _radioTelemetryQueueForm;
         LocationEnquiryForm _locationEnquiryForm;

        private const string TestCaseNumber = @"40589";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, $"resetData_{TestCaseNumber}");
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>("USER40589");
        }


        [TestMethod]
        public void SystemHeldStatusForReleaseRoadJob()
        {
            MTNInitialize();
            
            /* 
             * To test that 
             * 
             * when the receipt cargo on a road job is queued manually after the auto planning fails, 
             * the status of release cargo on the same road job changes from System Held to Unactioned in Radio Telemetry Queue form
            */

            // Open Road Gate
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: $@"Road Gate {terminalId}");

            roadGateForm.SetRegoCarrierGate(TestCaseNumber);

            // Receive Full Container JLG40589A01
            roadGateForm.btnReceiveFull.DoClick();

            // Enter ISO Type, Cargo ID, Commodity, Total Weight, IMEX, Voyage, Operator, Discharge Port
            _receiveFullContainerForm = new RoadGateDetailsReceiveForm($@"Receive Full Container {terminalId}");
            _receiveFullContainerForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            _receiveFullContainerForm.TxtCargoId.SetValue(@"JLG40589A01");
            _receiveFullContainerForm.CmbCommodity.SetValue(Commodity.APPL, doDownArrow: true, searchSubStringTo: 3);
            _receiveFullContainerForm.MtTotalWeight.SetValueAndType("4000");
            _receiveFullContainerForm.CmbImex.SetValue(IMEX.Export, doDownArrow: true); //, 1000);
            _receiveFullContainerForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            _receiveFullContainerForm.CmbOperator.SetValue(Operator.FLS,  doDownArrow: true);
            _receiveFullContainerForm.CmbDischargePort.SetValue(Port.BLUNZ, doDownArrow: true);
            _receiveFullContainerForm.BtnSave.DoClick();

            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out");

            // Release Full Container JLG40589A02
            roadGateForm.btnReleaseFull.DoClick();

            _roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm(formTitle: $@"Release Full Container  {terminalId}");
            _roadGateDetailsReleaseForm.TxtCargoId.SetValue(@"JLG40589A02");
            _roadGateDetailsReleaseForm.TxtDeliveryReleaseNo.SetValue(@"40589");
            _roadGateDetailsReleaseForm.BtnSave.DoClick();

            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out");

            roadGateForm.btnSave.DoClick();
            roadGateForm.CloseForm();

            // Open Road Operations form
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm();
            // Tuesday, 18 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInList(roadOperationsForm.lstVehicles, @"40589 (1/1) - ICA - Yard Interchange");

            // Check the RT column for JLG40589A01 and JLG40589A02
            /*// Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"RT^~Vehicle Id^40589~Cargo ID^JLG40589A01", rowHeight: 16);
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"RT^Q H~Vehicle Id^40589~Cargo ID^JLG40589A02", rowHeight: 16);*/
            roadOperationsForm.TblYard2.FindClickRow(new[]
                { "RT^~Vehicle Id^40589~Cargo ID^JLG40589A01", "RT^Q H~Vehicle Id^40589~Cargo ID^JLG40589A02" });

            // Open Radio Telemetry Queues
            FormObjectBase.MainForm.OpenRadioTelemetryQueuesFromToolbar();
            _radioTelemetryQueueForm = new RadioTelemetryQueueForm($@"Radio Telemetry Queue {terminalId}");

            // Find the cargo JLG40589A02 
            _radioTelemetryQueueForm.FindCargoUsingFindCargoInTable(@"JLG40589A02");

            // Check the Task Status column
            _radioTelemetryQueueForm.FindClickRowInQueuedItemsTable(@"Master Queue JLG40589A02^System Held^Pickup^MKBS01^40589(1) Road^MSCK000002", doContextMenu: false);

            roadOperationsForm.SetFocusToForm();

            Miscellaneous.ClickAtSpecifiedPoint(roadOperationsForm.moveModeOptions, ClickType.RightClick);
            roadOperationsForm.ContextMenuSelect(@"Queued Mode");

            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"RT^~Vehicle Id^40589~Cargo ID^JLG40589A01", rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "RT^~Vehicle Id^40589~Cargo ID^JLG40589A01" });

            Point startPoint = Mouse.Position;

            FormObjectBase.MainForm.OpenLocationEnquiryFromToolbar();
            _locationEnquiryForm = new LocationEnquiryForm($@"MKBS03 Location Enquiry {terminalId}");

            _locationEnquiryForm.SetFocusToForm();
            _locationEnquiryForm.DoSearch();

            Miscellaneous.ClickAtSpecifiedPoint(_locationEnquiryForm.moveModeOptions, ClickType.RightClick);
            _locationEnquiryForm.ContextMenuSelect(@"Queued Mode");

            Point endpoint = new Point(_locationEnquiryForm.TblSearchResults1.GetElement().BoundingRectangle.X + 100,
               _locationEnquiryForm.TblSearchResults1.GetElement().BoundingRectangle.Y + 150);
            Mouse.Click(endpoint);

            Mouse.Drag(startPoint, endpoint);

            WarningErrorForm.CompleteWarningErrorForm("Warnings for Move Error");

            _radioTelemetryQueueForm.SetFocusToForm();
            _radioTelemetryQueueForm.FindCargoUsingFindCargoInTable(@"JLG40589A02");
            _radioTelemetryQueueForm.FindClickRowInQueuedItemsTable(@"Master Queue JLG40589A02^Unactioned^Pickup^MKBS01^40589(1) Road^MSCK000002", doContextMenu: false);
            roadOperationsForm.SetFocusToForm();

            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^40589~Cargo ID^JLG40589A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Vehicle Id^40589~Cargo ID^JLG40589A01" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect("Move It|Move Selected");
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Operations Move");

            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^40589~Cargo ID^JLG40589A02", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Vehicle Id^40589~Cargo ID^JLG40589A02" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^40589~Cargo ID^JLG40589A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Vehicle Id^40589~Cargo ID^JLG40589A01" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");
     

        }



        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            
            searchFor = $"_{TestCaseNumber}_";

            //Delete and Create Cargo Onsite
            CreateDataFileToLoad(@"DeleteCreateCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>  \n  <JMTInternalCargoOnSite   \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>  \n  <AllCargoOnSite>  \n  <operationsToPerform>Verify;Load To DB</operationsToPerform>  \n  <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>  \n  <CargoOnSite Terminal='TT1'>  \n  <cargoTypeDescr>ISO Container</cargoTypeDescr>  \n  <id>JLG40589A02</id>  \n  <isoType>2200</isoType>  \n  <voyageCode>MSCK000002</voyageCode>  \n  <operatorCode>MSL</operatorCode>  \n  <dischargePort>NZAKL</dischargePort>  \n  <locationId>MKBS01</locationId>  \n  <weight>5101.0000</weight>  \n  <imexStatus>Import</imexStatus>  \n  <totalQuantity>1</totalQuantity>  \n  <commodity>APPL</commodity>  \n  <messageMode>A</messageMode>  \n  </CargoOnSite>  \n  <CargoOnSite Terminal='TT1'>  \n  <cargoTypeDescr>ISO Container</cargoTypeDescr>  \n  <id>JLG40589A01</id>  \n  <isoType>2200</isoType>  \n  <voyageCode>MSCK000002</voyageCode>  \n  <operatorCode>FLS</operatorCode>  \n  <dischargePort>NZAKL</dischargePort>  \n  <locationId>MKBS01</locationId>  \n  <weight>4000</weight>  \n  <imexStatus>Export</imexStatus>  \n  <totalQuantity>1</totalQuantity>  \n  <commodity>APPL</commodity>  \n  <messageMode>D</messageMode>  \n  </CargoOnSite>  \n  </AllCargoOnSite>  \n  </JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);


        }




    }

}

