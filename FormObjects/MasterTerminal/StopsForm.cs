using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using MTNAutomationTests.BaseClasses.MasterTerminal;
using System.Drawing;
using FlaUI.Core.Input;

namespace MTNAutomationTests.FormObjects.MasterTerminal
{
    class StopsForm : FormObjectBase
    {
        
        // Header Buttons
        public AutomationElement tblStops;
        public AutomationElement btnSave;
        public AutomationElement btnSaveAndClose;
        public AutomationElement tblAddStops;


        public StopsForm(string formTitle = null)
        {

            GetFormElement(formTitle);

            GetFormControl(@"2004", form, ref tblStops, @"Stops Table", ControlType.Table);
            GetFormControl(@"2009", form, ref btnSave, @"Save button", ControlType.Button);
            GetFormControl(@"2014", form, ref btnSaveAndClose, @"Save and Close button", ControlType.Button);
            GetFormControl(@"2005", form, ref tblAddStops, @"Add Stops Table", ControlType.Table);
        }

        
        ~StopsForm()
        {

        }


    }


}
