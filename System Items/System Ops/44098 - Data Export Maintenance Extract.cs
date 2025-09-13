using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNUtilityClasses.Classes;
using System;
using System.IO;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44098 : MTNBase
    {
        DataExportMaintenanceForm _dataExportMaintenanceForm;
            
        const string PartialName = "Commodity_Master_Export01Commodity";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(dataDirectory);
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + PartialName + "*");

            foreach (FileSystemInfo foundFile in filesAndDirs)
                foundFile.Delete();

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void DataExtractCommodityObject()
        {
            
            MTNInitialize();

            //string expFile1 = "M_40953_" + GetUniqueID() + ".csv";

            // 1. Navigate to Agent Debtor Maintenace and open the NZ Customs TSW File Types
            FormObjectBase.NavigationMenuSelection(@"System Ops|Data Export Maintenance");


            _dataExportMaintenanceForm = new DataExportMaintenanceForm();
            // MTNControlBase.FindClickRowInTable(_dataExportMaintenanceForm.tblExtractDefintion, @"Description^Commodity Master Export");
            _dataExportMaintenanceForm.TblExtractDefintion.FindClickRow(["Description^Commodity Master Export"]);
            _dataExportMaintenanceForm.ShowXMLFileTab();
            //dataExportMaintenanceForm.btnEdit.DoClick();
            _dataExportMaintenanceForm.DoEdit2();
            MTNControlBase.SetValueInEditTable(_dataExportMaintenanceForm.tblXMLFileDetails, @"Terminal", @"Test Terminal 1 - Non Cash",rowDataType: EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(_dataExportMaintenanceForm.tblXMLFileDetails, @"Folder Name", dataDirectory);
            //dataExportMaintenanceForm.btnSave.DoClick();
            _dataExportMaintenanceForm.DoSave2();
            //MTNControlBase.FindTabOnForm(dataExportMaintenanceForm.tabXMLFile, @"Select Object");
            _dataExportMaintenanceForm.ShowSelectObjectTab();
            //MTNControlBase.SetValue(dataExportMaintenanceForm.cmbExtractClass, @"CM_Commodity:Commodity");
            _dataExportMaintenanceForm.cmbExtractClass.SetValue(@"CM_Commodity:Commodity");

            string[] objectsToCheckFor = 
            {
                "(MT)  Empty",
                "(REEF)  Reefer",
                "(TIMB)  Timber"
            };
            _dataExportMaintenanceForm.listSelect.MakeSureInCorrectList(_dataExportMaintenanceForm.lstAvailableObjects, objectsToCheckFor);
            _dataExportMaintenanceForm.btnExtract.DoClick();

            // wait 2 seconds for the extract
            Wait.UntilInputIsProcessed(waitTimeout: TimeSpan.FromSeconds(2));

            string partialName = "Commodity_Master_Export01Commodity";

            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(dataDirectory);
            FileSystemInfo[] filesAndDirs = hdDirectoryInWhichToSearch.GetFileSystemInfos("*" + partialName + "*");

            string fileText = "";
            foreach (FileSystemInfo foundFile in filesAndDirs)
            {
                fileText = File.ReadAllText(foundFile.FullName);
                break;
            }


            Assert.IsTrue(fileText.Contains(@"<code>MT</code>"), "Extracted File expected to contain <code>MT</code>");
            Assert.IsTrue(fileText.Contains(@"<description>Empty</description>"), "Extracted File expected to contain <description>Empty</description>");
            Assert.IsTrue(fileText.Contains(@"<code>REEF</code>"), "Extracted File expected to contain <code>REEF</code>");
            Assert.IsTrue(fileText.Contains(@"<description>Reefer</description>"), "Extracted File expected to contain <description>Reefer</description>");
            Assert.IsTrue(fileText.Contains(@"<code>TIMB</code>"), "Extracted File expected to contain <code>TIMB</code>");
            Assert.IsTrue(fileText.Contains(@"<description>Timber</description>"), "Extracted File expected to contain <description>Timber</description>");


        }



    }

}
