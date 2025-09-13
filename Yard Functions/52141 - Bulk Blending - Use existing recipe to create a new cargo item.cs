using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNForms.FormObjects.Yard_Functions;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase52141 : MTNBase
    {
        BlendingCargoForm _blendingCargoForm;
        CargoSearchForm _cargoSearchForm;
        private const string TestCaseNumber = @"52141";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_" + TestCaseNumber + "_";
            BaseClassInitialize_New(testContext);
        }
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            //MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void BulkBlendingUseExistingRecipeToCreateNewCargoItem()
        {
            MTNInitialize();
            
            // Step 1 Open Yard Functions | Blending Cargo
            FormObjectBase.NavigationMenuSelection(@"Yard Functions | Blending Cargo");

            // Step 2 Enter Create New Cargo Item(s) - ticked, Cargo Id(s) -JLG52141A01, Cargo Type -Builders Mix, Operator - MSC, Terminal Area -MHBS01
            _blendingCargoForm = new BlendingCargoForm(@"Blend Cargo TT1");
            _blendingCargoForm.rdoCreateNewCargo.Click();
            //MTNControlBase.SendTextToTextbox(blendingCargoForm.txtCargoId, @"JLG52141A01");
            _blendingCargoForm.txtCargoId.SetValue(@"JLG52141A01");
            //MTNControlBase.SendTextToCombobox(blendingCargoForm.cmbCargoType, @"Builders Mix");
            _blendingCargoForm.cmbCargoType.SetValue(CargoType.BuildersMix);
            //MTNControlBase.SendTextToCombobox(blendingCargoForm.cmbOperator, @"MSC");
            _blendingCargoForm.cmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            //MTNControlBase.SendTextToCombobox(blendingCargoForm.cmbTerminalArea, @"MHBS01");
            _blendingCargoForm.cmbTerminalArea.SetValue(@"MHBS01");
            // Step 3 Select Use Existing Recipe and select GEN MIX
            _blendingCargoForm.UseExistingRecipe();
            //MTNControlBase.SendTextToCombobox(blendingCargoForm.cmbSelectRecipe, @"GEN MIX");
            _blendingCargoForm.cmbSelectRecipe.SetValue(@"GEN MIX - General Purpose Mix");
            // MTNControlBase.FindClickRowInTable(_blendingCargoForm.tblComponents, @"Cargo / Sub Cargo Type^SAND / WHITE SAND", ClickType.ContextClick);
            _blendingCargoForm.TblComponents.FindClickRow(["Cargo / Sub Cargo Type^SAND / WHITE SAND"], ClickType.ContextClick);
            _blendingCargoForm.ContextMenuSelect(@"Find Component Cargo...");

            // Step 5 On the Cargo Search form, click Search All button
            _cargoSearchForm = new CargoSearchForm(@"Cargo Search TT1");
            _cargoSearchForm.btnSearchAll.DoClick();
            // MTNControlBase.FindClickRowInTable(_cargoSearchForm.tblResults, @"ID^JLG52141A02");
            _cargoSearchForm.TblResults.FindClickRow(["ID^JLG52141A02"]);

            // Step 7 Click Add button and click OK button
            _cargoSearchForm.btnAdd.DoClick();
            _cargoSearchForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_blendingCargoForm.tblComponents, @"Cargo / Sub Cargo Type^CEMENT / CEMENT20", ClickType.ContextClick);
            _blendingCargoForm.TblComponents.FindClickRow(["Cargo / Sub Cargo Type^CEMENT / CEMENT20"], ClickType.ContextClick);
            _blendingCargoForm.ContextMenuSelect(@"Find Component Cargo...");

            _cargoSearchForm = new CargoSearchForm(@"Cargo Search TT1");
            _cargoSearchForm.btnSearchAll.DoClick();
            // MTNControlBase.FindClickRowInTable(_cargoSearchForm.tblResults, @"ID^JLG52141A04");
            _cargoSearchForm.TblResults.FindClickRow(["ID^JLG52141A04"]);

            _cargoSearchForm.btnAdd.DoClick();
            _cargoSearchForm.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_blendingCargoForm.tblComponents, @"Cargo / Sub Cargo Type^AGGREGATE / 10MM", ClickType.ContextClick);
            _blendingCargoForm.TblComponents.FindClickRow(["Cargo / Sub Cargo Type^AGGREGATE / 10MM"], ClickType.ContextClick);
            _blendingCargoForm.ContextMenuSelect(@"Find Component Cargo...");

            _cargoSearchForm = new CargoSearchForm(@"Cargo Search TT1");
            _cargoSearchForm.txtCargoId.SetValue("JLG52141A03");
            _cargoSearchForm.btnSearchAll.DoClick();
            // MTNControlBase.FindClickRowInTable(_cargoSearchForm.tblResults, @"ID^JLG52141A03");
            _cargoSearchForm.TblResults.FindClickRow(["ID^JLG52141A03"]);

            _cargoSearchForm.btnAdd.DoClick();
            _cargoSearchForm.btnOK.DoClick();

            // Step 9 Click Blend button
            _blendingCargoForm.btnBlend.DoClick();

            // Step 10 Search Cargo Type -Aggregate, Cargo Id -JLG52141A03 and check Total Weight
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.Aggregate, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG52141A03");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + @"JLG52141A03");
            cargoEnquiryForm.tblData2.FindClickRow(["ID^" + @"JLG52141A03"]);
            cargoEnquiryForm.GetInformationTable();
            cargoEnquiryForm.CargoEnquiryGeneralTab();


            string strTotalWeight = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Total Weight");
            Assert.IsTrue(strTotalWeight.Contains(@"21.683 MT"), @"Cargo Weight is not as expected: Expected 21.683 MT, Actual " + strTotalWeight);

            // Search Cargo Type -Cement , Cargo Id -JLG52141A04 and check Total Quantity
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.CementBagged, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG52141A04");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + @"JLG52141A04");
            cargoEnquiryForm.tblData2.FindClickRow(["ID^" + @"JLG52141A04"]);
            cargoEnquiryForm.GetInformationTable();
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            string strTotalQuantity = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Total Quantity");
            Assert.AreEqual(@"46", strTotalQuantity, @"Cargo Quantity is not as expected: Expected 46, Actual " + strTotalQuantity);

            // Search Cargo Type -Sand , Cargo Id -JLG52141A02 and check Total Volume
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo Type", CargoType.Sand, EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, @"Cargo ID", @"JLG52141A02");
            //cargoEnquiryForm.btnSearch.DoClick();
            cargoEnquiryForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^" + @"JLG52141A02");
            cargoEnquiryForm.tblData2.FindClickRow(["ID^" + @"JLG52141A02"]);
            cargoEnquiryForm.GetInformationTable();
            cargoEnquiryForm.CargoEnquiryGeneralTab();

            string strTotalVolume = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblGeneralEdit, @"Total Volume");
            Assert.IsTrue(strTotalVolume.Contains(@"5.7941 m3"), @"Cargo Volume is not as expected: Expected 5.7941 m3, Actual " + strTotalVolume);

        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo OnSite
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n      <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Builders Mix</cargoTypeDescr>\n         <id>JLG52141A01</id>\n         <operatorCode>MSC</operatorCode>\n         <weight>5000</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n	  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Sand</cargoTypeDescr>\n         <id>JLG52141A02</id>\n         <operatorCode>MSC</operatorCode>\n         <volume>50</volume>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n	  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Aggregate</cargoTypeDescr>\n         <id>JLG52141A03</id>\n         <operatorCode>MSC</operatorCode>\n         <weight>22046</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n	  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Cement Bagged</cargoTypeDescr>\n         <id>JLG52141A04</id>\n         <operatorCode>MSC</operatorCode>\n         <quantity>50</quantity>\n		 <weight>1600</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>D</messageMode>\n      </CargoOnSite>\n	      \n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n\n\n\n");


            // Create Cargo OnSite
            CreateDataFileToLoad(@"CreateCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n   <AllCargoOnSite>\n      <operationsToPerform>Verify;Load To DB</operationsToPerform>\n      <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n     <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Sand</cargoTypeDescr>\n         <product>WHITE SAND</product>\n         <id>JLG52141A02</id>\n         <operatorCode>MSC</operatorCode>\n          <weight>10000</weight>\n  <volume>5</volume>\n        <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Aggregate</cargoTypeDescr>\n         <product>10MM</product>\n         <id>JLG52141A03</id>\n         <operatorCode>MSC</operatorCode>\n         <weight>22046</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>A</messageMode>\n      </CargoOnSite>\n  <CargoOnSite Terminal='TT1'>\n         <cargoTypeDescr>Cement Bagged</cargoTypeDescr>\n         <product>CEMENT20</product>\n         <id>JLG52141A04</id>\n         <operatorCode>MSC</operatorCode>\n         <totalQuantity>50</totalQuantity>\n         <weight>1600</weight>\n         <imexStatus>Export</imexStatus>\n         <dischargePort>NZAKL</dischargePort>\n         <voyageCode>MSCK000010</voyageCode>\n         <locationId>MHBS01</locationId>\n         <messageMode>A</messageMode>\n      </CargoOnSite>   \n   </AllCargoOnSite>\n</JMTInternalCargoOnSite>\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }
}
