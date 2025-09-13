using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Quick_Find;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase37171 : MTNBase
    {
        // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date string[] _warningErrorToCheck = null;
        static string _mark;

        RoadGateDetailsReleaseForm _roadGateDetailsReleaseForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            _mark = millisecondsSince20000101.ToString();
            CallJadeScriptToRun(TestContext, "resetData_37171");
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void GateOutToTruckBasic()
            {
            
            MTNInitialize();

            // Step 4 - 5
            Keyboard.TypeSimultaneously(VirtualKeyShort.F10);
            var quickFindForm = new QuickFindForm();
            Keyboard.Type(@"F GATE", 100);
            quickFindForm.btnFind.DoClick();
            quickFindForm.CloseForm();

            roadGateForm = new RoadGateForm(formTitle: $"Road Gate {terminalId}");
            roadGateForm.SetRegoCarrierGate("37171");
            roadGateForm.btnReleaseEmpty.DoClick();

            _roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm($"Release Empty Container  {terminalId}");

            // Step 8 - 10
            _roadGateDetailsReleaseForm.TxtCargoId.SetValue("JLG37171A01");
            Keyboard.Press(VirtualKeyShort.TAB);

            Assert.IsTrue(_roadGateDetailsReleaseForm.TxtCargoId.GetText().Equals(@"JLG37171A01"),
                @"TestCase37171 - Cargo ID - Actual: " + _roadGateDetailsReleaseForm.TxtCargoId.GetText() +
                " doesn't match Expected: JLG37171A01");

            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbOperator.GetValue().Equals(@"MSL	Messina Line"),
                @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbOperator.GetValue() +
                " doesn't match Expected: MSL	Messina Line");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbISOType.GetValue().Equals(@"2200	GENERAL"),
                @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbISOType.GetValue() +
                " doesn't match Expected: 2200	GENERAL");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbIMEX.GetValue().Equals(@"Export"),
                @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbIMEX.GetValue() +
                " doesn't match Expected: Export");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbSubTerminal.GetValue().Equals(@"Depot"),
                @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbSubTerminal.GetValue() +
                " doesn't match Expected: Depot");

            _roadGateDetailsReleaseForm.BtnSave.DoClick();

            /*// Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date 
            _warningErrorToCheck = new []
            {
                  "Code :79688. Cargo JLG37171A01's availability status of Unavailable for Release does not match the request's availability status of Available for release"
            };*/
            WarningErrorForm.CompleteWarningErrorForm(@"Warnings for Gate In/Out TT1",
                new[]
                { "Code :79688. Cargo JLG37171A01's availability status of Unavailable for Release does not match the request's availability status of Available for release" });

            roadGateForm.SetFocusToForm();
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Empty~Detail^JLG37171A01; MSL; 2200");
            roadGateForm.TblGateItems.FindClickRow(new[] { "Type^Release Empty~Detail^JLG37171A01; MSL; 2200" });

            // Step 11 - 14
            roadGateForm.btnReleaseFull.DoClick();

            _roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm($"Release Full Container  {terminalId}");

            _roadGateDetailsReleaseForm.TxtCargoId.SetValue("JLG37171A02");

            Assert.IsTrue(_roadGateDetailsReleaseForm.TxtCargoId.GetText().Equals(@"JLG37171A02"),
                @"TestCase37171 - Cargo ID - Actual: " + _roadGateDetailsReleaseForm.TxtCargoId.GetText() +
                " doesn't match Expected: JLG37171A02");

            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbOperator.GetValue().Equals(@"MSL	Messina Line"),
                @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbOperator.GetValue() +
                " doesn't match Expected: MSL	Messina Line");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbISOType.GetValue().Equals(@"2200	GENERAL"),
                @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbISOType.GetValue() +
                " doesn't match Expected: 2200	GENERAL");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbIMEX.GetValue().Equals(@"Export"),
                @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbIMEX.GetValue() +
                " doesn't match Expected: Export");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbCommodity.GetValue().Equals(@"GEN General"),
               @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbCommodity.GetValue() +
               " doesn't match Expected: GEN General");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbVoyage.GetValue().Equals(@"MSCK000002	MSC KATYA R."),
                @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbVoyage.GetValue() +
                " doesn't match Expected: MSCK000002	MSC KATYA R.");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbSubTerminal.GetValue().Equals(@"Depot"),
                @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbSubTerminal.GetValue() +
                " doesn't match Expected: Depot");

            _roadGateDetailsReleaseForm.BtnSave.DoClick();

            WarningErrorForm.CompleteWarningErrorForm($@"Warnings for Gate In/Out {terminalId}",
                new[]
                {
                    "Code :79688. Cargo JLG37171A02's availability status of Unavailable for Release does not match the request's availability status of Available for release"
                });

            roadGateForm.SetFocusToForm();
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date     @"Type^Release Full~Detail^JLG37171A02; MSL; 2200");
            roadGateForm.TblGateItems.FindClickRow(new[] { "Type^Release Full~Detail^JLG37171A02; MSL; 2200" });

            // Step 15 - 19
            roadGateForm.btnReleaseCargo.DoClick();

            _roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm($@"Release General Cargo  {terminalId}");

            _roadGateDetailsReleaseForm.CmbCargoType.SetValue(CargoType.BagOfSand, doDownArrow: true,
                searchSubStringTo: CargoType.BagOfSand.Length - 1);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(100));
            _roadGateDetailsReleaseForm.TxtCargoId.SetValue("JLG37171A03");
            _roadGateDetailsReleaseForm.TxtTotalQuantity.SetValue("10");

            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(2));

            Assert.IsTrue(_roadGateDetailsReleaseForm.TxtCargoId.GetText().Equals(@"JLG37171A03 GCARA1 SMALL_SAND"),
                @"TestCase37171 - Cargo ID - Actual: " + _roadGateDetailsReleaseForm.TxtCargoId.GetText() +
                " doesn't match Expected: JLG37171A03 GCARA1 SMALL_SAND");

            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbOperator.GetValue().Equals(@"MSL	Messina Line"),
                @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbOperator.GetValue() +
                " doesn't match Expected: MSL	Messina Line");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbCargoTypeRbT.GetValue().Equals(@"Bag of Sand"),
                @"TestCase37171 - Cargo Details - Cargo Type - Actual: " +
                _roadGateDetailsReleaseForm.CmbCargoTypeRbT.GetValue() + " doesn't match Expected: Bag of Sand");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbCargoSubTypeRbt.GetValue().Equals(@"Small bag of Sand"),
                @"TestCase37171 - Cargo Details - Cargo Subtype - Actual: " +
                _roadGateDetailsReleaseForm.CmbCargoSubTypeRbt.GetValue() + " doesn't match Expected: Small bag of Sand");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbVoyage.GetValue().Equals(@"MSCK000002	MSC KATYA R."),
                 @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbVoyage.GetValue() +
                 " doesn't match Expected: MSCK000002	MSC KATYA R.");
            Assert.IsTrue(_roadGateDetailsReleaseForm.CmbSubTerminal.GetValue().Equals(@"Depot"),
                @"TestCase37171 - Release Item - Actual: " + _roadGateDetailsReleaseForm.CmbSubTerminal.GetValue() +
                " doesn't match Expected: Depot");

            _roadGateDetailsReleaseForm.BtnSave.DoClick();

            /*// Tuesday, 28 January 2025 navmh5 
            string[] warningErrorToCheck =
            {
                  "Code :79688. Cargo JLG37171A03's availability status of Unavailable for Release does not match the request's availability status of Available for release"
              };*/
            WarningErrorForm.CompleteWarningErrorForm($@"Warnings for Gate In/Out {terminalId}",
                new[]
                {
                    "Code :79688. Cargo JLG37171A03's availability status of Unavailable for Release does not match the request's availability status of Available for release"
                });
            

            roadGateForm.SetFocusToForm();
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date     @"Type^Release General Cargo~Detail^JLG37171A03(10); MSL; Bag of Sand; Small bag of Sand");
            roadGateForm.TblGateItems.FindClickRow(new[]
                { "Type^Release General Cargo~Detail^JLG37171A03(10); MSL; Bag of Sand; Small bag of Sand" });

            roadGateForm.btnSave.DoClick();

            // Step 20 - 23
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit($@"Road Operations {terminalId}", new[] { "37171" });

            // Step 24 - 28;
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm($@"Cargo Enquiry {terminalId}");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Blank, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG37171A0");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", Site.OffSite, 
                EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Mark", _mark, EditRowDataType.TextBox3, doReverse: true);
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG37171A01~State^Off Site" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");

            string[] transactionsToValidate =
            {
                "Type^Moved~Details^From 37171 Road to Nowhere",
                "Type^Released - Road~Details^Release",
                "Type^Moved~Details^From MKBS01 to 37171 Road",
                "Type^Queued~Details^From MKBS01 to 37171 Road",
                "Type^Moved~Details^From Nowhere to MKBS01"
            };
            CargoEnquiryTransactionForm.OpenAndValidateTransactions(@"Transactions for JLG37171A01 TT1",
                transactionsToValidate);

            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG37171A03");
            cargoEnquiryForm.DoSearch();
           
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG37171A03~State^Off Site" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");
            
            transactionsToValidate = new []
            {
                @"Type^Moved~Details^(10) From 37171 Road to Nowhere",
                @"Type^Released - Road~Details^Release (10)",
                @"Type^Sub-Term Depot Release (Road)~Details^From Depot",
                @"Type^Split~Details^[+] Split off  10 Small bag of Sand from JLG37171A03 at location GCARA1 SMALL_SAND (100). Created this cargo JLG37171A03 at location 37171 Road.",
                @"Type^Moved~Details^(10) From GCARA1 SMALL_SAND to 37171 Road"
            };
            CargoEnquiryTransactionForm.OpenAndValidateTransactions($"Transactions for JLG37171A03 {terminalId}",
                transactionsToValidate);
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_37171_";
            
            // Create Cargo on Site 
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37171</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37171A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <mark>" + _mark + "</mark>\n <messageMode>A</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37171</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG37171A02</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>6000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GEN</commodity>\n      <dischargePort>USJAX</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <mark>" + _mark + "</mark>\n <messageMode>A</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>37171</TestCases>\n      <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n      <product>SMSAND</product>\n      <id>JLG37171A03</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>GCARA1 SMALL_SAND</locationId>\n      <weight>1000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <dischargePort>NZNPE</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <totalQuantity>100</totalQuantity>\n      <commodity>SANC</commodity>\n	  <mark>" + _mark + "</mark>\n <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
