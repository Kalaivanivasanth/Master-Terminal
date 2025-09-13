using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using MTNAutomationTests.BaseClasses.MasterTerminal;
using System.Drawing;
using FlaUI.Core.Input;

namespace MTNAutomationTests.FormObjects.MasterTerminal
{
    class HazardousDetailsForm : FormObjectBase
    {

        public AutomationElement cmbHazardClass;
        public AutomationElement txtUNDGCodes;
        public AutomationElement cmbPackingGroup;
        public AutomationElement btnAdd;
        public AutomationElement btnOK;
        public AutomationElement tblHazardDetails;

        public HazardousDetailsForm(string formTitle = null)
        {

            GetFormElement(formTitle);

            GetFormControl(@"4011", form, ref cmbHazardClass, @"Hazard Class combobox", ControlType.ComboBox);
            GetFormControl(@"4014", form, ref txtUNDGCodes, @"UNDG Codes textbox", ControlType.Edit);
            GetFormControl(@"4017", form, ref cmbPackingGroup, @"Packing Group combobox", ControlType.ComboBox);
            GetFormControl(@"4001", form, ref btnAdd, @"Add button", ControlType.Button);
            GetFormControl(@"2003", form, ref btnOK, @"OK button", ControlType.Button);
            GetFormControl(@"4002", form, ref tblHazardDetails, @"Hazard Details table", ControlType.Table);
        }


        ~HazardousDetailsForm()
        {

        }


    }


}
