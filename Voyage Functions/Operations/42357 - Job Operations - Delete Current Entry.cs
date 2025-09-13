using FlaUI.Core.AutomationElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using MTNUtilityClasses.Classes;
using System.Collections.Generic;
using System.Linq;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions.Operations
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase42357 : MTNBase
    {

        OrderOfWorkForm _orderOfWorkForm;
        VoyageJobOperationsForm _voyageJobOperationsForm;
        WarningFormOKCancel _warningFormOKCancel;
        ConfirmationFormYesNo _confirmationFormYesNo;

        const string TestCaseNumber = @"42357";
        const string VoyageId = TT1.Voyage.MESDAI200001;


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
       
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        private void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>("TT1VOYUSER");
            CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);
        }


        [TestMethod]
        public void JobOperations_DeleteCurrentEntry()
        {

            //Step 2 - Cargo Loading, LOLO colour coding and Job load to operations are now done by resetData_42357
            MTNInitialize();

            //Step 3
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|Order Of Work");
            FormObjectBase.MainForm.OpenOrderOfWorkFromToolbar();
            _orderOfWorkForm = new OrderOfWorkForm();

            _orderOfWorkForm.cmbVoyage.SetValue(VoyageId, doDownArrow: true);
            
            _orderOfWorkForm.DoFind();

            _orderOfWorkForm.CreateOOWAllJobsTable();

            List<DataGridViewRow> rowInTable =
                new List<DataGridViewRow>
                (_orderOfWorkForm.tblOOWAllJobs2.GetElement().AsDataGridView()
                    .Rows
                .Where(r => r.Cells.Any() && r.Cells[1].Name.Contains(@"01  OD") &&
                       r.Cells[2].Name.Contains(@"Discharge") && r.Cells[3].Name.Contains(@"CRN1")));

            Assert.IsTrue(rowInTable.Count > 0,
                @"TestCase8000 - Can not find required OOW details: Aboard^01  OD~Type^Discharge~Crane^CRN1");

            var jobNumber = Miscellaneous.ReturnTextFromTableString(rowInTable[0].Cells[0].Name);
            
            
            // Step 4 - Job Operations - Delete the Current Entry
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Operations|Job Operations");
            FormObjectBase.MainForm.OpenJobOperationsFromToolbar();
            _voyageJobOperationsForm = new VoyageJobOperationsForm();

            _voyageJobOperationsForm.cmbVoyage.SetValue(VoyageId, doDownArrow: true);
            _voyageJobOperationsForm.lstVoyageJobs.FindItemInList(@"2036", @"MESDAI200001|CRN1|Import  " + jobNumber);

           // _voyageJobOperationsForm.GetJobDetails();

            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date     @"Location Id^011092~Seq.^1~Cargo ID^JLG42357A01~Total Quantity^1",
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date     clickType: ClickType.ContextClick, rowHeight: 16);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow(new[] { "Location Id^011092~Seq.^1~Cargo ID^JLG42357A01~Total Quantity^1" },
                ClickType.ContextClick);
            _voyageJobOperationsForm.ContextMenuSelect(@"Delete Current Entry");

            _warningFormOKCancel = new WarningFormOKCancel(@"Warning");
            _warningFormOKCancel.btnYes.DoClick();

            _voyageJobOperationsForm.CloseForm();
            
            //Step 5 - Delete the job 
            _orderOfWorkForm.SetFocusToForm();
            _orderOfWorkForm.CreateOOWAllJobsTable();

            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_orderOfWorkForm.tblOOWAllJobs,
             // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date         @"Job ID^" + jobNumber, ClickType.ContextClick, noOfHeaderRows: 3, rowHeight: 16);
            _orderOfWorkForm.tblOOWAllJobs2.FindClickRow(new[] { $"Job ID^{jobNumber}" }, ClickType.ContextClick);
            

            _orderOfWorkForm.DoDelete();
            _confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Confirm Job Delete");
            _confirmationFormYesNo.btnYes.DoClick();

        }

       

    }

}
