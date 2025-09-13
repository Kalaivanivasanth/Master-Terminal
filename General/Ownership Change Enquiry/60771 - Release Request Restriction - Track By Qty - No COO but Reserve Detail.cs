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
    public class TestCase60771 : MTNBase
    {
        readonly string[] _expectedMessages =
        {
            "Code :96299. Item CG60771 has 500 Items in current release requests, cannot transfer further 500 Items"
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
            OwnershipChangeEnquiryForm.FindAndSelectCOOToUpdate("60771", "Number^60771");
            OwnershipChangeEnquiryForm.AddCOODetailsWithExpectedMessage("CG60771", _expectedMessages);
            /*OwnershipChangeEnquiryForm.SaveCOODetailsExpectedMessage(
                "Ownership Change Detail TT1 MSL/60771/MSK/CG60771 TT1", "CG60771", _expectedMessages);
            OwnershipChangeEnquiryForm.SaveCOOExpectedMessage(
                "Errors for Ownership Change Updated TT1", _expectedMessages);*/
        }
    }

}
