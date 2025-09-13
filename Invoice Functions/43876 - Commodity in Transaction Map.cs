using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using MTNForms.Controls.Table;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43876 : MTNBase
    {
        SystemAdminForm systemAdminForm;
        InvoiceTransactionMapForm transactionMapForm;
        ConfirmationFormYesNo confirmationForm;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void CommodityInTransactionMap()
        {
            
            MTNInitialize();
            
            // Step 2 Open System Items | System Ops | System Admin 
            // Monday, 17 March 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"System Ops|System Admin");
            FormObjectBase.MainForm.OpenSystemAdminFromToolbar();

            // Step 3 Select Commodities from the Table drop down 
            systemAdminForm = new SystemAdminForm(@"System Administration");

            //MTNControlBase.SetValue(systemAdminForm.cmbTable, @"Commodities");
            systemAdminForm.cmbTable.SetValue(@"Commodities");
            //MTNControlBase.SetValue(systemAdminForm.txtFilter, @"QMT");
            systemAdminForm.txtFilter.SetValue(@"QMT");
            Wait.UntilResponsive(systemAdminForm.TblAdministrationItemsRH19A.GetElement());


            // Step 4 Click on the item where the Commodity Code - QMT
            //MTNControlBase.FindClickRowInTable(systemAdminForm.tblAdministrationItems, @"Commodity Code^QMT~Description^Q empty",
            //    searchType: SearchType.Exact, rowHeight: 19);
            //systemAdminForm.TblAdministrationItemsRH19A.FindClickRow(["Commodity Code^QMT~Description^Q empty"], searchType: SearchType.Exact);
            systemAdminForm.TblAdministrationItemsRH19A.FindClickRowFiltered(new FindClickRowArguments
                { DetailsToSearchFor = ["Commodity Code^QMT~Description^Q empty"], SearchType = SearchType.Exact });
            //systemAdminForm.TblAdministrationItemsRH19A.FindClickRow(new[] { "Commodity Code^QMT~Description^Q empty"}, searchType: SearchType.Exact);

            // Step 5 Click the Edit button in the toolbar, select Is Empty Commodity For Container on the Details tab and click the Save button
            MTNControlBase.ValidateValueInEditTable(systemAdminForm.tblDetails, @"Is Empty Commodity For Container", @"1");
            systemAdminForm.CloseForm();

            // Step 6 Open Invoice Functions | Maintenance | Transaction Map
            FormObjectBase.NavigationMenuSelection(@"Invoice Functions|Maintenance|Transaction Map", true);

            transactionMapForm = new InvoiceTransactionMapForm();


            // Step 7 On the Searcher Pane, select the Cargo checkbox and click the Search button
            transactionMapForm.chkCargo.DoClick();
            transactionMapForm.chkEvent.DoClick(false);
            transactionMapForm.chkVoyage.DoClick(false);
            transactionMapForm.chkSnapshotSummary.DoClick(false);
            transactionMapForm.chkCargoAllocation.DoClick(false);
            transactionMapForm.chkBillOfLading.DoClick(false);
            //transactionMapForm.btnSearch.DoClick();
            transactionMapForm.DoSearch();


            // Step 8 Click on the adjacent triangle symbol at the left of the Cargo node
            transactionMapForm.lstTransactionMaps.FindItemInList(@"Cargo|Test43876"); //, hasExpandIcon: true);


            // Step 9 Double - click on the transaction map Test43876
            Mouse.DoubleClick();


            // Step 10 Under the cargo type ISO Container, right-click on the MT contents status and select New > Commodity...
            transactionMapForm.lstTransactionMaps.FindItemInList(@"MT"); //, hasExpandIcon:true);
            Mouse.RightClick();
            transactionMapForm.ContextMenuSelect(@"New|Commodity...");

            // Step 11 Check whether the QMT commodity is present in the Commodities window 
            transactionMapForm.GetSelectionList();

            // Step 12 Select the QMT commodity and click the Save button
            transactionMapForm.lstItems.FindItemInList(@"QMT");
            transactionMapForm.btnSaveItem.DoClick();
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            // Step 13 Select the QMT commodity in the transaction map and click the Delete button in the toolbar
            //transactionMapForm.lstTransactionMaps.SelectMenuItem(@"QMT", hasExpandIcon: true);
            //transactionMapForm.btnDelete.DoClick();
            transactionMapForm.DoDelete();

            // Step 14 Click the Yes button 
            confirmationForm = new ConfirmationFormYesNo(@"Confirm Delete");
            confirmationForm.btnYes.DoClick();
        }
    }
}
