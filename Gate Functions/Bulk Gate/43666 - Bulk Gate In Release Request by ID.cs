using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Bulk_Gate;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
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
    public class TestCase43666 : MTNBase
    {
        BulkGateInForm _bulkGateInForm;
        BulkGateOutForm _bulkGateOutForm;
        ReleaseRequestForm _releaseRequestForm;
        ReleaseRequestAddForm _releaseRequestEdit;

        const string EDIFile1 = "M_43666_AddCargo.xml";
        const string EDIFile2 = "M_43666_AddReleaseRequest.xml";

        // setting the cargo ID and release request number to be (essentially) unique
        // unless run in same minute on different day
        // easier than resetting the release request if there is a re-run
        static readonly Int32 Minutes = (Int32)DateTime.Now.TimeOfDay.TotalMinutes;
        static readonly Int32 Day = (Int32)DateTime.Now.DayOfYear;
        static readonly string Suffix = Day.ToString() + Minutes.ToString();
        static readonly string StrCargoID = @"MT43666" + Suffix;
        static readonly string StrReleaseRequestNo = @"RR43666" + Suffix;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
         void MTNInitialize()
        {
            CreateDataFile(EDIFile1,
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>COAL</cargoTypeDescr>\n			<product>BSC</product>\n            <id>" +
                StrCargoID +
                "</id>\n            <voyageCode>MESDIAM0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>10000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            CreateDataFile(EDIFile2,
                " <?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalReleaseRequest>\n	<AllReleaserequest>\n		<Releaserequest>\n			<releaseByType>false</releaseByType>\n			<cargoTypeDescr>COAL</cargoTypeDescr>\n			<product>BSC</product>\n			<id>" +
                StrCargoID +
                "</id>\n			<weight>10000</weight>\n			<messageMode>A</messageMode>\n			<operatorCode>MSL</operatorCode>\n			<releaseRequestNumber>" +
                StrReleaseRequestNo +
                "</releaseRequestNumber>\n			<releaseTypeStr>Road</releaseTypeStr>\n			<statusBulkRelease>Active</statusBulkRelease>\n			<subTerminalCode>Depot</subTerminalCode>\n		</Releaserequest>\n	</AllReleaserequest>\n</JMTInternalReleaseRequest>\n");

            LogInto<MTNLogInOutBO>("BULKGATE");

            // 21/01/2025FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            var ediOperations = new EDIOperationsForm(@"EDI Operations TT1")
         ;

            ediOperations.DeleteEDIMessages(EDIOperationsDataType.CargoOnSite, @"43666", ediStatus:  EDIOperationsStatusType.Loaded);
            ediOperations.DeleteEDIMessages(EDIOperationsDataType.ReleaseRequest, @"43666", ediStatus:  EDIOperationsStatusType.Loaded);

            ediOperations.LoadEDIMessageFromFile(EDIFile1);
            ediOperations.ChangeEDIStatus(EDIFile1,  EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);

            ediOperations.LoadEDIMessageFromFile(EDIFile2);
            ediOperations.ChangeEDIStatus(EDIFile2,  EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);

            ediOperations.CloseForm();
        }


        [TestMethod]
        public void BulkGateInReleaseByID()
        {
            MTNInitialize();

            // 21/01/2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate In", forceReset: true);
            FormObjectBase.MainForm.OpenBulkGateInFromToolbar();
            _bulkGateInForm = new BulkGateInForm(vehicleId: @"43666");
            _bulkGateInForm.cmbGate.SetValue(@"North_Gate");
            _bulkGateInForm.txtRegistration.SetValue(@"43666");
            _bulkGateInForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt, doDownArrow: true);
            _bulkGateInForm.txtNewItem.SetValue(StrReleaseRequestNo, 10);
            _bulkGateInForm.tblScaleWeight.SetValueAndType(@"3000", @"kg");
            _bulkGateInForm.btnSave.DoClick();
            _bulkGateInForm.CloseForm();

            // 21/01/2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate Out", forceReset: true);
            FormObjectBase.MainForm.OpenBulkGateOutFromToolbar();
            _bulkGateOutForm = new BulkGateOutForm();
            // Friday, 31 January 2025 navmh5  MTNControlBase.FindClickRowInTable(_bulkGateOutForm.tblGateInList, @"Vehicle^43666", clickType: ClickType.Click, rowHeight: 16);
            _bulkGateOutForm.TblGateInList.FindClickRow(new[] { "Vehicle^43666" });
            _bulkGateOutForm.SetWeightValues(@"12000", @"kg");
            _bulkGateOutForm.chkReceiptRequired.DoClick(false);
            _bulkGateOutForm.btnSave.DoClick();
            _bulkGateOutForm.CloseForm();

            // 21/01/2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Release Requests", forceReset: true);
            FormObjectBase.MainForm.OpenReleaseRequestFromToolbar();
            _releaseRequestForm = new ReleaseRequestForm();

            _releaseRequestForm.cmbView.SetValue(@"Active");
            _releaseRequestForm.cmbType.SetValue(@"Road");
            MTNControlBase.SetValueInEditTable(_releaseRequestForm.tblSearchTable, @"Release No", StrReleaseRequestNo);
            _releaseRequestForm.DoSearch();
            // Friday, 31 January 2025 navmh5 MTNControlBase.FindClickRowInTable(_releaseRequestForm.tblReleaseRequests, @"Release No^" + StrReleaseRequestNo, clickType: ClickType.DoubleClick);
            _releaseRequestForm.TblReleaseRequests.FindClickRow(new[] { $"Release No^{StrReleaseRequestNo}" }, clickType: ClickType.DoubleClick);

            _releaseRequestEdit = new ReleaseRequestAddForm(@"Editing request " + StrReleaseRequestNo + "... TT1");
            MTNControlBase.FindClickRowInList(_releaseRequestEdit.tblReleaseRequestList, StrReleaseRequestNo);
            MTNControlBase.FindClickRowInList(_releaseRequestEdit.tblReleaseRequestList, @"COAL, Bituminous Steam Coal");
            MTNControlBase.FindClickRowInList(_releaseRequestEdit.tblReleaseRequestList, @"1.000 MT Items assigned");
            MTNControlBase.FindClickRowInList(_releaseRequestEdit.tblReleaseRequestList, @"1.000 MT x " + StrCargoID + " MKBS01");
            MTNControlBase.FindClickRowInList(_releaseRequestEdit.tblReleaseRequestList, @"9.000 MT Released items");
            MTNControlBase.FindClickRowInList(_releaseRequestEdit.tblReleaseRequestList, @"9.000 MT x " + StrCargoID);
            
            //_releaseRequestEdit.btnCancel.DoClick();
            _releaseRequestEdit.DoCancel();

        }

    }

}