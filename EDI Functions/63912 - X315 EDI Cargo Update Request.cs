using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects;
using MTNUtilityClasses.Classes;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Gate_Functions;
using FlaUI.Core.Input;
using MTNForms.Controls;
using System.Text;
using System;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestClass63912 : MTNBase
    {
        EDIOperationsForm _ediOperationsForm;
        CargoUpdateRequestForm _cargoUpdateRequestForm;
        private const string ediFile = "X315_63912.edi";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            BaseClassInitialize_New(testContext);
        }

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        private void MTNInitialize()
        {
            // Log into MTN
            LogInto<MTNLogInOutBO>();

            // call Jade to load file(s)
            CallJadeScriptToRun(TestContext, @"resetData_63912");

            // Create the EDI file
            CreateDataFile(ediFile,
                "ISA*00*          *00*          *ZZ*WWSU           *ZZ*TIDEWORKS      *230306*2311*U*00401*008604479*0*P*|~\r\nGS*QO*WWSU*TIDEWORKS*20230306*2311*008604479*X*004010~\r\nST*315*512584431~\r\nB4***CR*20230304*1242*USSEA*CXRU*125502*L*45R1*USSEA*UN*2~\r\nN9*BM*ET027SWSZ003~\r\nN9*CT*SC-5453~\r\nN9*ZZ*CR*JPSMZ*20230304*1242~\r\nN9*BN*H22CAB01901~\r\nN9*4F*P000099696~\r\nN9*4F*P000114717~\r\nN9*4F*ITCHFMINJP~\r\nN9*4F*ICINTELSCA~\r\nN9*SI*LOE2200634~\r\nN9*HB*027ETSWSZ003~\r\nN9*SCA*WWSU~\r\nN9*SE*CXRU 125502-2~\r\nN9*82*0710400000~\r\nN9*EQ*CXRU 125502-2~\r\nN9*SN*TCF132786~\r\nQ2*9301469*MT***20230304****027W*SCA*STC*L*ETOILE~\r\nR4*R*UN*USSEA*SEATTLE, WA*US***WA~\r\nDTM*140*20230131*1134*PT~\r\nR4*L*UN*USSEA*SEATTLE, WA*US***WA~\r\nDTM*140*20230205*1842*PT~\r\nR4*D*UN*JPSMZ*SHIMIZU*JP~\r\nDTM*140*20230303*1942*PT~\r\nR4*E*UN*JPSMZ*SHIMIZU*JP~\r\nDTM*139*20230303*1942*PT~\r\nSE*27*512584431~\r\nGE*1*008604479~\r\nIEA*1*008604479~\r\n");

        }


        [TestMethod]
        public void X315EDICargoUpdateRequest()
        {
            MTNInitialize();
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // Delete any existing EDI messages for this test case
            StringBuilder statuses = new StringBuilder(EDIOperationsStatusType.DBPartiallyLoadedAsterisk + "^" +
                                                       EDIOperationsStatusType.DBLoaded + "^" +
                                                       EDIOperationsStatusType.Loaded);
            _ediOperationsForm.DeleteEDIMessages(EDIOperationsDataType.GateDocument, @"X315_63912.edi", statuses.ToString());

            MTNControlBase.SetValueInEditTable(_ediOperationsForm.tblEDISearch, @"Status");
            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));

            // Load the EDI message from file
            _ediOperationsForm.LoadEDIMessageFromFile(ediFile);

            _ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile, ClickType.ContextClick);

            _ediOperationsForm.ContextMenuSelect(@"Load To DB");

            // Open Gate Functions | Cargo Update Request form 
            FormObjectBase.NavigationMenuSelection(@"Gate Functions | Cargo Update Request");
            _cargoUpdateRequestForm = new CargoUpdateRequestForm(@"Cargo Update Request TT1");

            // Enter Cargo ID - CXRU1255022 and click the Search button and check that Cargo Update request is created successfully
            _cargoUpdateRequestForm.ValidateCargoUpdateRequestCargoId("CXRU1255022",
                "Cargo ID^CXRU1255022~Status^Pending",
                false);
        }
    }
}
