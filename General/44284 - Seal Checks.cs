using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44284 : MTNBase
    {
        RoadGateDetailsReceiveForm roadGateDetailsReceiveForm;
        CargoEnquiryTransactionForm cargoEnquiryTransactionForm;
        ConfirmationFormYesNo confirmationFormYesNo;
        CargoChecksForm cargoChecksForm;

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
        public void SealChecks()
        {
            MTNInitialize();
            
            /* Resetting this test in the event of failure
            * 1. ensure prenote JLG44284A01 has been deleted
            * 2. ensure truck 44284 has been gated out and cargo moved onto site
            */

            // 1. Open road gate form and enter vehicle visit details
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"44284");

            roadGateForm.SetRegoCarrierGate("44284");
            roadGateForm.txtNewItem.SetValue(@"JLG44284A01", 10);

            // 2. In road gate details form, press click and accept warning 
            roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Full Container TT1");
            roadGateDetailsReceiveForm.BtnSave.DoClick();
            /*warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out");

            // 3. click on the seals check
            cargoChecksForm = new CargoChecksForm(formTitle: @"Cargo Checks Form TT1");
            MTNControlBase.FindClickRowInTableVHeader(cargoChecksForm.tblChecks, "Seals", tableRowHeight: 25);

            // 4. Find the shippers seal and change the seal number, click save and accept warning
            cargoChecksForm.ShowSealsTable();
            MTNControlBase.FindClickRowInTableVHeader(cargoChecksForm.tblSeals, "Shippers Seal",
                clickType: ClickType.DoubleClick, tableRowHeight: 25, tableHeaderRows: 2, tableHeaderRowHeight: 12,
                additionalOffsetX: 50);
            Keyboard.Type("XYZSEAL44284");

            cargoChecksForm.btnSave.DoClick();
            confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Warning", automationIdMessage: @"2", automationIdYes: @"3",automationIdNo: @"4");
            Assert.IsTrue(confirmationFormYesNo.lblMessage.Name == @"Value 'XYZSEAL44284' entered for 'Shippers Seal' does not match recorded seal value(s)
Do you want to continue and update the value(s) for 'Shippers Seal'?", @"Message: " + confirmationFormYesNo.lblMessage.Name + @" is incorrect");

            confirmationFormYesNo.btnYes.DoClick();

            // 5. complete seal check
            cargoChecksForm.btnComplete.DoClick();

            // 6. complete road gate and accept warnings
            roadGateForm.btnSave.DoClick();
            //warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            //warningErrorForm.btnSave.DoClick();

            // 7. Open cargo details, navigate to status tab and check that the seal is the new seal number
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG44284A01");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            //cargoEnquiryForm.CargoEnquiryGeneralTab();
            //MTNControlBase.FindTabOnForm(cargoEnquiryForm.tabGeneral, @"Status");
            cargoEnquiryForm.GetStatusTable(@"4087");
            string fieldData = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblStatus, @"Shippers Seal");
            Assert.IsTrue(fieldData == @"XYZSEAL44284", @"Field: Shippers Seal: " + fieldData + " doesn't equal: " + @"XYZSEAL44284");


            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();


            // 8. Open cargo transactions and ensure transaction "seal recorded" is correct
            cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>("Transactions for JLG44284A01 TT1");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Seal Recorded~Details^Recorded, SHIP : From  'SEAL44284' :  To  'XYZSEAL44284'", ClickType.Click);
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(["Type^Seal Recorded~Details^Recorded, SHIP : From  'SEAL44284' :  To  'XYZSEAL44284'"], ClickType.Click);
            cargoEnquiryTransactionForm.CloseForm();
            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.CloseForm();

            // 9. Goto road ops and complete cargo movement to yard and gate out truck
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(RoadOperationsForm.FormTitle, new [] { "44284" });
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_44284_";
            
            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0'?> <JMTInternalCargoOnSite  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n<AllCargoOnSite>\n     <operationsToPerform>Verify;Load To DB</operationsToPerform>\n <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses> \n    <CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>2200</isoType>\n		<dischargePort>NZAKL</dischargePort>\n		<id>JLG44284A01</id>\n		<locationId>MKBS07</locationId>\n		<messageMode>D</messageMode>\n		<imexStatus>Export</imexStatus>\n		<operatorCode>MSC</operatorCode>\n		<voyageCode>MSCK000002</voyageCode>\n		<weight>4500</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	</AllCargoOnSite>\n </JMTInternalCargoOnSite>\n\n");


            // Create prenote
            CreateDataFileToLoad(@"CreatePreNote.xml",
                "<?xml version='1.0'?>\n<JMTInternalPrenote>\n	<AllPrenote>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n		<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<Prenote Terminal='TT1'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<id>JLG44284A01</id>\n			<isoType>2200</isoType>\n			<commodity>GEN</commodity>\n			<imexStatus>Export</imexStatus>\n			<weight>1000</weight>\n			<voyageCode>MSCK000002</voyageCode>\n			<operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<AllSealDetails>\n				<SealDetails>\n					<sealEDIType>SHIP</sealEDIType>\n					<sealEDINum>SEAL44284</sealEDINum>\n				</SealDetails>\n			</AllSealDetails>\n			<messageMode>A</messageMode>\n		</Prenote>\n	</AllPrenote>\n</JMTInternalPrenote>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
