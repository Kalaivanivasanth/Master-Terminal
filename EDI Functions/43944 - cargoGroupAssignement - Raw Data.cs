using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using System.IO;
using MTNForms.FormObjects;
using MTNWindowDialogs.WindowsDialog;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43944 : MTNBase
    {
        EDIOperationsForm ediOperationsForm;
        EDIRawDataForm ediRawDataForm;

        protected static string ediFile1 = "M_43944_CargoGroupAssignment.xml";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_43944_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {

            //Create Cargo Group Assignment EDI load
            CreateDataFile(ediFile1,
               "<?xml version='1.0' encoding='UTF-8'?><CargoGroupAssignments><CargoGroupAssignment><Action>A</Action><CargoGroupAssignmentId>83736157/2</CargoGroupAssignmentId><SelectableCargoIds><CargoId>001-FA1704270.F-055</CargoId><CargoId>001-FA1704270.F-057</CargoId><CargoId>001-FA1704270.F-058</CargoId><CargoId>001-FA1704270.F-059</CargoId><CargoId>001-FA1704270.F-060</CargoId><CargoId>001-FA1704270.G-061</CargoId><CargoId>001-FA1704270.G-062</CargoId><CargoId>001-FA1704270.G-063</CargoId><CargoId>001-FA1704270.G-064</CargoId><CargoId>001-FA1704270.G-065</CargoId><CargoId>001-FA1704270.G-066</CargoId><CargoId>001-FA1704270.G-067</CargoId><CargoId>001-FA1704270.G-068</CargoId><CargoId>001-FA1704270.G-069</CargoId><CargoId>001-FA1704270.G-070</CargoId><CargoId>001-FA1704280.A-002</CargoId><CargoId>001-FA1704280.A-003</CargoId><CargoId>001-FB1704270.A-009</CargoId><CargoId>001-FB1704270.A-010</CargoId><CargoId>001-FB1704270.B-011</CargoId><CargoId>001-FB1704270.B-012</CargoId><CargoId>001-FB1704270.B-013</CargoId><CargoId>001-FB1704270.B-014</CargoId><CargoId>001-FB1704270.B-015</CargoId><CargoId>001-FB1704270.B-016</CargoId><CargoId>001-FB1704270.B-017</CargoId><CargoId>001-FB1704270.B-018</CargoId><CargoId>001-FB1704270.B-019</CargoId><CargoId>001-FB1704270.B-020</CargoId><CargoId>001-FB1704270.C-021</CargoId><CargoId>001-FB1704270.C-022</CargoId><CargoId>001-FB1704270.C-023</CargoId><CargoId>001-FB1704270.C-024</CargoId><CargoId>001-FB1704270.C-025</CargoId><CargoId>001-FB1704270.C-026</CargoId><CargoId>001-FB1704270.C-027</CargoId><CargoId>001-FB1704270.C-028</CargoId><CargoId>001-FB1704270.C-029</CargoId><CargoId>001-FB1704270.C-030</CargoId><CargoId>001-FB1704270.D-031</CargoId><CargoId>001-FB1704270.D-032</CargoId><CargoId>001-FB1704270.D-033</CargoId><CargoId>001-FB1704270.D-034</CargoId><CargoId>001-FB1704270.D-035</CargoId><CargoId>001-FB1704270.D-036</CargoId><CargoId>001-FB1704270.D-037</CargoId><CargoId>001-FB1704270.D-038</CargoId><CargoId>001-FB1704270.D-039</CargoId><CargoId>001-FB1704270.D-040</CargoId><CargoId>001-FB1704270.E-041</CargoId><CargoId>001-FB1704270.E-042</CargoId><CargoId>001-FB1704270.E-043</CargoId><CargoId>001-FB1704270.E-044</CargoId><CargoId>001-FB1704270.E-045</CargoId><CargoId>001-FB1704270.E-046</CargoId><CargoId>001-FB1704270.E-047</CargoId><CargoId>001-FB1704270.E-048</CargoId><CargoId>001-FB1704270.E-049</CargoId><CargoId>001-FB1704270.E-050</CargoId><CargoId>001-FB1704270.H-080</CargoId><CargoId>001-FB1704270.I-081</CargoId><CargoId>001-FB1704270.I-082</CargoId><CargoId>001-FB1704270.I-083</CargoId></SelectableCargoIds><CargoGroup><CargoGroupId>PB9695799</CargoGroupId><DefaultLocation/><MinWeight>24361</MinWeight><MaxWeight>25500</MaxWeight><Remarks/><CargoType>PBGR</CargoType><Operator>****</Operator></CargoGroup><CargoGroup><CargoGroupId>PB9695800</CargoGroupId><DefaultLocation/><MinWeight>24361</MinWeight><MaxWeight>25500</MaxWeight><Remarks/><CargoType>PBGR</CargoType><Operator>****</Operator></CargoGroup><CargoGroup><CargoGroupId>PB9695801</CargoGroupId><DefaultLocation/><MinWeight>1</MinWeight><MaxWeight>25500</MaxWeight><Remarks/><CargoType>PBGR</CargoType><Operator>****</Operator></CargoGroup></CargoGroupAssignment></CargoGroupAssignments><TerminalCode>T4</TerminalCode>");

            MTNSignon(TestContext);
        }
        

        [TestMethod]
        public void CargoGroupAssignmentRawData()
        {

            MTNInitialize();
            
            // 1. Open EDI Operations
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // 1b. Delete Existing EDI messages relating to this test.
            ediOperationsForm.DeleteEDIMessages(@"Cargo Group Assignment", @"43944", "Loaded");

            // 2. Load Cargo Group Assignement EDI message
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
        
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile1,clickType: ClickType.ContextClick, xOffset:150);
            ediOperationsForm.ContextMenuSelect(@"View Raw Data");

            ediRawDataForm = new EDIRawDataForm("Message");
            ediRawDataForm.btnExportRequest.DoClick();
            Miscellaneous.WaitForSeconds(2);

            var fileName = $"{saveDirectory}TestCase43944CargoGroupAssignment.xml";
            var windowsSaveDialog = new WindowsSaveDialog("Save As");
            windowsSaveDialog.txtFileName.SetValue(fileName);
            windowsSaveDialog.btnSave.DoClick();

            ediRawDataForm.btnCancel.DoClick();

            var fileDetails = File.ReadAllText(fileName);
            Console.WriteLine($"fileDetails: {fileDetails}");
            var expectedMessage = "<?xml version='1.0' encoding='UTF-8'?><CargoGroupAssignments><CargoGroupAssignment><Action>A</Action><CargoGroupAssignmentId>83736157/2</CargoGroupAssignmentId><SelectableCargoIds><CargoId>001-FA1704270.F-055</CargoId><CargoId>001-FA1704270.F-057</CargoId><CargoId>001-FA1704270.F-058</CargoId><CargoId>001-FA1704270.F-059</CargoId><CargoId>001-FA1704270.F-060</CargoId><CargoId>001-FA1704270.G-061</CargoId><CargoId>001-FA1704270.G-062</CargoId><CargoId>001-FA1704270.G-063</CargoId><CargoId>001-FA1704270.G-064</CargoId><CargoId>001-FA1704270.G-065</CargoId><CargoId>001-FA1704270.G-066</CargoId><CargoId>001-FA1704270.G-067</CargoId><CargoId>001-FA1704270.G-068</CargoId><CargoId>001-FA1704270.G-069</CargoId><CargoId>001-FA1704270.G-070</CargoId><CargoId>001-FA1704280.A-002</CargoId><CargoId>001-FA1704280.A-003</CargoId><CargoId>001-FB1704270.A-009</CargoId><CargoId>001-FB1704270.A-010</CargoId><CargoId>001-FB1704270.B-011</CargoId><CargoId>001-FB1704270.B-012</CargoId><CargoId>001-FB1704270.B-013</CargoId><CargoId>001-FB1704270.B-014</CargoId><CargoId>001-FB1704270.B-015</CargoId><CargoId>001-FB1704270.B-016</CargoId><CargoId>001-FB1704270.B-017</CargoId><CargoId>001-FB1704270.B-018</CargoId><CargoId>001-FB1704270.B-019</CargoId><CargoId>001-FB1704270.B-020</CargoId><CargoId>001-FB1704270.C-021</CargoId><CargoId>001-FB1704270.C-022</CargoId><CargoId>001-FB1704270.C-023</CargoId><CargoId>001-FB1704270.C-024</CargoId><CargoId>001-FB1704270.C-025</CargoId><CargoId>001-FB1704270.C-026</CargoId><CargoId>001-FB1704270.C-027</CargoId><CargoId>001-FB1704270.C-028</CargoId><CargoId>001-FB1704270.C-029</CargoId><CargoId>001-FB1704270.C-030</CargoId><CargoId>001-FB1704270.D-031</CargoId><CargoId>001-FB1704270.D-032</CargoId><CargoId>001-FB1704270.D-033</CargoId><CargoId>001-FB1704270.D-034</CargoId><CargoId>001-FB1704270.D-035</CargoId><CargoId>001-FB1704270.D-036</CargoId><CargoId>001-FB1704270.D-037</CargoId><CargoId>001-FB1704270.D-038</CargoId><CargoId>001-FB1704270.D-039</CargoId><CargoId>001-FB1704270.D-040</CargoId><CargoId>001-FB1704270.E-041</CargoId><CargoId>001-FB1704270.E-042</CargoId><CargoId>001-FB1704270.E-043</CargoId><CargoId>001-FB1704270.E-044</CargoId><CargoId>001-FB1704270.E-045</CargoId><CargoId>001-FB1704270.E-046</CargoId><CargoId>001-FB1704270.E-047</CargoId><CargoId>001-FB1704270.E-048</CargoId><CargoId>001-FB1704270.E-049</CargoId><CargoId>001-FB1704270.E-050</CargoId><CargoId>001-FB1704270.H-080</CargoId><CargoId>001-FB1704270.I-081</CargoId><CargoId>001-FB1704270.I-082</CargoId><CargoId>001-FB1704270.I-083</CargoId></SelectableCargoIds><CargoGroup><CargoGroupId>PB9695799</CargoGroupId><DefaultLocation/><MinWeight>24361</MinWeight><MaxWeight>25500</MaxWeight><Remarks/><CargoType>PBGR</CargoType><Operator>****</Operator></CargoGroup><CargoGroup><CargoGroupId>PB9695800</CargoGroupId><DefaultLocation/><MinWeight>24361</MinWeight><MaxWeight>25500</MaxWeight><Remarks/><CargoType>PBGR</CargoType><Operator>****</Operator></CargoGroup><CargoGroup><CargoGroupId>PB9695801</CargoGroupId><DefaultLocation/><MinWeight>1</MinWeight><MaxWeight>25500</MaxWeight><Remarks/><CargoType>PBGR</CargoType><Operator>****</Operator></CargoGroup></CargoGroupAssignment></CargoGroupAssignments><TerminalCode>T4</TerminalCode>";
            
            Assert.IsTrue(fileDetails.Equals(expectedMessage), $"TestCase43944 - Details do NOT match:\r\nExpected: {expectedMessage}\r\nActual: {fileDetails}");

        }

    }

}
