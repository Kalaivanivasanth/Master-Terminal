using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Print_Preview;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Planning;
using MTNUtilityClasses.Classes;
using System;
using System.Drawing;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions.Planning
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43755 : MTNBase
    {
        private LOLOPlanningForm _loloPlanning;

        private Point _cellToClick;

        private const string TestCaseNumber = @"43755";
        private const string VoyageId = @"MSCK000002";
        private const string VoyageName = @" MSC KATYA R.";
        private const string VoyageNameFull = VoyageId + VoyageName;
        private const string FileName = TestCaseNumber + @" - Bays0103Print";
        private const string FileNameDiff = FileName + @"_Diff";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() { base.TestCleanup(); }

        [TestMethod]
        public void BayPlanAndDrawingAttribute()
        {
            MTNInitialize();

            // Step 4
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Planning|LOLO Planning");
            _loloPlanning = new LOLOPlanningForm();

            // Get the voyage and bay required
            _loloPlanning.SelectBaySelectionOption(@"Select Individual Bays");
            _loloPlanning.SelectSingleVoyageBay();
            //MTNControlBase.SetValue(_loloPlanning.cmbVoyage, VoyageNameFull);
            _loloPlanning.cmbVoyage.SetValue(VoyageNameFull);
            //MTNControlBase.SetValue(_loloPlanning.cmbBay, @"01");
            _loloPlanning.cmbBay.SetValue(@"01");

            _loloPlanning.DoCloseSearchPanel();

            // Set the zoom to 2 scale (Saved on CM_USER Open Windows)
             _loloPlanning.SelectZoomLevel(@"View At 2 Scale");

            // Discharge Colour Code
            _loloPlanning.btnDisplayDischargeColourCode.DoClick();

            // Close the Palette / Update windows
            //Keyboard.TypeSimultaneously(VirtualKeyShort.ALT, VirtualKeyShort.F4);
            //Keyboard.TypeSimultaneously(VirtualKeyShort.ALT, VirtualKeyShort.F4);
            Keyboard.Type(VirtualKeyShort.ENTER);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
            Keyboard.Type(VirtualKeyShort.ENTER);

            _loloPlanning.SetFocusToForm();
           
            _cellToClick = new Point(_loloPlanning.grpBays.BoundingRectangle.X + 90, 
                _loloPlanning.grpBays.BoundingRectangle.Y + 90);
            Mouse.Click(_cellToClick);

            _loloPlanning.GetDischargeDetails();
            _loloPlanning.btnDischargePen.DoClick();

            // Set crane to use
            _loloPlanning.btnDischargeJobMode.DoClick();
            _loloPlanning.GetDischargeJobModeDetails();
            //MTNControlBase.SetValue(_loloPlanning.cmbDJMCrane, @"CRN1 - Portainer Crane");
            _loloPlanning.cmbDJMCrane.SetValue(@"CRN1 - Portainer Crane");
            _loloPlanning.btnDJMSave.DoClick();

            // Select Print... from the context menu
            Point positionToClick = new Point(_loloPlanning.grpBays.BoundingRectangle.X + 5,
                _loloPlanning.grpBays.BoundingRectangle.Y + 5);
            Mouse.RightClick(positionToClick);
            //MTNControlBase.ContextMenuSelection(_loloPlanning.contextMenus[1], @"Print...");
            _loloPlanning.ContextMenuSelect(@"Print...");

            // Set details and print
            _loloPlanning.GetContextMenuPrintDetails();
            _loloPlanning.SetValue(_loloPlanning.optSmall, @"1");
            _loloPlanning.chkPrintWorked.DoClick();
            _loloPlanning.SetValue(_loloPlanning.optLoadDischarge, @"1");
            _loloPlanning.btnCMPPrint.DoClick();

            // Save the current frame as a png
            PrintPreviewForm printPreviewForm = new PrintPreviewForm(@"Print Preview - " + VoyageId + @" -" + VoyageName + @" Bays 01 03 ");
            Miscellaneous.CaptureElementAsImage(TestContext, printPreviewForm.rptPane, FileName + @".png");
            printPreviewForm.btnCancel.DoClick();

            // Check the image using an image mask to denote what we don't want to check
            Miscellaneous.ComparePNGFiles(TestContext, FileName, FileName, FileNameDiff);

        }

        private void MTNInitialize()
        {
            searchFor = @"_" + TestCaseNumber + "_";
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;

            // Delete Cargo On Ship
            CreateDataFileToLoad(@"DeleteCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <TestCases>43755</TestCases>\n			<id>JLG43755A01</id>\n			<operatorCode>MSL</operatorCode>\n			<weight>4000</weight>\n			<product>pipe</product>\n			<isoType>4300</isoType>\n		  	<imexStatus>Import</imexStatus>\n			<voyageCode>MSCK000002</voyageCode>\n			<locationId>020682</locationId>\n			<totalQuantity>1</totalQuantity>\n			<messageMode>D</messageMode>\n        </CargoOnShip>\n	</AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n\n\n");

            // Create Cargo On Ship
            CreateDataFileToLoad(@"CreateCargoOnShip.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnShip \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnShip.xsd'>\n    <AllCargoOnShip>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnShip Terminal='TT1'>\n            <TestCases>43755</TestCases>\n			<id>JLG43755A01</id>\n			<operatorCode>MSL</operatorCode>\n			<weight>4000</weight>\n			<product>pipe</product>\n			<isoType>4300</isoType>\n		  	<imexStatus>Import</imexStatus>\n			<voyageCode>MSCK000002</voyageCode>\n			<locationId>020682</locationId>\n			<totalQuantity>1</totalQuantity>\n			<messageMode>A</messageMode>\n        </CargoOnShip>\n	</AllCargoOnShip>\n</JMTInternalCargoOnShip>\n\n");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }
    }

}
