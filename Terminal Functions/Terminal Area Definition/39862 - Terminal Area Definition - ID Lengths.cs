using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using FlaUI.Core.WindowsAPI;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Terminal_Functions.Terminal_Area_Definition
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39862 : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();
            
            CallJadeScriptToRun(TestContext, @"resetData_39862");
        }


        [TestMethod]
        public void IDLengths()
        {
            MTNInitialize();
            
            // Step 1
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Area Definition");
            TerminalAreaDefinitionForm terminalAreaDefinitionForm = new TerminalAreaDefinitionForm("Terminal Area Definition TT1");
            
            var terminalAreaType = "Yard Interchange";
            CreateTerminalAreaAndValidateCreated(terminalAreaDefinitionForm, terminalAreaType, "IA39862", 
                "INTERCHANGE AREA FOR 39682", "4", doSearch: true);

            // Step 4
            terminalAreaDefinitionForm.DoEdit();
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, "ID", "IA39862A");
            terminalAreaDefinitionForm.DoSave();

            // Step 5 - 6
            DeleteTerminalArea(terminalAreaDefinitionForm, terminalAreaType, "IA39862A",
                "INTERCHANGE AREA FOR 39682", "4");

            // Step 7
            CreateTerminalAreaAndValidateCreated(terminalAreaDefinitionForm, terminalAreaType, "IA39862B", 
                "INTERCHANGE AREA FOR 39682B", "6");
            
            // Step 8 - 9
            DeleteTerminalArea(terminalAreaDefinitionForm, terminalAreaType, @"IA39862B",
                @"INTERCHANGE AREA FOR 39682B", @"6");
            
            
            terminalAreaType = "Gridded Block Stack";
            terminalAreaDefinitionForm.SearchForAreaType(terminalAreaType);
            
            terminalAreaDefinitionForm.DoNew();

            terminalAreaDefinitionForm.GetDefinitionData();
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, "ID", "GB39862");
            MTNControlBase.FindValidateClickEditTableData(terminalAreaDefinitionForm.tblDefinitionData, "ID^");

            // Step 11
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, "ID", "GB3986");
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, "Comment", "GRIDDED BLOCK STACK 39862");
            terminalAreaDefinitionForm.DoSave();

            // Step 12 - 13
            DeleteTerminalArea(terminalAreaDefinitionForm, terminalAreaType, @"GB3986",
                $"{terminalAreaType.ToUpper()} 39862");
            
            terminalAreaType = "Crane Transfer Area";
            terminalAreaDefinitionForm.SearchForAreaType(terminalAreaType);
            
            terminalAreaDefinitionForm.DoNew();

            terminalAreaDefinitionForm.GetDefinitionData();
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, "ID", "CTA39862");
            MTNControlBase.FindValidateClickEditTableData(terminalAreaDefinitionForm.tblDefinitionData, "ID^");

            // Step 11
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, "ID", "CTA398");
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, "Comment", $"{terminalAreaType.ToUpper()} 39862");
            terminalAreaDefinitionForm.DoSave();

            // Step 12 - 13
            DeleteTerminalArea(terminalAreaDefinitionForm, terminalAreaType, @"CTA398",
                $"{terminalAreaType.ToUpper()} 39862") ;
            
        }

        static void CreateTerminalAreaAndValidateCreated(TerminalAreaDefinitionForm terminalAreaDefinitionForm,
            string terminalAreaType, string terminalAreaId, string terminalAreaComment, string terminalAreaTypeNoOfLanes,
            bool doSearch = false)
        {
            if (doSearch)
                terminalAreaDefinitionForm.SearchForAreaType(terminalAreaType);
            
            terminalAreaDefinitionForm.DoNew();
            terminalAreaDefinitionForm.GetDefinitionData();

            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, "ID", terminalAreaId);
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, "Comment",
                terminalAreaComment, waitTime: 100);
            MTNControlBase.SetValueInEditTable(terminalAreaDefinitionForm.tblDefinitionData, "No. Of Lanes", terminalAreaTypeNoOfLanes);
            terminalAreaDefinitionForm.DoSave();
            // MTNControlBase.FindClickRowInTable(terminalAreaDefinitionForm.tblTerminalAreas, $"ID^{terminalAreaId}~Type^{terminalAreaType}");
            terminalAreaDefinitionForm.TblTerminalAreas.FindClickRow([$"ID^{terminalAreaId}~Type^{terminalAreaType}"]);
            MTNControlBase.FindValidateClickEditTableData(terminalAreaDefinitionForm.tblDefinitionData, $"Comment^{terminalAreaComment}");
            MTNControlBase.FindValidateClickEditTableData(terminalAreaDefinitionForm.tblDefinitionData, $"No. Of Lanes^{terminalAreaTypeNoOfLanes}");
        }


        private void DeleteTerminalArea(TerminalAreaDefinitionForm terminalAreaDefinitionForm, string terminalAreaType, 
            string terminalAreaId, string terminalAreaComment, string noLanes = null)
        {
            MTNKeyboard.Press(VirtualKeyShort.F5, 200);
         
           // terminalAreaDefinitionForm.GetTerminalAreasTable();
            // MTNControlBase.FindClickRowInTable(terminalAreaDefinitionForm.tblTerminalAreas,
                // @"ID^" + terminalAreaId + "~Type^" + terminalAreaType, rowHeight: 16);
            terminalAreaDefinitionForm.TblTerminalAreas.FindClickRow(["ID^" + terminalAreaId + "~Type^" + terminalAreaType]);
            
            terminalAreaDefinitionForm.GetDefinitionData();
            MTNControlBase.FindValidateClickEditTableData(terminalAreaDefinitionForm.tblDefinitionData,
                @"Comment^" + terminalAreaComment);

            if (!string.IsNullOrEmpty(noLanes))
            {
                MTNControlBase.FindValidateClickEditTableData(terminalAreaDefinitionForm.tblDefinitionData,
                    @"No. Of Lanes^" + noLanes);
            }

            terminalAreaDefinitionForm.DoDelete();

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Confirm Deletion");
            confirmationFormYesNo.btnYes.DoClick();
        }
        
    }

}
