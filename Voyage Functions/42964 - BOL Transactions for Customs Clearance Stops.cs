using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNGlobal;
using MTNGlobal.EnumsStructs;


namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase42964 : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() { base.TestCleanup(); }

        void MTNInitialize()
        {
            searchFor = @"_42964_";
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void BOLTransactionsForCustomsClearanceStops()
        {
            MTNInitialize();

            //Step 4
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage BOL Maintenance");
            VoyageBOLMaintenanceForm voyageBOLMaintenanceForm = new VoyageBOLMaintenanceForm();
            voyageBOLMaintenanceForm.GetSearcher();
            //voyageBOLMaintenanceForm.SetValue(voyageBOLMaintenanceForm.cmbVoyage, @"MSCK000002");
            voyageBOLMaintenanceForm.cmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true, searchSubStringTo: 10); 
            //voyageBOLMaintenanceForm.btnFind.DoClick();
            voyageBOLMaintenanceForm.DoFind();

            //Step 5
           // voyageBOLMaintenanceForm.BillsofLading();
            //MTNControlBase.FindClickRowInTable(voyageBOLMaintenanceForm.tblBillsofLading, @"Bill of Lading^BOL42964", ClickType.ContextClick) ;
            voyageBOLMaintenanceForm.TblBillsOfLading.FindClickRow(new[] { "Bill of Lading^BOL42964" }, ClickType.ContextClick);
            voyageBOLMaintenanceForm.ContextMenuSelect(@"Edit Stops...");

            //Step 6
            
            StopsForm stopsForm = new StopsForm(@"BOL42964 TT1");
            // MTNControlBase.FindClickRowInTable(stopsForm.tblStops, @"Stop^BOL Customs Stop_42964");
            stopsForm.TblStops.FindClickRow(["Stop^BOL Customs Stop_42964"]);
            stopsForm.btnSaveAndClose.DoClick();

            //Step 7
            voyageBOLMaintenanceForm.SetFocusToForm();
            //MTNControlBase.FindClickRowInTable(voyageBOLMaintenanceForm.tblBillsofLading, @"Bill of Lading^BOL42964", ClickType.ContextClick);
            voyageBOLMaintenanceForm.TblBillsOfLading.FindClickRow(new[] { "Bill of Lading^BOL42964" }, ClickType.ContextClick);
            voyageBOLMaintenanceForm.ContextMenuSelect(@"Edit Stops...");

            //Step 8
            stopsForm = new StopsForm(@"BOL42964 TT1");
            // MTNControlBase.FindClickRowInTable(stopsForm.tblAddStops, @"Stop^BOL Customs Stop_42964");
            stopsForm.TblAddStops.FindClickRow(["Stop^BOL Customs Stop_42964"]);
            stopsForm.btnSaveAndClose.DoClick();


            //Step 9
            voyageBOLMaintenanceForm.SetFocusToForm();
            //MTNControlBase.FindClickRowInTable(voyageBOLMaintenanceForm.tblBillsofLading, @"Bill of Lading^BOL42964", ClickType.ContextClick);
            voyageBOLMaintenanceForm.TblBillsOfLading.FindClickRow(new[] { "Bill of Lading^BOL42964" }, ClickType.ContextClick);
            voyageBOLMaintenanceForm.ContextMenuSelect(@"Transactions...");

            
            //Step 10
            string[] transactionsToValidate =
          {
                @"Type^BOL Stop Created~Details^Stop Set: BOL Customs Stop_42964",
                @"Type^BOL Stop Cleared~Details^Stop Cleared: BOL Customs Stop_42964",
                @"Type^BOL Stop Created~Details^Stop Set: BOL Customs Stop_42964"

           };
            CargoEnquiryTransactionForm.OpenAndValidateTransactions(@"BOL Transactions for BOL42964 TT1",
                transactionsToValidate);



        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Ship
            CreateDataFileToLoad(@"BOLDelete.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalBOL \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalBOL.xsd'>\n   <AllBOLHeader>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <BOLHeader Terminal='TT1'>\n                  <id>BOL42964</id>\n                    <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSK</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n                 <messageMode>D</messageMode>\n        <AllBOLDetails>\n          <BOLDetails>\n   <messageMode>D</messageMode>\n   <cargoTypeDescr>ISO Container</cargoTypeDescr>\n   <commodity>GEN</commodity>\n   </BOLDetails>\n       </AllBOLDetails>\n       </BOLHeader>\n     </AllBOLHeader>\n </JMTInternalBOL>\n\n\n\n");


            CreateDataFileToLoad(@"BOLAdd.xml",
               "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalBOL \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalBOL.xsd'>\n   <AllBOLHeader>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <BOLHeader Terminal='TT1'>\n                  <id>BOL42964</id>\n                    <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSK</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n                 <messageMode>A</messageMode>\n        <AllBOLDetails>\n          <BOLDetails>\n   <messageMode>A</messageMode>\n   <cargoTypeDescr>ISO Container</cargoTypeDescr>\n   <commodity>GEN</commodity>\n   </BOLDetails>\n       </AllBOLDetails>\n       </BOLHeader>\n     </AllBOLHeader>\n </JMTInternalBOL>\n\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
