using FlaUI.Core.AutomationElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using MTNUtilityClasses.Classes;
using System.Collections.Generic;
using System.Linq;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using FlaUI.Core.Input;
using System.Drawing;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions.Operations
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44130 : MTNBase
    {

        OrderOfWorkForm _orderOfWorkForm;
        ConfirmationFormYesNo _confirmationFormYesNo;
        LOLOPlanningForm _loloPlanning;

        const string TestCaseNumber = @"44130";
        const string VoyageId = @"VOY44130 - JOLLY DIAMANTE";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();


        private void MTNInitialize()
        {
            userName = "USER44130";
            LogInto<MTNLogInOutBO>(userName);

            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);

        }

        [TestMethod]
        public void AutoDischargeColourAfterLoadingSequenceAndPlan()
        {

            // Cargo Loading, LOLO colour coding are now done by resetData_44130
            MTNInitialize();

            // Go to Cargo Enquiry and get the Start position in the Planned mode
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry");
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG44130A01");
            cargoEnquiryForm.DoSearch();

            Miscellaneous.ClickAtSpecifiedPoint(cargoEnquiryForm.MoveModeOptions, ClickType.RightClick);
            cargoEnquiryForm.ContextMenuSelect(@"Planned Mode");
            
            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG44130A01", ClickType.Click);
            cargoEnquiryForm.tblData2.FindClickRow(new  [] { "ID^JLG44130A01" });
            Point startPoint = Mouse.Position;

            // Go to LOLO Planning screen
            //FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|LOLO Planning");
            FormObjectBase.MainForm.OpenLOLOPlanningFromToolbar();
            _loloPlanning = new LOLOPlanningForm();

            // Get the voyage and bay required
            _loloPlanning.SelectMultipleBaysHolds();
            _loloPlanning.cmbBHVoyage.SetValue(TT1.Voyage.VOY44130FullSpace);
            _loloPlanning.lstBaysHolds.MoveItemsBetweenList(_loloPlanning.lstBaysHolds.LstLeft, new [] { "Hold 07 - Bays 25, 27" });
            _loloPlanning.btnBHShow.DoClick();

            // Load Colour Code
            _loloPlanning.btnDisplayLoadColourCode.DoClick();
            _loloPlanning.GetLoadDetails();
            _loloPlanning.btnLoadPlanningMode.DoClick();

            //Get the endpoint
            var endpoint = new Point(_loloPlanning.grpBays.BoundingRectangle.X + 50,
                _loloPlanning.grpBays.BoundingRectangle.Y + 100);
            Mouse.Click(endpoint);

            // Drag and drop from the Cargo Enquiry to Lolo planning 
            Mouse.Drag(startPoint, endpoint);

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Move Error TT1");
            warningErrorForm.btnSave.DoClick();

            // On the Discharge mode, get the click point of the cell
            _loloPlanning.btnDisplayDischargeColourCode.DoClick();

            var clickpoint = new Point(_loloPlanning.grpBays.BoundingRectangle.X + 50,
               _loloPlanning.grpBays.BoundingRectangle.Y + 100);
            Mouse.Click(clickpoint);

            // Auto Discharge Colour Coding
            _loloPlanning.btnAutoDischargeColourCode.DoClick();
            _loloPlanning.GetAutoDischargeColourCodeActions();
            _loloPlanning.btnOK.DoClick();

            // Open Order of Work and get the Job number
            //FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|Order Of Work", forceReset: true);
            FormObjectBase.MainForm.OpenOrderOfWorkFromToolbar();
            _orderOfWorkForm = new OrderOfWorkForm(@"Order of Work TT1");

            _orderOfWorkForm.cmbVoyage.SetValue(VoyageId, doDownArrow: true);
            _orderOfWorkForm.DoFind();
            _orderOfWorkForm.CreateOOWAllJobsTable();

            var rowInTable =
                new List<DataGridViewRow>
                (_orderOfWorkForm.tblOOWAllJobs2.GetElement().AsDataGridView()
                    .Rows
                .Where(r => r.Cells.Any() && r.Cells[1].Name.Contains(@"25  OD") &&
                       r.Cells[2].Name.Contains(@"Load") && r.Cells[3].Name.Contains(@"CRN1")));

            Assert.IsTrue(rowInTable.Count > 0,
                @"TestCase44130 - Can not find required OOW details: Aboard^25  OD~Type^Load~Crane^CRN1");

            var jobNumber = Miscellaneous.ReturnTextFromTableString(rowInTable[0].Cells[0].Name);
            // MTNControlBase.FindClickRowInTable(_orderOfWorkForm.tblOOWAllJobs2.GetElement(),
                     // @"Job ID^" + jobNumber, ClickType.ContextClick, noOfHeaderRows: 3, rowHeight: 16);
            _orderOfWorkForm.tblOOWAllJobs2.FindClickRow(["Job ID^" + jobNumber], ClickType.ContextClick);

            _orderOfWorkForm.DoDelete();
            _confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Confirm Job Delete");
            _confirmationFormYesNo.btnYes.DoClick();

        }
    }

}
