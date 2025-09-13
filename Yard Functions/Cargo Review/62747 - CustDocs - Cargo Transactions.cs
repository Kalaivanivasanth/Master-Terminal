using DataObjects.LogInOutBO;
using FlaUI.Core.WindowsAPI;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Yard_Functions.Cargo_Review;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;


namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Cargo_Review
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase62747 : MTNBase
    {
        private const string CargoId = "62747-CUSTOMS1";
        private const string RemarksValue = "62747";
        private const string DocumentType = "KVR27\tKVR DOC TYPE";


            
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"resetData_62747");
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void CargoTransactionForCustomDocument()
        {
            MTNInitialize();

            FormObjectBase.NavigationMenuSelection("Yard Functions|Cargo Review");
            CargoReviewForm.DoSelectorQuery(new[]
            {
                new SelectorQueryArguments(CargoReviewForm.Property.Site, CargoReviewForm.Operation.EqualTo, Site.OnSite),
                new SelectorQueryArguments(CargoReviewForm.Property.Id, CargoReviewForm.Operation.EqualTo, "62747-CUSTOMS1"),
                new SelectorQueryArguments(CargoReviewForm.Property.Mark, CargoReviewForm.Operation.StartsWith, "62747"),
            });
            CargoReviewForm.DoSearch();
                      

            CargoReviewForm.OpenRequiredContextMenuForCreate(CargoReviewForm.QueryResultsContextMenu.CreateCustomDocument);
            var doCustomDocumentProcess = new CustomDocumentMaintenanceForm($"Custom Document Maint TT1");
            doCustomDocumentProcess.DoCustomDocumentProcess(RemarksValue, DocumentType);

           FormObjectBase.NavigationMenuSelection(@"General Functions|Custom Document Enquiry", forceReset: true);            
            customDocumentEnquiryForm = new CustomDocumentEnquiryForm($"Custom Document Enquiry TT1");
            customDocumentEnquiryForm.CmbDocumentType.SetValue("KVR27\tKVR DOC TYPE", searchSubStringTo: 4, doDownArrow: true);
            customDocumentEnquiryForm.DoFind();
            
            customDocumentEnquiryForm.tblResults.FindClickRow(
                new[] { @"Remarks^62747" }, clickType: ClickType.Click);
            customDocumentEnquiryForm.DoRemove();
            Keyboard.TypeSimultaneously(VirtualKeyShort.ENTER);
            Keyboard.TypeSimultaneously(VirtualKeyShort.ENTER);


            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm($"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Metal, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnSite, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.SetFocusToForm();
            

            cargoEnquiryForm.tblData2.FindClickRow(new[] { $"ID^{CargoId}" });
            cargoEnquiryForm.DoViewTransactions();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm =
                FormObjectBase.CreateForm<CargoEnquiryTransactionForm>($@"Transactions for {CargoId} TT1");
            
            
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow( new[]
            {
                "Type^Custom Document Added~Details^KVR27 Inward",
                "Type^Custom Document Updated~Details^KVR27 Inward",
                "Type^Custom Document Deleted~Details^KVR27 Inward",
            });



        }
     
        
        void SetupAndLoadInitializeData(TestContext testContext)
        {

            fileOrder = 1;
            searchFor = "_62747_";

            // Delete Cargo OnShip
            CreateDataFileToLoad("DeleteCargoOnSite.xml",
      "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>METAL</cargoTypeDescr>\n            <id>62747-CUSTOMS1</id>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>BULK</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>IMPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n                 <mark>62747</mark>\n                <countryOfOriginCode>US</countryOfOriginCode>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnShip
            CreateDataFileToLoad("CreateCargoOnSite.xml",
      "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>METAL</cargoTypeDescr>\n            <id>62747-CUSTOMS1</id>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>BULK</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>IMPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n                 <mark>62747</mark>\n                <countryOfOriginCode>US</countryOfOriginCode>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
