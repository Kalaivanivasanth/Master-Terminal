using System;
using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNUtilityClasses.Classes;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General.Future_Storage
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45834 : MTNBase
    {
        readonly string[] _detailsToCheck = new[]
        {
            "Request Type^Future Storage Process~Terminal^TT1~Cargo Id^JLG45834A01",
            "Request Type^Future Storage Process~Terminal^TT1~Cargo Id^JLG45834A02"
        };
        

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() =>base.TestCleanup();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void FutureStorageBGPEntriesCreatedCorrectly()
        {
            MTNInitialize();
            
            // Step 3
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.Blank, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG45834");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", Site.Anywhere, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);

            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.tblData2.FindClickRow(
                new[]
                {
                    "ID^JLG45834A01~Cargo Type^Clinker~State^Prenotified",
                    "ID^JLG45834A02~Cargo Type^STEEL~State^On ship"
                }, ClickType.Click);

            FormObjectBase.MainForm.OpenBackgroundProcessingFromToolbar();
            var backgroundApplicationForm = new BackgroundApplicationForm();
            backgroundApplicationForm.CheckFutureStorageBGP(new FutureStorageDetailsToValidateOrDeleteArgs
            {
                CargoId = "JLG45834A*",
                DetailsToValidateOrDelete = _detailsToCheck,
                DoAssert = false
            }, out var tableDetailsNotFound);
            Assert.IsTrue(!string.IsNullOrEmpty(tableDetailsNotFound) && tableDetailsNotFound.Length != 2,
                $"TestCase45834 -  Expected to NOT find Future Storage requests:\r\n{string.Join(Environment.NewLine, _detailsToCheck)}\r\nBUT only did NOT find:\r\n{string.Join(Environment.NewLine, tableDetailsNotFound)}");
            
            // Step 6
            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.GetGenericTabTableDetails(@"General", @"4042");

            // Step 7
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG45834A01~Cargo Type^Clinker~State^Prenotified" });
            cargoEnquiryForm.DoEdit();

            // Step 8
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Total Weight", @"15000");
            cargoEnquiryForm.DoSave();

            // Step 9
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG45834A02~Cargo Type^STEEL~State^On ship" });
            cargoEnquiryForm.DoEdit();

            // Step 10
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblGeneric, @"Total Weight", @"15000");
            cargoEnquiryForm.DoSave();

            // Step 4
            backgroundApplicationForm.SetFocusToForm();
      
            FormObjectBase.MainForm.OpenBackgroundProcessingFromToolbar();
            backgroundApplicationForm = new BackgroundApplicationForm();
            backgroundApplicationForm.CheckFutureStorageBGP(new FutureStorageDetailsToValidateOrDeleteArgs
            {
                CargoId = "JLG45834A*",
                DetailsToValidateOrDelete = _detailsToCheck,
                DoAssert = false
            }, out  tableDetailsNotFound);
            Assert.IsTrue(!string.IsNullOrEmpty(tableDetailsNotFound) && tableDetailsNotFound.Length != 2,
                $"TestCase45834 -  Expected to NOT find Future Storage requests:\r\n{string.Join(Environment.NewLine, _detailsToCheck)}\r\nBUT only did NOT find:\r\n{string.Join(Environment.NewLine, tableDetailsNotFound)}");
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_45834_";

            // Delete Cargo On Ship
            CreateDataFileToLoad(@"DeleteCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n <JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n			<cargoTypeDescr>STEEL</cargoTypeDescr>\n            <product>PIPE</product>\n            <id>JLG45834A02</id>\n            <operatorCode>PAC</operatorCode>\n            <locationId>MSCK000005</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>MSCK000005</voyageCode>\n            <totalQuantity>300</totalQuantity>\n		   <messageMode>D</messageMode>\n        </CargoOnShip>\n          </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // Delete Prenote
            CreateDataFileToLoad(@"DeletePrenote.xml",
                "<?xml version='1.0'?>\n	<JMTInternalPrenote\n	xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalPrenote.xsd'>\n		<AllPrenote>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n			<Prenote Terminal='TT1'>\n				<cargoTypeDescr>Clinker</cargoTypeDescr>\n				<commodity>CLNK</commodity>\n				<dischargePort>USJAX</dischargePort>\n				<id>JLG45834A01</id>\n				<imexStatus>Import</imexStatus>\n				<messageMode>D</messageMode>\n				<operatorCode>PAC</operatorCode>\n				<weight>6000</weight>\n				<voyageCode>MSCK000005</voyageCode>\n				<totalQuantity>200</totalQuantity>\n			</Prenote>\n		</AllPrenote>\n	</JMTInternalPrenote>");

            // Create Cargo On Ship
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n			<cargoTypeDescr>STEEL</cargoTypeDescr>\n            <product>PIPE</product>\n            <id>JLG45834A02</id>\n            <operatorCode>PAC</operatorCode>\n            <locationId>MSCK000005</locationId>\n            <weight>6000</weight>\n            <imexStatus>Import</imexStatus>\n            <dischargePort>USJAX</dischargePort>\n            <voyageCode>MSCK000005</voyageCode>\n            <totalQuantity>300</totalQuantity>\n		   <messageMode>A</messageMode>\n        </CargoOnShip>\n          </AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // Create Prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n	<JMTInternalPrenote xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalPrenote.xsd'>\n\n		<AllPrenote>\n		<operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n\n			<Prenote Terminal='TT1'>\n				<cargoTypeDescr>Clinker</cargoTypeDescr>\n				<commodity>CLNK</commodity>\n				<dischargePort>USJAX</dischargePort>\n				<id>JLG45834A01</id>\n				<imexStatus>Import</imexStatus>\n				<messageMode>A</messageMode>\n				<operatorCode>PAC</operatorCode>\n				<weight>6000</weight>\n				<voyageCode>MSCK000005</voyageCode>\n				<totalQuantity>200</totalQuantity>\n			</Prenote>\n		</AllPrenote>\n	</JMTInternalPrenote>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads

    }

}
