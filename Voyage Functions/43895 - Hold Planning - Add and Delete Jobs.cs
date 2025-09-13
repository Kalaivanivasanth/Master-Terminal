using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43895 : MTNBase
    {
        HoldPlanningForm _holdPlanningForm;
        ConfirmQuantityToMoveForm _confirmQuantityToMoveForm;
        ConfirmationFormYesNo _confirmationFormYesNo;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"resetData_43895");
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void RemoveHoldPlanningJobBreakBulk()
        {
            
            MTNInitialize();
           
            // 1. set up dates for the test
            var dateToday = DateTime.Today.Date;
            var dateTomorrow = dateToday.AddDays(1);

            // 2. Navigate to Hold Planning and open voyage MSCK000002
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|Hold Planning");
            _holdPlanningForm = new HoldPlanningForm();
            _holdPlanningForm.cmbVoyage.SetValue(TT1.Voyage.MSCK000002FullSpace);

            // 3. Navigate to Unallocated cargo on vessl and find cargo JLG43895A01
            _holdPlanningForm.GetTabTableGeneric(@"Unallocated Cargo On Vessel", @"2027");
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric, @"ID^JLG43895A01", ClickType.Click,rowHeight: 16);
             _holdPlanningForm.TabGeneric.TableWithHeader.FindClickRow(["ID^JLG43895A01"], ClickType.Click);
            //_holdPlanningForm.TblGeneric.FindClickRow(["ID^JLG43895A01"], ClickType.Click);

            // 4. (1st Iteration) Create new hold job for JLG43895A01 - create a job for part of the break-bulk cargo
            _holdPlanningForm.DoNewHoldPlanning();
            _holdPlanningForm.cmbWorkPoint.SetValue("WP0001 - Work point WP0001 description", searchSubStringTo: 6, additionalWaitTimeout: 200, doDownArrow: true);
            _holdPlanningForm.txtJobId.SetValue(@"43895JOB");
            _holdPlanningForm.cmbTransferType.SetValue(VoyageJobTransferType.LIFTONLISTOFFLOLO,
                additionalWaitTimeout: 1000, doDownArrow: true, searchSubStringTo: VoyageJobTransferType.LIFTONLISTOFFLOLO.Length - 1);
            _holdPlanningForm.txtShiftStartDate.SetValue(dateToday.ToString(@"ddMMyyyy"));
            _holdPlanningForm.txtShiftStartTime.SetValue(@"1200");
            _holdPlanningForm.txtShiftEndDate.SetValue(dateTomorrow.ToString(@"ddMMyyyy"));
            _holdPlanningForm.txtShiftEndTime.SetValue(@"1200");
            _holdPlanningForm.cmbPlanDischargeTo.SetValue(TT1.TerminalArea.BS4078, searchSubStringTo: 3, additionalWaitTimeout: 200, doDownArrow: true);

            _holdPlanningForm.DoSaveHoldPlanning();
            _confirmQuantityToMoveForm = new ConfirmQuantityToMoveForm(formTitle: @"Confirm quantity to move");
            _confirmQuantityToMoveForm.txtQuantity.SetValue(@"20");
            
            _confirmQuantityToMoveForm.btnOK.DoClick();

            // 5. (1st Iteration) Open new job tab and confirm the cargo entry
            _holdPlanningForm.GetForm().FocusNative();
            _holdPlanningForm.GetTabTableGeneric(@"43895JOB Discharge");
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric,@"Total Quantity^20~ID^JLG43895A01~D Job ID^43895JOB", ClickType.Click, rowHeight: 16);
             _holdPlanningForm.TabGeneric.TableWithHeader.FindClickRow(["Total Quantity^20~ID^JLG43895A01~D Job ID^43895JOB"], ClickType.Click);
            //_holdPlanningForm.TblGeneric.FindClickRow(["Total Quantity^20~ID^JLG43895A01~D Job ID^43895JOB"], ClickType.Click);

            // 6. (1st Iteration) Delete the job and go back to Unallocated Cargo on Vessel Tab
            _holdPlanningForm.DoDeleteHoldPlanning();
            _confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Confirm Job Delete");
            _confirmationFormYesNo.btnYes.DoClick();
            _holdPlanningForm.GetForm().SetForeground();
            _holdPlanningForm.GetTabTableGeneric(@"Unallocated Cargo On Vessel", @"2027");
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric, @"ID^JLG43895A01", ClickType.Click, rowHeight: 16);
             _holdPlanningForm.TabGeneric.TableWithHeader.FindClickRow(["ID^JLG43895A01"], ClickType.Click);
            //_holdPlanningForm.TblGeneric.FindClickRow(["ID^JLG43895A01"], ClickType.Click);
            _holdPlanningForm.DoNewHoldPlanning();
            _holdPlanningForm.cmbWorkPoint.SetValue("WP0001 - Work point WP0001 description", searchSubStringTo: 6, additionalWaitTimeout: 200, doDownArrow: true);
            _holdPlanningForm.txtJobId.SetValue(@"43895JOB");
            _holdPlanningForm.cmbTransferType.SetValue(VoyageJobTransferType.LIFTONLISTOFFLOLO, 
                additionalWaitTimeout: 200, doDownArrow: true,
                searchSubStringTo: VoyageJobTransferType.LIFTONLISTOFFLOLO.Length < 6
                    ? VoyageJobTransferType.LIFTONLISTOFFLOLO.Length - 1 : 6);
            _holdPlanningForm.txtShiftStartDate.SetValue(dateToday.ToString(@"ddMMyyyy"));
            _holdPlanningForm.txtShiftStartTime.SetValue(@"1200");
            _holdPlanningForm.txtShiftEndDate.SetValue(dateTomorrow.ToString(@"ddMMyyyy"));
            _holdPlanningForm.txtShiftEndTime.SetValue(@"1200");
            _holdPlanningForm.cmbPlanDischargeTo.SetValue(TT1.TerminalArea.BS4078, searchSubStringTo: 3, additionalWaitTimeout: 200, doDownArrow: true);
            _holdPlanningForm.DoSaveHoldPlanning();
            _confirmQuantityToMoveForm = new ConfirmQuantityToMoveForm(formTitle: @"Confirm quantity to move");
            _confirmQuantityToMoveForm.txtQuantity.SetValue(@"40");
            _confirmQuantityToMoveForm.btnOK.DoClick();

            // 8. (2nd Iteration) Open new job tab and confirm the cargo entry
            _holdPlanningForm.GetForm().FocusNative();
            _holdPlanningForm.GetTabTableGeneric(@"43895JOB Discharge");
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric, @"Total Quantity^40~ID^JLG43895A01~D Job ID^43895JOB", ClickType.Click, rowHeight: 16);
             _holdPlanningForm.TabGeneric.TableWithHeader.FindClickRow(["Total Quantity^40~ID^JLG43895A01~D Job ID^43895JOB"], ClickType.Click);
            //_holdPlanningForm.TblGeneric.FindClickRow(["Total Quantity^40~ID^JLG43895A01~D Job ID^43895JOB"], ClickType.Click);

            // 9. (2nd Iteration) Delete the job and go back to Unallocated Cargo on Vessel Tab
            _holdPlanningForm.DoDeleteHoldPlanning();
            _confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Confirm Job Delete");
            _confirmationFormYesNo.btnYes.DoClick();
            _holdPlanningForm.GetTabTableGeneric(@"Unallocated Cargo On Vessel", @"2027");
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric, @"ID^JLG43895A02", ClickType.Click, rowHeight: 16);
             _holdPlanningForm.TabGeneric.TableWithHeader.FindClickRow(["ID^JLG43895A02"], ClickType.Click);
            //_holdPlanningForm.TblGeneric.FindClickRow(["ID^JLG43895A02"], ClickType.Click);
            _holdPlanningForm.DoNewHoldPlanning();
            _holdPlanningForm.cmbWorkPoint.SetValue("WP0001 - Work point WP0001 description", searchSubStringTo: 6, additionalWaitTimeout: 200, doDownArrow: true);
            _holdPlanningForm.txtJobId.SetValue(@"43895JOB");
            _holdPlanningForm.cmbTransferType.SetValue(VoyageJobTransferType.LIFTONLISTOFFLOLO,
                additionalWaitTimeout: 200, doDownArrow: true, searchSubStringTo: VoyageJobTransferType.LIFTONLISTOFFLOLO.Length - 1);
            _holdPlanningForm.txtShiftStartDate.SetValue(dateToday.ToString(@"ddMMyyyy"));
            _holdPlanningForm.txtShiftStartTime.SetValue(@"1200");
            _holdPlanningForm.txtShiftEndDate.SetValue(dateTomorrow.ToString(@"ddMMyyyy"));
            _holdPlanningForm.txtShiftEndTime.SetValue(@"1200");
            _holdPlanningForm.cmbPlanDischargeTo.SetValue(TT1.TerminalArea.BS4078, searchSubStringTo: 3,
                additionalWaitTimeout: 200, doDownArrow: true);
            _holdPlanningForm.DoSaveHoldPlanning();

            // 11. (3rd Iteration) navigate to new discharge job and validate data
            _holdPlanningForm.GetTabTableGeneric(@"43895JOB Discharge");
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric,@"Total Quantity^1~ID^JLG43895A02~D Job ID^43895JOB", ClickType.Click, rowHeight: 16);
             _holdPlanningForm.TabGeneric.TableWithHeader.FindClickRow(["Total Quantity^1~ID^JLG43895A02~D Job ID^43895JOB"], ClickType.Click);
            //_holdPlanningForm.TblGeneric.FindClickRow(["Total Quantity^1~ID^JLG43895A02~D Job ID^43895JOB"], ClickType.Click);

            // 12. (3rd Iteration) delete job
            //holdPlanningForm.btnDeleteHoldPlanning.DoClick();
            _holdPlanningForm.DoDeleteHoldPlanning();
            _confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Confirm Job Delete");
            _confirmationFormYesNo.btnYes.DoClick();
         
        }
       

    }

}
