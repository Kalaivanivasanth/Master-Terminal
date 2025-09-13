using FlaUI.Core.AutomationElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Voyage_Functions;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using MTNUtilityClasses.Classes;
using System.Collections.Generic;
using System.Linq;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions.Operations
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41750 : MTNBase
    {
        OrderOfWorkForm _orderOfWorkForm;
        AllocateCranesForm _allocateCranesForm;
        VoyageJobOperationsForm _voyageJobOperationsForm;
        BulkTransferJobsForm _bulkTransferJobsForm;

        const string TestCaseNumber = "41750";
        const string VoyageId = TT1.Voyage.BISL41750A;

        readonly string[] CargoIds =
        {
            "JLG" + TestCaseNumber + "A01",
            "JLG" + TestCaseNumber + "A02",
            "JLG" + TestCaseNumber + "A03"
        };

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

       
        private void MTNInitialize()
        {
            TT1VOYUSER();
            LogInto<MTNLogInOutBO>();

            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);

        }

        [TestMethod]
        public void TransferDischargeJobFromOneCraneToAnother()
        {

            MTNInitialize();

            // Cargo Loading and LOLO colour coding are now done by resetData_41750

            // Step 3 - 5
            FormObjectBase.MainForm.OpenOrderOfWorkFromToolbar();
            _orderOfWorkForm = new OrderOfWorkForm(@"Order of Work TT1");

            _orderOfWorkForm.cmbVoyage.SetValue(VoyageId, doDownArrow: true);
            
            _orderOfWorkForm.DoFind();

            _orderOfWorkForm.DoCreateCraneJSSelectCrane();

            _allocateCranesForm = new AllocateCranesForm(@"Allocate Cranes TT1");

            string[] detailsToMove =
            {
                @"POS Normal Lift Crane",
                @"POT Twin Lift Crane"
            };
            _allocateCranesForm.lstCranes.MoveItemsBetweenList(_allocateCranesForm.lstCranes.LstLeft, detailsToMove);
            _allocateCranesForm.DoSave();

            // Get the Job ID
            _orderOfWorkForm.SetFocusToForm();
            _orderOfWorkForm.CreateOOWAllJobsTable();

            List<DataGridViewRow> rowInTable =
                new List<DataGridViewRow>
                (_orderOfWorkForm.tblOOWAllJobs2.GetElement().AsDataGridView()
                    .Rows
                .Where(r => r.Cells.Any() && r.Cells[1].Name.Contains(@"03  OD") &&
                       r.Cells[2].Name.Contains(@"Discharge") && r.Cells[3].Name.Contains(@"POT")));

            Assert.IsTrue(rowInTable.Count > 0,
                @"TestCase41750 - Can not find required OOW details: Aboard^03 OD~Type^Discharge~Crane^POT");

            var jobNumber = Miscellaneous.ReturnTextFromTableString(rowInTable[0].Cells[0].Name);
           
            // Step 7 - Job Operations
            FormObjectBase.MainForm.OpenJobOperationsFromToolbar();
            _voyageJobOperationsForm = new VoyageJobOperationsForm(@"Job Operations TT1");

            _voyageJobOperationsForm.cmbVoyage.SetValue(VoyageId);
            _voyageJobOperationsForm.lstVoyageJobs.FindItemInList(@"2036", @"BISL41750A|POT|Import  " + jobNumber);

            //_voyageJobOperationsForm.GetJobDetails();
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^030584~Seq.^2~Cargo ID^" + CargoIds[0] + @"~Total Quantity^1",
                // clickType: ClickType.None, rowHeight: 16);
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^050584~Seq.^2~Cargo ID^" + CargoIds[2] + @"~Total Quantity^1",
                // clickType: ClickType.None, rowHeight: 16);
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^030586~Seq.^1~Cargo ID^" + CargoIds[1] + @"~Total Quantity^1", 
                // ClickType.ContextClick, rowHeight: 16);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow([
                "Location Id^030584~Seq.^2~Cargo ID^" + CargoIds[0] + @"~Total Quantity^1",
                "Location Id^050584~Seq.^2~Cargo ID^" + CargoIds[2] + @"~Total Quantity^1",
            ], clickType: ClickType.None);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow([
                "Location Id^030586~Seq.^1~Cargo ID^" + CargoIds[1] + @"~Total Quantity^1"
            ], clickType: ClickType.ContextClick);
            _voyageJobOperationsForm.ContextMenuSelect(@"Transfer Work...");

            _bulkTransferJobsForm = new BulkTransferJobsForm();
            _bulkTransferJobsForm.cmbSelectCrane.SetValue(@"POS");
            var newJobNumber = _bulkTransferJobsForm.txtNewJobID.GetText();
            _bulkTransferJobsForm.chkUnqueue.DoClick();
            _bulkTransferJobsForm.DoOK();

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"JMTerminal");
            confirmationFormYesNo.CheckMessageMatch(@"Transfer ALL job items to crane POS?");
            confirmationFormYesNo.btnYes.DoClick();

            ConfirmationFormOK confirmationFormOK = 
                new ConfirmationFormOK(@"JMTerminal", automationIdMessage: @"3", automationIdOK: @"4");
            confirmationFormOK.CheckMessageMatch(@"New job " + newJobNumber + 
                                                         @" has been created from the items selected in Job " + jobNumber);
            confirmationFormOK.btnOK.DoClick();

            _voyageJobOperationsForm.SetFocusToForm();
            _voyageJobOperationsForm.CloseForm();

            FormObjectBase.MainForm.OpenJobOperationsFromToolbar();
            _voyageJobOperationsForm = new VoyageJobOperationsForm(@"Job Operations TT1");

            _voyageJobOperationsForm.lstVoyageJobs.FindItemInList(@"2036", @"BISL41750A|POS|Import  " + newJobNumber);
            //_voyageJobOperationsForm.GetJobDetails();
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^030584~Seq.^2~Cargo ID^" + CargoIds[0] + @"~Total Quantity^1",
                // clickType: ClickType.None, rowHeight: 16);
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^050584~Seq.^3~Cargo ID^" + CargoIds[2] + @"~Total Quantity^1",
                // clickType: ClickType.None, rowHeight: 16);
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^030586~Seq.^1~Cargo ID^" + CargoIds[1] + @"~Total Quantity^1",
                // ClickType.ContextClick, rowHeight: 16);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow([
                "Location Id^030584~Seq.^2~Cargo ID^" + CargoIds[0] + @"~Total Quantity^1",
                "Location Id^050584~Seq.^3~Cargo ID^" + CargoIds[2] + @"~Total Quantity^1",
                "Location Id^030586~Seq.^1~Cargo ID^" + CargoIds[1] + @"~Total Quantity^1"
            ], clickType: ClickType.None);
      }

       

    }

}
