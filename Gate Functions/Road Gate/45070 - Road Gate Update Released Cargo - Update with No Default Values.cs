using DataObjects.LogInOutBO;
using FlaUI.Core.WindowsAPI;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45070 : MTNBase
    {
        TerminalConfigForm _terminalConfigForm;

        private const string _testNumber = @"45070";
        private const string _cargoId1 = @"JLG" + _testNumber + @"A01";
        private const string _cargoId2 = @"JLG" + _testNumber + @"A02";
        private const string _cargoId3 = @"JLG" + _testNumber + @"A03";
     

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + _testNumber + @"_";
            BaseClassInitialize_New(testContext);
        }
        
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

            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();

           // FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
           FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            _terminalConfigForm = new TerminalConfigForm();
            _terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Road Gate Update Released Cargo",
                @"Update With No Default Values", rowDataType: EditRowDataType.ComboBox, doReverse: true);
            _terminalConfigForm.CloseForm();
        }


        [TestMethod]
        public void RoadGateUpdateReleasedCargoWithNoDefaultValues()
        {
            MTNInitialize();
            
            string[,] consignees =
            {
               {_cargoId1, @""},
               {_cargoId2, @""},
               {_cargoId3, @""}
            };

            // Step 7 - 12
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG" + _testNumber);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Site"/*, EditRowDataType.ComboBoxEdit, waitTime: 200, fromCargoEnquiry: true*/);
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.CargoEnquiryReceiveRelease(@"4085");

            string consigneeNotFound = null;
            for (var index = 0; index < consignees.GetLength(0); index++)

            {
                // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + consignees[index, 0]);
                cargoEnquiryForm.tblData2.FindClickRow(["ID^" + consignees[index, 0]]);                var consignee = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblReceiveRelease, @"Consignee");
                if (consignee != consignees[index, 1])
                {
                    consigneeNotFound += @"\n" + consignees[index, 0] + @" \ " + consignees[index, 1];
                }
            }
            Assert.IsTrue(consigneeNotFound == null,
                @"TestCase45070 - The following cargoId / consignee combinations were not found: " + "\n" + consigneeNotFound);

            // Step 7 - 22
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"45070");

            roadGateForm.SetRegoCarrierGate("45070");
            roadGateForm.txtNewItem.SetValue(@"JLG45070ROAD001");
            Keyboard.Press(VirtualKeyShort.TAB);

            RoadGatePickerForm roadGatePickerForm = new RoadGatePickerForm(formTitle: @"Picker");
            // MTNControlBase.FindClickRowInTable(roadGatePickerForm.tblPickerItems,
                // @"Cargo Id^" + _cargoId1 + "~Type^Release~Item Id^JLG45070ROAD001", rowHeight: 16);
            roadGatePickerForm.TblPickerItems.FindClickRow(["Cargo Id^" + _cargoId1 + "~Type^Release~Item Id^JLG45070ROAD001"]);            roadGatePickerForm.btnOK.DoClick();

            // Step 16 - 18
            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm("Release Full Container  TT1");
            roadGateDetailsReleaseForm.SelectCargo45070();
            //MTNControlBase.SendTextToCombobox(roadGateDetailsReleaseForm.cmbConsignee, @"FFWHT");
            roadGateDetailsReleaseForm.CmbConsignee.SetValue(ConsigneeConsignorFreightF.FFWHT, doDownArrow: true);
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, 
                // @"Type^Release Full~Detail^" + consignees[0, 0] + "; MSC; 2200~Booking/Release^JLG45070ROAD001");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // @"Type^Release Full~Detail^" + consignees[1, 0] + "; MSC; 2200~Booking/Release^JLG45070ROAD001");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // @"Type^Release Full~Detail^" + consignees[2, 0] + "; MSC; 2200~Booking/Release^JLG45070ROAD001");
            roadGateForm.TblGateItems.FindClickRow([
                "Type^Release Full~Detail^" + consignees[0, 0] + "; MSC; 2200~Booking/Release^JLG45070ROAD001",
                "Type^Release Full~Detail^" + consignees[1, 0] + "; MSC; 2200~Booking/Release^JLG45070ROAD001",
                "Type^Release Full~Detail^" + consignees[2, 0] + "; MSC; 2200~Booking/Release^JLG45070ROAD001"
            ]);
            roadGateForm.btnSave.DoClick();

            // Step 23 - 25
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(@"Road Operations TT1", new[] { _testNumber });

            // Step 25 - 31
            consignees = new string[,]
            {
                {_cargoId1, @"FFWHT"},
                {_cargoId2, @"FFWHT"},
                {_cargoId3, @"FFWHT"}
            };

            cargoEnquiryForm.SetFocusToForm();
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Receive/Release");
            //cargoEnquiryForm.GetGenericTabTableDetails(@"Receive/Release", @"4085");
            cargoEnquiryForm.CargoEnquiryReceiveRelease(@"4085");

            consigneeNotFound = null;
            for (var index = 0; index < consignees.GetLength(0); index++)

            {
                // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + consignees[index, 0]);
                cargoEnquiryForm.tblData2.FindClickRow(["ID^" + consignees[index, 0]]);                var consignee = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblReceiveRelease, @"Consignee");
                if (consignee != consignees[index, 1])
                {
                    consigneeNotFound += "\n" + consignees[index, 0] + @" \ " + consignees[index, 1];
                }
            }
            Assert.IsTrue(consigneeNotFound == null,
                @"TestCase45070 - The following cargoId / consignee combinations were not found: " + "\n" + consigneeNotFound);

            // Step 32 - 34
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Release Requests", forceReset: true);
            ReleaseRequestForm releaseRequestForm = new ReleaseRequestForm(@"Release Requests TT1");
            releaseRequestForm.DeleteReleaseRequests("JLG45070ROAD001", @"Release No^JLG45070ROAD001", @"Completed");

        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            
            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>45070</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45070A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 040002</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>45070</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45070A02</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 040003</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>45070</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45070A03</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 040004</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Release Request
            CreateDataFileToLoad(@"CreateReleaseRequest.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalRequestMultiLine>\n   <AllRequestHeader>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <RequestHeader Terminal='TT1'>\n         <releaseByType>false</releaseByType>\n         <messageMode>A</messageMode>\n         <operatorCode>MSL</operatorCode>\n         <voyageCode>MSCK000002</voyageCode>\n         <releaseRequestNumber>JLG45070ROAD001</releaseRequestNumber>\n         <releaseTypeStr>Road</releaseTypeStr>\n         <statusBulkRelease>Active</statusBulkRelease>\n         <subTerminalCode>Depot</subTerminalCode>\n         <carrierCode>CARRIER1</carrierCode>\n         <AllRequestDetail>\n            <RequestDetail>\n               <quantity>1</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			     <isoType>2200</isoType>\n			\n               <releaseRequestNumber>JLG45070ROAD001</releaseRequestNumber>\n               <id>JLG45070A01</id>\n               <requestDetailID>001</requestDetailID>\n            </RequestDetail>\n            <RequestDetail>\n               <quantity>1</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			     <isoType>2200</isoType>\n			\n               <releaseRequestNumber>JLG45070ROAD001</releaseRequestNumber>\n               <id>JLG45070A02</id>\n               <requestDetailID>002</requestDetailID>\n            </RequestDetail>\n			<RequestDetail>\n               <quantity>1</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			   <isoType>2200</isoType>\n			\n               <releaseRequestNumber>JLG45070ROAD001</releaseRequestNumber>\n               <id>JLG45070A03</id>\n               <requestDetailID>002</requestDetailID>\n			</RequestDetail>\n			         </AllRequestDetail>\n      </RequestHeader>\n   </AllRequestHeader>\n</JMTInternalRequestMultiLine>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
