using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using System.Drawing;
using System.Text;
using DataObjects.LogInOutBO;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41318 : MTNBase
    {

        private EDIOperationsForm _ediOperationsForm;
        private EDIPartyEnquiryForm _ediPartyEnquiryForm;
        private EDIPartyMaintenanceForm _ediPartyMaintenanceForm;
        int _count;

        private const string ediFile = "M_" + TestCaseNumber + "_COPARN.edi";
        private const string ediFile1 = "M_" + TestCaseNumber + "A_COPARN.edi";
        private const string ediFile2 = "M_" + TestCaseNumber + "B_COPARN.edi";
        private const string ediFile3 = "M_" + TestCaseNumber + "C_COPARN.edi";
        
        private const string TestCaseNumber = @"41318";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";

        
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
        
         private void MTNInitialize()
        {

            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();
                
            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);

            // Step 2 - 6 - Handled by resetTerminalConfig 
            SetupAndLoadInitializeData(TestContext);

            CreateDataFile(ediFile,
                "UNA:+.'\nUNB+UNOC:3+LENAVI-GOA:14+ITGOAAL:14+181003:0921+8153'\nUNH+4+COPARN:D:00B:UN:SMDG20'\nBGM+11+41318+9'\nRFF+BN:41318'\nTDT+20+MSCK000002+1++MSC:172:20+++D5PF2:103::MSC ASLI'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nDTM+133:201810060400:203'\nNAD+CF+MSC'\nNAD+MS+MSC'\nNAD+CB+ANGELO PROVERA SRL:160:ZZZ'\nEQD+CN+JLG41318A01+2200:102:5++2+5'\nEQN+1:2'\nDTM+132:201810051200:203'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nLOC+11+NZAKL:139:6:GIOIA TAURO+IT:162:5:ITALY'\nLOC+88+ITCRM:139:6:Carmagnola+IT:162:5:Italy'\nLOC+7+NZBLU:139:6:NOVOROSSIYSK+RU:162:5:RUSSIAN FEDERATION'\nMEA+AAE+G+KGM:25000'\nFTX+AAA+++GEN'\nCNT+16:1'\nUNT+20+4'\nUNZ+4+8153'\n");

            CreateDataFile(ediFile1,
                "UNA:+.'\nUNB+UNOC:3+LENAVI-GOA:14+ITGOAAL:14+181003:1531+8183'\nUNH+1+COPARN:D:00B:UN:SMDG20'\nBGM+11+41318+5'\nRFF+BN:41318'\nTDT+20+MSCK000002+1++MSC:172:20+++D5PF2:103::MSC ASLI'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nDTM+133:201810052000:203'\nNAD+CF+MSC'\nNAD+MS+MSC'\nNAD+CB+ANGELO PROVERA SRL:160:ZZZ'\nEQD+CN+JLG41318A01+4510:102:5++2+5'\nRFF+VGR:VGMG0012954000'\nEQN+1:2'\nDTM+132:201810050400:203'\nDTM+798:201810031300:203'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nLOC+11+NZAKL:139:6:GIOIA TAURO+IT:162:5:ITALY'\nLOC+88+ITCRM:139:6:Carmagnola+IT:162:5:Italy'\nLOC+7+NZBLU:139:6:NOVOROSSIYSK+RU:162:5:RUSSIAN FEDERATION'\nMEA+AAE+VGM+KGM:20000'\nFTX+AAA+++GEN'\nFTX+ABS++SM1:ZZZ:SMD'\nNAD+AM+++TESTER'\nNAD+SPC+++TEST41318'\nCNT+16:1'\nUNT+25+1'\n\n");

            CreateDataFile(ediFile2,
                "UNA:+.'\nUNB+UNOC:3+LENAVI-GOA:14+ITGOAAL:14+181003:0921+8153'\nUNH+4+COPARN:D:00B:UN:SMDG20'\nBGM+11+41318+9'\nRFF+BN:41318'\nTDT+20+MSCK000002+1++MSC:172:20+++D5PF2:103::MSC ASLI'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nDTM+133:201810060400:203'\nNAD+CF+MSC'\nNAD+MS+MSC'\nNAD+CB+ANGELO PROVERA SRL:160:ZZZ'\nEQD+CN+JLG41318A01+2200:102:5++2+5'\nEQN+1:2'\nDTM+132:201810051200:203'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nLOC+11+NZAKL:139:6:GIOIA TAURO+IT:162:5:ITALY'\nLOC+88+ITCRM:139:6:Carmagnola+IT:162:5:Italy'\nLOC+7+NZBLU:139:6:NOVOROSSIYSK+RU:162:5:RUSSIAN FEDERATION'\nMEA+AAE+G+KGM:25000'\nFTX+AAA+++GEN'\nCNT+16:1'\nUNT+20+4'\nUNZ+4+8153'\n\n");

            CreateDataFile(ediFile3,
                "UNA:+.'\nUNB+UNOC:3+LENAVI-GOA:14+ITGOAAL:14+181003:1531+8183'\nUNH+1+COPARN:D:00B:UN:SMDG20'\nBGM+11+41318+5'\nRFF+BN:41318'\nTDT+20+MSCK000002+1++MSC:172:20+++D5PF2:103::MSC ASLI'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nDTM+133:201810052000:203'\nNAD+CF+MSC'\nNAD+MS+MSC'\nNAD+CB+ANGELO PROVERA SRL:160:ZZZ'\nEQD+CN+JLG41318A01+4510:102:5++2+5'\nRFF+VGR:VGMG0012954000'\nEQN+1:2'\nDTM+132:201810050400:203'\nDTM+798:201810031300:203'\nLOC+9+ITGOA:139:6:GENOVA+IT:162:5:ITALY'\nLOC+11+NZAKL:139:6:GIOIA TAURO+IT:162:5:ITALY'\nLOC+88+ITCRM:139:6:Carmagnola+IT:162:5:Italy'\nLOC+7+NZBLU:139:6:NOVOROSSIYSK+RU:162:5:RUSSIAN FEDERATION'\nMEA+AAE+VGM+KGM:20000'\nFTX+AAA+++GEN'\nFTX+ABS++SM1:ZZZ:SMD'\nNAD+AM+++TESTER'\nNAD+SPC+++TEST41318'\nCNT+16:1'\nUNT+25+1'\n\n");


        }


        [TestMethod]
        public void COPARNUpdateVGMCargoOnSite()
        {
            MTNInitialize();


            // Step 8 - 25
            // Will default set the columns
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            StringBuilder statuses = new StringBuilder(EDIOperationsStatusType.DBPartiallyLoadedAsterisk + "^" +
                                                       EDIOperationsStatusType.DBLoaded + "^" +
                                                       EDIOperationsStatusType.Loaded);
            _ediOperationsForm.DeleteEDIMessages(EDIOperationsDataType.GateDocument, @"M_" + TestCaseNumber, statuses.ToString() );

            MTNControlBase.SetValueInEditTable(_ediOperationsForm.tblEDISearch, @"Status");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            
            StepsToDo(ediFile, ediFile1, 
                @"ID^" + CargoId + "~Is Weight Certified^~Total Weight MT^25.000~Weight Certifying Person^~Weight Certifying Authority^",
                @"ID^" + CargoId + "~Is Weight Certified^Yes~Total Weight MT^20.000~Weight Certifying Person^TESTER~Weight Certifying Authority^TEST41318",
                @"ID^" + CargoId + "~Is Weight Certified^~Weight Certifying Authority^~Weight Certifying Person^~Total Weight MT^25.000", 
                @"Status^DB Partially Loaded~File Name^");

            // Step 26 - 29
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Party Enquiry", forceReset: true);
            _ediPartyEnquiryForm = new EDIPartyEnquiryForm(@"EDI Party Enquiry Form TT1");
           
            MTNControlBase.FindClickRowInTable(_ediPartyEnquiryForm.tblDetails, @"Name^Coparn_41318", ClickType.DoubleClick,
                rowHeight: 16);

            _ediPartyMaintenanceForm = new EDIPartyMaintenanceForm(@"EDI Party Maintenance TT1");
            _ediPartyMaintenanceForm.GetCoparnTabDetails();
            _ediPartyMaintenanceForm.chkUpdateVGMIfCargoOnSite.DoClick();
            _ediPartyMaintenanceForm.btnSave.DoClick();

            // Step 29 - 32
            SetupAndLoadInitializeData2(TestContext);

            _ediOperationsForm.SetFocusToForm();

            StepsToDo(ediFile2, ediFile3,
                @"ID^" + CargoId + "~Is Weight Certified^~Total Weight MT^25.000~Weight Certifying Person^~Weight Certifying Authority^",
                @"ID^" + CargoId + "~Is Weight Certified^Yes~Total Weight MT^20.000~Weight Certifying Person^TESTER~Weight Certifying Authority^TEST41318",
                @"ID^" + CargoId + "~Is Weight Certified^~Weight Certifying Authority^~Weight Certifying Person^~Total Weight MT^20.000", 
                @"Status^DB Loaded~File Name^");

        }

        private void StepsToDo(string ediFileAdd, string ediFileUpdate, string lineDetailsToCheckAdd,
            string lineDetailsToCheckUpdate, string lineDetailsToCheckCargoEnquiry, string ediOpsLineText)
        {
            
            
            
            LoadEDIFile(ediFileAdd);

            var tabGenericBRect = _ediOperationsForm.TabGenericBoundingRectangle;
            var pointToClick = new Point(tabGenericBRect.Left + 150, tabGenericBRect.Top + 45);
            Mouse.Click(pointToClick);
            Miscellaneous.CaptureFailureScreenShot(TestContext, $"41318_EDIClick{_count++}.png", true);

            /*_ediOperationsForm.tabPrenote = MTNControlBase.FindTabOnForm(_ediOperationsForm.tabEDIDetails, @"Prenote");
            _ediOperationsForm.ShowEDIDataTable(@"6166");*/
           // _ediOperationsForm.GetTabTableGeneric(@"Prenote", @"6188");
            _ediOperationsForm.GetTabTableGeneric("Prenote");
            MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails, lineDetailsToCheckAdd, ClickType.None, rowHeight: 16);
            
            if (roadGateForm == null)
            {
                FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
                roadGateForm = new RoadGateForm(@"Road Gate TT1", vehicleId: TestCaseNumber);
            }
            else
            {
                roadGateForm.SetFocusToForm();
            }
            
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);
            roadGateForm.txtNewItem.SetValue(CargoId);

            var receiveFullContainerForm = new RoadGateDetailsReceiveForm(@"Receive Full Container TT1");
            //FormObjectBase.ClickButton(receiveFullContainerForm.btnSave);
            receiveFullContainerForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();

            _ediOperationsForm.SetFocusToForm();

            LoadEDIFile(ediFileUpdate);

            //MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIMessages, ediOpsLineText + ediFileUpdate);
           ////_ediOperationsForm.TblEDIMessages.FindClickRow(ediOpsLineText + ediFileUpdate);
           
           Mouse.Click(pointToClick);
           Miscellaneous.CaptureFailureScreenShot(TestContext, $"41318_EDIClick{_count++}.png", true);

           // _ediOperationsForm.GetTabTableGeneric(@"Prenote", @"6188", @"6187");
            _ediOperationsForm.GetTabTableGeneric("Prenote");
            Wait.UntilResponsive(_ediOperationsForm.tblEDIDetails);
            MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails, lineDetailsToCheckUpdate);

            // Check Cargo Enquiry Details
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, lineDetailsToCheckCargoEnquiry, rowHeight: 18);
            cargoEnquiryForm.CloseForm();

            // Exit vehicle
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new [] { TestCaseNumber });
        }

        private void LoadEDIFile(string fileToLoad)
        {
            _ediOperationsForm.SetFocusToForm();

            _ediOperationsForm.LoadEDIMessageFromFile(fileToLoad);

            //MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIMessages,
            //    @"Status^Loaded~File Name^" + fileToLoad, ClickType.ContextClick, rowHeight: 16, xOffset: -1);
            _ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + fileToLoad, ClickType.ContextClick);
           
            _ediOperationsForm.ContextMenuSelect(@"Load To DB");
            //_ediOperationsForm.ClickEDIDetailsTab();
            //_ediOperationsForm.GetTabTableGeneric("EDI Details");
           // _ediOperationsForm.tabEDIDetails.Click();
           
        }


       


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG41318A01</id>\n            <operatorCode>MSC</operatorCode>\n            <locationId>MKBS01</locationId>\n            <weight>2000</weight>\n            <commodity>GEN</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <isoType>2200</isoType>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }


        private static void SetupAndLoadInitializeData2(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber + "Z_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG41318A01</id>\n            <operatorCode>MSC</operatorCode>\n            <locationId>MKBS01</locationId>\n            <weight>2000</weight>\n            <commodity>GEN</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <isoType>2200</isoType>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }


        #endregion - Setup and Run Data Loads

    }

}
