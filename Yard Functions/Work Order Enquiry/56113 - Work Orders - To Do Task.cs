using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Yard_Functions.Work_Order;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects.Message_Dialogs;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Work_Order_Enquiry
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase56113 : MTNBase
    {
        WorkOrderEnquiryForm _workOrderEnquiryForm;
        WorkOrderOperationsForm _workOrderOperationsForm;
        NewWorkOrderForm _newWorkOrderForm;
        ConfirmationFormYesNo confirmationFormYesNo;
        CompleteJobForm _completeJobForm;

        private const string TestCaseNumber = @"56113";
        private const string TerminalId = @"PEL";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            userName = "WOUSER";
            BaseClassInitialize_New(testContext);
        }

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        private void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            CallJadeScriptToRun(TestContext, @"reset_WorkOrders_ToDoWO");
            LogInto<MTNLogInOutBO>(userName);
        }

        /// <summary>
        /// To test the Work Order To Do Task functionality using Work Order Enquiry form
        /// </summary>
        [TestMethod]
        public void WorkOrdersToDoTask()
        {
            MTNInitialize();

            // Open Work Order Enquiry form
            FormObjectBase.MainForm.OpenWorkOrderEnquiryFromToolbar();
            _workOrderEnquiryForm = new WorkOrderEnquiryForm($"Work Order Enquiry {TerminalId}");

            // Click on New Cargo Work Order
            _workOrderEnquiryForm.DoNewCargoWorkOrder();

            _newWorkOrderForm = new NewWorkOrderForm($"Cargo Work Order New Screen {TerminalId}");
            var date = DateTime.Now;

            _newWorkOrderForm.txtReference.SetValue(@"TODOTASK");
            _newWorkOrderForm.txtRequiredByDate.SetValue(date.ToString(@"ddMMyyyy"));
            _newWorkOrderForm.txtRequiredByTime.SetValue(date.ToString(@"HHmm"));

            // Add multiple cargo work order todo tasks for Req Paint and ToDo_56113
            //_newWorkOrderForm.AddCargoToDoTask(@"JLG56113A01", @"Req Paint");
            _newWorkOrderForm.AddCargoToDoTask($"JLG{TestCaseNumber}A01", @"ToDo_56113");

            _newWorkOrderForm.btnSave.DoClick();

            _workOrderEnquiryForm.txtCustomerReference.SetValue(@"TODOTASK");
            _workOrderEnquiryForm.lstStatus.BtnAllLeft.DoClick();

            string[] detailsToMove =
            {
                 @"User Entry"
              };
            _workOrderEnquiryForm.lstStatus.MoveItemsBetweenList(_workOrderEnquiryForm.lstStatus.LstLeft, detailsToMove);
            _workOrderEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                // $"Status^User Entry~Work Order Description^To Do Task~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow([$"Status^User Entry~Work Order Description^To Do Task~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Set Approved");

            var workOrderId = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
                $"Status^Approved~Work Order Description^To Do Task~Voyage^{PEL.Voyage.VOYWORKORDERS}", @"Work Order #");
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                // $"Status^Approved~Work Order Description^To Do Task~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow([$"Status^Approved~Work Order Description^To Do Task~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Action Task|To Do Task");

            // Open Cargo Enquiry form
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", $"JLG{TestCaseNumber}A");
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.GetInformationTable();

            // Check if the to do task ToDo_56113 is not applied to the cargo item JLG56113A02
            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^JLG{TestCaseNumber}A01" });
            var infoDetails1 =
               MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblInformation, @"Info").Replace(
                   "\r", String.Empty).Replace("\n", String.Empty);
            Assert.IsTrue(infoDetails1.Equals(@"Stops Present. "),
                @"TestCase56113 - Info does not match.  Expected: Stops Present. " +
                @"Actual: " + infoDetails1);

            // Go to Work Order Operations form
            _workOrderEnquiryForm.SetFocusToForm();
            _workOrderEnquiryForm.DoWorkOrderOperations();

            //Allocate Jobs to gangs for the todo task Req Paint and ToDo_56113
            _workOrderOperationsForm = new WorkOrderOperationsForm($"Work Order Operations {TerminalId}");
            _workOrderOperationsForm.cmbOperationMode.SetValue(@"Todo Task");
            _workOrderOperationsForm.chkJobSheet.DoClick(false);
            _workOrderOperationsForm.chkFilterCriteria.DoClick();
            _workOrderOperationsForm.GetFilterCriteria();
            _workOrderOperationsForm.cmbShiftFrom.SetValue(@" ");
            _workOrderOperationsForm.cmbShiftTo.SetValue(@" ");
            _workOrderOperationsForm.btnSearch.DoClick();
            _workOrderOperationsForm.GetTabTableGeneric(@"Unallocated");
            // it's failing to find the row in the table because there are 2 columns which start with the column heading To Do Task
            MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.TabGeneric.TableWithHeader.GetElement(),
                @"Job Type^ToDo Cargo~Work Order^" + workOrderId + "~To Do Task^ToDo_56113", ClickType.ContextClick,
                rowHeight: 16);
            //_workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
             //   [$"Job Type^ToDo Cargo~Work Order^{workOrderId}~To Do Task^ToDo_56113"],
              //  ClickType.ContextClick);
            _workOrderOperationsForm.ContextMenuSelect(@"Allocate to job sheet...|TODOGANG");
            confirmationFormYesNo = new ConfirmationFormYesNo(@"Allocate to job sheet...");
            confirmationFormYesNo.btnYes.DoClick();
            
            _workOrderOperationsForm.GetTabTableGeneric(@"Allocated");
            
            MTNControlBase.FindClickRowInTable(_workOrderOperationsForm.TabGeneric.TableWithHeader.GetElement(),
                @"Work Order^" + workOrderId + "~Job Type^ToDo Cargo~To Do Task^ToDo_56113", ClickType.ContextClick,
                rowHeight: 16);
            //_workOrderEnquiryForm.TabGeneric.TableWithHeader.FindClickRow(
            //    [$"Work Order^{workOrderId}~Job Type^ToDo Cargo~To Do Task^ToDo_56113"],
             //   ClickType.ContextClick);
            _workOrderOperationsForm.ContextMenuSelect(@"Complete Job...");

            _completeJobForm = new CompleteJobForm($"Complete Job {TerminalId}");

            _completeJobForm.TblToDoTaskResource.FindClickRow(
                new[] { @"Resource^ToDoTaskResource56113" });

            _completeJobForm.GetToDoTaskResourceTableDetails();
            _completeJobForm.txtPerBox.SetValue(@"10");
            _completeJobForm.btnUpdate.DoClick();
            _completeJobForm.btnCompleteJob.DoClick();

            _workOrderEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
               // $"Status^Complete~Work Order Description^To Do Task~Voyage^{PEL.Voyage.VOYWORKORDERS}",
               // ClickType.Click);
            _workOrderEnquiryForm.TblData.FindClickRow([$"Status^Complete~Work Order Description^To Do Task~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.Click);
        }

        #region - Setup and Run Data Loads
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_56113_";

            //Delete Cargo Onsite
            CreateDataFileToLoad(@"DeleteCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='PEL'>\n         <TestCases>56113</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG56113A01</id>\n         <isoType>2200</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>BS02</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>VOYWORKORDERS</voyageCode>\n         <dischargePort>NZBLU</dischargePort>\n		 <messageMode>D</messageMode>\n      </CargoOnSite>\n   <CargoOnSite Terminal='PEL'>\n         <TestCases>56113</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG56113A02</id>\n         <isoType>2200</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>BS02</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>VOYWORKORDERS</voyageCode>\n         <dischargePort>NZBLU</dischargePort>\n\t\t <messageMode>D</messageMode>\n      </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //Create Cargo Onsite
            CreateDataFileToLoad(@"CreateCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='PEL'>\n         <TestCases>56113</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG56113A01</id>\n         <isoType>2200</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>BS02</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>VOYWORKORDERS</voyageCode>\n         <dischargePort>NZBLU</dischargePort>\n		 <messageMode>A</messageMode>\n      </CargoOnSite>\n   <CargoOnSite Terminal='PEL'>\n         <TestCases>56113</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG56113A02</id>\n         <isoType>2200</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>BS02</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>VOYWORKORDERS</voyageCode>\n         <dischargePort>NZBLU</dischargePort>\n\t\t <messageMode>A</messageMode>\n      </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
        #endregion - Setup and Run Data Loads
    }
}
