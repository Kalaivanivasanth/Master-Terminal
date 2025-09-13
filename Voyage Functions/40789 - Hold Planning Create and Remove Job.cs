using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Message_Dialogs;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40789 : MTNBase
    {
        HoldPlanningForm _holdPlanningForm;
        ConfirmationFormYesNo _confirmationFormYesNo;

        private const string TestCaseNumber = @"40789";
        private readonly string[] _cargoId =
        {
            @"JLG" + TestCaseNumber + @"A01",
            @"JLG" + TestCaseNumber + @"A02"
        };

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, $"resetData_{TestCaseNumber}");
            
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void HoldPlanningRemoveSingleCargoFromJob()
        {
            MTNInitialize();

            // 1. set dates for the test
            var dateToday = DateTime.Today.Date;
            var dateTomorrow = dateToday.AddDays(1);
            

            // 2. Navigate to Hold Planning
            FormObjectBase.MainForm.OpenHoldPlanningFromToolbar();
            _holdPlanningForm = new HoldPlanningForm();

            // 3. Get hold planning details for MSCK000002 and tab to Unallocated cargo on vessel
            _holdPlanningForm.cmbVoyage.SetValue(TT1.Voyage.MSCK000002FullSpace);
            _holdPlanningForm.GetTabTableGeneric(@"Unallocated Cargo On Vessel", @"2027");

           // 4. multi-select 2 containers and add to new job
            
            MTNControlBase.FindClickRowInTableMulti(_holdPlanningForm.tblGeneric,
                new[] { $"ID^{_cargoId[1]}~Cargo Type^ISO Container", $"ID^{_cargoId[0]}~Cargo Type^ISO Container" },
                rowHeight: 16);
            
            _holdPlanningForm.DoNewHoldPlanning();
            _holdPlanningForm.cmbWorkPoint.SetValue("WP0001 - Work point WP0001 description", searchSubStringTo: 6, additionalWaitTimeout:200, doDownArrow: true);
            _holdPlanningForm.txtJobId.SetValue($"{TestCaseNumber}JOB");
            _holdPlanningForm.cmbTransferType.SetValue(@"LIFT ON LIFT OFF (LOLO)", searchSubStringTo: 4, additionalWaitTimeout: 200, doDownArrow: true);
            _holdPlanningForm.txtShiftStartDate.SetValue(dateToday.ToString(@"ddMMyyyy"));
            _holdPlanningForm.txtShiftStartTime.SetValue(@"1200");
            _holdPlanningForm.txtShiftEndDate.SetValue(dateTomorrow.ToString(@"ddMMyyyy"));
            _holdPlanningForm.txtShiftEndTime.SetValue(@"1200");
            _holdPlanningForm.cmbPlanDischargeTo.SetValue(@"BS4078", searchSubStringTo: 3, additionalWaitTimeout: 200, doDownArrow: true);
            _holdPlanningForm.DoSaveHoldPlanning();

            // 5. go to the new job tab and find the cargo - remove 1 cargo from the job
            _holdPlanningForm.GetTabTableGeneric($"{TestCaseNumber}JOB Discharge");
            _holdPlanningForm.DoEditHoldPlanning();
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric, $"ID^{_cargoId[1]}", rowHeight: 16);
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric, $"ID^{_cargoId[0]}", ClickType.ContextClick, rowHeight: 16);
             _holdPlanningForm.TabGeneric.TableWithHeader.FindClickRow(
             [$"ID^{_cargoId[1]}"], ClickType.None);
             _holdPlanningForm.TabGeneric.TableWithHeader.FindClickRow(
                 [$"ID^{_cargoId[0]}"], ClickType.ContextClick);
;           
;            _holdPlanningForm.ContextMenuSelect(@"Remove From Job");
            _holdPlanningForm.DoSaveHoldPlanning();


            // 6. go to the new job tab delete the job
            _holdPlanningForm.GetForm().FocusNative();
            Miscellaneous.WaitForSeconds(2); // Increase delay to ensure UI is fully ready
            _holdPlanningForm.GetTabTableGeneric($"{TestCaseNumber}JOB Discharge");

            // Ensure the UI is ready and form has focus before deleting
            _holdPlanningForm.GetForm().FocusNative();
            Miscellaneous.WaitForSeconds(2); // Increase delay to ensure UI is fully ready
            //_holdPlanningForm.btnDeleteHoldPlanning.DoClick();
            _holdPlanningForm.DoDeleteHoldPlanning();
            _confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Confirm Job Delete");
            _confirmationFormYesNo.btnYes.DoClick();
            
            
            _holdPlanningForm.GetTabTableGeneric(@"Unallocated Cargo On Vessel", @"2027");
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric, $"ID^{_cargoId[0]}", rowHeight: 16);
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric, $"ID^{_cargoId[1]}", rowHeight: 16);
             _holdPlanningForm.TabGeneric.TableWithHeader.FindClickRow(
              [$"ID^{_cargoId[0]}", $"ID^{_cargoId[1]}"]);
;
;
        }
           

            private static void SetupAndLoadInitializeData(TestContext testContext)
            {
               fileOrder = 1;
               searchFor = "_40789_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40789A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <weight>6000.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GENL</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnShip>\n         <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40789A02</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <weight>6000.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GENL</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnShip>\n    </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40789A01</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <weight>6000.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GENL</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n         <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40789A02</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <weight>6000.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>GENL</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n    </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
            }



        }

    }
