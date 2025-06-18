using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44033 : MTNBase
    {

        EDIOperationsForm _ediOperationsForm;
       
        const string EdiFile1 = "M_44033_DeleteCargo.xml";
        const string EdiFile2 = "M_44033_CargoAdd.csv";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_440433_";
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
            //delete cargo on ship
            CreateDataFile(EdiFile1,
               "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnShip Terminal='TT1'>\n      <TestCases>44033</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n	  <product>PIPE</product>\n      <id>JLG44033A01</id>\n      <operatorCode>MSL</operatorCode>\n	  <loadPort>USJAX</loadPort>\n	  <bol>TC44033A</bol>\n	  <consignee>ABCNR</consignee>\n	  <totalQuantity>83</totalQuantity>\n      <locationId>TEST44033 010882</locationId>\n      <weight>4000</weight>\n      <imexStatus>Import</imexStatus>\n      <voyageCode>TEST44033</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnShip>\n    <CargoOnShip Terminal='TT1'> \n      <TestCases>44033</TestCases>\n      <cargoTypeDescr>Steel</cargoTypeDescr>\n      <id>JLG44033A02</id>\n	  <product>PIPE</product>\n      <operatorCode>MSL</operatorCode>\n	  <loadPort>USJAX</loadPort>\n	  <bol>TC44033B</bol>\n	  <consignee>ABCNR</consignee>\n	  <totalQuantity>348</totalQuantity>\n      <locationId>TEST44033 010682</locationId>\n      <weight>4000</weight>\n      <imexStatus>Import</imexStatus>\n      <voyageCode>TEST44033</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnShip> \n  </AllCargoOnShip>\n</JMTInternalCargoOnShip>");

            //load ship manifest 
            CreateDataFile(EdiFile2,
               "MSL,TEST44033,USJAX,TC44033A,ABCNR,JLG44033A01,83,4000,STEEL,Pipe,010882,Import,4.5in_12.6lb_Reg_OCTG_SMLS\nMSL,TEST44033,USJAX,TC44033B,ABCNR,JLG44033A02,348,4000,STEEL,Pipe,010682,Import,7in_26lb_Reg_OCTG_SMLS\n");

            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();
            
            CallJadeScriptToRun(TestContext, "resetData_44033"); 

        }


        [TestMethod]
        public void MultiLoadOfBreakBulkCargoViaEDI()
        {
            
            MTNInitialize();

            // 1. Open EDI Operations and DB laod the 2 EDI files. 
            // EDI file 1 is just a cargo delete
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            _ediOperationsForm.DeleteEDIMessages(@"Cargo On Ship", @"44033",ediStatus: @"Loaded");
            _ediOperationsForm.LoadEDIMessageFromFile(EdiFile1);
            _ediOperationsForm.ChangeEDIStatus(EdiFile1, @"Loaded", @"Load To DB");

            _ediOperationsForm.DeleteEDIMessages(@"Cargo", @"44033", ediStatus: @"Loaded");
            _ediOperationsForm.LoadEDIMessageFromFile(EdiFile2, specifyType: true, fileType:@"44033");
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile2, rowHeight: 16, xOffset: -1);
            _ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + EdiFile2);
            //ediOperationsForm.ShowEDIDataTable(@"4040");
            //_ediOperationsForm.GetTabTableGeneric(@"EDI Details", @"4040");
            _ediOperationsForm.GetTabTableGeneric("EDI Details");
            //ediOperationsForm.btnEdit.DoClick();
            _ediOperationsForm.DoEdit();

            MTNControlBase.SetValueInEditTable(_ediOperationsForm.tblEDIDetails, @"Site", @"On Ship",
                rowDataType: EditRowDataType.ComboBoxEdit); //, doDownArrow: true, searchSubStringTo: "On Ship".Length - 1);
            //ediOperationsForm.btnSaveVerify.DoClick();
            _ediOperationsForm.DoSaveVerify();
            _ediOperationsForm.ChangeEDIStatus(EdiFile2, @"Verified", @"Load To DB");
           
            // 2. Open cargo enquiry and find the cargo details
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"STEEL",EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG44033");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Ship",EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            //3. validate the cargo data has been loaded as expected
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44033A01~Total Quantity^83~Consignee^ABCNR~Bill Of Lading^TC44033A", rowHeight: 18, clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44033A02~Total Quantity^348~Consignee^ABCNR~Bill Of Lading^TC44033B", rowHeight: 18, clickType: ClickType.None);   
            
        }




    }

}
