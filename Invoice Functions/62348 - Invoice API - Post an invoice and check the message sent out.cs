using DataObjects;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items;
using MTNUtilityClasses.Classes;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase62348 : MTNBase
    {
        private const string TestCaseNumber = @"62348";
        protected static string ediFile1 = "62348Cancel.json";
        protected static string ediFile2 = "62348Create.json";
        protected static string expectedMessage = "{\r\n  \"myTestRecordObject\": null,\r\n  \"warningsHaveBeenAcknowledged\": \"false\",\r\n  \"invoice\": {\r\n    \"myTestRecordObject\": null,\r\n    \"warningsHaveBeenAcknowledged\": \"false\",\r\n    \"accuralType\": \"Until Closed\",\r\n    \"alternativeAddress\": null,\r\n    \"bankCode\": \"DFBU\",\r\n    \"billingAddress\": {\r\n      \"myTestRecordObject\": null,\r\n      \"warningsHaveBeenAcknowledged\": \"false\",\r\n      \"addr1\": \"1/8\",\r\n      \"addr2\": \"ABC STREET\",\r\n      \"addr3\": \"XYZ \",\r\n      \"city\": \"MAIN CITY\",\r\n      \"country\": \"ALBANIA\",\r\n      \"email\": \"abc@xyz.com\"\r\n    },\r\n    \"billOfLadingNumber\": null,\r\n    \"bookingReference\": null,\r\n    \"cargoGroupName\": null,\r\n    \"chargeTime\": \"\",\r\n    \"contractDesc\": null,\r\n    \"creditNoteReasonCode\": null,\r\n    \"customerDetails\": {\r\n      \"myTestRecordObject\": null,\r\n      \"warningsHaveBeenAcknowledged\": \"false\",\r\n      \"additionalInformation1\": \"Voyage Operator is Debtor\",\r\n      \"additionalInformation2\": \"Cargo Operator is Debtor\",\r\n      \"customerCode\": \"EUP TEST\",\r\n      \"customerName\": \"EUP TEST DEBTOR\",\r\n      \"externalReferenceCode\": \"PO EXT\",\r\n      \"externalReferenceName\": null,\r\n      \"postingNumber\": \"0\",\r\n      \"purchaseOrderCode\": \"PO EUP\"\r\n    },\r\n    \"dates\": {\r\n      \"myTestRecordObject\": null,\r\n      \"warningsHaveBeenAcknowledged\": \"false\",\r\n      \"batchDate\": \"\",\r\n      \"dueDate\": \"\",\r\n      \"fromDate\": \"\",\r\n      \"invoiceDate\": \"\",\r\n      \"invoiceEndDate\": \"\",\r\n      \"pendingDate\": \"\",\r\n      \"postedDate\": \"\",\r\n      \"prependDate\": \"\",\r\n      \"taxDate\": \"\",\r\n      \"toDate\": \"\"\r\n    },\r\n    \"externalInvoiceNumber\": \"\",\r\n    \"externalTaxNumber\": null,\r\n    \"fxRateInvEndDTMonthEnd\": \"0\",\r\n    \"fxRateInvEndDTMonthStart\": \"0\",\r\n    \"invoiceCurrency\": null,\r\n    \"invoiceLines\": [\r\n      {\r\n        \"myTestRecordObject\": null,\r\n        \"warningsHaveBeenAcknowledged\": \"false\",\r\n        \"bank\": null,\r\n        \"cargoDetails\": {\r\n          \"myTestRecordObject\": null,\r\n          \"warningsHaveBeenAcknowledged\": \"false\",\r\n          \"cargoSubType\": \"Rolls1\",\r\n          \"cargoType\": null,\r\n          \"commodity\": \"Rolls1\",\r\n          \"commodityGroup\": null,\r\n          \"dischargePort\": \"Antwerp Churchill Terminal\",\r\n          \"imex\": \"IMPORT\",\r\n          \"quantity\": \"1\",\r\n          \"tiID\": \"PAPER62348C\",\r\n          \"vesselName\": \"Bermuda islander\",\r\n          \"volume\": \"0\",\r\n          \"voyageCode\": \"BISLCC0001\",\r\n          \"weight\": \"2335\"\r\n        },\r\n        \"charges\": [\r\n          {\r\n            \"myTestRecordObject\": null,\r\n            \"warningsHaveBeenAcknowledged\": \"false\",\r\n            \"chargeSeqNo\": \"\",\r\n            \"glCode\": \"GL1\",\r\n            \"rate\": \"50\",\r\n            \"service\": {\r\n              \"myTestRecordObject\": null,\r\n              \"warningsHaveBeenAcknowledged\": \"false\",\r\n              \"calculationType\": {\r\n                \"myTestRecordObject\": null,\r\n                \"warningsHaveBeenAcknowledged\": \"false\",\r\n                \"amountMax\": \"0\",\r\n                \"amountMin\": \"0\",\r\n                \"calculationUnit\": \"Value of 1\",\r\n                \"code\": \"CCT00\",\r\n                \"fromTime\": \"\",\r\n                \"rateTypeDesc\": \"Value of 1\",\r\n                \"toTime\": \"\"\r\n              },\r\n              \"chargeByUnits\": \"false\",\r\n              \"externalReference1\": \"Ext Ref1\",\r\n              \"externalReference2\": \"Ext Ref2\",\r\n              \"remarks\": \"Service Remarks test\",\r\n              \"serviceName\": \"Service Type1\"\r\n            }\r\n          }\r\n        ],\r\n        \"chargeTime\": \"\",\r\n        \"contractDesc\": null,\r\n        \"contractID\": null,\r\n        \"externals\": {\r\n          \"myTestRecordObject\": null,\r\n          \"warningsHaveBeenAcknowledged\": \"false\",\r\n          \"externalRef1\": null,\r\n          \"externalRef2\": null,\r\n          \"externalRef3\": null,\r\n          \"externalRef4\": null,\r\n          \"externalTS01\": \"\",\r\n          \"externalTS02\": \"\",\r\n          \"externalTS03\": \"\",\r\n          \"externalTS04\": \"\",\r\n          \"externalTS05\": \"\",\r\n          \"externalTS06\": \"\",\r\n          \"externalTS07\": \"\"\r\n        },\r\n        \"invoiceLineDesc\": {\r\n          \"myTestRecordObject\": null,\r\n          \"warningsHaveBeenAcknowledged\": \"false\",\r\n          \"extRef\": null,\r\n          \"narration\": \"Service Type1\",\r\n          \"remarks\": null\r\n        },\r\n        \"invoiceLineNumber\": \"\",\r\n        \"lineRemarks\": null,\r\n        \"lineTotalAmount\": \"50\",\r\n        \"paymentTypeDescription\": \"None\",\r\n        \"quantity1\": \"1\",\r\n        \"quantity2\": \"0\",\r\n        \"quantityTotal\": \"1\",\r\n        \"quantityUOM1\": \"UNIT\",\r\n        \"quantityUOM2\": null,\r\n        \"remarks\": null,\r\n        \"taxAmount\": \"0\",\r\n        \"taxRate\": \"0\",\r\n        \"taxRateCode\": \"GST\",\r\n        \"totalRate\": \"0\",\r\n        \"trackedItemID\": null,\r\n        \"transactionDate\": \"\",\r\n        \"transactionID\": \"\",\r\n        \"transactionType\": \"Moved\",\r\n        \"uomCode\": \"Quantity\",\r\n        \"woID\": null\r\n      },\r\n      {\r\n        \"myTestRecordObject\": null,\r\n        \"warningsHaveBeenAcknowledged\": \"false\",\r\n        \"bank\": null,\r\n        \"cargoDetails\": {\r\n          \"myTestRecordObject\": null,\r\n          \"warningsHaveBeenAcknowledged\": \"false\",\r\n          \"cargoSubType\": \"Rolls1\",\r\n          \"cargoType\": null,\r\n          \"commodity\": \"Rolls1\",\r\n          \"commodityGroup\": null,\r\n          \"dischargePort\": \"Antwerp Churchill Terminal\",\r\n          \"imex\": \"IMPORT\",\r\n          \"quantity\": \"1\",\r\n          \"tiID\": \"PAPER62348D\",\r\n          \"vesselName\": \"Bermuda islander\",\r\n          \"volume\": \"0\",\r\n          \"voyageCode\": \"BISLCC0001\",\r\n          \"weight\": \"2328\"\r\n        },\r\n        \"charges\": [\r\n          {\r\n            \"myTestRecordObject\": null,\r\n            \"warningsHaveBeenAcknowledged\": \"false\",\r\n            \"chargeSeqNo\":\"\",\r\n            \"glCode\": \"GL1\",\r\n            \"rate\": \"50\",\r\n            \"service\": {\r\n              \"myTestRecordObject\": null,\r\n              \"warningsHaveBeenAcknowledged\": \"false\",\r\n              \"calculationType\": {\r\n                \"myTestRecordObject\": null,\r\n                \"warningsHaveBeenAcknowledged\": \"false\",\r\n                \"amountMax\": \"0\",\r\n                \"amountMin\": \"0\",\r\n                \"calculationUnit\": \"Value of 1\",\r\n                \"code\": \"CCT00\",\r\n                \"fromTime\": \"\",\r\n                \"rateTypeDesc\": \"Value of 1\",\r\n                \"toTime\": \"\"\r\n              },\r\n              \"chargeByUnits\": \"false\",\r\n              \"externalReference1\": \"Ext Ref1\",\r\n              \"externalReference2\": \"Ext Ref2\",\r\n              \"remarks\": \"Service Remarks test\",\r\n              \"serviceName\": \"Service Type1\"\r\n            }\r\n          }\r\n        ],\r\n        \"chargeTime\": \"\",\r\n        \"contractDesc\": null,\r\n        \"contractID\": null,\r\n        \"externals\": {\r\n          \"myTestRecordObject\": null,\r\n          \"warningsHaveBeenAcknowledged\": \"false\",\r\n          \"externalRef1\": null,\r\n          \"externalRef2\": null,\r\n          \"externalRef3\": null,\r\n          \"externalRef4\": null,\r\n          \"externalTS01\": \"\",\r\n          \"externalTS02\": \"\",\r\n          \"externalTS03\": \"\",\r\n          \"externalTS04\": \"\",\r\n          \"externalTS05\": \"\",\r\n          \"externalTS06\": \"\",\r\n          \"externalTS07\": \"\"\r\n        },\r\n        \"invoiceLineDesc\": {\r\n          \"myTestRecordObject\": null,\r\n          \"warningsHaveBeenAcknowledged\": \"false\",\r\n          \"extRef\": null,\r\n          \"narration\": \"Service Type1\",\r\n          \"remarks\": null\r\n        },\r\n        \"invoiceLineNumber\": \"\",\r\n        \"lineRemarks\": null,\r\n        \"lineTotalAmount\": \"50\",\r\n        \"paymentTypeDescription\": \"None\",\r\n        \"quantity1\": \"1\",\r\n        \"quantity2\": \"0\",\r\n        \"quantityTotal\": \"1\",\r\n        \"quantityUOM1\": \"UNIT\",\r\n        \"quantityUOM2\": null,\r\n        \"remarks\": null,\r\n        \"taxAmount\": \"0\",\r\n        \"taxRate\": \"0\",\r\n        \"taxRateCode\": \"GST\",\r\n        \"totalRate\": \"0\",\r\n        \"trackedItemID\": null,\r\n        \"transactionDate\": \"\",\r\n        \"transactionID\": \"\",\r\n        \"transactionType\": \"Moved\",\r\n        \"uomCode\": \"Quantity\",\r\n        \"woID\": null\r\n      }\r\n    ],\r\n    \"invoiceNarration\": null,\r\n    \"invoiceNote\": null,\r\n    \"invoiceProcess\": \"InvoiceAPI Process\",\r\n    \"invoiceType\": \"Invoice\",\r\n    \"invoiceTypeDesc\": \"Invoice\",\r\n    \"isManual\": \"false\",\r\n    \"isZeroRatedTax\": \"false\",\r\n    \"locale\": null,\r\n    \"otherRelatedInvoices\": {\r\n      \"myTestRecordObject\": null,\r\n      \"warningsHaveBeenAcknowledged\": \"false\",\r\n      \"additionalFeesInvoice\": null,\r\n      \"mergedInvoice\": null,\r\n      \"postedCreditInvoice\": null,\r\n      \"postedPendingInvoice\": null,\r\n      \"reversedCreditInvoice\": null,\r\n      \"reversedPendingInvoice\": null\r\n    },\r\n    \"paymentTermsCode\": \"30 Days\",\r\n    \"postedByUserCode\": null,\r\n    \"releaseRequestID\": null,\r\n    \"sequenceNumber\": \"1\",\r\n    \"terminalCode\": \"EUP\",\r\n    \"termsOfTrade\": \"30 Days\",\r\n    \"totalInvoiceAmountPart\": \"100\",\r\n    \"totalInvoiceAmountWhole\": \"100\",\r\n    \"totalNumberInvoiceLinesPart\": \"2\",\r\n    \"totalNumberInvoiceLinesWhole\": \"2\",\r\n    \"totalTaxAmount\": \"0\",\r\n    \"voyageID\": \"BISLCC0001\",\r\n    \"woID\": null,\r\n    \"workOrderNumber\": null\r\n  },\r\n  \"metaData\": {\r\n    \"myTestRecordObject\": null,\r\n    \"warningsHaveBeenAcknowledged\": \"false\",\r\n    \"firstInvoiceLineIdx\": \"1\",\r\n    \"invoiceMessageUniqueIdentifier\": \"DFBU-EUP-BEEUPIN240001-1-1-1-1\",\r\n    \"invoiceOutputMode\": \"No Consolidation\",\r\n    \"lastInvoiceLineIdx\": \"2\",\r\n    \"partNumber\": \"1\",\r\n    \"pathToExportedData\": \"TODO-TODO-TODO\",\r\n    \"postingUser\": {\r\n      \"myTestRecordObject\": null,\r\n      \"warningsHaveBeenAcknowledged\": \"false\",\r\n      \"userCode\": \"USER62348\",\r\n      \"userName\": \"USER62348\"\r\n    },\r\n    \"sendDate\": \"\",\r\n    \"totalNumberOfInvoiceLines\": \"2\"\r\n  }\r\n}";
        private InvoiceListForm _invoiceListForm;
        ConfirmationFormYesNo _confirmationFormYesNo;
        EWSMessageForm eWSMessageForm;
        string uniqueID;
        CallAPI callAPI;
        GetSetArgumentsToCallAPI setArgumentsToCallAPI;
        string baseURL;
        string uniqueMsgID;


        void MTNInitialize()
        {
            TestRunDO.GetInstance().SetKillBPGToFalse();
            TestRunDO.GetInstance().SetDoResetConfigsToFalse();
            BaseCommon.terminalId = "EUP";
            baseURL = TestContext.GetRunSettingValue(@"BaseUrl");

            LogInto<MTNLogInOutBO>(@"USER62348", terminal: "EUP");
            //MTNSignon(TestContext, @"USER62348");

            uniqueID = GetUniqueId();
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

            SetupAndLoadInitializeData2(TestContext);
            base.TestCleanup();
        }

        [TestMethod]
        public void InvoiceAPIPostInvoiceAndCheckMessage()
        {
            MTNInitialize();

            DateTime now = DateTime.Now;
            
            FormObjectBase.NavigationMenuSelection(@"Invoice Functions|Invoice List");
            _invoiceListForm = new InvoiceListForm();

            //_invoiceListForm.DoQueryByDebtor();
            
            _invoiceListForm.CmbDebtor.SetValue(@"EUP TEST DEBT", doTab: false, additionalWaitTimeout: 1000, doDownArrow: true);
            _invoiceListForm.CmbStatusSearcher.SetValue(@"Open", doTab: false, additionalWaitTimeout: 1000, doDownArrow: true);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            _invoiceListForm.DoFind();

            // Change the status to Pending
            _invoiceListForm.GetDetailsTable();
            MTNControlBase.FindClickRowInTable(_invoiceListForm.tblDetails, @"Details^BISLCC0002", ClickType.LeftClick, countOffset: -1);

            _invoiceListForm.cmbStatus.SetValue("Pending", additionalWaitTimeout: 200);
            _invoiceListForm.GetStatusPendingPostedGroup();
            _invoiceListForm.btnStatusSave.DoClick();

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Changing Invoice Status to Pending");
            _confirmationFormYesNo.btnYes.DoClick();

            // Change the status to Posted
            _invoiceListForm.cmbStatus.SetValue("Posted", additionalWaitTimeout: 200);
            _invoiceListForm.GetStatusPendingPostedGroup();
            _invoiceListForm.btnStatusSave.DoClick();


            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Changing Invoice Status to Posted Pending");
            _confirmationFormYesNo.btnYes.DoClick();
            

            FormObjectBase.NavigationMenuSelection(@"System Ops|External Web Service Monitor");
            ExternalWebServiceMonitorForm ewsMonitorForm = new ExternalWebServiceMonitorForm();

            ewsMonitorForm.cmbInterfaceName.SetValue(@"Invoice API", doDownArrow: true);
            
            ewsMonitorForm.GetSearcher();
            string date = now.AddMinutes(-5).ToString("dd/MM/yyyy");
            date = date.Replace("\\", "");
            ewsMonitorForm.txtCreatedFromDate.SetValue(date);
            string time = now.ToString("HH:mm");
            ewsMonitorForm.txtCreatedFromTime.SetValue(time);
            ewsMonitorForm.btnSearch.DoClick();
            ewsMonitorForm.btnClose.DoClick();
            // MTNControlBase.FindClickRowInTable(ewsMonitorForm.tblDetails, @"Interface^Invoice API", clickType: ClickType.DoubleClick);
            ewsMonitorForm.TblDetails.FindClickRow(["Interface^Invoice API"], clickType: ClickType.DoubleClick);

            eWSMessageForm = new EWSMessageForm();
            eWSMessageForm.SetFocusToForm();
            eWSMessageForm.GetMessage();

            string messagetxt = Clipboard.GetText();
            
            string messageActual = Regex.Replace(messagetxt,@"\[^]+\\s*:\s*(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:Z|[+-]\d{2}:\d{2}))", "");

            // save the Unique Message ID to send back a Success Response later in the test
             Match m = Regex.Match(messageActual, @"DFBU-EUP-BEEUPIN[0-9][0-9][0-9][0-9][0-9][0-9]-[0-9][0-9]-[0-9]-[0-9]-[0-9]");
            if (m.Success)

            {
                uniqueMsgID = m.Value;
                Console.WriteLine("Found '{0}' at position {1}.", m.Value, m.Index);
            }

            messageActual = Regex.Replace(messageActual, @"^BEEUPIN\d\d\d\d\d\d$", "");
            Regex.Replace(messageActual, "\"invoiceLineNumber\": \"[0-9]+\"", "\"invoiceLineNumber\""+ ":" + " " + "\"\"");
            Regex.Replace(messageActual, "\"transactionID\": \"[0-9]*\\.[0-9]+\"", "\"transactionID\"" + ":" + " " + "\"\"");
            Regex.Replace(messageActual, "\"chargeSeqNo\": \"[0-9]+\"", "\"chargeSeqNo\"" + ":" + " " + "\"\"");

            Assert.AreEqual(expectedMessage, messageActual, @"Expected: " + expectedMessage  + "Actual: " + messageActual);

            
        }

        #region - Setup and Run Data Loads

        private void SetupAndLoadInitializeData(TestContext testContext)
        {
           string data = "{\r\n  \"Header\": {\r\n    \"Sender\": \"PERFDEBTOR\",\r\n    \"Receiver\": \"EUROPORTS TERMINALS\",\r\n    \"Date\": \"2024-02-20T12:04:27+13:00\",\r\n    \"Type\": \"Manifest\",\r\n    \"Status\": \"Create\",\r\n    \"Number\": \"" + uniqueID + "\",\r\n    \"Id\": \"hfd2nk9g-y1yb-j4ar-ty3o-yk4wfrqlp4oc\",\r\n    \"HeaderReferences\": {\r\n      \"HeaderReference\": [\r\n        {\r\n          \"ReferenceName\": \"ReferenceCustomer\",\r\n          \"ReferenceValue\": \"2022040213371069\"\r\n        },\r\n        {\r\n          \"ReferenceName\": \"ReferenceInternal\",\r\n          \"ReferenceValue\": \"CATHERINE 821320\"\r\n        }\r\n      ]\r\n    }\r\n  },\r\n  \"Transports\": {\r\n    \"Transport\": [\r\n      {\r\n        \"TransportationMode\": \"Sea\",\r\n        \"EstimatedDateTimeOfArrival\": \"2024-02-27T12:04:27+13:00\",\r\n        \"CarrierParty\": {\r\n          \"Code\": \"16941\",\r\n          \"Name\": \"American Auto Tpt\",\r\n          \"Address\": {\r\n            \"Street1\": \".\",\r\n            \"Street2\": \"Klippan 1 A\",\r\n            \"PostalCode\": \"41451 G TEBORG\",\r\n            \"City\": \"G TEBORG\",\r\n            \"Country\": \"SWEDEN\",\r\n            \"CountryCode\": \"SE\"\r\n          }\r\n        },\r\n        \"VesselInfo\": {\r\n          \"VesselCategory\": \"RORO\",\r\n          \"VoyageNumber\": \"CATHERINE 821320\",\r\n          \"VesselName\": \"CATHERINE\",\r\n          \"DischargingPort\": \"USJAX\"\r\n        },\r\n        \"Goods\": {\r\n          \"Good\": [\r\n            {\r\n              \"MovementType\": \"Import\",\r\n              \"BOLReference\": \"BISLCC0002\",\r\n              \"GoodsOwner\": {\r\n                \"Code\": \"EUP\",\r\n                \"Name\": \"BillerudKorsn s AB\",\r\n                \"Address\": {\r\n                  \"Street1\": null,\r\n                  \"Street2\": \"Box 703\",\r\n                  \"PostalCode\": \"169 27\",\r\n                  \"City\": \"SOLNA\",\r\n                  \"Country\": \"SWEDEN\",\r\n                  \"CountryCode\": \"SE\"\r\n                }\r\n              },\r\n              \"ID\": 1,\r\n              \"KeyMarks\": \"62348\",\r\n              \"Reference\": \"PRIME WHITE\",\r\n              \"MillOrderNumber\": \"50241212\",\r\n              \"MillOrderLineNumber\": \"1\",\r\n              \"Remarks\": \"AX201\",\r\n              \"Width\": 100,\r\n              \"Length\": null,\r\n              \"Diameter\": 100,\r\n              \"LengthUOM\": \"Millimeter\",\r\n              \"Grammage\": 80.0,\r\n              \"PiecesPerPack\": 10,\r\n              \"Type\": \"Breakbulk\",\r\n              \"CargoType\": \"PAPER1\",\r\n              \"PackageType\": \"ROLLS1\",\r\n              \"QuantityExpected\": 1,\r\n              \"GrossWeightExpected\": 4283,\r\n              \"NettWeightExpected\": 4263,\r\n              \"WeightUOM\": \"Kilogram\",\r\n              \"Mill\": {\r\n                \"Code\": \"CSKO\",\r\n                \"Name\": \"BILLERUDKORSN S SWEDEN AB\",\r\n                \"Address\": {\r\n                  \"Street1\": \".\",\r\n                  \"Street2\": \"Box 703\",\r\n                  \"PostalCode\": \"SE-169 27\",\r\n                  \"City\": \"SOLNA\",\r\n                  \"Country\": \"SWEDEN\",\r\n                  \"CountryCode\": \"SE\"\r\n                }\r\n              },\r\n              \"Buyer\": {\r\n                \"Code\": \"ABCNE\",\r\n                \"Name\": \"BillerudKorsn s AB\",\r\n                \"Address\": {\r\n                  \"Street1\": null,\r\n                  \"Street2\": \"Box 703\",\r\n                  \"PostalCode\": \"169 27\",\r\n                  \"City\": \"SOLNA\",\r\n                  \"Country\": \"SWEDEN\",\r\n                  \"CountryCode\": \"SE\"\r\n                }\r\n              },\r\n              \"Customs\": false,\r\n              \"ContainerNumber\": null,\r\n              \"LoadingPort\": \"NZNSN\",\r\n              \"CustomerReference\": \"6001939\",\r\n              \"Items\": {\r\n                \"Item\": [\r\n                  {\r\n                    \"ItemId\": \"PAPER62348C\",\r\n                    \"ItemBarcode1\": 723159300,\r\n                    \"ItemBarcode2\": 321664642,\r\n                    \"QuantityExpected\": 1,\r\n                    \"GrossWeightExpected\": 514,\r\n                    \"NettWeightExpected\": 511,\r\n                    \"PiecesPerPack\": 10,\r\n                    \"ContainerNumber\": null,\r\n                    \"RunningLength\": 63358,\r\n                    \"RunningLengthUOM\": \"Meter\"\r\n                  },\r\n                  {\r\n                    \"ItemId\": \"PAPER62348D\",\r\n                    \"ItemBarcode1\": 509240624,\r\n                    \"ItemBarcode2\": 660504325,\r\n                    \"QuantityExpected\": 1,\r\n                    \"GrossWeightExpected\": 579,\r\n                    \"NettWeightExpected\": 576,\r\n                    \"PiecesPerPack\": 10,\r\n                    \"ContainerNumber\": null,\r\n                    \"RunningLength\": 66445,\r\n                    \"RunningLengthUOM\": \"Meter\"\r\n                  }\r\n                ]\r\n              }\r\n            }\r\n          ]\r\n        }\r\n      }\r\n    ]\r\n  }\r\n}\r\n";
           /* HttpClient client = new HttpClient();
            // client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(
            // System.Text.ASCIIEncoding.ASCII.GetBytes(
            //   $"{"jlgmk3"}:{"Password1*"}")));
            BearerToken token = new BearerToken();
            bearer = token.GetToken().Result.ToString();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearer);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://web1.navtmast.nav.cnw.co.nz/bin_public/jadehttp.dll/SendEDI?MasterTerminalAPI");
            request.Content = new StringContent(data, Encoding.UTF8, "application/json");
            Console.WriteLine("Request URL: " + request.RequestUri + "Request Method: " + request.Method.ToString() + "Request content: " + request.Content.ToString());
            var response = client.SendAsync(request);
            Assert.IsTrue(response.Result.IsSuccessStatusCode, "Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());
            Console.WriteLine("Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());
            */
            callAPI = new CallAPI();
            setArgumentsToCallAPI = new GetSetArgumentsToCallAPI();
            setArgumentsToCallAPI.MessageBody = data;
            setArgumentsToCallAPI.Authorization = "Bearer";
            setArgumentsToCallAPI.RequestURL = baseURL + "SendEDI?MasterTerminalAPI";
            setArgumentsToCallAPI.UserName = "USER62348";
            setArgumentsToCallAPI.Password = "Password1*";
            setArgumentsToCallAPI.TerminalId = "EUP";
            setArgumentsToCallAPI.OperatorId = "EuroPorts";
            var response = callAPI.DoCallAPI(setArgumentsToCallAPI);
            Assert.IsTrue(response.Result.IsSuccessStatusCode, "Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());
            Console.WriteLine("Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());

        }

        private void SetupAndLoadInitializeData2(TestContext testContext)
        {
             string data = "{\r\n  \"Header\": {\r\n    \"Sender\": \"PERFDEBTOR\",\r\n    \"Receiver\": \"EUROPORTS TERMINALS\",\r\n    \"Date\": \"2024-02-20T12:04:27+13:00\",\r\n    \"Type\": \"Manifest\",\r\n    \"Status\": \"Cancel\",\r\n    \"Number\": \"" + uniqueID + "\",\r\n    \"Id\": \"hfd2nk9g-y1yb-j4ar-ty3o-yk4wfrqlp4oc\",\r\n    \"HeaderReferences\": {\r\n      \"HeaderReference\": [\r\n        {\r\n          \"ReferenceName\": \"ReferenceCustomer\",\r\n          \"ReferenceValue\": \"2022040213371069\"\r\n        },\r\n        {\r\n          \"ReferenceName\": \"ReferenceInternal\",\r\n          \"ReferenceValue\": \"CATHERINE 821320\"\r\n        }\r\n      ]\r\n    }\r\n  },\r\n  \"Transports\": {\r\n    \"Transport\": [\r\n      {\r\n        \"TransportationMode\": \"Sea\",\r\n        \"EstimatedDateTimeOfArrival\": \"2024-02-27T12:04:27+13:00\",\r\n        \"CarrierParty\": {\r\n          \"Code\": \"16941\",\r\n          \"Name\": \"American Auto Tpt\",\r\n          \"Address\": {\r\n            \"Street1\": \".\",\r\n            \"Street2\": \"Klippan 1 A\",\r\n            \"PostalCode\": \"41451 G TEBORG\",\r\n            \"City\": \"G TEBORG\",\r\n            \"Country\": \"SWEDEN\",\r\n            \"CountryCode\": \"SE\"\r\n          }\r\n        },\r\n        \"VesselInfo\": {\r\n          \"VesselCategory\": \"RORO\",\r\n          \"VoyageNumber\": \"CATHERINE 821320\",\r\n          \"VesselName\": \"CATHERINE\",\r\n          \"DischargingPort\": \"USJAX\"\r\n        },\r\n        \"Goods\": {\r\n          \"Good\": [\r\n            {\r\n              \"MovementType\": \"Import\",\r\n              \"BOLReference\": \"BISLCC0002\",\r\n              \"GoodsOwner\": {\r\n                \"Code\": \"EUP\",\r\n                \"Name\": \"BillerudKorsn s AB\",\r\n                \"Address\": {\r\n                  \"Street1\": null,\r\n                  \"Street2\": \"Box 703\",\r\n                  \"PostalCode\": \"169 27\",\r\n                  \"City\": \"SOLNA\",\r\n                  \"Country\": \"SWEDEN\",\r\n                  \"CountryCode\": \"SE\"\r\n                }\r\n              },\r\n              \"ID\": 1,\r\n              \"KeyMarks\": \"62348\",\r\n              \"Reference\": \"PRIME WHITE\",\r\n              \"MillOrderNumber\": \"50241212\",\r\n              \"MillOrderLineNumber\": \"1\",\r\n              \"Remarks\": \"AX201\",\r\n              \"Width\": 100,\r\n              \"Length\": null,\r\n              \"Diameter\": 100,\r\n              \"LengthUOM\": \"Millimeter\",\r\n              \"Grammage\": 80.0,\r\n              \"PiecesPerPack\": 10,\r\n              \"Type\": \"Breakbulk\",\r\n              \"CargoType\": \"PAPER1\",\r\n              \"PackageType\": \"ROLLS1\",\r\n              \"QuantityExpected\": 1,\r\n              \"GrossWeightExpected\": 4283,\r\n              \"NettWeightExpected\": 4263,\r\n              \"WeightUOM\": \"Kilogram\",\r\n              \"Mill\": {\r\n                \"Code\": \"CSKO\",\r\n                \"Name\": \"BILLERUDKORSN S SWEDEN AB\",\r\n                \"Address\": {\r\n                  \"Street1\": \".\",\r\n                  \"Street2\": \"Box 703\",\r\n                  \"PostalCode\": \"SE-169 27\",\r\n                  \"City\": \"SOLNA\",\r\n                  \"Country\": \"SWEDEN\",\r\n                  \"CountryCode\": \"SE\"\r\n                }\r\n              },\r\n              \"Buyer\": {\r\n                \"Code\": \"ABCNE\",\r\n                \"Name\": \"BillerudKorsn s AB\",\r\n                \"Address\": {\r\n                  \"Street1\": null,\r\n                  \"Street2\": \"Box 703\",\r\n                  \"PostalCode\": \"169 27\",\r\n                  \"City\": \"SOLNA\",\r\n                  \"Country\": \"SWEDEN\",\r\n                  \"CountryCode\": \"SE\"\r\n                }\r\n              },\r\n              \"Customs\": false,\r\n              \"ContainerNumber\": null,\r\n              \"LoadingPort\": \"NZNSN\",\r\n              \"CustomerReference\": \"6001939\"\r\n            }\r\n          ]\r\n        }\r\n      }\r\n    ]\r\n  }\r\n}\r\n";
            /* HttpClient client = new HttpClient();
             client.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearer);

             var request = new HttpRequestMessage(HttpMethod.Post, "https://web1.navtmast.nav.cnw.co.nz/bin_public/jadehttp.dll/SendEDI?MasterTerminalAPI");
             request.Content = new StringContent(data, Encoding.UTF8, "application/json");
             Console.WriteLine("Request URL: " + request.RequestUri + "Request Method: " + request.Method.ToString() + "Request content: " + request.Content.ToString());
             var response = client.SendAsync(request);
             Assert.IsTrue(response.Result.IsSuccessStatusCode, "Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());
             Console.WriteLine("Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());
            */
            setArgumentsToCallAPI.MessageBody = data;
            var response = callAPI.DoCallAPI(setArgumentsToCallAPI);
            Assert.IsTrue(response.Result.IsSuccessStatusCode, "Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());
            Console.WriteLine("Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());

        }


        #endregion - Setup and Run Data Loads

        public void UpdateInvoiceAsPass()
        {
            string data = "{\r\n  \"invoiceMessageUniqueIdentifier\": \"" + uniqueMsgID + "\",\r\n  \"responseDate\": \"string\",\r\n  \"invoiceProcessingResult\": \"SUCCESS\",\r\n  \"responseMessage\": \"string\"\r\n}";
            callAPI = new CallAPI();
            setArgumentsToCallAPI = new GetSetArgumentsToCallAPI();
            setArgumentsToCallAPI.MessageBody = data;
            setArgumentsToCallAPI.Authorization = "Bearer";
            setArgumentsToCallAPI.RequestURL = baseURL + "invoiceAsyncResponse?MasterTerminalAPI";
            setArgumentsToCallAPI.UserName = "USER62348";
            setArgumentsToCallAPI.Password = "Password1*";
            setArgumentsToCallAPI.TerminalId = "EUP";
            setArgumentsToCallAPI.OperatorId = "EuroPorts";
            var response = callAPI.DoCallAPI(setArgumentsToCallAPI);
            Assert.IsTrue(response.Result.IsSuccessStatusCode, "Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());
            Console.WriteLine("Response is :" + response.Result.StatusCode + response.Result.ReasonPhrase + response.Result.RequestMessage.ToString());

        }
    }
}
