using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General.Future_Storage
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43689 : MTNBase
    {
        string _tableDetailsNotFound;

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
        public void FutureStorageRequestOnSplittingCargo()
        {
            MTNInitialize();
            
            // Step 4
            FormObjectBase.MainForm.OpenVoyageOperationsFromToolbar();
            VoyageOperationsForm voyageOperationsForm = new VoyageOperationsForm();
            voyageOperationsForm.GetSearcherTab_ClassicMode();
            voyageOperationsForm.GetSearcherTab();

            // Step 5
            voyageOperationsForm.ChkLoloBays.DoClick();
            voyageOperationsForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            voyageOperationsForm.DoSelect();
            voyageOperationsForm.GetMainDetails();

            // Step 6
            Mouse.RightClick();
            voyageOperationsForm.ContextMenuSelect(@"Cargo|Create New Cargo On Vessel...");

            RoadGateDetailsReceiveForm roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Add Cargo TT1");

            // Step 7
            roadGateDetailsReceiveForm.CmbCargoType.SetValue(CargoType.Steel, doDownArrow: true, searchSubStringTo: CargoType.Steel.Length - 1);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(100));
            roadGateDetailsReceiveForm.CmbCargoSubtype.SetValue(CargoSubtype.COIL, doDownArrow: true);
            roadGateDetailsReceiveForm.TxtCargoId.SetValue("JLG43689A01");
            roadGateDetailsReceiveForm.TxtTotalQuantity.SetValue("100");
            roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("2000");
            roadGateDetailsReceiveForm.TxtLocation.SetValue("MSCK000002 ");
            roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.AER,  doDownArrow: true, searchSubStringTo: 3);
            roadGateDetailsReceiveForm.CmbDischargePort.SetValue(Port.JAXUS, doDownArrow: true);
            roadGateDetailsReceiveForm.BtnSave.DoClick();

            // Step 8
            voyageOperationsForm.CmbDischargeTo.SetValue(@"FSA1");
            Assert.IsTrue(voyageOperationsForm.CmbSubArea.GetValue() == "FSA1 - GL1", @"TestCase43689 - Sub Area is not FSA1 - GL1");

            // Step 9
            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(voyageOperationsForm.TblOnVessel,
            // Monday, 3 February 2025 navmh5     @"ID^JLG43689A01~Location^MSCK000002~Total Quantity^100~Cargo Type^STEEL", ClickType.ContextClick, rowHeight: 16);
            voyageOperationsForm.TblOnVessel1.FindClickRow(
                new[] { "ID^JLG43689A01~Location^MSCK000002~Total Quantity^100~Cargo Type^STEEL" }, 
                ClickType.ContextClick);

            // Step 10
            voyageOperationsForm.ContextMenuSelect(@"Actual Discharge | Actual Discharge Selected");

            // Step 11
            ConfirmQuantityToMoveForm confirmQuantityToMoveForm = new ConfirmQuantityToMoveForm(formTitle: @"Confirm quantity to move");
            confirmQuantityToMoveForm.txtQuantity.SetValue(@"60");
            confirmQuantityToMoveForm.btnOK.DoClick();

            /*// Monday, 3 February 2025 navmh5 
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Discharge Cargo TT1");
            string[] warningErrorToCheck =
            {
                "Code :80390. FSA1 GL1 is already full, and will be over capacity if this move eventuates",
                "Code :88457. Location FSA1 GL1's maximum weight is 5000.000 lbs, the current total weight of cargo is 7799.400 lbs, moving cargo item JLG43689A01 to FSA1 GL1 will exceed the weight limit by making to total weight 8999.400 lbs."
               
            };

            // Step 12
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Discharge Cargo TT1", new[]
            {
                "Code :80390. FSA1 GL1 is already full, and will be over capacity if this move eventuates",
                "Code :88457. Location FSA1 GL1's maximum weight is 5000.000 lbs, the current total weight of cargo is 7799.400 lbs, moving cargo item JLG43689A01 to FSA1 GL1 will exceed the weight limit by making to total weight 8999.400 lbs." });

            // Step 13
            //voyageOperationsForm.SetValue(voyageOperationsForm.cmbDischargeTo, @"FSA2");
            voyageOperationsForm.CmbDischargeTo.SetValue(@"FSA2");
            Assert.IsTrue(voyageOperationsForm.CmbSubArea.GetValue() == "FSA2 - GL1", @"TestCase43689 - Sub Area is not FSA2 - GL1");
            // Step 14
            voyageOperationsForm.TblOnVessel1.FindClickRow(
                new[] { "ID^JLG43689A01~Location^MSCK000002~Total Quantity^40~Cargo Type^STEEL" }, 
                ClickType.ContextClick);

            // Step 15
            voyageOperationsForm.ContextMenuSelect(@"Actual Discharge | Actual Discharge Selected");

            // Step 16
            confirmQuantityToMoveForm = new ConfirmQuantityToMoveForm(formTitle: @"Confirm quantity to move");
            confirmQuantityToMoveForm.btnOK.DoClick();

            /*// Monday, 3 February 2025 navmh5 
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Discharge Cargo TT1");
            warningErrorToCheck = new string[]
             {
                 //"Code :82959. Item JLG45742A01 does not match Allocation Auto Allocation - FSA1 (JLG43689A01) at location FSA2 GL1",
                 //"Code :80390. FSA2 GL1 is already full, and will be over capacity if this move eventuates",
                 //"Code :88457. Location FSA2 GL1's maximum weight is 5000.000 lbs, the current total weight of cargo is 0lbs, moving cargo item JLG45742A01 to FSA2 GL1 will exceed the weight limit by making to total weight 10582.197 lbs",
                 //"Code :82959. Item JLG45742A01 does not match Allocation Auto Allocation - FSA1 (JLG43689A01) at location FSA2 GL1",
                 "Code :80390. FSA2 GL1 is already full, and will be over capacity if this move eventuates",
                 //"Code :88457. Location FSA2 GL1's maximum weight is 5000.000 lbs, the current total weight of cargo is 0lbs, moving cargo item JLG45742A01 to FSA2 GL1 will exceed the weight limit by making to total weight 5291.099 lbs.",
             };
            //warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);

            // Step 17
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Discharge Cargo TT1", new[]
            {
                //"Code :82959. Item JLG45742A01 does not match Allocation Auto Allocation - FSA1 (JLG43689A01) at location FSA2 GL1",
                //"Code :80390. FSA2 GL1 is already full, and will be over capacity if this move eventuates",
                //"Code :88457. Location FSA2 GL1's maximum weight is 5000.000 lbs, the current total weight of cargo is 0lbs, moving cargo item JLG45742A01 to FSA2 GL1 will exceed the weight limit by making to total weight 10582.197 lbs",
                //"Code :82959. Item JLG45742A01 does not match Allocation Auto Allocation - FSA1 (JLG43689A01) at location FSA2 GL1",
                "Code :80390. FSA2 GL1 is already full, and will be over capacity if this move eventuates",
                //"Code :88457. Location FSA2 GL1's maximum weight is 5000.000 lbs, the current total weight of cargo is 0lbs, moving cargo item JLG45742A01 to FSA2 GL1 will exceed the weight limit by making to total weight 5291.099 lbs.",
                });

            // Step 18
            //FormObjectBase.NavigationMenuSelection(@"Background Process | Background Processing", forceReset: true);
            FormObjectBase.MainForm.OpenBackgroundProcessingFromToolbar();
            var backgroundApplicationForm = new BackgroundApplicationForm();

            // Step 19
         
            var actionFutureDate = DateTime.Today;
            actionFutureDate = actionFutureDate.AddDays(11);
            var anniversaryDate = DateTime.Today;
            anniversaryDate = anniversaryDate.AddDays(10);
            Console.WriteLine($"DateTime.Today: {DateTime.Today}     actionFuture: {actionFutureDate}     anniversaryDate: {anniversaryDate}");

            backgroundApplicationForm
                .EnableFutureTimerEnquiry()
                .DoFutureTimerEnquiry()
                .ValidateFutureTimerEnquiryTableDetails(new FutureTimerEnquiryArguments
                    {
                        SearchCriteria = new[]
                        {
                            new GetSetFieldsOnFormArguments
                                { FieldName = FutureTimerEnquiryForm.FieldNames.CargoId, FieldValue = "JLG43689A01" },
                        }, 
                        TableDetailsToValidateOrDelete = new[]
                        {
                            $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate.ToString("dd/MM/yyyy")} 00:00" +
                                 "~Terminal^TT1~Cargo Id^JLG43689A01~Cargo Type^STEEL~Cargo Subtype^Steel Coil~Location^FSA1 GL1~Cargo Qty^60~" +
                                 "Anniversary Date^" + anniversaryDate.ToString("dd/MM/yyyy") + " 00:00",
                            $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate.ToString("dd/MM/yyyy")} 00:00" +
                                "~Terminal^TT1~Cargo Id^JLG43689A01~Cargo Type^STEEL~Cargo Subtype^Steel Coil~Location^FSA2 GL1~Cargo Qty^40~" +
                                "Anniversary Date^" + anniversaryDate.ToString("dd/MM/yyyy") + " 00:00" } }, 
                    out  _tableDetailsNotFound )
                .DeleteRowsInFutureTimerTable(new FutureTimerEnquiryArguments
                    {
                        SearchCriteria = new [] { new GetSetFieldsOnFormArguments { FieldName = FutureTimerEnquiryForm.FieldNames.CargoId, FieldValue = "JLG43689A01"},  }, 
                        TableDetailsToValidateOrDelete = new [] 
                        {
                            $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate:dd/MM/yyyy} 00:00~Terminal^TT1~Cargo Id^JLG43689A01~Cargo Type^STEEL~Cargo Subtype^Steel Coil~Location^FSA1 GL1~Cargo Qty^60~Anniversary Date^{anniversaryDate:dd/MM/yyyy} 00:00",
                        },
                        
                    }, 
                    out var  tableDetailsNotFound1 )
                 .DeleteRowsInFutureTimerTable(new FutureTimerEnquiryArguments
                    {
                        SearchCriteria = null, 
                        TableDetailsToValidateOrDelete = new [] 
                        {
                            $"Request Type^Future Storage Process~Action Future Date^{actionFutureDate:dd/MM/yyyy} 00:00~Terminal^TT1~Cargo Id^JLG43689A01~Cargo Type^STEEL~Cargo Subtype^Steel Coil~Location^FSA2 GL1~Cargo Qty^40~Anniversary Date^{anniversaryDate:dd/MM/yyyy} 00:00"
                        },
                        
                    }, 
                    out  _tableDetailsNotFound );
            

        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_43689_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>STEEL</cargoTypeDescr>\n			<product>COIL</product>\n            <id>JLG43689A01</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>AER</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>FSA1 GL1</locationId>\n            <weight>2204.6</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>60</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>STEEL</cargoTypeDescr>\n			<product>COIL</product>\n            <id>JLG43689A01</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>AER</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>FSA1 GL1</locationId>\n            <weight>2204.6</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>60</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            CreateDataFileToLoad(@"DeleteCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>STEEL</cargoTypeDescr>\n			<product>COIL</product>\n            <id>JLG43689A01</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>AER</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>FSA1 GL1</locationId>\n            <weight>2204.6</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>60</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnShip>\n    </AllCargoOnShip>\n</JMTInternalCargoOnShip>");

            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
