using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Gate_Functions.Security_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39921 : MTNBase
    {

        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        RoadGateDetailsReleaseForm _roadGateDetailsReleaseForm;
        SecurityGateForm _securityGateForm;
        SecurityGateNewVisitForm _securityGateNewVisitForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_39921");

            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void ReceiveEmptyandTailerWithGenAttachedReleaseFull()
        {
            MTNInitialize();

            var departureDate = System.DateTime.Now.AddMinutes(15);
            // Tuesday, 22 April 2025 navmh5 Can be removed 6 months after specified date var strDepartureTime = date.ToString("HHmm");
            // Tuesday, 22 April 2025 navmh5 Can be removed 6 months after specified date var strDepartureDate = date.ToString("ddMMyyyy");

            //1 . go to security gate and receive truck
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Security Gate");
            _securityGateForm = new SecurityGateForm(vehicleId: @"39921");
            //securityGateForm.btnNew.DoClick();
            _securityGateForm.DoNew();
            _securityGateNewVisitForm = new SecurityGateNewVisitForm(@"New Vehicle Visit (Security Gate) TT1");
            _securityGateNewVisitForm.cmbBatNumber.SetValue(@"39921");
            _securityGateNewVisitForm.txtRegistration.SetValue(@"39921");
            _securityGateNewVisitForm.btnSave.DoClick();
            //securityGateNewVisitForm.CloseForm();

            //2. go to road gate and enter details of truck and trailer
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"39921");
            roadGateForm.GetTrailerFields();
            roadGateForm.txtRegistration.SetValue(@"39921");
            roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.txtTrailerID.SetValue(@"TRAILER39921A");
            roadGateForm.cmbTrailerType.SetValue(@"TRAILER39771");
            roadGateForm.cmbTrailerOperator.SetValue("ABOC", doDownArrow: true);
            roadGateForm.cmbImex.SetValue(@"Remain On Board");
            roadGateForm.btnReceiveEmpty.DoClick();


            //3. enter the details of the receival container 
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive Empty Container TT1");
            _roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO2230, additionalWaitTimeout: 2000, doDownArrow: true);
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG39921A01");
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("2000");
            _roadGateDetailsReceiveForm.CmbImex.SetValue(IMEX.Storage, additionalWaitTimeout: 2000, doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbDischargePort.SetValue(Port.LYTNZ, doDownArrow: true);
            _roadGateDetailsReceiveForm.BtnSave.DoClick();
            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            //4. check the gate items are correct
            roadGateForm.SetFocusToForm();
            /*// Monday, 7 April 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Chassis~Detail^TRAILER39921A");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Chassis~Detail^TRAILER39921A");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Empty~Detail^JLG39921A01");*/
            roadGateForm.TblGateItems.FindClickRow(new[]
            {
                "Type^Receive Chassis~Detail^TRAILER39921A", "Type^Release Chassis~Detail^TRAILER39921A",
                "Type^Receive Empty~Detail^JLG39921A01"
            });

            //5. also release a full container
            roadGateForm.btnReleaseFull.DoClick();
            _roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm(@"Release Full Container  TT1");
            _roadGateDetailsReleaseForm.TxtCargoId.SetValue(@"JLG39921A02");
            _roadGateDetailsReleaseForm.BtnSave.DoClick();

            //6. check the gate items are correct
            roadGateForm.SetFocusToForm();
            /*// Monday, 7 April 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Chassis~Detail^TRAILER39921A");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Chassis~Detail^TRAILER39921A");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Empty~Detail^JLG39921A01");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Full~Detail^JLG39921A02");*/
            roadGateForm.TblGateItems.FindClickRow(new[]
            {
                "Type^Receive Chassis~Detail^TRAILER39921A", "Type^Release Chassis~Detail^TRAILER39921A",
                "Type^Receive Empty~Detail^JLG39921A01", "Type^Release Full~Detail^JLG39921A02"
            });
            roadGateForm.btnSave.DoClick();
            
            // 06/03/2024 Removed due to change in 62445 with trailer / mafi validation
            /*warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();*/

            //7. In Road Ops check that there are 5 items in the yard to process for this truck (includes generator)
            //FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm();
            /*// Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^TRAILER39921A~Status^Remain On Board", rowHeight: 16);
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^TRAILER39921A~Status^Remain On Board", rowHeight: 16,findInstance:2);
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^GEN39921~Status^Remain On Board", rowHeight: 16);
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG39921A01~Status^Storage", rowHeight: 16);
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG39921A02~Status^Storage", rowHeight: 16);*/
            roadOperationsForm.TblYard2.FindClickRow(new[]
            {
                "Cargo ID^TRAILER39921A~Status^Remain On Board", "Cargo ID^GEN39921~Status^Remain On Board",
                "Cargo ID^JLG39921A01~Status^Storage", "Cargo ID^JLG39921A02~Status^Storage"
            });
            roadOperationsForm.TblYard2.FindClickRow(new[]
            { "Cargo ID^TRAILER39921A~Status^Remain On Board", }, rowInstance: 2);

            //8. Move the empty to yard and the full to the truck and process road exit
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTableMulti(roadOperationsForm.tblYard,
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date     new[] { "Cargo ID^JLG39921A01~Status^Storage", "Cargo ID^JLG39921A02~Status^Storage" },
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date     ClickType.ContextClick, rowHeight: 16);
            // Tuesday, 22 April 2025 navmh5 Can be removed 6 months after specified date roadOperationsForm.TblYard2.FindClickRow(
            // Tuesday, 22 April 2025 navmh5 Can be removed 6 months after specified date     new[] { "Cargo ID^JLG39921A01~Status^Storage", "Cargo ID^JLG39921A02~Status^Storage" },
            // Tuesday, 22 April 2025 navmh5 Can be removed 6 months after specified date     ClickType.ContextClick, multiSelect: true);
            // Tuesday, 22 April 2025 navmh5 Can be removed 6 months after specified date roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");
            roadOperationsForm.MoveQueuedAllCargo(["39921"]);
            

            roadOperationsForm.ClickOnVehicle(@"39921 (3/3) - ICA - Yard Interchange");
            Mouse.RightClick();
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");

            _securityGateForm.SetFocusToForm();
            _securityGateForm.ProcessDeparture("39921", departureDate);
            
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            
            searchFor = "_39921_";

            // Cargo on Site Delete
            CreateDataFileToLoad(@"CargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG39921A01</id>\n         <isoType>2230</isoType>\n         <voyageCode>MSCK000002</voyageCode>\n         <operatorCode>MSC</operatorCode>\n         <dischargePort>NZAKL</dischargePort>\n         <locationId>MKBS01</locationId>\n         <weight>5500.0000</weight>\n         <imexStatus>Storage</imexStatus>\n         <totalQuantity>1</totalQuantity>\n         <commodity>APPL</commodity>\n		 <messageMode>D</messageMode>\n      </CargoOnSite>\n	  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG39921A02</id>\n         <isoType>2230</isoType>\n         <voyageCode>MSCK000002</voyageCode>\n         <operatorCode>MSL</operatorCode>\n         <dischargePort>USJAX</dischargePort>\n         <locationId>MKBS01</locationId>\n         <weight>5500.0000</weight>\n         <imexStatus>Storage</imexStatus>\n         <totalQuantity>1</totalQuantity>\n         <commodity>ICEC</commodity>\n		 <temperature>24</temperature>\n		 <messageMode>A</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

         
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
