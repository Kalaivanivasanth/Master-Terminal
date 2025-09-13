using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNUtilityClasses.Classes;
using System;
using System.Diagnostics;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44063 : MTNBase
    {

        private const string TestCaseNumber = @"44063";

        const string EDIFile1 = "M_" + TestCaseNumber + "_LoadDischargeList.edi";


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
    
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();

            // Create Load / Discharge List
            CreateDataFile(EDIFile1,
                "UNB+UNOA:1+03715600965+ITSALSCT+180210:0000+97'\nUNH+97+COPRAR:D:95B:UN'\nBGM+43:::VRHO2032E+201802090000302711+9'\nRFF+XXX:1'\nTDT+20+MSCK000002+1++KKK:172:20+++VRHO2:103::BROTONNE BRIDGE'\nLOC+11+NZLYT:139:6+DKAAR::ZZZ'\nDTM+132:201802150600:203'\nDTM+133:201802151400:203'\nNAD+CA+MSL:172:20'\nEQD+CN+JLG44063A01+42G1:102:5+2+3+5't\nRFF+BN'\nLOC+9+:139:6+:TER:ZZZ'\nLOC+7+NZBLU:139:6+:TER:ZZZ'\nLOC+11+NZBLU:139:6+DKAAR:TER:ZZZ'\nMEA+AAE+VGM+KGM:21885'\nSEL+1088840+CA'\nNAD+CF+MSL:160:ZZZ'\nCNT+16:1'\nUNT+18+97'\nUNZ+1+97'\n");

        }


        [TestMethod]
        public void CargoComparisonProcess()
        {
            MTNInitialize();

            //  Step 12 - 15
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            EDIOperationsForm ediOperationsForm = new EDIOperationsForm();

            ediOperationsForm.DeleteEDIMessages(@"Load/Discharge List", TestCaseNumber);
            
            ediOperationsForm.LoadEDIMessageFromFile(EDIFile1);

            //    rowHeight: 16, xOffset: -1);
            // Thursday, 27 February 2025 navmh5 Can be removed 6 months after specified date ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + EDIFile1, clickType: ClickType.ContextClick);
            ediOperationsForm.TblMessages.FindClickRow(new[] { $"Status^Loaded~File Name^{EDIFile1}" }, clickType: ClickType.ContextClick);
            ediOperationsForm.ContextMenuSelect(@"Verify");

            ediOperationsForm.GetTabTableGeneric(@"EDI Details", $"Status^Verified~File Name^{EDIFile1}");

            var loaded = MTNControlBase.GetValueInEditTable(ediOperationsForm.TabGeneric.TableWithHeader.GetElement(), @"Loaded");
            var operatorCode = MTNControlBase.GetValueInEditTable(ediOperationsForm.TabGeneric.TableWithHeader.GetElement(), @"Operator");
            var voyage = MTNControlBase.GetValueInEditTable(ediOperationsForm.TabGeneric.TableWithHeader.GetElement(), @"Voyage");
            var typeOfList = MTNControlBase.GetValueInEditTable(ediOperationsForm.TabGeneric.TableWithHeader.GetElement(), @"Type of List");

            var listName = typeOfList + " - " + voyage + " - " + loaded + " - " + operatorCode;
            Trace.TraceInformation(@"listName: {0}", listName);
            Trace.TraceInformation(@"voyage: {0}", voyage);

            // Step 16 - 21
            FormObjectBase.MainForm.OpenCargoComparisonFromToolbar();
            CargoComparisonForm cargoComparisonForm = new CargoComparisonForm();

            cargoComparisonForm.GetSearcherListComparisonTab();

            cargoComparisonForm.cmbVoyage.SetValue($"{voyage} - MSC KATYA R.");
            try
            {
                WarningErrorForm.CompleteWarningErrorForm("Warnings for Cargo Comparison");
            }
            catch (Exception) { /* do nothing */ }
            
            cargoComparisonForm.cmbList.SetValue(listName);
            try
            {
                WarningErrorForm.CompleteWarningErrorForm("Warnings for Cargo Comparison");
            }
            catch (Exception) { /* do nothing */ }
            
            cargoComparisonForm.chkUpdateRTS.DoClick(false);

            cargoComparisonForm.GetSearcherFieldSettingsTab();
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(2));
            cargoComparisonForm.tlcFieldSettings.BtnAllLeft.DoClick();
            string[] fieldSettingsToUse =
            {
                "ISO Type",
                "Operator",
                "Total Weight"
            };
            cargoComparisonForm.tlcFieldSettings.MoveItemsBetweenList(cargoComparisonForm.tlcFieldSettings.LstLeft, fieldSettingsToUse);

            cargoComparisonForm.GetSearcherOperatorsTab();
            cargoComparisonForm.ClearAllOperators();
            string[,] operatorsToSet =
            {
                { @"MSC - Mediterranean Shipping  Company", @"1" }
            };
            MTNControlBase.SetUnsetValueCheckboxTable(cargoComparisonForm.tblOperators, operatorsToSet);
           
            cargoComparisonForm.GetSearcherUpdateSettingsTab();
            cargoComparisonForm.tlcFieldsToUpdate.BtnAllLeft.DoClick();
            string[] updateSettingsToUse =
            {
                "ISO Type",
                "Operator",
                "Total Weight"
            };
            cargoComparisonForm.tlcFieldsToUpdate.MoveItemsBetweenList(cargoComparisonForm.tlcFieldsToUpdate.LstLeft, updateSettingsToUse);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(200));
            cargoComparisonForm.DoRunComparison();

            try
            {
                WarningErrorForm.CompleteWarningErrorForm("Warnings for Cargo Comparison");
            }
            catch (Exception) { /* do nothing */ }

            cargoComparisonForm.cmbView.SetValue("Matching with discrepancies");
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(10));
            
            cargoComparisonForm.DoLoadComparison();

            try
            {
                WarningErrorForm.CompleteWarningErrorForm("Warnings for Cargo Comparison");
            }
            catch (Exception) { /* do nothing */ }

            /*// Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(cargoComparisonForm.tblDetails,
                @"ID^JLG44063A01 (DB)~Voyage^MSCK000002~Operator^MSC~ISO Type^2200~Total Weight kg^5000");
            MTNControlBase.FindClickRowInTable(cargoComparisonForm.tblDetails,
                @"ID^JLG44063A01 (LL)~Voyage^MSCK000002~Operator^MSL~ISO Type^42G1~Total Weight kg^21885");*/
            cargoComparisonForm.TblDetails2.FindClickRow(new[]
            {
                "ID^JLG44063A01 (DB)~Voyage^MSCK000002~Operator^MSC~ISO Type^2200~Total Weight kg^5000",
                "ID^JLG44063A01 (LL)~Voyage^MSCK000002~Operator^MSL~ISO Type^42G1~Total Weight kg^21885"
            });

            cargoComparisonForm.DoUpdateFromLoadList();

            WarningErrorForm.CompleteWarningErrorForm("Warnings for Update From Cargo List");

            WarningErrorForm.CompleteWarningErrorForm("Warnings for Update From Cargo List");

            /*// Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(cargoComparisonForm.tblDetails,
                @"ID^JLG44063A01 (DB)~Voyage^MSCK000002~Operator^MSL~ISO Type^42G1~Total Weight kg^21885");
            MTNControlBase.FindClickRowInTable(cargoComparisonForm.tblDetails,
                @"ID^JLG44063A01 (LL)~Voyage^MSCK000002~Operator^MSL~ISO Type^42G1~Total Weight kg^21885");*/
            cargoComparisonForm.TblDetails2.FindClickRow(new[]
            {
                "ID^JLG44063A01 (DB)~Voyage^MSCK000002~Operator^MSL~ISO Type^42G1~Total Weight kg^21885",
                "ID^JLG44063A01 (LL)~Voyage^MSCK000002~Operator^MSL~ISO Type^42G1~Total Weight kg^21885"
            });
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = "_44063_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44063</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44063A01</id>\n      <isoType>42G1</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5101</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44063</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG44063A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Delete stops
            CreateDataFileToLoad(@"DeleteStops",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalStop xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalStop.xsd'>\n	<AllCargoUpdateRequest>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<CargoUpdateRequest Terminal='TT1'>\n			<directDelivery/>\n			<clearanceExpiryDate/>\n			<updateType/>\n			<billOfLading/>\n			<cargoRemarks/>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<deliveryReleaseNumber>1</deliveryReleaseNumber>\n			<description/>\n			<fullOrMT/>\n			<gradeCode/>\n			<id>JLG44063A01</id>\n			<messageMode>A</messageMode>\n			<stopAction>D</stopAction>\n			<stopCode>5</stopCode>\n			<subTerminalCode/>\n			<toDoAction/>\n			<toDoRemarks/>\n			<toDoTaskShort/>\n			<voyageCode/>\n		</CargoUpdateRequest>\n		<CargoUpdateRequest Terminal='TT1'>\n			<directDelivery/>\n			<clearanceExpiryDate/>\n			<updateType/>\n			<billOfLading/>\n			<cargoRemarks/>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<deliveryReleaseNumber/>\n			<description/>\n			<fullOrMT/>\n			<gradeCode/>\n			<id>JLG44063A01</id>\n			<messageMode>A</messageMode>\n			<stopAction>D</stopAction>\n			<stopCode>1</stopCode>\n			<subTerminalCode/>\n			<toDoAction/>\n			<toDoRemarks/>\n			<toDoTaskShort/>\n			<voyageCode/>\n		</CargoUpdateRequest>\n	</AllCargoUpdateRequest>\n</JMTInternalStop>\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
