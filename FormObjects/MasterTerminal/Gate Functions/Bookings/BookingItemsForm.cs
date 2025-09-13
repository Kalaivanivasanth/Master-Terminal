using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using MTNAutomationTests.BaseClasses.MasterTerminal;

namespace MTNAutomationTests.FormObjects.MasterTerminal.Gate_Functions.Bookings
{
    
    class BookingItemsForm : FormObjectBase
    {

        public AutomationElement txtReference; 
        public AutomationElement cmbOperator;  
        public AutomationElement cmbVoyage;  
        public AutomationElement cmbDischargePort;  
        public AutomationElement cmbDestinationPort;  
        public AutomationElement chkAdditionalRequirements;  
        public AutomationElement btnAdd;  
        public AutomationElement btnSave;
        public AutomationElement btnRoll;
        public AutomationElement tblItems;
        public AutomationElement btnCancel;
        public AutomationElement btnSplit;
        public AutomationElement cmbStevedore;
        public AutomationElement btnHazardDetails;
        
        // when additional checked
        public AutomationElement chkReleaseEmpties; 

        public BookingItemsForm(string formTitle = null)
        {
            // get the form
            GetFormElement(formTitle);

  
            GetFormControl(@"2003", form, ref txtReference, @"Reference textbox", ControlType.Edit);
            GetFormControl(@"2009", form, ref cmbOperator, @"Operator combobox", ControlType.ComboBox);
            GetFormControl(@"2018", form, ref cmbVoyage, @"Voyage combobox", ControlType.ComboBox);
            GetFormControl(@"2014", form, ref cmbDischargePort, @"Discharge Port combobox", ControlType.ComboBox);
            GetFormControl(@"2022", form, ref cmbDestinationPort, @"Destination Port combobox", ControlType.ComboBox);
            GetFormControl(@"2068", form, ref btnAdd, @"Items - Add button", ControlType.Button);
            GetFormControl(@"2035", form, ref btnSave, @"Save button", ControlType.Button);
            GetFormControl(@"2038", form, ref btnRoll, @"Roll button", ControlType.Button);
            GetFormControl(@"2037", form, ref btnSplit, @"Split button", ControlType.Button);
            GetFormControl(@"2036", form, ref btnCancel, @"Edit button", ControlType.Button);
            GetFormControl(@"2033", form, ref chkAdditionalRequirements, @"Additional Requirements checkbox", ControlType.CheckBox);
            GetFormControl(@"2031", form, ref tblItems, @"Items table", ControlType.Table);
            GetFormControl(@"2154", form, ref cmbStevedore, @"Stevedore combobox", ControlType.ComboBox);
            GetFormControl(@"2152", form, ref btnHazardDetails, @"Hazard Details button", ControlType.Button);

        }

        public void showAdditionalRequirements()
        {

           
            GetFormControl(@"2029", form, ref chkReleaseEmpties, @"Release Empties checkbox", ControlType.CheckBox);

        }

        ~BookingItemsForm()
        {

        }

    }

}
