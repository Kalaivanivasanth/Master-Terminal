using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using System;
using DataObjects;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase41156 : MTNBase
    {
        
        const string UserName = "MTNOLDDAM";
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_41156_";
            BaseClassInitialize_New(testContext);
        }

        [TestInitialize]
        public new void TestInitialize() {}
      
        
        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //MTNSignon(TestContext, UserName);
            TestRunDO.GetInstance().SetUserName(UserName);
            LogInto<MTNLogInOutBO>();
            SetupAndLoadInitializeData(TestContext);
        }


        [TestMethod]
        public void ClearDamageAfterRepairMultipeDamageDetails()
        {
            
            MTNInitialize();
            
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Settings", @"Clear Damage After Repair", @"1",
                rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();

            // Step 5 - 8
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            // Step 9 - 15
            DoDamagePartOfTest(@"", true);

            
            // Step 16 - 17
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Repair Details~Details^Back DENT(3) (x2)");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Repaired.");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Repair Details~Details^Front BENT(2) (x1)");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^Edited~Details^allDamageInstancesByPos Front BENT(2)( x1), Back DENT(3)( x2),  =>: damaged Yes => No");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Cams Audit~Details^Damage Location - transaction deleted");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Cams Audit~Details^Damage Location - transaction deleted", findInstance: 2);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^*** DELETED *** Damage Location~Details^Transaction deleted by MTNOLDDAM.  Reason given: User deleted the damage entry Front BENT(2)( x1)");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^*** DELETED *** Damage Location~Details^Transaction deleted by MTNOLDDAM.  Reason given: User deleted the damage entry Back DENT(3)( x2)");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Damaged.");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^Edited~Details^allDamageInstancesByPos  => Front BENT(2)( x1), Back DENT(3)( x2),: damaged No => Yes");
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow([
                "Type^Repair Details~Details^Back DENT(3) (x2)",
                "Type^Repaired.",
                "Type^Repair Details~Details^Front BENT(2) (x1)",
                "Type^Edited~Details^allDamageInstancesByPos Front BENT(2)( x1), Back DENT(3)( x2),  =>: damaged Yes => No",
                "Type^Cams Audit~Details^Damage Location - transaction deleted",
                "Type^Cams Audit~Details^Damage Location - transaction deleted",
                "Type^*** DELETED *** Damage Location~Details^Transaction deleted by MTNOLDDAM.  Reason given: User deleted the damage entry Front BENT(2)( x1)",
                "Type^*** DELETED *** Damage Location~Details^Transaction deleted by MTNOLDDAM.  Reason given: User deleted the damage entry Back DENT(3)( x2)",
                "Type^Damaged.",
                "Type^Edited~Details^allDamageInstancesByPos  => Front BENT(2)( x1), Back DENT(3)( x2),: damaged No => Yes"
            ]);

            // Step 18
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Settings", @"Clear Damage After Repair", @"0",
                rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();

            // Step 21 - 30
            cargoEnquiryForm.SetFocusToForm();
            DoDamagePartOfTest(@"Front has Bent (2), Back has Denty (3)");
            
        }

        private void DoDamagePartOfTest(string damageDetailText, bool findStatusTab = false)
        {
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG41156A01");
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG41156A01", clickType: ClickType.Click);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG41156A01"], clickType: ClickType.Click);

            if (findStatusTab)
                cargoEnquiryForm.GetGenericTabTableDetails(@"Status", @"4087");

            // cargoEnquiryForm.DoEdit();

            MTNControlBase.FindClickRowInTableVHeader(cargoEnquiryForm.tblGeneric, @"Damage Detail (dbl click)",
                clickType: ClickType.DoubleClick, tableRowHeight: 19);

            DamageForm damageForm = new DamageForm(@"Damage JLG41156A01 TT1");

            damageForm.EnterNewDamageDetail(@"Front - Front", @"BENT - Bent", @"2	Two", @"1",
                "Position^Front~Damaged^BENT~Severity^2~Quantity^1");
            damageForm.EnterNewDamageDetail(@"Back - Back", @"DENT - Denty", @"3	Three", @"2",
                "Position^Back~Damaged^DENT~Severity^3~Quantity^2");

            damageForm.btnOk.DoClick();
            
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Cargo TT1", 
                new [] { "Code :75016. The Container Id (JLG41156A01) failed the validity checks and may be incorrect." });

            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Damaged", @"Yes");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Damage Detail (dbl click)",
                @"Front has Bent (2), Back has Denty (3)");

            //cargoEnquiryForm.btnEdit.DoClick();
            //cargoEnquiryForm.DoEdit();

            MTNControlBase.FindClickRowInTableVHeader(cargoEnquiryForm.tblGeneric, @"Damage Detail (dbl click)",
                clickType: ClickType.DoubleClick, tableRowHeight: 19);

            damageForm = new DamageForm(@"Damage JLG41156A01 TT1");
            DamageRepaired(damageForm);

            WarningErrorForm.CompleteWarningErrorForm("Warnings for Cargo TT1", 
                new [] { "Code :75016. The Container Id (JLG41156A01) failed the validity checks and may be incorrect." });

            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Damaged", @"No");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Damage Detail (dbl click)",
                damageDetailText);
                
        }

        private void DamageRepaired(DamageForm damageForm)
        {
            // MTNControlBase.FindClickRowInTable(damageForm.tblDamageDetails,
                // @"Position^Front~Damaged^BENT~Severity^2~Quantity^1",rowHeight:16);
            damageForm.TblDamageDetails.FindClickRow(["Position^Front~Damaged^BENT~Severity^2~Quantity^1"]);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            damageForm.chkRepaired.DoClick();
            damageForm.btnUpdate.DoClick();
            // MTNControlBase.FindClickRowInTable(damageForm.tblDamageDetails,
                // @"Position^Front~Damaged^BENT~Severity^2~Quantity^1^Repaired^Yes",ClickType.None);
            // MTNControlBase.FindClickRowInTable(damageForm.tblDamageDetails,
                // @"Position^Back~Damaged^DENT~Severity^3~Quantity^2", rowHeight: 16);
            damageForm.TblDamageDetails.FindClickRow([
                "Position^Front~Damaged^BENT~Severity^2~Quantity^1^Repaired^Yes",
                "Position^Back~Damaged^DENT~Severity^3~Quantity^2"
            ], ClickType.None);
;            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));
            damageForm.chkRepaired.DoClick();
            damageForm.btnUpdate.DoClick();
            // MTNControlBase.FindClickRowInTable(damageForm.tblDamageDetails,
                // @"Position^Back~Damaged^DENT~Severity^3~Quantity^2^Repaired^Yes",ClickType.None);
            damageForm.TblDamageDetails.FindClickRow(["Position^Back~Damaged^DENT~Severity^3~Quantity^2^Repaired^Yes"], ClickType.None);

            damageForm.btnOk.DoClick();
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0'?> <JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <messageMode>D</messageMode>\n      <id>JLG41156A01</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>2000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>2200</isoType>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0'?> <JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n	<operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n	  <messageMode>A</messageMode>\n      <id>JLG41156A01</id>\n      <operatorCode>MSL</operatorCode>\n      <locationId>MKBS01</locationId>\n      <weight>2000</weight>\n      <imexStatus>Export</imexStatus>\n      <commodity>GENL</commodity>\n      <dischargePort>NZAKL</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <isoType>2200</isoType>\n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
