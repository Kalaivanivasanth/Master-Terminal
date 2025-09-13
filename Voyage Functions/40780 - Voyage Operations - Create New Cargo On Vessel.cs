using DataObjects.LogInOutBO;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40780 : MTNBase
    {
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void CreateNewCargoOnVessel()
        {
            MTNInitialize();

            // Step 4
            FormObjectBase.MainForm.OpenVoyageOperationsFromToolbar();
            var voyageOperationsForm = new VoyageOperationsForm();
            voyageOperationsForm.GetSearcherTab_ClassicMode();
            voyageOperationsForm.GetSearcherTab();

            // Step 5
            voyageOperationsForm.ChkRoroDecks.DoClick();
            voyageOperationsForm.CmbVoyage.SetValue(TT3.Voyage.GRIG001, doDownArrow: true);
            voyageOperationsForm.DoSelect();
            voyageOperationsForm.GetMainDetails();

            // Step 6
            Mouse.RightClick();
            voyageOperationsForm.ContextMenuSelect(@"Cargo|Create New Cargo On Vessel...");

            var roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Add Cargo TT1");

            // Step 7
            roadGateDetailsReceiveForm.CmbCargoType.SetValue(CargoType.ISOContainer, doDownArrow: true, searchSubStringTo: CargoType.ISOContainer.Length - 1);
            roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG40780A01");
            roadGateDetailsReceiveForm.CmbCommodity.SetValue(Commodity.GEN, doDownArrow: true);
            roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("2000");
            roadGateDetailsReceiveForm.ChkIsOOTL.DoClick();
            roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            roadGateDetailsReceiveForm.BtnSave.DoClick();

            // Step 8
            var confirmationFormOK = new ConfirmationFormOK(@"Error", ControlType.Window, 
                automationIdMessage: @"3", automationIdOK: @"4");
            confirmationFormOK.CheckMessageMatch(@"Error Code :90909. General Cargo and OOTL Containers Require a Default X/Y Coordinate " +
                "in Metres to Draw a Default Rectangle");
           confirmationFormOK.btnOK.DoClick();

            // Step 9
            roadGateDetailsReceiveForm.TxtLocation.SetValue(TT1.TerminalArea.GRIG001);
            roadGateDetailsReceiveForm.ChkIsOOTL.DoClick(false);
            roadGateDetailsReceiveForm.BtnSave.DoClick();

            // Step 10
            /*// Monday, 27 January 2025 navmh5 
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Pre-Notes TT1");
            warningErrorForm.CheckWarningsErrorsExist(new[] { "Code :75016. The Container Id (JLG40780A01) failed the validity checks and may be incorrect." });
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Pre-Notes TT1",
                new[]
                {
                    "Code :75016. The Container Id (JLG40780A01) failed the validity checks and may be incorrect."
                });

            voyageOperationsForm.TblOnVessel1.FindClickRow(
                new[] { @"ID^JLG40780A01~Location^GRIG001~Total Quantity^1~Cargo Type^ISO Container~ISO Type^2200" });
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_40780_";

            // Delete Cargo On Ship
            CreateDataFileToLoad(@"DeleteCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40780A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>GRIG001</voyageCode>\n            <operatorCode>MSC</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <weight>6000.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GEN</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnShip>\n    </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n\n\n");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
