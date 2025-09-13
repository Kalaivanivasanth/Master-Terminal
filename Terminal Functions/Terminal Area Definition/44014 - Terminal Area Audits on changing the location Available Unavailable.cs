using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Terminal_Functions.Terminal_Area_Definition
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44014 : MTNBase
    {
        ConfirmationFormYesNo _confirmationFormYesNo;
        AuditEnquiryForm _auditEnquiryForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void TerminalAreaAuditsonChangingLocationAvailableUnavailable()
        {
            
            MTNInitialize();
            
            // Step 1 Open Terminal Ops | Terminal Area Definition
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Area Definition");
            TerminalAreaDefinitionForm terminalAreaDefinitionForm = new TerminalAreaDefinitionForm(@"Terminal Area Definition TT1");


            // Step 2 Select the terminal area with ID = 44014 
            try
            {
                // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(terminalAreaDefinitionForm.tblTerminalAreas, @"ID^44014~Type^Straddle Stack", rowHeight: 16);
                terminalAreaDefinitionForm.TblTerminalAreas.FindClickRow(new[] { "ID^44014~Type^Straddle Stack" });
                
                // Step 3 Click the Delete button 
                //terminalAreaDefinitionForm.btnDelete.DoClick();
                terminalAreaDefinitionForm.DoDelete();


                // Step 4 Click the Yes button 
                _confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Confirm Deletion");
                _confirmationFormYesNo.btnYes.DoClick();

            }
            catch
            {

            }

            // Step 5 Click the New button and enter Type = Straddle Stack, ID = 44014
            terminalAreaDefinitionForm.DoNew();
            terminalAreaDefinitionForm.GetDefinitionData();

            terminalAreaDefinitionForm.RetryTypeDetails(@"Straddle Stack", @"Straddle Stack");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, @"ID", @"44014");

            // Step 6 Click the Save button 
            terminalAreaDefinitionForm.DoSave();

            // Step 7 Click the Edit button and click the New Straddle Stack tab 
            terminalAreaDefinitionForm.DoEdit();

            // Step 8 Select the first box, right click and select Unavailable
            terminalAreaDefinitionForm.GetNewStraddleStackTab();
            terminalAreaDefinitionForm.grpRowsColumns.Click();
            terminalAreaDefinitionForm.btnUnavailable.DoClick();

            // Step 9 Click the Save button 
            terminalAreaDefinitionForm.DoSave();

            // Step 10 Click the Yes button
            _confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Terminal Area Definition TT1");
            _confirmationFormYesNo.btnYes.DoClick();


            // Step 11 In the left hand table with ID = 44014 selected, right click and select View Audits for Straddle Stack - 44014 
            //terminalAreaDefinitionForm.GetTerminalAreasTable();
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblTermAreaType, @"Term Area Type", @"Straddle Stack",
                    EditRowDataType.ComboBox, doDownArrow: true, searchSubStringTo: 5);
            terminalAreaDefinitionForm.DoSearch();
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(terminalAreaDefinitionForm.tblTerminalAreas, @"ID^44014~Type^Straddle Stack", ClickType.ContextClick, rowHeight:16);
            terminalAreaDefinitionForm.TblTerminalAreas.FindClickRow(new[] { "ID^44014~Type^Straddle Stack" }, ClickType.ContextClick);
            terminalAreaDefinitionForm.ContextMenuSelect(@"View Audits for Straddle Stack - 44014");

            // Step 12 Select the last entry and check that the audit details
            _auditEnquiryForm = new AuditEnquiryForm();
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_auditEnquiryForm.tblAuditItems, @"Description^Straddle Stack - 44014~Audit Type^Updated", rowHeight: 16, findInstance:2);
            _auditEnquiryForm.TblAuditItems.FindClickRow(new[] { "Description^Straddle Stack - 44014~Audit Type^Updated" }, rowInstance: 2);
            // column 0 (before)
            string tableValue = MTNControlBase.FindClickRowInTableVHeader(_auditEnquiryForm.tblAuditDetails, @"033", clickType: ClickType.None);
            Assert.IsTrue(tableValue == "", @"Expected value: , Actual value: " + tableValue);
            // column 1 (after)
            tableValue = MTNControlBase.FindClickRowInTableVHeader(_auditEnquiryForm.tblAuditDetails, @"033", clickType: ClickType.None, returnColumn: 1);
            Assert.IsTrue(tableValue == "LOCATION UNAVAILABLE - ADDED - NOT AVAILABLE", @"Expected value: LOCATION UNAVAILABLE - ADDED - NOT AVAILABLE, Actual value: " + tableValue);


            // Step 13 Close the Audit Details form 
            _auditEnquiryForm.CloseForm();

            // Step 14 In the Terminal Area Definition form, click the Edit button
            terminalAreaDefinitionForm.DoEdit();

            // Step 15 Select the first box which was marked Unavailable and right click and select Clear (Available) 
            terminalAreaDefinitionForm.GetNewStraddleStackTab();
            terminalAreaDefinitionForm.grpRowsColumns.RightClick();
            terminalAreaDefinitionForm.ContextMenuSelect(@"Clear (Available)");

            // Step 16 Click the Save button 
            terminalAreaDefinitionForm.DoSave();

            // Step 17 Click the Yes button 
            _confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Terminal Area Definition TT1");
            _confirmationFormYesNo.btnYes.DoClick();

            // Step 18 In the left hand table with ID = 44014 selected, right click and select View Audits for Straddle Stack - 44014  
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date  MTNControlBase.FindClickRowInTable(terminalAreaDefinitionForm.tblTerminalAreas, @"ID^44014~Type^Straddle Stack", ClickType.ContextClick, rowHeight:16);
            terminalAreaDefinitionForm.TblTerminalAreas.FindClickRow(new[] { "ID^44014~Type^Straddle Stack" }, ClickType.ContextClick);
            terminalAreaDefinitionForm.ContextMenuSelect(@"View Audits for Straddle Stack - 44014");

            // Step 19 Select the last entry and check that the audit details 
            _auditEnquiryForm = new AuditEnquiryForm();
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_auditEnquiryForm.tblAuditItems, @"Description^Straddle Stack - 44014~Audit Type^Updated", rowHeight: 16, findInstance:3);
            _auditEnquiryForm.TblAuditItems.FindClickRow(new[] { "Description^Straddle Stack - 44014~Audit Type^Updated" }, rowInstance: 3);
            // column 0 (before)
            tableValue = MTNControlBase.FindClickRowInTableVHeader(_auditEnquiryForm.tblAuditDetails, @"033", clickType: ClickType.None);
            Assert.IsTrue(tableValue == "", @"Expected value: , Actual value: " + tableValue);
            // column 1 (after)
            tableValue = MTNControlBase.FindClickRowInTableVHeader(_auditEnquiryForm.tblAuditDetails, @"033", clickType: ClickType.None, returnColumn: 1);
            Assert.IsTrue(tableValue == "LOCATION AVAILABILITY - ADDED - FULLY AVAILABLE", @"Expected value: LOCATION AVAILABILITY - ADDED - FULLY AVAILABLE, Actual value: " + tableValue);

        }
    }
}
