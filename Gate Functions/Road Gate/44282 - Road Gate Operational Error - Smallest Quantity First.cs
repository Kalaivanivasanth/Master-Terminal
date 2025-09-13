using FlaUI.Core.AutomationElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Gate_Functions.Vehicle;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Print_Preview;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNWindowDialogs.WindowsDialog;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44282 : MTNBase
    {

        GateConfigurationForm _gateConfigurationForm = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
       
        [TestInitialize]
        public new void TestInitialize() {}
      
        [TestCleanup]
        public new void TestCleanup()
        {
            
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Gate Configuration", forceReset: true);
            _gateConfigurationForm = new GateConfigurationForm(@"Gate Configuration " + terminalId);
            _gateConfigurationForm.chkSiteHasSecurityGate.DoClick();

            _gateConfigurationForm.GetApplicationTabDetails();
            _gateConfigurationForm.chkAutoCompleteIsDefault.DoClick(false);
            _gateConfigurationForm.chkAllowAutoComplete.DoClick(false);

            _gateConfigurationForm.GetPrintingTabDetails();
            _gateConfigurationForm.chkPrintPreview.DoClick();
            _gateConfigurationForm.btnSave.DoClick();

            base.TestCleanup();
        }


        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();

            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Gate Configuration");
            _gateConfigurationForm = new GateConfigurationForm(@"Gate Configuration " + terminalId);
            _gateConfigurationForm.chkSiteHasSecurityGate.DoClick(false);

            _gateConfigurationForm.GetApplicationTabDetails();
            _gateConfigurationForm.chkAllowAutoComplete.DoClick();
            _gateConfigurationForm.chkAutoCompleteIsDefault.DoClick();

            _gateConfigurationForm.GetPrintingTabDetails();
            _gateConfigurationForm.btnSave.DoClick();

            SetupAndLoadInitializeData(TestContext);
            
        }


        [TestMethod]
        public void RoadGateOperationalErrorSmallestQuantityFirst()
        {
            MTNInitialize();
            
            var startDate = DateTime.Now;

            // Step 8
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Steel, EditRowDataType.ComboBoxEdit, waitTime: 100, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"MXN44282A01, MXN44282B01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^MXN44282A01~Total Quantity^7~Location ID^MKBS01~Voyage^MSCK000002", $"ID^MXN44282B01~Total Quantity^12~Location ID^MKBS01~Voyage^MSCK000002" });

            // Step 10
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: $"Road Gate {terminalId}");

            // Step 11
            roadGateForm.txtRegistration.SetValue(@"44282");
            roadGateForm.cmbCarrier.SetValue(Carrier.CARRIER1);
            roadGateForm.txtDriverCode.SetValue(@"44282_DRIVER");
            roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.chkAutoComplete.DoClick();
            roadGateForm.chkReceiptRequired.DoClick();
            roadGateForm.btnReleaseCargo.DoClick();

            // Step 12 - 13
            var roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm();
            roadGateDetailsReleaseForm.CmbCargoType.SetValue(CargoType.Steel, doDownArrow: true, searchSubStringTo: CargoType.Steel.Length - 1);
            roadGateDetailsReleaseForm.TxtCargoId.SetValue(@"MXN44282A01");
            roadGateDetailsReleaseForm.BtnSaveNext.DoClick();

            // Step 14 - 15
            roadGateDetailsReleaseForm.CmbCargoType.SetValue(CargoType.Steel, doDownArrow: true, searchSubStringTo: CargoType.Steel.Length - 1);
            roadGateDetailsReleaseForm.TxtCargoId.SetValue(@"MXN44282B01");
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release General Cargo~Detail^MXN44282A01(7); MSL; STEEL; Pipes", ClickType.None);
            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release General Cargo~Detail^MXN44282B01(12); MSL; STEEL; Pipes", ClickType.None);
            roadGateForm.TblGateItems.FindClickRow(new[]
            {
                "Type^Release General Cargo~Detail^MXN44282A01(7); MSL; STEEL; Pipes",
                "Type^Release General Cargo~Detail^MXN44282B01(12); MSL; STEEL; Pipes"
            });            

            // Step 16
            var visitNumber = roadGateForm.txtVisitNumber.GetText();
            roadGateForm.btnSave.DoClick();

            var printPreviewForm = new PrintPreviewForm(@"Print Preview - Road Receipt");
            printPreviewForm.DoPrintToPrinter(needToDoPrinterSetup: false);

            var fileName = saveDirectory + "44282_RoadGate_Save.pdf";
            Miscellaneous.DeleteFile(fileName);

            // Save the print output
            /*WindowsSaveDialog windowsSaveDialog = new WindowsSaveDialog("Save Print Output As");
            windowsSaveDialog.txtFileName.SetValue(fileName);
            windowsSaveDialog.btnSave.DoClick();*/
            WindowsSaveDialog.DoWindowsSaveDialog(WindowsSaveDialog.FormTitleSavePrintOutAs, fileName);

            MTNPDF.GetTextFromDocument(fileName);

            string[] stringsToSearchFor = 
            {
                "ROADEXCHANGERECEIPT",
                "ReleasedSTEELMXN44282A01STOBBLK50,000MSL",
                "ReleasedSTEELMXN44282B01STOBBLK50,000MSL"
                // @"Released STEEL MXN44282A01 STO BBLK 50,000 MSL GENL MSCK000002 MSCKATYAR.",
                // @"Released STEEL MXN44282B01 STO BBLK 50,000 MSL GENL MSCK000002 MSCKATYAR."
            };
            MTNPDF.FindText(stringsToSearchFor);

            // Step 17
            FormObjectBase.MainForm.OpenVehicleVisitEnquiryFromToolbar();
            var vehicleVisitForm = new VehicleVisitForm($"Vehicle Visit Enquiry {TestRunDO.GetInstance().TerminalId}");

            // Step 18
            var endDate = DateTime.Now;
            MTNControlBase.SetValueInEditTable(vehicleVisitForm.TblSearchCriteria, @"Date From", startDate.ToString(@"ddMMyyyy"));
            MTNControlBase.SetValueInEditTable(vehicleVisitForm.TblSearchCriteria, @"Date To", endDate.ToString(@"ddMMyyyy"));
            MTNControlBase.SetValueInEditTable(vehicleVisitForm.TblSearchCriteria, @"Reg Number", @"44282");
            vehicleVisitForm.DoSearch();

            // Monday, 3 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(vehicleVisitForm.tblVisits, @"Vehicle^44282~Cargo Out^19~Carrier Code^CARRIER1");
            vehicleVisitForm.TblVisits.FindClickRow(new[] { "Vehicle^44282~Cargo Out^19~Carrier Code^CARRIER1" });

            CheckPlacementOrder(vehicleVisitForm.tblDetails);
        }

        private void CheckPlacementOrder(AutomationElement table)
        {
            // get the 2 rows in the table
            List<DataGridViewRow> rowsInTable =
                new List<DataGridViewRow>
                (from DataGridViewRow r in table.AsDataGridView().Rows
                    where r.Cells.Any() && r.Name.Contains(@"Cargo Placed") || r.Name.Contains(@"STEEL Placed")
                select r);

            Assert.IsTrue(rowsInTable.Count() == 2, @"TestCase44282 - Couldn't find 2 STEEL Placed lines");

            if (!rowsInTable[0].Name.Contains(@"STEEL Placed (7xMXN44282A01)") && !rowsInTable[0].Name.Contains(@"Cargo Placed (7xMXN44282A01)"))
            {
                Assert.IsTrue(false, @"TestCase44282 - STEEL Placed (7xMXN44282A01) is not the 1st line");
            }

            if (!rowsInTable[1].Name.Contains(@"STEEL Placed (12xMXN44282B01)") && !rowsInTable[1].Name.Contains(@"Cargo Placed (12xMXN44282B01)"))
            {
                Assert.IsTrue(false, @"TestCase44282 - STEEL Placed (12xMXN44282B01) is not the 2nd line");
            }

        }



        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_45052_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44282</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>pipe</product>\n      <id>MXN44282A01</id>\n      <operatorCode>MSL</operatorCode>\n	  <totalQuantity>7</totalQuantity>\n      <locationId>MKBS01</locationId>\n      <weight>50000.0000</weight>\n      <imexStatus>Storage</imexStatus>\n        <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n	<CargoOnSite Terminal='TT1'>\n      <TestCases>44282</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>pipe</product>\n      <id>MXN44282B01</id>\n      <operatorCode>MSL</operatorCode>\n	  <totalQuantity>12</totalQuantity>\n      <locationId>MKBS01</locationId>\n      <weight>50000.0000</weight>\n      <imexStatus>Storage</imexStatus>\n          <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44282</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>pipe</product>\n      <id>MXN44282A01</id>\n      <operatorCode>MSL</operatorCode>\n	  <totalQuantity>7</totalQuantity>\n      <locationId>MKBS01</locationId>\n      <weight>50000.0000</weight>\n      <imexStatus>Storage</imexStatus>\n       <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	<CargoOnSite Terminal='TT1'>\n      <TestCases>44282</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>pipe</product>\n      <id>MXN44282B01</id>\n      <operatorCode>MSL</operatorCode>\n	  <totalQuantity>12</totalQuantity>\n      <locationId>MKBS01</locationId>\n      <weight>50000.0000</weight>\n      <imexStatus>Storage</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }


    }

}
