using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using CargoSearchForm = MTNForms.FormObjects.Gate_Functions.Road_Gate.CargoSearchForm;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45069 : MTNBase
    {
        TerminalConfigForm _terminalConfigForm = null;

        private const string _testNumber = @"45069";
        private const string _cargoId1 = @"JLG" + _testNumber + @"A01";
        private const string _cargoId2 = @"JLG" + _testNumber + @"A02";
        private const string _cargoId3 = @"JLG" + _testNumber + @"A03";
        private const string _cargoId4 = @"JLG" + _testNumber + @"A04";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            searchFor = @"_" + _testNumber + @"_";
            
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();

            //FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            _terminalConfigForm = new TerminalConfigForm();
            _terminalConfigForm.SetTerminalConfiguration(@"Gate", @"Road Gate Update Released Cargo",
                @"Update With Default Values - Most", rowDataType: EditRowDataType.ComboBox, doReverse: true);
            _terminalConfigForm.CloseForm();
        }


        [TestMethod]
        public void RoadGateUpdateReleasedCargoWithDefaultValuesMost()
        {
            MTNInitialize();
            
            string[,] consignees =
            {
                {_cargoId1, @"ABCNE"},
                {_cargoId2, @"CCS"},
                {_cargoId3, @"CCS"},
                {_cargoId4, @""}
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
                @"TestCase45069 - The following cargoId / consignee combinations were not found: " + @"\n" + consigneeNotFound);

            // Step 7 - 22
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"45069");

            roadGateForm.SetRegoCarrierGate("45069");
            roadGateForm.btnReleaseFull.DoClick();

            // Step 16 - 18
            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm("Release Full Container  TT1");
            roadGateDetailsReleaseForm.GetReleaseByIdCargoTable();
            roadGateDetailsReleaseForm.BtnCargoSearch.DoClick();

            CargoSearchForm cargoSearchForm = new CargoSearchForm(@"Cargo Search TT1");
            //MTNControlBase.SetValue(cargoSearchForm.txtCargoId, @"JLG" + _testNumber);
            cargoSearchForm.txtCargoId.SetValue(@"JLG" + _testNumber);
            cargoSearchForm.btnSearchAll.DoClick();

            string rowNotFound = null;
            for (var index = 0; index < consignees.GetLength(0); index++)
            {
                // MTNControlBase.FindClickRowInTable(cargoSearchForm.tblResults, @"ID^" + consignees[index, 0]);
                cargoSearchForm.TblResults.FindClickRow(["ID^" + consignees[index, 0]]);                cargoSearchForm.btnAdd.DoClick();
                var rowFound = MTNControlBase.FindClickRowInTable(cargoSearchForm.tblSelectedItems, @"ID^" + consignees[index, 0],
                    doAssert: false);
                if (!rowFound)
                {
                    rowNotFound += @"\n" + consignees[index, 0];
                }
            }
            Assert.IsTrue(rowNotFound == null,
                @"TestCase45069 - The following cargoId were not found: " + @"\n" + rowNotFound);

            cargoSearchForm.btnOK.DoClick();

            // Step 19
            roadGateDetailsReleaseForm.SetFocusToForm();
            roadGateDetailsReleaseForm.SelectCargo45069();

            Assert.IsTrue(
                roadGateDetailsReleaseForm.CmbConsignee.GetValue().Contains(@"CCS	Christchurch Cool Stores Lim"),
                @"TestCase45069 - Consignee is incorrect.  Expected: CCS	Christchurch Cool Stores Lim" + "\r\n" +
                @"Actual: " + roadGateDetailsReleaseForm.CmbConsignee.GetValue());
            
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // @"Type^Release Full~Detail^" + consignees[0, 0] + "; MSC; 2200");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // @"Type^Release Full~Detail^" + consignees[1, 0] + "; MSC; 2200");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // @"Type^Release Full~Detail^" + consignees[2, 0] + "; MSC; 2200");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                // @"Type^Release Full~Detail^" + consignees[3, 0] + "; MSC; 2200");
            roadGateForm.TblGateItems.FindClickRow([
                "Type^Release Full~Detail^" + consignees[0, 0] + "; MSC; 2200",
                "Type^Release Full~Detail^" + consignees[1, 0] + "; MSC; 2200",
                "Type^Release Full~Detail^" + consignees[2, 0] + "; MSC; 2200",
                "Type^Release Full~Detail^" + consignees[3, 0] + "; MSC; 2200"
            ]);
            roadGateForm.btnSave.DoClick();

            // Step 23 - 25
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(@"Road Operations TT1", new[] { _testNumber });

            // Step 25 - 31
            consignees = new string[,]
            {
                {_cargoId1, @"CCS"},
                {_cargoId2, @"CCS"},
                {_cargoId3, @"CCS"},
                {_cargoId4, @"CCS"}
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
                    consigneeNotFound += @"\n" + consignees[index, 0] + @" \ " + consignees[index, 1];
                }
            }
            Assert.IsTrue(consigneeNotFound == null,
                @"TestCase45069 - The following cargoId / consignee combinations were not found: " + @"\n" + consigneeNotFound);


        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>45069</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45069A01</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 030002</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <consignee>ABCNE</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>45069</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45069A02</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 030003</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <consignee>CCS</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	   <CargoOnSite Terminal='TT1'>\n      <TestCases>45069</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45069A03</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 030004</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	    <consignee>CCS</consignee>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n	 <CargoOnSite Terminal='TT1'>\n      <TestCases>45069</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <isoType>2200</isoType>\n      <id>JLG45069A04</id>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKSS01 030005</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Import</imexStatus>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <totalQuantity>1</totalQuantity>\n      <commodity>GEN</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");
    
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
