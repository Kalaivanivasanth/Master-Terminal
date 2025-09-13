using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Reefer;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.Testing.Extensions.Telemetry;
using MTNArguments.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39247 : MTNBase
    {

        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        ReeferConnectToPowerForm _reeferConnectToPowerForm;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"resetData_39247");
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>("USERDWAT");
        }


        [TestMethod]
        public void PackDetailsOnAllowablePowerOff()
        {
            MTNInitialize();
            
            var dt = DateTime.Now;
            
            //1. go to road gate and enter truck details and click receive full
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"39247");
            roadGateForm.SetRegoCarrierGate("39247");
            roadGateForm.btnReceiveFull.DoClick();

            //2. enter details of container (reefer) A01 - Note that the allowable time off power is set to 5 mins
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive Full Container TT1");
            _roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO2230, additionalWaitTimeout: 1000, doDownArrow: true);
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG39247A01");
            _roadGateDetailsReceiveForm.CmbCommodity.SetValue(Commodity.FZBF, additionalWaitTimeout: 2000, doDownArrow: true);
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("25000");
            _roadGateDetailsReceiveForm.CmbImex.SetValue(IMEX.Export, additionalWaitTimeout: 3000, doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true,
                searchSubStringTo: TT1.Voyage.MSCK000002.Length - 1);
            _roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.MSL,  doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbDischargePort.SetValue(Port.LYTNZ, doDownArrow: true);
            //roadGateDetailsReceiveForm.ShowReeferDetails();
            _roadGateDetailsReceiveForm.TxtReeferCarriageTemperature.SetValue(@"-18");
            _roadGateDetailsReceiveForm.TxtReeferPackTemperature.SetValue(@"-19");
            _roadGateDetailsReceiveForm.TxtReeferPackOffPowerDate.SetValue(dt.ToString("ddMMyyyy"));
            _roadGateDetailsReceiveForm.TxtReeferPackOffPowerTime.SetValue(dt.ToString("HHmm"));
            _roadGateDetailsReceiveForm.TxtReeferAllowableTimeOffPower.SetValue(@"0.05");
            _roadGateDetailsReceiveForm.ChkReeferOnPowerDuringTransit.DoClick();
            _roadGateDetailsReceiveForm.TxtReeferTransitOnPowerDate.SetValue(dt.ToString("ddMMyyyy"));
            _roadGateDetailsReceiveForm.TxtReeferTransitOnPowerTime.SetValue(dt.ToString("HHmm"));
            _roadGateDetailsReceiveForm.TxtReeferTransitOnPowerRemarks.SetValue(@"Check power stays on");

            //3. do a save-next and add another reefer A02
            _roadGateDetailsReceiveForm.BtnSaveNext.DoClick();
            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG39247A02");
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("20000");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();
            /*warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out");

            //4. back in road gate check the gate items before saving
            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Full~Detail^JLG39247A01");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Full~Detail^JLG39247A02");
            roadGateForm.TblGateItems.FindClickRow([
                "Type^Receive Full~Detail^JLG39247A01",
                "Type^Receive Full~Detail^JLG39247A02"
            ]);
            roadGateForm.btnSave.DoClick();
            /*warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            // warning should be that reefers are still connected to power
            string[] warningsToCheck = 
            {
                @"Code :73022. Warning: Container JLG39247A01 is still connected to power.",
                @"Code :73022. Warning: Container JLG39247A02 is still connected to power."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningsToCheck);
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out", new[]
            {
                "Code :73022. Warning: Container JLG39247A01 is still connected to power.",
                "Code :73022. Warning: Container JLG39247A02 is still connected to power.",
            });

            //5. check the reefer details in cargo enquiry
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG39247A");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39247A01");
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG39247A01" });
            cargoEnquiryForm.GetGenericTabTableDetails(@"Reefer", @"4093");
            
            /*MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric,"Is Connected", "Yes");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "Carriage Temperature (C)","-18.00");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "Requires Power","Yes");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "Requires Temperature","Yes");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "Pack/Sender Off Power Date", dt.ToString("dd/MM/yyyy"));
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "Pack/Sender Off Power Time",dt.ToString("HH:mm"));
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "Allowable Time Off Power","0.05");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "Remaining Allowable Time Off Power",@"0.05");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "Pack Temperature (deg C)","-19.0");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "On Power During Transit","1");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "Transit On Power Date",dt.ToString("dd/MM/yyyy"));
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "Transit On Power Time",dt.ToString("HH:mm"));
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, "Transit On Power Remarks","Check power stays on");*/
            
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, new [] {
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Is Connected", FieldRowValue ="Yes" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Carriage Temperature (C)", FieldRowValue ="-18.00" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Requires Power", FieldRowValue ="Yes" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Requires Temperature", FieldRowValue ="Yes" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Pack/Sender Off Power Date", FieldRowValue =dt.ToString("dd/MM/yyyy") },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Pack/Sender Off Power Time", FieldRowValue =dt.ToString("HH:mm") },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Allowable Time Off Power", FieldRowValue ="0.05" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Remaining Allowable Time Off Power", FieldRowValue ="0.05" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Pack Temperature (deg C)", FieldRowValue ="-19.0" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "On Power During Transit", FieldRowValue ="1" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Transit On Power Date", FieldRowValue =dt.ToString("dd/MM/yyyy") },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Transit On Power Time", FieldRowValue =dt.ToString("HH:mm") },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Transit On Power Remarks", FieldRowValue ="Check power stays on" }
            });

            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39247A02");
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG39247A02" });
            
            /*MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Is Connected", "Yes");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Carriage Temperature (C)", "-18.00");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Requires Power", "Yes");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Requires Temperature", "Yes");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Pack/Sender Off Power Date", dt.ToString("dd/MM/yyyy"));
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Pack/Sender Off Power Time", dt.ToString("HH:mm"));
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Allowable Time Off Power", "0.05");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Remaining Allowable Time Off Power", "0.05");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Pack Temperature (deg C)", "-19.0");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"On Power During Transit", "1");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Transit On Power Date", dt.ToString("dd/MM/yyyy"));
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Transit On Power Time", dt.ToString("HH:mm"));
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Transit On Power Remarks", "Check power stays on");*/
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, new[]
            {
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Is Connected", FieldRowValue = "Yes" },
                new MTNGeneralArguments.FieldRowNameValueArguments
                    { FieldRowName = "Carriage Temperature (C)", FieldRowValue = "-18.00" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Requires Power", FieldRowValue = "Yes" },
                new MTNGeneralArguments.FieldRowNameValueArguments
                    { FieldRowName = "Requires Temperature", FieldRowValue = "Yes" },
                new MTNGeneralArguments.FieldRowNameValueArguments
                    { FieldRowName = "Pack/Sender Off Power Date", FieldRowValue = dt.ToString("dd/MM/yyyy") },
                new MTNGeneralArguments.FieldRowNameValueArguments
                    { FieldRowName = "Pack/Sender Off Power Time", FieldRowValue = dt.ToString("HH:mm") },
                new MTNGeneralArguments.FieldRowNameValueArguments
                    { FieldRowName = "Allowable Time Off Power", FieldRowValue = "0.05" },
                new MTNGeneralArguments.FieldRowNameValueArguments
                    { FieldRowName = "Remaining Allowable Time Off Power", FieldRowValue = "0.05" },
                new MTNGeneralArguments.FieldRowNameValueArguments
                    { FieldRowName = "Pack Temperature (deg C)", FieldRowValue = "-19.0" },
                new MTNGeneralArguments.FieldRowNameValueArguments
                    { FieldRowName = "On Power During Transit", FieldRowValue = "1" },
                new MTNGeneralArguments.FieldRowNameValueArguments
                    { FieldRowName = "Transit On Power Date", FieldRowValue = dt.ToString("dd/MM/yyyy") },
                new MTNGeneralArguments.FieldRowNameValueArguments
                    { FieldRowName = "Transit On Power Time", FieldRowValue = dt.ToString("HH:mm") },
                new MTNGeneralArguments.FieldRowNameValueArguments
                    { FieldRowName = "Transit On Power Remarks", FieldRowValue = "Check power stays on" }
            });

            //6. In Road Ops, disconnect both reefers and move to yard
            //FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG39247A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Cargo ID^JLG39247A01" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Disconnect JLG39247A01");
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG39247A02", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Cargo ID^JLG39247A02" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Disconnect JLG39247A02");

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG39247A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Cargo ID^JLG39247A01" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");

            try
            {
                WarningErrorForm.CompleteWarningErrorForm("Warnings for Operations Move");
            }
            catch {}

            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG39247A02", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Cargo ID^JLG39247A02" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");
            roadOperationsForm.CloseForm();

            //7. In cargo enquiry check A01 is not connected and then connect - set connect time + 3 mins (within allowable)
            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39247A01");
            cargoEnquiryForm.SetFocusToForm2();
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG39247A01" });
            /*strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Is Connected");
            Assert.IsTrue(strValue == @"No");*/
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Is Connected", "No");
            var strLastDisconnectedDT = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Last Disconnect DT");
            var newDT = DateTime.ParseExact(strLastDisconnectedDT, "dd/MM/yyyy HH:mm", null);
            newDT = newDT.AddMinutes(3);
            var strNewDate = newDT.ToString("ddMMyyyy");
            var strNewTime = newDT.ToString("HHmm");

            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39247A01", clickType: ClickType.ContextClick);
            cargoEnquiryForm.SetFocusToForm2();
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG39247A01" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|JLG39247A01 Connect To Power");

            _reeferConnectToPowerForm = new ReeferConnectToPowerForm(@"JLG39247A01 Connect To Power");
            MTNControlBase.SetValueInEditTable(_reeferConnectToPowerForm.tblDetails, @"Date", strNewDate );
            MTNControlBase.SetValueInEditTable(_reeferConnectToPowerForm.tblDetails, @"Time", strNewTime);
            _reeferConnectToPowerForm.btnOK.DoClick();

            //8. Check transactions - should have connected/disconnected but NOT a Time off Power exceeded
            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39247A01", clickType: ClickType.ContextClick);
            cargoEnquiryForm.SetFocusToForm2();
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG39247A01" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG39247A01 TT1");
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Power Connected~Details^Connection to  Power", clickType: ClickType.None);
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Power Disconnected~Details^Disconnection from Power", clickType: ClickType.None);
            // bool findRow = MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Time off Power Exceeded~Details^Allowable Time off Power: 0h 5m", clickType: ClickType.None, doAssert: false);
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow([
                "Type^Power Connected~Details^Connection to  Power",
                "Type^Power Disconnected~Details^Disconnection from Power",
                //"Type^Time off Power Exceeded~Details^Allowable Time off Power: 0h 5m"
            ], clickType: ClickType.None);           
            bool findRow = MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.TblTransactions2.GetElement(), @"Type^Time off Power Exceeded~Details^Allowable Time off Power: 0h 5m", clickType: ClickType.None, doAssert: false);
            Assert.IsFalse((findRow), "Power Exceed Transaction should not be listed");
            _cargoEnquiryTransactionForm.CloseForm();
            cargoEnquiryForm.SetFocusToForm();


            //9. repeat for A02 but set connect time + 6 mins
            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39247A02");
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG39247A02" });
            /*strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Is Connected");
            Assert.IsTrue(strValue == @"No");*/
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Is Connected", "No");
            strLastDisconnectedDT = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Last Disconnect DT");
            newDT = DateTime.ParseExact(strLastDisconnectedDT, "dd/MM/yyyy HH:mm", null);
            newDT = newDT.AddMinutes(6);
            strNewDate = newDT.ToString("ddMMyyyy");
            strNewTime = newDT.ToString("HHmm");

            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39247A02", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG39247A02" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|JLG39247A02 Connect To Power");

            _reeferConnectToPowerForm = new ReeferConnectToPowerForm(@"JLG39247A02 Connect To Power");
            MTNControlBase.SetValueInEditTable(_reeferConnectToPowerForm.tblDetails, @"Date", strNewDate);
            MTNControlBase.SetValueInEditTable(_reeferConnectToPowerForm.tblDetails, @"Time", strNewTime);
            _reeferConnectToPowerForm.btnOK.DoClick();

            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39247A02", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG39247A02" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");

            //10. check that the Allowable time off power has been generated
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG39247A02 TT1");
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Power Connected~Details^Connection to  Power", clickType: ClickType.None);
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Power Disconnected~Details^Disconnection from Power", clickType: ClickType.None);
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Time off Power Exceeded~Details^Allowable Time off Power: 0h 5m", clickType: ClickType.None);
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow([
                "Type^Power Connected~Details^Connection to  Power",
                "Type^Power Disconnected~Details^Disconnection from Power",
                "Type^Time off Power Exceeded~Details^Allowable Time off Power: 0h 5m"
            ], clickType: ClickType.None);        
            _cargoEnquiryTransactionForm.CloseForm();

            //11. Road exit to complete visit
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Exit", forceReset: true);
            RoadExitForm roadExitForm = new RoadExitForm(@"Road Exit TT1");
            // MTNControlBase.FindClickRowInTable(roadExitForm.tblVehicleVisits, @"Vehicle ID^39247", rowHeight: 16);
            roadExitForm.TblVehicleVisits.FindClickRow(["Vehicle ID^39247"]);            //roadExitForm.btnProcessRoadExit.DoClick();
            roadExitForm.DoProcessRoadExit();
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_39247_";

            // Cargo on Site Delete
            CreateDataFileToLoad(@"CargoOnSiteDelete.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n        <id>JLG39247A01</id>\n         <isoType>2230</isoType>\n         <voyageCode>MSCK000002</voyageCode>\n         <operatorCode>MSL</operatorCode>\n         <dischargePort>NZAKL</dischargePort>\n         <imexStatus>Export</imexStatus>\n         <totalQuantity>1</totalQuantity>\n         <commodity>FZBF</commodity>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n        <id>JLG39247A02</id>\n         <isoType>2230</isoType>\n         <voyageCode>MSCK000002</voyageCode>\n         <operatorCode>MSL</operatorCode>\n         <dischargePort>NZAKL</dischargePort>\n         <imexStatus>Export</imexStatus>\n         <totalQuantity>1</totalQuantity>\n         <commodity>FZBF</commodity>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
