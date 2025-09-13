using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase42018 : MTNBase
    {
        CargoMoveItForm _cargoMoveItForm;
        ConfirmationFormYesNo _confirmationFormYesNo;


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"resetData_42018");
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void PlanParentAndChildCargo()
        {
            MTNInitialize();

            string fieldValue = null;

            /*
              TEST 1: 
              JLG42018A01 (parent container)
              JLG42018A02 (child cargo)

              Plan the child and ensure the parent is not planned
            */

            // 1. Find child Break-Bulk cargo JLG42018A02 and plan it (move It) to a terminal area
            // Tuesday, 28 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry");
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.BreakBulkCargo, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG42018A02");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, doReverse: true, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG42018A02", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new[] {"ID^JLG42018A02" }, clickType: ClickType.ContextClick );
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Move It...");

            _cargoMoveItForm = new CargoMoveItForm(formTitle: "Move JLG42018A02 TT1");
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"Move Mode", @"Autoplan (Plan Only)", EditRowDataType.ComboBox, waitTime: 150);
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"To Area Type", @"Bulk Area", EditRowDataType.ComboBoxEdit,waitTime: 150);
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"To Terminal Area", @"BULK", EditRowDataType.ComboBoxEdit, waitTime: 150);


            //_cargoMoveItForm.btnMoveIt.DoClick();
            _cargoMoveItForm.DoMoveIt();
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Move - 1 Items");
            _confirmationFormYesNo.btnYes.DoClick();
            _cargoMoveItForm.CloseForm();
            
            // 2. Check that the cargo is planned to BULK
            cargoEnquiryForm.SetFocusToForm();
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG42018A02");
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG42018A02" });
            cargoEnquiryForm.GetLocationTable(@"4085");
            fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblLocationDetails, @"Planned Location");
            Assert.IsTrue(fieldValue == @"BULK", @"Field: Planned Location has a value of " + fieldValue + @" and should equal: BULK");

          
            // 3. Now check that the parent container JLG42018A01 has NOT been planned
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG42018A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit,doReverse: true, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();


            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG42018A01");
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG42018A01" });
            cargoEnquiryForm.GetLocationTable(@"4084");
            fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblLocationDetails, @"Planned Location");
            Assert.IsTrue(fieldValue.Length == 0, @"Field: Planned Location has a value of " + fieldValue + @" and should be empty");

            /*
            TEST 2: 
            JLG42018A03 (parent container)
            JLG42018A04 (child cargo)

            Plan the parent and ensure the child inherits planned location of parent
            */

            

            // 3. Now plan container JLG42018A03 to a block stack
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG42018A03");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit,doReverse: true, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG42018A03",  clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG42018A03" }, clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Move It...");

            _cargoMoveItForm = new CargoMoveItForm(formTitle: "Move JLG42018A03 TT1");
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"Move Mode", @"Autoplan (Plan Only)", EditRowDataType.ComboBox, waitTime: 150);
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"To Area Type", @"Block Stack", EditRowDataType.ComboBoxEdit,waitTime: 150);
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, @"To Terminal Area", TT1.TerminalArea.MKBS02, EditRowDataType.ComboBoxEdit, waitTime: 150);
            _cargoMoveItForm.DoMoveIt();
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Move - 1 Items");
            _confirmationFormYesNo.btnYes.DoClick();
            _cargoMoveItForm.CloseForm();

            //4.  check that container JLG42018A03 has been planned correctly
            cargoEnquiryForm.SetFocusToForm();


            // Tuesday, 28 January 2025 navmh5  MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG42018A03");
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG42018A03" });
            cargoEnquiryForm.GetLocationTable(@"4084");
            fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblLocationDetails, @"Planned Location");
            Assert.IsTrue(fieldValue == TT1.TerminalArea.MKBS02, @"Field: Planned Location has a value of " + fieldValue + @" and should equal: MKBS02");

            // 4. Now check that child cargo JLG42018A04 (child of container JLG42018A03) has inherited the planned location from the parent
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.BreakBulkCargo, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG42018A04");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, doReverse: true, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG42018A04");
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG42018A04" });
            cargoEnquiryForm.GetLocationTable(@"4085", doReverse: true);
            fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblLocationDetails, @"Planned Location");
            Assert.IsTrue(fieldValue == @"MKBS02 (Parent)", @"Field: Planned Location has a value of " + fieldValue + @" and should equal: MKBS02 (Parent)");


            /*
            TEST 3: 
            JLG42018A02 (child cargo) - UnPlan the child and ensure it is unplanned correctly
            */

            
            
            // 5. unplan JLG42018A02 (break bulk cargo) and check that JLG42018A02 isn't planned
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.BreakBulkCargo, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG42018A02");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, doReverse: true, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG42018A02", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG42018A02" }, clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Unplan JLG42018A02");
            
            fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblLocationDetails, @"Planned Location");        
            Assert.IsTrue(fieldValue.Length == 0, @"Field: Planned Location has a value of " + fieldValue + @" and should be empty");


            /*
            TEST 4: 
            JLG42018A03 (parent) - UnPlan the parent and ensure child/parent are unplanned
            */
            
            
            // 6. unplan JLG42018A03 (ISO Container) and check that JLG42018A03 isn't planned
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG42018A03");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit,doReverse: true, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG42018A03", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG42018A03" }, clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Unplan JLG42018A03");
            cargoEnquiryForm.GetLocationTable(@"4084");
            fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblLocationDetails, @"Planned Location");    
            Assert.IsTrue(fieldValue.Length == 0, @"Field: Planned Location has a value of " + fieldValue + @" and should be empty");

            // 7. Also check that JLG42018A04 is no longer planned
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.BreakBulkCargo, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG42018A04");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, doReverse: true, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG42018A04");
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG42018A04" });
            cargoEnquiryForm.GetLocationTable(@"4085");
            fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblLocationDetails, @"Planned Location");
            Assert.IsTrue(fieldValue.Length == 0, @"Field: Planned Location has a value of " + fieldValue + @" and should be empty");
            cargoEnquiryForm.CloseForm();
            
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_42018_";

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnSite\n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n       <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG42018A01</id>\n            <isoType>220A</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>AER</operatorCode>\n            <dischargePort>NZLYT</dischargePort>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GEN</commodity>\n            <messageMode>A</messageMode>\n			<locationId>MKBS01</locationId>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n            <id>JLG42018A02</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>AER</operatorCode>\n            <dischargePort>NZLYT</dischargePort>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GENL</commodity>\n            <messageMode>A</messageMode>\n			<parentCargoType>ISO Container</parentCargoType>\n			<parentCargoID>JLG42018A01</parentCargoID>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG42018A03</id>\n            <isoType>220A</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>AER</operatorCode>\n            <dischargePort>NZLYT</dischargePort>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GEN</commodity>\n            <messageMode>A</messageMode>\n			<locationId>MKBS01</locationId>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Break-Bulk Cargo</cargoTypeDescr>\n            <id>JLG42018A04</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>AER</operatorCode>\n            <dischargePort>NZLYT</dischargePort>\n            <imexStatus>Export</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GENL</commodity>\n            <messageMode>A</messageMode>\n			<parentCargoType>ISO Container</parentCargoType>\n			<parentCargoID>JLG42018A03</parentCargoID>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
