using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNUtilityClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions.CAMS
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43988 : MTNBase
    {
        private BackgroundApplicationForm _backgroundApplicationForm;
        
        private DateTime _startTS;
      
        private const string _testCaseNumber = @"43988";
        private const string _cargoId = @"JLG" + _testCaseNumber + @"A01";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + _testCaseNumber + "_";
        }


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
            SetupAndLoadInitializeData(TestContext);

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void ForWeighedTransactionRFFPlusBNIsMissing()
        {
            MTNInitialize();

            // Step 5 - 7
            FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry");
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", _cargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", @"On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + _cargoId, rowHeight: 18);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            CargoEnquiryTransactionForm cargoEnquiryTransactionForm =
                new CargoEnquiryTransactionForm(@"Transactions for " + _cargoId + " TT1");

            List<DataGridViewRow> rowsInTable =
                new List<DataGridViewRow>
                (cargoEnquiryTransactionForm.tblTransactions.AsDataGridView()
                    .Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells.Any() && r.Cells[0].Name.Contains("Created")));

            _startTS = DateTime.Parse(Miscellaneous.ReturnTextFromTableString(rowsInTable[0].Cells[1].Name));
            
            cargoEnquiryTransactionForm.CloseForm();

            // Step 8 - 10.  Replaced by EDI File load as laready have this covered in other tests

            // Steps 14 - 15
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            // Step 16 - 23
            FormObjectBase.NavigationMenuSelection(@"EDI Functions | EDI CAMS Protocols", forceReset: true);
            EDICAMProtocolForm ediCAMProtocolForm = new EDICAMProtocolForm();

            MTNControlBase.FindClickRowInTable(ediCAMProtocolForm.tblProtocols,
                @"Protocol Description^Maersk Codeco~Name^Maersk Codeco", xOffset: 40);

            var endDateTime = DateTime.Now.AddMinutes(1);

            ediCAMProtocolForm.GetDetailsTabAndDetails();
            ediCAMProtocolForm.RunAdhocReport(_startTS.ToString(@"ddMMyyyy"),
                _startTS.ToString(@"HHmm"), endDateTime.ToString(@"ddMMyyyy"),
                endDateTime.ToString(@"HHmm"), getTab: true);

            // Step 26 - 28
            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + _cargoId, rowHeight: 18);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for " + _cargoId + " TT1");

            MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^EDI Info Sent",
                ClickType.DoubleClick);

            // Check logging details
            LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm(@"Logging Details");

            string[] linesToCheck =
            {
                @"RFF+BN:REF43988"
            };

            loggingDetailsForm.FindStringsInTable(linesToCheck);
            loggingDetailsForm.DoCancel();

        }



        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>43988</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG43988A01</id>\n      <bookingRef>REF43988</bookingRef>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>43988</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG43988A01</id>\n      <bookingRef>REF43988</bookingRef>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Clear Stops
            CreateDataFileToLoad(@"ClearStops.xml",
                "<?xml version='1.0'?> <JMTInternalStop xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalStop.xsd'>\n	<AllCargoUpdateRequest>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoUpdateRequest Terminal='TT1'>\n			<directDelivery/>\n			<clearanceExpiryDate/>\n			<updateType/>\n			<billOfLading/>\n			<cargoRemarks/>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<deliveryReleaseNumber>43988</deliveryReleaseNumber>\n			<description/>\n			<fullOrMT/>\n			<gradeCode/>\n			<id>JLG43988A01</id>\n			<messageMode>A</messageMode>\n			<stopAction>D</stopAction>\n			<stopCode>5</stopCode>\n			<subTerminalCode/>\n			<toDoAction/>\n			<toDoRemarks/>\n			<toDoTaskShort/>\n			<voyageCode/>\n		</CargoUpdateRequest>\n		<CargoUpdateRequest Terminal='TT1'>\n			<directDelivery/>\n			<clearanceExpiryDate/>\n			<updateType/>\n			<billOfLading/>\n			<cargoRemarks/>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<deliveryReleaseNumber>43988</deliveryReleaseNumber>\n			<description/>\n			<fullOrMT/>\n			<gradeCode/>\n			<id>JLG43988A01</id>\n			<messageMode>A</messageMode>\n			<stopAction>D</stopAction>\n			<stopCode>1</stopCode>\n			<subTerminalCode/>\n			<toDoAction/>\n			<toDoRemarks/>\n			<toDoTaskShort/>\n			<voyageCode/>\n		</CargoUpdateRequest>\n	</AllCargoUpdateRequest>\n</JMTInternalStop>\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads



    }

}
