using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Yard_Functions.Yard_Operations;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Release_Request
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40755 : MTNBase
    {

        YardOperationsForm _yardOperationsForm;

       [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup()
        {
           // Step 14
            _yardOperationsForm.SetFocusToForm();
            _yardOperationsForm.ShowToolBarDetails();
            _yardOperationsForm.optJob.Click();
            _yardOperationsForm.ShowJobDetails();

            // Step 15
            /*MTNControlBase.FindClickRowInTable(_yardOperationsForm.tblDetails,
                @"Job ID^RLS40755~Task Type^Move~Total Number^5~Request^MSL 2200~Release No.^RLS40755");*/
            _yardOperationsForm.TblDetailsJob.FindClickRow(new[] {
                "Job ID^RLS40755~Task Type^Move~Total Number^5~Request^MSL 2200~Release No.^RLS40755" });
            //yardOperationsForm.btnDelete.DoClick();
            _yardOperationsForm.DoDelete40755();

            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Deletion");
            confirmationFormYesNo.CheckMessageMatch(@"Are you sure you want to delete Jobs - RLS40755?");
            confirmationFormYesNo.btnYes.DoClick();
           
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void HeavyRatedReleaseRequestComingThroughToYardOperationsJobs()
        {
            MTNInitialize();
            
            // Step 4 
            FormObjectBase.NavigationMenuSelection("Gate Functions|Release Requests");
            ReleaseRequestForm releaseRequestForm = new ReleaseRequestForm(formTitle: @"Release Requests TT1");
            releaseRequestForm.cmbView.SetValue("All", doDownArrow: true, searchSubStringTo: "All".Length - 1);
            releaseRequestForm.cmbType.SetValue("Packing", doDownArrow: true, searchSubStringTo: "All".Length - 1);

            MTNControlBase.SetValueInEditTable(releaseRequestForm.tblSearchTable, @"Release No", @"RLS40755");
            releaseRequestForm.DoSearch();

            // Step 6
            releaseRequestForm.DoEdit40755();
            ReleaseRequestAddForm releaseRequestAddForm = new ReleaseRequestAddForm(@"Editing request RLS40755... TT1");
            releaseRequestAddForm.ClickOnReleaseRequestItem(@"5 x 2200  - GENERAL (0 released )");
            var value = MTNControlBase.GetValueInEditTable(releaseRequestAddForm.tblReleaseRequestData, @"Heavy Duty");
            Assert.IsTrue(value == @"1", @"TestCase40755 - Release Request does not have Heavy Duty selected");

            // Step 8
            releaseRequestAddForm.DoCancel();
            // MTNControlBase.FindClickRowInTable(releaseRequestForm.tblReleaseRequests,
                // @"Status^Active~Rel By Type^Yes~Release No^RLS40755~Tot.Reqd^5~Type^Packing", ClickType.ContextClick);
            releaseRequestForm.TblReleaseRequests.FindClickRow(["Status^Active~Rel By Type^Yes~Release No^RLS40755~Tot.Reqd^5~Type^Packing"], ClickType.ContextClick);            releaseRequestForm.ContextMenuSelect(@"Create Internal Move");

            // Step 10
            YardOperationsMoveJobForm yardOperationsMoveJobForm = new YardOperationsMoveJobForm(@"Add Move Job TT1");
            yardOperationsMoveJobForm.CmbTargetArea.SetValue(TT1.TerminalArea.MKBS01);
            yardOperationsMoveJobForm.btnSave.DoClick();

            // Step 11
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Yard Operations", forceReset:true);
            _yardOperationsForm = new YardOperationsForm();

            // Step 12
            _yardOperationsForm.ShowSearchTabDetails();
            _yardOperationsForm.txtSearcherJobId.SetValue(@"RLS40755");
            _yardOperationsForm.DoSearch();

            // Step 13
            _yardOperationsForm.ShowToolBarDetails();
            _yardOperationsForm.OptWorkList.Click();
            _yardOperationsForm.ShowWorkListDetails();
            /*MTNControlBase.FindClickRowInTable(_yardOperationsForm.tblDetails,
                @"Cargo ID^MSL 2200~Cargo Type^ISO Container~ISO Type^2200~Operator^MSL~Job ID^RLS40755");*/
            _yardOperationsForm.TblDetailsWorkList.FindClickRow(new[] {
                "Cargo ID^MSL 2200~Cargo Type^ISO Container~Operator^MSL~Job ID^RLS40755" });
                //"Cargo ID^MSL 2200~Cargo Type^ISO Container~ISO Type^2200~Operator^MSL~Job ID^RLS40755" });

            MTNControlBase.ValidateDataInInfoTable(_yardOperationsForm.tblInfo, @"Heavy Rated^Yes");
                        
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_40755_";

            // Release Request Delete
            CreateDataFileToLoad(@"ReleaseRequestDelete.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalRequestMultiLine xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalRequestMultiLine.xsd'>\n   <AllRequestHeader>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <RequestHeader Terminal='TT1'>\n         <releaseByType>true</releaseByType>\n         <messageMode>D</messageMode>\n         <operatorCode>MSL</operatorCode>\n         <releaseRequestNumber>RLS40755</releaseRequestNumber>\n         <releaseTypeStr>Packing</releaseTypeStr>\n         <statusBulkRelease>Active</statusBulkRelease>\n         <subTerminalCode>Depot</subTerminalCode>\n         <AllRequestDetail>\n            <RequestDetail>\n               <quantity>5</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n               <isoType>2200</isoType>\n               <releaseRequestNumber>RLS40755</releaseRequestNumber>\n               <requestDetailID>001</requestDetailID>\n			   <heavyDuty>true</heavyDuty>\n               <fullOrMT>E</fullOrMT>\n            </RequestDetail>\n         </AllRequestDetail>\n      </RequestHeader>\n   </AllRequestHeader>\n</JMTInternalRequestMultiLine>\n\n");

            // Release Request Create
            CreateDataFileToLoad(@"ReleaseRequestCreate.xml",
               "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalRequestMultiLine xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalRequestMultiLine.xsd'>\n   <AllRequestHeader>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <RequestHeader Terminal='TT1'>\n         <releaseByType>true</releaseByType>\n         <messageMode>A</messageMode>\n         <operatorCode>MSL</operatorCode>\n         <releaseRequestNumber>RLS40755</releaseRequestNumber>\n         <releaseTypeStr>Packing</releaseTypeStr>\n         <statusBulkRelease>Active</statusBulkRelease>\n         <subTerminalCode>Depot</subTerminalCode>\n         <AllRequestDetail>\n            <RequestDetail>\n               <quantity>5</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n               <isoType>2200</isoType>\n               <releaseRequestNumber>RLS40755</releaseRequestNumber>\n               <requestDetailID>001</requestDetailID>\n			   <heavyDuty>true</heavyDuty>\n               <fullOrMT>E</fullOrMT>\n            </RequestDetail>\n         </AllRequestDetail>\n      </RequestHeader>\n   </AllRequestHeader>\n</JMTInternalRequestMultiLine>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
