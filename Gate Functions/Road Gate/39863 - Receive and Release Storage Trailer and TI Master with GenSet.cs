using DataObjects;
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
    public class TestCase39863 : MTNBase
    {
        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        RoadGateDetailsReleaseForm _roadGateDetailsReleaseForm;

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
        public void ReceiveReleaseStorageTrailerWithGenSetAttached()
        {
            MTNInitialize();

            //1. go to road gate and enter details of truck and trailer
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"39863");
            roadGateForm.GetTrailerFields();
            roadGateForm.SetRegoCarrierGate("39863A");
            roadGateForm.txtTrailerID.SetValue(@"TRAILER39863A");
            roadGateForm.cmbTrailerType.SetValue(@"TRAILER39771");
            roadGateForm.cmbTrailerOperator.SetValue(Operator.ABOC, doDownArrow: true, searchSubStringTo: 3);
            roadGateForm.cmbImex.SetValue(IMEX.Storage);
            roadGateForm.btnReceiveEmpty.DoClick();

            //2. enter the details of the receival container
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive Empty Container TT1");
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG39863A01");
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("4000");

            //3. check the iso type and operator are correct from TI master
            string strValue = _roadGateDetailsReceiveForm.CmbIsoType.GetValue();
            Assert.IsTrue(strValue.Contains(ISOType.ISO2200),"ISO Type expected to be 2200, actual is: " + strValue);
            strValue = _roadGateDetailsReceiveForm.CmbOperator.GetValue();
            Assert.IsTrue(strValue.Contains(Operator.APL), "Operator expected to be APL, actual is: " + strValue);

            //4. save and return to road gate
            _roadGateDetailsReceiveForm.BtnSave.DoClick();
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out");

            //5. check the gate items for correct information
            roadGateForm.SetFocusToForm();
            /*// Thursday, 13 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Chassis~Detail^TRAILER39863A");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Empty~Detail^JLG39863A01");*/
            roadGateForm.TblGateItems.FindClickRow(new[]
                { "Type^Receive Chassis~Detail^TRAILER39863A", "Type^Receive Empty~Detail^JLG39863A01" });
            roadGateForm.btnSave.DoClick();
            roadGateForm.CloseForm();

            //6. check cargo enquiry for correct information
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.TrailerChassis, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"TRAILER39863A");
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^TRAILER39863A" });
            cargoEnquiryForm.GetGenericTabTableDetails(@"Trailer/Chassis Details", @"4084");
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Generator");
            Assert.IsTrue(strValue.Equals(@"GEN39863A"), "Generator expected to be GEN39863A, actual is: " + strValue);
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Cargo Onboard");
            Assert.IsTrue(strValue.Equals(@"JLG39863A01"), "Cargo Onboard expected to be JLG39863A01, actual is: " + strValue);
            cargoEnquiryForm.CloseForm();

            //7. goto road ops and move trailer/MT container to yard
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new [] { "39863A" });

            //8. goto road gate and release trailer
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1");
            roadGateForm.GetTrailerFields();
            roadGateForm.SetRegoCarrierGate("39863A");
            roadGateForm.txtReleaseTrailerID.SetValue(@"TRAILER39863A");
            roadGateForm.btnReleaseEmpty.DoClick();

            //9. in release details enter container ID
            _roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm();
            _roadGateDetailsReleaseForm.TxtCargoId.SetValue(@"JLG39863A01");
            _roadGateDetailsReleaseForm.BtnSave.DoClick();

            //10 check gate items in road gate
            /*// Thursday, 13 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Chassis~Detail^TRAILER39863A");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Empty~Detail^JLG39863A01");*/
            roadGateForm.TblGateItems.FindClickRow([
                "Type^Release Chassis~Detail^TRAILER39863A", "Type^Release Empty~Detail^JLG39863A01"
            ]);
            roadGateForm.btnSave.DoClick();

            try
            {
                WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out");
            } 
            catch {}

            //12 check details are correct in cargo enquiry
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.TrailerChassis, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"TRAILER39863A");
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^TRAILER39863A" });
            cargoEnquiryForm.GetGenericTabTableDetails(@"Trailer/Chassis Details", @"4084");
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Generator");
            Assert.IsTrue(strValue.Equals(@"GEN39863A"), "Generator expected to be GEN39863A, actual is: " + strValue);
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Cargo Queued Onboard");
            Assert.IsTrue(strValue.Equals(@"JLG39863A01"), "Cargo Onboard expected to be JLG39863A01, actual is: " + strValue);
            cargoEnquiryForm.CloseForm();

            //13 move container and trailer and gate out
            // Thursday, 19 June 2025, Michael.Hill, TODO:  RoadOperationsForm.OpenMoveAllCargoAndRoadExit("Road Operations TT1", new [] { "39863" });

        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_39863_";

            // Delete Cargo on Site 
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>39863</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG39863A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>APL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n	  <messageMode>D</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>39863</TestCases>\n      <cargoTypeDescr>Trailer</cargoTypeDescr>\n      <id>TRAILER39863A</id>\n    <operatorCode>ABOC</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>500.0000</weight>\n      <imexStatus>Storage</imexStatus>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
