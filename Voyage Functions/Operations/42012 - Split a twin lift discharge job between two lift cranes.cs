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
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions.Operations
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase42012 : MTNBase
    {

        private OrderOfWorkForm _orderOfWorkForm;
        private AllocateCranesForm _allocateCranesForm;
        private VoyageJobOperationsForm _voyageJobOperationsForm;
        private SplitWorkFromJobForm _splitWorkFromJobForm;

        private const string TestCaseNumber = @"42012";
        //private const string VoyageId = @"BISL42012A - Bermuda islander";


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
       
        private void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();
            CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);
        }

        [TestMethod]
        public void SplitTwinLiftDischargeJobBetweenTwoTwinLiftCranes()
        {
            //Step 2 - Cargo Loading and LOLO colour coding are now done by resetData_42012
            MTNInitialize();

            // Step 3 - 5
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|Order Of Work");
            _orderOfWorkForm = new OrderOfWorkForm(@"Order of Work TT1");

            _orderOfWorkForm.cmbVoyage.SetValue(TT1.Voyage.BISL42012A, doDownArrow: true);
            
            _orderOfWorkForm.DoFind();

            _orderOfWorkForm.DoCreateCraneJSSelectCrane();

            _allocateCranesForm = new AllocateCranesForm(@"Allocate Cranes TT1");
            
            _allocateCranesForm.lstCranes.MoveItemsBetweenList(_allocateCranesForm.lstCranes.LstLeft, new [] { "POT1 Twinlift Crane" });
            _allocateCranesForm.DoSave();

            // Step 6 - Get the Job ID
            _orderOfWorkForm.SetFocusToForm();
            _orderOfWorkForm.CreateOOWAllJobsTable();

            List<DataGridViewRow> rowInTable =
                new List<DataGridViewRow>
                (_orderOfWorkForm.tblOOWAllJobs2.GetElement().AsDataGridView()
                    .Rows
                .Where(r => r.Cells.Any() && r.Cells[1].Name.Contains(@"07  OD") &&
                       r.Cells[2].Name.Contains(@"Discharge") && r.Cells[3].Name.Contains(@"POT")));

            Assert.IsTrue(rowInTable.Count > 0,
                @"TestCase42012 - Can not find required OOW details: Aboard^07  OD~Type^Discharge~Crane^POT");

            var jobNumber = Miscellaneous.ReturnTextFromTableString(rowInTable[0].Cells[0].Name);

            // Step 7 - Job Operations
            FormObjectBase.NavigationMenuSelection(@"Operations|Job Operations");
            _voyageJobOperationsForm = new VoyageJobOperationsForm(@"Job Operations TT1");

            _voyageJobOperationsForm.cmbVoyage.SetValue(TT1.Voyage.BISL42012A);
            _voyageJobOperationsForm.lstVoyageJobs.FindItemInList(
                @"2036", $"{TT1.Voyage.BISL42012A}|POT|Import  {jobNumber}");

            //_voyageJobOperationsForm.GetJobDetails();

            // Step 8 - Move the Cargo
            MTNControlBase.FindClickRowInTableMulti(_voyageJobOperationsForm.TblJobDetails.GetElement(),
                new[]
                {
                    "Location Id^070186~Seq.^6~Cargo ID^JLG42012A04~Total Quantity^1",
                    "Location Id^090186~Seq.^6~Cargo ID^JLG42012A08~Total Quantity^1",
                }, clickType: ClickType.ContextClick, rowHeight: 16);
            Keyboard.Release(VirtualKeyShort.CONTROL);
            MTNControlBase.ContextMenuSelection(@"Table Context", @"Move It|Move Selected");
            // MTNControlBase.FindClickRowInTable(_voyageJobOperationsForm.tblJobDetails,
                // "Location Id^070186~Seq.^6~Cargo ID^JLG42012A04~Total Quantity^1",
                // clickType: ClickType.Click, rowHeight: 16);
            _voyageJobOperationsForm.TblJobDetails.FindClickRow(["Location Id^070186~Seq.^6~Cargo ID^JLG42012A04~Total Quantity^1"], clickType: ClickType.Click);
             MTNControlBase.FindClickRowInTableMulti(_voyageJobOperationsForm.TblJobDetails.GetElement(),
                new[]
                {
                    "Location Id^070686~Seq.^3~Cargo ID^JLG42012A01~Total Quantity^1",
                    "Location Id^090686~Seq.^3~Cargo ID^JLG42012A05~Total Quantity^1",
                }, clickType: ClickType.ContextClick, rowHeight: 16);
            MTNControlBase.ContextMenuSelection(@"Table Context", @"Split Work...");
            

            // Step 10 - 11
            _splitWorkFromJobForm = new SplitWorkFromJobForm(jobNumber);
            //MTNControlBase.SetValue(_splitWorkFromJobForm.cmbSelectCrane, @"POT1");
            _splitWorkFromJobForm.cmbSelectCrane.SetValue(@"POT1");
            var newJobNumber = _splitWorkFromJobForm.txtNewJobID.GetText();
            _splitWorkFromJobForm.chkUnqueue.DoClick();
            _splitWorkFromJobForm.DoOK();

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"JMTerminal");
            confirmationFormYesNo.btnYes.DoClick();

            _voyageJobOperationsForm.CloseForm();

            // Step 12 - 14 - 
            //FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Operations|Job Operations");
            FormObjectBase.NavigationMenuSelection(@"Job Operations");
            _voyageJobOperationsForm = new VoyageJobOperationsForm(@"Job Operations TT1");

            _voyageJobOperationsForm.lstVoyageJobs.FindItemInList(@"2036",
                $"{TT1.Voyage.BISL42012A}|POT1|Import  {newJobNumber}");
           // _voyageJobOperationsForm.GetJobDetails();

            _voyageJobOperationsForm.TblJobDetails.FindClickRow(new[]
            {
                "Location Id^070686~Seq.^3~Cargo ID^JLG42012A01~Total Quantity^1",
                "Location Id^090686~Seq.^3~Cargo ID^JLG42012A05~Total Quantity^1",
            });
            
            var rowsFound = _voyageJobOperationsForm.TblJobDetails.FindClickRow(new[]
            {
                "Cargo ID^JLG42012A02~Total Quantity^1", "Cargo ID^JLG42012A03~Total Quantity^1",
                "Cargo ID^JLG42012A04~Total Quantity^1", "Cargo ID^JLG42012A06~Total Quantity^1",
                "Cargo ID^JLG42012A07~Total Quantity^1", "Cargo ID^JLG42012A08~Total Quantity^1",
                "Cargo ID^JLG42012A09~Total Quantity^1", "Cargo ID^JLG42012A10~Total Quantity^1",
            }, doAssert: false);
            Assert.IsTrue(!string.IsNullOrEmpty(rowsFound),
                $"TestCase42012 - Should NOT have found the following row(s):\r\n{rowsFound}");
        }

       

    }

}

