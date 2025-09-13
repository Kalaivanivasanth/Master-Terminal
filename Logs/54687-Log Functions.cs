using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Logs;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class _54687_Log_Functions : MTNBase
    {


        private LogVoyageBerthMaintenanceForm _LogVoyageBerthMaintenanceForm;
        private CheckScalingForm _CheckScalingForm;
        private DisassemblyForm _DisassemblyForm;
        private LogAreasForm _LogAreasForm;
        private LogRowMaintenanceForm _LogRowMaintenanceForm;
        private RegradingForm _RegradingForm;
        private ReinstateRejectsForm _ReinstateRejectsForm;
        private LogRejectionsForm _LogRejectionsForm;
        private ReticketingForm _ReticketingForm;
        private TransfersForm _TransfersForm;
        private LogOutForm _LogOutForm;


        private const string TestCaseNumber = @"54687";
        private const string CargoId = @"JLG" + TestCaseNumber + @"A01";


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            // Setup data
            searchFor = @"_" + TestCaseNumber + "_";
            loadFileDeleteStartTime = DateTime.Now;

        }

        [TestInitialize]
        public new void TestInitialize()
        {
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }


        [TestMethod]
        public void VerifyLogFunctions()
        {
            MTNInitialize();
            VerifyLogVoyageBerthMaintenanceForm();
            // VerifyCheckScalingForm();
            //VerifyDisassemblyForm();
            //VerifyLogAreasForm();
            //VerifyLogRowMaintenanceForm();
            //VerifyRegradingForm();
            //VerifyReinstateRejectsForm();
            //VerifyLogRejectionsForm();
            //VerifyReticketingForm();
            //VerifyTransfersForm();
            //VerifyLogOutForm();
        }


        public void MTNInitialize()
        {
            // Start Master Terminal
            BaseClassInitialize(TestContext);

            // Signon Master Terminal
            signonForm = new SignonPageObject();
            signonForm.Signon(TestContext);
            signonForm.ClickSaveButton();

            base.TestInitialize();
        }

        public void VerifyLogVoyageBerthMaintenanceForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Log Functions|Log Voyage Berth Maintenance");

            //Verify Correctly opens the appropriate form
            _LogVoyageBerthMaintenanceForm = new LogVoyageBerthMaintenanceForm(@"Berth Maintenance TT1");

            //Verify Form Opens with No Error Messages
            Assert.IsTrue(_LogVoyageBerthMaintenanceForm.harbourCode.IsAvailable, @"Log Voyage Berth Maintenance Form Navigated Successfully");

            //Close Form
            _LogVoyageBerthMaintenanceForm.CloseForm();

        }


        public void VerifyCheckScalingForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Check Scaling");

            //Verify Correctly opens the appropriate form
            _CheckScalingForm = new CheckScalingForm(@"Check Scaling TT1");

            //Verify Form Opens with No Error Messages
            Assert.IsTrue(_CheckScalingForm.checkScalerTab.IsAvailable, @"Check Scaling Form Navigated Successfully");

            //Close Form
            _CheckScalingForm.CloseForm();

        }

        public void VerifyDisassemblyForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Disassembly");

            //Verify Correctly opens the appropriate form
            _DisassemblyForm = new DisassemblyForm(@"Disassembly TT1");

            
            //Verify Form Opens with No Error Messages
            Assert.IsTrue(_DisassemblyForm.logIdTab.IsAvailable, @"Disassembly Form Navigated Successfully");

            //Close Form
            _DisassemblyForm.CloseForm();

        }


        public void VerifyLogAreasForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Log Areas");

            //Verify Correctly opens the appropriate form
            _LogAreasForm = new LogAreasForm(@"Terminal Areas TT1");


            //Verify Form Opens with No Error Messages
            Assert.IsTrue(_LogAreasForm.availableAreas.IsAvailable, @"Log Areas Form Navigated Successfully");

            //Close Form
            _LogAreasForm.CloseForm();

        }

        public void VerifyLogRowMaintenanceForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Log Row Maintenance");

            //Verify Correctly opens the appropriate form
            _LogRowMaintenanceForm = new LogRowMaintenanceForm(@"Log Row Maintenance TT1");


            //Verify Form Opens with No Error Messages
            Assert.IsTrue(_LogRowMaintenanceForm.srcTab.IsAvailable, @"Log Row Maintenance Form Navigated Successfully");

            //Close Form
            _LogRowMaintenanceForm.CloseForm();

        }

        public void VerifyRegradingForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Regrading");

            //Verify Correctly opens the appropriate form
            _RegradingForm = new RegradingForm(@"Regrading TT1");


            //Verify Form Opens with No Error Messages
            Assert.IsTrue(_RegradingForm.logId.IsAvailable, @"Regrading Form Navigated Successfully");

            //Close Form
            _RegradingForm.CloseForm();

        }

        

             public void VerifyReinstateRejectsForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Reinstate Rejects");

            //Verify Correctly opens the appropriate form
            _ReinstateRejectsForm = new ReinstateRejectsForm(@"Reinstate Rejects TT1");


            //Verify Form Opens with No Error Messages
            Assert.IsTrue(_ReinstateRejectsForm.enteredDTStockMovementTab.IsAvailable, @"Reinstate Rejects Form Navigated Successfully");

            //Close Form
            _ReinstateRejectsForm.CloseForm();

        }

        public void VerifyLogRejectionsForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Log Rejections");

            //Verify Correctly opens the appropriate form
            _LogRejectionsForm = new LogRejectionsForm(@"Rejection TT1");


            //Verify Form Opens with No Error Messages
            Assert.IsTrue(_LogRejectionsForm.logIdTab.IsAvailable, @"Log Rejections Form Navigated Successfully");

            //Close Form
            _LogRejectionsForm.CloseForm();

        }

        public void VerifyReticketingForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Reticketing");

            //Verify Correctly opens the appropriate form
            _ReticketingForm = new ReticketingForm(@"Reticketing TT1");


            //Verify Form Opens with No Error Messages
            Assert.IsTrue(_ReticketingForm.logIdTabs.IsAvailable, @"Reticketing Form Navigated Successfully");

            //Close Form
            _ReticketingForm.CloseForm();

        }

        

             public void VerifyTransfersForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Transfers");

            //Verify Correctly opens the appropriate form
            _TransfersForm = new TransfersForm(@"Transfer TT1");


            //Verify Form Opens with No Error Messages
            Assert.IsTrue(_TransfersForm.logIdTab.IsAvailable, @"Transfers Form Navigated Successfully");

            //Close Form
            _TransfersForm.CloseForm();

        }



        public void VerifyLogOutForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Log Voyage Functions|Load Out");

            //Verify Correctly opens the appropriate form
            _LogOutForm = new LogOutForm(@"Load Out Entry TT1");


            //Verify Form Opens with No Error Messages
            Assert.IsTrue(_LogOutForm.itemBtn.IsAvailable, @"LogOut Form Navigated Successfully");

            //Close Form
            _LogOutForm.CloseForm();

        }
    }
}
