using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using DataObjects.LogInOutBO;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43570 : MTNBase
    {
        CustomsActionsForm customsActionsForm;

        const string ImpFile1 = "M_43750_CustomsFileImport.csv";

        string[] rowsToFind1 = new[] { "File Type^NZ Customs TSW Response~Name^ADD MPI WASH~Keyword^111~Key File Segment^FTX+DIN~Action^Add~Data^ToDo Task WASH" };
        string[] rowsToFind2 = new[] { "File Type^NZ Customs TSW Response~Name^ADD MPI DAMAGE~Keyword^43570~Key File Segment^Clearance~Action^Add~Data^Stop Damage" };

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            CreateDataFile(ImpFile1,
                "File Type,Name,Keyword,Key File Segment,Action,Data\nNZ Customs TSW Response,ADD MPI WASH,111,FTX+DIN,Add,ToDo Task WASH\n");

            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void CustomsActionsExport()
        {
            
            MTNInitialize();

            string expFile1 = "M_43750_" + GetUniqueId() + ".csv";

            // 1. Navigate to Agent Debtor Maintenace and open the NZ Customs TSW File Types
            FormObjectBase.MainForm.OpenCustomsActionsFromToolbar();
            customsActionsForm = new CustomsActionsForm();
            customsActionsForm.cmbFileType.SetValue(@"NZ Customs TSW Response", doTab: false);
            customsActionsForm.DoSearch();

            // 2. delete existing entries if there are any.
            customsActionsForm.DeleteEntryFromTable(rowsToFind1);
            customsActionsForm.DeleteEntryFromTable(rowsToFind2);

            // 3. Import the file created 
            customsActionsForm.ImportFile(dataDirectory + ImpFile1);

            // 4. Check that the import has imported the data correctly
            customsActionsForm.tblCustomsActions.FindClickRow(new[] { "File Type^NZ Customs TSW Response~Name^ADD MPI WASH~Keyword^111~Key File Segment^FTX+DIN~Action^Add~Data^ToDo Task WASH" });

            //5. Export this data
            customsActionsForm.ExportFile(dataDirectory + expFile1);

            //6. Add an extra row the exported file
            Miscellaneous.AddDataRowToFile(dataDirectory + expFile1, @"NZ Customs TSW Response,ADD MPI DAMAGE,43570,Clearance,Add,Stop Damage");

            //7. Delete the current entry and reimport the exported file (with the additional row in it)
            customsActionsForm.DeleteEntryFromTable(rowsToFind1);
            customsActionsForm.ImportFile(dataDirectory + expFile1);

            //8. check the 2 rows of data get imported. This will assert that the export worked as expected.
            customsActionsForm.tblCustomsActions.FindClickRow(rowsToFind1);
            customsActionsForm.tblCustomsActions.FindClickRow(rowsToFind2); 

            //9. May as well clean up in the test while we're here.
            customsActionsForm.DeleteEntryFromTable(rowsToFind1);
            customsActionsForm.DeleteEntryFromTable(rowsToFind2);
        }



    }

}
