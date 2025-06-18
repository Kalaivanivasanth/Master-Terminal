using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using System.IO;
using System.Linq;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase39568 : MTNBase
    {

       
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"_39568_";
        }

       
        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void VGMAuthorityAndSpecialCharacters()
        {

            MTNInitialize();
            
            //1. Go to road gate and enter truck information
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");

            EDIOperationsForm ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            var type = @"Actual";
            Miscellaneous.DeleteFile(string.Concat(dataDirectory, type, "_39568_BAPLIE22Extract.txt"));
            
            //ediOperationsForm.btnExtractNew.DoClick();
            ediOperationsForm.DoExtractNew();

            EDIExtractForm ediExtractForm = new EDIExtractForm(@"EDI Extract TT1");
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Extract Type", type + @" Bay Plan",
                rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Voyage", @"MSCK000002	MSC KATYA R.",
                rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Recipient", @"MSL	Messina Line",
                rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Cargo Operator", @"MSL	Messina Line",
                rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Format", @"Baplie 2.2",
                rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails,
                @"File Name (dbl click)", dataDirectory + type + @"_39568_BAPLIE22Extract.txt");
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Shipper", @"MSL");
            //ediExtractForm.btnSave.DoClick();
            ediExtractForm.DoSave();

            string[] detailsToCheck =
            {
                @"FTX+AAY++SHP+JAMES LAST  WEIGHT CERTS:GEORGE LASTS BEST PEOPLE'",
                @"FTX+AAY++SM1+JAMES LAST  WEIGHT CERTS;;;:GEORGE LASTS BEST PEOPLE::'",
                @"EQD+CN+JLG39568A02+2200++3+4'",
                @"FTX+AAY++SHP+JAMES LASTS  OLD WEIGHT CERTS:GEORGE LASTS BEST PEOPLE  CERTS'",
                @"FTX+AAY++SM2+JAMES LASTS  OLD WEIGHT CERTS;;;:GEORGE LASTS BEST PEOPLE  CERTS::'",
                @"EQD+CN+JLG39568A03+2200++3+4'",
                @"FTX+AAY++SHP+GEORGE BEST WEIGHT CERTS:GEORGE LAST'",
                @"EQD+CN+JLG39568A01+2200++3+4'"
            };

            Wait.UntilInputIsProcessed(waitTimeout: TimeSpan.FromSeconds(5));
            string[] lines = File.ReadAllLines(dataDirectory + type + @"_39568_BAPLIE22Extract.txt").ToArray();
            
            string linesNotFound = String.Empty;
            foreach (string checkThis in detailsToCheck)
            {
                int indexOf = lines.ToList().IndexOf(checkThis);
                if (indexOf < 0)
                {
                    linesNotFound += checkThis + "\n";
                }
            }

            type = @"Proposed";
            Miscellaneous.DeleteFile(string.Concat(dataDirectory, type, "_39568_BAPLIE22Extract.txt"));
            
            ediOperationsForm.SetFocusToForm();
            //ediOperationsForm.btnExtractNew.DoClick();
            ediOperationsForm.DoExtractNew();

            ediExtractForm = new EDIExtractForm(@"EDI Extract TT1");
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Extract Type", type + @" Bay Plan",
                rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Voyage", @"MSCK000002	MSC KATYA R.",
                rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Recipient", @"MSL	Messina Line",
                rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Cargo Operator", @"MSL	Messina Line",
                rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Format", @"Baplie 2.2",
                rowDataType: EditRowDataType.ComboBox, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails,
                @"File Name (dbl click)", dataDirectory + type + @"_39568_BAPLIE22Extract.txt");
            MTNControlBase.SetValueInEditTable(ediExtractForm.tblExtractDetails, @"Shipper", @"MSL");
            //ediExtractForm.btnSave.DoClick();
            ediExtractForm.DoSave();

            Wait.UntilInputIsProcessed(waitTimeout: TimeSpan.FromSeconds(5));
            lines = File.ReadAllLines(dataDirectory + type + @"_39568_BAPLIE22Extract.txt").ToArray();
           
            linesNotFound = String.Empty;
            foreach (string checkThis in detailsToCheck)
            {
                int indexOf = lines.ToList().IndexOf(checkThis);
                if (indexOf < 0)
                {
                    linesNotFound += checkThis + "\n";
                }
            }
        }

        

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Ship
            CreateDataFileToLoad(@"DeleteCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n  <AllCargoOnShip>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnShip Terminal='TT1'>\n      <TestCases>39568</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG39568A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>250303</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <weightCertifiedReference>George Best Weight Certs</weightCertifiedReference>\n      <weightCertifiedName>George Last</weightCertifiedName>\n      <messageMode>D</messageMode>\n    </CargoOnShip>\n\n    <CargoOnShip Terminal='TT1'>\n      <TestCases>39568</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG39568A02</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>250002</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <weightCertifiedReference>James Last : Weight Certs</weightCertifiedReference>\n      <weightCertifiedName>George Last's Best People</weightCertifiedName>\n      <messageMode>D</messageMode>\n    </CargoOnShip>\n\n    <CargoOnShip Terminal='TT1'>\n      <TestCases>39568</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG39568A03</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>250102</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <weightCertifiedReference>James Last's : Old Weight Certs</weightCertifiedReference>\n      <weightCertifiedName>George Last's Best People : Certs</weightCertifiedName>\n      <messageMode>D</messageMode>\n    </CargoOnShip>\n  </AllCargoOnShip>\n</JMTInternalCargoOnShip>");

            // Create Cargo On SHip
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n  xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n  <AllCargoOnShip>\n    <operationsToPerform>Verify;Load To DB</operationsToPerform>\n    <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n    <CargoOnShip Terminal='TT1'>\n      <TestCases>39568</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG39568A01</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>250202</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <weightCertifiedReference>George Best Weight Certs</weightCertifiedReference>\n      <weightCertifiedName>George Last</weightCertifiedName>\n      <messageMode>A</messageMode>\n    </CargoOnShip>\n\n    <CargoOnShip Terminal='TT1'>\n      <TestCases>39568</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG39568A02</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>250002</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <weightCertifiedReference>James Last : Weight Certs</weightCertifiedReference>\n      <weightCertifiedName>George Last's Best People</weightCertifiedName>\n      <messageMode>A</messageMode>\n    </CargoOnShip>\n\n    <CargoOnShip Terminal='TT1'>\n      <TestCases>39568</TestCases>\n      <cargoTypeDescr>ISO Container</cargoTypeDescr>\n      <id>JLG39568A03</id>\n      <isoType>2200</isoType>\n      <operatorCode>MSL</operatorCode>\n      <locationId>250102</locationId>\n      <weight>5000.0000</weight>\n      <imexStatus>Import</imexStatus>\n      <commodity>MT</commodity>\n      <dischargePort>NZLYT</dischargePort>\n      <voyageCode>MSCK000002</voyageCode>\n      <weightCertifiedReference>James Last's : Old Weight Certs</weightCertifiedReference>\n      <weightCertifiedName>George Last's Best People : Certs</weightCertifiedName>\n      <messageMode>A</messageMode>\n    </CargoOnShip>\n  </AllCargoOnShip>\n</JMTInternalCargoOnShip>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }

    }

}
