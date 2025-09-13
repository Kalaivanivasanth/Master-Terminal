using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using MTNUtilityClasses.Classes;
using System;
using System.Drawing;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using FlaUI.Core.Input;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions.Planning
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase53726 : MTNBase
    {
        LOLOPlanningForm _loloPlanning;
        LoadListForm _loadlistForm;
        RoadOperationsForm _roadOperationsForm;
        RoadGateDetailsReceiveForm receiveFullContainerForm;

        private const string TestCaseNumber = @"53726";
        private const string vehicleId = @"53726";


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() { base.TestCleanup(); }


        [TestMethod]
        public void RetainVesselPlanningAfterReceivingAPrenotedCargo()
        {
            MTNInitialize();

            // Open LOLO Planning, select the slot in the Planned mode
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|LOLO Planning");
            _loloPlanning = new LOLOPlanningForm();

            _loloPlanning.SelectBaySelectionOption(@"Select Multiple Holds");
            _loloPlanning.SelectMultipleBaysHolds();
            _loloPlanning.cmbBHVoyage.SetValue(@"VOY_53726 MSCK KATYA R."/*, doDownArrow: true*/);
            /*string[] detailsToMove =
            {
                 @"Hold 12 - Bays 45, 47"
             };*/
            _loloPlanning.lstBaysHolds.MoveItemsBetweenList(_loloPlanning.lstBaysHolds.LstLeft, new [] { "Hold 12 - Bays 45, 47"  });
            _loloPlanning.btnBHShow.DoClick();

            // Load Colour Code
            _loloPlanning.btnDisplayLoadColourCode.DoClick();
            _loloPlanning.GetLoadDetails();
            _loloPlanning.btnLoadPlanningMode.DoClick();

            //Get the clickpoint
            Point clickPoint = new Point(_loloPlanning.grpBays.BoundingRectangle.X + 50,
                _loloPlanning.grpBays.BoundingRectangle.Y + 140);
            Mouse.Click(clickPoint);

            // Open Load List
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|Load List", forceReset: true);
            _loadlistForm = new LoadListForm(@"Load List TT1");

            _loadlistForm.SetFocusToForm();
            _loadlistForm.searchPanel.Focus();
            _loadlistForm.GetSearcherVoyageTab();
            _loadlistForm.cmbVoyage.SetValue(TT1.Voyage.VOY_53726, doDownArrow: true);

            //_loadlistForm.btnFind.DoClick();
            _loadlistForm.DoFind();
            // Check the container is in the Non Planned tab and Plan Load to VOY_53726
            _loadlistForm.NonPlannedTab();
            // MTNControlBase.FindClickRowInTable(_loadlistForm.tblNonPlanned, @"ID^JLG53726A01", ClickType.ContextClick);
            _loadlistForm.TblNonPlanned.FindClickRow(["ID^JLG53726A01"], ClickType.ContextClick);
            _loadlistForm.ContextMenuSelect(@"Plan Load To VOY_53726 451282");

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Move Error TT1");
            warningErrorForm.btnSave.DoClick();

            _loadlistForm.SetFocusToForm();

            // Go To Planned tab and check if the container is in the Planned tab
            _loadlistForm.PlannedTab(@"Planned (1)");
            // MTNControlBase.FindClickRowInTable(_loadlistForm.tblPlanned, @"ID^JLG53726A01~Planned Location^451282", ClickType.Click);
            _loadlistForm.TblPlanned.FindClickRow(["ID^JLG53726A01~Planned Location^451282"], ClickType.Click);

            // Receive the container via gate
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"53726");

            roadGateForm.txtRegistration.SetValue(@"53726");
            roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.txtNewItem.SetValue(@"JLG53726A01", 10);

            receiveFullContainerForm = new RoadGateDetailsReceiveForm(@"Receive Full Container TT1");
            receiveFullContainerForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();

            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            //warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            //warningErrorForm.btnSave.DoClick();

            _loadlistForm.SetFocusToForm();
            // Check the container is still Planned
            _loadlistForm.PlannedTab(@"Planned (1)");
            // MTNControlBase.FindClickRowInTable(_loadlistForm.tblPlanned, @"ID^JLG53726A01~Planned Location^451282", ClickType.Click);
            _loadlistForm.TblPlanned.FindClickRow(["ID^JLG53726A01~Planned Location^451282"], ClickType.Click);

            // Delete the Vehicle Entry on the Road Operations
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            _roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_roadOperationsForm.tblYard, @"Vehicle Id^" + vehicleId +  @"~Cargo ID^" + @"JLG53726A01", ClickType.ContextClick, rowHeight: 16);
            _roadOperationsForm.TblYard2.FindClickRow(new [] { $"Vehicle Id^{vehicleId}~Cargo ID^JLG53726A01" }, ClickType.ContextClick);
            _roadOperationsForm.DeleteCurrentEntry(@"TESTING");
        }

        private void MTNInitialize()
        {
            searchFor = @"_" + TestCaseNumber + "_";
            CallJadeScriptToRun(TestContext, @"resetData_53726");
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Create Prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0'?>\n<JMTInternalPrenote>\n<AllPrenote>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n<Prenote Terminal='TT1'>\n	<cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG53726A01</id>\n            <isoType>2200</isoType>\n            <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000</weight>\n            <voyageCode>VOY_53726</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n			</Prenote>\n		</AllPrenote>\n</JMTInternalPrenote>\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }

}

