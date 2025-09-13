using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Yard_Functions;
using MTNUtilityClasses.Classes;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using CargoSearchForm = MTNForms.FormObjects.Gate_Functions.Road_Gate.CargoSearchForm;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase52206 : MTNBase
    {
        BlendingCargoForm _blendingCargoForm;
        CargoSearchForm _cargoSearchForm;
        private const string TestCaseNumber = @"52206";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
            BaseClassInitialize_New(testContext);
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

           // MTNSignon(TestContext);
           LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void BulkBlendingAddtoExistingCargoItemUseExistingRecipe()
        {
            MTNInitialize();
            
            // Step 1 Open Yard Functions | Blending Cargo
            FormObjectBase.NavigationMenuSelection(@"Yard Functions | Blending Cargo");

            // Step 2 Enter Add to Existing Cargo Item - ticked
            _blendingCargoForm = new BlendingCargoForm(@"Blend Cargo TT1");
            _blendingCargoForm.rdoAddToExistingCargo.Click();

            // Step 3 Click the Find button next to Add to Existing Cargo Item
            _blendingCargoForm.btnFind.DoClick();

            // Step 4 In the Cargo Search form, enter Cargo ID - JLG and click the Search All button
            _cargoSearchForm = new CargoSearchForm(@"Cargo Search TT1");
            //cargoSearchForm.GetCargoID(@"8053");
            //MTNControlBase.SetValue(cargoSearchForm.txtCargoId, @"JLG52206A01");
            _cargoSearchForm.txtCargoId.SetValue(@"JLG52206A01");
            _cargoSearchForm.btnSearchAll.DoClick();
            // MTNControlBase.FindClickRowInTable(_cargoSearchForm.tblResults, @"ID^JLG52206A01");
            _cargoSearchForm.TblResults.FindClickRow(["ID^JLG52206A01"]);

            _cargoSearchForm.btnAdd.DoClick();

            // Step 6 Click OK button
            _cargoSearchForm.btnOK.DoClick();

            // Step 7 Select Use Existing Recipe and select GEN MIX
            _blendingCargoForm.UseExistingRecipe();
            //MTNControlBase.SendTextToCombobox(blendingCargoForm.cmbSelectRecipe, @"GEN MIX");
            _blendingCargoForm.cmbSelectRecipe.SetValue(@"GEN MIX - General Purpose Mix");
            // MTNControlBase.FindClickRowInTable(_blendingCargoForm.tblComponents, @"Cargo / Sub Cargo Type^SAND / WHITE SAND", ClickType.ContextClick);
            _blendingCargoForm.TblComponents.FindClickRow(["Cargo / Sub Cargo Type^SAND / WHITE SAND"], ClickType.ContextClick);
            _blendingCargoForm.ContextMenuSelect(@"Find Component Cargo...");

            // Step 9 On the Cargo Search form, click Search All button
            _cargoSearchForm = new CargoSearchForm(@"Cargo Search TT1");
            _cargoSearchForm.btnSearchAll.DoClick();
            // MTNControlBase.FindClickRowInTable(_cargoSearchForm.tblResults, @"ID^JLG52206A02");
            _cargoSearchForm.TblResults.FindClickRow(["ID^JLG52206A02"]);

            // Step 11 Click Add button and click OK button
            _cargoSearchForm.btnAdd.DoClick();
            _cargoSearchForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_blendingCargoForm.tblComponents, @"Cargo / Sub Cargo Type^CEMENT / CEMENT20", ClickType.ContextClick);
            _blendingCargoForm.TblComponents.FindClickRow(["Cargo / Sub Cargo Type^CEMENT / CEMENT20"], ClickType.ContextClick);
            _blendingCargoForm.ContextMenuSelect(@"Find Component Cargo...");

            _cargoSearchForm = new CargoSearchForm(@"Cargo Search TT1");
            _cargoSearchForm.btnSearchAll.DoClick();
            // MTNControlBase.FindClickRowInTable(_cargoSearchForm.tblResults, @"ID^JLG52206A04");
            _cargoSearchForm.TblResults.FindClickRow(["ID^JLG52206A04"]);

            _cargoSearchForm.btnAdd.DoClick();
            _cargoSearchForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_blendingCargoForm.tblComponents, @"Cargo / Sub Cargo Type^AGGREGATE / 10MM", ClickType.ContextClick);
            _blendingCargoForm.TblComponents.FindClickRow(["Cargo / Sub Cargo Type^AGGREGATE / 10MM"], ClickType.ContextClick);
            _blendingCargoForm.ContextMenuSelect(@"Find Component Cargo...");

            _cargoSearchForm = new CargoSearchForm(@"Cargo Search TT1");
            _cargoSearchForm.txtCargoId.SetValue("JLG52206A03");
            _cargoSearchForm.btnSearchAll.DoClick();
            // MTNControlBase.FindClickRowInTable(_cargoSearchForm.tblResults, @"ID^JLG52206A03");
            _cargoSearchForm.TblResults.FindClickRow(["ID^JLG52206A03"]);

            _cargoSearchForm.btnAdd.DoClick();
            _cargoSearchForm.btnOK.DoClick();

            // Step 13 Enter Multiply Recipe By = 2.00 
            //MTNControlBase.SendTextToTextbox(blendingCargoForm.txtRecipeMultiplyBy, @"2.00");
            _blendingCargoForm.txtRecipeMultiplyBy.SetValue(@"2.00");
            // Step 14 Click Blend button
            _blendingCargoForm.btnBlend.DoClick();

            // Step 15 Open General Functions | Cargo Enquiry
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();

            // Step 16 Search Cargo Type -Aggregate, Cargo Id -JLG52206A03 and check Total Weight
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.Aggregate, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG52206A03");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + @"JLG52206A03");
            cargoEnquiryForm.tblData2.FindClickRow(["ID^" + @"JLG52206A03"]);
            cargoEnquiryForm.GetInformationTable();
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            // check weight remaining is as expected
            string strTotalWeight = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Total Weight");
            Assert.IsTrue(strTotalWeight.Contains(@"21.320 MT"), @"Cargo Weight is not as expected: Expected 21.683 MT, Actual " + strTotalWeight);

            // Step 17 Search Cargo Type -Cement , Cargo Id -JLG52206A04 and check Total Quantity
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.CementBagged, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG52206A04");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + @"JLG52206A04");
            cargoEnquiryForm.tblData2.FindClickRow(["ID^" + @"JLG52206A04"]);
            cargoEnquiryForm.GetInformationTable();
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            // check Quantity remaining is as expected
            string strTotalQuantity = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Total Quantity");
            Assert.AreEqual(@"42", strTotalQuantity, @"Cargo Quantity is not as expected: Expected 42, Actual " + strTotalQuantity);

            // Step 18 Search Cargo Type -Sand , Cargo Id -JLG52206A02 and check Total Volume
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.Sand, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG52206A02");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + @"JLG52206A02");
            cargoEnquiryForm.tblData2.FindClickRow(["ID^" + @"JLG52206A02"]);
            cargoEnquiryForm.GetInformationTable();
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            // check volume remaining is as expected
            string strTotalVolume = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Total Volume");
            Assert.IsTrue(strTotalVolume.Contains(@"1.8352 m3"), @"Cargo Volume is not as expected: Expected 1.8352 m3, Actual " + strTotalVolume);

            // Step 19 Search Cargo Type -Builders Mix , Cargo Id -JLG52206A01 and check Total Weight
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.BuildersMix, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG52206A01");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + @"JLG52206A01");
            cargoEnquiryForm.tblData2.FindClickRow(["ID^" + @"JLG52206A01"]);
            cargoEnquiryForm.GetInformationTable();
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            strTotalWeight = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Total Weight");
            Assert.IsTrue(strTotalWeight.Contains(@"7000 kg"), @"Total Weight is not as expected: Expected 7000 kg, Actual " + strTotalWeight);

        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Builders Mix</cargoTypeDescr>\n         <id>JLG52206A01</id>\n         <operatorCode>MSC</operatorCode>\n         <weight>5000</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n	  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Sand</cargoTypeDescr>\n         <id>JLG52206A02</id>\n         <operatorCode>MSC</operatorCode>\n         <volume>5</volume>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n	  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Aggregate</cargoTypeDescr>\n         <id>JLG52206A03</id>\n         <operatorCode>MSC</operatorCode>\n         <weight>22046</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n	  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Cement Bagged</cargoTypeDescr>\n         <id>JLG52206A04</id>\n         <operatorCode>MSC</operatorCode>\n         <quantity>50</quantity>\n		 <weight>1600</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n	      \n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n");


            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Builders Mix</cargoTypeDescr>\n         <id>JLG52206A01</id>\n         <operatorCode>MSC</operatorCode>\n         <weight>5000</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n	  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Sand</cargoTypeDescr>\n		 <product>WHITE SAND</product>\n         <id>JLG52206A02</id>\n         <operatorCode>MSC</operatorCode>\n         <volume>5</volume>\n		 <weight>5000</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n	  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Aggregate</cargoTypeDescr>\n		 <product>10MM</product>\n         <id>JLG52206A03</id>\n         <operatorCode>MSC</operatorCode>\n         <weight>22046</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n	  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Cement Bagged</cargoTypeDescr>\n		 <product>CEMENT20</product>\n         <id>JLG52206A04</id>\n         <operatorCode>MSC</operatorCode>\n         <totalQuantity>50</totalQuantity>\n		 <weight>1600</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }
}
