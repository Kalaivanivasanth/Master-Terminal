using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40873 : MTNBase
    {

        PackUnpackForm _packUnpackForm;
        CargoEnquiryDirectForm _cargoEnquiryDirectForm;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;
        TransactionResyncForm _transactionResyncForm;

       [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() { base.TestCleanup(); }

        void MTNInitialize()
        {
            searchFor = @"_40873_";
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void PackedTransaction()
        {
            MTNInitialize();
            
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Pack/Unpack");
            _packUnpackForm = new PackUnpackForm();
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"Terminal Area Type", @"General Cargo Area",
                rowDataType: EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"Terminal Area", @"GCARA1", rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"Row", @"SMALL_SAND", rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblSearcher, @"ID", @"JLG40873A02");
            //packUnpackForm.btnFind.DoClick();
            _packUnpackForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_packUnpackForm.tblPackItems, @"ID^JLG40873A02");
            _packUnpackForm.TblPackItems.FindClickRow(["ID^JLG40873A02"]);
            MTNControlBase.SetValueInEditTable(_packUnpackForm.tblContainerToPack, @"Cargo ID", "JLG40873A01");
            //packUnpackForm.btnPack.DoClick();
            //packUnpackForm.btnSave.DoClick();
            _packUnpackForm.DoPack();
            _packUnpackForm.DoSave();

                warningErrorForm = new WarningErrorForm(@"Warnings for Pack/Unpack TT1");
            warningErrorForm.btnSave.DoClick();
            _packUnpackForm.PackTransactionConfirmation();
            _packUnpackForm.btnPackTransactionOK.DoClick();
            warningErrorForm = new WarningErrorForm(@"Warnings for Pack/Unpack TT1");
            warningErrorForm.btnSave.DoClick();
            // MTNControlBase.FindClickRowInTable(_packUnpackForm.tblPackedItems, @"ID^JLG40873A02",ClickType.DoubleClick);
            _packUnpackForm.TblPackedItems.FindClickRow(["ID^JLG40873A02"], ClickType.DoubleClick);
            _cargoEnquiryDirectForm = new CargoEnquiryDirectForm(@"JLG40873A02 TT1");
            
            _cargoEnquiryDirectForm.tblInfo.RightClick();
            _cargoEnquiryDirectForm.ContextMenuSelect("Transactions...");
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>();
            // MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^Packed", clickType: ClickType.ContextClick);
            //_cargoEnquiryTransactionForm.tblTransactions2.FindClickRow(["Type^Packed"], clickType: ClickType.ContextClick);
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(["Type^Packed"], clickType: ClickType.ContextClick);

            _cargoEnquiryTransactionForm.ContextMenuSelect(@"Resync Transactions...");
            _transactionResyncForm = new TransactionResyncForm();
            var controlValue = MTNControlBase.GetValue(_transactionResyncForm.pneCargoVolumeTxn);
            Assert.IsTrue(controlValue == @"1.5000 m3", @"Expected transaction cargo volume = 1.5000m3, actual value " + controlValue);

            controlValue = MTNControlBase.GetValue(_transactionResyncForm.pneCargoWeightTxn);
            Assert.IsTrue(controlValue == @"16534.683 lbs", @"Expected transaction cargo weight = 16534.683 lbs, actual value " + controlValue);
            
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {

            //Delete Cargo Onsite
            CreateDataFileToLoad(@"DeleteCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40873A01</id>\n            <isoType>220A</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>COS</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>MT</commodity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n         <product>SMSAND</product>\n         <id>JLG40873A02</id>\n         <operatorCode>COS</operatorCode>\n         <locationId>GCARA1 SMALL_SAND</locationId>\n         <weight>7500</weight>\n		 <volume>1.5</volume>\n         <imexStatus>Import</imexStatus>\n         <voyageCode>MSCK000002</voyageCode>\n         <totalQuantity>50</totalQuantity>\n         <quantity>50</quantity>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            //Create Cargo Onsite
            CreateDataFileToLoad(@"CreateCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40873A01</id>\n            <isoType>220A</isoType>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>COS</operatorCode>\n            <dischargePort>NZAKL</dischargePort>\n            <locationId>MKBS01</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <commodity>MT</commodity>\n            <messageMode>A</messageMode>\n        </CargoOnSite>\n		<CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n         <product>SMSAND</product>\n         <id>JLG40873A02</id>\n         <operatorCode>COS</operatorCode>\n         <locationId>GCARA1 SMALL_SAND</locationId>\n         <weight>7500</weight>\n		 <volume>1.5</volume>\n         <imexStatus>Import</imexStatus>\n         <voyageCode>MSCK000002</voyageCode>\n         <totalQuantity>50</totalQuantity>\n         <quantity>50</quantity>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");


            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);


        }




    }

}
