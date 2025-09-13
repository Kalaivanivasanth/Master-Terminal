using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Yard_Functions.Work_Order;
using MTNForms.FormObjects.Yard_Functions.Yard_Operations;
using System;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNUtilityClasses.Classes;
using ClickType = MTNGlobal.EnumsStructs.ClickType;
using MTNForms.FormObjects.Message_Dialogs;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39585 : MTNBase
    {
        const string TestCaseNumber = @"39585";
        const string TerminalId = "PEL";
        const string CargoId = "JLG" + TestCaseNumber + "A01";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            BaseClassInitialize_New(testContext);
        }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            userName = "WOUSER";
            LogInto<MTNLogInOutBO>(userName);
            SetupAndLoadInitializeData(TestContext);
        }

        /// <summary>
        /// To test that a Work Order can be created and completed successfully for one cargo item, from the Cargo Enquiry form.
        /// </summary>
        [TestMethod]
        public void CreateWorkOrderFromCargoEnquiryFor1CargoItem()
        {
            MTNInitialize();

            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm($"Cargo Enquiry {TerminalId}");

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{CargoId}"}, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Create Work Order");

            WorkOrderTasksForm workOrderTasksForm = new WorkOrderTasksForm($"Work Order Details {TerminalId}");
            MTNControlBase.FindClickRowInList(workOrderTasksForm.lstTasks, @"Move");

            workOrderTasksForm.btnOK.DoClick();
            NewWorkOrderForm newWorkOrderForm = new NewWorkOrderForm($"Cargo Work Order New Screen {TerminalId}");

            var date = DateTime.Now;
            newWorkOrderForm.txtReference.SetValue(TestCaseNumber);
            newWorkOrderForm.txtRequiredByDate.SetValue(date.ToString(@"ddMMyyyy"));
            newWorkOrderForm.txtRequiredByTime.SetValue(date.ToString(@"HHmm"));

            newWorkOrderForm.GetTaskDetailsTab(@"Move");
            newWorkOrderForm.cmbToTerminalAreaType.SetValue(@"Block Stack", doDownArrow: true, searchSubStringTo: 5);
            newWorkOrderForm.cmbToTerminalArea.SetValue(@"BS02", doDownArrow: true, downArrowSearchType: SearchType.StartsWith, searchSubStringTo: 3);
            newWorkOrderForm.btnSave.DoClick();

            FormObjectBase.MainForm.OpenWorkOrderEnquiryFromToolbar();
            WorkOrderEnquiryForm workOrderEnquiryForm = new WorkOrderEnquiryForm($"Work Order Enquiry {TerminalId}");

            workOrderEnquiryForm.txtCustomerReference.SetValue(TestCaseNumber);

            workOrderEnquiryForm.lstStatus.BtnAllLeft.DoClick();
            workOrderEnquiryForm.lstStatus.MoveItemsBetweenList(workOrderEnquiryForm.lstStatus.LstLeft, new [] { "User Entry"  });
            workOrderEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(workOrderEnquiryForm.tblData,
                // $"Status^User Entry~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            workOrderEnquiryForm.TblData.FindClickRow([$"Status^User Entry~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);
            workOrderEnquiryForm.ContextMenuSelect(@"Set Approved");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            var workOrderId = MTNControlBase.ReturnValueFromTableCell(workOrderEnquiryForm.TblData.GetElement(),
                @"Status^Approved~Work Order Description^Move Task #1", @"Work Order #");
            // MTNControlBase.FindClickRowInTable(workOrderEnquiryForm.tblData,
                // $"Status^Approved~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}",
                // ClickType.ContextClick);
            workOrderEnquiryForm.TblData.FindClickRow([$"Status^Approved~Work Order Description^Move Task #1~Voyage^{PEL.Voyage.VOYWORKORDERS}"], ClickType.ContextClick);

            workOrderEnquiryForm.ContextMenuSelect(@"Action Task | Move Task #1");

            FormObjectBase.MainForm.OpenYardOperationsFromToolbar();
            YardOperationsForm yardOperationsForm = new YardOperationsForm($"Yard Operations {TerminalId}");
            yardOperationsForm.ShowSearchTabDetails();
            yardOperationsForm.TxtSearcherJobId.SetValue($"{workOrderId}-A");
            yardOperationsForm.lstTasks.BtnAllLeft.DoClick();
            yardOperationsForm.lstTasks.MoveItemsBetweenList(yardOperationsForm.lstTasks.LstLeft, new [] { "Move" });
            yardOperationsForm.DoSearch();

            yardOperationsForm.ShowToolBarDetails();
            MTNControlBase.SetValue(yardOperationsForm.optWorkList); ;
            yardOperationsForm.ShowWorkListDetails();

            yardOperationsForm.TblDetailsWorkList.FindClickRow(new[] {
                $"Task Type^Move~Cargo ID^{CargoId}~Operator^MSC~Location Id^BS01~Job ID^{workOrderId}-A" }, ClickType.ContextClick);
               // $"Task Type^Move~Cargo ID^{CargoId}~ISO Type^2200~Operator^MSC~Location Id^BS01~Job ID^{workOrderId}-A" }, ClickType.ContextClick);
            yardOperationsForm.ContextMenuSelect(@"Move It|Move Selected");

            WarningErrorForm.CheckErrorMessagesExist($"Warnings for Operations Move {TerminalId}", null, false);

            workOrderEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(workOrderEnquiryForm.tblData,
               // $"Work Order #^{workOrderId}~Status^Complete~Work Order Description^Move Task #1", ClickType.ContextClick);
            workOrderEnquiryForm.TblData.FindClickRow([$"Work Order #^{workOrderId}~Status^Complete~Work Order Description^Move Task #1"], ClickType.ContextClick);

            workOrderEnquiryForm.ContextMenuSelect(@"Delete Work Order");

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Delete");
            confirmationFormYesNo.btnYes.DoClick();
        }

        #region Setup and Load data
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_" + TestCaseNumber + "_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n<AllCargoOnSite>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n<CargoOnSite Terminal='PEL'>\n<site>1</site>\n<cargoTypeDescr>ISO Container</cargoTypeDescr>\n<isoType>2200</isoType>\n<dischargePort>NZAKL</dischargePort>\n<id>JLG39585A01</id>\n<locationId>BS02</locationId>\n<messageMode>D</messageMode>\n<imexStatus>Export</imexStatus>\n<operatorCode>MSC</operatorCode>\n<voyageCode>VOYWORKORDERS</voyageCode>\n<weight>4500</weight>\n<transportMode>Road</transportMode>\n</CargoOnSite>\n</AllCargoOnSite>\n</JMTInternalCargoOnSite>");
            
            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n<AllCargoOnSite>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n<CargoOnSite Terminal='PEL'>\n<site>1</site>\n<cargoTypeDescr>ISO Container</cargoTypeDescr>\n<isoType>2200</isoType>\n<dischargePort>NZAKL</dischargePort>\n<id>JLG39585A01</id>\n<locationId>BS01</locationId>\n<messageMode>A</messageMode>\n<imexStatus>Export</imexStatus>\n<operatorCode>MSC</operatorCode>\n<voyageCode>VOYWORKORDERS</voyageCode>\n<weight>4500</weight>\n<transportMode>Road</transportMode>\n</CargoOnSite>\n</AllCargoOnSite>\n</JMTInternalCargoOnSite>");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
        #endregion Setup and Load data
    }
}
