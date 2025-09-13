using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Ownership_Change_Enquiry;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.General.Ownership_Change_Enquiry
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase60772 : MTNBase
    {
        readonly string[] _expectedMessages =
        {
            "Code :96299. Item CG60772 has 335000.0000 m3 in current release requests, cannot transfer further 335000.0000 m3"
        };
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>("MTNCOO");


        [TestMethod]
        public void ReleaseRequestTrackedByQtyNoReserveDetailsWithCOO()
        {
            MTNInitialize();

            FormObjectBase.MainForm.OpenOwnershipChangeEnquiryFromToolbar();
            OwnershipChangeEnquiryForm.FindAndSelectCOOToUpdate("60772", "Number^60772");
            OwnershipChangeEnquiryForm.AddCOODetailsWithExpectedMessage("CG60772", _expectedMessages);
           // OwnershipChangeEnquiryForm.SaveCOODetailsExpectedMessage(
           //     "Ownership Change Detail TT1 MSL/60772/ANL/CG60772 TT1", "CG60772", _expectedMessages);
            //OwnershipChangeEnquiryForm.SaveCOOExpectedMessage(
            //    "Errors for Ownership Change Updated TT1", _expectedMessages);
        }
    }
}
