using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43763 : MTNBase
    {
        private HoldPlanningForm _holdPlanningForm;
        private OrderOfWorkForm _orderOfWorkForm;
        private ConfirmationFormYesNo _confirmationFormYesNo;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CallJadeScriptToRun(TestContext, @"resetData_43763");
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void DischargeToYardInterchangeArea()
        {
            MTNInitialize();

            // 1. set dates for the test
            var dateToday = DateTime.Today.Date;
            var dateTomorrow = dateToday.AddDays(1);
            
            // 2. Navigate to Hold Planning
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|Hold Planning");
            _holdPlanningForm = new HoldPlanningForm();

            // 3. Get hold planning details for MSCK000002 and tab to Unallocated cargo on vessel
            _holdPlanningForm.cmbVoyage.SetValue(TT1.Voyage.MSCK000002FullSpace);
            _holdPlanningForm.GetTabTableGeneric(@"Unallocated Cargo On Vessel", @"2027");
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric,@"ID^JLG43763A01",ClickType.Click, rowHeight: 16);
             _holdPlanningForm.TabGeneric.TableWithHeader.FindClickRow(["ID^JLG43763A01"], ClickType.Click);
            //_holdPlanningForm.TblGeneric.FindClickRow(["ID^JLG43763A01"], ClickType.Click);
            _holdPlanningForm.DoNewHoldPlanning();
            _holdPlanningForm.cmbWorkPoint.SetValue("WP0001 - Work point WP0001 description", searchSubStringTo: 6, additionalWaitTimeout: 200, doDownArrow: true);
            _holdPlanningForm.txtJobId.SetValue(@"43763JOB");
            _holdPlanningForm.cmbTransferType.SetValue(VoyageJobTransferType.LIFTONLISTOFFLOLO,
                additionalWaitTimeout: 1000, doDownArrow: true, searchSubStringTo: VoyageJobTransferType.LIFTONLISTOFFLOLO.Length - 1);
            _holdPlanningForm.txtShiftStartDate.SetValue(dateToday.ToString(@"ddMMyyyy"));
            _holdPlanningForm.txtShiftStartTime.SetValue(@"1200");
            _holdPlanningForm.txtShiftEndDate.SetValue(dateTomorrow.ToString(@"ddMMyyyy"));
            _holdPlanningForm.txtShiftEndTime.SetValue(@"1200");
            _holdPlanningForm.cmbPlanDischargeTo.SetValue(@TT1.TerminalArea.ICA, additionalWaitTimeout: 500);
            _holdPlanningForm.DoSaveHoldPlanning();

            // 5. go to the new job tab and find the planned cargo
            _holdPlanningForm.GetTabTableGeneric(@"43763JOB Discharge");
             // MTNControlBase.FindClickRowInTable(_holdPlanningForm.tblGeneric,@"ID^JLG43763A01", ClickType.Click);
             _holdPlanningForm.TabGeneric.TableWithHeader.FindClickRow(["ID^JLG43763A01"], ClickType.Click);
            //_holdPlanningForm.TblGeneric.FindClickRow(["ID^JLG43763A01"], ClickType.Click);
            _holdPlanningForm.CloseForm();

            //6. Unplan the cargo
            //FormObjectBase.NavigationMenuSelection(@"General Functions|Cargo Enquiry", forceReset: true);
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG43763A01");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", Site.OnShip, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^JLG43763A01", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^JLG43763A01"], clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Unplan JLG43763A01");
            cargoEnquiryForm.CloseForm();

            // 7. Open Order of works form and select for voyage
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|Order Of Work", forceReset: true);

            _orderOfWorkForm = new OrderOfWorkForm(@"Order of Work TT1");
            _orderOfWorkForm.cmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            _orderOfWorkForm.DoFind();
            _orderOfWorkForm.CreateOOWAllJobsTable();
             // MTNControlBase.FindClickRowInTable(_orderOfWorkForm.tblOOWAllJobs,@"Job ID^43763JOB", ClickType.Click,noOfHeaderRows: 3,rowHeight: 16);
             _orderOfWorkForm.tblOOWAllJobs2.FindClickRow(["Job ID^43763JOB"]);
            //_orderOfWorkForm.TblOOWAllJobs.FindClickRow(["Job ID^43763JOB"], ClickType.Click, noOfHeaderRows: 3);
            //_orderOfWorkForm.btnDelete.DoClick();
            _orderOfWorkForm.DoDelete();
            _confirmationFormYesNo = new ConfirmationFormYesNo(formTitle: @"Confirm Job Delete");
            _confirmationFormYesNo.btnYes.DoClick();
            
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            searchFor = "_43763_";
            
            // Create Cargo OnShip
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?> <JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'> \n    <AllCargoOnShip> \n    <operationsToPerform>Verify;Load To DB</operationsToPerform> \n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses> \n    <CargoOnShip Terminal='TT1'> \n        <cargoTypeDescr>ISO Container</cargoTypeDescr> \n        <isoType >2200</isoType> \n        <id>JLG43763A01</id> \n        <voyageCode>MSCK000002</voyageCode> \n        <operatorCode>MSK</operatorCode> \n        <dischargePort>USJAX</dischargePort> \n        <locationId>030986</locationId> \n        <weight>6000</weight> \n       <imexStatus> Import</imexStatus> \n       <totalQuantity>1</totalQuantity> \n        <commodity>GENL</commodity> \n        <messageMode>A</messageMode> \n    </CargoOnShip> \n  </AllCargoOnShip> \n </JMTInternalCargoOnShip> \n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }



    }

}
