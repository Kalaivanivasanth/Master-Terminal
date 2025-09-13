using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using FlaUI.Core.Tools;
using MTNAutomationTests.BaseClasses.MasterTerminal;

namespace MTNAutomationTests.FormObjects.MasterTerminal.Gate_Functions.Road_Gate
{
    
    class RoadGateDetailsReceiveForm : FormObjectBase
    {
       
        public AutomationElement btnSave;
        public AutomationElement btnSaveNext;
        public AutomationElement btnCancel;
        public AutomationElement txtCargoId;
        public AutomationElement btnCargoIDs;
        public AutomationElement txtImex;
        public AutomationElement cmbIsoType;
        public AutomationElement cmbCommodity;
        public AutomationElement txtTotalWeight;
        public AutomationElement cmbVoyage;
        public AutomationElement cmbOperator;
        public AutomationElement cmbDischargePort;
        public AutomationElement txtExpectedArrivalDate;
        public AutomationElement txtExpectedArrivalTime;
        public AutomationElement txtTareWeight;
        public AutomationElement txtBooking;
        public AutomationElement btnWeight;
        public AutomationElement btnHazardDetails;

        // Reefer fields
        public AutomationElement txtReeferCarriageTemperature;
        public AutomationElement txtReeferPackTemperature;
        public AutomationElement txtReeferPackOffPowerDate;
        public AutomationElement txtReeferPackOffPowerTime;
        public AutomationElement txtReeferAllowableTimeOffPower;
        public AutomationElement chkReeferOnPowerDuringTransit;
        public AutomationElement txtReeferTransitOnPowerDate;
        public AutomationElement txtReeferTransitOnPowerTime;
        public AutomationElement txtReeferTransitOnPowerRemarks;
        public AutomationElement txtReeferExpiryDate;
        public AutomationElement txtReeferHumidity;
        public AutomationElement tblReeferVentilation;
        public AutomationElement txtReeferCO2Percent;
        public AutomationElement txtReeferO2Percent;
        public AutomationElement cmbReeferVoltageType;
        public AutomationElement cmbReeferCoolingType;
        public AutomationElement cmbCargoType;

        #region - Voyage Operations - Add Cargo

        #region - Voyage Operations - Add Cargo - Main Details
        public AutomationElement txtLocation;
        public AutomationElement chkISOOTL;
        public AutomationElement txtCoordinatesX;
        public AutomationElement txtCoordinatesY;
        #endregion - Voyage Operations - Add Cargo - Main Details

        #region - Non ISO Container
        public AutomationElement cmbCargoSubtype;
        public AutomationElement txtTotalQuantity;
        #endregion - Non ISO Container

        #region - Voyage Operations - Add Cargo - Cargo Details
        public AutomationElement btnAttachments;
        public AutomationElement txtHazardDetails;
        public AutomationElement txtOperatorSeal;
        #endregion - Voyage Operations - Add Cargo - Cargo Details

        #endregion - Voyage Operations - Add Cargo

        EnterCargoIDsForm enterCargoIDsForm;



        public RoadGateDetailsReceiveForm(string formTitle = null)
        {
            // get the form
            GetFormElement(formTitle);


            // All the below are on all gate receive and prenote and voyage operations - add cargo forms

            GetFormControl(@"8364", form, ref txtCargoId, @"Cargo ID textbox", ControlType.Edit);
            GetFormControl(@"8238", form, ref txtImex, @"IMEX textbox", ControlType.Edit);
            GetFormControl(@"2004", form, ref btnSave, @"Save button", ControlType.Button);
            GetFormControl(@"2002", form, ref btnSaveNext, @"Save Next button", ControlType.Button);
            GetFormControl(@"6105", form, ref cmbVoyage, @"Voyage Combobox", ControlType.ComboBox);
            GetFormControl(@"6080", form, ref cmbOperator, @"Operator combobox", ControlType.ComboBox);
            GetFormControl(@"6106", form, ref cmbDischargePort, @"Discharge Port combobox", ControlType.ComboBox);
            GetFormControl(@"8164", form, ref txtTotalWeight, @"Total Weight textbox", ControlType.Table);
            GetFormControl(@"2003", form, ref btnCancel, @"Cancel button", ControlType.Button);
            GetFormControl(@"8154", form, ref txtTareWeight, @"Tare Weight textbox", ControlType.Table);
            

            // the following may/may not be there from the start - try to get them but carry on if it can't
            try { GetFormControl(@"8253", form, ref cmbCommodity, @"Commodity Type combobox", ControlType.ComboBox); }catch { }
            try { GetFormControl(@"8240", form, ref cmbIsoType, @"ISO Type combobox", ControlType.ComboBox);} catch { }
            try { GetFormControl(@"8228", form, ref cmbCargoType, @"Cargo Type combobox", ControlType.ComboBox); } catch { }
            try { GetFormControl(@"6082", form, ref btnAttachments, @"Attachements button", ControlType.Button); } catch { }
            try { GetFormControl(@"6147", form, ref btnCargoIDs, @"Cargo IDs button", ControlType.Button); } catch { }
            try { GetFormControl(@"8354", form, ref txtBooking, @"Booking textbox", ControlType.Edit); } catch { }
            
            // not Voyage Opeartions Add Cargo
            if (!formTitle.Contains(@"Add Cargo"))
            {
                GetFormControl(@"8199", form, ref txtExpectedArrivalDate, @"Expected Arrival Date textbox", ControlType.Edit);
                GetFormControl(@"8196", form, ref txtExpectedArrivalTime, @"Expected Arrival Time textbox", ControlType.Edit);
            }
            else
            {
                GetFormControl(@"8280", form, ref txtLocation, @"Location textbox", ControlType.Edit);
                GetFormControl(@"8046", form, ref chkISOOTL, @"Is OOTL checkbox", ControlType.CheckBox);
                GetFormControl(@"6197", form, ref txtCoordinatesX, @"Coordinates (m) X textbox", ControlType.Edit);
                GetFormControl(@"6196", form, ref txtCoordinatesY, @"Coordinates (m) Y textbox", ControlType.Edit);
            }
            

            if (formTitle.Contains(@"PreNote") || formTitle.Contains(@"General Cargo") || formTitle.Contains(@"Add Cargo"))
            {
                ShowCargoType();
            }
            


        }

        public void ShowCargoType()
        {

            GetFormControl(@"8228", form, ref cmbCargoType, @"Cargo Type combobox", ControlType.ComboBox);

        }

        public void ShowContainerDetails()
        {

            GetFormControl(@"8240", form, ref cmbIsoType, @"ISO Type combobox", ControlType.ComboBox);

        }

        public void ShowCommodity()
        {

            GetFormControl(@"8253", form, ref cmbCommodity, @"Commodity Type combobox", ControlType.ComboBox);

        }

        public void ShowCommodityForP37631()
        {

           // GetFormControl(@"8250", form, ref cmbCommodity, @"Commodity Type combobox", ControlType.ComboBox);
            try
            {
                Retry.WhileException(() =>
                {
                    cmbCommodity =
                        form.FindFirstDescendant(cf =>
                            cf.ByAutomationId(@"8253").And(cf.ByControlType(ControlType.ComboBox)));
                }, System.TimeSpan.FromSeconds(10), null, true);
            }
            catch
            {

            }
        }


        public void ShowReeferDetails()
        {

            GetFormControl(@"8296", form, ref txtReeferCarriageTemperature, @"Carriage Temperature textbox", ControlType.Edit);
            GetFormControl(@"8309", form, ref txtReeferPackTemperature, @"Pack Temperature textbox", ControlType.Edit);
            GetFormControl(@"8307", form, ref txtReeferPackOffPowerDate, @"Pack Off Power Date textbox", ControlType.Edit);
            GetFormControl(@"8312", form, ref txtReeferPackOffPowerTime, @"Pack Off Power Time textbox", ControlType.Edit);   
            GetFormControl(@"8177", form, ref txtReeferAllowableTimeOffPower, @"Allowable Time Off Power textbox", ControlType.Edit);
            GetFormControl(@"8173", form, ref txtReeferTransitOnPowerDate, @"Transit On Power Date textbox", ControlType.Edit);
            GetFormControl(@"8170", form, ref txtReeferTransitOnPowerTime, @"Transit On Power Time textbox", ControlType.Edit);
            GetFormControl(@"8370", form, ref txtReeferTransitOnPowerRemarks, @"Transit On Power Remarks textbox", ControlType.Edit);
            GetFormControl(@"8043", form, ref txtReeferExpiryDate, @"Expiry Date textbox", ControlType.Edit);
            GetFormControl(@"8304", form, ref txtReeferHumidity, @"Humidity textbox", ControlType.Edit);
            GetFormControl(@"8298", form, ref txtReeferCO2Percent, @"CO2 % textbox", ControlType.Edit);
            GetFormControl(@"8300", form, ref txtReeferO2Percent, @"O2 % textbox", ControlType.Edit);
            GetFormControl(@"8314", form, ref cmbReeferVoltageType, @"Voyage Type combobox", ControlType.ComboBox);
            GetFormControl(@"8317", form, ref cmbReeferCoolingType, @"Cooling Type combobox", ControlType.ComboBox);
            GetFormControl(@"8089", form, ref tblReeferVentilation, @"Ventilation table", ControlType.Table);
            GetFormControl(@"8175", form, ref chkReeferOnPowerDuringTransit, @"Reefer On Power checkbox", ControlType.CheckBox);

        }

        public void GetCargoDetails()
        {
            GetFormControl(@"6082", form, ref btnAttachments, @"Attachments button", ControlType.Button);
            GetFormControl(@"6100", form, ref txtHazardDetails, @"Hazard Details textbox", 
                ControlType.Edit);
            GetFormControl(@"6208", form, ref btnHazardDetails, @"Hazard Details button",
                ControlType.Button);
            GetFormControl(@"8483", form, ref txtOperatorSeal, @"Operator Seal textbox",
                ControlType.Edit);
        }

        public void ShowNonISOContainerDetails()
        {
            GetFormControl(@"8259", form, ref cmbCargoSubtype, @"Cargo Subtype combobox", ControlType.ComboBox);
            GetFormControl(@"8220", form, ref txtTotalQuantity, @"Total Quantity textbox", ControlType.Edit);
        }


        public void AddMulitpleCargoIds(string strFirstID, string strNumberToCreate)
        {
            ClickButton(btnCargoIDs, 3000);
            enterCargoIDsForm = new EnterCargoIDsForm(@"Enter Cargo Ids TT1");
            enterCargoIDsForm.SetValue(enterCargoIDsForm.txtFirstID, strFirstID);
            enterCargoIDsForm.SetValue(enterCargoIDsForm.txtNumberToCreate, strNumberToCreate);
            enterCargoIDsForm.ClickButton(enterCargoIDsForm.btnGenerate);
            enterCargoIDsForm.ClickButton(enterCargoIDsForm.btnOK);


        }

        public void AddMulitpleCargoIds(string cargoIds)
        {
            ClickButton(btnCargoIDs, 3000);
            enterCargoIDsForm = new EnterCargoIDsForm(@"Enter Cargo Ids TT1");
            enterCargoIDsForm.SetValue(enterCargoIDsForm.txtCargoIds, cargoIds);
            enterCargoIDsForm.ClickButton(enterCargoIDsForm.btnGenerate);
            enterCargoIDsForm.ClickButton(enterCargoIDsForm.btnOK);


        }

        public void ClickWeightButton()
        {
            GetFormControl(@"6187", form, ref btnWeight, @"Weight button", ControlType.Button);
            ClickButton(btnWeight);
        }
        ~RoadGateDetailsReceiveForm()
        {


        }

    }
}
