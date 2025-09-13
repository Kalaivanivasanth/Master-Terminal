using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNUtilityClasses.Classes;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using HardcodedData.SystemData;
using MTNArguments.Classes;
using MTNForms.FormObjects.General_Functions.Ownership_Change_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Message_Dialogs;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General.Ownership_Change_Enquiry.Approval
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase66341 : MTNBase
    {
        private const string CargoId = "66341-C1";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>("MTNCOO");

        // Validates the error message displayed when an invalid operator is assigned for ownership change approval.
        [TestMethod]
        public void OwnershipChangeErrorForOperator()
        {
            MTNInitialize();

            FormObjectBase.MainForm.OpenOwnershipChangeEnquiryFromToolbar();
            var ownershipChangeEnquiryForm = new OwnershipChangeEnquiryForm();
            ownershipChangeEnquiryForm.DoAdd();

            var ownershipChangeMaintForm = new OwnershipChangeMaintForm();
            ownershipChangeMaintForm
                .AddCOO(new OwnershipChangeDetailsAddArguments
                {
                    COOFieldsToSet = new[]
                    {
                        new MTNGeneralArguments.FieldRowNameValueArguments
                        { FieldRowName = OwnershipChangeMaintForm.ControlFieldName.COONumber, FieldRowValue = "66341A" },
                        new MTNGeneralArguments.FieldRowNameValueArguments
                        { FieldRowName = OwnershipChangeMaintForm.ControlFieldName.CurrentOperatorOperator, FieldRowValue = "MES\tMEDITERRANEAN SHIPPING CO" },
                        new MTNGeneralArguments.FieldRowNameValueArguments
                        { FieldRowName = OwnershipChangeMaintForm.ControlFieldName.NewOperatorOperator, FieldRowValue = $"{Operator.ABOC}\t" },
                    },

                    CargoSearchArguments = new CargoSearchFormArguments
                    {
                        CargoSearchCriteria = new[]
                           {
                                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = CargoSearchForm.ControlFieldName.Site, FieldRowValue = "On Site" },
                                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = CargoSearchForm.ControlFieldName.CargoID, FieldRowValue = $"{CargoId}" }
                            },
                        CargoRowsToAdd = new[]
                           {
                                $"ID^{CargoId}~Cargo Type^METAL",
                            }
                    },
                });
            ownershipChangeMaintForm.SetFocusToForm();
            ownershipChangeMaintForm.DoApprove(new WarningErrorFormArguments
            {
                FormTitle = $"Errors for Ownership Change Approval {terminalId}",
                ErrorMessages = new[] { "Code :75021. Operator ABOC is not valid for voyage AUTOVOY01." },
                DoCancel = true
            });
         
            // Navigate to OwnershipChange Enquiry Form and remove the OwnerShip change Created 
            ownershipChangeEnquiryForm.SetFocusToForm();

            ownershipChangeEnquiryForm.FindSelectSpecificCOO(new OwnershipChangeSearchArguments
            { Number = "66341A" }, "Number^66341A");

            // Attempt to remove the ownership change but click "No" on the confirmation popup, so the ownership change remains.
            ownershipChangeEnquiryForm.DoRemoveProcess(confirmRemovalYes: false);

            // Attempt to remove the ownership change again, but this time click "Yes" on the confirmation popup, so the ownership change gets removed.
            ownershipChangeEnquiryForm.DoRemoveProcess(confirmRemovalYes: true);
        }
        
    }
}

