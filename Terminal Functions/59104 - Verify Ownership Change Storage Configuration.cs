using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNArguments.Classes;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Terminal_Functions;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Terminal_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase59104 : MTNBase
    {
        const string StorageConfigurationDescription = "CONT IMP MDH1 MDHF1 59104";
        
        StorageConfigurationForm _storageConfigurationForm; 
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>("MTNCOO");

        [TestMethod]
        public void VerifyOwnershipChangeStorageConfig()
        {
            MTNInitialize();

            _storageConfigurationForm = FormObjectBase.NavigateToAndCreateForm<StorageConfigurationForm>()
                .DeleteStorageConfigurationRows([$"Description^{StorageConfigurationDescription}"]);

            SetupAndLoadInitializeData(TestContext);
            
            _storageConfigurationForm.SetFocusToForm();
            _storageConfigurationForm.AddStorageConfiguration(new StorageDetailsArguments
                {
                    TabArguments =
                    [
                        // Criteria Tab
                        new CriteriaArguments
                        {
                            FieldRowNameValueArguments =
                            [
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.CargoType, FieldRowValue = "CONT" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.IMEXStatus, FieldRowValue = "IMP" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.OperatorItem, FieldRowValue = "MDH1" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.IsDamaged, FieldRowValue = "true" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.IsEmpty, FieldRowValue = "true" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.IsHazardous, FieldRowValue = "true" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.IsHazardousLimited, FieldRowValue = "true" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.IsHazardousNotLimited, FieldRowValue = "true" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.IsHazardousOrLimited, FieldRowValue = "true" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.IsOverDimensional, FieldRowValue = "true" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.IsReefer, FieldRowValue = "true" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.FreightForwarder, FieldRowValue = "MDHF1" },
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = CriteriaFields.Description, FieldRowValue = StorageConfigurationDescription }
                            ]
                        },

                        // Change of Ownership Tab
                        new ChangeOfOwnershipArguments
                        {
                            FieldRowNameValueArguments =
                            [
                                new MTNGeneralArguments.FieldRowNameValueArguments
                                    { FieldRowName = ChangeOfOwnershipFields.FreeStorageDaysNumber, FieldRowValue = "5" }
                            ]
                        }
                    ],
                    RowDetailsToValidate = [ $"Description^{StorageConfigurationDescription}~Change of Ownership Free Storage Days^5" ]
                });

            //cargoEnquiryForm = FormObjectBase.NavigateToAndCreateForm<CargoEnquiryForm>();
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            //cargoEnquiryForm.DoSearch([
           //     new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = CargoEnquiryForm.SearchFields.CargoID, FieldRowValue = "NAV59104A01" },
           //     new MTNGeneralArguments.FieldRowNameValueArguments { FieldRowName = CargoEnquiryForm.SearchFields.Operator, FieldRowValue = "MDH1" }])
           MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, CargoEnquiryForm.SearchFields.CargoID, "NAV59104A01");
           MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, CargoEnquiryForm.SearchFields.Operator, "MDH1", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
           cargoEnquiryForm.DoSearch();
           
            cargoEnquiryForm.DoReevaluateFreeStorageDays("ID^NAV59104A01~Opr^MDH1")
              .OpenTransactionsForm("ID^NAV59104A01~Opr^MDH1", out var transactionsForm);

            transactionsForm.ValidateLoggingForLine("Type^Information~Details^Free Storage Log",
                ["Storage Detail - Change of Ownership Free Storage Days Are - 5 All Days"]);

        }

        #region Setup and Load data
        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = "_59104_";

            // Delete Cargo OnSite
            CreateDataFileToLoad("DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\r\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\r\n   <AllCargoOnSite>\r\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\r\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\r\n      <CargoOnSite Terminal='TT1'>\r\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\r\n         <id>NAV59104A01</id>\r\n         <operatorCode>MDH1</operatorCode>\r\n         <locationId>MKBS01</locationId>\r\n         <weight>11999</weight>\r\n         <imexStatus>Import</imexStatus>\r\n         <commodity>GENL</commodity>\r\n         <dischargePort>NZAKL</dischargePort>\r\n         <voyageCode>MSCK000002</voyageCode>\r\n         <freightForwarder>MDHF1</freightForwarder>\r\n         <isoType>2200</isoType>\r\n         <messageMode>D</messageMode>\r\n      </CargoOnSite>\r\n   </AllCargoOnSite>\r\n</JMTInternalCargoOnSite>");
            // Create Cargo OnSite
            CreateDataFileToLoad("CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\r\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\r\n   <AllCargoOnSite>\r\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\r\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\r\n      <CargoOnSite Terminal='TT1'>\r\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\r\n         <id>NAV59104A01</id>\r\n         <operatorCode>MDH1</operatorCode>\r\n         <locationId>MKBS01</locationId>\r\n         <weight>11999</weight>\r\n         <imexStatus>Import</imexStatus>\r\n         <commodity>GENL</commodity>\r\n         <dischargePort>NZAKL</dischargePort>\r\n         <voyageCode>MSCK000002</voyageCode>\r\n         <freightForwarder>MDHF1</freightForwarder>\r\n         <isoType>2200</isoType>\r\n         <messageMode>A</messageMode>\r\n      </CargoOnSite>\r\n   </AllCargoOnSite>\r\n</JMTInternalCargoOnSite>");
            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
        #endregion Setup and Load data

    }

}
