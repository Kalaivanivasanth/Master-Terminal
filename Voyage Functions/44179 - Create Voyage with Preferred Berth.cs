using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.Harbor;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44179 : MTNBase
    {

        HarbourAdminForm _harbourAdminForm;
        VoyageEnquiryForm _voyageEnquiryForm;
        VoyageDefinitionForm _voyageDefinitionForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup()
        {
             FormObjectBase.NavigationMenuSelection(@"Harbour|Harbour Admin", forceReset: true);
             _harbourAdminForm = new HarbourAdminForm();
             _harbourAdminForm.cmbTable.SetValue(@"Harbour");
             // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date  MTNControlBase.FindClickRowInTable(_harbourAdminForm.tblAdminItems, @"Code^HMSAVON", rowHeight: 16);
             _harbourAdminForm.TblAdminItems.FindClickRow(new[] {"Code^HMSAVON" });
             _harbourAdminForm.GetTabTableGeneric(@"Harbour Defaults", @"4153");
             _harbourAdminForm.DoEdit();
             _harbourAdminForm.GetEditTable(@"4153");
             MTNControlBase.SetValueInEditTable(_harbourAdminForm.tblEditTable, @"Default Arrival Berth", @"Sea (SEA)",
                 EditRowDataType.ComboBoxEdit);
             MTNControlBase.SetValueInEditTable(_harbourAdminForm.tblEditTable, @"Default Depart Berth",
                 @"07 (Cave Rock) (07)", EditRowDataType.ComboBoxEdit, doDownArrow: true, searchSubStringTo: 14); 
             _harbourAdminForm.DoSave();

             /*/ Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date 
             warningErrorForm = new WarningErrorForm(@"Warnings for Harbour");

             var warningsToCheck = new []
             {
                 @"Code :89550. An input is required for the Harbour Email Group field."
             };
             warningErrorForm.CheckWarningsErrorsExist(warningsToCheck);
             warningErrorForm.btnSave.DoClick();*/
             WarningErrorForm.CompleteWarningErrorForm("Warnings for Harbour",
                 new[] { "Code :89550. An input is required for the Harbour Email Group field." });
             
             _harbourAdminForm.CloseForm();

             base.TestCleanup();
        }


        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();

            FormObjectBase.NavigationMenuSelection(@"Harbour|Harbour Admin");
            HarbourAdminForm harbourAdminForm = new HarbourAdminForm();
            //MTNControlBase.SetValue(harbourAdminForm.cmbTable, @"Harbour");
            harbourAdminForm.cmbTable.SetValue(@"Harbour");
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(harbourAdminForm.tblAdminItems, @"Code^HMSAVON", rowHeight: 16);
            harbourAdminForm.TblAdminItems.FindClickRow(new[] {"Code^HMSAVON" });
            //MTNControlBase.FindTabOnForm(harbourAdminForm.tabHarbourAdminTab, @"Harbour Defaults");
            harbourAdminForm.GetTabTableGeneric(@"Harbour Defaults", @"4153");
            //FormObjectBase.ClickButton(harbourAdminForm.btnEdit);
            //harbourAdminForm.btnEdit.DoClick();
            //harbourAdminForm.DoToolbarClick(harbourAdminForm.MainToolbar,
            //    (int)HarbourAdminForm.Toolbar.MainToolbar.Edit, "Edit");
            harbourAdminForm.DoEdit();
            harbourAdminForm.GetEditTable(@"4153");
            MTNControlBase.SetValueInEditTable(harbourAdminForm.tblEditTable, @"Default Arrival Berth", @" ", EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(harbourAdminForm.tblEditTable, @"Default Depart Berth", @" ", EditRowDataType.ComboBoxEdit);
            //FormObjectBase.ClickButton(harbourAdminForm.btnSave);
            //harbourAdminForm.btnSave.DoClick();
            //harbourAdminForm.DoToolbarClick(harbourAdminForm.MainToolbar,
            //    (int)HarbourAdminForm.Toolbar.MainToolbar.Save, "Save");
            harbourAdminForm.DoSave();

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date 
            /*WarningErrorForm warningErrorForm = new WarningErrorForm(@"Warnings for Harbour");

            string[] warningsToCheck = new string[]
            {
                @"Code :89548. An input is required for the Default Arrival Berth field.",
                @"Code :89549. An input is required for the Default Departure Berth field.",
                @"Code :89550. An input is required for the Harbour Email Group field."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningsToCheck);
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Harbour", new[]
            {
                "Code :89548. An input is required for the Default Arrival Berth field.",
                "Code :89549. An input is required for the Default Departure Berth field.",
                "Code :89550. An input is required for the Harbour Email Group field." });

            harbourAdminForm.CloseForm();

            //due to a bug in MTN have to close an re-open to delete 2 voyages in a row
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenVoyageEnquiryFromToolbar();
            _voyageEnquiryForm = new VoyageEnquiryForm();
            _voyageEnquiryForm.DeleteVoyageByVoyageCode(@"PDC00001");
            _voyageEnquiryForm.CloseForm();

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Voyage Enquiry");
            FormObjectBase.MainForm.OpenVoyageEnquiryFromToolbar();
            _voyageEnquiryForm = new VoyageEnquiryForm();
            _voyageEnquiryForm.DeleteVoyageByVoyageCode(@"PDC00002");
            _voyageEnquiryForm.CloseForm();

            base.TestInitialize();
        }


        [TestMethod]
        public void CreateVoyageWithPreferredBerths()
        {
            MTNInitialize();

            string dateTomorrow = DateTime.Today.Date.AddDays(1).ToString(@"ddMMyyyy");
            string dateTodayPlus2 = DateTime.Today.Date.AddDays(2).ToString(@"ddMMyyyy");
            string dateTodayPlus3 = DateTime.Today.Date.AddDays(3).ToString(@"ddMMyyyy");

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenVoyageEnquiryFromToolbar();
            _voyageEnquiryForm = new VoyageEnquiryForm();
            _voyageEnquiryForm.btnNewVoyage.DoClick();
            _voyageDefinitionForm = new VoyageDefinitionForm();

            _voyageDefinitionForm.cmbVessel.SetValue(@"PERLA DEL CARIBE - (PDC)");
            _voyageDefinitionForm.txtVoyageCode.SetValue(@"PDC00001");
            _voyageDefinitionForm.TxtDepartureDate.SetValue(dateTomorrow);
            _voyageDefinitionForm.TxtDepartureTime.SetValue(@"12:00");
            _voyageDefinitionForm.cmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _voyageDefinitionForm.BerthageDefaultsTab();
            MTNControlBase.SetValue(_voyageDefinitionForm.rdoToOtherBerth);//.Click();
            _voyageDefinitionForm.cmbToOtherBerth.SetValue(@"Brunt Quay (BQ)");
            _voyageDefinitionForm.DoSave();
            _voyageDefinitionForm.DoNew();
            _voyageDefinitionForm.cmbVessel.SetValue(@"PERLA DEL CARIBE - (PDC)");
            _voyageDefinitionForm.txtVoyageCode.SetValue(@"PDC00002");
            _voyageDefinitionForm.TxtArrivalDate.SetValue(dateTodayPlus2);
            _voyageDefinitionForm.TxtArrivalTime.SetValue(@"12:00");
            _voyageDefinitionForm.TxtDepartureDate.SetValue(dateTodayPlus3);
            _voyageDefinitionForm.TxtDepartureTime.SetValue(@"12:00");
            _voyageDefinitionForm.cmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _voyageDefinitionForm.BerthageDefaultsTab();
            MTNControlBase.SetValue(_voyageDefinitionForm.rdoToPreferredBerth);//.Click();
            _voyageDefinitionForm.cmbToPreferredBerth.SetValue(@"BERTH44179");

            // voyageDefinitionForm.btnSaveVoyage.DoClick();
            _voyageDefinitionForm.DoSave();
            _voyageDefinitionForm.CloseForm();

            //make sure both voyages exist
            _voyageEnquiryForm.SetFocusToForm();
            _voyageEnquiryForm.FindVoyageByVoyageCode(@"PDC00001");
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages, @"Code^PDC00001");
            _voyageEnquiryForm.TblVoyages.FindClickRow(new[] { "Code^PDC00001" });
            _voyageEnquiryForm.FindVoyageByVoyageCode(@"PDC00002");
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages, @"Code^PDC00002");
            _voyageEnquiryForm.TblVoyages.FindClickRow(new[] { "Code^PDC00002" });
            

        }



    }

}
