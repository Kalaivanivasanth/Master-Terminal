using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44070 : MTNBase
    {

        const string TestCaseNumber = "44070";
        
        FleetMaintenanceForm _fleetMaintenanceForm;
        FleetMaintenanceISOContainerForm _fleetMaintenanceISOContainerForm;
        ConfirmationFormYesNo _confirmationFormYesNo;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();

           CallJadeScriptToRun(TestContext, $"resetData_{TestCaseNumber}");
        }


        [TestMethod]
        public void DeleteFleetMaintenanceFile()
        {
            MTNInitialize();
            
            // 1. Navigate to Fleet Maintenance and open ISO container fleet
            FormObjectBase.NavigationMenuSelection(@"System Ops|Fleet Maintenance");
            _fleetMaintenanceForm = new FleetMaintenanceForm();

            MTNControlBase.SetValueInEditTable(_fleetMaintenanceForm.tblSearchDetails, @"Fleet Type", @"I", rowDataType: EditRowDataType.ComboBox);
            //fleetMaintenanceForm.btnSearch.DoClick();
            //fleetMaintenanceForm.btnNew.DoClick();
            _fleetMaintenanceForm.DoSearch();
            _fleetMaintenanceForm.DoNew();

                // 2. Add a new container to fleet file
            _fleetMaintenanceISOContainerForm = new FleetMaintenanceISOContainerForm();
            _fleetMaintenanceISOContainerForm.txtID.SetValue("JLG44070A01");
            _fleetMaintenanceISOContainerForm.cmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _fleetMaintenanceISOContainerForm.cmbOwner.SetValue("MSC", doDownArrow: true);
            _fleetMaintenanceISOContainerForm.cmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);

            _fleetMaintenanceISOContainerForm.btnOK.DoClick();

            // 3. Delete the container from the fleet file
            //MTNControlBase.FindClickRowInTable(fleetMaintenanceForm.tblFleetItems, @"ID^JLG44070A01~ISO^2200~Operator^MSC~Owner^MSC",rowHeight: 18);
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_fleetMaintenanceForm.tblFleetItems, @"ID^JLG44070A01~ISO^2200~Operator^MSC~Owner^MSC",rowHeight: 18);
            _fleetMaintenanceForm.TblFleetItems.FindClickRow(new[] { @"ID^JLG44070A01~ISO^2200~Operator^MSC~Owner^MSC" });
            //fleetMaintenanceForm.btnDelete.DoClick();
            _fleetMaintenanceForm.DoDelete();

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"ISO Container Maintenance");
            _confirmationFormYesNo.btnYes.DoClick();

            //4. Ensure the container on site has not been deleted.
            // Thursday, 30 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG44070A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44070A01", rowHeight: 18);
            cargoEnquiryForm.tblData2.FindClickRow(new[] { @"ID^JLG44070A01" });

        }

    }

}
