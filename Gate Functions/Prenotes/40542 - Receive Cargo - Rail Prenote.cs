using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using System.Windows.Forms;
using System.Windows.Input;
using DataObjects.LogInOutBO;
using FlaUI.Core.WindowsAPI;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Prenotes
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40542 : MTNBase
    {
        PreNoteForm _preNoteForm;
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

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
        public void ReceiveCargoRailPrenote()
        {
            MTNInitialize();
            
            // Step 4 Open Gate Functions | Pre-Notes
            FormObjectBase.MainForm.OpenPreNotesFromToolbar();

            // Step 5 Enter Transport Mode -Rail, Cargo Id -@cargoID and click the Search button
            _preNoteForm = new PreNoteForm(@"Pre-Notes TT1");
            _preNoteForm.SetTransportMode("Rail");
            MTNControlBase.SetValueInEditTable(_preNoteForm.tblPreNoteSearch, @"Cargo Id", @"JLG40542A01");
            _preNoteForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_preNoteForm.tblPreNotes, "ID^JLG40542A01", rowHeight: 18, doAssert: false, clickType:ClickType.ContextClick);
            _preNoteForm.TblPreNotes.FindClickRow(["ID^JLG40542A01"], doAssert: false, clickType:ClickType.ContextClick);            _preNoteForm.ContextMenuSelect("Receive Cargo");

            // Step 7 Enter Transfer Area -40542A and click the OK button:
            _preNoteForm.GetTransferAreaScreen();
            _preNoteForm.cmbTransferArea.SetValue("40542A", searchSubStringTo: 4, additionalWaitTimeout: 200, doDownArrow: true);
            _preNoteForm.btnTransferAreaOK.DoClick();


            // Step 8 Enter Transport Mode -Road, Cargo Id -@cargoID1 and click the Search button
            _preNoteForm.SetTransportMode("Road");
            MTNControlBase.SetValueInEditTable(_preNoteForm.tblPreNoteSearch, @"Cargo Id", @"JLG40542A02");
            _preNoteForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_preNoteForm.tblPreNotes, "ID^JLG40542A02", rowHeight: 18, doAssert: false, clickType: ClickType.ContextClick);
            _preNoteForm.TblPreNotes.FindClickRow(["ID^JLG40542A02"], doAssert: false, clickType: ClickType.ContextClick);            _preNoteForm.ContextMenuSelect("Receive Cargo");

            // Step 10 Enter Transfer Area -40542B and click the OK button
            _preNoteForm.GetTransferAreaScreen();
            _preNoteForm.cmbTransferArea.SetValue("40542B", searchSubStringTo: 4, additionalWaitTimeout: 200, doDownArrow: true);
            _preNoteForm.btnTransferAreaOK.DoClick();
           
            // Step 11 Enter Transport Mode -Road & Rail, Cargo Id - << Cleared >> and click the Search button
            _preNoteForm.SetTransportMode("Road & Rail");
            MTNControlBase.SetValueInEditTable(_preNoteForm.tblPreNoteSearch, @"Cargo Id", @"");
            _preNoteForm.DoSearch();

            // Step 12 Right Click on the Result Table - The Context Menu will pop-up and "Receive Cargo" is NOT one of the option
            _preNoteForm.TblPreNotes.GetElement().RightClick();
            Assert.IsFalse(_preNoteForm.ContextMenuSelect(@"Receive Cargo", validateOnly: true));

            // Step 13 Choose Additions in the Transport Mode and Click the Search button 
            _preNoteForm.SetTransportMode("Additions");
            _preNoteForm.DoSearch();

            // Step 14 Right Click on the Result Table  - The Context Menu will pop-up and "Receive Cargo" is NOT one of the option 
            _preNoteForm.TblPreNotes.GetElement().RightClick();
            Assert.IsFalse(_preNoteForm.ContextMenuSelect(@"Receive Cargo", validateOnly: true));
            MTNKeyboard.Press(VirtualKeyShort.ESC);

            // Step 15 Open General Functions | Cargo Enquiry
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            // Step 16 Enter Cargo ID - JLG40542A01 and click the Search button
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG40542A01");
            cargoEnquiryForm.DoSearch();

            // Step 17 Select the @cargoID and from the context menu (Right - Click) select Transactions
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40542A01", ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(new [] { "ID^JLG40542A01" }, ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Transactions...");
            _cargoEnquiryTransactionForm = FormObjectBase.CreateForm<CargoEnquiryTransactionForm>(@"Transactions for JLG40542A01 TT1");
            /*// Tuesday, 28 January 2025 navmh5 
            MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions,
                @"Type^Received - Rail");*/
            _cargoEnquiryTransactionForm.TblTransactions2.FindClickRow(new [] { "Type^Received - Rail" });
            _cargoEnquiryTransactionForm.CloseForm();


        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = @"_40542_";
                        
            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40542A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>40542A</locationId>\n            <weight>5000</weight>\n            <imexStatus>Import</imexStatus>\n            <commodity>MT</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <isoType>2200</isoType>\n			<messageMode>D</messageMode>\n			  </CargoOnSite>\n			<CargoOnSite Terminal='TT1'>\n			  <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40542A02</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>40542B</locationId>\n            <weight>5000</weight>\n            <imexStatus>Import</imexStatus>\n            <commodity>MT</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <isoType>2200</isoType>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n");

            // Create Prenote
            CreateDataFileToLoad(@"CreatePrenote.xml",
                "<?xml version='1.0'?>\n<JMTInternalPrenote>\n<AllPrenote>\n<operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n<Prenote Terminal='TT1'>\n	<cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40542A01</id>\n            <isoType>2200</isoType>\n            <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Rail</transportMode>\n			<messageMode>A</messageMode>\n			</Prenote>\n			<Prenote Terminal='TT1'>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG40542A02</id>\n            <isoType>2200</isoType>\n            <commodity>GEN</commodity>\n            <imexStatus>Export</imexStatus>\n             <weight>1000</weight>\n            <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n</Prenote>\n</AllPrenote>\n</JMTInternalPrenote>\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }
}
