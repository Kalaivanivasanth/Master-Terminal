using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNArguments.Classes;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.General_Functions.Ownership_Change_Enquiry;
using MTNForms.FormObjects.Invoice_Functions;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.General.Ownership_Change_Enquiry
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase65890 : MTNBase
    {
        const string MSLDebtor = "RDT";   // Operator MSL's Debtor
        const string OIT1Debtor = "DBTOPINCOT";   // Operator OIT1's Debtor
        
        const string CargoId = "COO65890A0";
        const string COOId = "COO60589";
       
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
           LogInto<MTNLogInOutBO>("MTNCOO");
            
           FormObjectBase.MainForm.OpenInvoiceTransactionMapFromToolbar();
            new InvoiceTransactionMapForm()
                .GetSelectTransactionMapTypes(new TransactionMapTypeArguments { Cargo = true })
                .SetActiveTransactionMap("Cargo|TM_INCOT");
            
            CallJadeScriptToRun(TestContext, "resetData_65890");
        }
        
        [TestMethod]
        public void NoINCOPastChargedOld()
        {
            MTNInitialize();

            var effective = DateTime.Now.AddDays(-10);
            
            FormObjectBase.MainForm.OpenOwnershipChangeEnquiryFromToolbar();
            var ownershipChangeEnquiryForm = new OwnershipChangeEnquiryForm();
            ownershipChangeEnquiryForm.DoAdd();

            var ownershipChangeMaintForm = new OwnershipChangeMaintForm();
            ownershipChangeMaintForm
                .AddCOO(new OwnershipChangeDetailsAddArguments
                {
                    COOFieldsToSet = new []
                    {
                        new MTNGeneralArguments.FieldRowNameValueArguments
                        { FieldRowName = OwnershipChangeMaintForm.ControlFieldName.COONumber, FieldRowValue = $"{COOId}A" },
                        new MTNGeneralArguments.FieldRowNameValueArguments
                        { FieldRowName = OwnershipChangeMaintForm.ControlFieldName.EffectiveDateDate, FieldRowValue = effective.ToString("ddMMyyyy") },
                        new MTNGeneralArguments.FieldRowNameValueArguments
                        { FieldRowName = OwnershipChangeMaintForm.ControlFieldName.EffectiveDateTime, FieldRowValue = effective.ToString("HHmm") },
                        new MTNGeneralArguments.FieldRowNameValueArguments
                        { FieldRowName = OwnershipChangeMaintForm.ControlFieldName.CurrentOperatorOperator, FieldRowValue = $"{Operator.MSL}\t" },
                        new MTNGeneralArguments.FieldRowNameValueArguments
                        { FieldRowName = OwnershipChangeMaintForm.ControlFieldName.NewOperatorOperator, FieldRowValue = $"{Operator.AAIN}\t" },
                    }, 
                    CargoSearchArguments = new CargoSearchFormArguments
                    {
                            CargoSearchCriteria = new []
                            {
                                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = CargoSearchForm.ControlFieldName.Site, FieldRowValue = Site.OnShip },
                                new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = CargoSearchForm.ControlFieldName.CargoID, FieldRowValue = $"{CargoId}" }
                            },
                            CargoRowsToAdd = new []
                            {
                                $"ID^{CargoId}~Cargo Type^{CargoType.CT_INCOT}",
                            }
                    },
                    /*CheckRowsExist = new FindClickRowArguments
                    {
                       DetailsToSearchFor = new []
                       {
                           "Cargo Id^COO65890A00~Current Total Quantity^10~Current Total Weight kg^500",
                           "Cargo Id^COO65890A01~Current Total Quantity^10~Current Total Weight kg^500",
                       },
                    }*/
                });

            effective = effective.AddDays(-20);
            
            FormObjectBase.MainForm.OpenVoyageOperationsFromToolbar();
            var voyageOperationsForm = new VoyageOperationsForm();
            voyageOperationsForm
                .DoSearchForVoyageGetDetails(new VoyageOperationsSearcherArguments
                {
                    Voyage = TT1.Voyage.MSCK000010,
                    LOLOBays = true
                })
                .DischargeSelectedCargo(new VoyageOperationsFormArguments
                 {
                     FieldsToSet = new []
                     {
                         new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = VoyageOperationsForm.ControlFieldName.DischargeTo, FieldRowValue = TT1.TerminalArea.INCOCA },
                         new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = VoyageOperationsForm.ControlFieldName.OpsRetrospective, FieldRowValue = CheckboxValue.Set },
                         new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = VoyageOperationsForm.ControlFieldName.EffectiveDateDate, FieldRowValue = effective.ToString("ddMMyyyy") },
                         new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = VoyageOperationsForm.ControlFieldName.EffectiveDateTime, FieldRowValue = effective.ToString("HHmm") },
                     }
                 }, new[] { $"ID^{CargoId}0", });
            
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm($"Cargo Enquiry {terminalId}");
            cargoEnquiryForm.SearchForCargoItems(new []
            {
                "Cargo Type^ ^^^",
                $"Cargo ID^{CargoId}0^^^"
            }, $"ID^{CargoId}0~Cargo Type^{CargoType.CT_INCOT}");
            
            cargoEnquiryForm.DoEdit();
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, "IMEX Status", IMEX.Export, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSave();
            
            cargoEnquiryForm.DoViewTransactions();
            var cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            cargoEnquiryTransactionForm.TblTransactions2.CheckTransactionIsInRow( "Type^Edited");
            cargoEnquiryTransactionForm.GetChargesTab();   // Currently have to do it this way as we use the tabcontrol to get the tab
            cargoEnquiryTransactionForm.tblCharges1.CheckTransactionIsInRow($"Debtor^{MSLDebtor}~Transaction^Edited~Narration^ILTINCOT");
           
            ownershipChangeMaintForm.SetFocusToForm();
            ownershipChangeMaintForm.DoApprove();
            
            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.tblData2.FindClickRow(new[]
            {
                $"ID^{CargoId}0~Cargo Type^{CargoType.CT_INCOT}~Location ID^{TT1.TerminalArea.INCOCA}~Opr^{Operator.AAIN}"
            });
            
            cargoEnquiryForm.DoViewTransactions();
             cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new[] { "Type^Edited" });
            cargoEnquiryTransactionForm.GetChargesTab();   // Currently have to do it this way as we use the tabcontrol to get the tab
            cargoEnquiryTransactionForm.INCODebtorChargedValidation(effective,
                new[]
                {
                    $"Debtor^{OIT1Debtor}~Transaction^Edited~Narration^ILTINCOT",
                    $"Debtor^{OIT1Debtor}~Transaction^Edited~Narration^ILTINCOT"
                }, new[] { OIT1Debtor, OIT1Debtor, });

        }
    }

    
}
