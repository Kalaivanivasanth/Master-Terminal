using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40478 : MTNBase
    {
        private TerminalConfigForm _terminalConfigForm = null;
        private CargoSearchForm _cargoSearchForm;
        private RoadGateDetailsReleaseForm _roadGateDetailsReleaseForm;

        private const string TestCaseNumber = "40748";
        private const string CargoId1 = $"JLG{TestCaseNumber}A01";
        private const string CargoId2 = $"JLG{TestCaseNumber}A02";
        private const string CargoId3 = $"JLG{TestCaseNumber}A03";

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
        public void RoadGateUpdateReleasedCargoWithDefaultValuesFirst()
        {
            MTNInitialize();

            InitializeDetails();

            string[,] consignees =
           {
                {CargoId1, "ABCNE"},
                {CargoId2, "CCS"},
                {CargoId3, "CSKO"}
             };

            // Step 7 - 11
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, 
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", $"JLG{TestCaseNumber}");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", Site.OnSite);
            cargoEnquiryForm.DoSearch();
           
            string consigneeNotFound = null;
            for (var index = 0; index < consignees.GetLength(0); index++)
            {
                var found = MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData2.GetElement(),
                    $"ID^{consignees[index, 0]}~Consignee^{consignees[index, 1]}", doAssert: false);
                if (found == false)
                {
                    consigneeNotFound += $@"{Environment.NewLine}{consignees[index, 0]} \ {consignees[index, 1]}";
                }
              
            }
            Assert.IsTrue(string.IsNullOrEmpty(consigneeNotFound),
                $@"TestCase40748 - The following cargoId / consignee combinations were not found: {consigneeNotFound}");

            // Step 12 - 22
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: TestCaseNumber);
            
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);
            roadGateForm.btnReleaseFull.DoClick();

            _roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm("Release Full Container  TT1");
            _roadGateDetailsReleaseForm.GetReleaseByIdCargoTable();
            _roadGateDetailsReleaseForm.BtnCargoSearch.DoClick();

            _cargoSearchForm = new CargoSearchForm(@"Cargo Search TT1");
            _cargoSearchForm.txtCargoId.SetValue($@"JLG{TestCaseNumber}");
            _cargoSearchForm.btnSearchAll.DoClick();

            string rowNotFound = null;
            for (var index = 0; index < consignees.GetLength(0); index++)
            {
                // MTNControlBase.FindClickRowInTable(_cargoSearchForm.tblResults, $"ID^{consignees[index, 0]}");
                _cargoSearchForm.TblResults.FindClickRow([$"ID^{consignees[index, 0]}"]);                _cargoSearchForm.btnAdd.DoClick();
                var rowFound = MTNControlBase.FindClickRowInTable(_cargoSearchForm.tblSelectedItems,
                    $"ID^{consignees[index, 0]}", doAssert: false);
                if (!rowFound)
                {
                    rowNotFound += $"\r\n{consignees[index, 0]}";
                }
            }
            Assert.IsTrue(rowNotFound == null,
                $"TestCase40748 - The following cargoId were not found:\r\n{rowNotFound}");

            _cargoSearchForm.btnOK.DoClick();

            // Step 19
            _roadGateDetailsReleaseForm.SetFocusToForm();
            _roadGateDetailsReleaseForm.SelectCargo40748();

            string consigneeValue =
                _roadGateDetailsReleaseForm.CmbConsignee.GetValue()/*.Replace("\t", string.Empty).Replace(" ", string.Empty)*/.Trim();
            Console.WriteLine($"Consignee: {consigneeValue}");
            Assert.IsTrue(consigneeValue.Contains("ABCNE\tAB Consignee"),
                $"TestCase40748 - Consignee is incorrect.  Expected: ABCNE\tAB Consignee\r\nActual: {_roadGateDetailsReleaseForm.CmbConsignee.GetValue()}");

            _roadGateDetailsReleaseForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // $"Type^Release Full~Detail^{CargoId1}; MSC; 2200");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // $"Type^Release Full~Detail^{CargoId2}; MSC; 2200");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // $"Type^Release Full~Detail^{CargoId3}; MSC; 2200");
            roadGateForm.TblGateItems.FindClickRow([
                $"Type^Release Full~Detail^{CargoId1}; MSC; 2200",
                $"Type^Release Full~Detail^{CargoId2}; MSC; 2200",
                $"Type^Release Full~Detail^{CargoId3}; MSC; 2200"
            ]);
            roadGateForm.btnSave.DoClick();

            // Step 23 - 25
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(RoadOperationsForm.FormTitle, new [] {TestCaseNumber });

            // Step 25 - 31
            consignees = new [,]
            {
                {CargoId1, "ABCNE"},
                {CargoId2, "ABCNE"},
                {CargoId3, "ABCNE"},
            };

            cargoEnquiryForm.SetFocusToForm();

            consigneeNotFound = null;
            for (var index = 0; index < consignees.GetLength(0); index++)
            {
                var found = MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData2.GetElement(),
                    $"ID^{consignees[index, 0]}~Consignee^{consignees[index, 1]}", doAssert: false);
                if (found == false)
                {
                    consigneeNotFound += $"\r\n{consignees[index, 0]} \\ {consignees[index, 1]}";
                }
            }
            Assert.IsTrue(string.IsNullOrEmpty(consigneeNotFound),
                $@"TestCase40748 - The following cargoId / consignee combinations were not found: {consigneeNotFound}");

        }

        private void InitializeDetails()
        {
            //FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            _terminalConfigForm = new TerminalConfigForm();
            _terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Road Gate Update Released Cargo", @"Update With Default Values - First",
                rowDataType: EditRowDataType.ComboBox, doReverse: true);
            _terminalConfigForm.CloseForm();
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = $"_{TestCaseNumber}_";

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>40748</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG40748A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 010002</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <consignee>ABCNE</consignee>\n        <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>40748</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG40748A02</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 010003</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <consignee>CCS</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>40748</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG40748A03</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 010004</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <consignee>CSKO</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
