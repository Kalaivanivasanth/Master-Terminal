using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions.Reefer;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;
using Org.BouncyCastle.Asn1.Tsp;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Reefer_Activities_Reefer_API_App
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase50160 : MTNBase
    {
        
        private ReeferMonitoringForm _reeferMonitoringForm;
        private ReeferWalkPathDefinitionForm _reeferWalkPathDefinitionForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
     
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"resetData_50160");

            // Thursday, 30 January 2025 navmh5 MTNSignon(TestContext, userName);
            userName = "REEFTT3";
            LogInto<MTNLogInOutBO>(userName);
        }


        [TestMethod]
        public void DeleteReeferWalkPaths()
        {
            MTNInitialize();
            
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Reefer Activities|Reefer Monitoring");
            _reeferMonitoringForm = new ReeferMonitoringForm();

            _reeferMonitoringForm.DoWalkPathDefinition();

            _reeferWalkPathDefinitionForm = new ReeferWalkPathDefinitionForm();
            _reeferWalkPathDefinitionForm.cmbGriddedArea.SetValue(@"RFPMGT");
            string[] detailsToMove =
            {
                 "RFPMGT 1001 1",
                 "RFPMGT 1001 2",
                 "RFPMGT 1001 3",
                 "RFPMGT 1201 1",
                 "RFPMGT 1201 2",
                 "RFPMGT 1201 3",
                 "RFPMGT 1401 1",
                 "RFPMGT 1401 2",
                 "RFPMGT 1401 3"
            };
            AddReeferWalkPath(@"50160ROW1", detailsToMove);
            _reeferWalkPathDefinitionForm.btnDelete.DoClick();

            bool matchFound = MTNControlBase.FindClickRowInTable(_reeferWalkPathDefinitionForm.TblBlockName.GetElement(),
                @"Reefer Walk - Block Name^50160ROW1", rowHeight: 16, doAssert: false);

            Assert.IsFalse(matchFound, @"TestCase50160::DeleteReeferWalkPaths - Row 50160ROW1 should not exist in " +
                @"Reefer Walk - Block Name");

        }

       

        private void AddReeferWalkPath(string walkPathName, string[] detailsToMove)
        {
           _reeferWalkPathDefinitionForm.btnAdd.DoClick();
            _reeferWalkPathDefinitionForm.txtBlockName.SetValue(walkPathName);
            
            _reeferWalkPathDefinitionForm.positionsLists.MoveItemsBetweenList(
                _reeferWalkPathDefinitionForm.positionsLists.LstLeft, detailsToMove);
            _reeferWalkPathDefinitionForm.btnUpdate.DoClick();

            if (walkPathName.Length > 4)
            {
                /*// Thursday, 30 January 2025 navmh5 
                warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Reefer Walk Path TT3");
                string[] warningErrorToCheck = new []
                {
                    "Code :91531. Name '" + walkPathName + "' is too big. Entering more than 4 characters may cause the Walk Path " +
                    @"to not display correctly in the Reefer Monitor application"
                };
                warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
                warningErrorForm.btnSave.DoClick();*/
                WarningErrorForm.CompleteWarningErrorForm("Warnings for Reefer Walk Path TT3", new []
                {
                    $"Code :91531. Name '{walkPathName}' is too big. Entering more than 4 characters may cause the Walk Path to not display correctly in the Reefer Monitor application"
                });
            }
            // MTNControlBase.FindClickRowInTable(_reeferWalkPathDefinitionForm.tblBlockName, @"Reefer Walk - Block Name^" + walkPathName,
                // rowHeight: 16);
            _reeferWalkPathDefinitionForm.TblBlockName.FindClickRow(["Reefer Walk - Block Name^" + walkPathName]);
            _reeferWalkPathDefinitionForm.positionsLists.ValidateInCorrectList(
                _reeferWalkPathDefinitionForm.positionsLists.LstRight, detailsToMove);
        }

      
    }

}
