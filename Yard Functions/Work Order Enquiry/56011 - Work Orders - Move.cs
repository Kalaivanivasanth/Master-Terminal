using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Work_Order;
using MTNForms.FormObjects.Yard_Functions.Yard_Operations;
using MTNUtilityClasses.Classes;
using System;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;


namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Work_Order_Enquiry
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase56011 : MTNBase
    {
        WorkOrderEnquiryForm _workOrderEnquiryForm;
        NewWorkOrderForm _newWorkOrderForm;
        WorkOrderTasksForm _workOrderTasksForm;
        CargoSearchForm _cargoSearchForm;
        YardOperationsForm _yardOperationsForm;

        private const string TestCaseNumber = @"56011";
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
            SetupAndLoadInitializeData(TestContext);
        }      

        /// <summary>
        /// To test the Move functionality using Work Order Enquiry
        /// </summary>
        [TestMethod]
        public void WorkOrderEnquiryMove()
        {
            MTNInitialize();
         
            // Open Work Order Enquiry
            FormObjectBase.MainForm.OpenWorkOrderEnquiryFromToolbar();
            _workOrderEnquiryForm = new WorkOrderEnquiryForm($"Work Order Enquiry {TerminalId}");

            _workOrderEnquiryForm.lstStatus.BtnAllLeft.DoClick();
            string[] detailsToMove =
            {
                @"User Entry"
            };
            _workOrderEnquiryForm.lstStatus.MoveItemsBetweenList(_workOrderEnquiryForm.lstStatus.LstLeft, detailsToMove);
            _workOrderEnquiryForm.DoSearch();

            // Click on New Cargo Work Order
            _workOrderEnquiryForm.DoNewCargoWorkOrder();

            _newWorkOrderForm = new NewWorkOrderForm($"Cargo Work Order New Screen {TerminalId}");
            var date = DateTime.Now;

            _newWorkOrderForm.txtReference.SetValue(TestCaseNumber);
            _newWorkOrderForm.txtRequiredByDate.SetValue(date.ToString(@"ddMMyyyy"));
            _newWorkOrderForm.txtRequiredByTime.SetValue(date.ToString(@"HHmm"));
            _newWorkOrderForm.btnAddTask.DoClick();

            // Create a work order for Move Task
            _workOrderTasksForm = new WorkOrderTasksForm($"Work Order Details {TerminalId}");
            MTNControlBase.FindClickRowInList(_workOrderTasksForm.lstTasks, @"Move");

            _workOrderTasksForm.btnOK.DoClick();
            _newWorkOrderForm.SetFocusToForm();

            _newWorkOrderForm.GetTaskDetailsTab(@"Move");
            _newWorkOrderForm.cmbToTerminalAreaType.SetValue(@"Block Stack");
            _newWorkOrderForm.cmbToTerminalArea.SetValue(@"BS01 [E JLG39438*; E JLG41361; E 52561", doDownArrow: true,
                downArrowSearchType: SearchType.StartsWith, searchSubStringTo: 4);
            _newWorkOrderForm.chkAutoQueue.DoClick();

            _newWorkOrderForm.GetTaskDetailsTab(@"Selected Cargo Items [0]");
            _newWorkOrderForm.btnAddCargoItems.DoClick();

            _cargoSearchForm = new CargoSearchForm($"Cargo Search {TerminalId}");
            _cargoSearchForm.txtCargoId.SetValue($"JLG{TestCaseNumber}A01");
            _cargoSearchForm.btnSearchAll.DoClick();
            // MTNControlBase.FindClickRowInTable(_cargoSearchForm.tblResults, $"ID^JLG{TestCaseNumber}A01");
            _cargoSearchForm.TblResults.FindClickRow([$"ID^JLG{TestCaseNumber}A01"]);

            _cargoSearchForm.btnAdd.DoClick();
            _cargoSearchForm.btnOK.DoClick();

            _newWorkOrderForm.SetFocusToForm();
            _newWorkOrderForm.btnSave.DoClick();
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                // $"Status^User Entry~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow([$"Status^User Entry~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Set Approved");

            var workOrderId = MTNControlBase.ReturnValueFromTableCell(_workOrderEnquiryForm.TblData.GetElement(),
                $"Status^Approved~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}", @"Work Order #");
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
                // $"Status^Approved~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow([$"Status^Approved~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);
            _workOrderEnquiryForm.ContextMenuSelect(@"Action Task|Move Task #1");

            warningErrorForm = new WarningErrorForm(formTitle: $"Warnings for Work Order Enquiry {TerminalId}");
            warningErrorForm.btnSave.DoClick();

            // Open Cargo Enquiry and check if the cargo item is queued to BS01
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", $"JLG{TestCaseNumber}A01");
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.GetInformationTable();

            var infoDetails =
               MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblInformation, @"Info").Replace(
                   "\r", String.Empty).Replace("\n", String.Empty);
            Assert.IsTrue(infoDetails.Equals(@"Stops Present. QUEUED to BS01"),
                @"TestCase56011 - Info does not match.  Expected: Stops Present. QUEUED to BS01" +
                @"Actual: " + infoDetails);

            // On the Yard Operations form, check if the Move job is created
            FormObjectBase.MainForm.OpenYardOperationsFromToolbar();
            _yardOperationsForm = new YardOperationsForm($"Yard Operations {TerminalId}");
            _yardOperationsForm.ShowSearchTabDetails();
            _yardOperationsForm.lstTasks.BtnAllLeft.DoClick();

            detailsToMove = new string[]
            {
                @"Move"
            };
            _yardOperationsForm.lstTasks.MoveItemsBetweenList(_yardOperationsForm.lstTasks.LstLeft, detailsToMove);
            _yardOperationsForm.DoSearch();

            _yardOperationsForm.ShowToolBarDetails();
            MTNControlBase.SetValue(_yardOperationsForm.optWorkList);
            _yardOperationsForm.ShowWorkListDetails();
            // MTNControlBase.FindClickRowInTable(_yardOperationsForm.tblDetails, @"Task Type^Move~Job ID^" + workOrderId + "-A",
                // ClickType.ContextClick);
            _yardOperationsForm.TblDetails.FindClickRow(["Task Type^Move~Job ID^" + workOrderId + "-A"], ClickType.ContextClick);
            _yardOperationsForm.ContextMenuSelect(@"Move It|Move Selected");

            warningErrorForm = new WarningErrorForm(formTitle: $"Warnings for Operations Move {TerminalId}");
            warningErrorForm.btnSave.DoClick();

            // On the Cargo Enquiry check the Location id is BS01
            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.GetLocationTable(@"4084");
            string strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblLocationDetails, @"Location ID");
            Assert.IsTrue(strValue.Equals(@"BS01"), "Location ID expected to be BS01, actual is: " + strValue);

            _workOrderEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_workOrderEnquiryForm.tblData,
               // $"Status^Complete~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
               // ClickType.ContextClick);
            _workOrderEnquiryForm.TblData.FindClickRow([$"Status^Complete~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);

            _workOrderEnquiryForm.ContextMenuSelect(@"Delete Work Order");

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Delete");
            confirmationFormYesNo.btnYes.DoClick();
        }
        #region - Setup and Run Data Loads
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_56011_";
            
            //Delete Cargo Onsite
            CreateDataFileToLoad(@"DeleteCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='PEL'>\n         <TestCases>56011</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG56011A01</id>\n         <isoType>2200</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>BS01</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>VOYWORKORDERS</voyageCode>\n         <dischargePort>NZBLU</dischargePort>\n		 <messageMode>D</messageMode>\n      </CargoOnSite>\n       </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //Create Cargo Onsite
            CreateDataFileToLoad(@"CreateCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='PEL'>\n         <TestCases>56011</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG56011A01</id>\n         <isoType>2200</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>BS02</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>VOYWORKORDERS</voyageCode>\n         <dischargePort>NZBLU</dischargePort>\n		 <messageMode>A</messageMode>\n      </CargoOnSite>\n       </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
        #endregion - Setup and Run Data Loads
    }
}
