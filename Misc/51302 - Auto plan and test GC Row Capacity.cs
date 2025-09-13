using DataObjects;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Gate_Functions.Vehicle;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Misc
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase51302 : MTNBase
    {
        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        const string VehicleId = @"51302";
        
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
            CallJadeScriptToRun(TestContext, @"resetData_51302");
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void AutoPlanAndTestGCRowCapacity()
        {
            MTNInitialize();
            
            // 22/01/2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1");

            // Step 1 - 6
            GateInCargo(VehicleId, CargoType.Metal, @"20000", @"Export", @"JLG51302A01", @"20");

            // 22/01/2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");

            // Step 7 - 8
            ValidateQueuedLocation(VehicleId, @"GCAR1 01-03", 15);
            ValidateQueuedLocation(VehicleId, @"GCAR1 08-10", 5);

            // Step 9 cancel visit
            VehicleVisitForm.CancelVehicleVisit(VehicleId, TestRunDO.GetInstance().TerminalId, $"Finished with vehicle: {VehicleId}");

            // Step 10 - 13
            GateInCargo(VehicleId + @"A", CargoType.CPODANTURUNAN, @"15", @"Export", @"JLG51302B01");

            // Step  14 - 15
            ValidateQueuedLocation(VehicleId + @"A", @"GCAR2 01-03", 1, @"10.000 MT");
            ValidateQueuedLocation(VehicleId + @"A", @"GCAR2 08-10", 1, @"5.000 MT");

            // Step 16 cancel visit
            roadOperationsForm.SetFocusToForm();
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^" + VehicleId + @"A" + @"~Queued To^" + @"GCAR2 01-03" + @"~Move Amount^" + @"10.000 MT", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[]
                { $"Vehicle Id^{VehicleId}A~Queued To^GCAR2 01-03~Move Amount^10.000 MT" }, ClickType.ContextClick);
            roadOperationsForm.DeleteCurrentEntry(@"TESTING");

            
            // Step 17 - 20
            GateInCargo(VehicleId + @"B", CargoType.Metal, @"20000", @"Import", @"JLG51302C01", @"20");

            // Step 21 - 22
            ValidateQueuedLocation(VehicleId + @"B", @"GCAR3 01-03", 10);
            ValidateQueuedLocation(VehicleId + @"B", @"GCAR3 06-10", 10);

            // Step 23 cancel visit
            VehicleVisitForm.CancelVehicleVisit(VehicleId + @"B", TestRunDO.GetInstance().TerminalId, $"Finished with vehicle: {VehicleId}B");


            // Step 24 - 27
            GateInCargo(VehicleId + @"C", CargoType.GeneralCargo, @"15000", @"Export", @"JLG51302D01", totalQuantity: @"15");

            // Step 28 - 29
            ValidateQueuedLocation(VehicleId + @"C", @"GCAR4 01-03", moveAmount: @"10");
            ValidateQueuedLocation(VehicleId + @"C", @"GCAR4 08-10", moveAmount: @"5");

            // Step 30 cancel visit
            roadOperationsForm.SetFocusToForm();
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^" + VehicleId + @"C" + @"~Queued To^" + @"GCAR4 01-03" + @"~Move Amount^" + @"10", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[]
                { $"Vehicle Id^{VehicleId}C~Queued To^GCAR4 01-03~Move Amount^10" }, ClickType.ContextClick);
            roadOperationsForm.DeleteCurrentEntry(@"TESTING");
        }

        void GateInCargo(string vehicleId, string cargoType, string totalWeight, string imex, string cargoId, string itemsNumber = @"", string totalQuantity = @"")
        {
            roadGateForm.SetFocusToForm();
            roadGateForm.SetRegoCarrierGate(vehicleId);
            roadGateForm.btnReceiveCargo.DoClick();


            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive General Cargo TT1");
            _roadGateDetailsReceiveForm.CmbCargoType.SetValue(cargoType, doDownArrow: true, searchSubStringTo: cargoType.Length < 7 ? cargoType.Length - 1 : 7);
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType(totalWeight);
            _roadGateDetailsReceiveForm.CmbImex.SetValue(imex, additionalWaitTimeout: 2000, doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbDischargePort.SetValue(Port.LYTNZ, doDownArrow: true);

           // 22/01/0225 navmh5 if (itemsNumber == @"")
           if(string.IsNullOrEmpty(itemsNumber))
                _roadGateDetailsReceiveForm.TxtCargoId.SetValue(cargoId);
            else
                _roadGateDetailsReceiveForm.AddMultipleCargoIds(cargoId, itemsNumber);

            // 22/01/2025 navmh5if (totalQuantity != @"")
            if(!string.IsNullOrEmpty(totalQuantity))
                _roadGateDetailsReceiveForm.TxtTotalQuantity.SetValue(totalQuantity);
            
            Wait.UntilResponsive(_roadGateDetailsReceiveForm.BtnSave.GetElement());

            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            try
            {
                warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
                warningErrorForm.btnSave.DoClick();
                Wait.UntilResponsive(roadGateForm.btnSave.GetElement());
            }
            catch { }
            
            roadGateForm.btnSave.DoClick();
        }

        void ValidateQueuedLocation(string vehicleId, string queuedTo, int count = 1, string moveAmount = @"1")
        {
            roadOperationsForm.SetFocusToForm();
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date  MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard,
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date      @"RT^Q~Vehicle Id^" + vehicleId + @"~Queued To^" + queuedTo + @"~Move Amount^" + moveAmount,
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date      ClickType.None, rowHeight: 16, findInstance: count);
            roadOperationsForm.TblYard2.FindClickRow(new[]
                { $"Vehicle Id^{vehicleId}~Queued To^{queuedTo}~Move Amount^{moveAmount}" }, ClickType.None, rowInstance: count);
        }
    }
}
