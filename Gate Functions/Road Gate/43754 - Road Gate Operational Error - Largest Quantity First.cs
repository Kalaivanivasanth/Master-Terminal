using FlaUI.Core.AutomationElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Gate_Functions.Vehicle;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System.Collections.Generic;
using System.Linq;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43754 : MTNBase
    {

        private GateConfigurationForm _gateConfigurationForm;
        private VehicleVisitForm _vehicleVisitForm;
        private RoadGateDetailsReleaseForm _releaseCargoForm;
       
        private const string TestCaseNumber = @"43754";
        private static readonly string[] CargoId = 
        {
            @"JLG" + TestCaseNumber + @"A01",
            @"JLG" + TestCaseNumber + @"B01"
        };

        private static readonly string Mark = TestCaseNumber + millisecondsSince20000101.ToString();


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
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
            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();
            
            //////CallJadeScriptToRun(TestContext, $"resetData_{TestCaseNumber}");

            //////base.TestInitialize();

            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Gate Configuration");
            _gateConfigurationForm = new GateConfigurationForm(@"Gate Configuration " + terminalId);
            _gateConfigurationForm.GetGateTabDetails();
            _gateConfigurationForm.chkSiteHasSecurityGate.DoClick(false);

            _gateConfigurationForm.GetApplicationTabDetails();
            _gateConfigurationForm.chkAllowAutoComplete.DoClick();
            _gateConfigurationForm.chkAutoCompleteIsDefault.DoClick();

            _gateConfigurationForm.GetPrintingTabDetails();
            _gateConfigurationForm.chkPrintPreview.DoClick(false);
            _gateConfigurationForm.btnSave.DoClick();
        }


        [TestMethod]
        public void RoadGateLargestQuantityFirst()
        {
            
            MTNInitialize();
            
            // Step 6 - 7
            SetupAndLoadInitializeData(TestContext);

            // Step 8 - 9
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Steel, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId[0] + "," + CargoId[1]);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Mark", Mark);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId[0] + @"~Total Quantity^12");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId[1] + @"~Total Quantity^7");
            cargoEnquiryForm.tblData2.FindClickRow([
                "ID^" + CargoId[0] + @"~Total Quantity^12",
                "ID^" + CargoId[1] + @"~Total Quantity^7"
            ]);
            // Step  10 - 17
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: TestCaseNumber);
            //MTNControlBase.SetValue(roadGateForm.txtRegistration, TestCaseNumber);
            roadGateForm.txtRegistration.SetValue(TestCaseNumber);
            //MTNControlBase.SetValue(roadGateForm.txtDriverCode, @"TEST");
            roadGateForm.txtDriverCode.SetValue(@"43754_DRIVER");
            //MTNControlBase.SetValue(roadGateForm.cmbCarrier, @"CARRIER1");
            roadGateForm.cmbCarrier.SetValue(Carrier.CARRIER1);
            //MTNControlBase.SetValue(roadGateForm.cmbGate, @"GATE");
            roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.chkAutoComplete.DoClick();
            roadGateForm.chkReceiptRequired.DoClick(false);
            roadGateForm.btnReleaseCargo.DoClick();

            _releaseCargoForm = new RoadGateDetailsReleaseForm(@"Release General Cargo  TT1");
            //_releaseCargoForm.GetCargoTypeComboDetails();
            //_releaseCargoForm.GetTotalQuantityDetails();
            //MTNControlBase.SetValue(_releaseCargoForm.cmbCargoType, @"STEEL");
            _releaseCargoForm.CmbCargoType.SetValue(CargoType.Steel, doDownArrow: true, searchSubStringTo: CargoType.Steel.Length - 1);
            //MTNControlBase.SetValue(_releaseCargoForm.txtCargoId, CargoId[0]);
            _releaseCargoForm.TxtCargoId.SetValue(CargoId[0]);
            _releaseCargoForm.BtnSaveNext.DoClick();

            //MTNControlBase.SetValue(_releaseCargoForm.cmbCargoType, @"STEEL");
            _releaseCargoForm.CmbCargoType.SetValue(CargoType.Steel, doDownArrow: true, searchSubStringTo: CargoType.Steel.Length - 1);
            //MTNControlBase.SetValue(_releaseCargoForm.txtCargoId, CargoId[1]);
            _releaseCargoForm.TxtCargoId.SetValue(CargoId[1]);
            _releaseCargoForm.BtnSave.DoClick();

            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // @"Type^Release General Cargo~Detail^" + CargoId[0] + @"(12); MSL; STEEL; Pipes");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // @"Type^Release General Cargo~Detail^" + CargoId[1] + @"(7); MSL; STEEL; Pipes");
            roadGateForm.TblGateItems.FindClickRow([
                "Type^Release General Cargo~Detail^" + CargoId[0] + @"(12); MSL; STEEL; Pipes",
                "Type^Release General Cargo~Detail^" + CargoId[1] + @"(7); MSL; STEEL; Pipes"
            ]); 
            roadGateForm.btnSave.DoClick();



            // Step 18 - 19
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Vehicle Visit Enquiry", forceReset: true);
            _vehicleVisitForm = new VehicleVisitForm(@"Vehicle Visit Enquiry TT1");
            MTNControlBase.SetValueInEditTable(_vehicleVisitForm.tblSearchCriteria, @"Date From",
                loadFileDeleteStartTime.ToString(@"ddMMyyyy"));
            MTNControlBase.SetValueInEditTable(_vehicleVisitForm.tblSearchCriteria, @"Time From",
                loadFileDeleteStartTime.ToString(@"HHmm"));
            MTNControlBase.SetValueInEditTable(_vehicleVisitForm.tblSearchCriteria, @"Reg Number", TestCaseNumber);
            //_vehicleVisitForm.btnSearch.DoClick();
            _vehicleVisitForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_vehicleVisitForm.tblVisits, @"Vehicle^" + TestCaseNumber + @"~Cargo Out^19");
            _vehicleVisitForm.TblVisits.FindClickRow(["Vehicle^" + TestCaseNumber + @"~Cargo Out^19"]);
            CheckPlacementOrder(_vehicleVisitForm.tblDetails);

            // Step 21 - Resetting of Gate Configuration is done in resetConfig

        }

        private void CheckPlacementOrder(AutomationElement table)
        {
            // get the 2 rows in the table
            List<DataGridViewRow> rowsInTable =
                new List<DataGridViewRow>
                (from DataGridViewRow r in table.AsDataGridView().Rows
                    where r.Cells.Any() && r.Name.Contains(@"Cargo Placed") || r.Name.Contains(@"STEEL Placed")
                    select r);

            Assert.IsTrue(rowsInTable.Count() == 2, @"TestCase43754 - Couldn't find 2 STEEL Placed lines");

            if (!rowsInTable[0].Name.Contains(@"STEEL Placed (12x" + CargoId[0] + ")") && !rowsInTable[0].Name.Contains(@"Cargo Placed (12x" + CargoId[0] + ")"))
            {
                Assert.IsTrue(false, @"TestCase43754 - STEEL Placed (12x" + CargoId[0] + ") is not the 1st line");
            }

            if (!rowsInTable[1].Name.Contains(@"STEEL Placed (7x" + CargoId[1] + ")") && !rowsInTable[1].Name.Contains(@"Cargo Placed (7x" + CargoId[1] + ")"))
            {
                Assert.IsTrue(false, @"TestCase43754 - STEEL Placed (7x" + CargoId[1] + ") is not the 2nd line");
            }

        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
             fileOrder = 1;

             // Cargo on Site Delete
             CreateDataFileToLoad(@"DeleteOnSiteCargo.xml",
                 "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>43754</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>pipe</product>\n      <id>JLG43754A01</id>\n      <operatorCode>MSL</operatorCode>\n	  <totalQuantity>12</totalQuantity>\n      <locationId>MKBS01</locationId>\n      <weight>50000.0000</weight>\n      <imexStatus>Storage</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n	<CargoOnSite Terminal='TT1'>\n      <TestCases>43754</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>pipe</product>\n      <id>JLG43754B01</id>\n      <operatorCode>MSL</operatorCode>\n	  <totalQuantity>7</totalQuantity>\n      <locationId>MKBS01</locationId>\n      <weight>50000.0000</weight>\n      <imexStatus>Storage</imexStatus>\n        <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>43754</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>pipe</product>\n      <id>JLG43754A01</id>\n      <operatorCode>MSL</operatorCode>\n	  <totalQuantity>12</totalQuantity>\n      <locationId>MKBS01</locationId>\n      <weight>50000.0000</weight>\n      <mark>" + Mark + "</mark>\n      <imexStatus>Storage</imexStatus>\n          <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	<CargoOnSite Terminal='TT1'>\n      <TestCases>43754</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>pipe</product>\n      <id>JLG43754B01</id>\n      <operatorCode>MSL</operatorCode>\n	  <totalQuantity>7</totalQuantity>\n      <locationId>MKBS01</locationId>\n      <weight>50000.0000</weight>\n      <mark>" + Mark + "</mark>\n      <imexStatus>Storage</imexStatus>\n        <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
