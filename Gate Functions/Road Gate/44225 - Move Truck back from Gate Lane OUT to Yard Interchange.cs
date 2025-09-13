using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44225 : MTNBase
    {

        RoadGateDetailsReleaseForm _roadGateDetailsReleaseForm;
        RoadOperationsMoveVehicleForm _roadOperationsMoveVehicleForm;

        private const string TestCaseNumber = @"44225";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
       
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        void MTNInitialize()
        {
            /*CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);
            SetupAndLoadInitializeData(TestContext);*/
            LogInto<MTNLogInOutBO>("USER44225");
        }


        [TestMethod]
        public void MoveTruckBackFromGateLaneOutToYardInterchange()
        {
            MTNInitialize();

            //1. Open Road Gate and enter truck details and release cargo
            /*FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            //roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"44225");
            roadGateForm = FormObjectBase.CreateForm<RoadGateForm>();
          //  roadGateForm.GetTrailerFields();
            roadGateForm.SetRegoCarrierGate("44225");
            roadGateForm.btnReleaseCargo.DoClick();

            //2. Enter cargo to release and complete road gate
            _roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm(@"Release General Cargo  TT1");
            _roadGateDetailsReleaseForm.CmbCargoType.SetValue(CargoType.BagOfSand, doDownArrow: true, searchSubStringTo: CargoType.BagOfSand.Length - 3);
            _roadGateDetailsReleaseForm.TxtCargoId.SetValue(@"JLG44225A01");
            _roadGateDetailsReleaseForm.BtnSave.DoClick();

            /*warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            string[] warningErrorsToCheck = new string[]
            {
                @"Code :79688. Cargo JLG44225A01's availability status of Unavailable for Release does not match the request's availability status of Available for release. Has Stops."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorsToCheck);
            warningErrorForm.btnSave.DoClick();#1#
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out",
                new[]
                {
                    "Code :79688. Cargo JLG44225A01's availability status of Unavailable for Release does not match the request's availability status of Available for release. Has Stops."
                });

            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();*/

            //3. In road operations, move cargo and then move truck to POUT lane 3
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm();
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date roadOperationsForm.tblYard1.FindClickRow($"Cargo ID^JLG44225A01",
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date     ClickType.ContextClick);
            MTNControlBase.FindClickRowInTable(roadOperationsForm.TblYard2.GetElement(), @"Cargo ID^JLG44225A01", searchType: SearchType.Exact, clickType: ClickType.ContextClick, rowHeight: 16);
            //roadOperationsForm.TblYard2.FindClickRow(new[] { "Cargo ID^JLG44225A01" },
            //ClickType.ContextClick);
            roadOperationsForm.contextMenu.MenuSelect(RoadOperationsForm.ContextMenuItems.YardMoveSelected);
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date roadOperationsForm.tblYard1.FindClickRow($"Cargo ID^JLG44225A01",
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date     ClickType.ContextClick);
            MTNControlBase.FindClickRowInTable(roadOperationsForm.TblYard2.GetElement(), @"Cargo ID^JLG44225A01", searchType: SearchType.Exact, clickType: ClickType.ContextClick, rowHeight: 16);
            //roadOperationsForm.TblYard2.FindClickRow(new[] { "Cargo ID^JLG44225A01" },
                //ClickType.ContextClick);
            roadOperationsForm.contextMenu.MenuSelect(RoadOperationsForm.ContextMenuItems.YardProceedToPOUTLane4);

            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date
            MTNControlBase.FindClickRowInTable(roadOperationsForm.TblGateLanesOut.GetElement(), @"Cargo ID^JLG44225A01", rowHeight: 16);
            //roadOperationsForm.TblGateLanesOut.FindClickRow(new[] { "Cargo ID^JLG44225A01" });

            //4. Move the truck back to the yard interchange area
            roadOperationsForm.ClickOnVehicle(@"44225 (0/1) - POUT - Gate Road Area");
            Mouse.RightClick();
            roadOperationsForm.ContextMenuSelect(@"Move Vehicle...", @"ListContext");
            _roadOperationsMoveVehicleForm = new RoadOperationsMoveVehicleForm(@"Move Vehicle TT1");
            _roadOperationsMoveVehicleForm.cmbNewLocation.SetValue(@"VTA");
            _roadOperationsMoveVehicleForm.btnOK.DoClick();

            //5. check that the truck has been moved back to yard interchange area and then complete gate out, check the vehicle is not in any tables
            roadOperationsForm.SetFocusToForm();
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date bool rowFound = MTNControlBase.FindClickRowInTable(roadOperationsForm.tblGateLanesOut, @"Cargo ID^JLG44225A01", rowHeight: 16, doAssert: false, clickType: ClickType.None);
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date Assert.IsTrue(!(rowFound),@"Vehicle found in Gate Out Lanes Table. it should not longer be there");
            var rowFound = roadOperationsForm.TblGateLanesOut.FindClickRow(new[] { "Cargo ID^JLG44225A01" }, ClickType.None, doAssert: false);
             Assert.IsTrue(!string.IsNullOrEmpty(rowFound), "Vehicle found in Gate Out Lanes Table. It should not longer be there");

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG44225A01", clickType: ClickType.ContextClick, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(roadOperationsForm.TblYard2.GetElement(), @"Cargo ID^JLG44225A01", searchType: SearchType.Exact, clickType: ClickType.ContextClick, rowHeight: 16);
            //roadOperationsForm.TblYard2.FindClickRow(new[] { "Cargo ID^JLG44225A01" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date rowFound = MTNControlBase.FindClickRowInTable(roadOperationsForm.tblGateLanesOut, @"Cargo ID^JLG44225A01", rowHeight: 16, doAssert: false, clickType: ClickType.None);
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date Assert.IsTrue(!(rowFound), @"Vehicle found in Gate Out Lanes Table. It should not longer be there");
            rowFound = roadOperationsForm.TblGateLanesOut.FindClickRow(new[] { "Cargo ID^JLG44225A01" }, ClickType.None, doAssert: false);
            Assert.IsTrue(!string.IsNullOrEmpty(rowFound), "Vehicle found in Gate Out Lanes Table. It should not longer be there");

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date rowFound = MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG44225A01", rowHeight: 16, doAssert: false, clickType: ClickType.None);
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date Assert.IsTrue(!(rowFound), @"Vehicle found in Yard Table. it should not longer be there");
            rowFound = roadOperationsForm.TblYard2.FindClickRow(new[] {"Cargo ID^JLG44225A01"}, doAssert: false, clickType: ClickType.None);
            Assert.IsTrue(!string.IsNullOrEmpty(rowFound), "Vehicle found in Gate Out Lanes Table. It should not longer be there");
            
        }


        

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            
            searchFor = @"_" + TestCaseNumber + "_";
            
            // Cargo on Site Delete
            CreateDataFileToLoad(@"CargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>44225</TestCases>\n      <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n      <product>SMSAND</product>\n      <id>JLG44225A01</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>GCARA1 SMALL_SAND</locationId>\n      <weight>1000.0000</weight>\n	  <tareWeight>600.0000</tareWeight>\n      <imexStatus>Export</imexStatus>\n      <dischargePort>NZNPE</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <totalQuantity>100</totalQuantity>\n      <commodity>SANC</commodity>\n	  <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

         
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
