using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Invoice_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class _54685_Invoice_Functions_Module : MTNBase
    {

        private InvoiceGenerationForm _InvoiceGenerationForm;

        private const string TestCaseNumber = @"54685";
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
        public void VerifyInvoiceFunctions()
        {
            MTNInitialize();
            VerifyInvoiceGenerationForm();
            
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

        public void VerifyInvoiceGenerationForm()
        {

            FormObjectBase.NavigationMenuSelection(@"Invoice Functions|Invoice Generation");

            //Verify Correctly opens the appropriate form
            _InvoiceGenerationForm = new InvoiceGenerationForm(@"Invoice Generation TT1");

            //Verify Form Opens with No Error Messages
           
            String value = _InvoiceGenerationForm.invoiceType.GetText();
            Console.WriteLine("Get Value " + value);
            if (value != null)
            {
                Assert.IsTrue(value.Contains("Invoice Type"), @"Invoice Generation Form Navigated Successfully");
            } 

            //Close Form
            _InvoiceGenerationForm.CloseForm();

        }

    }
}
