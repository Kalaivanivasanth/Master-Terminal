using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39920 : MTNBase
    {

        RoadGateDetailsReceiveForm roadGateDetailsReceiveForm;
       
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void ReceiveISOWithGeneratorChassiNoGenerator()
        {
            MTNInitialize();

            //1. go to road gate and enter details of truck and trailer
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"39920");
            roadGateForm.GetTrailerFields();
            roadGateForm.SetRegoCarrierGate("39920");
            roadGateForm.txtTrailerID.SetValue(@"TRAILER39920A");
            roadGateForm.cmbTrailerType.SetValue(@"TRAILER39771");
            roadGateForm.cmbTrailerOperator.SetValue("ABOC", doDownArrow: true);
            roadGateForm.cmbImex.SetValue(@"Storage");
            roadGateForm.btnReceiveFull.DoClick();

            //2. enter the details of the receival container (reefer in this case)
            roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive Full Container TT1");
            roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG39920A01");
            roadGateDetailsReceiveForm.CmbCommodity.SetValue(Commodity.ICEC, doDownArrow: true);
            roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("4500");
            roadGateDetailsReceiveForm.CmbImex.SetValue(IMEX.Storage, doDownArrow: true);
            roadGateDetailsReceiveForm.TxtReeferCarriageTemperature.SetValue(@"24");
            roadGateDetailsReceiveForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            //3. check the gate items before saving
            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Chassis~Detail^TRAILER39920A");
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Full~Detail^JLG39920A01");
            roadGateForm.TblGateItems.FindClickRow([
                "Type^Receive Chassis~Detail^TRAILER39920A",
                "Type^Receive Full~Detail^JLG39920A01"
            ]);
            roadGateForm.btnSave.DoClick();


            //4. Check that generator is on the reefer and the reefer is on the trailer
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG39920A01");
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG39920A01");
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG39920A01"]);
            cargoEnquiryForm.GetLocationTable(@"4084");
            string strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblLocationDetails, @"Location ID");
            Assert.IsTrue(strValue.Equals(@"TRAILER39920A 1"), "Location ID expected to be TRAILER39920A, actual is: " + strValue);
  
            cargoEnquiryForm.CargoEnquiryReeferTab();
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblReeferEdit, @"Generator");
            Assert.IsTrue(strValue.Equals(@"GEN39920A"), "Generator expected to be GEN39920A, actual is: " + strValue);
            cargoEnquiryForm.CloseForm();

            //5. go to road ops and complet the gate in process.
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("", new [] { "39920" });

        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_39920_";

            // Cargo on Site Delete
            CreateDataFileToLoad(@"CargoOnSiteDelete.xml",
                "<?xml version='1.0'?> <JMTInternalCargoOnSite>\n<AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>2230</isoType>\n		<id>JLG39920A01</id>\n		<operatorCode>ABOC</operatorCode>\n		<messageMode>D</messageMode>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>Trailer</cargoTypeDescr>\n		<operatorCode>ABOC</operatorCode>\n		<id>TRAILER39920A</id>\n		<messageMode>D</messageMode>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	</AllCargoOnSite>\n </JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
