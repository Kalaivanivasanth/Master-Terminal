using DataObjects;
using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40982 : MTNBase
    {
       
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_40982_";
            BaseClassInitialize_New(testContext);
            //userName = "MTNOLDDAM";
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //MTNSignon(TestContext, userName);
            TestRunDO.GetInstance().SetUserName("MTNOLDDAM");
            LogInto<MTNLogInOutBO>();

            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config");
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Settings", @"Clear Damage After Repair", @"1", 
                rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();

            SetupAndLoadInitializeData(TestContext);
        }


        [TestMethod]
        public void ClearDamageAfterRepair1ItemOfDamage()
        {

            MTNInitialize();
            
            // Step 5 - 8
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            // Step 9 - 15
            DoDamagePartOfTest(@"", true);

            // Step 16 - 17
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();

            CargoEnquiryTransactionForm cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions,
                // @"Type^Edited~Details^allDamageInstancesByPos Front BENT(2)( x1),  =>: damaged Yes => No");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, 
                // @"Type^Repair Details~Details^Front BENT(2) (x1)");
            // MTNControlBase.FindClickRowInTable(cargoEnquiryTransactionForm.tblTransactions, @"Type^Repaired.");
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow([
                "Type^Edited~Details^allDamageInstancesByPos Front BENT(2)( x1),  =>: damaged Yes => No",
                "Type^Repair Details~Details^Front BENT(2) (x1)",
                "Type^Repaired."
            ]);

            // Step 18
            FormObjectBase.NavigationMenuSelection(@"Terminal Ops|Terminal Config", forceReset: true);
            TerminalConfigForm terminalConfigForm = new TerminalConfigForm();
            terminalConfigForm.SetTerminalConfiguration(@"Settings", @"Clear Damage After Repair", @"0",
                rowDataType: EditRowDataType.CheckBox);
            terminalConfigForm.CloseForm();

            // Step 21 - 30
            cargoEnquiryForm.SetFocusToForm();
            DoDamagePartOfTest(@"Front has Bent (2)");
        }

        private void DoDamagePartOfTest(string damageDetailText, bool findStatusTab = false)
        {
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG40982A01");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40982A01", clickType: ClickType.Click);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG40982A01"], clickType: ClickType.Click);

            if (findStatusTab)
            {
                cargoEnquiryForm.GetGenericTabTableDetails(@"Status", @"4087");
            }

            //cargoEnquiryForm.DoEdit();
           
            MTNControlBase.FindClickRowInTableVHeader(cargoEnquiryForm.tblGeneric, @"Damage Detail (dbl click)",
                clickType: ClickType.DoubleClick, tableRowHeight: 19);

            DamageForm damageForm = new DamageForm(@"Damage JLG40982A01 TT1");
         
            damageForm.EnterNewDamageDetail("Front - Front", "BENT - Bent", "2	Two", "1",
                "Position^Front~Damaged^BENT~Severity^2~Quantity^1");

            damageForm.btnOk.DoClick();
            
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Cargo TT1", 
                new [] { "Code :75016. The Container Id (JLG40982A01) failed the validity checks and may be incorrect." });

            /*cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.DoSave();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Tracked Item Update TT1");
            string[] warningErrorToCheck =
            {
                "Code :75016. The Container Id (JLG40982A01) failed the validity checks and may be incorrect."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();*/

            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Damaged", @"Yes");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Damage Detail (dbl click)",
                @"Front has Bent (2)");

            //cargoEnquiryForm.btnEdit.DoClick();
            //cargoEnquiryForm.DoEdit();

            MTNControlBase.FindClickRowInTableVHeader(cargoEnquiryForm.tblGeneric, @"Damage Detail (dbl click)",
                clickType: ClickType.DoubleClick, tableRowHeight: 19);

            damageForm = new DamageForm(@"Damage JLG40982A01 TT1");
            damageForm.chkRepaired.DoClick();
            damageForm.btnUpdate.DoClick();
            // MTNControlBase.FindClickRowInTable(damageForm.tblDamageDetails,
                // @"Position^Front~Damaged^BENT~Severity^2~Quantity^1^Repaired^Yes");
            damageForm.TblDamageDetails.FindClickRow(["Position^Front~Damaged^BENT~Severity^2~Quantity^1^Repaired^Yes"]);

            damageForm.btnOk.DoClick();
            
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Cargo TT1", 
                new [] { "Code :75016. The Container Id (JLG40982A01) failed the validity checks and may be incorrect." });

            /* cargoEnquiryForm.SetFocusToForm();
            //cargoEnquiryForm.btnSave.DoClick();
            cargoEnquiryForm.DoSave();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Tracked Item Update TT1");
            string[] warningErrorToCheck = new string[]
            {
                "Code :75016. The Container Id (JLG40982A01) failed the validity checks and may be incorrect."
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();*/

            cargoEnquiryForm.SetFocusToForm();
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Damaged", @"No");
            MTNControlBase.ValidateValueInEditTable(cargoEnquiryForm.tblGeneric, @"Damage Detail (dbl click)",
                damageDetailText);
        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <TestCases>40982</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG40982A01</id>\n         <isoType>2200</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>APPL</commodity>\n         <voyageCode>MSCK000002</voyageCode>\n         <dischargePort>NZAKL</dischargePort>\n		 <messageMode>D</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <TestCases>40982</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG40982A01</id>\n         <isoType>2200</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>APPL</commodity>\n         <voyageCode>MSCK000002</voyageCode>\n         <dischargePort>NZAKL</dischargePort>\n		 <messageMode>A</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }




    }

}
