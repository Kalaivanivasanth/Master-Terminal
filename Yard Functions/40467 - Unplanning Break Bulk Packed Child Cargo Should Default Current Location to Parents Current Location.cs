using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions;
using MTNUtilityClasses.Classes;
using System;
using System.Diagnostics;
using DataObjects.LogInOutBO;
using HardcodedData.TerminalData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40647 : MTNBase
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            LogInto<MTNLogInOutBO>();
            CallJadeScriptToRun(TestContext, @"resetData_40647");
            SetupAndLoadInitializeData(TestContext);
        }


        [TestMethod]
        public void UnplanningBreakBulkPackedChildCargoDefaultCurrentLocationToParents()
        {
            MTNInitialize();
           
            // Step 4 
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            string[] detailsToSearchFor =
            {
                @"Cargo Type^^^^",
                @"Cargo ID^JLG40647^^^"
            };
            cargoEnquiryForm.SearchForCargoItems(detailsToSearchFor, @"ID^JLG40647A01~Location ID^MKBS03");

            // Step 5 - 9
            /*using (Keyboard.Pressing(VirtualKeyShort.CONTROL))
            {
                MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647A01~Location ID^MKBS03",
                    ClickType.Click);
                MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647A02~Location ID^MKBS01",
                    ClickType.ContextClick);
            }*/
            MTNControlBase.FindClickRowInTableMulti(cargoEnquiryForm.tblData2.GetElement(), [
                "ID^JLG40647A01~Location ID^MKBS03",
                "ID^JLG40647A02~Location ID^MKBS01"], ClickType.ContextClick);
            /*cargoEnquiryForm.tblData2.FindClickRow([
                "ID^JLG40647A01~Location ID^MKBS03",
                "ID^JLG40647A02~Location ID^MKBS01"
            ], ClickType.ContextClick, multiSelect: true);*/
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Move It...");

            CargoMoveItForm cargoMoveItForm = new CargoMoveItForm(formTitle: "Move TT1");
            //Keyboard.Release(VirtualKeyShort.CONTROL);

            MTNControlBase.SetValueInEditTable(cargoMoveItForm.tblMoveDetails, @"Move Mode", @"Planned Move", EditRowDataType.ComboBox,
                waitTime: 150);
            MTNControlBase.SetValueInEditTable(cargoMoveItForm.tblMoveDetails, @"To Terminal Area", @"MKBS07", EditRowDataType.ComboBoxEdit, waitTime: 150);

            //cargoMoveItForm.btnMoveIt.DoClick();
            cargoMoveItForm.DoMoveIt();
            
            ConfirmationFormYesNo confirmationFormYesNo = new ConfirmationFormYesNo(@"Multi Move");
            confirmationFormYesNo.CheckMessageMatch(@"Planned Move cargo item(s) JLG40647A01, JLG40647A02 to location MKBS07. Do you wish to continue?");
            confirmationFormYesNo.btnYes.DoClick();

            confirmationFormYesNo = new ConfirmationFormYesNo(@"Multi Move");
            confirmationFormYesNo.CheckMessageMatch(@"Enter 'Yes to ignore warnings and Planned Move the move(s) anyway, or 'No' to stop when a warning is encountered");
            confirmationFormYesNo.btnYes.DoClick();

            ConfirmationFormOK confirmationFormOK = new ConfirmationFormOK(@"Multi Move", automationIdOK: @"4");
            confirmationFormOK.CheckMessageMatch(@"2 Planned Move move(s) of 2 were successful.");
            confirmationFormOK.btnOK.DoClick();

            cargoMoveItForm.CloseForm();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647A01~Location ID^MKBS03~RT^P", ClickType.None);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647A02~Location ID^MKBS01~RT^P", ClickType.None);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647A03~Location ID^MKBS01", ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow([
                "ID^JLG40647A01~Location ID^MKBS03~RT^P",
                "ID^JLG40647A02~Location ID^MKBS01~RT^P",
            ], ClickType.None);
            cargoEnquiryForm.tblData2.FindClickRow(
                ["ID^JLG40647A03~Location ID^MKBS01"], ClickType.ContextClick);
           cargoEnquiryForm.ContextMenuSelect(@"Cargo|Move It...");

            cargoMoveItForm = new CargoMoveItForm(formTitle: "Move JLG40647A03 TT1");

            MTNControlBase.SetValueInEditTable(cargoMoveItForm.tblMoveDetails, @"Move Mode", @"Queued Move", EditRowDataType.ComboBox,
                waitTime: 150);
            MTNControlBase.SetValueInEditTable(cargoMoveItForm.tblMoveDetails, @"To Terminal Area", @"MKBS08", EditRowDataType.ComboBoxEdit, waitTime: 150);

            //cargoMoveItForm.btnMoveIt.DoClick();
            cargoMoveItForm.DoMoveIt();
            
            confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Move - 1 Items");
            confirmationFormYesNo.CheckMessageMatch(@"Queued Move cargo item(s) JLG40647A03 to location MKBS08. Do you wish to continue?");
            confirmationFormYesNo.btnYes.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Move TT1");
            string[] warningErrorToCheck =
            {
                "Code :82959. Item JLG40647A03 does not match Allocation E CONT MSC 51908 at location MKBS08"
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();

            cargoMoveItForm.CloseForm();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647A03~Location ID^MKBS01~RT^Q",
                // ClickType.None);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647B02~Location ID^GCARA1 SMALL_SAND",
                // ClickType.Click);
            /*cargoEnquiryForm.tblData2.FindClickRow([
                "ID^JLG40647A03~Location ID^MKBS01~RT^Q",
                "ID^JLG40647B02~Location ID^GCARA1 SMALL_SAND"
            ]);*/
;            /*using (Keyboard.Pressing(VirtualKeyShort.CONTROL))
            {
                MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647B02~Location ID^GCARA1 SMALL_SAND",
                    ClickType.Click);
                MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647B03~Location ID^GCARA1 SMALL_SAND",
                    ClickType.ContextClick);
            }*/
            MTNControlBase.FindClickRowInTableMulti(cargoEnquiryForm.tblData2.GetElement(), new[]
            {
                "ID^JLG40647B02~Location ID^GCARA1 SMALL_SAND",
                "ID^JLG40647B03~Location ID^GCARA1 SMALL_SAND" }, ClickType.ContextClick);
            /*cargoEnquiryForm.tblData2.FindClickRow([
                "ID^JLG40647B02~Location ID^GCARA1 SMALL_SAND",
                "ID^JLG40647B03~Location ID^GCARA1 SMALL_SAND"
            ], ClickType.ContextClick, multiSelect: true);*/
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Move It...");

            cargoMoveItForm = new CargoMoveItForm(formTitle: "Move TT1");
            //Keyboard.Release(VirtualKeyShort.CONTROL);

            MTNControlBase.SetValueInEditTable(cargoMoveItForm.tblMoveDetails, @"Move Mode", @"Planned Move", EditRowDataType.ComboBox,
                waitTime: 150);
            MTNControlBase.SetValueInEditTable(cargoMoveItForm.tblMoveDetails, @"To Terminal Area", @"GCARA2", EditRowDataType.ComboBoxEdit, waitTime: 150);
            MTNControlBase.SetValueInEditTable(cargoMoveItForm.tblMoveDetails, @"To Yard Location", @"SAND2", EditRowDataType.ComboBoxEdit, waitTime: 150);

            //cargoMoveItForm.btnMoveIt.DoClick();
            cargoMoveItForm.DoMoveIt();

            confirmationFormYesNo = new ConfirmationFormYesNo(@"Multi Move");
            confirmationFormYesNo.CheckMessageMatch(@"Planned Move cargo item(s) JLG40647B02, JLG40647B03 to location GCARA2 SAND2. Do you wish to continue?");
            confirmationFormYesNo.btnYes.DoClick();

            confirmationFormYesNo = new ConfirmationFormYesNo(@"Multi Move");
            confirmationFormYesNo.CheckMessageMatch(@"Enter 'Yes to ignore warnings and Planned Move the move(s) anyway, or 'No' to stop when a warning is encountered");
            confirmationFormYesNo.btnYes.DoClick();

            confirmationFormOK = new ConfirmationFormOK(@"Multi Move", automationIdOK: @"4");
            confirmationFormOK.CheckMessageMatch(@"2 Planned Move move(s) of 2 were successful.");
            confirmationFormOK.btnOK.DoClick();

            cargoMoveItForm.CloseForm();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647B02~Location ID^GCARA1 SMALL_SAND~RT^P",
                // ClickType.None);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647B03~Location ID^GCARA1 SMALL_SAND~RT^P",
                // ClickType.None);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647B04~Location ID^GCARA1 SMALL_SAND",
                // ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow([
                "ID^JLG40647B02~Location ID^GCARA1 SMALL_SAND~RT^P",
                "ID^JLG40647B03~Location ID^GCARA1 SMALL_SAND~RT^P",
            ], ClickType.None);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG40647B04~Location ID^GCARA1 SMALL_SAND"],
                ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Move It...");

            cargoMoveItForm = new CargoMoveItForm(formTitle: "Move JLG40647B04 TT1");

            MTNControlBase.SetValueInEditTable(cargoMoveItForm.tblMoveDetails, @"Move Mode", @"Planned Move", EditRowDataType.ComboBox,
                waitTime: 150);
            MTNControlBase.SetValueInEditTable(cargoMoveItForm.tblMoveDetails, @"To Terminal Area", TT1.TerminalArea.MKBS02, EditRowDataType.ComboBoxEdit, waitTime: 150);

            //cargoMoveItForm.btnMoveIt.DoClick();
            cargoMoveItForm.DoMoveIt();
            
            confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Move - 1 Items");
            confirmationFormYesNo.CheckMessageMatch(@"Planned Move cargo item(s) JLG40647B04 to location MKBS02.  Do you wish to continue?");
            confirmationFormYesNo.btnYes.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Move TT1");
            /*warningErrorToCheck = new string[]
            {
                "Code :82959. Item JLG40647B04 does not match Allocation E 44072 at location MKBS02"
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);*/
            warningErrorForm.btnSave.DoClick();

            cargoMoveItForm.CloseForm();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647B04~Location ID^GCARA1 SMALL_SAND~RT^P",
                // ClickType.None);
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647B05~Location ID^GCARA1 SMALL_SAND",
                // ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow([
                "ID^JLG40647B04~Location ID^GCARA1 SMALL_SAND~RT^P",
              //  "ID^JLG40647B05~Location ID^GCARA1 SMALL_SAND"
            ], ClickType.None);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG40647B05~Location ID^GCARA1 SMALL_SAND"],
                ClickType.ContextClick);
;            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Move It...");

            cargoMoveItForm = new CargoMoveItForm(formTitle: "Move JLG40647B05 TT1");

            MTNControlBase.SetValueInEditTable(cargoMoveItForm.tblMoveDetails, @"Move Mode", @"Planned Move", EditRowDataType.ComboBox,
                waitTime: 150);
            MTNControlBase.SetValueInEditTable(cargoMoveItForm.tblMoveDetails, @"To Terminal Area", TT1.TerminalArea.MKBS03, EditRowDataType.ComboBoxEdit, waitTime: 150);

            //cargoMoveItForm.btnMoveIt.DoClick();
            cargoMoveItForm.DoMoveIt();
            
            confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm Move - 1 Items");
            confirmationFormYesNo.CheckMessageMatch(@"Planned Move cargo item(s) JLG40647B05 to location MKBS03.  Do you wish to continue?");
            confirmationFormYesNo.btnYes.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Move TT1");
            warningErrorToCheck = new string[]
            {
                "Code :82959. Item JLG40647B05 does not match Allocation I CONT GENL MT MSCK000002 0439 2200 TRAI FLTP MKBS at location MKBS03"
            };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnSave.DoClick();

            cargoMoveItForm.CloseForm();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG40647B05~Location ID^GCARA1 SMALL_SAND~RT^P",
                // ClickType.None);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG40647B05~Location ID^GCARA1 SMALL_SAND~RT^P"], ClickType.None);

            cargoEnquiryForm.CloseForm();

            // Step 26 - 29
            PackUnpackForm packUnpackForm = OpenSearchPackUnpack();
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackItems, @"ID^JLG40647B01");
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackItems, @"ID^JLG40647B02");
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackItems, @"ID^JLG40647B03");
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackItems, @"ID^JLG40647B04");
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackItems, @"ID^JLG40647B05");
            packUnpackForm.TblPackItems.FindClickRow([
                "ID^JLG40647B01", "ID^JLG40647B02",
                "ID^JLG40647B03", "ID^JLG40647B04",
                "ID^JLG40647B05"
            ]);
            PackItemIntoContainer(packUnpackForm, @"JLG40647A01", @"JLG40647B01");

            // Step 30 - 33
            packUnpackForm = OpenSearchPackUnpack();
            PackItemIntoContainer(packUnpackForm, @"JLG40647A02", @"JLG40647B02");

            // Step 34 - 37
            packUnpackForm = OpenSearchPackUnpack();

            MTNControlBase.SetValueInEditTable(packUnpackForm.tblContainerToPack, @"Cargo ID", @"JLG40647A03");
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackItems, @"ID^JLG40647B03");
            packUnpackForm.TblPackItems.FindClickRow(["ID^JLG40647B03"]);
           // packUnpackForm.btnPack.DoClick();
            packUnpackForm.DoPack();
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackedItems, @"ID^JLG40647B03");
            packUnpackForm.TblPackedItems.FindClickRow(["ID^JLG40647B03"]);
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackItems, @"ID^JLG40647B04");
            packUnpackForm.TblPackItems.FindClickRow(["ID^JLG40647B04"]);
            //packUnpackForm.btnPack.DoClick();
            packUnpackForm.DoPack();
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackedItems, @"ID^JLG40647B04");
            packUnpackForm.TblPackedItems.FindClickRow(["ID^JLG40647B04"]);
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackItems, @"ID^JLG40647B05");
            packUnpackForm.TblPackItems.FindClickRow(["ID^JLG40647B05"]);
            //packUnpackForm.btnPack.DoClick();
            packUnpackForm.DoPack();
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackedItems, @"ID^JLG40647B05");
            packUnpackForm.TblPackedItems.FindClickRow(["ID^JLG40647B05"]);

            //packUnpackForm.btnSave.DoClick();
            packUnpackForm.DoSave();

            warningErrorForm = new WarningErrorForm(@"Warnings for Pack/Unpack TT1");
            warningErrorForm.btnSave.DoClick();

            packUnpackForm.PackTransactionConfirmation();
            packUnpackForm.btnPackTransactionOK.DoClick();

            warningErrorForm = new WarningErrorForm(@"Warnings for Pack/Unpack TT1");
            warningErrorForm.btnSave.DoClick();

            packUnpackForm.CloseForm();
            

            // Step 38
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            detailsToSearchFor = new string[]
            //string[] detailsToSearchFor =
            {
                @"Cargo Type^^^^",
                @"Cargo ID^JLG40647B^^^"
            };
            cargoEnquiryForm.SearchForCargoItems(detailsToSearchFor);

            // Step 39 - 44
            UnplanCheckLocation(@"JLG40647B01", TT1.TerminalArea.MKBS03, validateOnly: true);
            UnplanCheckLocation(@"JLG40647B02", TT1.TerminalArea.MKBS01);
            UnplanCheckLocation(@"JLG40647B03", TT1.TerminalArea.MKBS01);
            UnplanCheckLocation(@"JLG40647B04", TT1.TerminalArea.MKBS01);
            UnplanCheckLocation(@"JLG40647B05", TT1.TerminalArea.MKBS01);

        }

        private static PackUnpackForm OpenSearchPackUnpack()
        {
            FormObjectBase.NavigationMenuSelection(@"Yard Functions|Pack/Unpack", forceReset: true);
            PackUnpackForm packUnpackForm = new PackUnpackForm();

            MTNControlBase.SetValueInEditTable(packUnpackForm.tblSearcher, @"Terminal Area Type", @"General Cargo Area", EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(packUnpackForm.tblSearcher, @"Terminal Area", @"GCARA1", rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(packUnpackForm.tblSearcher, @"Row", @"SMALL_SAND", rowDataType: EditRowDataType.ComboBox);
            MTNControlBase.SetValueInEditTable(packUnpackForm.tblSearcher, @"ID", @"JLG40647B");
            //packUnpackForm.btnFind.DoClick();
            packUnpackForm.DoSearch();
            return packUnpackForm;
        }

        private void UnplanCheckLocation(string cargoId, string expectedLocation, bool validateOnly = false)
        {
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + cargoId + "~Location ID^" + expectedLocation,
                // ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^" + cargoId + "~Location ID^" + expectedLocation], ClickType.ContextClick);

            bool doAssert = validateOnly != true;
            Trace.TraceInformation(@"validateOnly: {0}    doAssert: {1}", validateOnly, doAssert);

            var menuItemFound = cargoEnquiryForm.ContextMenuSelect(@"Cargo|Unplan " + cargoId, validateOnly: validateOnly, doAssert: doAssert);
            Trace.TraceInformation(@"menuItemFound: {0}", menuItemFound);

            if (validateOnly)
            {
                Assert.IsFalse(menuItemFound,
                    @"TestCase40467::UnplanCheckLocation - Menu item Cargo|Unplan " + cargoId + @" should NOT exist");
            }
            
            cargoEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + cargoId + "~Location ID^" + expectedLocation);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^" + cargoId + "~Location ID^" + expectedLocation]);

            /* FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + cargoId, ClickType.ContextClick);
             cargoEnquiryForm.ContextMenuSelect(@"Cargo|Unplan " + cargoId);
             MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + cargoId + "~Location ID^" + expectedLocation,
                 ClickType.ContextClick);*/
        }

        private void PackItemIntoContainer(PackUnpackForm packUnpackForm, string containerId, string itemToPack, 
            bool doSave = true)
        {
            MTNControlBase.SetValueInEditTable(packUnpackForm.tblContainerToPack, @"Cargo ID", containerId);
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackItems, @"ID^" + itemToPack);
                        packUnpackForm.TblPackItems.FindClickRow(["ID^" + itemToPack]);
            //packUnpackForm.btnPack.DoClick();
            packUnpackForm.DoPack();
            // MTNControlBase.FindClickRowInTable(packUnpackForm.tblPackedItems, @"ID^" + itemToPack);
            packUnpackForm.TblPackedItems.FindClickRow(["ID^" + itemToPack]);

            if (doSave)
            {
                //packUnpackForm.btnSave.DoClick();
                packUnpackForm.DoSave();

                warningErrorForm = new WarningErrorForm(@"Warnings for Pack/Unpack TT1");
                warningErrorForm.btnSave.DoClick();

                packUnpackForm.PackTransactionConfirmation();
                packUnpackForm.btnPackTransactionOK.DoClick();

                warningErrorForm = new WarningErrorForm(@"Warnings for Pack/Unpack TT1");
                warningErrorForm.btnSave.DoClick();
            }

            packUnpackForm.CloseForm();

        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = @"_40647_";
            
            //Create Cargo Onsite
            CreateDataFileToLoad(@"CreateCargoOnsite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <TestCases>40647</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG40647A01</id>\n         <isoType>220A</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS03</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>MSCK000002</voyageCode>\n         <dischargePort>NZAKL</dischargePort>\n		 <messageMode>A</messageMode>\n      </CargoOnSite>\n      <CargoOnSite Terminal='TT1'>\n         <TestCases>40647</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG40647A02</id>\n         <isoType>220A</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>MSCK000002</voyageCode>\n         <dischargePort>NZAKL</dischargePort>\n		 <messageMode>A</messageMode>\n      </CargoOnSite>\n      <CargoOnSite Terminal='TT1'>\n         <TestCases>40647</TestCases>\n         <cargoTypeDescr>ISO Container</cargoTypeDescr>\n         <id>JLG40647A03</id>\n         <isoType>220A</isoType>\n         <operatorCode>MSL</operatorCode>\n         <locationId>MKBS01</locationId>\n         <weight>5000.0000</weight>\n         <imexStatus>Export</imexStatus>\n         <commodity>MT</commodity>\n         <voyageCode>MSCK000002</voyageCode>\n         <dischargePort>NZAKL</dischargePort>\n		 <messageMode>A</messageMode>\n      </CargoOnSite>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n         <product>SMSAND</product>\n         <id>JLG40647B01</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>GCARA1 SMALL_SAND</locationId>\n         <weight>7500</weight>\n         <imexStatus>Import</imexStatus>\n         <voyageCode>MSCK000002</voyageCode>\n         <totalQuantity>50</totalQuantity>\n         <quantity>50</quantity>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n       <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n         <product>SMSAND</product>\n         <id>JLG40647B02</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>GCARA1 SMALL_SAND</locationId>\n         <weight>7500</weight>\n         <imexStatus>Import</imexStatus>\n         <voyageCode>MSCK000002</voyageCode>\n         <totalQuantity>50</totalQuantity>\n         <quantity>50</quantity>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n         <product>SMSAND</product>\n         <id>JLG40647B03</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>GCARA1 SMALL_SAND</locationId>\n         <weight>7500</weight>\n         <imexStatus>Import</imexStatus>\n         <voyageCode>MSCK000002</voyageCode>\n         <totalQuantity>10</totalQuantity>\n         <quantity>10</quantity>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n         <product>SMSAND</product>\n         <id>JLG40647B03</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>GCARA1 SMALL_SAND</locationId>\n         <weight>7500</weight>\n         <imexStatus>Import</imexStatus>\n         <voyageCode>MSCK000002</voyageCode>\n         <totalQuantity>10</totalQuantity>\n         <quantity>10</quantity>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n         <product>SMSAND</product>\n         <id>JLG40647B04</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>GCARA1 SMALL_SAND</locationId>\n         <weight>7500</weight>\n         <imexStatus>Import</imexStatus>\n         <voyageCode>MSCK000002</voyageCode>\n         <totalQuantity>10</totalQuantity>\n         <quantity>10</quantity>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Bag of Sand</cargoTypeDescr>\n         <product>SMSAND</product>\n         <id>JLG40647B05</id>\n         <operatorCode>MSL</operatorCode>\n         <locationId>GCARA1 SMALL_SAND</locationId>\n         <weight>7500</weight>\n         <imexStatus>Import</imexStatus>\n         <voyageCode>MSCK000002</voyageCode>\n         <totalQuantity>10</totalQuantity>\n         <quantity>10</quantity>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);


        }




    }

}
