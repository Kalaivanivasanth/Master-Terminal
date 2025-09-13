using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNUtilityClasses.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41189 : MTNBase
    {
        InvoiceTransactionMapForm _transactionMapForm;
        ConfirmationFormYesNo _confirmationForm;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_41189_";
            BaseClassInitialize_New(testContext);
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }


        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void ExportImportTransactionMapsCorrectly()
        {
            
            MTNInitialize();
            
            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_41189");
            
            string filename = "M_41189A_" + GetUniqueId() + ".csv";
            string directory = string.Concat(Directory.GetCurrentDirectory(), TestContext.GetRunSettingValue(@"CompareUploadFilesDirectory"));
            directory = directory.Replace(@".\", @"\");

            // Step 2 Open Invoice Functions | Maintenance | Transaction Map 
            OpenTransactionMapForm(@"Invoice Functions|Maintenance|Transaction Map", @"Cargo|Test41189");

            // Step 5 Select Test41189 transaction map and click the Extract To CSV file button in the toolbar
            // Step 6 Enter the location to save the output .csv file, enter the File Name - 41189A.csv and click the Save button
            _transactionMapForm.ExtractToCSV(directory + filename);

            
            // Step 7 Open 41189A.csv file
            // Step 8 Change the Transaction Map name to 41189, press (Ctrl+S) to save the changes and close the file
            
            List<string> lines = new List<string>();

            if (File.Exists(directory + filename)) 
            {
                using (StreamReader reader = new StreamReader(directory + filename))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(","))
                        {
                            string[] split = line.Split(',');

                            if (split[1].Contains("Test41189"))
                            {
                                split[1] = "41189";
                                line = string.Join(",", split);
                            }
                        }

                        lines.Add(line);
                    }
                }

                using (StreamWriter writer = new StreamWriter(directory + filename, false, Encoding.ASCII))
                {
                    foreach (string line in lines)
                        writer.WriteLine(line);
                }
            }

            // Step 9 Go back to Invoice Functions | Maintenance | Transaction Map
            _transactionMapForm.SetFocusToForm();

            // Step 10 Click the Load From CSV file button in the toolbar
            // Step 11 Select the file 41189A.csv from the location
            _transactionMapForm.LoadFromCSV(directory + filename);

            
            // Step 12 Click the Open button and check for the new Transaction Map 41189 is displayed in the Transaction Map screen
            // Step 13 Double-Click on the 41189 to expand the transaction map
            _transactionMapForm.lstTransactionMaps.FindItemInList(@"41189", doDoubleClick: true); //, hasExpandIcon: false);
            //Mouse.DoubleClick();

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            // Step 14 Select the Default (Commodity) right click and select New | Transaction Type | Standard...
            _transactionMapForm.lstTransactionMaps.FindItemInList(@"Default (Commodity)"); //, hasExpandIcon: false);
            Mouse.RightClick();
            _transactionMapForm.ContextMenuSelect(@"New|Transaction Type|Standard...");

            // Step 15 Select Received - Ship transaction type from the list and click the  Save button
            _transactionMapForm.GetSelectionList();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            _transactionMapForm.lstItems.FindItemInList(@"Received - Ship");
            _transactionMapForm.btnSaveItem.DoClick();

            // Step 16 Select the 41189 transaction map and click the Extract to CSV file button in the toolbar
            _transactionMapForm.lstTransactionMaps.FindItemInList(@"41189"); //, hasExpandIcon: true);
            Mouse.Click();
            
            // Step 17 Select the 41189A.csv file from the previous noted location and click the Save button
            // Step 18 Replace existing file ith this one - done this by deleting existing file 
            Miscellaneous.DeleteFile(directory + filename);
            Assert.IsFalse(File.Exists(filename), @"File has not been deleted");

            _transactionMapForm.ExtractToCSV(directory + filename);

            // Step 19 Open 41189A.csv file and check for the Received - Ship transaction in the excel sheet
            // Step 20 Change the Debtor from (Default) to RDT for Received - Ship transaction and press the Ctrl+S keys to save the changes and close the file
            lines = new List<string>();
            if (File.Exists(directory + filename))
            {
                using (StreamReader reader = new StreamReader(directory + filename))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains(","))
                        {
                            string[] split = line.Split(',');

                            if (split[8].Contains("Received - Ship"))
                            {
                                split[10] = "RDT";
                                line = string.Join(",", split);
                            }
                        }

                        lines.Add(line);
                    }
                }

                using (StreamWriter writer = new StreamWriter(directory + filename, false, Encoding.ASCII))
                {
                    foreach (string line in lines)
                        writer.WriteLine(line);
                }
            }


            // Step 21 Go back to  Invoice Functions | Maintenance | Transaction Map 
            _transactionMapForm.SetFocusToForm();

            // Step 22 Click the Load From CSV file button in the toolbar and select the 41189A.csv file to load
            _transactionMapForm.LoadFromCSV(directory + filename);

            // Step 23 Double-Click on the 41189 to expand the transaction map
            _transactionMapForm.lstTransactionMaps.FindItemInList(@"41189"); //, hasExpandIcon: false);
            Mouse.DoubleClick();
            Mouse.DoubleClick();

            // Step 24 Check for the Debtor RDT for the Received - Ship transaction type
            _transactionMapForm.lstTransactionMaps.FindItemInList(@"Received - Ship"); //, hasExpandIcon: false);
            _transactionMapForm.lstTransactionMaps.FindItemInList(@"RDT"); //, hasExpandIcon: false);

            // Step 25 Select 41189 and click the Delete button in the toolbar
            _transactionMapForm.CloseForm();
            OpenTransactionMapForm("Transaction Map", @"Cargo|41189");
            //transactionMapForm.btnDelete.DoClick();
            _transactionMapForm.DoDelete();

            // Step 26 Click the Yes button
            _confirmationForm = new ConfirmationFormYesNo(@"Confirm Delete");
            _confirmationForm.btnYes.DoClick();
        }

        private void OpenTransactionMapForm(string navigateTo, string subItemToFind)
        {
            FormObjectBase.NavigationMenuSelection(navigateTo);

            _transactionMapForm = new InvoiceTransactionMapForm();

            // Step 3 In the Searcher, select Cargo and click the Search button 
            _transactionMapForm.chkCargo.DoClick();
            _transactionMapForm.chkEvent.DoClick(false);
            _transactionMapForm.chkVoyage.DoClick(false);
            _transactionMapForm.chkSnapshotSummary.DoClick(false);
            _transactionMapForm.chkCargoAllocation.DoClick(false);
            _transactionMapForm.chkBillOfLading.DoClick(false);
            //transactionMapForm.btnSearch.DoClick();
            _transactionMapForm.DoSearch();

            // Step 4 Click on the adjacent triangle symbol at the left of the Cargo node
            _transactionMapForm.lstTransactionMaps.FindItemInList(subItemToFind); 
            Mouse.Click();
        }
    }
}
