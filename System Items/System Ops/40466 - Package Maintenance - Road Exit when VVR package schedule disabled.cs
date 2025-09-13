using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40466 : MTNBase
    {

        const string TestCaseNumber = @"40466";
        const string CargoId = @"JLG" + TestCaseNumber + "A01";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void PackageMaintRoadExitVVRPackageScheduleDisabled()
        {
            MTNInitialize();
            
            // Step 4 - 7
            FormObjectBase.NavigationMenuSelection(@"System Ops|Package Maintenance");
            PackageMaintenanceForm packageMaintenanceForm = new PackageMaintenanceForm(@"Package Maintenance");
            // MTNControlBase.FindClickRowInTable(packageMaintenanceForm.tblData,
                // @"Code^Package-40466~Description^Package-40466~Type^Vehicle Visit Report Package");
            packageMaintenanceForm.TblData.FindClickRow(["Code^Package-40466~Description^Package-40466~Type^Vehicle Visit Report Package"]);
            packageMaintenanceForm.DoEdit();

            PackageConfigurationForm packageConfigurationForm = new PackageConfigurationForm(@"Package Configuration");

            packageConfigurationForm.MoveToSelectedTerminals(new [] { "Test Terminal 1 - Non Cash" });
            //packageConfigurationForm.btnSave.DoClick();
            packageConfigurationForm.DoSave();

            packageMaintenanceForm.SetFocusToForm();
            packageMaintenanceForm.CloseForm();

            // Step 8 - 12
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(vehicleId: TestCaseNumber);
            
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);
            roadGateForm.btnReceiveEmpty.DoClick();

            RoadGateDetailsReceiveForm roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Empty Container TT1");
            roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            roadGateDetailsReceiveForm.TxtCargoId.SetValue(CargoId, additionalWaitTimeout: 30);
            roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("6000");
            roadGateDetailsReceiveForm.CmbImex.SetValue(IMEX.Storage, doDownArrow: true); //, 2000);
            roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.ABOC, doDownArrow: true);
            roadGateDetailsReceiveForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();

            // Step 13 - 16
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(@"Road Operations TT1", new [] {TestCaseNumber });

            // Step 17 - 18
            FormObjectBase.NavigationMenuSelection(@"General Functions | System Alerts", forceReset: true);
            SystemAlertsForm systemAlertsForm = new SystemAlertsForm();

            Console.WriteLine("date: " + loadFileDeleteStartTime.ToString("dd/MM/yyyy"));
            // MTNControlBase.FindClickRowInTable(systemAlertsForm.TblDetails,
                // @"Terminal^TT1~Detail^(runEventScheduleVVR) - Gate Out has occurred, but there is no active VVR Package task",
                 // searchType: SearchType.Contains);
            systemAlertsForm.TblDetails1.FindClickRow(
            [
                "Terminal^TT1~Detail^(runEventScheduleVVR) - Gate Out has occurred, but there is no active VVR Package task"
            ], searchType: SearchType.Contains);


            // Step 19 - 22 : Since this is the only test that calls this, it is reset in the resetTermConfig script

        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber + "_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <TestCases>40466</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG40466A01</id>\n         <isoType>2200</isoType>\n         <operatorCode>ABOC</operatorCode>\n         <locationId></locationId>\n         <weight>6000</weight>\n         <imexStatus>Storage</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode></voyageCode>\n         <dischargePort></dischargePort>\n		 <messageMode>D</messageMode>\n      </CargoOnSite>\n	</AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }

}
