using DataObjects.LogInOutBO;
using FlaUI.Core.WindowsAPI;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.Print_Preview;
using MTNForms.FormObjects.System_Items.Report_Maintenance;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40465 : MTNBase
    {
        private SelectorQueryForm _selectorQueryForm;
        private ReportRunnerForm _reportRunnerForm;
        private PrintPreviewForm _printPreviewForm;
        private ReportMaintenanceForm _reportMaintenanceForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void SaveSelectorQueryAndReport()
        {
            
            MTNInitialize();

            // 1 .create a uniqueID for use in the test
            var uniqueId = GetUniqueId();
            
            // 2. Navigate to Selector Query
            FormObjectBase.MainForm.OpenSelectorFromToolbar();

            // 3. Build the query
            _selectorQueryForm = new SelectorQueryForm();
            _selectorQueryForm.cmbMode.SetValue(@"Tracked Item mode");
            _selectorQueryForm.QueryDetailsProperty();
            
            // criteria 1
            _selectorQueryForm.cmbProperty.SetValue(@"Release Date");
            _selectorQueryForm.QueryDetailsOperation();
            _selectorQueryForm.QueryDetailsArgumentUpper();
            
            _selectorQueryForm.cmbOperation.SetValue(@"Days passed greater than");
            _selectorQueryForm.txtArgument.SetValue(@"5");
            _selectorQueryForm.btnAddLine.DoClick();

            // criteria 2
            _selectorQueryForm.cmbProperty.SetValue(@"Release Time Only", additionalWaitTimeout: 2000);
            _selectorQueryForm.QueryDetailsOperation();
            _selectorQueryForm.QueryDetailsArgumentUpperTime();
            _selectorQueryForm.cmbOperation.SetValue(@"less than");
            _selectorQueryForm.txtArgumentTimeUpper.SetValue(@"16:00");
            _selectorQueryForm.btnAddLine.DoClick();
            _selectorQueryForm.tblQueryDetails.Click();

            // criteria 3
            Keyboard.TypeSimultaneously(VirtualKeyShort.ALT, VirtualKeyShort.HOME);
            _selectorQueryForm.QueryDetailsArgumentCombo();
            _selectorQueryForm.cmbArgument.SetValue(Site.OffSite);

            _selectorQueryForm.btnUpdateLine.DoClick();

            // 4. retrieve the query results and save
            _selectorQueryForm.btnFind.DoClick();
            _selectorQueryForm.DoSave();
            _selectorQueryForm.ShowSaveTable();
            MTNControlBase.SetValueInEditTable(_selectorQueryForm.tblSaveTable, @"Query Name", $"Query{uniqueId}");
            MTNControlBase.SetValueInEditTable(_selectorQueryForm.tblSaveTable, @"Query Description",
                $"Testing {uniqueId}");
            _selectorQueryForm.btnSaveQuery.DoClick();

            // 5. go to report maintenance and create a new report using selector
            FormObjectBase.MainForm.OpenReportMaintenanceFromToolbar();

            _reportMaintenanceForm = new ReportMaintenanceForm();
            _reportMaintenanceForm.DoNew();

            MTNControlBase.SetValueInEditTable(_reportMaintenanceForm.tblReportDetails, "Report Name", $"Report {uniqueId}");
            MTNControlBase.SetValueInEditTable(_reportMaintenanceForm.tblReportDetails, "Report Description", "Report For Test " + uniqueId);
            MTNControlBase.SetValueInEditTable(_reportMaintenanceForm.tblReportDetails, "Report Type", "Selector Query", rowDataType: EditRowDataType.ComboBox, doDownArrow: true, searchSubStringTo: "Selector Query".Length - 1, waitTime: 50);
            MTNControlBase.SetValueInEditTable(_reportMaintenanceForm.tblReportDetails, "JRW Design(s)", "CM_ContainerBasic", rowDataType: EditRowDataType.TableWithCheckBox);
            MTNControlBase.SetValueInEditTable(_reportMaintenanceForm.tblReportDetails, "Selector Query",
                $"Query{uniqueId}", rowDataType: EditRowDataType.ComboBox, doDownArrow: true, searchSubStringTo: $"Query{uniqueId}".Length - 1);
            MTNControlBase.SetValueInEditTable(_reportMaintenanceForm.tblReportDetails, "Report Availability", "JMT", rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(_reportMaintenanceForm.tblReportDetails, "Web Report Processing", "Foreground", rowDataType: EditRowDataType.ComboBox);
           
            _reportMaintenanceForm.DoSave();


            // 6. print the report
            FormObjectBase.MainForm.OpenReportRunnerFromToolbar();

            _reportRunnerForm = new ReportRunnerForm($"Report Runner {terminalId}");

            _reportRunnerForm.txtReportName.SetValue($"Report {uniqueId}");
            _reportRunnerForm.BtnFind.DoClick();
            _reportRunnerForm.PrintReport(_reportRunnerForm.tblOutputOptions, _reportRunnerForm.btnPrint);

            _printPreviewForm = new PrintPreviewForm($"Print Preview - Report {uniqueId}");
            Miscellaneous.CaptureElementAsImage(TestContext, _printPreviewForm.rptPane, $"Report {uniqueId}.png");
            _printPreviewForm.btnCancel.DoClick();

        }

    }

}
