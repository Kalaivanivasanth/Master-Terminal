using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.Controls;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.General_Cargo;
using FlaUI.Core.Input;
using MTNGlobal.EnumsStructs;



namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase66306 : MTNBase
    {
        private CargoTypesForm _cargoTypesForm;
        private ConfirmationFormYesNo _confirmationFormYesNo;
        private CargoSubTypesForm _cargoSubTypesForm;
        SystemAdminForm _systemAdminForm;


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize()
        {
        }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();




        [TestMethod]
        public void VoyageInvoicingValidation()
        {
            MTNInitialize();

            FormObjectBase.NavigationMenuSelection(NavigatorMenus.CargoTypes);
            _cargoTypesForm = new CargoTypesForm();

            MTNControlBase.SetValueInEditTable(_cargoTypesForm.tblSearch, "Abbreviation", "66306");
            _cargoTypesForm.DoSearch();

            _cargoTypesForm.tblCargoDetails1.FindClickRow(new[] { "Abbreviation^66306" });
            _cargoTypesForm.DoDelete();


            _confirmationFormYesNo = new ConfirmationFormYesNo("Confirm Deletion");
            _confirmationFormYesNo.btnYes.DoClick();

            WarningErrorForm.CheckErrorMessagesExist("Warnings for Cargo type",
                new[] { "Code :96786. Cargo type 66306 is used in Invoice Mapping HMS/Site Occupation/ANL Container Line Pty Ltd/Default (Quantity)/Default (0 to MAX)/Site Occupation, are you sure you want to delete?" });

            _cargoTypesForm.DoCancel();
            _cargoTypesForm.OpenCargoSubTypes();

            _cargoSubTypesForm = new CargoSubTypesForm();
            _cargoSubTypesForm.tblCargoSubtypeDetails1.FindClickRow(new[] { "Code^SUB66306" });
            _cargoSubTypesForm.DoDelete();

            _confirmationFormYesNo = new ConfirmationFormYesNo("Confirm Deletion");
            _confirmationFormYesNo.btnYes.DoClick();
            WarningErrorForm.CheckErrorMessagesExist("Warnings for Cargo Subtype",
                new[] { "Code :96787. Cargo subtype SUB66306 is used in Invoice Mapping HMS/Site Occupation/ANL Container Line Pty Ltd/Default (Quantity)/Default (0 to MAX)/Site Occupation, are you sure you want to delete?" });
            _cargoSubTypesForm.DoCancel();

            FormObjectBase.MainForm.OpenSystemAdminFromToolbar();


            _systemAdminForm = new SystemAdminForm(@"System Administration");

            _systemAdminForm.cmbTable.SetValue(@"Commodities");

            _systemAdminForm.txtFilter.SetValue(@"6306");

            Wait.UntilResponsive(_systemAdminForm.TblAdministrationItemsRH16A.GetElement());

            _systemAdminForm.TblAdministrationItemsRH19A.FindClickRow(new[] { "Commodity Code^6306~Description^66306" }, searchType: SearchType.Exact);

            _systemAdminForm.DoDelete();

            WarningErrorForm.CheckErrorMessagesExist("Warnings for Delete",
                new[] { "Code :96788. Commodity 6306 is used in Invoice Mapping HMS/Site Occupation/ANL Container Line Pty Ltd/Default (Quantity)/Default (0 to MAX)/Site Occupation - Charged, are you sure you want to delete?" });

            _systemAdminForm.CloseForm();

        }

    }
}
