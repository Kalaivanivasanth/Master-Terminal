using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNBaseClasses.BaseClasses.Web;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using MTNWebPages.PageObjects.Mobile_Apps;
using System;
using System.Net.Http.Formatting;
using DataObjects.LogInOutBO;
using FlaUI.Core.Input;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNDesktopFlaUI.Classes;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNForms.FormObjects.Yard_Functions.Road_Operations;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNWebPages.PageObjects.Mobile_Apps.Cargo_Apps.Pack_Container;
using MTNWebPages.PageObjects.Popups;


namespace MTNAutomationTests.TestCases.Web.Mobile_Apps.Cargo_Apps.Pack_Container
{
    [TestClass, TestCategory(TestCategories.MTNWeb)]
    public class TestCase41223 : MobileAppsBase
    {
        
        const string TestCaseNumber = "41223";

        string[,] _updateDetails;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize()  {}
        
        [ClassCleanup]
        public new static void ClassCleanUp() => WebBase.ClassCleanUp();

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
            MTNBase.MTNTestCleanup(TestContext);
            MTNBase.MTNCleanup();
        }

        public void MTNInitialize()
        {

            SetupAndLoadInitializeData(TestContext);
            CallJadeScriptToRun(TestContext, $"resetData_{TestCaseNumber}" );

            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date LogIntoMTN();
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]

        public void OperatorSelectionPackContainerFunction()
        {

            MTNInitialize();

            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection($"{NavigatorMenus.RoadGate}");
            FormObjectBase.MainForm.OpenRoadGateFromToolbar();

            roadGateForm = new RoadGateForm(formTitle: $"{RoadGateForm.FormTitle} {terminalId}");
            /*// Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date 
            roadGateForm.txtRegistration.SetValue("41223");
            roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
            roadGateForm.cmbGate.SetValue("GATE");*/
            roadGateForm.SetRegoCarrierGate("41223");
            roadGateForm.btnReceiveEmpty.DoClick();

            RoadGateDetailsReceiveForm roadGateDetailsReceiveForm =
               new RoadGateDetailsReceiveForm(
                   formTitle: $"{RoadGateDetailsReceiveForm.FormTitleReceiveEmptyContainer} {terminalId}");

            roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO2200, doDownArrow: true);
            roadGateDetailsReceiveForm.TxtCargoId.SetValue("JLG41223A01");
            roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("2000");
            roadGateDetailsReceiveForm.CmbImex.SetValue(IMEX.Storage, additionalWaitTimeout: 1000, doDownArrow: true);
            roadGateDetailsReceiveForm.CmbVoyage.SetValue(TT1.Voyage.NULL, doDownArrow: true);
            roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.ABOC, doDownArrow: true);
            roadGateDetailsReceiveForm.CmbDischargePort.SetValue(Port.AKLNZ, doDownArrow: true);

            roadGateDetailsReceiveForm.BtnSave.DoClick();

            /*// Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date 
            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();*/
            WarningErrorForm.CompleteWarningErrorForm($"Warnings for Gate In/Out {terminalId}");

            roadGateForm.btnSave.DoClick();

            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry");
            FormObjectBase.MainForm.OpenVoyageEnquiryFromToolbar();

            VoyageEnquiryForm voyageEnquiryForm = new VoyageEnquiryForm(formTitle: $"Voyage Enquiry {terminalId}");
            voyageEnquiryForm.btnVoyageSearcher.DoClick();
            //voyageEnquiryForm.DoVoyageSearcher();
            voyageEnquiryForm.ShowSearcher();
            voyageEnquiryForm.txtSearcherVoyage.SetValue("TEST41223");
            voyageEnquiryForm.btnSearcherRunSearch.DoClick();
            //voyageEnquiryForm.DoSearcherRunSearch();
            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(voyageEnquiryForm.tblVoyages, @"Code^TEST41223", ClickType.DoubleClick);
            voyageEnquiryForm.TblVoyages.FindClickRow(new[] { "Code^TEST41223" }, ClickType.DoubleClick);
            
            VoyageDefinitionForm voyageDefinitionForm = new VoyageDefinitionForm(); // In this case passing null finds the form where passing the name doesn't
            voyageDefinitionForm.PortsOfCallandOperators();

            // Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date 
            /*string[] validOperators =
           {
               // Selected Operators list

               "AER	European Operator",
               "COS	COS HAKONE",
               "CSA    CONTSHIP LINE",
               "FLS    Frontier Line Systems",
               "MSC    Mediterranean Shipping  Company",
               "MSK    MAERSK",
               "MSL    Messina Line"

            };*/
            voyageDefinitionForm.CheckSelectedOperators(new[]
            {
                "AER	European Operator", "COS	COS HAKONE", "CSA    CONTSHIP LINE", "FLS    Frontier Line Systems",
                "MSC    Mediterranean Shipping  Company", "MSK    MAERSK", "MSL    Messina Line"
            });
            voyageDefinitionForm.CloseForm();

            LogInto<MobileAppsLogInOutBO>();
            HomePage = new MA_TilesPage(TestContext);

            HomePage.ClickTile(HomePage.BtnPackReact);

            MA_PackContainerSearchReactPage searchReactPage = new MA_PackContainerSearchReactPage(TestContext);
            searchReactPage.SearchForCargoId(@"JLG41223A01");

            MA_PackContainerReactPage packContainerPage = new MA_PackContainerReactPage(TestContext);
            packContainerPage.ValidateHaveCorrectCargoId("JLG41223A01");

            _updateDetails = new [,]
            {
                { MA_PackContainerReactPage.constCommodity, "GEN - General" },
                { MA_PackContainerReactPage.constVoyage, "TEST41223" },
            };
            packContainerPage.SetFields(_updateDetails);

            /*// Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date 
            validOperators = new[]
            {
                "AER - European Operator",
                "COS - COS HAKONE",
                "CSA - CONTSHIP LINE",
                "FLS - Frontier Line Systems",
                "MSC - Mediterranean Shipping Company",
                "MSK - MAERSK",
                "MSL - Messina Line"
            };*/
            packContainerPage.ValidateOperatorListDetails(new[]
            {
                "AER - European Operator", "COS - COS HAKONE", "CSA - CONTSHIP LINE", "FLS - Frontier Line Systems",
                "MSC - Mediterranean Shipping Company", "MSK - MAERSK", "MSL - Messina Line"
            });
            
            _updateDetails = new [,]
            {
                { MA_PackContainerReactPage.constOperator, "COS HAKONE" },
                { MA_PackContainerReactPage.constImex, "Export"}
            };
            packContainerPage.SetFields(_updateDetails);
            
            searchReactPage.DoNext();

            WarningErrorPopup warningErrorPopupVMT = new WarningErrorPopup(TestContext);
            warningErrorPopupVMT.DoOK();

            MA_PackContainerDetailsReactPage packContainerDetailsPage = new MA_PackContainerDetailsReactPage(TestContext);
            packContainerDetailsPage.DoBack();

            packContainerPage = new MA_PackContainerReactPage(TestContext);
            packContainerPage.DoExpander();

            string[,] detailsToValidate =
             {
                { MA_PackContainerReactPage.constOperator, "COS" },
                { MA_PackContainerReactPage.constVoyage, "TEST41223" },
                { MA_PackContainerReactPage.constImex, "Export" },
                { MA_PackContainerReactPage.constCommodity, "General" }

             };
            packContainerPage.ValidateDetails(detailsToValidate);
        
            MTNDesktop.SetFocusToMainWindow();

            // Road Operation - Move the vehicle and process the Road Exit
            /*// Monday, 3 March 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection($"{NavigatorMenus.RoadOperations}", forceReset: true);
            FormObjectBase.MainForm.OpenRoadOperationsFromToolbar();
            roadOperationsForm = new RoadOperationsForm($"{RoadOperationsForm.FormTitle} {terminalId}");

            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, "Cargo ID^JLG41223A01",
                ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.tblYard2.FindClickRow(new[] { "Cargo ID^JLG41223A01" }, ClickType.ContextClick);
            roadOperationsForm.ContextMenuSelect($"{RoadOperationsForm.ContextMenuItems.YardMoveSelected}");
            
            warningErrorForm = new WarningErrorForm(formTitle: "Warnings for Operations Move TT1");
            warningErrorForm.btnSave.DoClick();

            MTNControlBase.FindClickRowInTable(roadOperationsForm.tblYard, "Cargo ID^JLG41223A01",
                ClickType.ContextClick, rowHeight: 16);
            roadOperationsForm.ContextMenuSelect($"{RoadOperationsForm.ContextMenuItems.ProcessRoadExit}");*/
            RoadOperationsForm.OpenMoveAllCargoAndRoadExit($"{RoadOperationsForm.FormTitle} {terminalId}",
                new[] { "41223" });

        }

        private static void SetupAndLoadInitializeData(TestContext testContext)
        {
            fileOrder = 1;
            searchFor = $"_{TestCaseNumber}_";

            // Delete Cargo On Site
            CreateDataFileToLoad(@"DeleteCargoOnSite.xml",
                "<?xml version='1.0' encoding='UTF-8'?>\n<JMTInternalCargoOnSite \n    xmlns:xsi='http://www.jademasterterminallogistics.com/JMTInternal' xsi:noNamespaceSchemaLocation='JMTInternalCargoOnSite.xsd'>\n    <AllCargoOnSite>\n        <operationsToPerform>Verify;Load To DB</operationsToPerform>\n        <operationsToPerformStatuses>EDI_STATUS_VERIFIED / EDI_STATUS_VERIFIED_WARNINGS;EDI_STATUS_DBLOADED</operationsToPerformStatuses>\n        <CargoOnSite Terminal='TT1'>\n            <cargoTypeDescr>ISO Container</cargoTypeDescr>\n              <id>JLG41223A01</id>\n       <isoType>2200</isoType>\n     <operatorCode>COS</operatorCode>\n            <locationId>MKBS01</locationId>\n            <weight>6000</weight>\n            <imexStatus>Export</imexStatus>\n            <commodity>GEN</commodity>\n            <dischargePort>NZAKL</dischargePort>\n            <voyageCode>TEST41223</voyageCode>\n        		<messageMode>D</messageMode>\n        </CargoOnSite>\n	    </AllCargoOnSite>\n</JMTInternalCargoOnSite>");

            // call Jade to load file(s)
            CallJadeToLoadFiles(testContext);
        }


        
        

    }
}
