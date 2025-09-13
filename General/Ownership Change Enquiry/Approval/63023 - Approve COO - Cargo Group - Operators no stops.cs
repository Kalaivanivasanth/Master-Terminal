using System;
using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.General_Functions.Ownership_Change_Enquiry;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.General.Ownership_Change_Enquiry.Approval
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase63023 : MTNBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, "resetData_63023");
            LogInto<MTNLogInOutBO>("MTNCOO");
        }

        [TestMethod]
        public void NonCargoGroupOperatorsNoStops()
        {
            MTNInitialize();
            
            /////var approvalDateTime = DateTime.Parse("15/05/2024 11:55");
            
            var approvalDateTime = DateTime.MaxValue;

            FormObjectBase.MainForm.OpenOwnershipChangeEnquiryFromToolbar();
            var ownershipChangeEnquiryForm = FormObjectBase.CreateReturnFormFromFormManger<OwnershipChangeEnquiryForm>();
            ownershipChangeEnquiryForm.FindSelectSpecificCOO(new OwnershipChangeSearchArguments { Number = "COO63023A" }, "Number^COO63023A")
                .DoApprovalProcess(ref approvalDateTime, false)                    // Clicking No to the messagebox 1st time to check this works correctly
                .DoApprovalProcess(ref approvalDateTime)                         // Clicking Yes to the messagebox
                .CheckApproveRemoveButtonsNotAvailable();
           
            Console.WriteLine($"approvalDateTime: {approvalDateTime.FormatddMMyyyyHHmmWithSlashes()}");
            
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            cargoEnquiryForm.SearchForCargo(
                 new []
                 {
                     new GetSetFieldsOnFormArguments
                     {
                         FieldName = CargoEnquiryForm.SearchFields.CargoType,
                         FieldValue = CargoEnquiryForm.CargoTypes.KeyMark, RowDataType = EditRowDataType.ComboBoxEdit,
                         FromCargoEnquiryForm = true, SearchSubStringTo = 3
                     },
                     new GetSetFieldsOnFormArguments { FieldName = CargoEnquiryForm.SearchFields.CargoID, FieldValue = "CG63023A"},
                 })
                .ValidateDataTableRowDetails(new [] { "ID^CG63023A~Opr^COO1" })
                .ValidateTabEditTableDetails(new[]
                {
                    new GetSetFieldsOnFormArguments
                        { FieldName = CargoEnquiryForm.TabGeneralFields.Operator, FieldValue = "COO1", TabName = "General" },
                })
                /*.CheckTransactionsRowDetails(new []
                {
                    "Type^Ownership Changed~Details^Change of Ownership - COO63023A",
                    $"Type^Ownership Changed~Details^Effective - {approvalDateTime:dd/MM/yyyy}",
                    "Type^Ownership Changed~Details^Old Operator - MSL",
                    "Type^Ownership Changed~Details^New Operator - COO1",
                    "Type^Ownership Changed~Details^Transferred Quantity - 1",
                    "Type^Ownership Changed~Details^Transferred Weight - 4409.249 lbs",
                    "Type^Ownership Changed~Details^Transferred Volume - 6.6660 m3",
                })*/
                .SearchForCargo(
                    new []
                    { 
                        new GetSetFieldsOnFormArguments 
                            { FieldName = CargoEnquiryForm.SearchFields.CargoType, FieldValue = CargoEnquiryForm.CargoTypes.ISOContainer, RowDataType = EditRowDataType.ComboBoxEdit, FromCargoEnquiryForm = true }, 
                        new GetSetFieldsOnFormArguments 
                            { FieldName = CargoEnquiryForm.SearchFields.CargoID, FieldValue = "COO63023A"}, 
                    })
                .ValidateDataTableRowDetails(new [] { "ID^COO63023A01~Opr^COO1~Location ID^MKBS01" })
                .ValidateTabEditTableDetails(new[]
                {
                    new GetSetFieldsOnFormArguments
                        { FieldName = CargoEnquiryForm.TabGeneralFields.Operator, FieldValue = "COO1", TabName = "General" },
                    new GetSetFieldsOnFormArguments
                        { FieldName = CargoEnquiryForm.TabStatusFields.Stops, FieldValue = "STOP_39019, Delivery Order, STOP_65105_3", TabName = "Status" },
                })
                .CheckTransactionsRowDetails(new []
                {
                    "Type^Operator Change~Details^myOperator Operator - MSL => Operator - COO1: operatorCode MSL => COO1",
                    "Type^Ownership Changed~Details^Change of Ownership - COO63023A",
                    $"Type^Ownership Changed~Details^Effective - {approvalDateTime:dd/MM/yyy}",
                    "Type^Ownership Changed~Details^Old Operator - MSL",
                    "Type^Ownership Changed~Details^New Operator - COO1",
                    "Type^Ownership Changed~Details^Transferred Quantity - 1",
                    "Type^Ownership Changed~Details^Transferred Weight - 4409.249 lbs",
                    "Type^Ownership Changed~Details^Transferred Volume - 6.6660 m3",
                });
        }
    }

    
}
