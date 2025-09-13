using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNUtilityClasses.Classes;
using System.IO;
using DataObjects.LogInOutBO;
using MTNGlobal.Classes;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44040 : MTNBase
    {
        InvoiceTransactionMapForm transactionMapForm;
        ConfirmationFormYesNo confirmationForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            searchFor = "_44040_";
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void ImportHandlesZeroRangesInTransactionMap()
        {
            MTNInitialize();
            
            string filename = "M_44040A.csv";
            string directory = string.Concat(Directory.GetCurrentDirectory(), TestContext.GetRunSettingValue(@"CompareUploadFilesDirectory"));
            directory = directory.Replace(@".\", @"\");

            // Step 3 Open Invoice Functions | Maintenance | Transaction Map
            FormObjectBase.NavigationMenuSelection(@"Invoice Functions|Maintenance|Transaction Map");

            transactionMapForm = new InvoiceTransactionMapForm();

            // Step 4 In the Searcher, select Cargo and click the Search button 
            transactionMapForm.chkCargo.DoClick();
            transactionMapForm.chkEvent.DoClick(false);
            transactionMapForm.chkVoyage.DoClick(false);
            transactionMapForm.chkSnapshotSummary.DoClick(false);
            transactionMapForm.chkCargoAllocation.DoClick(false);
            transactionMapForm.chkBillOfLading.DoClick(false);
            //transactionMapForm.btnSearch.DoClick();
            transactionMapForm.DoSearch();

            // Step 5 Click on the adjacent triangle symbol at the left of the Cargo node
            // Step 6 Select Default (Active) transaction map and click the Load from CSV file button in the toolbar
            transactionMapForm.lstTransactionMaps.FindItemInList(@"Cargo|Default (Active)"); //, hasExpandIcon: true);
            
            // Step 7 Select the file 44040.csv to load
            // Step 8 Click the Open button and check that the file is loaded without any errors in to the Transaction Map  
            transactionMapForm.LoadFromCSV(directory + filename);

            // Step 9 Double-Click on the cargo type Bag of Sand and check whether the 0 to 0 quantity range is present in the transaction map
            transactionMapForm.lstTransactionMaps.FindItemInList(@"Bag of Sand", doDoubleClick: true); 
            transactionMapForm.lstTransactionMaps.FindItemInList(@"0 to 0"); 
                        
            // Step 10 Select the cargo type Bag of Sand and click the Delete button in the toolbar
            transactionMapForm.lstTransactionMaps.FindItemInList(@"Bag of Sand");
            Mouse.Click();
            //transactionMapForm.btnDelete.DoClick();
            transactionMapForm.DoDelete();

            // Step 11 Click the Yes button
            confirmationForm = new ConfirmationFormYesNo(@"Confirm Delete");
            confirmationForm.btnYes.DoClick();
        }
    }
}
