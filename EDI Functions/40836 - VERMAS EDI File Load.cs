using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40836 : MTNBase
    {

        EDIOperationsForm ediOperationsForm;
        SystemAdminForm systemAdminForm;
        CargoEnquiryTransactionForm cargoEnquiryTransactionForm;
        TerminalConfigForm terminalConfigForm;

        protected static string ediFile1 = "M_40836_DeleteCargo.xml";
        protected static string ediFile2 = "M_40836_CreateCargo.xml";
        protected static string ediFile3 = "M_40836_VERMAS.edi";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_40836_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            //reset configuration
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"VGM", @"Certified Weight Needs VGM Details", @"0", rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.SetTerminalConfiguration(@"VGM", @"Default Weight Certifying Authority", @"TEST TEAM", rowDataType: EditRowDataType.ComboBox);
            terminalConfigForm.CloseForm();

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //delete cargo on site
            CreateDataFile(ediFile1,
               "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40836A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>COS</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //create cargo on site
             CreateDataFile(ediFile2,
               "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40836A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>COS</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>APPL</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");


            //vermas file
            CreateDataFile(ediFile3,
             "UNA:+.'\nUNB+UNOA:2+ONEY:02+ITSCT+180723:2210+3'\nUNH+3+VERMAS:D:16A:UN:SMDG04'\nBGM+++9'\nDTM+137:201807232210:203'\nRFF+SI'\nEQD+CN+JLG40836A01:6346:5+2200:6346:5+0++5'\nRFF+BM:LONU00064500'\nRFF+BN:LONU00064500'\nMEA+AAE+VGM+KGM:2230'\nDTM+WAT:201807231610:203'\nDOC+SM1:VGM:306'\nDTM+WAT:201807231610:203'\nNAD+AM'\nCTA+RP+:SUNITHA'\nCOM+SUNITHA.BABU@ONE-LINE.COM:EM'\nUNT+15+3'\nUNZ+1+3'");


            MTNSignon(TestContext);

            // set configuration
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"VGM", @"Certified Weight Needs VGM Details", @"0", rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.SetTerminalConfiguration(@"VGM", @"Default Weight Certifying Authority", @"TEST TEAM", rowDataType: EditRowDataType.ComboBox);
            terminalConfigForm.CloseForm();
        }


        [TestMethod]
        public void VermasEDILoad()
        {
            
            MTNInitialize();
  
            // 1. Open EDI Operations and create the cargo on-site - first delete then create 
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperationsForm.DeleteEDIMessages(@"Cargo On Site", @"40836",ediStatus: @"Loaded");
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Loaded", @"Load To DB");
            ediOperationsForm.LoadEDIMessageFromFile(ediFile2);
            ediOperationsForm.ChangeEDIStatus(ediFile2, @"Loaded", @"Load To DB");
            
            //2. Check the weigh authority has the correct email address
            FormObjectBase.NavigationMenuSelection(@"System Ops|System Admin", forceReset: true);
            systemAdminForm = new SystemAdminForm(@"System Administration");
            //MTNControlBase.SetValue(systemAdminForm.cmbTable, @"Weigh Authority");
            systemAdminForm.cmbTable.SetValue(@"Weigh Authority");
            //MTNControlBase.FindClickRowInTable(systemAdminForm.tblAdministrationItems, @"Weight Certifying Authority^TEST TEAM~Email Address 1^TEST.TEAM@JADELOGISTICS.COM", clickType: ClickType.None,searchType: SearchType.Exact);
            systemAdminForm.TblAdministrationItemsRH19.FindClickRow(
                @"Weight Certifying Authority^TEST TEAM~Email Address 1^TEST.TEAM@JADELOGISTICS.COM",
                clickType: ClickType.None, searchType: SearchType.Exact);

            //3. Load the Vermas file
            ediOperationsForm.SetFocusToForm();
            ediOperationsForm.LoadEDIMessageFromFile(ediFile3);
            ediOperationsForm.ChangeEDIStatus(ediFile3, @"Loaded", @"Load To DB");

            //4. Goto cargo enquiry and check the vermas has been load correctly
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG40836A01");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40836A01", rowHeight: 18);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for JLG40836A01 TT1");
            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Edited~Details^isWeightCertified No => Yes: totalWeight 11245.789 lbs => 4916.312 lbs");
                
            //5. Check the email address hasn't changed.
            systemAdminForm.SetFocusToForm();
            //MTNControlBase.SetValue(systemAdminForm.cmbTable, @"Availability Grade");
            systemAdminForm.cmbTable.SetValue(@"Availability Grade");
            //MTNControlBase.SetValue(systemAdminForm.cmbTable, @"Weigh Authority");
            systemAdminForm.cmbTable.SetValue(@"Weigh Authority");
            //MTNControlBase.FindClickRowInTable(systemAdminForm.tblAdministrationItems, @"Weight Certifying Authority^TEST TEAM~Email Address 1^TEST.TEAM@JADELOGISTICS.COM", clickType: ClickType.None, searchType: SearchType.Exact);
            systemAdminForm.TblAdministrationItemsRH19.FindClickRow(
                @"Weight Certifying Authority^TEST TEAM~Email Address 1^TEST.TEAM@JADELOGISTICS.COM",
                clickType: ClickType.None, searchType: SearchType.Exact);
            
        }

    }

}
