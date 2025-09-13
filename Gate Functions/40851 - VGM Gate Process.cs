using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using System.Xml;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class VgmGateProcess : MTNBase
    {
        ToDoTaskForm _toDoTaskForm;
        ConfirmationFormOK _confirmationFormOk;
        WeightForm_Weighbridge _weightForm;
        CargoChecksForm _cargoChecksForm;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        private void MTNInitialise()
        {
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();

            // Open Terminal Ops | Terminal Config | VGM tab
            // Click the Edit button, enter Default Weight Certifying Authority, Default Weight Certifying Person and click the Save button
            // Monday, 27 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            var terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"VGM", @"Default Weight Certifying Authority", @"TEST TEAM",
                rowDataType: EditRowDataType.ComboBox, doReverseTab: true);
            terminalConfigForm.SetTerminalConfiguration(@"VGM", @"Default Weight Certifying Person", @"TESTER",
                rowDataType: EditRowDataType.ComboBox, doReverseTab: true);
            terminalConfigForm.CloseForm();

        }


        [TestMethod]
        public void VGMGateProcess()
        {
            MTNInitialise();
            
            // Open General Functions | Cargo Enquiry 
            // Enter Cargo ID, Site / State  and click Search button :
            // Tuesday, 28 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG40851A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"Pre-Notified",
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true); //, doDownArrow: true, searchSubStringTo: "Pre-Notified".Length - 1);
            cargoEnquiryForm.DoSearch();
            // Monday, 27 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40851A01", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG40851A01" }, ClickType.ContextClick);

            // Select the cargo and from the context menu (Right - Click) select Cargo | Add Tasks...
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Add Tasks...");

            // Check the Check box for the task VGM_40851 and click the Save & Close button
            _toDoTaskForm = new ToDoTaskForm(@"JLG40851A01 TT1");
            _toDoTaskForm.AddCompleteTask(@"VGM_40851", _toDoTaskForm.btnSaveAndClose);

            // Click the OK button 
            _confirmationFormOk = new ConfirmationFormOK(@"Tasks Added");
            _confirmationFormOk.btnOK.DoClick();
            cargoEnquiryForm.CloseForm();
            
            // Open Gate Functions | Road Gate
            // Monday, 27 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();

            // Enter Registration, Carrier, Gate and click Receive Full button
            roadGateForm = new RoadGateForm(@"Road Gate TT1", vehicleId: @"40851");
            roadGateForm.SetRegoCarrierGate("40851");
            roadGateForm.btnReceiveFull.DoClick();

            // Enter ISO Type, Cargo ID and press Tab key
            var receiveFullContainerForm = new RoadGateDetailsReceiveForm(@"Receive Full Container TT1");
            receiveFullContainerForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            receiveFullContainerForm.TxtCargoId.SetValue(@"JLG40851A01");
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);

            // Click ellipses button next to Total Weight
            receiveFullContainerForm.ClickWeightButton();

            // Clear the details from Scale combo box
            _weightForm = new WeightForm_Weighbridge(@"Weight form TT1");
            _weightForm.cmbScale.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.UP);

            // Under the Manual Adjustment (+/-) enter the weight 500 lbs, click Weigh Certified and click the Update button as shown in the screenshot
            
            _weightForm.ManualWeightAdjustment();
            _weightForm.tblManualAdjustWeight.Focus();
            MTNKeyboard.Type(@"500", 30);
            _weightForm.chkWeightCertified.DoClick();
            _weightForm.GetUpdateButton();
            _weightForm.btnUpdate.DoClick();

            // Click the Save button 
            receiveFullContainerForm.BtnSave.DoClick();

            // Click the Save button on Warnings window
            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
           warningErrorForm.btnSave.DoClick();

            // Click the green tick for the Weight field
            _cargoChecksForm = new CargoChecksForm(@"Cargo Checks Form TT1");
            _cargoChecksForm.CheckWeightTable();
            _cargoChecksForm.tblWeight.Focus();
            Keyboard.Press(FlaUI.Core.WindowsAPI.VirtualKeyShort.TAB);
            Mouse.MoveTo(_cargoChecksForm.tblWeight.BoundingRectangle.Center());
            Mouse.Click();


            // Click the Complete button
            _cargoChecksForm.btnComplete.DoClick();

            // Click the Save button  
            roadGateForm.btnSave.DoClick();

            // Click the Save button  
            try
            {
                warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
                warningErrorForm.btnSave.DoClick();
                roadGateForm.CloseForm();
            }
            catch {}
            
            // Open Yard Functions | Road Operations 
            // Monday, 27 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();

            // Find the Vehicle 40851 in the Yard and from the context menu (Right - Click) select Move It | Move Selected
            // Select the vehicle 40851 right click and select Process Road Exit 
            /*// Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date 
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            MTNControlBase.FindClickRowInList(roadOperationsForm.lstVehicles, @"40851 (1/0) - ICA - Yard Interchange");

            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^40851~Cargo ID^JLG40851A01",
                ClickType.RightClick, rowHeight: 10);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^40851", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");
            roadOperationsForm.CloseForm();*/
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit($"Road Operations {terminalId}", new [] { "40851" });
            
            // Go to Cargo Enquiry screen and enter Cargo ID and click the Search button
            // Monday, 27 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer,
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG40851A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Site",
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true); //, doDownArrow: true, searchSubStringTo: "On Site".Length - 1);
            cargoEnquiryForm.DoSearch();

            // In the Status Tab check the To Do Tasks (dbl click) field is empty
            cargoEnquiryForm.GetStatusTable(@"4087");
            var fieldData = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblStatus, @"To Do Tasks (dbl click)");
            Console.WriteLine($"fieldData: {fieldData}    {fieldData.Length}");
            Assert.IsTrue(fieldData.Equals(""));


            // Click the View Transactions button in the toolbar 
            cargoEnquiryForm.DoViewTransactions();

            // Should show transactions for the cargo item JLG40851A01
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>("Transactions for JLG40851A01 TT1");
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Weighed~Details^Is Weight Certified:Yes", ClickType.Click);
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[]
                { "Type^Weighed~Details^Is Weight Certified:Yes" });

            // Click the Charges tab  
            _cargoEnquiryTransactionForm.GetChargesTab(@"Charges (1)");

            // Should show an entry under charges tab 
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblCharges,
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date     @"Debtor^Mediterranean Shipping Co~Qty^226.796~Status^Normal~Type^Invoice Line~Narration^CargoWeight~Amount^4,535.92",
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date     ClickType.None);       
            _cargoEnquiryTransactionForm.tblCharges1.FindClickRow(new[]
                { "Debtor^Mediterranean Shipping Co~Qty^226.796~Status^Normal~Type^Invoice Line~Narration^CargoWeight~Amount^4,535.92" }, ClickType.None);
        }

        public static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_40851_";

            //Create Data Files to load
            //File to Delete Cargo Onsite
            CreateDataFileToLoad(@"CargoOnSite_Del.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n   \n    <CargoOnSite Terminal='TT1'>\n      <TestCases>40851</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG40851A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>COS</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>6000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");

            //File to Add Prenote
            CreateDataFileToLoad(@"Prenote_Add.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n<AllPrenote>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n <Prenote Terminal='TT1'>\n        <CargoType>ISO Container</CargoType>\n	 <id>JLG40851A01</id>\n            <isoType>2200</isoType>\n            <Commodity>GEN</Commodity>\n            <IMEX>Export</IMEX>\n            <dischargePort>NZAKL</dischargePort>\n            <weight>9000.00</weight>\n            <VoyageCode>MSCK000002</VoyageCode>\n            <operatorCode>MSC</operatorCode>\n			<messageMode>A</messageMode>\n</Prenote>\n</AllPrenote>\n</SystemXMLPrenote>\n\n\n");
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }
}
