using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Bulk_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNForms.FormObjects;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Bulk_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase44042 : MTNBase
    {
        BulkGateInForm bulkGateInForm;
        BulkGateOutForm bulkGateOutForm;
        
        protected static string ediFile1 = "M_44042_AddPrenote.xml";

        // setting the cargo ID and release request number to be (essentially) unique
        // unless run in same minute on different day
        // easier than resetting the release request if there is a re-run
        protected static Int32 minutes = (Int32)DateTime.Now.TimeOfDay.TotalMinutes;
        protected static Int32 day = (Int32)DateTime.Now.DayOfYear;
        protected static string suffix = day.ToString() + minutes.ToString();
        protected static string strCargoID = @"MT44042" + suffix ;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            searchFor = @"M_44042_";
            
            // CreateDataFile(ediFile1,
            //"<?xml version='1.0'?> <JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n  <AllCargoOnSite>\n    <CargoOnSite Terminal='TT1'>\n     \n            <cargoTypeDescr>COAL44042</cargoTypeDescr>\n            <id>" + strCargoID +"</id>\n            <imexStatus>Export</imexStatus>\n             <weight>8000000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>BULK</locationId>\n			<transportMode>Road</transportMode>\n			<messageMode>D</messageMode>\n        \n    </CargoOnSite>\n  </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n");

            CreateDataFile(ediFile1,
             "<?xml version='1.0' encoding='UTF-8'?>\n<SystemXMLPrenote>\n    <AllPrenote>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <Prenote Terminal='TT1'>\n            <cargoTypeDescr>COAL44042</cargoTypeDescr>\n            <id>" + strCargoID +"</id>\n            <imexStatus>Export</imexStatus>\n             <weight>8000000</weight>\n			 <voyageCode>MSCK000002</voyageCode>\n            <operatorCode>MSC</operatorCode>\n			<dischargePort>NZAKL</dischargePort>\n			<locationId>BULK</locationId>\n			<transportMode>Road</transportMode>\n			<messageMode>A</messageMode>\n        </Prenote>\n    </AllPrenote>\n</SystemXMLPrenote>\n\n\n");

            LogInto<MTNLogInOutBO>();
            
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            EDIOperationsForm ediOperations = new EDIOperationsForm(@"EDI Operations TT1");  
            ediOperations.DeleteEDIMessages(@"Prenote", @"44042", ediStatus: @"Loaded");
            ediOperations.LoadEDIMessageFromFile(ediFile1);
            ediOperations.ChangeEDIStatus(ediFile1, @"Loaded", @"Load To DB");
            ediOperations.CloseForm();
        }


        [TestMethod]
        public void BulkGateWeightDiscrepency()
        {
            
            MTNInitialize();
            
            //1 Go to Bulk Gate In and set truck weight
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate In", forceReset: true);
            bulkGateInForm = new BulkGateInForm(vehicleId: @"44042");
            //MTNControlBase.SetValue(bulkGateInForm.cmbGate, @"North");
            bulkGateInForm.cmbGate.SetValue(@"North_Gate");
            //MTNControlBase.SetValue(bulkGateInForm.txtRegistration, @"44042");
            bulkGateInForm.txtRegistration.SetValue(@"44042");
            //MTNControlBase.SetValue(bulkGateInForm.cmbCarrier, @"American Auto Tpt");
            bulkGateInForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            //MTNControlBase.SetValue(bulkGateInForm.txtNewItem, strCargoID);
            bulkGateInForm.txtNewItem.SetValue(strCargoID, 10);
            //MTNControlBase.SetWeightValues(bulkGateInForm.tblScaleWeight,@"80", @"MT");
            bulkGateInForm.tblScaleWeight.SetValueAndType(@"80", @"MT");
            bulkGateInForm.btnSave.DoClick();
 
            //warningErrorForm = new WarningErrorForm(@"Warnings for Bulk Gate In TT1");
            //warningErrorForm.btnSave.DoClick();
            bulkGateInForm.CloseForm();

            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate Out", forceReset: true);
            bulkGateOutForm = new BulkGateOutForm();
            // MTNControlBase.FindClickRowInTable(bulkGateOutForm.tblGateInList, @"Vehicle^44042", clickType: ClickType.Click, rowHeight: 16);
            bulkGateOutForm.TblGateInList.FindClickRow(["Vehicle^44042"], clickType: ClickType.Click);            bulkGateOutForm.SetWeightValues(@"20", @"MT");
            bulkGateOutForm.chkReceiptRequired.DoClick(false);
            bulkGateOutForm.btnSave.DoClick();
            bulkGateOutForm.CloseForm();

            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate In", forceReset: true);
            bulkGateInForm = new BulkGateInForm(vehicleId: @"44042");
            //MTNControlBase.SetValue(bulkGateInForm.cmbGate, @"North");
            bulkGateInForm.cmbGate.SetValue(@"North_Gate");
            //MTNControlBase.SetValue(bulkGateInForm.txtRegistration, @"44042");
            bulkGateInForm.txtRegistration.SetValue(@"44042");
            //MTNControlBase.SetValue(bulkGateInForm.cmbCarrier, @"American Auto Tpt");
            bulkGateInForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            //MTNControlBase.SetValue(bulkGateInForm.txtNewItem, strCargoID);
            bulkGateInForm.txtNewItem.SetValue(strCargoID, 10);
            //MTNControlBase.SetWeightValues(bulkGateInForm.tblScaleWeight,@"60", @"MT");
            bulkGateInForm.tblScaleWeight.SetValueAndType(@"60", @"MT");
            bulkGateInForm.btnSave.DoClick();

            //warningErrorForm = new WarningErrorForm(@"Warnings for Bulk Gate In TT1");
            //warningErrorForm.btnSave.DoClick();
            bulkGateInForm.CloseForm();

            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate Out", forceReset: true);
            bulkGateOutForm = new BulkGateOutForm();
            // MTNControlBase.FindClickRowInTable(bulkGateOutForm.tblGateInList, @"Vehicle^44042", clickType: ClickType.Click, rowHeight: 16);
            bulkGateOutForm.TblGateInList.FindClickRow(["Vehicle^44042"], clickType: ClickType.Click);            bulkGateOutForm.SetWeightValues(@"20", @"MT");
            bulkGateOutForm.chkReceiptRequired.DoClick(false);
            bulkGateOutForm.btnSave.DoClick();
            bulkGateOutForm.CloseForm();

            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate In", forceReset: true);
            bulkGateInForm = new BulkGateInForm(vehicleId: @"44042");
            //MTNControlBase.SetValue(bulkGateInForm.cmbGate, @"North");
            bulkGateInForm.cmbGate.SetValue(@"North_Gate");
            //MTNControlBase.SetValue(bulkGateInForm.txtRegistration, @"44042");
            bulkGateInForm.txtRegistration.SetValue(@"44042");
            //MTNControlBase.SetValue(bulkGateInForm.cmbCarrier, @"American Auto Tpt");
            bulkGateInForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            //MTNControlBase.SetValue(bulkGateInForm.txtNewItem, strCargoID);
            bulkGateInForm.txtNewItem.SetValue(strCargoID, 10);
            //MTNControlBase.SetWeightValues(bulkGateInForm.tblScaleWeight,@"40", @"MT");
            bulkGateInForm.tblScaleWeight.SetValueAndType(@"40", @"MT");
            bulkGateInForm.btnSave.DoClick();

            //warningErrorForm = new WarningErrorForm(@"Warnings for Bulk Gate In TT1");
            //warningErrorForm.btnSave.DoClick();
            bulkGateInForm.CloseForm();

            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Bulk Gate Out", forceReset: true);
            bulkGateOutForm = new BulkGateOutForm();
            // MTNControlBase.FindClickRowInTable(bulkGateOutForm.tblGateInList, @"Vehicle^44042", clickType: ClickType.Click, rowHeight: 16);
            bulkGateOutForm.TblGateInList.FindClickRow(["Vehicle^44042"], clickType: ClickType.Click);            bulkGateOutForm.SetWeightValues(@"20", @"MT");
            bulkGateOutForm.chkReceiptRequired.DoClick(false);
            bulkGateOutForm.btnSave.DoClick();
            bulkGateOutForm.CloseForm();
            
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.COAL44042, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", strCargoID);
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + strCargoID);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^" + strCargoID]);            cargoEnquiryForm.GetInformationTable();
            cargoEnquiryForm.CargoEnquiryGeneralTab();
            string strCargoWeight = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Cargo Weight");  
            string strTotalWeight = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Total Weight"); 

           // Assert.IsTrue(strCargoWeight.Contains(@"120.000 MT"),@"Cargo Weight is not as expected: Expected 120.000 MT, Actual " + strCargoWeight);
            Assert.IsTrue(strTotalWeight.Contains(@"120.000 MT"), @"Cargo Weight is not as expected: Expected 120.000 MT, Actual " + strCargoWeight);

        }





    }

}
