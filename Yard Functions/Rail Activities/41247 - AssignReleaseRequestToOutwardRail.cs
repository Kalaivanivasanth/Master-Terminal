using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.Yard_Functions.Rail_Activities;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Rail_Activities
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class Test41247 : MTNBase
    {
        ReleaseRequestForm _releaseRequestForm;
        ReleaseToOutwardRailForm _releaseToOutwardRail;
        RailOperationsForm _railOperationsForm;
      
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();


        private void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();
            CallJadeScriptToRun(TestContext, @"resetData_41247");
            SetupAndLoadInitializeData(TestContext);
        }


        [TestMethod]
        public void RailReleaseRequestAssignToOutwardRail()
        {

            MTNInitialize();

            // Open Gate Functions | Release Requests
            FormObjectBase.MainForm.OpenReleaseRequestFromToolbar();

            _releaseRequestForm = new ReleaseRequestForm(formTitle: @"Release Requests TT1");
            _releaseRequestForm.cmbView.SetValue(@"Active");
            _releaseRequestForm.cmbType.SetValue(@"Rail");

            MTNControlBase.SetValueInEditTable(_releaseRequestForm.tblSearchTable, @"Release No", @"JLG41247RAIL");
            _releaseRequestForm.DoSearch();

            _releaseRequestForm.TblReleaseRequests.GetElement().RightClick();
            _releaseRequestForm.ContextMenuSelect(@"Assign To Outward Rail|Outward Rail RT0001...");

            _releaseToOutwardRail = new ReleaseToOutwardRailForm(@"RT0001 Release To Outward Rail TT1");
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_releaseToOutwardRail.tblReleaseRequests, "Release Number^JLG41247RAIL ISO Container~Total To Process^5");
            _releaseToOutwardRail.TblReleaseRequests.FindClickRow(new[] {"Release Number^JLG41247RAIL ISO Container~Total To Process^5" });

            _releaseToOutwardRail.DoSave();

            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Rail Activities|Rail Operations", forceReset: true);
            _railOperationsForm = new RailOperationsForm();

            _railOperationsForm.radioBtnOutward.Click();

            _railOperationsForm.DoSelectTrainFromToolbar();

            _railOperationsForm.GetSelectRailArea();
            MTNControlBase.FindClickRowInList(_railOperationsForm.lstRailAreas, @"RT0001");

            _railOperationsForm.btnShow.DoClick();

            /*// Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date 
            MTNControlBase.FindClickRowInTable(_railOperationsForm.tblRailOpsItems, @"Release No.^JLG41247RAIL~Count^1/5");
            MTNControlBase.FindClickRowInTable(_railOperationsForm.tblRailOpsItems, @"Release No.^JLG41247RAIL~Count^2/5");
            MTNControlBase.FindClickRowInTable(_railOperationsForm.tblRailOpsItems, @"Release No.^JLG41247RAIL~Count^3/5");
            MTNControlBase.FindClickRowInTable(_railOperationsForm.tblRailOpsItems, @"Release No.^JLG41247RAIL~Count^4/5");
            MTNControlBase.FindClickRowInTable(_railOperationsForm.tblRailOpsItems, @"Release No.^JLG41247RAIL~Count^5/5");*/
            _railOperationsForm.TblRailOpsItems.FindClickRow(new[]
            {
                "Release No.^JLG41247RAIL~Count^1/5", "Release No.^JLG41247RAIL~Count^2/5",
                "Release No.^JLG41247RAIL~Count^3/5", "Release No.^JLG41247RAIL~Count^4/5",
                "Release No.^JLG41247RAIL~Count^5/5",
            });

            // 08/07/2021 - NAVMH5 - The deleting of the jobs is now done in resetData_41247 which deletes the Release Request

        }
        

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_41247_";
            
            //Create Release Request
            CreateDataFileToLoad(@"CreateReleaseRequest.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalRequestMultiLine xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalRequestMultiLine.xsd'>\n   <AllRequestHeader>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <RequestHeader Terminal='TT1'>\n         <releaseByType>true</releaseByType>\n         <messageMode>A</messageMode>\n         <operatorCode>MSL</operatorCode>\n         <releaseRequestNumber>JLG41247RAIL</releaseRequestNumber>\n         <releaseTypeStr>Rail</releaseTypeStr>\n         <statusBulkRelease>Active</statusBulkRelease>\n         <subTerminalCode>Depot</subTerminalCode>\n         <AllRequestDetail>\n            <RequestDetail>\n               <quantity>5</quantity>\n               <cargoTypeDescr>ISO Container</cargoTypeDescr>\n               <isoType>2200</isoType>\n               <releaseRequestNumber>JLG41247RAIL</releaseRequestNumber>\n               <requestDetailID>001</requestDetailID>\n               <fullOrMT>E</fullOrMT>\n            </RequestDetail>\n         </AllRequestDetail>\n      </RequestHeader>\n   </AllRequestHeader>\n</JMTInternalRequestMultiLine>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);

        }

    }
}
