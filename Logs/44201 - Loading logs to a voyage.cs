using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Logs;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNUtilityClasses.Classes;
using System;
using System.Drawing;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Logs
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44201 : MTNBase
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
            LogInto<MTNLogInOutBO>();

            CallJadeScriptToRun(TestContext, @"resetData_44201");
        }
       

        [TestMethod]
        public void LoadingLogsToAVoyage()
        {
            MTNInitialize();

            //FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry");
            FormObjectBase.MainForm.OpenVoyageEnquiryFromToolbar();
            VoyageEnquiryForm voyageEnquiryForm = new VoyageEnquiryForm();

            // Step 2
            voyageEnquiryForm.DeleteVoyageByVoyageCode(@"VOY44201");

            // Step 1
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Gate In/Out", forceReset: true);
            GateInOutForm gateInOutForm = new GateInOutForm(@"Road Entry " + terminalId);

            // Step 2
            gateInOutForm.DoReceival();

            // Step 3
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblVehicleDetails, @"Vehicle ID", @"44201");
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblVehicleDetails, @"Carrier", @"AAUTO	American Auto Tpt",
                EditRowDataType.ComboBoxEdit, doDownArrow: true);

            Console.WriteLine($"tblCargoIn: {gateInOutForm.tblCargoIn}");
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblCargoIn, "Cargo Type",  CargoType.LogDocket, EditRowDataType.ComboBoxEdit);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblCargoIn, @"Bill Of Lading", @"BOL44201");
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblCargoIn, @"Operator", Operator.BCL,
                EditRowDataType.ComboBoxEdit, doDownArrow: true);

            //Keyboard.Press(VirtualKeyShort.TAB);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            gateInOutForm.ScrollSpeciesTableToTop();
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblDetailsIn, @"Species", @"S	Species 1", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblDetailsIn, @"Grade", @"GRADE1	Grade 1", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblDetailsIn, @"Log Type", @"TY	Type 1", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblDetailsIn, @"Treatment", @"TR	Treatment 1", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblDetailsIn, @"Length/Random", @"17.0	17.0", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblDetailsIn, @"No. Items", @"10");
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblDetailsIn, @"Scaler Initials", @"ADMIN", EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblDetailsIn, @"Yard Location", @"ROW1		0	   0.000	    0.00",
                EditRowDataType.ComboBoxEdit, doDownArrow: true, searchSubStringTo: 4, downArrowSearchType: SearchType.StartsWith);
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblDetailsIn, @"Gross Weight (Tonnes)", @"50");
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblDetailsIn, @"Tare Weight (Tonnes)", @"10");
            MTNControlBase.SetValueInEditTable(gateInOutForm.tblDetailsIn, @"Net Weight (Tonnes)", @"40");

            // Step 4 
            gateInOutForm.DoSave();

            WarningErrorForm.CompleteWarningErrorForm("Warnings for Gate In/Out", new []
            {
                "Code :75140. There is no record of vehicle 44201. It will be added as a new vehicle.",
                "Code :75140. There is no record of vehicle 44201. It will be added as a new vehicle."
            });

            // Step 5
            FormObjectBase.NavigationMenuSelection(@"Docket Reconciliation", forceReset: false);

            // Step 6
            DocketReconciliationForm docketReconciliationForm = new DocketReconciliationForm(@"Docket Reconciliation " + terminalId);
            // MTNControlBase.FindClickRowInTable(docketReconciliationForm.tblUnreconciledDockets,
                // @"Bill Of Lading^BOL44201~Operator^BCL~Piece Count^10~Match Found^No");
            docketReconciliationForm.TblUnreconciledDockets.FindClickRow(["Bill Of Lading^BOL44201~Operator^BCL~Piece Count^10~Match Found^No"]);

            // Step 7
            docketReconciliationForm.DoAddReceipt();

            // Step 8 - 10
            LogScalingDataForm logScalingDataForm = new LogScalingDataForm(@"Log Scaling Data TT1");

            MTNControlBase.SetValueInEditTable(logScalingDataForm.TblHeaderDetails, @"Bill Of Lading", @"BOL44201");
            MTNControlBase.SetValueInEditTable(logScalingDataForm.TblHeaderDetails, @"Operator", Operator.BCL,
                EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(logScalingDataForm.TblHeaderDetails, @"No. Items", @"10");

            MTNControlBase.SetValueInEditTable(logScalingDataForm.TblDetailsEntry, @"Log ID", @"LOG0");
            MTNControlBase.SetValueInEditTable(logScalingDataForm.TblDetailsEntry, @"Min SED", @"8");
            MTNControlBase.SetValueInEditTable(logScalingDataForm.TblDetailsEntry, @"Maximum SED", @"8");
            MTNControlBase.SetValueInEditTable(logScalingDataForm.TblDetailsEntry, @"Condition", @"LC01", EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(logScalingDataForm.TblDetailsEntry, @"Scaler Initials", @"ADMIN", EditRowDataType.ComboBoxEdit);

            while (!logScalingDataForm.TxtRemaining.GetText().Equals("0"))
            {
                logScalingDataForm.BtnAdd.DoClick();
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(200));
            }
            logScalingDataForm.CheckAllLogsDetailsEntered();
            logScalingDataForm.BtnSave.DoClick();

            // Step 11 - 13
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");

        
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type",  CargoType.LogDocket, EditRowDataType.ComboBoxEdit, doDownArrow: true, searchSubStringTo: 5, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Bill Of Lading", "BOL44201");
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.SearchToolbar,
                (int)CargoEnquiryForm.Toolbar.SearcherToolbar.Search, "Search");

            cargoEnquiryForm.tblData2.FindClickRow(new[]
            {
                "Bill of Lading^BOL44201~Operator^BCL~Species^S~Grade^GRADE1~Type^TY~Treatment^TR~Length cm^17.0~Number of Items^10"
            });

            cargoEnquiryForm.CargoEnquiryLogDataTab();
           
            cargoEnquiryForm.TblLogData.FindClickRow(new[]
            {
                "Ticket^LOG0~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes",
                "Ticket^LOG1~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes", 
                "Ticket^LOG2~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes", 
                "Ticket^LOG3~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes", 
                "Ticket^LOG4~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes", 
                "Ticket^LOG5~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes", 
                "Ticket^LOG6~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes", 
                "Ticket^LOG7~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes", 
                "Ticket^LOG8~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes", 
                "Ticket^LOG9~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes", 
            }, ClickType.None);
            cargoEnquiryForm.CloseForm();
            
            // Step 14 - 17
            FormObjectBase.NavigationMenuSelection(@"Log Functions|Log Voyage Functions|Log Voyage Operations", forceReset: true);
            LogVoyageOperationsForm logVoyageOperationsForm = new LogVoyageOperationsForm(@"Log Voyage Operations " + terminalId);
            logVoyageOperationsForm.btnVoyageAdminNew.DoClick();

            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblVoyageAdminVoyageDetails, @"Voyage Code", @"VOY44201");
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblVoyageAdminVoyageDetails, @"Vessel Name", @"MSCK");
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblVoyageAdminVoyageDetails, @"Hatches", @"2");

            logVoyageOperationsForm.unassignedAssigned.MakeSureInCorrectList(logVoyageOperationsForm.lstAssigned, 
                new [] { "Test Terminal 1 - Non Cash" });
            logVoyageOperationsForm.btnVoyageAdminSave.DoClick();   

            // Step 18 - 19
            logVoyageOperationsForm.GetVoyageTabDetails();
            logVoyageOperationsForm.btnVoyageEdit.DoClick();
            
            logVoyageOperationsForm.tblVoyageTerminalPortDetails.Focus();
            Point point = new Point(logVoyageOperationsForm.tblVoyageTerminalPortDetails.BoundingRectangle.X + 150,
                logVoyageOperationsForm.tblVoyageTerminalPortDetails.BoundingRectangle.Y + 20);
            Mouse.Click(point);

            // Arrival Date
            var date = DateTime.Today;
            SetFieldInTable(date, 2);

            // Sail Date
            date = DateTime.Today.AddDays(1);
            SetFieldInTable(date, 5);
            
            logVoyageOperationsForm.btnVoyageSave.DoClick();

            // Step 20 - 21
            logVoyageOperationsForm.GetMainSuppliersTabDetails();

            logVoyageOperationsForm.btnSupplierNew.DoClick();
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblSupplierEntry, @"Operator",
                Operator.BCL, EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblSupplierEntry, @"Terminal/Port", @"Test Terminal 1 - Non Cash", 
                EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblSupplierEntry, @"Agent", @"BW000	BARW", 
                EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblSupplierEntry, @"Stevedore 1", @"STEVEDORE1	Stevedore for 44201", 
                EditRowDataType.ComboBoxEdit, doDownArrow: true);
            
            logVoyageOperationsForm.btnSupplierSave.DoClick();
            // MTNControlBase.FindClickRowInTable(logVoyageOperationsForm.tblSupplierDetails,
                // @"Operator^BCL~Terminal/Port^Test Terminal 1 - Non Cash~Agent^BW000~Stevedore 1^STEVEDORE1");
            logVoyageOperationsForm.TblSupplierDetails.FindClickRow(["Operator^BCL~Terminal/Port^Test Terminal 1 - Non Cash~Agent^BW000~Stevedore 1^STEVEDORE1"]);
            
            // Step 22 - 23
            logVoyageOperationsForm.GetPortsTabDetails();

            MTNControlBase.FindClickRowInList(logVoyageOperationsForm.lstPorts, @"Test Terminal 1 - Non Cash");
            point = new Point(logVoyageOperationsForm.lstPorts.BoundingRectangle.X + 50,
                logVoyageOperationsForm.lstPorts.BoundingRectangle.Y + 50);
            Mouse.Click(point);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            logVoyageOperationsForm.CheckGlobalVoyagesPorts("VOY44201  MSCK|Test Terminal 1 - Non Cash");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            // MTNControlBase.FindClickRowInTable(logVoyageOperationsForm.tblSupplierDetails,
                // @"Operator^BCL~Terminal/Port^Test Terminal 1 - Non Cash~Agent^BW000~Stevedore 1^STEVEDORE1");
            logVoyageOperationsForm.TblSupplierDetails.FindClickRow(["Operator^BCL~Terminal/Port^Test Terminal 1 - Non Cash~Agent^BW000~Stevedore 1^STEVEDORE1"]);
            //MTNControlBase.FindTabOnForm(logVoyageOperationsForm.tabVoyageAdmin, @"Details");
            logVoyageOperationsForm.GetDetailsTabDetails();

            logVoyageOperationsForm.btnDetailsNew.DoClick();
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblDetails, @"Lot No.", @"44201");
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblDetails, @"Operator", Operator.BCL, EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblDetails, @"Species", @"S	Species 1", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblDetails, @"Grade", @"GRADE1	Grade 1", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblDetails, @"Type", @"TY	Type 1", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblDetails, @"Treatment", @"TR	Treatment 1", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblDetails, @"Length", @"17.0	17.0", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(logVoyageOperationsForm.tblDetails, @"Required Volume", @"20");
            logVoyageOperationsForm.btnDetailsSave.DoClick();
            // MTNControlBase.FindClickRowInTable(logVoyageOperationsForm.tblLots,
                // @"Lot No.^44201~Operator^BCL~Specifications^S:GRADE1:TY:TR:17.0~Required Volume^20");
            logVoyageOperationsForm.TblLots.FindClickRow(["Lot No.^44201~Operator^BCL~Specifications^S:GRADE1:TY:TR:17.0~Required Volume^20"]);

            // Step 25 - 29
            logVoyageOperationsForm.GetAllocateRowsTabDetails();

            logVoyageOperationsForm.btnAllocateRowsSearch.DoClick();
            // MTNControlBase.FindClickRowInTable(logVoyageOperationsForm.tblAllocateRowsEntry,
                // @"Row^ROW1^Operator^BCL~Specifications^S:GRADE1:TY:TR:17.0~Mean SED^7.99~Total Volume^3.570~Avail Volume^3.570");
            logVoyageOperationsForm.TblAllocateRowsEntry.FindClickRow(["Row^ROW1^Operator^BCL~Specifications^S:GRADE1:TY:TR:17.0~Mean SED^7.99~Total Volume^3.570~Avail Volume^3.570"]);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Keyboard.Type(@"3.570");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            logVoyageOperationsForm.btnAllocateRowsSave.DoClick();
            // MTNControlBase.FindClickRowInTable(logVoyageOperationsForm.tblAllocateRowsDetails,
                // @"Row^ROW1^Operator^BCL~Specifications^S:GRADE1:TY:TR:17.0~Mean SED^7.99~Total Volume^3.570~Avail Volume^0.000~Alloc Volume^3.570");
            logVoyageOperationsForm.TblAllocateRowsDetails.FindClickRow(["Row^ROW1^Operator^BCL~Specifications^S:GRADE1:TY:TR:17.0~Mean SED^7.99~Total Volume^3.570~Avail Volume^0.000~Alloc Volume^3.570"]);

            // Step 30 - 34
            logVoyageOperationsForm.GetShipsideScanTabDetails();


            var testfileTxt = @"44201_TestFile.txt";
            CreateDataFile(testfileTxt, @"LOG0,44201,H,1,20190409,15:04,0.357,1,TT1,VOY44201,BCL,S,GRADE1,17.0,TY,TR
LOG1,44201,H,1,20190409,15:04,0.357,1,TT1,VOY44201,BCL,S,GRADE1,17.0,TY,TR
LOG2,44201,H,1,20190409,15:04,0.357,1,TT1,VOY44201,BCL,S,GRADE1,17.0,TY,TR");
            logVoyageOperationsForm.txtShipsideScanLogsLoadedOutFile.SetValue(dataDirectory + testfileTxt);

            logVoyageOperationsForm.btnShipsideScanImport.DoClick();
            // MTNControlBase.FindClickRowInTable(logVoyageOperationsForm.tblLoadOutFiles,
                // @"File Name^" + testfileTxt + "~Status^Dockets Loaded~Imported By^USERDWAT");
            logVoyageOperationsForm.TblLoadOutFiles.FindClickRow(["File Name^" + testfileTxt + "~Status^Dockets Loaded~Imported By^USERDWAT"]);

            // Step 35 - 38
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm("Cargo Enquiry TT1");
   
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.LogDocket, 
                EditRowDataType.ComboBoxEdit, doDownArrow: true, searchSubStringTo: 5, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Terminal Area", "LOGS",
                EditRowDataType.ComboBoxEdit, doDownArrow: true, searchSubStringTo: 2, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Yard Location", "ROW1", 
                EditRowDataType.ComboBoxEdit, doDownArrow: true, downArrowSearchType: SearchType.StartsWith, fromCargoEnquiry: true, searchSubStringTo: "ROW1".Length - 1);
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.SearchToolbar,
                (int)CargoEnquiryForm.Toolbar.SearcherToolbar.Search, "Search");

            cargoEnquiryForm.tblData2.FindClickRow(new[]
            {
                "Bill of Lading^BOL44201~Operator^BCL~Species^S~Grade^GRADE1~Type^TY~Treatment^TR~Length cm^17.0~Number of Items^10"
            });

            
            cargoEnquiryForm.CargoEnquiryLogDataTab();

            cargoEnquiryForm.TblLogData.FindClickRow(new[]
            {
                "Ticket^LOG0~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^VOY44201/44201/1/Hold~Scaled^Yes",
                "Ticket^LOG1~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^VOY44201/44201/1/Hold~Scaled^Yes",
                "Ticket^LOG2~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^VOY44201/44201/1/Hold~Scaled^Yes",
                "Ticket^LOG3~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes",
                "Ticket^LOG4~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes",
                "Ticket^LOG5~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes",
                "Ticket^LOG6~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes",
                "Ticket^LOG7~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes",
                "Ticket^LOG8~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes",
                "Ticket^LOG9~Min^8.00~Max^8.00~Mean^8.00~Scaler^ADMIN~Location^ROW1~Scaled^Yes",
            }, ClickType.None);

        }

        private static void SetFieldInTable(DateTime date, int xTimes)
        {
            int times = 0;
            do
            {
                Keyboard.Press(VirtualKeyShort.TAB);
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(400));
                times++;
            } while (times <= xTimes);

            Keyboard.Type(date.ToString(@"ddMMyyyy"));
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(400));
        }
    }

}
