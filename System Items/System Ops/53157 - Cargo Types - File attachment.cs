using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase53157 : MTNBase
    {
        private CargoTypesForm _cargoTypesForm;
        private FileAttachment _fileAttachment;
        private FileAttachmentPopup _fileAttachmentPopup;
        private FileUploadWindow _fileUpload;

        private const string TestCaseNumber = @"53157";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();
        
        private void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();

            InitializeDetails();
        }

        [TestMethod]
        public void VerifyFileAttachmentFunction()
        {
            MTNInitialize();
            
            // Step 3 - 4
            //_cargoTypesForm = new CargoTypesForm();
            //_cargoTypesForm.btnFileAttachment.DoClick();
            _cargoTypesForm.DoFileAttachment();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            _fileAttachment = new FileAttachment();

            _fileAttachment.btnLoad.DoClick();
            
            //@"File Load for BXT Bauxite Generic"
            _fileAttachmentPopup = new FileAttachmentPopup(@"File Load for BXT Bauxite Generic");
            _fileAttachmentPopup.BtnBrowse.DoClick();

            _fileUpload = new FileUploadWindow(@"Open");
            _fileUpload.EnterFilePath(@"53157\file.txt");

            //_fileAttachmentPopup = new FileAttachmentPopup(@"File Load for BXT Bauxite Generic");
            _fileAttachmentPopup.SetFocusToForm();
            _fileAttachmentPopup.BtnOK.DoClick();

            // Step 5
            _fileAttachment = new FileAttachment();
            // MTNControlBase.FindClickRowInTable(_fileAttachment.tblAttachmentDetails, $"File Name^file.txt", ClickType.Click);
            _fileAttachment.TblAttachmentDetails.FindClickRow([$"File Name^file.txt"], ClickType.Click);
            
            //Step 6
            _fileAttachment.btnDelete.DoClick(1000);
            ConfirmationFormYesNo confirmationForm = new ConfirmationFormYesNo("Confirm Delete",
                automationIdMessage: "1", automationIdYes: "3", automationIdNo: "4");
            confirmationForm.ConfirmationFormYes();
            
            //Step 7
            var filesNotFound = _fileAttachment.CheckFilesExistsInTable(new[] { "file.txt" }, false);
            Assert.IsTrue(!string.IsNullOrEmpty(filesNotFound), "Test Case 53157 - 'file.txt' should have been deleted");

        }

        

        private void InitializeDetails()
        {
            FormObjectBase.NavigationMenuSelection(@"System Ops|Cargo Types");
            _cargoTypesForm = new CargoTypesForm();

            //_cargoTypesForm.txtFilter.SetValue("BXT", 50);
            
            //_cargoTypesForm.GetCargoDetailsTable();
            //
            
            MTNControlBase.SetValueInEditTable(_cargoTypesForm.tblSearch, @"Abbreviation", "BXT");
            _cargoTypesForm.DoSearch();
            
            _cargoTypesForm.tblCargoDetails1.FindClickRow(new string[] { "Abbreviation^BXT~Description^Bauxite Generic~Track As^Bulk" });
            
        }

    }

}
