using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Logs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using System;
using System.Drawing;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Logs
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43932 : MTNBase
    {

        LogVoyageOperationsForm _logVoyageOperationsForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void CreateAndEditALogVoyage()
        {
            MTNInitialize();
            
            // Step 1
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry");
            VoyageEnquiryForm voyageEnquiryForm = new VoyageEnquiryForm();

            // Step 2
            voyageEnquiryForm.DeleteVoyageByVoyageCode(@"VOY43932");
           
            // Step 4
            FormObjectBase.NavigationMenuSelection(@"Log Functions|Log Voyage Functions|Log Voyage Operations", forceReset: true);
            _logVoyageOperationsForm = new LogVoyageOperationsForm(@"Log Voyage Operations TT1");
            _logVoyageOperationsForm.GetVoyageAdminDetails();

            // Step 5
            _logVoyageOperationsForm.btnVoyageAdminNew.DoClick();

            // Step 6
            MTNControlBase.SetValueInEditTable(_logVoyageOperationsForm.tblVoyageAdminVoyageDetails, @"Voyage Code", @"VOY43932");
            MTNControlBase.SetValueInEditTable(_logVoyageOperationsForm.tblVoyageAdminVoyageDetails, @"Vessel Name", @"MSCK");
            MTNControlBase.SetValueInEditTable(_logVoyageOperationsForm.tblVoyageAdminVoyageDetails, @"Hatches", @"2");

            // Step 7
            _logVoyageOperationsForm.unassignedAssigned.MakeSureInCorrectList(_logVoyageOperationsForm.lstAssigned, 
                new [] { "Test Terminal 1 - Non Cash" });

            // Step 8
            _logVoyageOperationsForm.btnVoyageAdminSave.DoClick();

            // Step 9
            //MTNControlBase.FindTabOnForm(logVoyageOperationsForm.tabVoyageAdmin, @"Voyage");
            _logVoyageOperationsForm.GetVoyageTabDetails();
            MTNControlBase.FindClickRowInTable(_logVoyageOperationsForm.tblVoyageTerminalPortDetails,
                @"Status^Pending", countOffset: -1);

            // Step 10
            _logVoyageOperationsForm.btnVoyageEdit.DoClick();

            _logVoyageOperationsForm.tblVoyageTerminalPortDetails.Focus();
            Point point = new Point(_logVoyageOperationsForm.tblVoyageTerminalPortDetails.BoundingRectangle.X + 150,
                _logVoyageOperationsForm.tblVoyageTerminalPortDetails.BoundingRectangle.Y + 20);
            Mouse.Click(point);
            for (int times = 1; times <= 12; times++)
            {
                Keyboard.Press(VirtualKeyShort.TAB);
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(400));
            }
            var date = DateTime.Now;
            
            Keyboard.Type(@"Archived");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(100));

            _logVoyageOperationsForm.btnVoyageSave.DoClick();

            // Step 11
            voyageEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(voyageEnquiryForm.tblVoyages,
                // $"Code^VOY43932~Vessel Name^MSCK~Voyage Type^Log Structure~Working Status^Archived~Archived Date/Time^{date:dd/MM/yyyy}");
            voyageEnquiryForm.TblVoyages.FindClickRow([$"Code^VOY43932~Vessel Name^MSCK~Voyage Type^Log Structure~Working Status^Archived~Archived Date/Time^{date:dd/MM/yyyy}"]);
            
    
        }

   
    }

}
