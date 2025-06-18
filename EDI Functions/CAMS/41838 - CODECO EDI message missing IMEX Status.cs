using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions.CAMS
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41838 : MTNBase
    {
        private BackgroundApplicationForm _backgroundApplicationForm;
        private EDICAMProtocolForm _ediCAMProtocolForm;

        private DateTime _startTS;
        private bool _matchFound;

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
            _backgroundApplicationForm?.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^",
                false);

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();
            SetupAndLoadInitializeData(TestContext);
        }


        [TestMethod]
        public void CODECOEDIMessageMissingIMEXStatus()
        {

            MTNInitialize();

            _startTS = DateTime.Now;

            // Step 5 - 6
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry");
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG41838A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            //cargoEnquiryForm.btnEdit.DoClick();
            cargoEnquiryForm.DoEdit();
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"IMEX Status", @"Storage", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSave.DoClick();
            cargoEnquiryForm.DoSave();

            warningErrorForm = new WarningErrorForm("Warnings for Tracked Item Update " + cargoEnquiryForm.GetTerminalName());
            warningErrorForm.btnSave.DoClick();

            // Step 8
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^");

            // Step 9 - 17
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Admin", forceReset: true);
            TerminalAdministrationForm terminalAdministrationForm = new TerminalAdministrationForm();
            //MTNControlBase.SetValue(terminalAdministrationForm.cmbTable, @"Cam Protocol");
            terminalAdministrationForm.cmbTable.SetValue(@"Cam Protocol");

            var endTS = DateTime.Now;
            endTS = endTS.AddMinutes(2);

            _ediCAMProtocolForm = new EDICAMProtocolForm(@"Terminal Administration TT1");
            MTNControlBase.FindClickRowInTable(_ediCAMProtocolForm.tblProtocols,
                @"Protocol Description^Maersk Codeco~Name^Maersk Codeco", xOffset: 40);

            _ediCAMProtocolForm.GetDetailsTabAndDetails();
            _ediCAMProtocolForm.GetScheduledTimesTabAndDetails(findTab: true);

            //MTNControlBase.SetValue(_ediCAMProtocolForm.txtStartDate, _startTS.ToString(@"ddMMyyyy"),
              //  additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtStartDate.SetValue(_startTS.ToString(@"ddMMyyyy"),
                additionalWaitTimeout: 100);
            //MTNControlBase.SetValue(_ediCAMProtocolForm.txtStartTime, _startTS.ToString(@"HHmm"),
              //  additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtStartTime.SetValue(_startTS.ToString(@"HHmm"),
                additionalWaitTimeout: 100);
            //MTNControlBase.SetValue(_ediCAMProtocolForm.txtEndDate, endTS.ToString(@"ddMMyyyy"),
              //  additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtEndDate.SetValue(endTS.ToString(@"ddMMyyyy"),
                additionalWaitTimeout: 100);
            //MTNControlBase.SetValue(_ediCAMProtocolForm.txtEndTime, endTS.ToString(@"HHmm"),
              //  additionalWaitTimeout: 100);
            _ediCAMProtocolForm.txtEndTime.SetValue(endTS.ToString(@"HHmm"),
                additionalWaitTimeout: 100);
            _ediCAMProtocolForm.btnRunAdHoc.DoClick();

            // Adhoc Protocol run popup 1
            ConfirmationFormYesNo _confirmationFormYesNo = new ConfirmationFormYesNo(@"Adhoc Protocol Run");
            _confirmationFormYesNo.btnYes.DoClick();

            // Adhoc Protocol run popup 2
            ConfirmationFormOK _confirmationFormOk = new ConfirmationFormOK(@"Adhoc Protocol Run", automationIdMessage: @"3",
                automationIdOK: @"4");
            _confirmationFormOk.btnOK.DoClick();

            _ediCAMProtocolForm.SetFocusToForm();
            _ediCAMProtocolForm.GetProtocolTaskHistoryTabAndDetails(@"Protocol Task History [1]");

            MTNControlBase.FindClickRowInTable(_ediCAMProtocolForm.tblProtocolTaskHistory, @"Task Status^Complete", ClickType.DoubleClick);

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Warning");
            _confirmationFormYesNo.btnYes.DoClick();

            var ediProtocolTaskHistoryForm = new EDIProtocolTaskHistoryForm(TestContext);
            //ediProtocolTaskHistoryForm.SaveMessageAsFile(dataDirectory + @"41838_CAMProtocolTaskHistory.txt");

            /*List<string> fileLines =
                ediProtocolTaskHistoryForm.ReadMessageDetailsFromFile(
                    dataDirectory + @"41838_CAMProtocolTaskHistory.txt");
            ediProtocolTaskHistoryForm.CloseForm();*/

            /*string[] linesToCheck =
            {
                @"EQD+CN+JLG41838A01+2200:102:5++3+5'"
            };

            //string linesNotFound = null;

            var allLines = File.ReadAllLines(dataDirectory + @"41838_CAMProtocolTaskHistory.txt");
            var linesNotFound = linesToCheck.Where(line => !allLines.Contains(line)).ToArray();*/

            /*foreach (string line in linesToCheck)
            {
                _matchFound = false;

                for (int index = 0; index < fileLines.Count; index++)
                {
                   if (line.Equals(fileLines[index].Trim()))
                   {
                        _matchFound = true;
                        break;
                   }
                }

                if (!_matchFound)
                {
                    linesNotFound += line + "\r\n";
                }
            }*/

            ediProtocolTaskHistoryForm.ValidateDetailsExist(new[] { "EQD+CN+JLG41838A01+2200:102:5++3+5'" },
                out var linesNotFound, false);
            
            Assert.IsTrue(linesNotFound is null || !linesNotFound.Any(),
                $"TestCase41838- The following lines where not found in the file:\r\n{string.Join("\r\n", linesNotFound)}");

        }



        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = "_41838_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG41838A01</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSK</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n			<isoType>2200</isoType>\n			<commodity>APPL</commodity>\n            <locationId>MKBS01</locationId>\n            <weight>2000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG41838A01</id>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSK</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n			<isoType>2200</isoType>\n			<commodity>APPL</commodity>\n            <locationId>MKBS01</locationId>\n            <weight>2000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");


            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads



    }

}
