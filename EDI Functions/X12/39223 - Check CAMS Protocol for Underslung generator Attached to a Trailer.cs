using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Gate_Functions.Vehicle;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNUtilityClasses.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions.X12
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39923 : MTNBase
    {

        private BackgroundApplicationForm _backgroundApplicationForm;
        private EDICAMProtocolForm _ediCAMProtocolForm;

        private DateTime _startTS;

        private bool _matchFound = false;

        private const string TestCaseNumber = @"39923";


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}


        [TestCleanup]
        public new void TestCleanup()
        {

            // Stop the CAMS(Server)
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
                        _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^",
                false);

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();

            _startTS = DateTime.Now;

            // Setup data
            searchFor = @"_" + TestCaseNumber + "_";
            loadFileDeleteStartTime = DateTime.Now; // REMOVE AT YOUR OWN PERIL

            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_" + TestCaseNumber);

            SetupAndLoadInitializeData(TestContext);

        }


    [TestMethod]
        public void CheckCAMSProtocolForUnderslungGeneratorAttachedToTrailer()
        {
            MTNInitialize();

            // Step 5 - 9
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"39223");

            /*roadGateForm.cmbCarrier.SetValue("American Auto Tpt");
            roadGateForm.txtRegistration.SetValue("39923");
            roadGateForm.cmbGate.SetValue("GATE");*/
            roadGateForm.SetRegoCarrierGate("39923");

            roadGateForm.GetTrailerFields();
            roadGateForm.txtTrailerID.SetValue("TRAILER39923A");
            roadGateForm.cmbTrailerType.SetValue("TRAILER39771", doDownArrow: true, searchSubStringTo: "TRAILER39771".Length - 1);
            roadGateForm.cmbTrailerOperator.SetValue("ABOC", doDownArrow: true);
            roadGateForm.cmbImex.SetValue("Storage", doDownArrow: true, searchSubStringTo: "Storage".Length - 1);
            roadGateForm.txtReleaseTrailerID.SetValue("TRAILER39923B");
            roadGateForm.btnReceiveEmpty.DoClick();
            RoadGateDetailsReceiveForm roadGateDetailsForm =
                new RoadGateDetailsReceiveForm(formTitle: @"Receive Empty Container TT1");
            //roadGateDetailsForm.ShowContainerDetails();

            // Step 8
            roadGateDetailsForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            roadGateDetailsForm.TxtCargoId.SetValue("JLG39923A01");
            roadGateDetailsForm.MtTotalWeight.SetValueAndType("2000");
            roadGateDetailsForm.CmbImex.SetValue(IMEX.Storage, additionalWaitTimeout: 2000, doDownArrow: true);
            //Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            roadGateDetailsForm.CmbOperator.SetValue(Operator.ABOC, doDownArrow: true);

            // Step 9
            roadGateDetailsForm.BtnSave.DoClick();
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");

            string[] msgToCheck =
            {
                @"Code :75016. The Container Id (JLG39923A01) failed the validity checks and may be incorrect."
            };
            warningErrorForm.CheckWarningsErrorsExist(msgToCheck);
            warningErrorForm.btnSave.DoClick();

            roadGateForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Chassis~Detail^TRAILER39923A; ABOC; Trailer/Chassis");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Release Chassis~Detail^TRAILER39923B; ABOC; Trailer/Chassis");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Empty~Detail^JLG39923A01; ABOC; 2200");

            // Step 10
            var visitNumber = roadGateForm.txtVisitNumber.GetText(); 


            // Step 11
            roadGateForm.btnReleaseEmpty.DoClick();

            RoadGateDetailsReleaseForm roadGateDetailsReleaseForm = new RoadGateDetailsReleaseForm();
            roadGateDetailsReleaseForm.TxtCargoId.SetValue("JLG39923A02");
            roadGateDetailsReleaseForm.TxtDeliveryReleaseNo.SetValue("39923");
            roadGateDetailsReleaseForm.BtnSave.DoClick();

            // Step 12 - 14
            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Gate In/Out TT1");

            msgToCheck = new string[]
            {
                @"Code :79688. Cargo JLG39923A02's availability status of Unavailable for Release does not match the request's availability status of Available for release"
            };
            warningErrorForm.CheckWarningsErrorsExist(msgToCheck);
            warningErrorForm.btnSave.DoClick();

            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Receive Chassis~Detail^TRAILER39923A; ABOC; Trailer/Chassis");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems,
                @"Type^Release Chassis~Detail^TRAILER39923B; ABOC; Trailer/Chassis");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Receive Empty~Detail^JLG39923A01; ABOC; 2200");
            MTNControlBase.FindClickRowInTable(roadGateForm.tblGateItems, @"Type^Release Empty~Detail^JLG39923A02; ABOC; 4200");

            roadGateForm.SetFocusToForm();
            roadGateForm.btnSave.DoClick();

            // Step 14 - 19
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Road Operations", forceReset: true);

            roadOperationsForm = new RoadOperationsForm(@"Road Operations TT1");
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^39923~Cargo ID^TRAILER39923A",
                ClickType.Click, rowHeight: 16);
            //roadOperationsForm.btnMoveIt.DoClick();
            //roadOperationsForm.DoToolbarClick(roadOperationsForm.MainToolbar, (int)RoadOperationsForm.Toolbar.MainToolbar.MoveIt, "Move It");
            roadOperationsForm.DoMoveIt();
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^39923~Cargo ID^TRAILER39923B",
                ClickType.Click, rowHeight: 16);
            //roadOperationsForm.btnMoveIt.DoClick();
            //roadOperationsForm.DoToolbarClick(roadOperationsForm.MainToolbar, (int)RoadOperationsForm.Toolbar.MainToolbar.MoveIt, "Move It");
            roadOperationsForm.DoMoveIt();
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^39923~Cargo ID^JLG39923A02",
                ClickType.Click, rowHeight: 16);
            //roadOperationsForm.btnMoveIt.DoClick();
            //roadOperationsForm.DoToolbarClick(roadOperationsForm.MainToolbar, (int)RoadOperationsForm.Toolbar.MainToolbar.MoveIt, "Move It");
            roadOperationsForm.DoMoveIt();
            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, @"Vehicle Id^39923", ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect(@"Process Road Exit");
            roadOperationsForm.CloseForm();

            // Step 20 - 21
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^");
       
            // Step 22 - 24
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Vehicle Visit Enquiry", forceReset: true);
            VehicleVisitForm vehicleVisitForm = new VehicleVisitForm();

            MTNControlBase.SetValueInEditTable(vehicleVisitForm.tblSearchCriteria, @"Date From", _startTS.ToString(@"ddMMyyyy"));
            MTNControlBase.SetValueInEditTable(vehicleVisitForm.tblSearchCriteria, @"Reg Number", @"39923");
            MTNControlBase.SetValueInEditTable(vehicleVisitForm.tblSearchCriteria, @"Visit Number", visitNumber);
            //vehicleVisitForm.btnSearch.DoClick();
            vehicleVisitForm.DoToolbarClick(vehicleVisitForm.SearchToolbar, (int)VehicleVisitForm.Toolbar.SearcherToolbar.Search, "Search");

            MTNControlBase.FindClickRowInTable(vehicleVisitForm.tblVisits, @"Vehicle^39923~Cargo In^1~Cargo Out^2~Carrier Code^AAUTO");

            DateTime vvTimeOut = Convert.ToDateTime(MTNControlBase.GetValueInEditTable(vehicleVisitForm.tblDetails,
                @"Time Out"));
            vvTimeOut = vvTimeOut.AddMinutes(1);
           
            // Step 25
            FormObjectBase.NavigationMenuSelection(@"EDI Functions | EDI CAMS Protocols", forceReset: true);
            _ediCAMProtocolForm = new EDICAMProtocolForm();
            
            MTNControlBase.FindClickRowInTable(_ediCAMProtocolForm.tblProtocols,
                @"Protocol Description^X12 322 - Chassis~Name^Test - 39923", xOffset: 40);
            _ediCAMProtocolForm.GetDetailsTabAndDetails();
            
            _ediCAMProtocolForm.GetScheduledTimesTabAndDetails(findTab: true);
            _ediCAMProtocolForm.txtStartDate.SetValue(_startTS.ToString(@"ddMMyyyy"),
                additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtStartTime.SetValue( _startTS.ToString(@"HHmm"),
                additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtEndDate.SetValue(vvTimeOut.ToString(@"ddMMyyyy"),
                additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtEndTime.SetValue(vvTimeOut.ToString(@"HHmm"),
                additionalWaitTimeout: 100);
            _ediCAMProtocolForm.btnRunAdHoc.DoClick();

            // Adhoc Protocol run popup 1
            ConfirmationFormYesNo _confirmationFormYesNo = new ConfirmationFormYesNo(@"Adhoc Protocol Run");
            _confirmationFormYesNo.CheckMessageMatch(
                @"Are you sure you want to process the CAM Protocol X12 322 EDI Messages ?");
            _confirmationFormYesNo.btnYes.DoClick();

            // Adhoc Protocol run popup 2
            ConfirmationFormOK _confirmationFormOk = new ConfirmationFormOK(@"Adhoc Protocol Run",
                automationIdMessage: @"3",
                automationIdOK: @"4");
            _confirmationFormOk.btnOK.DoClick();
           
            _ediCAMProtocolForm.SetFocusToForm();
            _ediCAMProtocolForm.GetProtocolTaskHistoryTabAndDetails(@"Protocol Task History [1]");

            MTNControlBase.FindClickRowInTable(_ediCAMProtocolForm.tblProtocolTaskHistory,
                @"Task Status^Complete", ClickType.DoubleClick);

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Warning");
            _confirmationFormYesNo.btnYes.DoClick();
            

            EDIProtocolTaskHistoryForm ediProtocolTaskHistoryForm = new EDIProtocolTaskHistoryForm(TestContext, @"CNWCHCS101");
            ediProtocolTaskHistoryForm.SaveMessageAsFile(dataDirectory + @"39223_CAMProtocolTaskHistory.txt");
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(10));
           
            List<string> fileLines =
                ediProtocolTaskHistoryForm.ReadMessageDetailsFromFile(
                    dataDirectory + @"39223_CAMProtocolTaskHistory.txt");
            //ediProtocolTaskHistoryForm.CloseForm();

            string[] linesToCheck =
            {
                @"N7*JLG*39923A0*907*G*0******CC*MSC***02000*A*K*1*HH***2200*MSC*~",
                @"W2*JLG*39923A0**CC*E******TRAILER*39923A*1***~",
                @"NA***GEN*39923*H~",
                @"N7*JLG*39923A0*4500*G*1500******CC*MSC***04000*A*K*2*HH***4200*MSC*~",
                @"W2*JLG*39923A0**CC*E******TRAILER*39923B*2***~"
            };

            string linesNotFound = null;
         
            foreach (string line in linesToCheck)
            {
                _matchFound = false;

                for (int index = 0; index < fileLines.Count; index++)
                {
                    Trace.TraceInformation(@"index: {0}", index);
                    Trace.TraceInformation(line);
                    Trace.TraceInformation(fileLines[index].Trim());
                    if (line.Equals(fileLines[index].Trim()))
                    {
                        _matchFound = true;
                        break;
                    }
                }

                if (!_matchFound)
                {
                    linesNotFound += Environment.NewLine + line;
                }
            }

            Assert.IsTrue(string.IsNullOrEmpty(linesNotFound), 
                $"TestCase39223 - The following lines where not found in the file:{linesNotFound}");

            // Step 35 - 36 - Done in TestCleanup

            // Step 37 - 38
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG39923A01 ");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoToolbarClick(cargoEnquiryForm.SearchToolbar, (int)CargoEnquiryForm.Toolbar.SearcherToolbar.Search, "Search");
            //MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"Location ID^TRAILER39923A 1~ID^JLG39923A01~RT^Q", ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new string[] { @"Location ID^TRAILER39923A 1~ID^JLG39923A01~RT^Q" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect( @"Cargo|Unqueue JLG39923A01");

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(200));

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"Location ID^TRAILER39923A 1~ID^JLG39923A01~RT^");

        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0'?> <JMTInternalCargoOnSite>\n<AllCargoOnSite>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n	<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>2200</isoType>\n		<dischargePort>NZAKL</dischargePort>\n		<id>JLG39923A01</id>\n		<locationId>MKBS01</locationId>\n		<messageMode>D</messageMode>\n		<imexStatus>Storage</imexStatus>\n		<operatorCode>ABOC</operatorCode>\n		<weight>2000</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	<CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>Trailer/Chassis</cargoTypeDescr>\n		<dischargePort>NZAKL</dischargePort>\n		<id>TRAILER39923A</id>\n		<trailerType>TRAILER39771</trailerType>\n		<locationId>MKBS01</locationId>\n		<messageMode>D</messageMode>\n		<imexStatus>Storage</imexStatus>\n		<operatorCode>ABOC</operatorCode>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	</AllCargoOnSite>\n </JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0'?> <JMTInternalCargoOnSite>\n<AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n	<operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>ISO Container</cargoTypeDescr>\n		<isoType>4200</isoType>\n		<dischargePort>NZAKL</dischargePort>\n		<id>JLG39923A02</id>\n		<locationId>MKBS01</locationId>\n		<messageMode>A</messageMode>\n		<imexStatus>Export</imexStatus>\n		<operatorCode>ABOC</operatorCode>\n		<voyageCode>MSCK000002</voyageCode>\n		<weight>4500</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	<CargoOnSite Terminal='TT1'>\n		<site>1</site>\n		<cargoTypeDescr>Trailer/Chassis</cargoTypeDescr>\n		<dischargePort>NZAKL</dischargePort>\n		<id>TRAILER39923B</id>\n		<trailerType>TRAILER39771</trailerType>\n		<locationId>MKBS01</locationId>\n		<messageMode>A</messageMode>\n		<imexStatus>Storage</imexStatus>\n		<operatorCode>ABOC</operatorCode>\n		<voyageCode>MSCK000002</voyageCode>\n		<weight>4500</weight>\n		<transportMode>Road</transportMode>\n	</CargoOnSite>\n	</AllCargoOnSite>\n </JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }

}
