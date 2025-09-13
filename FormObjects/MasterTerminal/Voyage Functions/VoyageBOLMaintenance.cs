using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using MTNAutomationTests.BaseClasses.MasterTerminal;

namespace MTNAutomationTests.FormObjects.MasterTerminal.Voyage_Functions
{
    
    class VoyageBOLMaintenanceForm : FormObjectBase
    {
        
        // Header Buttons
        public AutomationElement btnFind;
        public AutomationElement cmbVoyage;

        public AutomationElement tblBOLMaintUnassignedCargo;
        public AutomationElement tblBillsofLading;


        public VoyageBOLMaintenanceForm(string formTitle = null)
        {
            GetFormElement(formTitle);

            contextMenus = new[] { @"Table Context" };
        }
        public void GetSearcher()
        { 
           
            GetFormControl(@"2087", form, ref cmbVoyage, @"Voyage combobox", ControlType.ComboBox);
            GetFormControl(@"4043", form, ref btnFind, @"Find button", ControlType.Pane);


        }

        /// <summary>
        /// Get BOL Maintenance table
        /// </summary>
        public void CreateBOLMaintenanceTables()
        {

            GetFormControl(@"2021", form, ref tblBOLMaintUnassignedCargo, @"BOL Maintenance Unassigned Cargo table", ControlType.Table);

        }
         public void BillsofLading()
        {
            GetFormControl(@"2019", form, ref tblBillsofLading, @"Bills of Lading table", ControlType.Table);

           
        }

        ~VoyageBOLMaintenanceForm()
        {

        }

        
    }
}
