using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.System_Items.Background_Processing;
using MTNUtilityClasses.Classes;
using System;
using DataObjects;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions.CAMS
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase46712 : MTNBase
    {
        CargoEnquiryTransactionForm _cargoEnquiryTransactionForm;

        BackgroundApplicationForm _backgroundApplicationForm;
      
        DateTime _startTS;
     
        const string TestCaseNumber = @"46712";
        const string CargoId = @"JLG" + TestCaseNumber + @"A01";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
            BaseClassInitialize_New(testContext);
            //userName = "MTNOLDDAM";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            // Stop the CAMS(Server)
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();
            //_backgroundApplicationForm.SetFocusToForm();
            _backgroundApplicationForm?.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^",
                false);

            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            //MTNSignon(TestContext, userName);
            TestRunDO.GetInstance().SetUserName("MTNOLDDAM");
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void CheckEDIInfoSentForDamageSegmentRunningStandardCODECO()
        {
            MTNInitialize();
            
            _startTS = DateTime.Now;

            // Step 6
            FormObjectBase.NavigationMenuSelection(@"General Functions | Cargo Enquiry", forceReset: true);
            cargoEnquiryForm = new CargoEnquiryForm(@"Cargo Enquiry TT1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", @"ISO Container", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", CargoId);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Site (Current)", "On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();

            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId, rowHeight: 18);

            cargoEnquiryForm.GetGenericTabTableDetails(@"Status", @"4087");

            //cargoEnquiryForm.DoEdit();

            MTNControlBase.FindClickRowInTableVHeader(cargoEnquiryForm.tblGeneric, @"Damage Detail (dbl click)",
                clickType: ClickType.DoubleClick, tableRowHeight: 19);

            DamageForm damageForm = new DamageForm(@"Damage " + CargoId + " TT1");
            damageForm.cmbPosition.SetValue(@"Bottom - Bottom");
            damageForm.cmbDamage.SetValue(@"SWP - Severe Weather Pitting");
            damageForm.cmbSeverity.SetValue(@"2	Two", doDownArrow: true);
            damageForm.txtQuantity.SetValue(@"2");
            damageForm.btnAdd.DoClick();

            MTNControlBase.FindClickRowInTable(damageForm.tblDamageDetails,
                @"Position^Bottom~Damaged^SWP~Severity^2~Quantity^2");
           
            damageForm.btnOk.DoClick();
            
            WarningErrorForm.CompleteWarningErrorForm("Warnings for Cargo TT1");

            /*
            cargoEnquiryForm.SetFocusToForm();
            //cargoEnquiryForm.btnSave.DoClick();
            cargoEnquiryForm.DoSave();
            */

            //warningErrorForm = new WarningErrorForm(formTitle: @"Warnings for Tracked Item Update TT1");
            //warningErrorForm.btnSave.DoClick();

            // Step 13 - 19
            FormObjectBase.NavigationMenuSelection(@"Background Process|Background Processing", forceReset: true);
            _backgroundApplicationForm = new BackgroundApplicationForm();
            _backgroundApplicationForm.StartStopServer(@"CAMS(Server)", @"Application Type^CAMS~Status^");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(1000));

            FormObjectBase.NavigationMenuSelection(@"EDI Functions | EDI CAMS Protocols", forceReset: true);
            EDICAMProtocolForm ediCAMProtocolForm = new EDICAMProtocolForm();
            MTNControlBase.FindClickRowInTable(ediCAMProtocolForm.tblProtocols,
                 @"Protocol Description^STANDARD CODECO SMDG162~Name^Test46712", xOffset: 40);

            var endDateTime = DateTime.Now.AddMinutes(1);

            ediCAMProtocolForm.GetDetailsTabAndDetails();
            ediCAMProtocolForm.RunAdhocReport(_startTS.ToString(@"ddMMyyyy"),
                _startTS.ToString(@"HHmm"), endDateTime.ToString(@"ddMMyyyy"),
                endDateTime.ToString(@"HHmm"), getTab: true);

            // Step 20 - 25
            cargoEnquiryForm.SetFocusToForm();
           
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + CargoId, rowHeight: 18);
            //cargoEnquiryForm.btnViewTransaction.DoClick();
            cargoEnquiryForm.DoViewTransactions();
            _cargoEnquiryTransactionForm = new CargoEnquiryTransactionForm(@"Transactions for " + CargoId + " TT1");

             MTNControlBase.FindClickRowInTable(_cargoEnquiryTransactionForm.tblTransactions, @"Type^EDI Info Sent",
                 ClickType.DoubleClick);

             // Check logging details
             LoggingDetailsForm loggingDetailsForm = new LoggingDetailsForm(@"Logging Details");

             string[] linesToCheck =
             {
                 @"DAM+1+SWP:::Bottom has Severe Weather Pitting(2'"
             };

             loggingDetailsForm.FindStringsInTable(linesToCheck);
             loggingDetailsForm.DoCancel();

             var startPos = linesToCheck[0].IndexOf(":::", StringComparison.Ordinal);
             var tempString = linesToCheck[0].Substring(startPos + 2, linesToCheck[0].Length - 1 - (startPos + 3));
             
             Assert.IsTrue(tempString.Length.Equals(35), 
                 @"TestCase46712 - Damage string [" + tempString + "] length should be 35 but is " + tempString.Length);
        }



        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG46712A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>MKBS01</locationId>\n            <weight>5000</weight>\n            <imexStatus>Import</imexStatus>\n            <commodity>MT</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <isoType>2200</isoType>\n			<messageMode>D</messageMode>\n        </CargoOnSite>\n		\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");

            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n            <id>JLG46712A01</id>\n            <operatorCode>MSL</operatorCode>\n            <locationId>MKBS01</locationId>\n            <weight>5000</weight>\n            <imexStatus>Import</imexStatus>\n            <commodity>MT</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>MSCK000002</voyageCode>\n            <isoType>2200</isoType>\n			<messageMode>A</messageMode>\n        </CargoOnSite>\n		\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads



    }

}
