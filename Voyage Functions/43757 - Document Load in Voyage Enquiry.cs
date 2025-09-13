using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Voyage_Functions;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43757 : MTNBase
    {

        VoyageEnquiryForm _voyageEnquiryForm;
        VoyageDefinitionForm _voyageDefinitionForm;
        DocumentLoadForm _documentLoadForm;
        FileAttachmentsForm _fileAttachmentsForm;
        ConfirmationFormYesNo _confirmationForm;

        const string LoadFile1 = "M_43757_LoadDocument_1.txt";
        const string LoadFile2 = "M_43757_LoadDocument_2.txt";
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CreateDataFile(LoadFile1, "Test Document for 43757 - Voyage BISL43757A");
            CreateDataFile(LoadFile2, "Test Document for 43757 - Voyage HHRV10001");

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void LoadVoyageDocuments()
        {
            MTNInitialize();

            var uniqueId = $"43757{GetUniqueId()}";
            
            // 1. Navigate to Voyage enquiry form and find voyage BISL43757A - double click to open
            //FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry");
            FormObjectBase.MainForm.OpenVoyageEnquiryFromToolbar();
            _voyageEnquiryForm = new VoyageEnquiryForm();

            _voyageEnquiryForm.FindVoyageByVoyageCode(TT1.Voyage.BISL43757A);
            // MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages,$"Code^{TT1.Voyage.BISL43757A}",ClickType.DoubleClick);
            _voyageEnquiryForm.TblVoyages.FindClickRow([$"Code^{TT1.Voyage.BISL43757A}"], ClickType.DoubleClick);

            // 2. go to documents tab and load a new document
            _voyageDefinitionForm = new VoyageDefinitionForm();
            _voyageDefinitionForm.DoViewDocuments();
            _fileAttachmentsForm = new FileAttachmentsForm(@"File Attachments - BISL43757A Bermuda islander 43757 TT1");
            _fileAttachmentsForm.btnLoad.DoClick();

            _documentLoadForm = new DocumentLoadForm(@"File Load for BISL43757A TT1");
            Console.WriteLine($"Loading file: {dataDirectory}{LoadFile1}");
            _documentLoadForm.LoadFile(dataDirectory + LoadFile1);

            _documentLoadForm.txtHeading.SetValue(@"Test43757A");

            _documentLoadForm.txtDescription.SetValue(@"Text Document for Test 43757A " + uniqueId);

            _documentLoadForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_fileAttachmentsForm.tblDocuments, @"Heading^Test43757A~File Name^" + LoadFile1 +"~Description^Text Document for Test 43757A " + uniqueId, rowHeight: 16);
            _fileAttachmentsForm.TblDocuments.FindClickRow([
                $"Heading^Test43757A~File Name^{LoadFile1}~Description^Text Document for Test 43757A {uniqueId}"
            ]);
            _fileAttachmentsForm.CloseForm();

            // 4. go to avoyage HHRV10001 and load another document
            _voyageDefinitionForm.cmbVoyageCodeCombo.SetValue("HYUNDAI Victory - (HHRV10001)", doDownArrow: true, searchSubStringTo: 14);
            _voyageDefinitionForm.DoViewDocuments();
            _fileAttachmentsForm = new FileAttachmentsForm(@"File Attachments - HHRV10001 HYUNDAI Victory TT1");
            _fileAttachmentsForm.btnLoad.DoClick();

            _documentLoadForm = new DocumentLoadForm(@"File Load for HHRV10001 TT1");
            _documentLoadForm.LoadFile(dataDirectory + LoadFile2);

            _documentLoadForm.txtHeading.SetValue(@"Test43757B");
            _documentLoadForm.txtDescription.SetValue(@"Text Document for Test 43757B " + uniqueId);
            
            _documentLoadForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_fileAttachmentsForm.tblDocuments, @"Heading^Test43757B~File Name^" + LoadFile2 +"~Description^Text Document for Test 43757B " + uniqueId, rowHeight: 16);
            _fileAttachmentsForm.TblDocuments.FindClickRow(["Heading^Test43757B~File Name^" + LoadFile2 +"~Description^Text Document for Test 43757B " + uniqueId]);
            _fileAttachmentsForm.CloseForm();

            //6. go back to voyage BISL43757A and delete loaded file
            _voyageDefinitionForm.cmbVoyageCodeCombo.SetValue(@"Bermuda islander 43757 - (BISL43757A)");
            _voyageDefinitionForm.DoViewDocuments();
            _fileAttachmentsForm = new FileAttachmentsForm(@"File Attachments - BISL43757A Bermuda islander 43757 TT1");
            // MTNControlBase.FindClickRowInTable(_fileAttachmentsForm.tblDocuments, @"Heading^Test43757A~File Name^"+ LoadFile1 +"~Description^Text Document for Test 43757A " + uniqueId, ClickType.Click, xOffset: 180);
            _fileAttachmentsForm.TblDocuments.FindClickRow(["Heading^Test43757A~File Name^"+ LoadFile1 +"~Description^Text Document for Test 43757A " + uniqueId], ClickType.Click, xOffset: 180);
            _fileAttachmentsForm.btnDelete.DoClick();
            _confirmationForm = new ConfirmationFormYesNo(formTitle: @"Confirm Delete", automationIdMessage: @"2", automationIdYes: @"3", automationIdNo: @"4");
            _confirmationForm.btnYes.DoClick();

            // validate file has been deleted
            bool noData = MTNControlBase.FindClickRowInTable(_fileAttachmentsForm.TblDocuments.GetElement(), @"Heading^Test43757A~File Name^" + LoadFile1 + "~Description^Text Document for Test 43757A " + uniqueId, rowHeight: 16, doAssert: false);
            Assert.IsTrue(noData == false, "Document Test43757A has not been deleted");

            _fileAttachmentsForm.CloseForm();

            //6. go back to voyage HHRV00001 and delete loaded file
            _voyageDefinitionForm.cmbVoyageCodeCombo.SetValue("HYUNDAI Victory - (HHRV10001)", doDownArrow: true, searchSubStringTo: 14);
            _voyageDefinitionForm.DoViewDocuments();

            _fileAttachmentsForm = new FileAttachmentsForm(@"File Attachments - HHRV10001 HYUNDAI Victory TT1");
            // MTNControlBase.FindClickRowInTable(_fileAttachmentsForm.tblDocuments, @"Heading^Test43757B~File Name^" + LoadFile2 + "~Description^Text Document for Test 43757B " + uniqueId, ClickType.Click, xOffset: 180);
            _fileAttachmentsForm.TblDocuments.FindClickRow(["Heading^Test43757B~File Name^" + LoadFile2 + "~Description^Text Document for Test 43757B " + uniqueId], ClickType.Click, xOffset: 180);
            _fileAttachmentsForm.btnDelete.DoClick();

            _confirmationForm = new ConfirmationFormYesNo(formTitle: @"Confirm Delete", automationIdMessage: @"2", automationIdYes: @"3", automationIdNo: @"4");
            _confirmationForm.btnYes.DoClick();
            // noData = MTNControlBase.FindClickRowInTable(_fileAttachmentsForm.tblDocuments, @"Heading^Test43757B~File Name^" + LoadFile2 +"~Description^Text Document for Test 43757B " + uniqueId, rowHeight: 16, doAssert:false);
            _fileAttachmentsForm.TblDocuments.FindClickRow(["Heading^Test43757B~File Name^" + LoadFile2 +"~Description^Text Document for Test 43757B " + uniqueId], doAssert:false);
            Assert.IsTrue(noData == false, "Document Test43757B has not been deleted");

        }



    }

}
