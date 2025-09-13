using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNForms.FormObjects;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40953 : MTNBase
    {
        CustomsActionsForm customsActionsForm;

        const string ImpFile1 = "M_40953_CustomsFileImport.csv";

        string[] rowsToFind1 = new[] { "File Type^NZ Customs TSW Response~Name^ADD MPI WASH~Keyword^MPI WASH.~Key File Segment^FTX+DIN~Action^Add~Data^ToDo Task WASH" };
        string[] rowsToFind2 = new[] { "File Type^NZ Customs TSW Response~Name^NZCS CUST DOC REMOVE 807~Keyword^807~Key File Segment^Clearance~Action^Remove~Data^Stop STOP_39019" };
        string[] rowsToFind3 = new[] { "File Type^NZ Customs TSW Response~Name^NZCS CUST DOC REMOVE 819~Keyword^819~Key File Segment^Clearance~Action^Remove~Data^Stop STOP_40797" };

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CreateDataFile(ImpFile1,
                "File Type,Name,Keyword,Key File Segment,Action,Data\nNZ Customs TSW Response,ADD MPI WASH,MPI WASH.,FTX+DIN,Add,ToDo Task WASH\nNZ Customs TSW Response,NZCS CUST DOC REMOVE 807,807,Clearance,Remove,Stop STOP_39019\nNZ Customs TSW Response,NZCS CUST DOC REMOVE 819,819,Clearance,Remove,Stop STOP_40797\n");

            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void CustomsActionsImport()
        {
            MTNInitialize();
            
            string expFile1 = "M_40953_" + GetUniqueId() + ".csv";

            // 1. Navigate to Agent Debtor Maintenace and open the NZ Customs TSW File Types
            //FormObjectBase.NavigationMenuSelection(@"System Ops|Customs Actions");
            FormObjectBase.MainForm.OpenCustomsActionsFromToolbar();
            customsActionsForm = new CustomsActionsForm();
            //MTNControlBase.SetValue(customsActionsForm.cmbFileType, @"NZ");
            customsActionsForm.cmbFileType.SetValue(@"NZ Customs TSW Response", doTab: false);
            //customsActionsForm.btnSearch.DoClick();
            customsActionsForm.DoSearch();
            //customsActionsForm.GetCustomsActionsTable();

            // 2. delete existing entries if there are any.
            customsActionsForm.DeleteEntryFromTable(rowsToFind1);
            customsActionsForm.DeleteEntryFromTable(rowsToFind2);
            customsActionsForm.DeleteEntryFromTable(rowsToFind3);
            // 3. Import the file created 
            customsActionsForm.ImportFile(dataDirectory + ImpFile1);

            // 4. Check that the import has imported the data correctly
            //MTNControlBase.FindClickRowInTable(customsActionsForm.tblCustomsActions, @"File Type^NZ Customs TSW Response~Name^ADD MPI WASH~Keyword^MPI WASH.~Key File Segment^FTX+DIN~Action^Add~Data^ToDo Task WASH", rowHeight: 16);
            //MTNControlBase.FindClickRowInTable(customsActionsForm.tblCustomsActions, @"File Type^NZ Customs TSW Response~Name^NZCS CUST DOC REMOVE 807~Keyword^807~Key File Segment^Clearance~Action^Remove~Data^Stop STOP_39019", rowHeight: 16);
            //MTNControlBase.FindClickRowInTable(customsActionsForm.tblCustomsActions, @"File Type^NZ Customs TSW Response~Name^NZCS CUST DOC REMOVE 819~Keyword^819~Key File Segment^Clearance~Action^Remove~Data^Stop STOP_40797", rowHeight: 16);
            customsActionsForm.tblCustomsActions.FindClickRow(rowsToFind1);
            customsActionsForm.tblCustomsActions.FindClickRow(rowsToFind2);
            customsActionsForm.tblCustomsActions.FindClickRow(rowsToFind3);

            //9. May as well clean up in the test while we're here.
            //customsActionsForm.DeleteEntryFromTable(@"File Type^NZ Customs TSW Response~Name^ADD MPI WASH~Keyword^MPI WASH.~Key File Segment^FTX+DIN~Action^Add~Data^ToDo Task WASH");
            //customsActionsForm.DeleteEntryFromTable(@"File Type^NZ Customs TSW Response~Name^NZCS CUST DOC REMOVE 807~Keyword^807~Key File Segment^Clearance~Action^Remove~Data^Stop STOP_39019");
            //customsActionsForm.DeleteEntryFromTable(@"File Type^NZ Customs TSW Response~Name^NZCS CUST DOC REMOVE 819~Keyword^819~Key File Segment^Clearance~Action^Remove~Data^Stop STOP_40797");
            customsActionsForm.DeleteEntryFromTable(rowsToFind1);
            customsActionsForm.DeleteEntryFromTable(rowsToFind2);
            customsActionsForm.DeleteEntryFromTable(rowsToFind3);
        }



    }

}
