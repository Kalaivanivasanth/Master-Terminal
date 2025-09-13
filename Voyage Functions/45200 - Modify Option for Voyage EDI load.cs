using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Misc;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45200 : MTNBase
    {
        VoyageEnquiryForm _voyageEnquiryForm;
        VoyageDefinitionForm _voyageDefinitionForm;
        EDIOperationsForm _ediOperationsForm;

        const string EDIFile1 = "M_45200_VoyageModify.csv";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CreateDataFile(EDIFile1,
                "Control Column,Action,Inward Code,Operator,Scheduled Arrival,Scheduled Departure,Shipping Line Code,Tidal Class Arrival,Tidal Class Departure,Trade Description,Valid Operators,Vessel,Voyage,Arrive_date,Depart_Date\nVOY_CNUM,M,HPD-001-19-9419,MSC,12/5/2019 23:00,13/05/2019 4:00,NZEC,DEFAULT,DEFAULT,DEFAULT,FPT,MSC KATYA R.,42500,13/05/2019  00:01:00 AM,13/05/2050  00:01:00 AM\nPORT_CNUM,NZAKL,NZAKL,,,,,,,,,,,,\n");

            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void ModifyVoyageViaEDI()
        {
            MTNInitialize();

            // 1. Navigate to Voyage enquiry form and find voyage MSCK000002 - double click to open
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry");
            _voyageEnquiryForm = new VoyageEnquiryForm();

            _voyageEnquiryForm.FindVoyageByVoyageCode(@"42500");
            // MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages, @"Code^42500", ClickType.DoubleClick);
            _voyageEnquiryForm.TblVoyages.FindClickRow(["Code^42500"], ClickType.DoubleClick);

            _voyageDefinitionForm = new VoyageDefinitionForm();
            MTNControlBase.ValidateRadioButton(_voyageDefinitionForm.rdoDontShow48Hrs);
            _voyageDefinitionForm.CloseForm();

            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            _ediOperationsForm.DeleteEDIMessages(@"Voyage", @"45200", ediStatus: @"Loaded");
            _ediOperationsForm.LoadEDIMessageFromFile(EDIFile1,specifyType: true,fileType: @"MG_INTEGRATION");
            _ediOperationsForm.ChangeEDIStatus(EDIFile1, @"Loaded", @"Load To DB");

            _voyageEnquiryForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages, @"Code^42500", ClickType.DoubleClick);
            _voyageEnquiryForm.TblVoyages.FindClickRow(["Code^42500"], ClickType.DoubleClick);
            _voyageDefinitionForm = new VoyageDefinitionForm();
            MTNControlBase.ValidateRadioButton(_voyageDefinitionForm.rdoDontShow48Hrs);

        }

        

    }

}
