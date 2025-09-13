using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNForms.FormObjects.Yard_Functions.Work_Order;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Work_Order_Enquiry
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase58858 : MTNBase
    {
        WorkOrderEnquiryForm _workOrderEnquiryForm;
        NewWorkOrderForm _newWorkOrderForm;
        WorkOrderTasksForm _workOrderTasksForm;
        ConfirmationFormYesNo _confirmationFormYesNo;
        WorkOrderOperationsForm _workOrderOperationsForm;
        RoadGateDetailsReleaseForm _releaseEmptyContainerForm;

        private const string TestCaseNumber = @"58858";
        const string TerminalId = @"PEL";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            userName = "WOUSER";
            LogInto<MTNLogInOutBO>(userName);
            CallJadeScriptToRun(TestContext, @"reset_WorkOrders_ReleaseWO");
            SetupAndLoadInitializeData(TestContext);
        }

        /// <summary>
        /// To test the Road Release Cargo Work Order jobs using Work Order Enquiry form
        /// </summary>
        [TestMethod]
        public void ReleaseRoadJobs()
        {
            MTNInitialize();

            // Open Work Order Enquiry
            FormObjectBase.MainForm.OpenWorkOrderEnquiryFromToolbar();
            _workOrderEnquiryForm = new WorkOrderEnquiryForm($"Work Order Enquiry {TerminalId}");

            // Click on New Cargo Work Order
            _workOrderEnquiryForm.DoNewCargoWorkOrder();

            _newWorkOrderForm = new NewWorkOrderForm($"Cargo Work Order New Screen {TerminalId}");
            var date = DateTime.Now;

            _newWorkOrderForm.txtReference.SetValue(@"RELEASE");
            _newWorkOrderForm.txtRequiredByDate.SetValue(date.ToString(@"ddMMyyyy"));
            _newWorkOrderForm.txtRequiredByTime.SetValue(date.ToString(@"HHmm"));
            _newWorkOrderForm.btnAddTask.DoClick();

            // Create Receive Cargo Work Order
            _workOrderTasksForm = new WorkOrderTasksForm($"Work Order Details {TerminalId}");
            MTNControlBase.FindClickRowInList(_workOrderTasksForm.lstTasks, @"Release Cargo");

            _workOrderTasksForm.btnOK.DoClick();
            _newWorkOrderForm.SetFocusToForm();
            _newWorkOrderForm.GetTaskDetailsTab(@"Release");

            _newWorkOrderForm.cmbReleaseRequest.SetValue($"{TestCaseNumber}RLS");

            _newWorkOrderForm.GetTaskDetailsTab(@"Selected Cargo Items [0]");
            _newWorkOrderForm.btnAttachCargoFromBookingRelease.DoClick();

            _newWorkOrderForm.btnSave.DoClick();

            _workOrderEnquiryForm.txtCustomerReference.SetValue(@"RELEASE");
            _workOrderEnquiryForm.lstStatus.BtnAllLeft.DoClick();
            
            _workOrderEnquiryForm.lstStatus.MoveItemsBetweenList(_workOrderEnquiryForm.lstStatus.LstLeft, new[] { @"User Entry" });
            _workOrderEnquiryForm.DoSearch();

            // Approve and Action the task
            /*// Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                $"Status^User Entry~Work Order Description^Release Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                ClickType.ContextClick);*/
            _workOrderEnquiryForm.TblData.FindClickRow(new[]
            {
                $"Status^User Entry~Work Order Description^Release Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"
            }, ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Set Approved");

            var workOrderId = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
                $"Status^Approved~Work Order Description^Release Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}", @"Work Order #", ClickType.None);

            /*// Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                $"Status^Approved~Work Order Description^Release Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                ClickType.ContextClick);*/
            _workOrderEnquiryForm.TblData.FindClickRow(new[]
            {
                $"Status^Approved~Work Order Description^Release Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"
            }, ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Action Task|Release Cargo - Road Task #1");

            var pinNumber = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
               $"Status^Approved~Work Order Description^Release Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}", @"Pin", ClickType.None);

            // Open Road Gate and save
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();
            roadGateForm = new RoadGateForm(formTitle: $"Road Gate {TerminalId}", vehicleId: TestCaseNumber);
            roadGateForm.SetRegoCarrierGate(TestCaseNumber);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            roadGateForm.txtNewItem.SetValue(pinNumber);
            Keyboard.Press(VirtualKeyShort.TAB);

            _releaseEmptyContainerForm = new RoadGateDetailsReleaseForm($"Release Empty Container  {TerminalId}");

            // Click Save button Release Container form
            _releaseEmptyContainerForm.BtnSave.DoClick();

            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out",
                new[]
                {
                    "Code :79688. Cargo JLG58858A01's availability status of Unavailable for Release does not match the request's availability status of Available for release"
                });

            roadGateForm.SetFocusToForm();
            //MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, $"Type^Release Empty~Detail^JLG{TestCaseNumber}A01; MSL; 2200~Booking/Release^{TestCaseNumber}RLS");
            roadGateForm.TblGateItems.FindClickRow([$"Type^Release Empty~Detail^JLG{TestCaseNumber}A01; MSL; 2200~Booking/Release^{TestCaseNumber}RLS"]);

            // Click Save on Road Gate form
            roadGateForm.btnSave.DoClick();

            _workOrderEnquiryForm.SetFocusToForm();
            _workOrderEnquiryForm.DoWorkOrderOperations();

            //Allocate Jobs to gangs
            _workOrderOperationsForm = new WorkOrderOperationsForm($"Work Order Operations {TerminalId}");
            _workOrderOperationsForm.cmbOperationMode.SetValue(@"Road");
            _workOrderOperationsForm.chkJobSheet.DoClick(false);
            _workOrderOperationsForm.chkFilterCriteria.DoClick();
            _workOrderOperationsForm.GetFilterCriteria();
            _workOrderOperationsForm.cmbShiftFrom.SetValue(@" ");
            _workOrderOperationsForm.cmbShiftTo.SetValue(@" ");
            _workOrderOperationsForm.GetTabTableGeneric(@"Unallocated");

            //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric,
            //    @"Job Type^Release Cargo - Road~Vehicle Id^" + TestCaseNumber + "~Work Order^" + workOrderId + "",
            //    ClickType.ContextClick, rowHeight: 16);
            _workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
                [$"Job Type^Release Cargo - Road~Vehicle Id^{TestCaseNumber}~Work Order^{workOrderId}"], ClickType.ContextClick);
            _workOrderOperationsForm.ContextMenuSelect(@"Allocate to job sheet...|ROADGANG");
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Allocate to job sheet...");
            _confirmationFormYesNo.btnYes.DoClick();

            _workOrderOperationsForm.GetTabTableGeneric(@"Allocated");
            //MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.tblGeneric, @"Work Order^" + workOrderId + "~Vehicle Id^" + TestCaseNumber + "~Job Type^Release Cargo - Road", ClickType.Click, rowHeight: 16);
            _workOrderOperationsForm.TabGeneric.TableWithHeader.FindClickRow(
                [$"Work Order^{workOrderId}~Vehicle Id^{TestCaseNumber}~Job Type^Release Cargo - Road"], ClickType.Click);

            _workOrderEnquiryForm.SetFocusToForm();

            /*// Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                $"Status^Approved~Work Order Description^Release Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                ClickType.Click);*/
            _workOrderEnquiryForm.TblData.FindClickRow(new[]
            {
                $"Status^Approved~Work Order Description^Release Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"
            });

            // Open Yard Functions | Road Operations
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit(RoadOperationsForm.FormTitle, new[] { $"{TestCaseNumber}" });

            _workOrderEnquiryForm.SetFocusToForm();

            /*// Tuesday, 4 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                $"Status^Complete~Work Order Description^Release Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                ClickType.ContextClick);*/
            _workOrderEnquiryForm.TblData.FindClickRow(new[]
            {
                $"Status^Complete~Work Order Description^Release Cargo - Road Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"
            }, ClickType.ContextClick);

            _workOrderEnquiryForm.ContextMenuSelect(@"Set Archived");
        }

        #region SetUp and load data
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_58858_";
            
            //Delete Cargo Onsite
            CreateDataFileToLoad(@"DeleteCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n<AllCargoOnSite>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n<CargoOnSite Terminal='PEL'>\n<TestCases>58858</TestCases>\n<cargoTypeDescr>ISO Container</cargoTypeDescr>\n<id>JLG58858A01</id>\n<isoType>2200</isoType>\n<operatorCode>MSL</operatorCode>\n<locationId>BS02</locationId>\n<weight>5000.0000</weight>\n<imexStatus>Import</imexStatus>\n<commodity>MT</commodity>\n<dischargePort>NZBLU</dischargePort>\n<voyageCode>VOYWORKORDERS</voyageCode>\n<messageMode>D</messageMode>\n</CargoOnSite>\n</AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //Create Cargo Onsite
            CreateDataFileToLoad(@"CreateCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n<AllCargoOnSite>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n<CargoOnSite Terminal='PEL'>\n<TestCases>58858</TestCases>\n<cargoTypeDescr>ISO Container</cargoTypeDescr>\n<id>JLG58858A01</id>\n<isoType>2200</isoType>\n<operatorCode>MSL</operatorCode>\n<locationId>BS02</locationId>\n<weight>5000.0000</weight>\n<imexStatus>Import</imexStatus>\n<commodity>MT</commodity>\n<dischargePort>NZBLU</dischargePort>\n<voyageCode>VOYWORKORDERS</voyageCode>\n<messageMode>A</messageMode>\n</CargoOnSite>\n</AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Delete Release Request
            /*
            CreateDataFileToLoad(@"DeleteReleaseRequest.xml",
                "<?xml version='1.0' encoding='UTF-8'?>   \n   <JMTInternalRequestMultiLine>   \n   <AllRequestHeader>   \n   <operationsToPerform>Verify;Load To DB</operationsToPerform>   \n   <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED;EDI_STATUS_VERIFIED_ERRORS</operationsToPerformStatuses>   \n   <RequestHeader Terminal='PEL'>   \n   <releaseByType>false</releaseByType>   \n   <messageMode>D</messageMode>   \n   <operatorCode>MSL</operatorCode>   \n   <voyageCode>VOYWORKORDERS</voyageCode>   \n   <releaseRequestNumber>58858RLS</releaseRequestNumber>   \n   <releaseTypeStr>Road</releaseTypeStr>   \n   <statusBulkRelease>Complete</statusBulkRelease>   \n   <subTerminalCode>Depot</subTerminalCode>   \n   <carrierCode>CARRIER1</carrierCode>   \n   <AllRequestDetail>   \n   <RequestDetail>   \n   <quantity>1</quantity>   \n   <cargoTypeDescr>ISO Container</cargoTypeDescr>   \n   <isoType>2200</isoType>   \n   <consignee>ABCNE</consignee>   \n   <releaseRequestNumber>58858RLS</releaseRequestNumber>   \n   <id>JLG58858A01</id>   \n   <requestDetailID>001</requestDetailID>   \n   </RequestDetail>   \n   </AllRequestDetail>   \n   </RequestHeader>   \n   </AllRequestHeader>   \n   </JMTInternalRequestMultiLine>   \n      \n   ");
                */

            // Create Release Request
            CreateDataFileToLoad(@"CreateReleaseRequest.xml",
                "<?xml version='1.0' encoding='UTF-8'?>   \n   <JMTInternalRequestMultiLine>   \n   <AllRequestHeader>   \n   <operationsToPerform>Verify;Load To DB</operationsToPerform>   \n   <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>   \n   <RequestHeader Terminal='PEL'>   \n   <releaseByType>false</releaseByType>   \n   <messageMode>A</messageMode>   \n   <operatorCode>MSL</operatorCode>   \n   <voyageCode>VOYWORKORDERS</voyageCode>   \n   <releaseRequestNumber>58858RLS</releaseRequestNumber>   \n   <releaseTypeStr>Road</releaseTypeStr>   \n   <statusBulkRelease>Active</statusBulkRelease>   \n   <subTerminalCode>Depot</subTerminalCode>   \n   <carrierCode>CARRIER1</carrierCode>   \n   <AllRequestDetail>   \n   <RequestDetail>   \n   <quantity>1</quantity>   \n   <cargoTypeDescr>ISO Container</cargoTypeDescr>   \n   <isoType>2200</isoType>   \n   <consignee>ABCNE</consignee>   \n   <releaseRequestNumber>58858RLS</releaseRequestNumber>   \n   <id>JLG58858A01</id>   \n   <requestDetailID>001</requestDetailID>   \n   </RequestDetail>   \n   </AllRequestDetail>   \n   </RequestHeader>   \n   </AllRequestHeader>   \n   </JMTInternalRequestMultiLine>   \n      \n   ");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
        #endregion SetUp and load data
    }
}





