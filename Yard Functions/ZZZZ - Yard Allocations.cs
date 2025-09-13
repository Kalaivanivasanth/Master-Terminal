using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Yard_Functions.Yard_Allocation_Manager;
using MTNGlobal.EnumsStructs;
using DataObjects.LogInOutBO;

namespace PNLAutomatedTests.TestCases.Master_Terminal.Yard_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]

    public class TestCaseYardAllocations : MTNBase
    {
        YardAllocationManagerForm yardAllocationManagerForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() { base.TestCleanup(); }

        private void MTNInitialize()
        {
            searchFor = "_53950_";
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void YardAllocations()
        {
            // MTN Initialize
            MTNInitialize();

            // Open Yard Allocation form
            FormObjectBase.NavigationMenuSelection(@"Yard Functions | Yard Allocation Manager");
            yardAllocationManagerForm = new YardAllocationManagerForm(@"Yard Allocation Manager PNL");
             MTNControlBase.FindClickRowInTable(yardAllocationManagerForm.tblYardAllocations.GetElement(),
               "Name^TEST 53950", noOfHeaderRows:2);

            //yardAllocationManagerForm.btnEdit.DoClick();
            yardAllocationManagerForm.DoEdit();
        }


        #region - Setup and Initalize Data

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='PNL'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>PNL53604A01</id>\n            <operatorCode>MSC</operatorCode>\n            <locationId>Q5 011 1</locationId>\n            <weight>500</weight>\n            <imexStatus>Import</imexStatus>\n            <commodity>MT</commodity>\n            <dischargePort>NZPOE</dischargePort>\n            <voyageCode>CALI00001</voyageCode>\n            <isoType>4510</isoType>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n    <CargoOnSite Terminal='PNL'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>PNL53604A02</id>\n            <operatorCode>MSC</operatorCode>\n            <locationId>Q5 022 2</locationId>\n            <weight>500</weight>\n            <imexStatus>Import</imexStatus>\n            <commodity>MT</commodity>\n            <dischargePort>NZPOE</dischargePort>\n                   <isoType>2210</isoType>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n         </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");


            // Create Cargo On Site
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='PNL'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>PNL53604A01</id>\n            <operatorCode>MSC</operatorCode>\n            <locationId>Q5 011 1</locationId>\n            <weight>5000</weight>\n            <imexStatus>Storage</imexStatus>\n            <commodity>MT</commodity>\n            <dischargePort>NZPOE</dischargePort>\n            <voyageCode>CALI00001</voyageCode>\n            <isoType>4510</isoType>\n			<messageMode>A</messageMode>\n			  </CargoOnSite>\n         </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");


            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Initalize Data

    }
}
