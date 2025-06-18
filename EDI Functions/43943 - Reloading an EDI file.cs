using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43943 : MTNBase
    {
        EDIOperationsForm ediOperationsForm;
        EDIDataTranslationsForm ediDataTranslationForm;
        EDITranslationMaintenanceForm ediTranslationMaintenanceForm;

        protected static string ediFile1 = "M_43943_BOL_ADD.edi";


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_43943_";
            BaseClassInitialize_New(testContext);

        }
        
        [TestInitialize]
        public new void TestInitailize() {}


        [TestCleanup]
        public new void TestCleanup()
        {
            FormObjectBase.NavigationMenuSelection(@"EDI Data Translations");
            ediDataTranslationForm = new EDIDataTranslationsForm(@"EDI Data Translations");
            ediDataTranslationForm.DeleteTranslation(@"Ports", "MSK	MAERSK");

            base.TestCleanup();
        }
        
        private void MTNInitialize()
        {
            //Create COPRAR S1 File
            CreateDataFile(ediFile1,
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalBOL \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalBOL.xsd'>\n    <AllBOLHeader>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <BOLHeader Terminal='TT1'>\n            <id>BOL43943</id>\n            <messageMode>A</messageMode>\n            <operatorCode>MSK</operatorCode>\n            <voyageCode>MSCK000002</voyageCode>\n            <dischargePort>NZAKL</dischargePort>\n            <operatorCode>MSK</operatorCode>\n            <AllBOLDetails>\n                <BOLDetails>\n                    <messageMode>A</messageMode>\n                    <cargoTypeDescr>ISO Container</cargoTypeDescr>\n                    <commodity>GEN</commodity>\n                </BOLDetails>\n            </AllBOLDetails>\n        </BOLHeader>\n    </AllBOLHeader>\n</JMTInternalBOL>");

            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();

            FormObjectBase.NavigationMenuSelection(@"System Ops|EDI Data Translations");
            ediDataTranslationForm = new EDIDataTranslationsForm(@"EDI Data Translations");
            ediDataTranslationForm.DeleteTranslation(@"Ports", "MSK	MAERSK");
        }


        [TestMethod]
        public void ReloadingEDIFile()
        {

            MTNInitialize();

            // 1 Open EDI Operations and delete existing files
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperationsForm.DeleteEDIMessages(@"BOL", @"43943", "Verified^Loaded", clearStatus: true);

            // 2.Load BOL EDI file and verify voyage is MSCK000002
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Loaded", @"Verify");
            /*ediOperationsForm.tabEDIDetails.Click();
            MTNControlBase.FindTabOnForm(ediOperationsForm.tabEDIDetails, @"BOL Header");
            ediOperationsForm.ShowEDIDataTable(@"4036");*/
            ediOperationsForm.ClickEDIDetailsTab();
            ediOperationsForm.GetTabTableGeneric(@"BOL Header", @"4036");
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Verified~File Name^" + ediFile1, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Verified~File Name^" + ediFile1);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Voyage^MSCK000002~ID^BOL43943~Discharge Port^NZAKL", clickType: ClickType.None, searchType: SearchType.Exact);
        
            //3. add an EDI data translation
            FormObjectBase.NavigationMenuSelection(@"System Ops|EDI Data Translations", forceReset: true);
            ediDataTranslationForm = new EDIDataTranslationsForm(@"EDI Data Translations");

            //ediDataTranslationForm.btnNew.DoClick();
            ediDataTranslationForm.DoAdd();
            ediTranslationMaintenanceForm = new EDITranslationMaintenanceForm(@"EDI Translation Maintenance ");
            //MTNControlBase.SetValue(ediTranslationMaintenanceForm.cmbTranslationType, @"Ports");
            ediTranslationMaintenanceForm.cmbTranslationType.SetValue(@"Ports");
            //MTNControlBase.SetValue(ediTranslationMaintenanceForm.cmbOperator, @"MSK");
            ediTranslationMaintenanceForm.cmbOperator.SetValue(Operator.MSK,  doDownArrow: true);
            //MTNControlBase.SetValue(ediTranslationMaintenanceForm.txtForeignIn, @"NZAKL");
            ediTranslationMaintenanceForm.txtForeignIn.SetValue(@"NZAKL");
            //MTNControlBase.SetValue(ediTranslationMaintenanceForm.cmbSystemCode, @"Aalter [BEAAL]");
            ediTranslationMaintenanceForm.cmbSystemCode.SetValue(@"Aalter [BEAAL]");
            //MTNControlBase.SetValue(ediTranslationMaintenanceForm.txtForeignOut, @"NZAKL");
            ediTranslationMaintenanceForm.txtForeignOut.SetValue(@"NZAKL");
            ediTranslationMaintenanceForm.btnOK.DoClick();
            //ediDataTranslationForm.btnCancel.DoClick();
            ediDataTranslationForm.DoCancel();

            //4. Go back into EDI Operations and reload/retranslate and verify voyage is now BEAAL
            ediOperationsForm.SetFocusToForm();
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Verified", @"Reload (re Translate)");
            /*MTNControlBase.FindTabOnForm(ediOperationsForm.tabEDIDetails, @"BOL Header");
            ediOperationsForm.ShowEDIDataTable(@"4036");*/
            ediOperationsForm.GetTabTableGeneric(@"BOL Header", @"4036");
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, rowHeight: 16, xOffset: -1);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile1);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIDetails, @"Voyage^MSCK000002~ID^BOL43943~Discharge Port^BEAAL", clickType: ClickType.None, searchType: SearchType.Exact);

        }




    }

}
