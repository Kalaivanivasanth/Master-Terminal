using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Bulk_Gate;
using MTNForms.FormObjects.Gate_Functions.Vehicle;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using System.Drawing;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Bulk_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43971 : MTNBase
    {
        BulkGateInForm _bulkGateInForm;
        BulkGateOutForm _bulkGateOutForm;
        VehicleCancelReasonForm _vehicleCancelReasonForm;
       
        const string EDIFile1 = "M_43971_AddCargo.xml";
        const string EDIFile2 = "M_43971_AddReleaseRequest.xml";

        // setting the cargo ID and release request number to be (essentially) unique
        // unless run in same minute on different day
        // easier than resetting the release request if there is a re-run
        static  Int32 _minutes = (Int32)DateTime.Now.TimeOfDay.TotalMinutes;
        static  Int32 _day = DateTime.Now.DayOfYear;
        static string _suffix = _day.ToString() + _minutes.ToString();
        readonly string _strCargoId = @"JLG43971" + _suffix;
        readonly string _strReleaseRequestNo = @"RR43971" + _suffix;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            Console.WriteLine("Cargo / Release Request Setup\r\n---------------------------------------");
            Console.WriteLine($"_mintues: {_minutes}");
            Console.WriteLine($"_day: {_day}");
            Console.WriteLine($"_suffix: {_suffix}");
            Console.WriteLine($"_strCargoId: {_strCargoId}");
            Console.WriteLine($"_strReleaseRequestNo: {_strReleaseRequestNo}");
            
            searchFor = "M_43971_";
            
            CreateDataFile(EDIFile1,
            "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>COAL</cargoTypeDescr>\n			<product>BSC</product>\n            <id>" + _strCargoId + "</id>\n            <voyageCode>MESDIAM0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>10000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            CreateDataFile(EDIFile2,
             " <?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalReleaseRequest>\n	<AllReleaserequest>\n		<Releaserequest>\n			<releaseByType>false</releaseByType>\n			<cargoTypeDescr>COAL</cargoTypeDescr>\n			<product>BSC</product>\n			<id>" + _strCargoId +"</id>\n			<weight>10000</weight>\n			<messageMode>A</messageMode>\n			<operatorCode>MSL</operatorCode>\n			<releaseRequestNumber>" + _strReleaseRequestNo + "</releaseRequestNumber>\n			<releaseTypeStr>Road</releaseTypeStr>\n			<statusBulkRelease>Active</statusBulkRelease>\n			<subTerminalCode>Depot</subTerminalCode>\n		</Releaserequest>\n	</AllReleaserequest>\n</JMTInternalReleaseRequest>\n");

            LogInto<MTNLogInOutBO>("BULKGATE");

            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm();
            // Monday, 17 February 2025 navmh5 roadOperationsForm = FormObjectBase.Create<RoadOperationsForm>(openFormAction: FormObjectBase.MainForm.OpenRoadOperationsFromToolbar);

            /*// Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date 
            var rowFound = MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"RT^~Vehicle Id^43971A",
                searchType: SearchType.Exact, clickType: ClickType.ContextClick, rowHeight: 16, doAssert: false);
            if (rowFound)
                roadOperationsForm.ContextMenuSelect(@"Queue|Queue Selected");*/
           // var rowFound = roadOperationsForm.tblYard2.FindClickRow(new[] { "RT^~Vehicle Id^43971A" },
            var rowFound = roadOperationsForm.TblYard2.FindClickRow(new[] { "RT^~Vehicle Id^43971A" },
                 ClickType.ContextClick, SearchType.Exact, doAssert: false);
            if (string.IsNullOrEmpty(rowFound))
                roadOperationsForm.ContextMenuSelect("Queue|Queue Selected");
            roadOperationsForm.CloseForm();
        }


        [TestMethod]
        public void BulkGateVehicleQueueing()
        {
            MTNInitialize();

            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            var ediOperations = new EDIOperationsForm($"EDI Operations {terminalId}");

            /*// Wednesday, 19 March 2025 navmh5 Can be removed 6 months after specified date 
            ediOperations.DeleteEDIMessages(EDIOperationsDataType.CargoOnSite, @"43971", ediStatus: EDIOperationsStatusType.Loaded);
            ediOperations.DeleteEDIMessages(EDIOperationsDataType.ReleaseRequest, @"43971", ediStatus: EDIOperationsStatusType.Loaded);
            
            ediOperations.LoadEDIMessageFromFile(EDIFile1);
            ediOperations.ChangeEDIStatus(EDIFile1, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);
            
            ediOperations.LoadEDIMessageFromFile(EDIFile2);
            ediOperations.ChangeEDIStatus(EDIFile2, EDIOperationsStatusType.Loaded, EDIOperationsStatusType.LoadToDB);*/
            
            ediOperations.DeleteLoadChangeStatus(
                new DeleteFileArgs { FileNameToSearch = EDIFile1, EDIStatus = EDIOperationsStatusType.Loaded, EDIFileType = EDIOperationsDataType.CargoOnSite },
                new LoadFileArgs { FileToLoad = EDIFile1 },
                new []
                {
                    new ChangeStatusFromToArgs
                        { FromStatus = EDIOperationsStatusType.Loaded, ToStatus = EDIOperationsStatusType.LoadToDB }
                });
            
            ediOperations.DeleteLoadChangeStatus(
                new DeleteFileArgs { FileNameToSearch = EDIFile2, EDIStatus = EDIOperationsStatusType.Loaded, EDIFileType = EDIOperationsDataType.ReleaseRequest},
                new LoadFileArgs { FileToLoad = EDIFile2 },
                new []
                {
                    new ChangeStatusFromToArgs
                        { FromStatus = EDIOperationsStatusType.Loaded, ToStatus = EDIOperationsStatusType.LoadToDB }
                });
            
            ediOperations.CloseForm();

            //1 Go to Bulk Gate In and set truck weight
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate In", forceReset: true);
            FormObjectBase.MainForm.OpenBulkGateInFromToolbar();
            _bulkGateInForm = new BulkGateInForm(vehicleId: @"43971");
            _bulkGateInForm.cmbGate.SetValue(@"North_Gate");
            _bulkGateInForm.txtRegistration.SetValue(@"43971");
            _bulkGateInForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            _bulkGateInForm.txtNewItem.SetValue(_strReleaseRequestNo);

            _bulkGateInForm.tblScaleWeight.SetValueAndType("10", "MT");
            
            _bulkGateInForm.btnSave.DoClick();
            _bulkGateInForm.CloseForm();
           
            //2 In road operations unqueue the truck
            //FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm();
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date
            MTNControlBase.FindClickRowInTable(roadOperationsForm.TblYard2.GetElement(), $"Cargo ID^{_strCargoId}", ClickType.ContextClick, rowHeight: 16);
            //roadOperationsForm.TblYard2.FindClickRow(new[] { $"Cargo ID^{_strCargoId}" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Queue|Unqueue Selected");

            // should not be able to Queue the truck
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date roadOperationsForm.tblYard1.FindClickRow($"Cargo ID^" + _strCargoId,
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date     ClickType.ContextClick);
            MTNControlBase.FindClickRowInTable(roadOperationsForm.TblYard2.GetElement(), $"Cargo ID^{_strCargoId}", ClickType.ContextClick, rowHeight: 16);
            //roadOperationsForm.TblYard2.FindClickRow(new[] { $"Cargo ID^{_strCargoId}" }, ClickType.ContextClick);
            var foundMenuItem = roadOperationsForm.ContextMenuSelect(@"Queue|Queue Selected", validateOnly: true, doAssert: false);
            Assert.IsTrue(foundMenuItem == false, @"Context Menu Item Queue Selected should not be available");
            
            Point point = new Point(roadOperationsForm.TblYard2.GetElement().BoundingRectangle.X + 2,
                roadOperationsForm.TblYard2.GetElement().BoundingRectangle.Y + 2);
            Mouse.Click(point);

            //3. In cargo enquiry auto queue the cargo
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.COAL, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", _strCargoId);
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.GetInformationTable();
            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{_strCargoId}" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect( @"Cargo|Auto Queue " + _strCargoId);
            
            var strInfo = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblInformation, @"Info"); 
            Assert.IsTrue(strInfo.ToUpper().Contains(@"QUEUED TO MKBS07(10.000 MT)"),
                $@"Info for Cargo {_strCargoId} expected to contain QUEUED TO MKBS07(10.000 MT) but is actually {strInfo}");

            //4. Back in road ops ensure that the cargo is still not queued
            roadOperationsForm.SetFocusToForm();
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date
            MTNControlBase.FindClickRowInTable(roadOperationsForm.TblYard2.GetElement(), $@"RT^~Cargo ID^{_strCargoId}",searchType: SearchType.Exact, clickType: ClickType.None, rowHeight: 16);
            //roadOperationsForm.TblYard2.FindClickRow(new[] { $"RT^~Cargo ID^{_strCargoId}" }, ClickType.None, SearchType.Exact);

            //5. Back in cargo enquiry unqueue the cargo, and in Bulk gate out cancel the visit
            cargoEnquiryForm.SetFocusToForm();
            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, $@"ID^{_strCargoId}", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{_strCargoId}" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Unqueue " + _strCargoId);
            cargoEnquiryForm.CloseForm();

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate Out", forceReset: true);
            FormObjectBase.MainForm.OpenBulkGateOutFromToolbar();
            _bulkGateOutForm = new BulkGateOutForm();
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date
             MTNControlBase.FindClickRowInTable(_bulkGateOutForm.TblGateInList.GetElement(), @"Vehicle^43971", clickType: ClickType.Click, rowHeight: 16);
            //_bulkGateOutForm.TblGateInList.FindClickRow(new[] { "Vehicle^43971" });
            _bulkGateOutForm.btnCancel.DoClick();
            _vehicleCancelReasonForm = new VehicleCancelReasonForm($@"Vehicle Cancel Reason {terminalId}");
            _vehicleCancelReasonForm.txtReason.SetValue(@"Test");
            _vehicleCancelReasonForm.btnOK.DoClick();

            /*// Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date 
            warningErrorForm = new WarningErrorForm(formTitle: $@"Warnings for Road Operations {terminalId}");
            string[] warningErrorToCheck = new string[]
             { "Code :75005. Are you sure you want to Delete COAL Release Road 43971?"
             };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm($@"Warnings for Road Operations {terminalId}",
                new[] { "Code :75005. Are you sure you want to Delete COAL Release Road 43971?" });
            
            _bulkGateOutForm.CloseForm();

            //6. Gate in the truck again via bulk gate in
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate In", forceReset: true);
            FormObjectBase.MainForm.OpenBulkGateInFromToolbar();
            _bulkGateInForm = new BulkGateInForm(vehicleId: @"43971");
            _bulkGateInForm.cmbGate.SetValue(@"North_Gate");
            _bulkGateInForm.txtRegistration.SetValue(@"43971");
            _bulkGateInForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            _bulkGateInForm.txtNewItem.SetValue(_strReleaseRequestNo, 10);
            
            _bulkGateInForm.tblScaleWeight.SetValueAndType("10", "MT");
            _bulkGateInForm.btnSave.DoClick();
            _bulkGateInForm.CloseForm();

            //7. Run some tests in road ops - bulk gate cargo cannot be queued once unqueued, normal truck can.
            roadOperationsForm.SetFocusToForm();
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date
             MTNControlBase.FindClickRowInTable(roadOperationsForm.TblYard2.GetElement(), @"RT^Q~Vehicle Id^43971", searchType: SearchType.Exact, clickType: ClickType.ContextClick, rowHeight: 16);
            //roadOperationsForm.TblYard2.FindClickRow(new[] { "RT^Q~Vehicle Id^43971" }, ClickType.ContextClick, searchType: SearchType.Exact);
            roadOperationsForm.ContextMenuSelect(@"Queue|Unqueue Selected");
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date
             MTNControlBase.FindClickRowInTable(roadOperationsForm.TblYard2.GetElement(), @"RT^Q~Vehicle Id^43971A", searchType: SearchType.Exact, clickType: ClickType.ContextClick, rowHeight: 16);
            //roadOperationsForm.TblYard2.FindClickRow(new[] { "RT^Q~Vehicle Id^43971A" }, ClickType.ContextClick, searchType: SearchType.Exact);
            roadOperationsForm.ContextMenuSelect(@"Queue|Unqueue Selected");
            /*roadOperationsForm.tblYard2.FindClickRow(new[] { "RT^Q~Vehicle Id^43971", "RT^Q~Vehicle Id^43971A" },
                searchType: SearchType.Exact, multiSelect: true, clickType: ClickType.ContextClick);*/
            //roadOperationsForm.ContextMenuSelect(@"Queue|Unqueue Selected");

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date
             MTNControlBase.FindClickRowInTable(roadOperationsForm.TblYard2.GetElement(), @"RT^~Vehicle Id^43971", searchType: SearchType.Exact, clickType: ClickType.ContextClick, rowHeight: 16);
            //roadOperationsForm.TblYard2.FindClickRow(new[] { "RT^~Vehicle Id^43971" }, ClickType.ContextClick, searchType: SearchType.Exact);
            foundMenuItem = roadOperationsForm.ContextMenuSelect(@"Queue|Queue Selected", validateOnly: true, doAssert: false);
            Assert.IsTrue(foundMenuItem == false, @"Context Menu Item Queue Selected should not be available");
            
            point = new Point(roadOperationsForm.TblYard2.GetElement().BoundingRectangle.X + 2,
                roadOperationsForm.TblYard2.GetElement().BoundingRectangle.Y + 2);
            Mouse.Click(point);

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date
             MTNControlBase.FindClickRowInTable(roadOperationsForm.TblYard2.GetElement(), @"RT^~Vehicle Id^43971A", searchType: SearchType.Exact, clickType: ClickType.ContextClick, rowHeight: 16);
            //roadOperationsForm.TblYard2.FindClickRow(new[] { "RT^~Vehicle Id^43971A" }, ClickType.ContextClick, searchType: SearchType.Exact);
            roadOperationsForm.ContextMenuSelect(@"Queue|Queue Selected");

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date 
            //MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"RT^~Vehicle Id^43971", searchType: SearchType.Exact, clickType: ClickType.None, rowHeight: 16); 
            MTNControlBase.FindClickRowInTable(roadOperationsForm.TblYard2.GetElement(), @"RT^Q~Vehicle Id^43971A", searchType: SearchType.Exact, clickType: ClickType.None, rowHeight: 16);
            //roadOperationsForm.TblYard2.FindClickRow(new[] { "RT^~Vehicle Id^43971", "RT^Q~Vehicle Id^43971A" }, ClickType.None, searchType: SearchType.Exact);

            //8. clean up in bulk gate out
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate Out", forceReset: true);
            FormObjectBase.MainForm.OpenBulkGateOutFromToolbar();
            _bulkGateOutForm = new BulkGateOutForm();
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date
             MTNControlBase.FindClickRowInTable(_bulkGateOutForm.TblGateInList.GetElement(), @"Vehicle^43971", clickType: ClickType.Click, rowHeight: 16);
            //_bulkGateOutForm.TblGateInList.FindClickRow(new[] { "Vehicle^43971" });
            _bulkGateOutForm.btnCancel.DoClick();
            _vehicleCancelReasonForm = new VehicleCancelReasonForm($@"Vehicle Cancel Reason {terminalId}");
            _vehicleCancelReasonForm.txtReason.SetValue(@"Test");
            _vehicleCancelReasonForm.btnOK.DoClick();

            /*// Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date 
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Road Operations TT1");
            var warningErrorToCheck2 = new string[]
             { "Code :75005. Are you sure you want to Delete COAL Release Road 43971?" };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck2);
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm($"Warnings for Road Operations {terminalId}",
                new[] { "Code :75005. Are you sure you want to Delete COAL Release Road 43971?" });

            _bulkGateOutForm.CloseForm();

        }





    }

}
