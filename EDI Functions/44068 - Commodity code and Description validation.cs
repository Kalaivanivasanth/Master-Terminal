using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44068 : MTNBase
    {
        PreNoteForm preNoteForm;
        EDIOperationsForm ediOperationsForm;

        protected static string ediFile1 = "M_44068_PrenoteAddSuccess.xml";
        protected static string ediFile2 = "M_44068_PrenoteAddFail.xml";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_44068_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //Add voyage 
            CreateDataFile(ediFile1,
               "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<Prenote Terminal='TT1'>\n            <cargoTypeDescr>GENERAL CARGO</cargoTypeDescr>\n			<id>JLG44068A01</id>\n             <commodity></commodity>\n			 <commodityDescription>Bottle Water</commodityDescription>\n			 <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS01</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n		\n		<Prenote Terminal='TT1'>\n            <cargoTypeDescr>GENERAL CARGO</cargoTypeDescr>\n			<id>JLG44068A02</id>\n             <commodity></commodity>\n			 <commodityDescription></commodityDescription>\n			 <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS01</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n				\n		<Prenote Terminal='TT1'>\n            <cargoTypeDescr>GENERAL CARGO</cargoTypeDescr>\n			<id>JLG44068A03</id>\n             <commodity>BOTT</commodity>\n			 <commodityDescription>Bottle Water</commodityDescription>\n			 <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS01</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n\n    </AllPrenote>\n</SystemXMLPrenote>\n\n\n");

            //Modify voyage 
            CreateDataFile(ediFile2,
               "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n		<Prenote Terminal='TT1'>\n            <cargoTypeDescr>GENERAL CARGO</cargoTypeDescr>\n			<id>JLG44068B01</id>\n             <commodity>Bott</commodity>\n			 <commodityDescription>Bottle Water</commodityDescription>\n			 <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS01</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote> \n		\n		<Prenote Terminal='TT1'>\n            <cargoTypeDescr>GENERAL CARGO</cargoTypeDescr>\n			<id>JLG44068B02</id>\n             <commodity>Bott</commodity>\n			 <commodityDescription></commodityDescription>\n			 <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS01</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n		\n		<Prenote Terminal='TT1'>\n            <cargoTypeDescr>GENERAL CARGO</cargoTypeDescr>\n			<id>JLG44068B03</id>\n             <commodity>Bott</commodity>\n			 <commodityDescription>Bott</commodityDescription>\n			 <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS01</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n		\n		<Prenote Terminal='TT1'>\n            <id>JLG44068B04</id>\n             <commodity></commodity>\n			 <commodityDescription></commodityDescription>\n			 <imexStatus>Export</imexStatus>\n             <weight>2000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>MKBS01</locationId>\n			<totalQuantity>1</totalQuantity>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n\n    </AllPrenote>\n</SystemXMLPrenote>\n\n\n");
            
            MTNSignon(TestContext);
        }

        [TestMethod]
        public void PreNoteCommodityAndDescriptionValidation()
        {
            MTNInitialize();

            // 1. Open pre-Notes form, find existing pre-notes and delete if any found
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Pre-Notes");
            preNoteForm = new PreNoteForm(formTitle: @"Pre-Notes TT1");
            preNoteForm.DeletePreNotes(@"JLG44068", @"ID^JLG44068");

            // 2. Open EDI Operations and delete any loaded messages relating to this test
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            ediOperationsForm.DeleteEDIMessages(@"Prenote", @"44068",ediStatus: @"Loaded");

            // 3. Load the Successfule prenote message and check the prenotes have been loaded
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);
            ediOperationsForm.ChangeEDIStatus(ediFile1, @"Loaded", @"Load To DB");

            preNoteForm.SetFocusToForm();
            MTNControlBase.FindClickRowInTable(preNoteForm.tblPreNotes, "ID^JLG44068A01~Commodity^BOTT", rowHeight: 18, doAssert: false, clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(preNoteForm.tblPreNotes, "ID^JLG44068A02~Commodity^GEN", rowHeight: 18, doAssert: false, clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(preNoteForm.tblPreNotes, "ID^JLG44068A03~Commodity^BOTT", rowHeight: 18, doAssert: false, clickType: ClickType.None);
            Wait.UntilInputIsProcessed(waitTimeout: TimeSpan.FromSeconds(1));

            //4. Load the prenote which will fail
            ediOperationsForm.SetFocusToForm();
            ediOperationsForm.LoadEDIMessageFromFile(ediFile2);
            ediOperationsForm.ChangeEDIStatus(ediFile2, @"Loaded", @"Verify");

            ediOperationsForm.ShowErrorWarningMessages();

            //5. check edi error messages for failure.
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIErrorWarningMessages, @"ID^JLG44068B01~Property^commodity~Error Message^Code :82804. ERROR: An invalid value 'Bott' was specified for field 'commodity'.", clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIErrorWarningMessages, @"ID^JLG44068B02~Property^commodity~Error Message^Code :82804. ERROR: An invalid value 'Bott' was specified for field 'commodity'.", clickType: ClickType.None);
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIErrorWarningMessages, @"ID^JLG44068B03~Property^commodity~Error Message^Code :82804. ERROR: An invalid value 'Bott' was specified for field 'commodity'.", clickType: ClickType.None);
           
        }




    }

}
