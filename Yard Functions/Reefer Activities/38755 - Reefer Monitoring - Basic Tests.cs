using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions.Reefer;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Reefer_Activities
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase38755 : MTNBase
    {

        ReeferMonitoringForm _reeferMonitoringForm;
        ReeferConnectToPowerForm _reeferConnectToPowerForm;
        ConfirmationFormYesNo _confirmationFormYesNo;
        ReeferAddReadingForm _reeferAddReadingForm;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void ReeferMonitoringBasic()
        {
            MTNInitialize();
            
            //1. Go to reefer monitoring and retrieve reefer containers from terminal area
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Reefer Activities|Reefer Monitoring");
            _reeferMonitoringForm = new ReeferMonitoringForm();
            MTNControlBase.SetValueInEditTable(_reeferMonitoringForm.tblSearcher, @"Type of Area",
                @"Gridded Block Stack", rowDataType: EditRowDataType.ComboBox, waitTime: 30, doDownArrow: true, searchSubStringTo: 2);
            MTNControlBase.SetValueInEditTable(_reeferMonitoringForm.tblSearcher, @"Terminal Area", TT1.TerminalArea.GBRF01, rowDataType: EditRowDataType.ComboBox, waitTime: 30);
            MTNControlBase.SetValueInEditTable(_reeferMonitoringForm.tblSearcher, @"Cargo ID", @"JLG38755");
            //reeferMonitoringForm.btnFind.DoClick();
            _reeferMonitoringForm.DoFind();

            //2. Find Container JLG38755A01 and connect the reefer to power with specified date/time
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_reeferMonitoringForm.tblReefers, @"ID^JLG38755A01", clickType: ClickType.ContextClick);
            _reeferMonitoringForm.TblReefers.FindClickRow(new string[] { @"ID^JLG38755A01" }, clickType: ClickType.ContextClick);
            _reeferMonitoringForm.ContextMenuSelect(@"Connect JLG38755A01...");

            _reeferConnectToPowerForm = new ReeferConnectToPowerForm(@"JLG38755A01 Connect To Power");
            MTNControlBase.SetValueInEditTable(_reeferConnectToPowerForm.tblDetails, @"Date", @"05052018");
            MTNControlBase.SetValueInEditTable(_reeferConnectToPowerForm.tblDetails, @"Time", @"0323");
            _reeferConnectToPowerForm.btnOK.DoClick();

            //3. Check the reefer is connected to power
            _reeferMonitoringForm.SetFocusToForm();
            string strValue = MTNControlBase.GetValueInEditTable(_reeferMonitoringForm.tblReeferDetails, @"Connected");
            Assert.IsTrue(strValue == "Yes", "Reefer is expected to be Connected = Yes, Actual is Connected = " + strValue);

            //4. Add a reading to the reefer (temp/date/time)
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_reeferMonitoringForm.tblReefers, @"ID^JLG38755A01", clickType: ClickType.ContextClick);
            _reeferMonitoringForm.TblReefers.FindClickRow(new string[] { @"ID^JLG38755A01" }, clickType: ClickType.ContextClick);
            _reeferMonitoringForm.ContextMenuSelect(@"Add Reading JLG38755A01...");

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Communication Check");
            _confirmationFormYesNo.btnNo.DoClick();
            _reeferAddReadingForm = new ReeferAddReadingForm(@"Add Temperature Readings TT1");
            _reeferAddReadingForm.txtReadingDate.SetValue(@"20052018");
            _reeferAddReadingForm.txtReadingTime.SetValue(@"1523");
             MTNControlBase.FindClickRowInTable(_reeferAddReadingForm.TblReadings.GetElement(), @"Reefer ID^JLG38755A01", xOffset: -1);
            ////_reeferAddReadingForm.TblReadings.FindClickRow(["Reefer ID^JLG38755A01"], xOffset: -1);
            ////_reeferAddReadingForm.TblReadings.FindClickRow(new [] { @"Reefer ID^JLG38755A01" }, xOffset: -1);
            Keyboard.Type(@"-10");
            _reeferAddReadingForm.DoSaveAndClose();

            //5. check that the reading has been added OK
            _reeferMonitoringForm.SetFocusToForm();
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_reeferMonitoringForm.tblMonitoringHistory, @"Date Time^20/05/2018 15:23~Temperature^-10.0");
            _reeferMonitoringForm.TblMonitoringHistory.FindClickRow(["Date Time^20/05/2018 15:23~Temperature^-10.0"]);

            //6. Now disconnect the reefer
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_reeferMonitoringForm.tblReefers, @"ID^JLG38755A01", clickType: ClickType.ContextClick);
            _reeferMonitoringForm.TblReefers.FindClickRow(new string[] { "ID^JLG38755A01" }, clickType: ClickType.ContextClick);
            _reeferMonitoringForm.ContextMenuSelect(@"Disconnect JLG38755A01...");
            _reeferConnectToPowerForm = new ReeferConnectToPowerForm(@"JLG38755A01 Disconnect From Power");
            MTNControlBase.SetValueInEditTable(_reeferConnectToPowerForm.tblDetails, @"Date", @"01062018");
            MTNControlBase.SetValueInEditTable(_reeferConnectToPowerForm.tblDetails, @"Time", @"2341");
            _reeferConnectToPowerForm.btnOK.DoClick();

            //7. check that the reefer is no longer connected
            _reeferMonitoringForm.SetFocusToForm();
            strValue = MTNControlBase.GetValueInEditTable(_reeferMonitoringForm.tblReeferDetails, @"Connected");
            Assert.IsTrue(strValue == "No", "Reefer is expected to be Connected = Yes, Actual is Connected = " + strValue);
            _reeferMonitoringForm.CloseForm();

            //8. Open cargo enquiry and check the reefer values in the reefer tab
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG38755A01");
            cargoEnquiryForm.DoSearch();
            cargoEnquiryForm.tblData2.FindClickRow(new [] { @"ID^JLG38755A01" });
            cargoEnquiryForm.GetGenericTabTableDetails(@"Reefer", @"4093");
            cargoEnquiryForm.CargoEnquiryReeferTab();

            //check the reefer fields in cargo enquiry
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblReeferEdit, @"First Connect Date/Time");
            Assert.IsTrue(strValue == "05/05/2018 03:23", "First connect DT expected to be 05/05/2018 03:23, Actual is = " + strValue);
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblReeferEdit, @"Last Disconnect DT");
            Assert.IsTrue(strValue == "01/06/2018 23:41", "Last disconnect DT expected to be 01/06/2018 23:41, Actual is = " + strValue);
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblReeferEdit, @"Last Connect DT");
            Assert.IsTrue(strValue == "05/05/2018 03:23", "Last connect DT expected to be 05/05/2018 03:23, Actual is = " + strValue);
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblReeferEdit, @"Last Reefer Reading");
            Assert.IsTrue(strValue.Contains(@"-10.0"), "Last Reefer reading expected to -10.0, Actual is = " + strValue);

            //9. also check the transactions
            cargoEnquiryForm.tblData2.FindClickRow(new [] { @"ID^JLG38755A01" }, clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG38755A01 TT1");
            /*// Tuesday, 28 January 2025 navmh5 
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Power Disconnected~Details^Disconnection from Power", clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Temperature Read~Details^-10.0", clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Edited~Details^isConnected No => Yes: onPower No => Yes", clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Power Connected~Details^Connection to  Power", clickType: ClickType.None);*/
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(
            [
                "Type^Power Disconnected~Details^Disconnection from Power", "Type^Temperature Read~Details^-10.0",
                "Type^Edited~Details^isConnected No => Yes: onPower No => Yes",
                "Type^Power Connected~Details^Connection to  Power"
            ], clickType: ClickType.None);
            _cargoEnquiryTransactionForm.CloseForm();

            
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = "_38755_";
            
            //Delete Cargo Onsite
            CreateDataFileToLoad(@"DeleteCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>38755</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG38755A01</id>\n      <isoType>2230</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>GBRF01 0108</locationId>\n      <weight>2000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>REEF</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <temperature>-12</temperature>\n      <receiveDate>01/05/2018 14:32</receiveDate>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>38755</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG38755A02</id>\n      <isoType>2230</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>GBRF01 0308</locationId>\n      <weight>2000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>REEF</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <temperature>-12</temperature>\n      <receiveDate>01/05/2018 14:32</receiveDate>\n      <messageMode>D</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //Create Cargo Onsite
            CreateDataFileToLoad(@"CreateCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>38755</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG38755A01</id>\n      <isoType>2230</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>GBRF01 0108</locationId>\n      <weight>2000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>REEF</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <temperature>-12</temperature>\n      <receiveDate>01/05/2018 14:32</receiveDate>\n      <messageMode>A</messageMode>\n    </CargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n      <TestCases>38755</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG38755A02</id>\n      <isoType>2230</isoType>\n      <operatorCode>FLS</operatorCode>\n      <locationId>GBRF01 0308</locationId>\n      <weight>2000.0000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>REEF</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <temperature>-12</temperature>\n      <receiveDate>01/05/2018 14:32</receiveDate>\n      <messageMode>A</messageMode>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");


            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);


        }




    }

}
