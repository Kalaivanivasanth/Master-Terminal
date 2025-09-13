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
    public class TestCase39814 : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
    
        [TestCleanup]
        public new void TestCleanup() =>base.TestCleanup();
        
        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>("USERTIGEN");
        }


        [TestMethod]
        public void ReleaseTIMasterContainerWithGeneratorAttached()
        {

            MTNInitialize();

            // Step 1
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate", forceReset: false);
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"39814");

            // Step 2
            roadGateForm.SetRegoCarrierGate("39814");

            // Step 3
            roadGateForm.btnReceiveFull.DoClick();
            RoadGateDetailsReceiveForm roadGateDetailsRecieveForm = new RoadGateDetailsReceiveForm(@"Receive Full Container TT1");

            // Step 4
            roadGateDetailsRecieveForm.TxtCargoId.SetValue(@"JLG39814A01");
            roadGateDetailsRecieveForm.MtTotalWeight.SetValueAndType("4000");
            roadGateDetailsRecieveForm.CmbCommodity.SetValue(Commodity.ICEC, doDownArrow: true);
            roadGateDetailsRecieveForm.CmbImex.SetValue(IMEX.Storage,additionalWaitTimeout: 2000, doDownArrow: true);
            roadGateDetailsRecieveForm.TxtReeferCarriageTemperature.SetValue(@"-5");
            Assert.IsTrue(roadGateDetailsRecieveForm.CmbIsoType.GetValue() == "2230\tPowered Reefer",
                @"TestCase39814 - ISO Type did not default to 2230");
            Assert.IsTrue(roadGateDetailsRecieveForm.CmbOperator.GetValue() == "APL\tAmerican Shipping Line",
                @"TestCase39814 - Operator did not default to APL");


            // Step 5
            roadGateDetailsRecieveForm.BtnSave.DoClick();
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");

            // Step 6
            string[] msgToCheck =  
            {
                @"Code :75016. The Container Id (JLG39814A01) failed the validity checks and may be incorrect."
            };
            warningErrorForm.CheckWarningsErrorsExist(msgToCheck);
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Full~Detail^JLG39814A01; APL; 2230");
            roadGateForm.TblGateItems.FindClickRow(["Type^Receive Full~Detail^JLG39814A01; APL; 2230"]);
            // Step 7
            roadGateForm.btnSave.DoClick();
            
            // Step 8
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            
            // Step 9
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date  MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^39814~Cargo ID^JLG39814A01~Commodity^ICEC", ClickType.Click, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { @"Vehicle Id^39814~Cargo ID^JLG39814A01~Commodity^ICEC" });

            // Step 10
            roadOperationsForm.GetInfoTableDetails();
            roadOperationsForm.ValidateDataInInfoTable(@"Attached GenSet^GEN39814A");

            // Step 11
            roadOperationsForm.DoMoveIt();

            // Step 12
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^39814~Cargo ID^JLG39814A01~Commodity^ICEC", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { @"Vehicle Id^39814~Cargo ID^JLG39814A01~Commodity^ICEC" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");

            // Step 13
            roadGateForm.SetFocusToForm();

            // Step 14
            //roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            //roadGateForm.txtRegistration.SetValue(@"39814");
            //roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.SetRegoCarrierGate("39814");

            // Step 15
            roadGateForm.btnReleaseFull.DoClick();
            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm(formTitle: @"Release Full Container  TT1");
            roadGateDetailsReleaseForm.TxtCargoId.SetValue(@"JLG39814A01");
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            // Step 16
            roadGateForm.btnSave.DoClick();

            // Step 17
            roadOperationsForm.SetFocusToForm();

            // Step 19
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^39814~Cargo ID^JLG39814A01~Commodity^ICEC", ClickType.Click, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { @"Vehicle Id^39814~Cargo ID^JLG39814A01~Commodity^ICEC" });

            // Step 20
            roadOperationsForm.GetInfoTableDetails();
            roadOperationsForm.ValidateDataInInfoTable(@"Attached GenSet^GEN39814A");

            // Step 21
            //roadOperationsForm.btnMoveIt.DoClick();
            roadOperationsForm.DoMoveIt();

            // Step 22
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^39814~Cargo ID^JLG39814A01~Commodity^ICEC", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { @"Vehicle Id^39814~Cargo ID^JLG39814A01~Commodity^ICEC" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");

            // Step 23
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG39814A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"Off Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"Generator^GEN39814A~ID^JLG39814A01~State^Off Site");
            cargoEnquiryForm.tblData2.FindClickRow(["Generator^GEN39814A~ID^JLG39814A01~State^Off Site"]);     
        }


       

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_39814_";
            
            // Cargo on Site Delete
            CreateDataFileToLoad(@"CargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG39814A01</id>\n         <isoType>2230</isoType>\n         <voyageCode>MSCK000002</voyageCode>\n         <operatorCode>APL</operatorCode>\n         <dischargePort>NZAKL</dischargePort>\n         <locationId>MKBS01</locationId>\n         <weight>4000.0000</weight>\n         <imexStatus>Storage</imexStatus>\n         <totalQuantity>1</totalQuantity>\n         <commodity>ICEC</commodity>\n		 <messageMode>D</messageMode>\n      </CargoOnSite>\n	  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Generator</cargoTypeDescr>\n         <id>JLG39814A</id>\n         <isoType>2230</isoType>\n         <voyageCode>MSCK000002</voyageCode>\n         <operatorCode>APL</operatorCode>\n         <dischargePort>USJAX</dischargePort>\n         <locationId>MKBS01</locationId>\n         <weight>500.0000</weight>\n         <imexStatus>Storage</imexStatus>\n         <totalQuantity>1</totalQuantity>\n 	 <messageMode>D</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");


            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }

}
