using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Bulk_Gate;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Bulk_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40220 : MTNBase
    {
        BulkGateInForm _bulkGateInForm;
        BulkGateOutForm _bulkGateOutForm;
        ReleaseRequestForm _releaseRequestForm;
        ReleaseRequestAddForm _releaseRequestEdit;
        AuditEnquiryForm _auditEnquiryForm;

        const string EDIFile1 = "M_40220_AddCargo.xml";
        const string EDIFile2 = "M_40220_AddReleaseRequest.xml";

        // setting the cargo ID and release request number to be (essentially) unique
        // unless run in same minute on different day
        // easier than resetting the release request if there is a re-run
        private static readonly Int32 Minutes = (Int32)DateTime.Now.TimeOfDay.TotalMinutes;
        private static readonly Int32 Day = (Int32)DateTime.Now.DayOfYear;
        private static readonly string Suffix = Day.ToString() + Minutes.ToString();
        private static readonly string StrCargoID = @"MT40220" + Suffix;
        private static readonly string StrReleaseRequestNo = @"RR40220" + Suffix;
        const string VehicleId = @"40220";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CreateDataFile(EDIFile1,
            "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>COAL</cargoTypeDescr>\n			<product>BSC</product>\n            <id>" + StrCargoID + "</id>\n            <voyageCode>MESDIAM0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>2000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            CreateDataFile(EDIFile2,
             " <?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalReleaseRequest>\n	<AllReleaserequest>\n		<Releaserequest>\n			<releaseByType>false</releaseByType>\n			<cargoTypeDescr>COAL</cargoTypeDescr>\n			<product>BSC</product>\n			<id>" + StrCargoID + "</id>\n			<weight>2000</weight>\n			<messageMode>A</messageMode>\n			<operatorCode>MSL</operatorCode>\n			<releaseRequestNumber>" + StrReleaseRequestNo + "</releaseRequestNumber>\n			<releaseTypeStr>Road</releaseTypeStr>\n			<statusBulkRelease>Active</statusBulkRelease>\n			<subTerminalCode>Depot</subTerminalCode>\n		</Releaserequest>\n	</AllReleaserequest>\n</JMTInternalReleaseRequest>\n");

            LogInto<MTNLogInOutBO>("BULKGATE");
            
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            var  ediOperations = new EDIOperationsForm(@"EDI Operations TT1");
            
            ediOperations.DeleteEDIMessages(EDIOperationsDataType.CargoOnSite, @"40220", ediStatus: EDIOperationsStatusType.Loaded);
            ediOperations.DeleteEDIMessages(EDIOperationsDataType.ReleaseRequest, @"40220", ediStatus: EDIOperationsStatusType.Loaded);
            
            ediOperations.LoadEDIMessageFromFile(EDIFile1);
            ediOperations.ChangeEDIStatus(EDIFile1, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);
            
            ediOperations.LoadEDIMessageFromFile(EDIFile2);
            ediOperations.ChangeEDIStatus(EDIFile2, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);
            ediOperations.CloseForm();
        }


        [TestMethod]
        public void BulkGateInReleaseWithAudits()
        {
            MTNInitialize();
            
            //1 bulk gate in truck = 1000kg
            // 21/01/2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate In", forceReset: true);
            FormObjectBase.MainForm.OpenBulkGateInFromToolbar();
            _bulkGateInForm = new BulkGateInForm(vehicleId: VehicleId);
            _bulkGateInForm.cmbGate.SetValue(@"North_Gate");
            _bulkGateInForm.txtRegistration.SetValue(@"40220");
            _bulkGateInForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            _bulkGateInForm.txtNewItem.SetValue(StrReleaseRequestNo, 10);
            _bulkGateInForm.tblScaleWeight.SetValueAndType(@"1000", @"kg");

            _bulkGateInForm.btnSave.DoClick();
            _bulkGateInForm.CloseForm();

            //2 bulk gate out truck+cargo = 1100kg
            // 21/01/2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate Out", forceReset: true);
            FormObjectBase.MainForm.OpenBulkGateOutFromToolbar();
            _bulkGateOutForm = new BulkGateOutForm();
            // MTNControlBase.FindClickRowInTable(_bulkGateOutForm.tblGateInList, @"Vehicle^40220", clickType: ClickType.Click, rowHeight: 16);
            _bulkGateOutForm.TblGateInList.FindClickRow(["Vehicle^40220"], clickType: ClickType.Click);            _bulkGateOutForm.SetWeightValues(@"1100", @"kg");
            _bulkGateOutForm.chkReceiptRequired.DoClick(false);
            _bulkGateOutForm.btnSave.DoClick();
            _bulkGateOutForm.CloseForm();

            //3 bulk gate in truck = 1000kg
            // 21/01/2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate In", forceReset: true);
            FormObjectBase.MainForm.OpenBulkGateInFromToolbar();
            _bulkGateInForm = new BulkGateInForm(vehicleId: VehicleId);
            _bulkGateInForm.cmbGate.SetValue(@"North_Gate");
            _bulkGateInForm.txtRegistration.SetValue(@"40220");
            _bulkGateInForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            _bulkGateInForm.txtNewItem.SetValue(StrReleaseRequestNo, 10);
            _bulkGateInForm.tblScaleWeight.SetValueAndType(@"1000", @"kg");

            _bulkGateInForm.btnSave.DoClick();
            _bulkGateInForm.CloseForm();

            //4 bulk gate out truck+cargo = 1100kg
            // 21/01/2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate Out", forceReset: true);
            FormObjectBase.MainForm.OpenBulkGateOutFromToolbar();
            _bulkGateOutForm = new BulkGateOutForm();
            // MTNControlBase.FindClickRowInTable(_bulkGateOutForm.tblGateInList, @"Vehicle^40220", clickType: ClickType.Click, rowHeight: 16);
            _bulkGateOutForm.TblGateInList.FindClickRow(["Vehicle^40220"], clickType: ClickType.Click);            _bulkGateOutForm.SetWeightValues(@"1100", @"kg");
            _bulkGateOutForm.chkReceiptRequired.DoClick(false);
            _bulkGateOutForm.btnSave.DoClick();
            _bulkGateOutForm.CloseForm();

            //5 go to release requests and delete request to complete it
            // 21/01/2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Release Requests", forceReset: true);
            FormObjectBase.MainForm.OpenReleaseRequestFromToolbar();
            _releaseRequestForm = new ReleaseRequestForm();
            _releaseRequestForm.cmbType.SetValue(@"Road");
            MTNControlBase.SetValueInEditTable(_releaseRequestForm.tblSearchTable, @"Release No", StrReleaseRequestNo);
            _releaseRequestForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_releaseRequestForm.tblReleaseRequests, @"Release No^" + StrReleaseRequestNo, clickType: ClickType.DoubleClick);
            _releaseRequestForm.TblReleaseRequests.FindClickRow(["Release No^" + StrReleaseRequestNo], clickType: ClickType.DoubleClick);            _releaseRequestEdit = new ReleaseRequestAddForm(@"Editing request " + StrReleaseRequestNo + "... TT1");
            _releaseRequestEdit.ClickOnReleaseRequestItem(@"1.800 MT x " + StrCargoID + " MKBS01");
            _releaseRequestEdit.DoDelete();
            _releaseRequestEdit.DoSave();

            //6 Examine Audits on release request to ensure it shows 200 released (i.e. 2x100)
            _releaseRequestForm.SetFocusToForm();
            _releaseRequestForm.cmbView.SetValue(@"Complete");
            _releaseRequestForm.cmbType.SetValue(@"All");
            MTNControlBase.SetValueInEditTable(_releaseRequestForm.tblSearchTable, @"Release No", StrReleaseRequestNo);
            _releaseRequestForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_releaseRequestForm.tblReleaseRequests, @"Release No^" + StrReleaseRequestNo, clickType: ClickType.ContextClick);
            _releaseRequestForm.TblReleaseRequests.FindClickRow(["Release No^" + StrReleaseRequestNo], clickType: ClickType.ContextClick);            _releaseRequestForm.ContextMenuSelect(@"View Audits for Release - " + StrReleaseRequestNo);

            _auditEnquiryForm = new AuditEnquiryForm();
            // MTNControlBase.FindClickRowInTable(_auditEnquiryForm.tblAuditItems, @"Description^Release - " + StrReleaseRequestNo + "~Audit Type^Created", rowHeight: 16);
            _auditEnquiryForm.TblAuditItems.FindClickRow(["Description^Release - " + StrReleaseRequestNo + "~Audit Type^Created"]);            string tableValue = MTNControlBase.FindClickRowInTableVHeader(_auditEnquiryForm.tblAuditDetails, @"weightRequested", clickType: ClickType.None);
            Assert.IsTrue(tableValue == "2000.0000",@"Expect value: 2000.0000, Actual value: " + tableValue);
            // MTNControlBase.FindClickRowInTable(_auditEnquiryForm.tblAuditItems, @"Description^Release - " + StrReleaseRequestNo + "~Audit Type^Updated", rowHeight: 16);
 _auditEnquiryForm.TblAuditItems.FindClickRow(["Description^Release - " + StrReleaseRequestNo + "~Audit Type^Updated"]);            // column 0 (before)
            tableValue = MTNControlBase.FindClickRowInTableVHeader(_auditEnquiryForm.tblAuditDetails, @"weightRequested", clickType: ClickType.None);
            Assert.IsTrue(tableValue == "2000.0000", @"Expect value: 2000.0000, Actual value: " + tableValue);
            // column 1 (after)
            tableValue = MTNControlBase.FindClickRowInTableVHeader(_auditEnquiryForm.tblAuditDetails, @"weightRequested", clickType: ClickType.None, returnColumn: 1);
            Assert.IsTrue(tableValue == "200.0000", @"Expect value: 200.0000, Actual value: " + tableValue);

        }





    }

}
