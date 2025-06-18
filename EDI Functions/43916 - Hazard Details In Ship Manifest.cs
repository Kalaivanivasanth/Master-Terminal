using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43916 : MTNBase
    {
        EDIOperationsForm ediOperationsForm;
        TerminalConfigForm terminalConfigForm;

        protected static string ediFile1 = "M_43916_DeleteCargo.xml";
        protected static string ediFile2 = "M_43916_ShipManifest.txt";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_43916_";
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        private void MTNInitialize()
        {
            //delete cargo on ship
            CreateDataFile(ediFile1,
               "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<isoType>2200</isoType>\n            <id>JLG43916A01</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>CSA</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n			 <messageMode>D</messageMode>\n        </CargoOnShip>\n		<CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>KENDARAAN</cargoTypeDescr>\n            <id>JLG43916A02</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>CSA</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n			 <messageMode>D</messageMode>\n        </CargoOnShip>\n		<CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>KENDARAAN</cargoTypeDescr>\n            <id>JLG43916A03</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>CSA</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n			 <messageMode>D</messageMode>\n        </CargoOnShip>\n          </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            //load ship manifest 
            CreateDataFile(ediFile2,
               "BOLH,A,BOL43916,MSCK000002,CSA,PRSJU,USJAX,CSA,,SHIP\nBOLD,BOL43916,VEHICLE,ABCNR,ABCNR,2013 SCION XD BASE            ,0001,00000000017070,test,\nCARGO,A,JLG43916A01,BOL43916,CONT,,,GEN,ABCNR,ABCNR,USJAX,,,,4200,,,CSA,,,,,,,,3,,,00000000020070,,,,,,MSCK000002,2015\nCARGO,A,JLG43916A02,BOL43916,VEHICLE,CAR1,,,ABCNR,ABCNR,USJAX,,,,,,,CSA,,,,,,JLG43916A01,CONT,3,,,00000000003000,TOYOTA,SCION,,,JLG43916A02,MSCK000002,2013\nCARGO,A,JLG43916A03,BOL43916,VEHICLE,CAR1,,,ABCNR,ABCNR,USJAX,,,,,,,CSA,,,,,,JLG43916A01,CONT,3,,,00000000003000,FIAT,500,,,JLG43916A03,MSCK000002,2017\nHAZ,,JLG43916A01,,CONT,9,ENGINES  INTERNAL COMBUSTION  INCLUDING WHEN FITTED IN MACHI,,N,N,I,3166\nHAZ,,JLG43916A02,,VEHICLE,9,ENGINES  INTERNAL COMBUSTION  INCLUDING WHEN FITTED IN      ,,,N,I,3166\nHAZ,,JLG43916A03,,VEHICLE,9,ENGINES  INTERNAL COMBUSTION  INCLUDING WHEN FITTED IN      ,,,N,II,3166");

            MTNSignon(TestContext);

            // set configuration
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Settings", @"Hazardous - Use Detailed Hazardous Entry Screen",
                @"1", rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();
        }


        [TestMethod]
        public void HazardousCargoOnShipManifest()
        {
            MTNInitialize();
            
            // 1. Open EDI Operations and DB laod the 2 EDI files. 
            // EDI file 1 is just a cargo delete
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperationsForm.DeleteEDIMessages(@"Cargo On Ship", @"43916",ediStatus: @"Loaded");
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Loaded", @"Load To DB");

            ediOperationsForm.DeleteEDIMessages(@"Ship Manifest", @"43916", ediStatus: @"Loaded");
            ediOperationsForm.LoadEDIMessageFromFile(ediFile2, specifyType: true, fileType:@"CPRC_ShipManifest");
            ediOperationsForm.ChangeEDIStatus(ediFile2, @"Loaded", @"Load To DB");
           

            // 2. Open cargo enquiry and find the cargo details
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @" ",EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG43916");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"**Anywhere**", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            //3. validate the hazard details of each cargo item
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43916A01", rowHeight: 18);
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            var tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Is Hazardous"); 
            Assert.IsTrue(tableValue == "Yes", "Data mismatch on Is Hazardous field: Expected=Yes; Actual=" + tableValue);
            tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Hazard Details"); 
            Assert.IsTrue(tableValue == "9 (3166) I, 9 (3166) II", "Data mismatch on Is Hazardous field: Expected=9 (3166) I, 9 (3166) II; Actual=" + tableValue);

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43916A02", rowHeight: 18);
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Is Hazardous");
            Assert.IsTrue(tableValue == "Yes", "Data mismatch on Is Hazardous field: Expected=Yes; Actual=" + tableValue);
            tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Hazard Details"); 
            Assert.IsTrue(tableValue == "9 (3166) I", "data not correct");

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43916A03", rowHeight: 18);
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Is Hazardous"); 
            Assert.IsTrue(tableValue == "Yes", "Data mismatch on Is Hazardous field: Expected=Yes; Actual=" + tableValue);
            tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Hazard Details"); 
            Assert.IsTrue(tableValue == "9 (3166) II", "data not correct");

            //4. Change the configuration
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Settings", @"Hazardous - Use Detailed Hazardous Entry Screen",
                @"0", rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();

            //5. recheck the hazard details on the each cargo item
            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43916A01", rowHeight: 18);
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Is Hazardous"); 
            Assert.IsTrue(tableValue == "Yes", "Data mismatch on Is Hazardous field: Expected=Yes; Actual=" + tableValue);
            tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Hazard Details"); 
            Assert.IsTrue(tableValue == "9 (3166) I", "Data mismatch on Is Hazardous field: Expected=9 (3166) I; Actual=" + tableValue);

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43916A02", rowHeight: 18);
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Is Hazardous"); 
            Assert.IsTrue(tableValue == "Yes", "Data mismatch on Is Hazardous field: Expected=Yes; Actual=" + tableValue);
            tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Hazard Details"); 
            Assert.IsTrue(tableValue == "9 (3166) I", "data not correct");

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43916A03", rowHeight: 18);
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Is Hazardous"); 
            Assert.IsTrue(tableValue == "Yes", "Data mismatch on Is Hazardous field: Expected=Yes; Actual=" + tableValue);
            tableValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Hazard Details"); 
            Assert.IsTrue(tableValue == "9 (3166) II", "data not correct");

        }




    }

}
