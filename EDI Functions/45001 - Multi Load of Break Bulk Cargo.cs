using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45001 : MTNBase
    {

        EDIOperationsForm ediOperationsForm;
 
        protected static string ediFile1 = "M_45001_DeleteCargo.xml";
        protected static string ediFile2 = "M_45001_CargoAdd.csv";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_45001_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //delete cargo on ship
            CreateDataFile(ediFile1,
               "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnShip Terminal='TT2'>\n      <TestCases>45001</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>PIPE</product>\n      <id>JLG45001A01</id>\n      <operatorCode>MSL</operatorCode>\n	  <loadPort>USJAX</loadPort>\n	  <bol>TC45001A</bol>\n	  <consignee>ABCNR</consignee>\n	  <totalQuantity>277</totalQuantity>\n      <locationId>TEST45001 010882</locationId>\n      <weight>4000</weight>\n      <imexStatus>Import</imexStatus>\n      <voyageCode>TEST45001</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnShip>\n    <CargoOnShip Terminal='TT2'> \n      <TestCases>45001</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n      <id>JLG45001A01</id>\n	  <product>PIPE</product>\n      <operatorCode>MSL</operatorCode>\n	  <loadPort>USJAX</loadPort>\n	  <bol>TC45001B</bol>\n	  <consignee>ABCNR</consignee>\n	  <totalQuantity>97</totalQuantity>\n      <locationId>TEST45001 010682</locationId>\n      <weight>4000</weight>\n      <imexStatus>Import</imexStatus>\n      <voyageCode>TEST45001</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnShip> \n  </AllCargoOnShip>\n</JMTInternalCargoOnShip>");

            //load ship manifest 
            CreateDataFile(ediFile2,
               "MSL,TEST45001,USJAX,TC45001A,ABCNR,JLG45001A01,277,9000,STEEL,Pipe,010882,Import,4.5in_12.6lb_Reg_OCTG_SMLS\nMSL,TEST45001,USJAX,TC45001B,ABCNR,JLG45001A01,97,4000,STEEL,Pipe,010682,Import,4.5in_12.6lb_Reg_OCTG_SMLS\n");

            MTNSignon(TestContext);
            
            CallJadeScriptToRun(TestContext, @"resetData_45001");  
        }


        [TestMethod]
        public void MultiLoadOfBreakBulkCargo()
        {
            MTNInitialize();
            
           // string tableValue = null;


            // 1. Open EDI Operations and DB laod the 2 EDI files. 
            // EDI file 1 is just a cargo delete
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            ediOperationsForm.DeleteEDIMessages(@"Cargo On Ship", @"45001",ediStatus: @"Loaded");
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Loaded", @"Load To DB");

            ediOperationsForm.DeleteEDIMessages(@"Cargo", @"45002", ediStatus: @"Loaded");
            ediOperationsForm.LoadEDIMessageFromFile(ediFile2, specifyType: true, fileType:@"44033");

            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile2, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile2);
            //ediOperationsForm.ShowEDIDataTable(@"4040");
            ediOperationsForm.GetTabTableGeneric(@"EDI Details", @"4040");
            //ediOperationsForm.btnEdit.DoClick();
            ediOperationsForm.DoEdit();

            MTNControlBase.SetValueInEditTable(ediOperationsForm.tblEDIDetails, @"Site", @"On Ship", rowDataType: EditRowDataType.ComboBox);
            //ediOperationsForm.btnSaveVerify.DoClick();
            ediOperationsForm.DoSaveVerify();
            ediOperationsForm.ChangeEDIStatus(ediFile2, @"Verified", @"Load To DB");
            ediOperationsForm.CloseForm();

            // 2. Open cargo enquiry and find the cargo details
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"STEEL", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG45001");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Ship", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            //3. validate the cargo data has been loaded as expected
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG45001A01~Total Quantity^97~Consignee^ABCNR~Bill Of Lading^TC45001B", rowHeight: 18, clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG45001A01~Total Quantity^277~Consignee^ABCNR~Bill Of Lading^TC45001A", rowHeight: 18, clickType: ClickType.None);

            
        }




    }

}
