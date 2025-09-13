using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Terminal_Functions;
using MTNForms.FormObjects.Yard_Functions;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Pack_Container;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43635 : MTNBase
    {

        PackUnpackForm _packUnpackForm;
      
       [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }

   
        [TestMethod]
        public void UnpackImexStatus()
        {
            MTNInitialize();

            /* To test that
             *
             * 1. with the Terminal Configs "Bulk Pack/Unpack Complete Transaction Container Processing" set to True,
             * when cargo is packed via Pack/Unpack form, the container's Content status and IMEX do not change.
             * 2. with the Terminal Config "Bulk Pack/Unpack Complete Transaction Container Processing" set to True,
             * when cargo is unpacked via Pack/Unpack form, the container's Content status and IMEX do not change
            */

            //1. Open Pack/Unpack Form and Search for cargo to pack
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Pack/Unpack", forceReset: true);
            _packUnpackForm = new PackUnpackForm();
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"Terminal Area Type", @"General Cargo Area", EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"Terminal Area", @"GCARA1", rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"Row", @"SMALL_SAND", rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"ID", @"JLG43635A02");
            _packUnpackForm.DoSearch();

            //2. Pack the cargo into container JLG43635A02
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_packUnpackForm.tblPackItems, @"ID^JLG43635A02");
            _packUnpackForm.TblPackItems.FindClickRow(new[] { "ID^JLG43635A02" });
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblContainerToPack, @"Cargo ID", @"JLG43635A01");
            _packUnpackForm.DoPack();
            _packUnpackForm.DoSave();
            _packUnpackForm.BasicSave();

            //3. Open cargo enquiry and check the container is set to FCL and Import
            // Tuesday, 28 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID",@"JLG43635A01");
            cargoEnquiryForm.DoSearch();
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43635A01");
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG43635A01" });
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            string strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Status");
            Assert.IsTrue(strValue == @"FCL", "Status expected to be FCL, actual is " + strValue);
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"IMEX Status");
            Assert.IsTrue(strValue == @"Import", "IMEX status expected to be Import, actual is " + strValue);

            //4. Go back to pack/unpack and unpack container
            _packUnpackForm.SetFocusToForm();
            // Wednesday, 26 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_packUnpackForm.tblPackedItems, @"ID^JLG43635A02");
            _packUnpackForm.TblPackedItems.FindClickRow(new[] { "ID^JLG43635A02" });
            _packUnpackForm.DoUnpack();
            _packUnpackForm.DoSave();
            _packUnpackForm.BasicSave();

            //5. change the status of the container to import
           
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblContainerToPack, @"ImEx Status", @"Import", rowDataType: EditRowDataType.ComboBox, doReverse: true);
            _packUnpackForm.DoSave();
            _packUnpackForm.BasicSave();

            //6. in cargo enquiry container should be status MT and Import
            cargoEnquiryForm.SetFocusToForm();
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43635A01");
            cargoEnquiryForm.tblData2.FindClickRow(new[] { "ID^JLG43635A01" });
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Status");
            Assert.IsTrue(strValue == @"MT", "Status expected to be MT, actual is " + strValue);
            strValue = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"IMEX Status");
            Assert.IsTrue(strValue == @"Import", "IMEX status expected to be Import, actual is " + strValue);


        }

        private void OpenSearchPackUnpack()
        {
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Pack/Unpack", forceReset: true);
            _packUnpackForm = new PackUnpackForm();
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"Terminal Area Type", @"General Cargo Area",
                EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"Terminal Area", @"GCARA1",
                rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"Row", @"SMALL_SAND",
                rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"ID", @"JLG43635A02");
            _packUnpackForm.DoSave();
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_43635_";

            //Delete Cargo Onsite
            CreateDataFileToLoad(@"DeleteCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		  <TestCases>43635</TestCases>\n		  <cargoTypeDescr>ISO Container</cargoTypeDescr>\n		  <id>JLG43635A01</id>\n		  <isoType>220A</isoType>\n		  <operatorCode>MSK</operatorCode>\n		  <locationId>MKBS01</locationId>\n		  <weight>1814.0000</weight>\n		  <imexStatus>Import</imexStatus>\n		  <commodity>GEN</commodity>\n		  <dischargePort>NZAKL</dischargePort>\n		  <voyageCode>MSCK000002</voyageCode>\n		  <messageMode>D</messageMode>\n    </CargoOnSite>\n	<CargoOnSite Terminal='TT1'>\n            <TestCases>43635</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG43635A02</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>454.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>300</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>454.000</tareWeight>\n            <weight>907.000</weight>\n		   <messageMode>D</messageMode>\n        </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //Create Cargo Onsite
            CreateDataFileToLoad(@"CreateCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnSite Terminal='TT1'>\n		  <TestCases>43635</TestCases>\n		  <cargoTypeDescr>ISO Container</cargoTypeDescr>\n		  <id>JLG43635A01</id>\n		  <isoType>220A</isoType>\n		  <operatorCode>MSK</operatorCode>\n		  <locationId>MKBS01</locationId>\n		  <weight>1814.0000</weight>\n		  <imexStatus>Import</imexStatus>\n		  <commodity>MT</commodity>\n		  <dischargePort>NZAKL</dischargePort>\n		  <voyageCode>MSCK000002</voyageCode>\n		  <messageMode>A</messageMode>\n    </CargoOnSite>\n	<CargoOnSite Terminal='TT1'>\n            <TestCases>43635</TestCases>\n            <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n            <product>SMSAND</product>\n            <id>JLG43635A02</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>GCARA1 SMALL_SAND</locationId>\n            <cargoWeight>454.0000</cargoWeight>\n            <imexStatus>Export</imexStatus>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <totalQuantity>300</totalQuantity>\n            <commodity>SANC</commodity>\n            <tareWeight>454.000</tareWeight>\n            <weight>907.000</weight>\n		   <messageMode>A</messageMode>\n        </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>");


            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);


        }




    }

}
