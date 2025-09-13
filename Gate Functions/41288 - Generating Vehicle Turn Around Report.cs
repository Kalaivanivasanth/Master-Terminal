using DataObjects.LogInOutBO;
using FlaUI.Core.Definitions;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Gate_Functions.Vehicle;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Print_Preview;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;
using MTNWindowDialogs.WindowsDialog;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41288 : MTNBase
    {
        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        ConfirmationFormOKwithText _confirmationFormOKWithText;
        VehicleVisitForm _vehicleVisitForm;
        ReportRunnerForm _reportRunnerForm;
        PrintPreviewForm _printPreviewForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        //void MTNInitialize() => MTNSignon(TestContext);
        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void GeneratingVehicleTurnaroundReport()
        {
            MTNInitialize();

            string uniqueId = GetUniqueId();

            // 1. Open road gate form and enter vehicle visit details
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"41288");
            roadGateForm.SetRegoCarrierGate("41288");
            roadGateForm.btnReceiveFull.DoClick();


            // 2. add details In road gate details form
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(formTitle: @"Receive Full Container TT1");
            _roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG41288A01");
            _roadGateDetailsReceiveForm.CmbCommodity.SetValue(Commodity.GEN, doDownArrow: true);
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("15000");
            _roadGateDetailsReceiveForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbDischargePort.SetValue(Port.LYTNZ, doDownArrow: true);
            _roadGateDetailsReceiveForm.BtnSave.DoClick();
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            // 3. in roadgate get the visit number for later and save
            var visitNumber = roadGateForm.txtVisitNumber.GetText();
            roadGateForm.btnSave.DoClick();

            // 4. open road ops
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);

            // 5. in road ops, right click and turn around, accept warnings and enter text in reason form
            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
             // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date .FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^41288", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new [] { "Vehicle Id^41288" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Turn Around as Release");
            
            _confirmationFormOKWithText = new ConfirmationFormOKwithText(@"Enter the Turn Around Reason value:", controlType: ControlType.Pane);
            _confirmationFormOKWithText.txtInput.SetValue(@"Test 41288");
            _confirmationFormOKWithText.btnOK.DoClick();
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Turn Around as Release TT1");
            warningErrorForm.btnSave.DoClick();
            // Friday, 4 April 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^41288", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.TblYard2.FindClickRow(new [] { "Vehicle Id^41288" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Vehicle Visit Enquiry", forceReset: true);

            // 7. find the vehicle visit and ensure no items where received or released.
            _vehicleVisitForm = new VehicleVisitForm(@"Vehicle Visit Enquiry TT1");
            MTNControlBase.SetValueInEditTable(_vehicleVisitForm.tblSearchCriteria, @"Visit Number",visitNumber);
            _vehicleVisitForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_vehicleVisitForm.tblVisits, @"Vehicle^41288~Cargo In^0~Cargo Out^0~Visit Number^" + visitNumber, rowHeight: 16);
            _vehicleVisitForm.TblVisits.FindClickRow([$"Vehicle^41288~Cargo In^0~Cargo Out^0~Visit Number^{visitNumber}"]);     
            _vehicleVisitForm.CloseForm();

            // 8. open report runner and run user defined report
            FormObjectBase.NavigationMenuSelection(@"General Functions|Report Runner", forceReset: true);
            _reportRunnerForm = new ReportRunnerForm(@"Report Runner TT1");

            _reportRunnerForm.txtReportName.SetValue(@"Test41288");
            //_reportRunnerForm.btnFind.DoClick();
            _reportRunnerForm.BtnFind.DoClick();
            _reportRunnerForm.ShowReportCriteria();

            MTNControlBase.SetValueInEditTable(_reportRunnerForm.tblReportCriteria, @"Visit Number",visitNumber, 
                rowDataType: EditRowDataType.ComboBox, doDownArrow: true, searchSubStringTo: visitNumber.Length - 1);
            MTNControlBase.SetValueInEditTable(_reportRunnerForm.tblOutputOptions,"Print Preview",@"1",rowDataType: EditRowDataType.CheckBox);
            _reportRunnerForm.btnPrint.DoClick();
            _printPreviewForm = new PrintPreviewForm(@"Print Preview - Test41288");
            
            _printPreviewForm.DoPrintToPrinter(needToDoPrinterSetup: true);

            var fileName = $"{saveDirectory}_41288_{uniqueId}_PrintPreview.pdf";
            Miscellaneous.DeleteFile(fileName);

            // Save the print output
            /*WindowsSaveDialog windowsSaveDialog = new WindowsSaveDialog("Save Print Output As");
            windowsSaveDialog.txtFileName.SetValue(fileName);
            windowsSaveDialog.btnSave.DoClick();*/
            WindowsSaveDialog.DoWindowsSaveDialog(WindowsSaveDialog.FormTitleSavePrintOutAs, fileName);

            MTNPDF.GetTextFromDocument(fileName);

            string []detailsToFind =
            {
                "VehicleVisitAnalysis", //sometimes the word Vehicle doesn't print out correct on the report
                "41288"
            };
            MTNPDF.FindText(detailsToFind);

        }

    }

}
