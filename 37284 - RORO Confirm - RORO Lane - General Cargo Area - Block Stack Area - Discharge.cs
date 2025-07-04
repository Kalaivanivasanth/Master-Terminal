using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.Web;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.ROROConfirm;
using System;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using MTNGlobal.EnumsStructs;
using MTNWebPages.PageObjects.Popups;
using MTNWebSelenium.Classes;

namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps
{
    [TestClass, TestCategory(TestCategories.Web)]
    public class TestCase37284 : MobileAppsBase
    {
        
        MA_CargoAppsSearchPage searchPage;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            searchFor = @"_37284_";
            CallJadeScriptToRun(TestContext, @"resetData_37284");
            SetupAndLoadInitializeData(TestContext);

            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);
        }

        [TestMethod]
        public void ROROConfirmDischargeROROLaneGCAreaBlockStack()
        {
            MTNInitialize();
            
            // Step 5
            /*HomePage.ClickTile(HomePage.BtnCargoApps);

            // Step 6 - 7
            string[,] fieldValueToSet =
            {
                { MA_CargoAppsSearchPage.constAdvCargoID, @"JLG37284W9U" },
                { MA_CargoAppsSearchPage.constAdvSiteState, @"On ship" }
            };
            
            string[] cargoToCheck =
            {
                @"JLG37284W9UA01",
                @"JLG37284W9UA02",
                @"JLG37284W9UA03",
                @"JLG37284W9UA04",
                @"JLG37284W9UA06",
                @"JLG37284W9UA08",
                @"JLG37284W9UA09",
                @"JLG37284W9UA10"
            };
            DoAdvancedCargoSearch(fieldValueToSet, cargoToCheck);
           
            // Step 8 - 9
            fieldValueToSet = new string[,]
            {
                { MA_CargoAppsSearchPage.constAdvCargoID, @"JLG37284W9U" },
                { MA_CargoAppsSearchPage.constAdvIMEXStatus, @"Import" },
                { MA_CargoAppsSearchPage.constAdvSiteState, @"On Site" }
            };

            cargoToCheck = new string[]
            {
                @"JLG37284W9UB01",
                @"JLG37284W9UB02",
                @"JLG37284W9UB04"
            };
            //DoAdvCargoSearch(fieldValueToSet, cargoToCheck);
            DoAdvancedCargoSearch(fieldValueToSet, cargoToCheck);

            // Step 10 - 11
            fieldValueToSet = new string[,]
            {
                { MA_CargoAppsSearchPage.constAdvCargoID, @"JLG37284W9U" },
                { MA_CargoAppsSearchPage.constAdvIMEXStatus, @"Tranship" },
                { MA_CargoAppsSearchPage.constAdvSiteState, @"On Site" }
            };

            cargoToCheck = null;
            //DoAdvCargoSearch(fieldValueToSet, cargoToCheck);
            DoAdvancedCargoSearch(fieldValueToSet, cargoToCheck);
            
            cargoToCheck = new []
            {
                @"JLG37284W9UB03",
            };
            DoValidation(cargoToCheck);
            

            // Step 12
            searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.ReturnToTilesPage();*/
            HomePage = new MA_TilesPage(TestContext);
            
            HomePage.ClickTile(HomePage.BtnROROConfirm);

            // Step 13
            MA_ROROConfirm roroConfirm = new MA_ROROConfirm(TestContext);

            string[] voyageDetailsToCheck =
            {
                @"On Site^4",
                @"Imports^3", 
                @"Tranship^1",
                @"On Ship^8"
            };

            roroConfirm.ClickDetailsForVoyage(@"M37284A0001", voyageDetailsToCheck);

            // Step 14
            MA_ROROConfirmSelectArea roroConfirmSelectArea = new MA_ROROConfirmSelectArea(TestContext);
            roroConfirmSelectArea.DoROROLane();

            // Step 15 - 16
            MA_ROROConfirmAreaDetails areaDetails = new MA_ROROConfirmAreaDetails(TestContext);

            var fieldValueToSet = new string[,]
            {
                { MA_ROROConfirmAreaDetails.constLaneArea, @"RLA001" },
                { MA_ROROConfirmAreaDetails.constLane, @"002" },
                { MA_ROROConfirmAreaDetails.constID, @"JLG37284W9UA01" }
            };
            areaDetails.SetFields(fieldValueToSet);

            string[] fieldValueToCheck = 
            {
                @"JLG37284W9UA01^1",
                @"^2",
                @"^3"
            };
            areaDetails.ValidateInCorrectLaneLocation(fieldValueToCheck);
            areaDetails.CheckFieldHighlighted(@"JLG37284W9UA01^1");

            // Step 17
            areaDetails.DoConfirm();
            areaDetails.ValidateInCorrectLaneLocation(fieldValueToCheck);
            areaDetails.ValidateCounts(@"5^7");
            
            // Step 18
            fieldValueToSet = new string[,]
            {
                { MA_ROROConfirmAreaDetails.constID, @"JLG37284W9UA02" }
            };
            areaDetails.SetFields(fieldValueToSet);

            fieldValueToCheck = new string[]
            {
                @"JLG37284W9UA01^1",
                @"JLG37284W9UA02^2",
                @"^3"
            };
            areaDetails.ValidateInCorrectLaneLocation(fieldValueToCheck);
            areaDetails.CheckFieldHighlighted(@"JLG37284W9UA02^2");

            // Step 19
            fieldValueToSet = new string[,]
            {
                { MA_ROROConfirmAreaDetails.constID, @"JLG37284W9UA03" }
            };
            areaDetails.SetFields(fieldValueToSet);

            fieldValueToCheck = new string[]
            {
                @"JLG37284W9UA01^1",
                @"JLG37284W9UA03^2",
                @"^3"
            };
            areaDetails.ValidateInCorrectLaneLocation(fieldValueToCheck);
            areaDetails.CheckFieldHighlighted(@"JLG37284W9UA03^2");

            // Step 20
            areaDetails.DoConfirm();
            areaDetails.ValidateInCorrectLaneLocation(fieldValueToCheck);
            areaDetails.ValidateCounts(@"6^6");

            // Step 21
            areaDetails.DoBack();

            // Step 22
            roroConfirmSelectArea = new MA_ROROConfirmSelectArea(TestContext);
            roroConfirmSelectArea.DoROROLane();

            // Step 23
            fieldValueToSet = new string[,]
            {
                { MA_ROROConfirmAreaDetails.constLaneArea, @"RLA001" },
                { MA_ROROConfirmAreaDetails.constLane, @"002" }
            };
            areaDetails.SetFields(fieldValueToSet);

            fieldValueToCheck = new string[]
            {
                @"JLG37284W9UA01^1",
                @"JLG37284W9UA03^2",
                @"^3"
            };
            areaDetails.ValidateInCorrectLaneLocation(fieldValueToCheck);
            areaDetails.ValidateCounts(@"6^6");
           
            // Step 24
            areaDetails.DoBack();

            // Step 25
            roroConfirmSelectArea = new MA_ROROConfirmSelectArea(TestContext);
            roroConfirmSelectArea.DoGeneralCargo();

            // Step 26 - 27
            areaDetails = new MA_ROROConfirmAreaDetails(TestContext);
            fieldValueToSet = new string[,]
            {
                { MA_ROROConfirmAreaDetails.constGeneralCargoArea, @"GCCAR1" },
                { MA_ROROConfirmAreaDetails.constLocation, @"04" },
                { MA_ROROConfirmAreaDetails.constID, @"JLG37284W9UA06" }
            };
            areaDetails.SetFields(fieldValueToSet);

            // Step 28
            areaDetails.DoConfirm();
            areaDetails.ValidateCounts(@"7^5");

            // Step 29
            areaDetails.DoBack();

            // Step 30
            roroConfirmSelectArea = new MA_ROROConfirmSelectArea(TestContext);
            roroConfirmSelectArea.DoGeneralCargo();

            // Step 31
            areaDetails = new MA_ROROConfirmAreaDetails(TestContext);
            fieldValueToSet = new string[,]
            {
                { MA_ROROConfirmAreaDetails.constGeneralCargoArea, @"GCCAR1" },
                { MA_ROROConfirmAreaDetails.constLocation, @"04" }
            };
            areaDetails.SetFields(fieldValueToSet);
            areaDetails.ValidateCounts(@"7^5");
            areaDetails.ValidateGCScannedCounts(@"1^1");

            // Step 32
            areaDetails.DoBack();

            // Step 33
            roroConfirmSelectArea = new MA_ROROConfirmSelectArea(TestContext);
            roroConfirmSelectArea.DoBlockStack();

            // Step 34 - 35
            areaDetails = new MA_ROROConfirmAreaDetails(TestContext);
            fieldValueToSet = new string[,]
            {
                { MA_ROROConfirmAreaDetails.constGeneralCargoArea, @"BSCAR1" },
                { MA_ROROConfirmAreaDetails.constID, @"JLG37284W9UA10" }
            };
            areaDetails.SetFields(fieldValueToSet);
            
            // Step 36
            areaDetails.DoConfirm();

            WarningErrorPopup warningErrorPopupVMT = new WarningErrorPopup(TestContext);
            warningErrorPopupVMT.DoOK();

            areaDetails.DoBack();

            // Step 33
            roroConfirmSelectArea = new MA_ROROConfirmSelectArea(TestContext);
            roroConfirmSelectArea.DoBlockStack();

            // Step 34 - 35
            areaDetails = new MA_ROROConfirmAreaDetails(TestContext);
            fieldValueToSet = new string[,]
            {
                { MA_ROROConfirmAreaDetails.constGeneralCargoArea, @"BSCAR1" },
                
            };

            areaDetails.SetFields(fieldValueToSet);
            areaDetails.ValidateCounts(@"8^4");
            areaDetails.ValidateBSScannedCount(@"1");

            // Step 37
            fieldValueToSet = new string[,]
            {
                { MA_ROROConfirmAreaDetails.constID, @"JLG37284W9UA09" }
            };
            areaDetails.SetFields(fieldValueToSet);
            
            // Step 38
            areaDetails.DoReset();
            Miscellaneous.CaptureElementAsImage(TestContext, MTNWeb.webDriver, @"37284_ROROLaneSuccessMessage_Step38.png");

            // Step 39
            areaDetails.DoBack();

            // Step 40
            roroConfirmSelectArea = new MA_ROROConfirmSelectArea(TestContext);
            roroConfirmSelectArea.DoROROLane();

            // Step 41 - 42
            areaDetails = new MA_ROROConfirmAreaDetails(TestContext);

            fieldValueToSet = new string[,]
            {
                { MA_ROROConfirmAreaDetails.constLaneArea, @"RLA001" },
                { MA_ROROConfirmAreaDetails.constLane, @"001" },
                { MA_ROROConfirmAreaDetails.constID, @"JLG37284W9UA04" }
            };
            areaDetails.SetFields(fieldValueToSet);

            fieldValueToCheck = new string[]
            {
                @"JLG37284W9UB02^2",
                @"JLG37284W9UB04^3",
                @"JLG37284W9UA04^4"
            };
            areaDetails.ValidateInCorrectLaneLocation(fieldValueToCheck);
            areaDetails.CheckFieldHighlighted(@"JLG37284W9UA04^4");

            // Step 43
            areaDetails.DoConfirm();
            areaDetails.ValidateInCorrectLaneLocation(fieldValueToCheck);
            areaDetails.ValidateCounts(@"9^3");

            // Step 44
            areaDetails.DoBack();

            // Step 45
            roroConfirmSelectArea = new MA_ROROConfirmSelectArea(TestContext);
            roroConfirmSelectArea.DoBack();

        }


        private void DoAdvancedCargoSearch(string[,] fieldValueToSet, string[] cargoToCheck)
        {
            searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.DoAdvanced();

            DoAdvCargoSearch(fieldValueToSet, cargoToCheck);
        }

        void DoAdvCargoSearch(string[,] fieldValueToSet, string[] cargoToCheck)
        {
            searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.DoAdvReset();
            
            searchPage = new MA_CargoAppsSearchPage(TestContext);
            searchPage.DoAdvancedCargoSearch(fieldValueToSet);

            if (cargoToCheck != null && cargoToCheck.Length > 0)
            {
                Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(3));
                searchPage = new MA_CargoAppsSearchPage(TestContext);
                searchPage.ValidateCargoReturned(cargoToCheck, exactCount: true);
            }
        }

        void DoValidation(string[] cargoToCheck)
        {
            searchPage = new MA_CargoAppsSearchPage(TestContext);
            MTNWeb.webDriver.WaitForPageLoad();
            bool doExactCount = MTNWeb.isFirefox;
            searchPage.ValidateCargoReturned(cargoToCheck, doExactCount, idSearchDetails: "{cargoId}");
            
            
        }


        #region - Setup and Run Data Loads

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Move On Site Cargo
            //CreateDataFileToLoad(@"MoveOnSiteCargo.xml",
            //    "<?xml version='1.0'?> \n    <JMTInternalMove xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalMove.xsd'>\n	<AllMove>\n	    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <Move Terminal='TT1'>\n			<moveType>REAL</moveType>\n			<quantity/>\n			<cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n			<description/>\n			<fullOrMT/>\n			<id>JLG37284W9UA01</id>\n			<messageMode>A</messageMode>\n			<sourceLocn>RLA001 002 1</sourceLocn>\n			<targetLocn>MKBS01</targetLocn>\n		</Move>\n		<Move Terminal='TT1'>\n			<moveType>REAL</moveType>\n			<quantity/>\n			<cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n			<description/>\n			<fullOrMT/>\n			<id>JLG37284W9UA03</id>\n			<messageMode>A</messageMode>\n			<sourceLocn>RLA001 002 2</sourceLocn>\n			<targetLocn>MKBS01</targetLocn>\n		</Move>\n		<Move Terminal='TT1'>\n			<moveType>REAL</moveType>\n			<quantity/>\n			<cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n			<description/>\n			<fullOrMT/>\n			<id>JLG37284W9UA04</id>\n			<messageMode>A</messageMode>\n			<sourceLocn>RLA001 001 4</sourceLocn>\n			<targetLocn>MKBS01</targetLocn>\n		</Move>\n	</AllMove>\n</JMTInternalMove>");

            // Delete Cargo On Ship
            CreateDataFileToLoad(@"DeleteCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA02</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>D3001G</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnShip>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA09</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>D3001G</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnShip>\n    </AllCargoOnShip>\n</JMTInternalCargoOnShip>");

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA01</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>RLA001 002 1</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA03</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>RLA001 002 2</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n          <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA04</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>RLA001 001 4</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA06</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>GCCAR1 04</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA10</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>BSCAR1</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>D</messageMode>\n        </CargoOnSite>\n    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // Create Cargo On Ship
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA01</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>D3001G</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA02</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>D3001G</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA03</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>D3001G</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n          <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA04</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>D3001G</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA06</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>D3001G</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA09</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>D3001G</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n        <CargoOnShip Terminal='TT1'>\n            <cargoTypeDescr>Motor Vehicle</cargoTypeDescr>\n            <id>JLG37284W9UA10</id>\n            <commodity>MCAR</commodity>\n            <voyageCode>M37284A0001</voyageCode>\n            <operatorCode>MSL</operatorCode>\n            <dischargePort>USJAX</dischargePort>\n            <locationId>D3001G</locationId>\n            <weight>5101.0000</weight>\n            <imexStatus>Import</imexStatus>\n            <totalQuantity>1</totalQuantity>\n            <messageMode>A</messageMode>\n        </CargoOnShip>\n    </AllCargoOnShip>\n</JMTInternalCargoOnShip>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

        #endregion - Setup and Run Data Loads


    }
}
