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
    public class TestCase41720 : MTNBase
    {

        private OrderOfWorkForm _orderOfWorkForm;
        private AllocateCranesForm _allocateCranesForm;
        private VoyageJobOperationsForm _voyageJobOperationsForm;
        private SplitWorkFromJobForm _splitWorkFromJobForm;

        private const string TestCaseNumber = @"41720";
        private const string VoyageId = TT1.Voyage.BISL41720A;

        private readonly string[] CargoIds =
        {
            $"JLG{TestCaseNumber}A01",
            $"JLG{TestCaseNumber}A02",
            $"JLG{TestCaseNumber}A03"
        };

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
      
        [TestInitialize]
        public new void TestInitailze() {}


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();

            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);

        }

        [TestMethod]
        public void SplitTwinLiftDischargeJobBetweenTwinLiftAndSingleLiftCranes()
        {

            MTNInitialize();

            // Cargo Loading and LOLO colour coding are now done by resetData_41720

            // Step 3 - 5
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|Order Of Work");
            _orderOfWorkForm = new OrderOfWorkForm(@"Order of Work TT1");

            _orderOfWorkForm.cmbVoyage.SetValue(VoyageId, doDownArrow: true, searchSubStringTo: VoyageId.Length - 1);
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
                @"TestCase41720 - Can not find required OOW details: Aboard^03 OD~Type^Discharge~Crane^POT");

            var jobNumber = Miscellaneous.ReturnTextFromTableString(rowInTable[0].Cells[0].Name);
           
            // Step 7 - Job Operations
            FormObjectBase.NavigationMenuSelection(@"Operations|Job Operations");
            _voyageJobOperationsForm = new VoyageJobOperationsForm(@"Job Operations TT1");

            //MTNControlBase.SendTextToCombobox(_voyageJobOperationsForm.cmbVoyage, VoyageId);
            _voyageJobOperationsForm.cmbVoyage.SetValue(VoyageId);
            _voyageJobOperationsForm.lstVoyageJobs.FindItemInList(
                @"2036", VoyageId + @"|POT|Import  " + jobNumber);

           // _voyageJobOperationsForm.GetJobDetails();
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^030686~Seq.^1~Cargo ID^" + CargoIds[1] + @"~Total Quantity^1",
                // ClickType.None, rowHeight: 16);
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^030684~Seq.^2~Cargo ID^" + CargoIds[0] + @"~Total Quantity^1",
                // clickType: ClickType.None, rowHeight: 16);
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^050684~Seq.^2~Cargo ID^" + CargoIds[2] + @"~Total Quantity^1",
                // clickType: ClickType.ContextClick, rowHeight: 16);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow([
                "Location Id^030686~Seq.^1~Cargo ID^" + CargoIds[1] + @"~Total Quantity^1",
                "Location Id^030684~Seq.^2~Cargo ID^" + CargoIds[0] + @"~Total Quantity^1",
                ], ClickType.None);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow([
                "Location Id^050684~Seq.^2~Cargo ID^" + CargoIds[2] + @"~Total Quantity^1"
            ], ClickType.ContextClick);
            _voyageJobOperationsForm.ContextMenuSelect(@"Queue|Queue All");

            // Step 11
            MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.TblJobDetails.GetElement(),
                @"Location Id^030684~Seq.^2~Cargo ID^" + CargoIds[0] + @"~Total Quantity^1",
                clickType: ClickType.Click, rowHeight: 16);
            _voyageJobOperationsForm.DoMoveIt();

            MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.TblJobDetails.GetElement(),
                @"Location Id^050684~Seq.^2~Cargo ID^" + CargoIds[2] + @"~Total Quantity^1",
                clickType: ClickType.Click, rowHeight: 16);
            _voyageJobOperationsForm.DoMoveIt();
            // The below method was not reliably clicking both the rows in the table, sometimes it clicked just one row and clicked Move It button, failing the test.
            /* MTNControlBase.FindClickRowInTableMulti(_voyageJobOperationsForm.TblJobDetails.GetElement(), new string[]
             {
                 @"Location Id^030684~Seq.^2~Cargo ID^" + CargoIds[0] + @"~Total Quantity^1",
                 @"Location Id^050684~Seq.^2~Cargo ID^" + CargoIds[2] + @"~Total Quantity^1"
             }, rowHeight: 16);
             _voyageJobOperationsForm.DoMoveIt();*/
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
            // @"Location Id^030686~Seq.^1~Cargo ID^" + CargoIds[1] + @"~Total Quantity^1",
            // ClickType.ContextClick, rowHeight: 16);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow(["Location Id^030686~Seq.^1~Cargo ID^" + CargoIds[1] + @"~Total Quantity^1"], ClickType.ContextClick);
            _voyageJobOperationsForm.ContextMenuSelect(@"Split Work...");

            // Step 13 - 14
            _splitWorkFromJobForm = new SplitWorkFromJobForm(jobNumber);
            _splitWorkFromJobForm.cmbSelectCrane.SetValue(@"POS");
            var newJobNumber = _splitWorkFromJobForm.txtNewJobID.GetText();
            _splitWorkFromJobForm.chkUnqueue.DoClick();
            _splitWorkFromJobForm.DoOK();

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"JMTerminal");
            confirmationFormYesNo.CheckMessageMatch(@"Transfer ALL job items to crane POS?");
            confirmationFormYesNo.btnYes.DoClick();
            
            // Step 15
            _voyageJobOperationsForm.SetFocusToForm();
            var rowFound = MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.TblJobDetails.GetElement(),
                @"Location Id^030686~Seq.^1~Cargo ID^" + CargoIds[1] + @"~Total Quantity^1",
                ClickType.Click, rowHeight: 16, doAssert: false);

            Assert.IsTrue(!rowFound, @"TestCase41720 - Should not have found row for cargo id: " + CargoIds[1]);
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^MKBS01~Seq.^2~Cargo ID^" + CargoIds[0] + @"~Total Quantity^1",
                // clickType: ClickType.None, rowHeight: 16);
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^MKBS01~Seq.^2~Cargo ID^" + CargoIds[2] + @"~Total Quantity^1",
                // clickType: ClickType.None, rowHeight: 16);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow([
                "Location Id^MKBS01~Seq.^2~Cargo ID^" + CargoIds[0] + @"~Total Quantity^1",
                "Location Id^MKBS01~Seq.^2~Cargo ID^" + CargoIds[2] + @"~Total Quantity^1"
            ], clickType: ClickType.None);
            _voyageJobOperationsForm.CloseForm();

            // Step 16 - 17
            FormObjectBase.NavigationMenuSelection(@"Job Operations");
            _voyageJobOperationsForm = new VoyageJobOperationsForm(@"Job Operations TT1");

            _voyageJobOperationsForm.lstVoyageJobs.FindItemInList(@"2036", VoyageId + @"|POS|Import  " + newJobNumber);
            
           // _voyageJobOperationsForm.GetJobDetails();
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Location Id^030686~Seq.^1~Cargo ID^" + CargoIds[1] + @"~Total Quantity^1",
                // ClickType.Click, rowHeight: 16);
            // rowFound = MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Cargo ID^" + CargoIds[0] + @"~Total Quantity^1",
                // clickType: ClickType.None, rowHeight: 16, doAssert: false);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow([
                "Location Id^030686~Seq.^1~Cargo ID^" + CargoIds[1] + @"~Total Quantity^1",
                ], ClickType.Click);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow([
                "Cargo ID^" + CargoIds[0] + @"~Total Quantity^1"
            ], ClickType.None, doAssert: false);
            // rowFound = MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // @"Cargo ID^" + CargoIds[2] + @"~Total Quantity^1",
                // clickType: ClickType.None, rowHeight: 16, doAssert: false);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow(["Cargo ID^" + CargoIds[2] + @"~Total Quantity^1"], clickType: ClickType.None, doAssert: false);
            //Assert.IsTrue(!rowFound, @"TestCase41720 - Should not have found row for cargo id: " + CargoIds[2]);

        }

        

    }

}
