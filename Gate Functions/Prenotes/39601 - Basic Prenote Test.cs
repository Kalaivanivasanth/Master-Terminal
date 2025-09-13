using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNArguments.Classes;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Prenotes
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39601 : MTNBase
    {

        RoadGateDetailsReceiveForm _preNoteDetailsForm;
        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        PreNoteForm _preNoteForm;
        ConfirmationFormOK _confirmationFormOK;
        ConfirmationFormOKwithText _confirmationFormOKwithText;
        CargoEnquiryDirectForm _cargoEnquiryDirectForm;

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
        public void BasicPreNote()
        {
            MTNInitialize();

            // 1. set dates for the test
            var dateToday = DateTime.Now;
            var dateYesterday = dateToday.AddDays(-1);
            var dateTomorrow = dateToday.AddDays(1);

            // 2. Open pre-Notes form, find existing pre-notes and delete if any found
            FormObjectBase.MainForm.OpenPreNotesFromToolbar();

            _preNoteForm = new PreNoteForm(formTitle: @"Pre-Notes TT1");
            _preNoteForm.DeletePreNotes(@"JLG39601", @"ID^JLG39601");

            // 3. add new prenote details for JLG39601A01 
            _preNoteForm.DoNew();
            _preNoteDetailsForm = new RoadGateDetailsReceiveForm(formTitle: @"PreNote Full Container TT1");

            // Main details
            _preNoteDetailsForm.TxtBooking.SetValue(@"BOOK39601"); // To include 63032
            _preNoteDetailsForm.CmbIsoType.SetValue(ISOType.ISO2230, doDownArrow: true);  //, 10);
            _preNoteDetailsForm.TxtCargoId.SetValue(@"JLG39601A01");
            _preNoteDetailsForm.CmbCommodity.SetValue(Commodity.REEF, doDownArrow: true);
            _preNoteDetailsForm.MtTotalWeight.SetValueAndType("9875");
            _preNoteDetailsForm.TxtExpectedArrivalDate.SetValue(dateToday.ToString(@"ddMMyyyy"));
            _preNoteDetailsForm.TxtExpectedArrivalTime.SetValue(dateToday.ToString(@"HHMM"));
            // voyage details
            _preNoteDetailsForm.CmbImex.SetValue(IMEX.Import, additionalWaitTimeout: 2000, doDownArrow: true);
            _preNoteDetailsForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            _preNoteDetailsForm.CmbOperator.SetValue(Operator.MSK,  doDownArrow: true);
            _preNoteDetailsForm.CmbDischargePort.SetValue(Port.AKLNZ, doDownArrow: true); 
            // reefer details

            _preNoteDetailsForm.TxtReeferCarriageTemperature.SetValue(@"-5.5");
            _preNoteDetailsForm.TxtReeferPackTemperature.SetValue(@"-6.5");
            _preNoteDetailsForm.TxtReeferPackOffPowerDate.SetValue(dateYesterday.ToString(@"ddMMyyyy"));
            _preNoteDetailsForm.TxtReeferPackOffPowerTime.SetValue(@"2333");
            _preNoteDetailsForm.TxtReeferAllowableTimeOffPower.SetValue(@".05");
            _preNoteDetailsForm.ChkReeferOnPowerDuringTransit.DoClick();
            
            _preNoteDetailsForm.TxtReeferTransitOnPowerDate.SetValue(dateYesterday.ToString(@"ddMMyyyy"));
            _preNoteDetailsForm.TxtReeferTransitOnPowerTime.SetValue(@"2334");
            _preNoteDetailsForm.TxtReeferTransitOnPowerRemarks.SetValue(@"On Power");
            _preNoteDetailsForm.TxtReeferExpiryDate.SetValue(dateTomorrow.ToString(@"ddMMyyyy"));
            _preNoteDetailsForm.TxtReeferHumidity.SetValue(@"45");
            _preNoteDetailsForm.MtReeferVentilation.SetValueAndType("23");
            _preNoteDetailsForm.TxtReeferCO2Percent.SetValue(@"5.5");
            _preNoteDetailsForm.TxtReeferO2Percent.SetValue(@"8.5");
            _preNoteDetailsForm.CmbReeferVoltageType.SetValue(VoltageType.SingleVolt, doDownArrow: true);
            _preNoteDetailsForm.CmbReeferCoolingType.SetValue(CoolingType.WaterCooled, doDownArrow: true);

            _preNoteDetailsForm.BtnSave.DoClick();
           
            WarningErrorForm.CheckErrorMessagesExist("Warnings for Pre-Notes TT1", new string[]
            {
                "Code :75016. The Container Id (JLG39601A01) failed the validity checks and may be incorrect.",
                "Code :75737. The operator MSK requires a consignor for item JLG39601A01."
            }, false);

            _confirmationFormOK = new ConfirmationFormOK("Pre-Note Added", automationIdOK: @"4");
           _confirmationFormOK.btnOK.DoClick();


            // 4. add new prenote details for JLG39601A02
            _preNoteForm.DoNew();
            _preNoteDetailsForm = new RoadGateDetailsReceiveForm(formTitle: @"PreNote Full Container TT1");

            // Main details
            _preNoteDetailsForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true); //, 10);
            _preNoteDetailsForm.TxtCargoId.SetValue(@"JLG39601A02");
            _preNoteDetailsForm.CmbCommodity.SetValue(Commodity.MT, doDownArrow: true);
            _preNoteDetailsForm.MtTotalWeight.SetValueAndType("3456");
            _preNoteDetailsForm.TxtExpectedArrivalDate.SetValue(dateToday.ToString(@"ddMMyyyy"));
            _preNoteDetailsForm.TxtExpectedArrivalTime.SetValue(dateToday.ToString(@"HHMM"));
            // voyage details
            _preNoteDetailsForm.CmbImex.SetValue(IMEX.Import,  additionalWaitTimeout: 2000, doDownArrow: true);
            _preNoteDetailsForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            _preNoteDetailsForm.CmbOperator.SetValue(Operator.MSK,  doDownArrow: true);
            _preNoteDetailsForm.CmbDischargePort.SetValue(Port.AKLNZ, doDownArrow: true); 

            _preNoteDetailsForm.BtnSave.DoClick();
            
            WarningErrorForm.CheckErrorMessagesExist("Warnings for Pre-Notes TT1", new []
            {
                "Code :75016. The Container Id (JLG39601A02) failed the validity checks and may be incorrect.",
                "Code :75737. The operator MSK requires a consignor for item JLG39601A02."
            }, false);

            _confirmationFormOK = new ConfirmationFormOK("Pre-Note Added", automationIdOK: @"4");
           _confirmationFormOK.btnOK.DoClick();



            //5. Validate prenote data in cargo enquiry for JLG39601A01
            // Friday, 31 January 2025 navmh5 MTNControlBase.FindClickRowInTable(_preNoteForm.tblPreNotes, "ID^JLG39601A01", clickType: ClickType.DoubleClick, rowHeight: 18, doAssert: false);
            _preNoteForm.TblPreNotes.FindClickRow(new [] { "ID^JLG39601A01" }, clickType: ClickType.DoubleClick,  doAssert: false);            

            _cargoEnquiryDirectForm = new CargoEnquiryDirectForm(@"JLG39601A01 TT1");
            MTNControlBase.ValidateValueInEditTable(_cargoEnquiryDirectForm.tblGeneral, [
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "ID", FieldRowValue = "JLG39601A01" }, 
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Voyage", FieldRowValue = TT1.Voyage.MSCK000002 },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Operator", FieldRowValue = Operator.MSK },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Discharge Port", FieldRowValue = "NZAKL" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "ISO Type", FieldRowValue = ISOType.ISO2230 },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Commodity", FieldRowValue = "REEF" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "IMEX Status", FieldRowValue = IMEX.Import },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Total Weight", FieldRowValue = "9875.000 lbs; 4.408 LT; 4.479 MT; 4.937 ST; 4479 kg" },
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Declared Weight", FieldRowValue = "9875.000 lbs; 4.408 LT; 4.479 MT; 4.937 ST; 4479 kg" }
            ]);

            _cargoEnquiryDirectForm.GetReeferTable(@"4093");
            
            /*ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Is Reefer", @"Yes");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Carriage Temperature (C)", @"-5.50");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Carriage Temperature (F)", @"22.10");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Monitor Connection", @"Yes");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Requires Power", @"Yes");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Humidity", @"45.00");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Last Humidity Reading", @"45.00 %");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Last Ventilation Reading", @"23.00 m3/hr");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Expiry Date", dateTomorrow.ToString(@"dd/MM/yyyy"));
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Cooling Type", @"Water Cooled");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Voltage Type", @"Single Volt");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Controlled Atmosphere", @"Yes");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Oxygen", @"8.50");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Carbon Dioxide", @"5.50");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Pack/Sender Off Power Date", dateYesterday.ToString(@"dd/MM/yyyy"));
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Pack/Sender Off Power Time", @"23:33");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Allowable Time Off Power", @"0.05");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Pack Temperature (deg C)", @"-6.5");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Transit On Power Date", dateYesterday.ToString(@"dd/MM/yyyy"));
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Transit On Power Time", @"23:34");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Transit On Power Remarks", @"On Power");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Curtains In", @"0");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"On Power During Transit", @"1");*/
            MTNControlBase.ValidateValueInEditTable(_cargoEnquiryDirectForm.tblReefer, [
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Is Reefer", FieldRowValue = "Yes"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Carriage Temperature (C)", FieldRowValue = @"-5.50"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Carriage Temperature (F)", FieldRowValue = @"22.10"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Monitor Connection", FieldRowValue = @"Yes"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Requires Power", FieldRowValue = @"Yes"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Humidity", FieldRowValue = @"45.00"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Last Humidity Reading", FieldRowValue = @"45.00 %"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Last Ventilation Reading", FieldRowValue = @"23.00 m3/hr"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Expiry Date", FieldRowValue = dateTomorrow.ToString(@"dd/MM/yyyy")},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Cooling Type", FieldRowValue = @"Water Cooled"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Voltage Type", FieldRowValue = @"Single Volt"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Controlled Atmosphere", FieldRowValue = @"Yes"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Oxygen", FieldRowValue = @"8.50"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Carbon Dioxide", FieldRowValue = @"5.50"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Pack/Sender Off Power Date", FieldRowValue = dateYesterday.ToString(@"dd/MM/yyyy")},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Pack/Sender Off Power Time", FieldRowValue = @"23:33"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Allowable Time Off Power", FieldRowValue = @"0.05"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Pack Temperature (deg C)", FieldRowValue = @"-6.5"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Transit On Power Date", FieldRowValue = dateYesterday.ToString(@"dd/MM/yyyy")},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Transit On Power Time", FieldRowValue = @"23:34"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Transit On Power Remarks", FieldRowValue = @"On Power"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="Curtains In", FieldRowValue = @"0"},
                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName ="On Power During Transit", FieldRowValue = @"1"},
            ]);

            _cargoEnquiryDirectForm.GetReceiveReleaseTable(@"4085");

            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReceiveRelease, @"Receipt Transport Mode", @"Road");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReceiveRelease, @"Expected Date/Time", dateToday.ToString(@"dd/MM/yyyy") + " " + dateToday.ToString(@"HH:MM"));

            _cargoEnquiryDirectForm.CloseForm();

            //5. Validate prenote data in cargo enquiry for JLG39601A02
            _preNoteForm.SetFocusToForm();
            // Monday, 3 February 2025 navmh5 MTNControlBase.FindClickRowInTable(_preNoteForm.tblPreNotes, "ID^JLG39601A02", clickType: ClickType.DoubleClick, rowHeight: 18, doAssert: false);
           
            _preNoteForm.TblPreNotes.FindClickRow(["ID^JLG39601A02"], clickType: ClickType.DoubleClick,  doAssert: false);
            _cargoEnquiryDirectForm = new CargoEnquiryDirectForm(@"JLG39601A02 TT1");

            /*ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblGeneral, @"ID", @"JLG39601A02");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblGeneral, @"Voyage", @"MSCK000002");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblGeneral, @"Operator", @"MSK");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblGeneral, @"Discharge Port", @"NZAKL");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblGeneral, @"ISO Type", @"2200");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblGeneral, @"Commodity", @"MT");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblGeneral, @"IMEX Status", @"Import");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblGeneral, @"Total Weight", @"3456.000 lbs; 1.543 LT; 1.568 MT; 1.728 ST; 1568 kg");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblGeneral, @"Declared Weight", @"3456.000 lbs; 1.543 LT; 1.568 MT; 1.728 ST; 1568 kg");*/
             MTNControlBase.ValidateValueInEditTable(_cargoEnquiryDirectForm.tblGeneral, [
                 new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "ID", FieldRowValue = "JLG39601A02"},
                 new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Voyage", FieldRowValue = "MSCK000002"},
                 new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Operator", FieldRowValue = "MSK"},
                 new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Discharge Port", FieldRowValue = "NZAKL"},
                 new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "ISO Type", FieldRowValue = "2200"},
                 new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Commodity", FieldRowValue = "MT"},
                 new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "IMEX Status", FieldRowValue = "Import"},
                 new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Total Weight", FieldRowValue = "3456.000 lbs; 1.543 LT; 1.568 MT; 1.728 ST; 1568 kg"},
                 new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = "Declared Weight", FieldRowValue = "3456.000 lbs; 1.543 LT; 1.568 MT; 1.728 ST; 1568 kg"},
            ]);
                
            _cargoEnquiryDirectForm.GetReeferTable(@"4093");

            /*ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Is Reefer", @"No");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Carriage Temperature (C)", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Carriage Temperature (F)", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Monitor Connection", @"No");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Requires Power", @"No");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Humidity", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Last Humidity Reading", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Last Ventilation Reading", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Expiry Date", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Cooling Type", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Voltage Type", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Controlled Atmosphere", @"No");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Oxygen", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Carbon Dioxide", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Pack/Sender Off Power Date", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Pack/Sender Off Power Time", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Allowable Time Off Power", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Pack Temperature (deg C)", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Transit On Power Date", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Transit On Power Time", @"00:00");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Transit On Power Remarks", checkNullOrEmpty: true);
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"Curtains In", "");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReefer, @"On Power During Transit", "0");*/
            MTNControlBase.ValidateValueInEditTable(_cargoEnquiryDirectForm.tblReefer, [
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Is Reefer", FieldRowValue =  "No"},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Carriage Temperature (C)", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Carriage Temperature (F)", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Monitor Connection", FieldRowValue =  "No"},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Requires Power", FieldRowValue =  "No"},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Humidity", FieldRowValue = string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Last Humidity Reading", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Last Ventilation Reading", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Expiry Date", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Cooling Type", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Voltage Type", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Controlled Atmosphere", FieldRowValue =  "No"},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Oxygen", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Carbon Dioxide", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Pack/Sender Off Power Date", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Pack/Sender Off Power Time", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Allowable Time Off Power", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Pack Temperature (deg C)", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Transit On Power Date", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Transit On Power Time", FieldRowValue =  "00:00"},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Transit On Power Remarks", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "Curtains In", FieldRowValue =  string.Empty},
                new MTNGeneralArguments.FieldRowNameValueArguments{ FieldRowName = "On Power During Transit", FieldRowValue =  "0"},
            ]);
            

            _cargoEnquiryDirectForm.GetReceiveReleaseTable(@"4085");

            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReceiveRelease, @"Receipt Transport Mode", @"Road");
            ValidateEditTable(_cargoEnquiryDirectForm, _cargoEnquiryDirectForm.tblReceiveRelease, @"Expected Date/Time",
                $"{dateToday:dd/MM/yyyy} {dateToday:HH:MM}");

            _cargoEnquiryDirectForm.CloseForm();
            _preNoteForm.CloseForm();

            //6. Gate in both pre-notes

            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: true);
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();

            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1");
            roadGateForm.SetRegoCarrierGate("39601");
            roadGateForm.txtNewItem.SetValue(@"JLG39601A01");

            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Full Reefer TT1");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            WarningErrorForm.CheckErrorMessagesExist("Warnings for Gate In/Out TT1", new string[]
            {
                "Code :75016. The Container Id (JLG39601A01) failed the validity checks and may be incorrect.",
                "Code :75737. The operator MSK requires a consignor for item JLG39601A01."
            }, false);

            roadGateForm.txtNewItem.SetValue(@"JLG39601A02");

            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Empty Container TT1");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();
            
            WarningErrorForm.CheckErrorMessagesExist("Warnings for Gate In/Out TT1", new []
            {
                "Code :75016. The Container Id (JLG39601A02) failed the validity checks and may be incorrect.",
                "Code :75737. The operator MSK requires a consignor for item JLG39601A02."
            }, false);

            roadGateForm.btnSave.DoClick();

            WarningErrorForm.CheckErrorMessagesExist("Warnings for Gate In/Out TT1", new []
            {
                "Code :75737. The operator MSK requires a consignor for item JLG39601A01.",
                "Code :75737. The operator MSK requires a consignor for item JLG39601A02."
            }, false);

            WarningErrorForm.CheckErrorMessagesExist("Warnings for Gate In/Out TT1", new []
            {
                "Code :75737. The operator MSK requires a consignor for item JLG39601A01.",
                "Code :75737. The operator MSK requires a consignor for item JLG39601A02.",
                "Code :73022. Warning: Container JLG39601A01 is still connected to power."
            }, false);

            roadGateForm.CloseForm();

            //7. Check that the pre-notes are gone
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Pre-Notes", forceReset: true);
            FormObjectBase.MainForm.OpenPreNotesFromToolbar();
            _preNoteForm = new PreNoteForm(formTitle: @"Pre-Notes TT1");

            MTNControlBase.SetValueInEditTable(_preNoteForm.tblPreNoteSearch, @"Cargo Id", @"JLG39601");
            _preNoteForm.DoNew();

            // Friday, 31 January 2025 navmh5 bool dataFound = MTNControlBase.FindClickRowInTable(_preNoteForm.tblPreNotes, "ID^JLG39601A01", rowHeight: 18, doAssert: false);
            // Friday, 31 January 2025 navmh5 var dataFound = _preNoteForm.TblPreNotes.FindClickRow(new [] { "ID^JLG39601A01" },  doAssert: false);
            // Friday, 31 January 2025 navmh5 Assert.IsTrue(dataFound == false, "JLG39601A01 should not be present in list");
            // Friday, 31 January 2025 navmh5 Assert.IsTrue(string.IsNullOrEmpty(dataFound), "JLG39601A01 should not be present in list");
            // Friday, 31 January 2025 navmh5 dataFound = MTNControlBase.FindClickRowInTable(_preNoteForm.tblPreNotes, "ID^JLG39601A02", rowHeight: 18, doAssert: false);
            // Friday, 31 January 2025 navmh5 Assert.IsTrue(dataFound == false, "JLG39601A02 should not be present in list");
            var rowsFound =
                _preNoteForm.TblPreNotes.FindClickRow(new[] { "ID^JLG39601A01", "ID^JLG39601A02" }, doAssert: false);
            Assert.IsTrue(!string.IsNullOrEmpty(rowsFound), "JLG39601A01 and/or JLG39601A02 should not be present in list");
            // Adding step to check Booking is still populated for JLG39601A01 to include Test case 63032
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
          
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG39601A01");
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            string fieldValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Booking");
            string textToCheck = @"BOOK39601";
            Assert.IsTrue(fieldValue == textToCheck, @"Field: Booking has a value of " + fieldValue.ToUpper() + @" and should equal: " + textToCheck);

            //8. Go to road operations
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");

            // 9. for JLG39601A02 - delete current entry to recreate the pre-note
            // Friday, 31 January 2025 navmh5 MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG39601A02", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new [] { "Cargo ID^JLG39601A02" }, ClickType.ContextClick);            
            roadOperationsForm.ContextMenuSelect(@"Delete Current Entry");
            _confirmationFormOKwithText = new ConfirmationFormOKwithText(@"Enter the Cancellation value:", controlType: ControlType.Pane);
            _confirmationFormOKwithText.txtInput.SetValue(@"Test 39601");
            _confirmationFormOKwithText.btnOK.DoClick();
            WarningErrorForm.CheckErrorMessagesExist("Warnings for Road Ops TT1", null, false);

            // 10. for JLG39601A01 - move item and process road exit
            // Friday, 31 January 2025 navmh5 MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Cargo ID^JLG39601A01", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new [] { "Cargo ID^JLG39601A01" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Move It|Move Selected");

            WarningErrorForm.CheckErrorMessagesExist("Warnings for Operations Move TT1", null, false);
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            // Friday, 31 January 2025 navmh5 MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^39601", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new [] { "Vehicle Id^39601" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");
            roadOperationsForm.CloseForm();

            // 11. for JLG39601A02 - Gate in pre-note again 
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"39601A");
            roadGateForm.SetRegoCarrierGate("39601A");
            roadGateForm.txtNewItem.SetValue(@"JLG39601A02");

            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Empty Container TT1");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            WarningErrorForm.CheckErrorMessagesExist("Warnings for Gate In/Out TT1", new [] {
                "Code :75016. The Container Id (JLG39601A02) failed the validity checks and may be incorrect.",
                "Code :75737. The operator MSK requires a consignor for item JLG39601A02."
            }, false);
            
            roadGateForm.btnSave.DoClick();

            WarningErrorForm.CheckErrorMessagesExist("Warnings for Gate In/Out TT1", new [] {
                "Code :75737. The operator MSK requires a consignor for item JLG39601A02."
            }, false);


            // 12. Go to road operations and move/process JLG39601A02
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(RoadOperationsForm.FormTitle, new[] { "39601A" });

        }


        private static void ValidateEditTable(CargoEnquiryDirectForm formToValidate, AutomationElement tableToValidate, string rowToCheck, string valueToAssert = null, bool checkNullOrEmpty = false)
        {

            string validationData = MTNControlBase.GetValueInEditTable(tableToValidate, rowToCheck);

            if (checkNullOrEmpty)
            {
                Assert.IsTrue(string.IsNullOrEmpty(validationData), rowToCheck + " incorrect: Actual: " + validationData + " Expected: Null");

            }
            else
            {
                Assert.IsTrue(validationData == valueToAssert, rowToCheck + " incorrect: Actual: " + validationData + " Expected: " + valueToAssert);
            }

        }
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            //fileOrder = 1;
            
            searchFor = @"_39601_";

            //Create Kiwi Rail File 1 A01
            CreateDataFileToLoad(@"DeleteCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG39601A01</id>\n            <isoType>2230</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <imexStatus>Import</imexStatus>\n            <commodity>REEF</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n       <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG39601A02</id>\n            <isoType>2200</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n            <locationId>MKBS01</locationId>\n            <imexStatus>Import</imexStatus>\n            <commodity>MT</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);

        }

    }

}
