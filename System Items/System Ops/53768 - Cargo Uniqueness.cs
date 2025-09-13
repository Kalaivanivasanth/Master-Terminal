using System.Linq;
using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.Controls;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Cargo_Storage_Areas;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase53768 : MTNBase
    {
        private CargoTypesForm _cargoTypesForm;
        private ConfirmationFormYesNo _confirmationFormYesNo;
        private LocationEnquiryForm _locationEnquiryForm;
        private CargoMoveItForm _cargoMoveItForm;

        string[] _rowNames =
            ["Allow Duplicate Id", "Use Cargo Group For Uniqueness", "Use Cargo Subtype For Uniqueness"];

    string _saveDirectory;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize()
        {
        }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            _saveDirectory = $@"{TestContext.GetRunSettingValue("SavedFilesDirectory")}\";
            LogInto<MTNLogInOutBO>();
            CallJadeScriptToRun(TestContext, "resetData_53867");
            SetupAndLoadInitializeData(TestContext);
        }


        [TestMethod]
        public void VerifyCargoUniqueness()
        {
            MTNInitialize();

            FormObjectBase.NavigationMenuSelection(NavigatorMenus.CargoTypes);
            _cargoTypesForm = new CargoTypesForm();

            MTNControlBase.SetValueInEditTable(_cargoTypesForm.tblSearch, "Abbreviation", "VPAK53867");
            _cargoTypesForm.DoSearch();

            _cargoTypesForm.tblCargoDetails1.FindClickRow(["Abbreviation^VPAK53867"]);

            _cargoTypesForm.GetDetailsTab();

            ValidateVPAK53867OnEntry();

            _cargoTypesForm.DoEdit();
            MTNControlBase.SetValueInEditTable(_cargoTypesForm.tblDetails, _rowNames[0], @"1",
                EditRowDataType.CheckBox);
            _cargoTypesForm.DoSave();

            SetAllowDuplicateIdAndValidate();

            _cargoTypesForm.DoEdit();
            MTNControlBase.SetValueInEditTable(_cargoTypesForm.tblDetails, _rowNames[1], @"1",
                EditRowDataType.CheckBox);
            MTNControlBase.SetValueInEditTable(_cargoTypesForm.tblDetails, _rowNames[2], @"1",
                EditRowDataType.CheckBox);
            _cargoTypesForm.DoSave();

            ValidateUniquenessRows();

            MTNControlBase.SetValueInEditTable(_cargoTypesForm.tblSearch, "Abbreviation", "CT_MK3A004");
            _cargoTypesForm.DoSearch();

            NonBulkBreakBulkValidation();
            
            FormObjectBase.NavigationMenuSelection("Yard Functions|Cargo Storage Areas|Location Enquiry");
            _locationEnquiryForm = new LocationEnquiryForm("Location Enquiry TT1");
            
            MTNControlBase.SetValueInEditTable(_locationEnquiryForm.TblSearcher, "Type of Area", "Gridded Block Stack",
                EditRowDataType.ComboBoxEdit, doDownArrow: true, searchSubStringTo: 2);
            MTNControlBase.SetValueInEditTable(_locationEnquiryForm.TblSearcher, "Terminal Area", "GBRF01",
                EditRowDataType.ComboBoxEdit, doDownArrow: true, searchSubStringTo: 2);
            MTNControlBase.SetValueInEditTable(_locationEnquiryForm.TblSearcher, "Cargo Type", "VPAK53867 DO NOT USE",
                EditRowDataType.ComboBoxEdit, doDownArrow: true, searchSubStringTo: 2);
            _locationEnquiryForm.DoSearch();
            
            _locationEnquiryForm.TblSearchResults1.FindClickRow(
            ["Cargo Type^VPAK53867 DO NOT USE~Voyage^MSCK000010~ID^NAV53768A01"]);
            _locationEnquiryForm.DoMoveIt();

            _cargoMoveItForm = new CargoMoveItForm();
            MTNControlBase.SetValueInEditTable(_cargoMoveItForm.tblMoveDetails, "To Terminal Area", "MKBS09",
                EditRowDataType.ComboBoxEdit, waitTime: 150);

            _cargoMoveItForm.btnMoveIt.DoClick();
            _confirmationFormYesNo = new ConfirmationFormYesNo("Confirm Move - 1 Items");
            _confirmationFormYesNo.btnYes.DoClick();
           
           _locationEnquiryForm.SetFocusToForm();
           _locationEnquiryForm.DoShowHideSearcher();
           
           MTNControlBase.SetValueInEditTable(_locationEnquiryForm.TblSearcher, "Type of Area", "Block Stack",
               EditRowDataType.ComboBoxEdit, doDownArrow: true, searchSubStringTo: 2);
           MTNControlBase.SetValueInEditTable(_locationEnquiryForm.TblSearcher, "Terminal Area", "MKBS09",
               EditRowDataType.ComboBoxEdit, doDownArrow: true, searchSubStringTo: 2);
           
           _locationEnquiryForm.DoSearch();
            
           _locationEnquiryForm.TblSearchResults1.FindClickRow([
               "Cargo Type^VPAK53867 DO NOT USE~Voyage^MSCK000010~ID^NAV53768A01"
           ]);
        }


        private void NonBulkBreakBulkValidation()
        {
            var text = GetTextFromPDF();
            var matching = _rowNames.Where(s => text.Contains(s)).ToList();
            Assert.IsTrue(matching.Count == 0,
                $"TestCase53867::ValidateUniquenessRows - {string.Join(", ", _rowNames)} should NOT be visible.  Expected:  Actual: {string.Join(", ", matching)}");
        }

        private void ValidateUniquenessRows()
        {
            MTNControlBase.ValidateValueInEditTable(_cargoTypesForm.tblDetails, _rowNames[1], "1");
            MTNControlBase.ValidateValueInEditTable(_cargoTypesForm.tblDetails, _rowNames[2], "1");

            var text = GetTextFromPDF();
            var matching = _rowNames.Where(s => text.Contains(s)).ToList();
            Assert.IsTrue(matching.Count == 3,
                $"TestCase53867::ValidateUniquenessRows - {string.Join(", ", _rowNames)} should be the only visible row.  Expected: {string.Join(", ", _rowNames)}    Actual: {string.Join(", ", matching)}");
        }

        void SetAllowDuplicateIdAndValidate()
        {
            MTNControlBase.ValidateValueInEditTable(_cargoTypesForm.tblDetails, _rowNames[0], "1");
            MTNControlBase.ValidateValueInEditTable(_cargoTypesForm.tblDetails, _rowNames[1], "0");
            MTNControlBase.ValidateValueInEditTable(_cargoTypesForm.tblDetails, _rowNames[2], "0");

            var text = GetTextFromPDF();
            var matching = _rowNames.Where(s => text.Contains(s)).ToList();
            Assert.IsTrue(matching.Count == 3,
                $"TestCase53867::SetAllowDuplicateIdAndValidate - {string.Join(", ", _rowNames)} should be the only visible row.  Expected: {string.Join(", ", _rowNames)}    Actual: {string.Join(", ", matching)}");
        }

        private string ValidateVPAK53867OnEntry()
        {
            MTNControlBase.ValidateValueInEditTable(_cargoTypesForm.tblDetails, _rowNames[0], string.Empty);

            var text = GetTextFromPDF();
            var matching = _rowNames.Where(s => text.Contains(s)).ToList();
            Assert.IsTrue(matching.Count == 1 && matching[0] == _rowNames[0],
                $"TestCase53867::ValidateVPAK53867OnEntry - {_rowNames[0]} should be the only visible row.  Expected: {_rowNames[0]}    Actual: {string.Join(", ", matching)}");
            return text;
        }

        string GetTextFromPDF()
        {
            var imagePath = "CargoTypesForm_DetailsTab.png";
            var pdfPath = _saveDirectory + @"CargoTypesForm_DetailsTab.PDF";

            Miscellaneous.CaptureElementAsImage(TestContext, _cargoTypesForm.tblDetails, imagePath);
            var imageConverter = new ImageToPdfConverter();
            imageConverter.ConvertImageToPdf(_saveDirectory + imagePath, pdfPath);
            var converter = new PdfImageToTextConverter();
            var text = converter.ConvertPdfImageToText(pdfPath);
            return text;
        }
        
        
        
        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = "_53768_";

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>VPAK53867 DO NOT USE</cargoTypeDescr>\n            <id>NAV53768A01</id>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>NZCHC</dischargePort>\n            <weight>9999.0000</weight>\n            <imexStatus>EXPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>VPAK53867 DO NOT USE</cargoTypeDescr>\n            <id>NAV53768A02</id>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>NZCHC</dischargePort>\n            <weight>9999.0000</weight>\n            <imexStatus>EXPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");
            
            // Create Cargo OnShip
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>VPAK53867 DO NOT USE</cargoTypeDescr>\n            <id>NAV53768A01</id>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>NZCHC</dischargePort>\n            <locationId>GBRF01 0105</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>EXPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>VPAK53867 DO NOT USE</cargoTypeDescr>\n            <id>NAV53768A02</id>\n            <voyageCode>MSCK000010</voyageCode>\n            <operatorCode>MES</operatorCode>\n            <dischargePort>NZCHC</dischargePort>\n            <locationId>GBRF01 0305</locationId>\n            <weight>9999.0000</weight>\n            <imexStatus>EXPORT</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");
            
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads
    }
    
    
    
}
