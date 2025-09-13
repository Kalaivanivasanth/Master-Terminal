using DataFiles.UserDefinedFiles;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestClass66326 : MTNBase
    {
        CallAPI callAPI;
        GetSetArgumentsToCallAPI setArgumentsToCallAPI;
        string baseURL;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void SetStopsOnNonMTTrailer()
        {
            MTNInitialize();
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", "Trailer/Chassis", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", @"MTN66326A1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", "On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();

            // Clear the stop to the trailer - this will confirm that the stop is available and is applied to the trailer
            cargoEnquiryForm.tblData2.FindClickRow(new[]
            { "ID^MTN66326A1~Cargo Type^Trailer/Chassis",}, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Set Stops...");

            StopsForm stopsForm = new StopsForm(@"MTN66326A1 TT1");
            // MTNControlBase.FindClickRowInTable(stopsForm.TblStops.GetElement(), @"Stop^Dogana Export", xOffset: 130);
            stopsForm.TblStops.FindClickRow(["Stop^Dogana Export"], xOffset: 130);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            stopsForm.EnterInput(@"66326"); // Using EnterInputForCustomsDocs because xOffset is 130
            stopsForm.btnSaveAndClose.DoClick();
        }

        private void SetupAndLoadInitializeData(TestContext testContext)
        {
            
            var setArgumentsToCallAPI = new GetSetArgumentsToCallAPI
            { RequestURL = $"{TestContext.GetRunSettingValue("BaseUrl")}SendEDI?MasterTerminalAPI", };

            CallAPI.AddDeleteData(new[] {
               new AddDeleteDataArguments { Data = new [] {
                    new CargoOnSiteXML { ID = "MTN66326A1", CargoTypeDescr = CargoType.TrailerChassis, IMEXStatus = IMEX.Export, Voyage = TT1.Voyage.MSCK000010, Operator = Operator.MSL, TotalWeight = "15999.0000",
                        DischargePort = "NZAKL", Location = TT1.TerminalArea.MKBS01, LoadPort = Port.SYDAU, TrailerType = "MAFI40XXX", Action = "D" },
                    new CargoOnSiteXML { ID = "MTN66326A2", CargoTypeDescr = CargoType.BottlesOfBeer, IMEXStatus = IMEX.Export, Voyage = TT1.Voyage.MSCK000010, Operator = Operator.MSL, TotalWeight = "10000.0000",
                        DischargePort = "NZAKL", Location = TT1.TerminalArea.MKBS01, LoadPort = Port.SYDAU, ParentCargoID = "MTN66326A1", ParentCargoType = CargoType.TrailerChassis, Action = "D" },
                    }, FileType = "CargoOnSiteXML", ArgumentsToCallAPIs = setArgumentsToCallAPI },
               new AddDeleteDataArguments { Data = new [] {
                    new CargoOnSiteXML { ID = "MTN66326A1", CargoTypeDescr = CargoType.TrailerChassis, IMEXStatus = IMEX.Export, Voyage = TT1.Voyage.MSCK000010, Operator = Operator.MSL, TotalWeight = "15999.0000",
                        DischargePort = "NZAKL", Location = TT1.TerminalArea.MKBS01, LoadPort = Port.SYDAU, TrailerType = "MAFI40XXX", Action = "A" },
                    new CargoOnSiteXML { ID = "MTN66326A2", CargoTypeDescr = CargoType.BottlesOfBeer, IMEXStatus = IMEX.Export, Voyage = TT1.Voyage.MSCK000010, Operator = Operator.MSL, TotalWeight = "10000.0000",
                        DischargePort = "NZAKL", Location = TT1.TerminalArea.MKBS01, LoadPort = Port.SYDAU, ParentCargoID = "MTN66326A1", ParentCargoType = CargoType.TrailerChassis, Action = "A" },
                    }, FileType = "CargoOnSiteXML", ArgumentsToCallAPIs = setArgumentsToCallAPI },
            });
        }
    }
}
