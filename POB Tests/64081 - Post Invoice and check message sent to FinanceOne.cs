using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNGlobal.Classes;
using System;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items;
using MTNUtilityClasses.Classes;
using DataFiles.UserDefinedFiles;
using HardcodedData.SystemData;
using FlaUI.Core.Input;
using MTNForms.Controls;
using MTNForms.FormObjects;
using System.Text.RegularExpressions;
using System.Windows;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.POB_Tests
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase64081  : MTNBase
    {
        private const string TestCaseNumber = @"64081";
        InvoiceListForm _invoiceListForm;
        InvoiceUpdateForm invoiceUpdateForm;
        ConfirmationFormYesNo _confirmationFormYesNo;
        ConfirmationFormOK confirmationFormOK;
        EWSMessageForm eWSMessageForm;
        static string _password;
        protected static string expectedMessage = "REQUEST\r\n\r\n{\r\n  \"invoices\": [\r\n    {\r\n      \"impName\": \"ARINVPI\",\r\n      \"docId\": \"1\",\r\n      \"lneId\": 1,\r\n      \"docRef1\": \"3\",\r\n      \"docDatei1\": \"\",\r\n      \"docPeriod\": 6,\r\n      \"lneLdgCode\": \"AR\",\r\n      \"lneAccnbri\": \"18409\",\r\n      \"lneAmt1\": {\r\n        \"units\": \"50\"\r\n      },\r\n      \"lneNarr1\": \"\",\r\n      \"lneNarr2\": \"\",\r\n      \"lneNarr3\": \"Import\",\r\n      \"batName\": \"F1OVER\",\r\n      \"batFmtName\": \"F1OVER\",\r\n      \"batDocType\": \"F1OVER\",\r\n      \"batPeriod\": 6,\r\n      \"batPgrpName\": \"F1OVER\",\r\n      \"batBalLdgCode\": \"GL\",\r\n      \"batBalAccnbri\": \"1999999999\",\r\n      \"docDocType\": \"ARINVPI\",\r\n      \"docSource\": \"AR\",\r\n      \"status\": \"S\",\r\n      \"lneVatType\": \"I\",\r\n      \"lneVatRateAmt\": {},\r\n      \"lneVatRateCode\": \"NA\",\r\n      \"lneVatAmt\": {},\r\n      \"lneVatExcAmt\": {\r\n        \"units\": \"50\"\r\n      }\r\n    },\r\n    {\r\n      \"impName\": \"ARINVPI\",\r\n      \"docId\": \"1\",\r\n      \"lneId\": 2,\r\n      \"docRef1\": \"3\",\r\n      \"docDatei1\": \"\",\r\n      \"docPeriod\": 6,\r\n      \"lneLdgCode\": \"GL\",\r\n      \"lneAccnbri\": \"1\",\r\n      \"lneAmt1\": {\r\n        \"units\": \"-50\"\r\n      },\r\n      \"lneNarr1\": \"\",\r\n      \"lneNarr2\": \"\",\r\n      \"lneNarr3\": \"Import\",\r\n      \"batName\": \"F1OVER\",\r\n      \"batFmtName\": \"F1OVER\",\r\n      \"batDocType\": \"F1OVER\",\r\n      \"batPeriod\": 6,\r\n      \"batPgrpName\": \"F1OVER\",\r\n      \"batBalLdgCode\": \"GL\",\r\n      \"batBalAccnbri\": \"1999999999\",\r\n      \"docDocType\": \"ARINVPI\",\r\n      \"docSource\": \"AR\",\r\n      \"status\": \"S\",\r\n      \"lneVatType\": \"I\",\r\n      \"lneVatRateAmt\": {},\r\n      \"lneVatRateCode\": \"F\",\r\n      \"lneVatAmt\": {},\r\n      \"lneVatExcAmt\": {\r\n        \"units\": \"-50\"\r\n      }\r\n    }\r\n  ]\r\n}\r\n\r\nRESPONSE\r\nUnavailable\r\n";

        void MTNInitialize()
        {
            _password = TestContext.GetRunSettingValue("POBDEFAULT");

            LogInto<MTNLogInOutBO>(@"POBDEFAULT", terminal: "POB");
            
            SetupAndLoadInitializeData(TestContext);

        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
            BaseClassInitialize_New(testContext);
        }

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup()
        {

            base.TestCleanup();
        }

        [TestMethod]
        public void PostInvoiceAndCheckMessageSentToFinanceOne()
        {
            MTNInitialize();

            FormObjectBase.MainForm.OpenInvoiceListFromToolbar();
            _invoiceListForm = new InvoiceListForm();

            //_invoiceListForm.DoQueryByDebtor();

            _invoiceListForm.CmbDebtor.SetValue(@"Maersk A/S", shortenValueBy: 4, doDownArrow: true);
            _invoiceListForm.CmbStatusSearcher.SetValue(@"Open");

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            _invoiceListForm.DoFind();

            _invoiceListForm.GetDetailsTable();
            MTNControlBase.FindClickRowInTable(_invoiceListForm.tblDetails, @"Details^MTN64081A01", ClickType.LeftClick, countOffset: -1);

            // Change the status to Pending
            _invoiceListForm.cmbStatus.SetValue("Pending", additionalWaitTimeout: 200);
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Changing Invoice Status to Pending");
            _confirmationFormYesNo.btnYes.DoClick();

            // Change the status to Posted
            _invoiceListForm.cmbStatus.SetValue("Posted", searchSubStringTo: 4, doDownArrow: true);

            invoiceUpdateForm = new InvoiceUpdateForm("Invoice Post  ");
            invoiceUpdateForm.DoSave();

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Changing Invoice Status to Posted");
            _confirmationFormYesNo.btnYes.DoClick();

            // Wait for message box to appear and click OK
            confirmationFormOK = WaitForConfirmationFormOK(@"Finance One", TimeSpan.FromSeconds(120));
            confirmationFormOK.btnOK.DoClick();

            // Check the message sent to FinanceOne
            FormObjectBase.MainForm.OpenExternalWebServicesMonitorFromToolbar();
            ExternalWebServiceMonitorForm ewsMonitorForm = new ExternalWebServiceMonitorForm();

            ewsMonitorForm.cmbInterfaceName.SetValue(@"FinanceOne gRPC", doDownArrow: true);

            ewsMonitorForm.GetSearcher();
            ewsMonitorForm.btnSearch.DoClick();
            ewsMonitorForm.btnClose.DoClick();
            // MTNControlBase.FindClickRowInTable(ewsMonitorForm.tblDetails, @"Interface^FinanceOne gRPC", clickType: ClickType.DoubleClick);
            ewsMonitorForm.TblDetails.FindClickRow(["Interface^FinanceOne gRPC"], clickType: ClickType.DoubleClick);

            eWSMessageForm = new EWSMessageForm();
            eWSMessageForm.SetFocusToForm();
            eWSMessageForm.GetMessage();

            string messagetxt = Clipboard.GetText();
            string messageActual = Regex.Replace(messagetxt, @"/\d\d\d\d-\d\d-\d\d\w\d\d:\d\d:\d\d.\d\d\d\w", "");

            Assert.AreEqual(expectedMessage, messageActual, @"Expected: " + expectedMessage + "Actual: " + messageActual);
        }

        #region - Setup and Run Data Loads

        private void SetupAndLoadInitializeData(TestContext testContext)
        {
            var setArgumentsToCallAPI = new GetSetArgumentsToCallAPI
            { RequestURL = $"{TestContext.GetRunSettingValue("BaseUrl")}SendEDI?MasterTerminalAPI", TerminalId = "POB", UserName = "POBDEFAULT", Password = _password };

            CallAPI.AddDeleteData(new[] {
               new AddDeleteDataArguments { Data = new [] {
                    new CargoOnSiteXML { ID = "MTN64081A01", CargoTypeDescr = CargoType.ISOContainer, IMEXStatus = IMEX.Import, Voyage = "BISL0001", Operator = Operator.MSK, TotalWeight = "15999.0000",
                        DischargePort = "NZAKL", Location = "BS1", Action = "D", ISOType = "2200" },
                    }, FileType = "CargoOnSiteXML", ArgumentsToCallAPIs = setArgumentsToCallAPI },
            });
            CallAPI.AddDeleteData(new[] {
               new AddDeleteDataArguments { Data = new [] {
                    new CargoOnSiteXML { ID = "MTN64081A01", CargoTypeDescr = CargoType.ISOContainer, IMEXStatus = IMEX.Import, Voyage = "BISL0001", Operator = Operator.MSK, TotalWeight = "15999.0000",
                        DischargePort = "NZAKL", Location = "BS1", Action = "A", ISOType = "2200"  },
                    }, FileType = "CargoOnSiteXML", ArgumentsToCallAPIs = setArgumentsToCallAPI },
            });
        }

        #endregion - Setup and Run Data Loads

        private ConfirmationFormOK WaitForConfirmationFormOK(string formTitle, TimeSpan timeout)
        {
            var start = DateTime.Now;
            while (DateTime.Now - start < timeout)
            {
                try
                {
                    var confirmationForm = new ConfirmationFormOK(formTitle);
                    // Optionally, check if the form is actually visible/ready
                    return confirmationForm;
                }
                catch
                {
                    // Form not found yet, wait and retry
                    System.Threading.Thread.Sleep(500);
                }
            }
            throw new TimeoutException($"ConfirmationFormOK with title '{formTitle}' did not appear within {timeout.TotalSeconds} seconds.");
        }
    }
}
