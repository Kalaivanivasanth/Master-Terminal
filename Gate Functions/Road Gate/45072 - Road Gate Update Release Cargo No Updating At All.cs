using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using System.Diagnostics;
using System.Drawing;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45072 : MTNBase
    {
        TerminalConfigForm _terminalConfigForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup()
        {
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            _terminalConfigForm = new TerminalConfigForm();
            _terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Road Gate Update Released Cargo", @"Update With Default Values - Single",
                rowDataType: EditRowDataType.ComboBox, doReverse: true);
            _terminalConfigForm.CloseForm();

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();

            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            _terminalConfigForm = new TerminalConfigForm();
            _terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Road Gate Update Released Cargo",  @"No Updating At All", 
                rowDataType: EditRowDataType.ComboBox, doReverse: true);
            _terminalConfigForm.CloseForm();
        }


        [TestMethod]
        public void RoadGateUpdateReleasedCargoNoUpdatingAtAll()
        {
            MTNInitialize();
            
            string[] consignees = 
            {
                @"ABCNE",
                @"CCS",
                @"CSKO"
            };
            
            // Step 7 - 8
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG45072");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, waitTime: 200, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"Location ID^MKSS01 020002~ID^JLG45072A01~Consignee^" + consignees[0]);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"Location ID^MKSS01 020003~ID^JLG45072A02~Consignee^" + consignees[1]);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"Location ID^MKSS01 020004~ID^JLG45072A03~Consignee^" + consignees[2]);
            cargoEnquiryForm.tblData2.FindClickRow([
                "Location ID^MKSS01 020002~ID^JLG45072A01~Consignee^" + consignees[0],
                "Location ID^MKSS01 020003~ID^JLG45072A02~Consignee^" + consignees[1],
                "Location ID^MKSS01 020004~ID^JLG45072A03~Consignee^" + consignees[2]
            ]);                
            // Step 12
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"45072");

            // Step 13
            //MTNControlBase.SetValue(roadGateForm.txtCarrier, @"American Auto Tpt");
            //roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            //MTNControlBase.SetValue(roadGateForm.txtRegistration, @"45072A");
            //roadGateForm.txtRegistration.SetValue("45072A");
            //MTNControlBase.SetValue(roadGateForm.cmbGate, @"GATE");
            //roadGateForm.cmbGate.SetValue(@"GATE");
            //MTNControlBase.SetValue(roadGateForm.txtNewItem, @"JLG45072ROAD001");
            roadGateForm.SetRegoCarrierGate("45072A");
            roadGateForm.txtNewItem.SetValue(@"JLG45072ROAD001", 200);
            // Step 14
            RoadGatePickerForm picker = new RoadGatePickerForm(formTitle: @"Picker");
            picker.btnOK.DoClick();
            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm("Release Full Container  TT1");
            
            // Step 15 - 16
            roadGateDetailsReleaseForm.TxtCargoId.Click();
            Keyboard.Press(VirtualKeyShort.TAB);
            roadGateDetailsReleaseForm.GetReleaseByIdCargoTable();
            CheckCargoTableForTestCase45072(roadGateDetailsReleaseForm.tblReleaseByIDCargo, roadGateDetailsReleaseForm.TxtCargoId.GetElement());
            // Step 17
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            // Step 18
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            string[] warningErrorToCheck =
             {
                   "Code :79688. Cargo JLG45072A02's availability status of Unavailable for Release does not match the request's availability status of Available for release",
                   "Code :79688. Cargo JLG45072A03's availability status of Unavailable for Release does not match the request's availability status of Available for release",
                   "Code :79688. Cargo JLG45072A01's availability status of Unavailable for Release does not match the request's availability status of Available for release"
             };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();

            // Step 19
            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Full~Detail^JLG45072A02; MSC; 2200~Booking/Release^JLG45072ROAD001");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Full~Detail^JLG45072A03; MSC; 2200~Booking/Release^JLG45072ROAD001");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Full~Detail^JLG45072A01; MSC; 2200~Booking/Release^JLG45072ROAD001");
            roadGateForm.TblGateItems.FindClickRow([
                "Type^Release Full~Detail^JLG45072A02; MSC; 2200~Booking/Release^JLG45072ROAD001",
                "Type^Release Full~Detail^JLG45072A03; MSC; 2200~Booking/Release^JLG45072ROAD001",
                "Type^Release Full~Detail^JLG45072A01; MSC; 2200~Booking/Release^JLG45072ROAD001"
            ]);
            roadGateForm.btnSave.DoClick();

            // Step 20 - 21
            cargoEnquiryForm.SetFocusToForm();
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"Location ID^MKSS01 020002~ID^JLG45072A01~Consignee^" + consignees[0]);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"Location ID^MKSS01 020003~ID^JLG45072A02~Consignee^" + consignees[1]);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"Location ID^MKSS01 020004~ID^JLG45072A03~Consignee^" + consignees[2]);
            cargoEnquiryForm.tblData2.FindClickRow([
                "Location ID^MKSS01 020002~ID^JLG45072A01~Consignee^" + consignees[0],
                "Location ID^MKSS01 020003~ID^JLG45072A02~Consignee^" + consignees[1],
                "Location ID^MKSS01 020004~ID^JLG45072A03~Consignee^" + consignees[2]
            ]);
            // Step 25 - 27
            /*FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^45072A~Cargo ID^JLG45072A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^45072A~Cargo ID^JLG45072A02", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^45072A~Cargo ID^JLG45072A03", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^45072A~Cargo ID^JLG45072A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");*/
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new [] { "45072A" });

        }


        public void CheckCargoTableForTestCase45072(AutomationElement table, AutomationElement textbox)
        {
            Point point = new Point(table.BoundingRectangle.X + 1, table.BoundingRectangle.Y + 1);
            Mouse.Click(point);

            int yPosition = 15;
            for (int index = 1; index <= 3; index++)
            {
                Point point2 = new Point(table.BoundingRectangle.X + 5, table.BoundingRectangle.Y + yPosition);
                Mouse.Click(point2);
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
                yPosition += 10;
            }

            point = new Point(table.BoundingRectangle.X + 5, table.BoundingRectangle.Y + 15);
            Mouse.Click(point);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            Keyboard.Press(VirtualKeyShort.TAB, 500);

            Trace.TraceInformation(@"textbox: {0}", textbox.AsTextBox().Text);
            Assert.IsTrue(textbox.AsTextBox().Text == @"JLG45072A02, JLG45072...",
                @"TestCase45072 - Selected cargo item(s) for release do not match JLG45072A02, JLG45072...");

        }
                     
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_45072_";
            
            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>45072</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45072A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 020002</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <consignee>ABCNE</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>45072</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45072A02</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 020003</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <consignee>CCS</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>45072</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45072A03</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 020004</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <consignee>CSKO</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>45072</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45072A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 020002</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <consignee>ABCNE</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>45072</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45072A02</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 020003</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <consignee>CCS</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>45072</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45072A03</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 020004</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <consignee>CSKO</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n\n\n\n");

            // Create Release Request
            CreateDataFileToLoad(@"CreateReleaseRequest.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalRequestMultiLine>\n   <AllRequestHeader>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <RequestHeader Terminal='TT1'>\n         <releaseByType>false</releaseByType>\n         <messageMode>A</messageMode>\n         <operatorCode>MSL</operatorCode>\n         <voyageCode>MSCK000002</voyageCode>\n         <releaseRequestNumber>JLG45072ROAD001</releaseRequestNumber>\n         <releaseTypeStr>Road</releaseTypeStr>\n         <statusBulkRelease>Active</statusBulkRelease>\n         <subTerminalCode>Depot</subTerminalCode>\n         <carrierCode>CARRIER1</carrierCode>\n         <AllRequestDetail>\n            <RequestDetail>\n               <quantity>1</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			     <isoType>2200</isoType>\n				 <consignee>ABCNE</consignee>\n               <releaseRequestNumber>JLG45072ROAD001</releaseRequestNumber>\n               <id>JLG45072A01</id>\n               <requestDetailID>001</requestDetailID>\n            </RequestDetail>\n            <RequestDetail>\n               <quantity>1</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			     <isoType>2200</isoType>\n				  <consignee>CCS</consignee>\n               <releaseRequestNumber>JLG45072ROAD001</releaseRequestNumber>\n               <id>JLG45072A02</id>\n               <requestDetailID>002</requestDetailID>\n            </RequestDetail>\n			<RequestDetail>\n               <quantity>1</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			   <isoType>2200</isoType>\n			    <consignee>CSKO</consignee>\n               <releaseRequestNumber>JLG45072ROAD001</releaseRequestNumber>\n               <id>JLG45072A03</id>\n               <requestDetailID>002</requestDetailID>\n			</RequestDetail>\n			\n         </AllRequestDetail>\n      </RequestHeader>\n   </AllRequestHeader>\n</JMTInternalRequestMultiLine>\n\n\n\n\n\n");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
