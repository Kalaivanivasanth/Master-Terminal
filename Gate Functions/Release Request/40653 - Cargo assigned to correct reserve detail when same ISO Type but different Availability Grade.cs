using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Release_Request
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40653 : MTNBase
    {
        RoadGatePickerForm roadGatePickerForm;
        RoadGateDetailsReleaseForm roadGateDetailsReleaseForm;
        ReleaseRequestForm releaseRequestForm;
        ReleaseRequestAddForm releaseRequestAddForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"resetData_40653");
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void CargoAssignedToCorrectReserveDetailWhenSameISOTypeButDifferentAvailabilityGrade()
        {
            MTNInitialize();
            
            // Step 4 - Open Road Gate
           // FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
           FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"40653");

            // Step 5 - Enter Registration = 40653, Carrier = American Auto Tpt, Gate = GATE, New Item = JLG40653ROAD001
            roadGateForm.txtRegistration.SetValue(@"40653");
            roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.txtNewItem.SetValue(@"JLG40653ROAD001", 10);

            // Step 6 - In Picker form, Select the 2nd item in the table and click the Ok button
            roadGatePickerForm = new RoadGatePickerForm(@"Picker"); 
            // MTNControlBase.FindClickRowInTable(roadGatePickerForm.tblPickerItems, @"Description^6 x 22G1  - GENERAL", clickType: ClickType.Click);
            roadGatePickerForm.TblPickerItems.FindClickRow(["Description^6 x 22G1  - GENERAL"], clickType: ClickType.Click);
            roadGatePickerForm.btnOK.DoClick();

            // Step 7 - In the Release by ID section, Enter Cargo ID = JLG40653A01 click the Tab key:
            roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm(@"Release Full Container  TT1");
            roadGateDetailsReleaseForm.TxtCargoId.SetValue(@"JLG40653A01");
            MTNKeyboard.Press(VirtualKeyShort.TAB);

            //roadGateDetailsReleaseForm.GetReleaseByTypeDetails();

            Assert.IsTrue(roadGateDetailsReleaseForm.CmbOperator.GetValue().Contains(@"MSL"));
            Assert.IsTrue(roadGateDetailsReleaseForm.CmbISOType.GetValue().Contains(@"22G1"));
            Assert.IsTrue(roadGateDetailsReleaseForm.CmbIMEX.GetValue().Contains(@"Export"));
            Assert.IsTrue(roadGateDetailsReleaseForm.CmbSubTerminal.GetValue().Contains(@"Depot")); 

            // Step 8 - Click the Save button
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            // Step 9 - Click the Save button
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            string[] warningErrorToCheck =
            {
                "Code :79688. Cargo JLG40653A01's availability status of Unavailable for Release does not match the request's availability status of Available for release"
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();

            // Step 10 - Click the Save button
            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();
            roadGateForm.CloseForm();

            // Step 11 - Open the Road Operations form
            // Step 12 - Find and select the vehicle 40653
            // Step 13 - Select the Release JLG40653A01 in the yard and click the Move It button in the toolbar  
            // Step 14 - Select 40653, right click and select Process Road Exit  
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(@"Road Operations TT1", new[] { "40653" }); // there are tests already testing the above steps, so using this method instead

            // Step 15 - Open the Release Request from 
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Release Requests", forceReset: true);
            FormObjectBase.MainForm.OpenReleaseRequestFromToolbar();

            // Step 16 - Enter View = Active, Type = Road, Release No = JLG40653ROAD001 and click the Search button:
            releaseRequestForm = new ReleaseRequestForm(formTitle: @"Release Requests TT1");
            releaseRequestForm.cmbView.SetValue(@"Active");
            releaseRequestForm.cmbType.SetValue(@"Road");
            MTNControlBase.SetValueInEditTable(releaseRequestForm.tblSearchTable, @"Release No", @"JLG40653ROAD001");

            //releaseRequestForm.btnFind.DoClick();
            releaseRequestForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(releaseRequestForm.tblReleaseRequests, @"Release No^JLG40653ROAD001", ClickType.DoubleClick);
            releaseRequestForm.TblReleaseRequests.FindClickRow(["Release No^JLG40653ROAD001"], ClickType.DoubleClick);
            // Step 18 - The line 6 X 22G1 - GENERAL should have under it the release container JLG40653A01.
            releaseRequestAddForm = new ReleaseRequestAddForm(@"Editing request JLG40653ROAD001... TT1");
            releaseRequestAddForm.ClickOnReleaseRequestItem(@"5 x 22G1 - GENERAL (0 released )");
            releaseRequestAddForm.ClickOnReleaseRequestItem(@"6 x 22G1 - GENERAL (1 released )");
            releaseRequestAddForm.ClickOnReleaseRequestItem(@"JLG40653A01");
            
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_40653_";

            // Release Request status updated to Archive so that it can be deleted
            CreateDataFileToLoad(@"ReleaseRequestUpdate.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalRequestMultiLine>\n	<AllRequestHeader>\n <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n	\n		<RequestHeader Terminal='TT1'>\n					<releaseByType>true</releaseByType>\n					<messageMode>U</messageMode>\n					<operatorCode>MSL</operatorCode>\n					<releaseRequestNumber>JLG40653ROAD001</releaseRequestNumber>\n					<releaseTypeStr>Road</releaseTypeStr>\n					<statusBulkRelease>Archive</statusBulkRelease>\n					<subTerminalCode>Depot</subTerminalCode>\n					<carrierCode>CARRIER1</carrierCode>\n			<AllRequestDetail>\n				<RequestDetail>\n					<quantity>5</quantity>\n					<cargoTypeDescr>ISO Container</cargoTypeDescr>\n					<isoType>22G1</isoType>\n					<releaseRequestNumber>JLG40653ROAD001</releaseRequestNumber>\n					<requestDetailID>001</requestDetailID>\n					<fullOrMT>E</fullOrMT>\n					<availabilityGrades>1</availabilityGrades>\n				</RequestDetail>\n				<RequestDetail>\n					<quantity>6</quantity>\n					<cargoTypeDescr>ISO Container</cargoTypeDescr>\n					<isoType>22G1</isoType>\n					<releaseRequestNumber>JLG40653ROAD001</releaseRequestNumber>\n					<requestDetailID>001</requestDetailID>\n					<fullOrMT>E</fullOrMT>\n					<availabilityGrades>6</availabilityGrades>\n				</RequestDetail>\n			</AllRequestDetail>\n		</RequestHeader>\n		\n		\n	</AllRequestHeader>\n</JMTInternalRequestMultiLine>");

            // Release Request Delete
            CreateDataFileToLoad(@"ReleaseRequestDelete.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalRequestMultiLine>\n	<AllRequestHeader>\n <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n	\n		<RequestHeader Terminal='TT1'>\n					<releaseByType>true</releaseByType>\n					<messageMode>D</messageMode>\n					<operatorCode>MSL</operatorCode>\n					<releaseRequestNumber>JLG40653ROAD001</releaseRequestNumber>\n					<releaseTypeStr>Road</releaseTypeStr>\n					<statusBulkRelease>Archive</statusBulkRelease>\n					<subTerminalCode>Depot</subTerminalCode>\n					<carrierCode>CARRIER1</carrierCode>\n			<AllRequestDetail>\n				<RequestDetail>\n					<quantity>5</quantity>\n					<cargoTypeDescr>ISO Container</cargoTypeDescr>\n					<isoType>22G1</isoType>\n					<releaseRequestNumber>JLG40653ROAD001</releaseRequestNumber>\n					<requestDetailID>001</requestDetailID>\n					<fullOrMT>E</fullOrMT>\n					<availabilityGrades>1</availabilityGrades>				</RequestDetail>\n				<RequestDetail>\n					<quantity>6</quantity>\n					<cargoTypeDescr>ISO Container</cargoTypeDescr>\n					<isoType>22G1</isoType>\n					<releaseRequestNumber>JLG40653ROAD001</releaseRequestNumber>\n					<requestDetailID>001</requestDetailID>\n					<fullOrMT>E</fullOrMT>\n					<availabilityGrades>6</availabilityGrades>\n				</RequestDetail>\n			</AllRequestDetail>\n		</RequestHeader>\n		\n		\n	</AllRequestHeader>\n</JMTInternalRequestMultiLine>");

            // Release Request Create
            CreateDataFileToLoad(@"ReleaseRequestCreate.xml",
               "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalRequestMultiLine>\n	<AllRequestHeader>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n \n		<RequestHeader Terminal='TT1'>\n					<releaseByType>true</releaseByType>\n					<messageMode>A</messageMode>\n					<operatorCode>MSL</operatorCode>\n					<releaseRequestNumber>JLG40653ROAD001</releaseRequestNumber>\n					<releaseTypeStr>Road</releaseTypeStr>\n					<statusBulkRelease>Active</statusBulkRelease>\n					<subTerminalCode>Depot</subTerminalCode>\n					<carrierCode>CARRIER1</carrierCode>\n			<AllRequestDetail>\n				<RequestDetail>\n					<quantity>5</quantity>\n					<cargoTypeDescr>ISO Container</cargoTypeDescr>\n					<isoType>22G1</isoType>\n					<releaseRequestNumber>JLG40653ROAD001</releaseRequestNumber>\n					<requestDetailID>001</requestDetailID>\n					<fullOrMT>E</fullOrMT>\n					<availabilityGrades>1</availabilityGrades>\n				</RequestDetail>\n				<RequestDetail>\n					<quantity>6</quantity>\n					<cargoTypeDescr>ISO Container</cargoTypeDescr>\n					<isoType>22G1</isoType>\n					<releaseRequestNumber>JLG40653ROAD001</releaseRequestNumber>\n					<requestDetailID>001</requestDetailID>\n					<fullOrMT>E</fullOrMT>\n					<availabilityGrades>6</availabilityGrades>\n				</RequestDetail>\n			</AllRequestDetail>\n		</RequestHeader>\n		\n		\n	</AllRequestHeader>\n</JMTInternalRequestMultiLine>");

            // Delete Cargo Onsite
            CreateDataFileToLoad(@"CargoOnsiteDelete.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <TestCases>40653</TestCases>\n     <messageMode>D</messageMode>\n    <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG40653A01</id>\n         <isoType>22G1</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>MSCK000002</voyageCode>\n         <dischargePort>NZAKL</dischargePort>\n         <availabilityGrade>6</availabilityGrade>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");


            // Create Cargo Onsite
            CreateDataFileToLoad(@"CargoOnsiteCreate.xml",
               "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <TestCases>40653</TestCases>\n     <messageMode>A</messageMode>\n    <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG40653A01</id>\n         <isoType>22G1</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>MSCK000002</voyageCode>\n         <dischargePort>NZAKL</dischargePort>\n         <availabilityGrade>6</availabilityGrade>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }
}

