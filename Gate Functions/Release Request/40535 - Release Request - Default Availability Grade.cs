using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.Terminal_Functions;
using MTNUtilityClasses.Classes;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Release_Request

{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40535 : MTNBase
    {
      
        private EDIOperationsForm _ediOperationsForm;
        private TerminalConfigForm _terminalConfigForm;
        private ReleaseRequestForm _releaseRequestForm;
        private ReleaseRequestAddForm _releaseRequestAddForm;

        private const string TestCaseNumber = @"40535";

        const string EDIFile1 = "M_" + TestCaseNumber + "_ReleaseRequest_NotNull_Del.edi";
        const string EDIFile2 = "M_" + TestCaseNumber + "_ReleaseRequest_NotNull_Add.edi";
        const string EDIFile3 = "M_" + TestCaseNumber + "_ReleaseRequest_Null_Del.edi";
        const string EDIFile4 = "M_" + TestCaseNumber + "_ReleaseRequest_Null_Add.edi";


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CreateDataFile(EDIFile1,
                "RELEASEREQUEST\n**IGNORE**,ACTION,REQUEST DATE,RELEASE REQUEST NUMBER,RELEASE TYPE (STRING),RELEASE BY TYPE,STATUS (BULK RELEASE),OPERATOR,CARRIER CODE,AVAILABILITY GRADES,CARGO TYPE DESCR,CLEAN,EMPTIES ONLY,ISO TYPE,ISO GROUP,QUANTITY,SUB-TERMINAL\n,D,09/30/2018,40535N,ROAD,YES,ACTIVE,MES,,,CONT,,YES,4510,,1,Depot");

            CreateDataFile(EDIFile2,
                "RELEASEREQUEST\n**IGNORE**,ACTION,REQUEST DATE,RELEASE REQUEST NUMBER,RELEASE TYPE (STRING),RELEASE BY TYPE,STATUS (BULK RELEASE),OPERATOR,CARRIER CODE,AVAILABILITY GRADES,CARGO TYPE DESCR,CLEAN,EMPTIES ONLY,ISO TYPE,ISO GROUP,QUANTITY,SUB-TERMINAL\n,A,09/30/2018,40535,ROAD,YES,ACTIVE,MES,,1,CONT,,YES,4510,,1,Depot");

            CreateDataFile(EDIFile3,
                "RELEASEREQUEST\n**IGNORE**,ACTION,REQUEST DATE,RELEASE REQUEST NUMBER,RELEASE TYPE (STRING),RELEASE BY TYPE,STATUS (BULK RELEASE),OPERATOR,CARRIER CODE,AVAILABILITY GRADES,CARGO TYPE DESCR,CLEAN,EMPTIES ONLY,ISO TYPE,ISO GROUP,QUANTITY,SUB-TERMINAL\n,D,09/30/2018,40535,ROAD,YES,ACTIVE,MES,,1,CONT,,YES,4510,,1,Depot");

            CreateDataFile(EDIFile4,
                "RELEASEREQUEST\n**IGNORE**,ACTION,REQUEST DATE,RELEASE REQUEST NUMBER,RELEASE TYPE (STRING),RELEASE BY TYPE,STATUS (BULK RELEASE),OPERATOR,CARRIER CODE,AVAILABILITY GRADES,CARGO TYPE DESCR,CLEAN,EMPTIES ONLY,ISO TYPE,ISO GROUP,QUANTITY,SUB-TERMINAL\n,A,09/30/2018,40535N,ROAD,YES,ACTIVE,MES,,,CONT,,YES,4510,,1,Depot");

            LogInto<MTNLogInOutBO>("USER40535");

            SetTerminalConfigAvGrade(@"1", false);

        }

        [TestMethod]
        public void DefaultAvailabilityGrade()
        {
            MTNInitialize();

            // Step 5 - 6
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT2");

            _ediOperationsForm.DeleteEDIMessages(@"Release request", @"M_" + TestCaseNumber);
           
            FindLoadToDb(EDIFile1);
            FindLoadToDb(EDIFile2);
           
            //  Step 7 to 10
            DoReleaseRequestStuff(TestCaseNumber, @"G1-Diary Standard");

            // Step 11 - 13
            SetTerminalConfigAvGrade(@"", true);

            // Step 14 - 15
            _ediOperationsForm.SetFocusToForm();

            FindLoadToDb(EDIFile3);
            FindLoadToDb(EDIFile4);

            // Step 16 - 19
            DoReleaseRequestStuff(TestCaseNumber + @"N", @"No Availability Grade");

            // Step 20 - 22 - Handled by the TerminalSystemReset script

        }

        private void DoReleaseRequestStuff(string releaseRequestNumber, string availabilityGrade)
        {
            FormObjectBase.MainForm.OpenReleaseRequestFromToolbar();
            _releaseRequestForm = new ReleaseRequestForm(@"Release Requests TT2");
            
            _releaseRequestForm.cmbView.SetValue(@"Active", doDownArrow: true, searchSubStringTo: "Active".Length - 1);
            _releaseRequestForm.cmbType.SetValue(@"Road", doDownArrow: true, searchSubStringTo: "Road".Length - 1);
            MTNControlBase.SetValueInEditTable(_releaseRequestForm.tblSearchTable, @"Release No", releaseRequestNumber);
            _releaseRequestForm.DoSearch();
            // MTNControlBase.FindClickRowInTable(_releaseRequestForm.tblReleaseRequests,
                // @"Status^Active~Release No^"+ releaseRequestNumber +"~O/S^1~Tot.Reqd^1~Type^Road", ClickType.DoubleClick);
            _releaseRequestForm.TblReleaseRequests.FindClickRow(["Status^Active~Release No^"+ releaseRequestNumber +"~O/S^1~Tot.Reqd^1~Type^Road"], ClickType.DoubleClick);         
            _releaseRequestAddForm = new ReleaseRequestAddForm(@"Editing request " + releaseRequestNumber + "... TT2");
            _releaseRequestAddForm.tblReleaseRequestList.Focus();
            Keyboard.Press(VirtualKeyShort.DOWN);
            MTNControlBase.ValidateValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Availability Grades",
                availabilityGrade);
            _releaseRequestAddForm.CloseForm();
           
        }

        private void FindLoadToDb(string fileDetailsToLoad)
        {

            _ediOperationsForm.LoadEDIMessageFromFile(fileDetailsToLoad, true, @"CPRC_ReleaseRequest", timeToWait: 0);

          //  _ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + fileDetailsToLoad,
          //      ClickType.ContextClick);
            _ediOperationsForm.TblMessages.FindClickRow([$"Status^Loaded~File Name^{fileDetailsToLoad}"],
                ClickType.ContextClick);
            _ediOperationsForm.ContextMenuSelect(@"Load To DB");
        }

        private void SetTerminalConfigAvGrade(string avGradeValue, bool reset)
        {
            FormObjectBase.MainForm.OpenTerminalConfigFromToolbar();
            _terminalConfigForm = new TerminalConfigForm();
            _terminalConfigForm.GetGenericTabAndTable(@"Defaults");
            _terminalConfigForm.DoEdit();
            MTNControlBase.SetValueInEditTable(_terminalConfigForm.tblGeneric, @"Availability Grade", avGradeValue);
            _terminalConfigForm.DoSave();
            _terminalConfigForm.CloseForm();
        }
    }

}
