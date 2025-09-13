using DataObjects.LogInOutBO;
using FlaUI.Core.Definitions;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Gate_Functions.Vehicle;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41868 : MTNBase
    {
        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        ConfirmationFormOKwithText _confirmationFormOKwithText;
        VehicleVisitForm _vehicleVisitForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_41868_";
            BaseClassInitialize_New(testContext);
        }
        
        [TestInitialize]
        public new void TestInitialize() {}


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void ChangingTareWeight()
        {
            MTNInitialize();

           // 1. Open road gate form and enter vehicle visit details
           FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
           roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"41868" );
            roadGateForm.txtRegistration.SetValue(@"41868");
            roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.txtNewItem.SetValue(@"JLG41868A01");
            
            // 2. In road gate details form, change the isotype, click save and check/accept warning 
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Full Container TT1");
            _roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO220A, doDownArrow: true);
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("5000");
            _roadGateDetailsReceiveForm.BtnSave.DoClick();

           warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
           warningErrorForm.btnSave.DoClick();

            string visitNumber = roadGateForm.txtVisitNumber.GetText();
            roadGateForm.TblGateItems.FindClickRow(["Type^Receive Full~Detail^JLG41868A01; FLS; 220A"]);
            roadGateForm.btnSave.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            string[] warningErrorToCheck =
            {
               "Code :88492. Changing ISO Type to 220A will mean the Tare Weight 3306.937 will not match the default Tare Weight for ISO Type 220A of 5097.092. Please manually update the Tare Weight as required.",
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();

            // 3. In road gate form save off visit number for later, check the cargo details and save. check and accept warnings
            //string visitNumber = roadGateForm.txtVisitNumber.GetText();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Full~Detail^JLG41868A01; FLS; 220A");
           /* roadGateForm.TblGateItems.FindClickRow(["Type^Receive Full~Detail^JLG41868A01; FLS; 220A"]);
            roadGateForm.btnSave.DoClick();*/
            /*warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorToCheck = new string[]
            {
                "Code :75780. Booking is unknown or invalid."
            };
           warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
           warningErrorForm.btnSave.DoClick();*/


            // 4. Open Road Ops and cancel visit, add cancel reason, check/accept warnings
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^41868", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Vehicle Id^41868" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Delete Current Entry");
            _confirmationFormOKwithText = new ConfirmationFormOKwithText(@"Enter the Cancellation value:", controlType: ControlType.Pane);
            //confirmationFormOKwithText.SetValue(confirmationFormOKwithText.txtInput, @"Cancellation: Test 41868");
            _confirmationFormOKwithText.txtInput.SetValue(@"Cancellation: Test 41868");
            _confirmationFormOKwithText.btnOK.DoClick();
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Road Ops TT1");
            warningErrorToCheck = new string[]
            {
                "Code :75005. Are you sure you want to Delete ISO Container Receival Road 41868?"
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();


            // 5. open vehicle visit enquiry form and check that no cargo was received/released.
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Vehicle Visit Enquiry", forceReset: true);

            _vehicleVisitForm = new VehicleVisitForm(@"Vehicle Visit Enquiry TT1");
            MTNControlBase.SetValueInEditTable(_vehicleVisitForm.tblSearchCriteria, @"Visit Number", visitNumber);
            //vehicleVisitForm.btnSearch.DoClick();
            _vehicleVisitForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_vehicleVisitForm.tblVisits, @"Vehicle^41868~Cargo In^0~Cargo Out^0~Visit Number^" + visitNumber, rowHeight: 16);
            _vehicleVisitForm.TblVisits.FindClickRow([
                $"Vehicle^41868~Cargo In^0~Cargo Out^0~Visit Number^{visitNumber}"
            ]);
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {

             //Delete Prenote
            CreateDataFileToLoad(@"DeletePrenote.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED/EDI_STATUS_DBLOADED_PARTIAL_X</operationsToPerformStatuses>\n        <Prenote Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <isoType>4200</isoType>\n            <id>JLG41868A01</id>\n            <commodity>GEN</commodity>\n            <weight>1500.1847</weight>\n            <tareWeight>1500</tareWeight>\n            <imexStatus>Export</imexStatus>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>FLS</operatorCode>\n            <dischargePort>NZNPE</dischargePort>\n            <transportMode>Road</transportMode>\n            <messageMode>D</messageMode>\n        </Prenote>\n    </AllPrenote>\n</SystemXMLPrenote>\n");


            // Create prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <Prenote Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <isoType>4200</isoType>\n            <id>JLG41868A01</id>\n            <commodity>GEN</commodity>\n            <weight>1500.1847</weight>\n            <tareWeight>1500</tareWeight>\n            <imexStatus>Export</imexStatus>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>FLS</operatorCode>\n            <dischargePort>NZNPE</dischargePort>\n            <transportMode>Road</transportMode>\n        </Prenote>\n    </AllPrenote>\n</SystemXMLPrenote>\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
