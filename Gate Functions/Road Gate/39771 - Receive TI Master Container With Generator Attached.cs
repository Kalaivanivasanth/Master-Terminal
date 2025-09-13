using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39771 : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            searchFor = @"_39771_";
            userName = "USERTIGEN";
            
            SetupAndLoadInitializeData(TestContext);

            LogInto<MTNLogInOutBO>(userName);
        }

        [TestMethod]
        public void ReceiveTIMasterContainerWithGeneratorAttached()
        {

            MTNInitialize();
            
            // Step 2
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"39771");

            // Step 3
            //roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            //roadGateForm.txtRegistration.SetValue(@"39771");
            //roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.SetRegoCarrierGate("39771");

            // Step 4
            roadGateForm.btnReceiveEmpty.DoClick();
            RoadGateDetailsReceiveForm roadGateDetailsForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Empty Container TT1");
            //roadGateDetailsForm.ShowContainerDetails();

            // Step 5
            roadGateDetailsForm.TxtCargoId.SetValue(@"JLG39771A01");
            //Keyboard.Press(VirtualKeyShort.TAB);
            roadGateDetailsForm.MtTotalWeight.SetValueAndType("4000");
            Assert.IsTrue(roadGateDetailsForm.CmbIsoType.GetValue() == "2230\tPowered Reefer",
                @"TestCase39771 - ISO Type did not default to 2230");
            Assert.IsTrue(roadGateDetailsForm.CmbOperator.GetValue() == "ABOC\tAB Cargo Operator",
                @"TestCase39771 - Operator did not default to ABOC");

            // Step 6
            roadGateDetailsForm.BtnSave.DoClick();
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");

            // Step 7
            string[] msgToCheck =
            {
                @"Code :75016. The Container Id (JLG39771A01) failed the validity checks and may be incorrect."
            };
            warningErrorForm.CheckWarningsErrorsExist(msgToCheck);
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Empty~Detail^JLG39771A01; ABOC; 2230");
            roadGateForm.TblGateItems.FindClickRow(["Type^Receive Empty~Detail^JLG39771A01; ABOC; 2230"]);
            // Step 8
            roadGateForm.btnSave.DoClick();
           
            // Step 9
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");

            // Step 10
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard,
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date     @"Vehicle Id^39771~Cargo ID^JLG39771A01~Commodity^MT", ClickType.Click, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Vehicle Id^39771~Cargo ID^JLG39771A01~Commodity^MT" });

            // Step 11
            roadOperationsForm.GetInfoTableDetails();
            roadOperationsForm.ValidateDataInInfoTable(@"Attached GenSet^GEN39649B");

            // Step 12
            //roadOperationsForm.btnMoveIt.DoClick();
            roadOperationsForm.DoMoveIt();

            // Step 13
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard,
             // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date    @"Vehicle Id^39771~Cargo ID^JLG39771A01~Commodity^MT", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new[] { "Vehicle Id^39771~Cargo ID^JLG39771A01~Commodity^MT" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");
        }


       


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0'?> <JMTInternalCargoOnSite>\n<AllCargoOnSite>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n	<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>2230</isoType>\n		<id>JLG39771A01</id>\n		<locationId>MKBS01</locationId>\n		<messageMode>D</messageMode>\n		<imexStatus>Storage</imexStatus>\n		<operatorCode>ABOC</operatorCode>\n		<weight>4000</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	</AllCargoOnSite>\n </JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
