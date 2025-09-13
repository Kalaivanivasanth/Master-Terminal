using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.FormObjects.Message_Dialogs;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Release_Request
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40942 : MTNBase
    {
        ReleaseRequestForm _releaseRequestForm;
        ReleaseRequestAddForm _releaseRequestAddForm;
        ConfirmationFormOK _confirmationFormOK;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void RestrictCarriersForOperators()
        {
            
            MTNInitialize();
            
            // this test seems to require a slower type speed for all the fields.
            int intWaitTime = 300;

            //1 Open release request form and delete release request if found
            //FormObjectBase.NavigationMenuSelection(@"Gate Functions|Release Requests");
            FormObjectBase.MainForm.OpenReleaseRequestFromToolbar();
            _releaseRequestForm = new ReleaseRequestForm(formTitle: @"Release Requests TT1");
            _releaseRequestForm.DeleteReleaseRequests(@"JLG40942ROAD00", @"Release No^JLG40942ROAD00");

            //2. Create a new release request JLG40942ROAD001
            //releaseRequestForm.btnNew.DoClick();
            _releaseRequestForm.DoNew();

            _releaseRequestAddForm = new ReleaseRequestAddForm(@"New request... TT1");

            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release No", @"JLG40942ROAD001", waitTime: intWaitTime);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release Type", @"Road", rowDataType: EditRowDataType.ComboBox, waitTime: intWaitTime);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Carrier", @"PENNOPNALL	PENN - Operator NOT Allowed (40942)", 
                EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Voyage", TT1.Voyage.MSCK000002, EditRowDataType.ComboBoxEdit,
                doDownArrow: true, searchSubStringTo: TT1.Voyage.MSCK000002.Length - 1);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Operator", @"MSLC	MSL Clone - Used for Testing Restricted", 
                EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Empties Only", @"0", rowDataType: EditRowDataType.CheckBox);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release By Type", @"1", rowDataType: EditRowDataType.CheckBox);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Discharge Port", Port.AKLNZ, EditRowDataType.ComboBoxEdit);


            // 3. Add containers - which should not be allowed.
            _releaseRequestAddForm.ClickOnReleaseRequestItem(@"1 x Unspecified ISO type (0 released )");
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"ISO Type", ISOType.ISO2200, EditRowDataType.ComboBoxEdit, doDownArrow: true); 
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Total Requested", @"3");

            //releaseRequestAddForm.btnSave.DoClick();
            _releaseRequestAddForm.DoSave();
            
            warningErrorForm = new WarningErrorForm(formTitle: @"Errors for Release Request update TT1");
            string[] warningErrorToCheck = new string[]
             {
               "Code :90924. Operator MSL Clone - Used for Testing Restricted has denied Carrier PENN - Operator NOT Allowed (40942) access for Cargo Type ISO Container."
             };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnCancel.DoClick();

            //4. change the carrier to allow save and ensure release request has been created
            _releaseRequestAddForm.ClickOnReleaseRequestItem(@"JLG40942ROAD001");
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Carrier", @"PENNOPALL	PENN - Operator Allowed (40942)", EditRowDataType.ComboBoxEdit, doDownArrow: true);

            //releaseRequestAddForm.btnSave.DoClick();
            _releaseRequestAddForm.DoSave();
            _confirmationFormOK = new ConfirmationFormOK(@"Release Request Added", automationIdMessage: @"3", automationIdOK: @"4");
            _confirmationFormOK.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_releaseRequestForm.tblReleaseRequests, @"Release No^JLG40942ROAD001");
            _releaseRequestForm.TblReleaseRequests.FindClickRow(["Release No^JLG40942ROAD001"]);
            //5. create new release JLG40942ROAD002
            //releaseRequestForm.btnNew.DoClick();
            _releaseRequestForm.DoNew();
            _releaseRequestAddForm = new ReleaseRequestAddForm(@"New request... TT1");

            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release No", @"JLG40942ROAD002", waitTime: intWaitTime);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release Type", @"Road", rowDataType: EditRowDataType.ComboBox, waitTime: intWaitTime);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Carrier", @"PENNOPNALL	PENN - Operator NOT Allowed (40942)", 
                EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Voyage", @"MSCK000002	MSC KATYA R.", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Operator", @"MSLC	MSL Clone - Used for Testing Restricted", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Empties Only", @"0", rowDataType: EditRowDataType.CheckBox);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release By Type", @"1", rowDataType: EditRowDataType.CheckBox);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Discharge Port", @"AKL (NZ) Auckland");

            //5. add bottles of beer which should be allowed
            _releaseRequestAddForm.ClickOnReleaseRequestItem(@"1 x Unspecified ISO type (0 released )");
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, "Cargo Type", CargoType.BottlesOfBeer, EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Total Requested", @"100");

            //releaseRequestAddForm.btnSave.DoClick();
            _releaseRequestAddForm.DoSave();
            // MTNControlBase.FindClickRowInTable(_releaseRequestForm.tblReleaseRequests, @"Release No^JLG40942ROAD002");
            _releaseRequestForm.TblReleaseRequests.FindClickRow(["Release No^JLG40942ROAD002"]);            _confirmationFormOK = new ConfirmationFormOK(@"Release Request Added", automationIdMessage: @"3", automationIdOK: @"4");
            _confirmationFormOK.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_releaseRequestForm.tblReleaseRequests, @"Release No^JLG40942ROAD002");
            _releaseRequestForm.TblReleaseRequests.FindClickRow(["Release No^JLG40942ROAD002"]);
            //6. create new release JLG40942ROAD003 - mixture of bottles beer/ container. Should not be allowed
            //releaseRequestForm.btnNew.DoClick();
            _releaseRequestForm.DoNew();
            _releaseRequestAddForm = new ReleaseRequestAddForm(@"New request... TT1");
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release No", @"JLG40942ROAD003", waitTime: intWaitTime);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release Type", @"Road", rowDataType: EditRowDataType.ComboBox, waitTime: intWaitTime);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Carrier", @"PENNOPNALL	PENN - Operator NOT Allowed (40942)", 
                EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Voyage", @"MSCK000002	MSC KATYA R.", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Operator", @"MSLC	MSL Clone - Used for Testing Restricted", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Empties Only", @"0", rowDataType: EditRowDataType.CheckBox);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release By Type", @"1", rowDataType: EditRowDataType.CheckBox);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Discharge Port", @"AKL (NZ) Auckland");

            _releaseRequestAddForm.ClickOnReleaseRequestItem(@"1 x Unspecified ISO type (0 released )");
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, "Cargo Type", CargoType.BottlesOfBeer, EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Total Requested", @"100"); 
            //releaseRequestAddForm.btnAddRequestLine.DoClick();
            _releaseRequestAddForm.DoAddRequestLine();
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, "Cargo Type", CargoType.ISOContainer, EditRowDataType.ComboBoxEdit);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"ISO Type", @"2200	GENERAL", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Total Requested", @"5");
            //releaseRequestAddForm.btnSave.DoClick();
            _releaseRequestAddForm.DoSave();

            warningErrorForm = new WarningErrorForm(formTitle: @"Errors for Release Request update TT1");
            warningErrorToCheck = new string[]
             {
               "Code :90924. Operator MSL Clone - Used for Testing Restricted has denied Carrier PENN - Operator NOT Allowed (40942) access for Cargo Type ISO Container."
             };
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnCancel.DoClick();

            //7. change carrier, now should save
            _releaseRequestAddForm.ClickOnReleaseRequestItem(@"JLG40942ROAD003");
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Carrier", @"PENNOPALL	PENN - Operator Allowed (40942)", EditRowDataType.ComboBoxEdit, doDownArrow: true);
            //releaseRequestAddForm.btnSave.DoClick();
            _releaseRequestAddForm.DoSave();
            _confirmationFormOK = new ConfirmationFormOK(@"Release Request Added", automationIdMessage: @"3", automationIdOK: @"4");
            _confirmationFormOK.btnOK.DoClick();
            // MTNControlBase.FindClickRowInTable(_releaseRequestForm.tblReleaseRequests, @"Release No^JLG40942ROAD003");
            // MTNControlBase.FindClickRowInTable(_releaseRequestForm.tblReleaseRequests, @"Release No^JLG40942ROAD001",clickType: ClickType.DoubleClick);
            _releaseRequestForm.TblReleaseRequests.FindClickRow(["Release No^JLG40942ROAD003"]);
            _releaseRequestForm.TblReleaseRequests.FindClickRow(
                ["Release No^JLG40942ROAD001"], clickType: ClickType.DoubleClick
            );
            _releaseRequestAddForm = new ReleaseRequestAddForm(@"Editing request JLG40942ROAD001... TT1");
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Carrier", @"PENNOPNALL	PENN - Operator NOT Allowed (40942)", 
                EditRowDataType.ComboBoxEdit, doDownArrow: true);
            //releaseRequestAddForm.btnSave.DoClick();
            _releaseRequestAddForm.DoSave();

            warningErrorForm = new WarningErrorForm(formTitle: @"Errors for Release Request update TT1");
            warningErrorToCheck =
            [
                "Code :90924. Operator MSL Clone - Used for Testing Restricted has denied Carrier PENN - Operator NOT Allowed (40942) access for Cargo Type ISO Container."
            ];
            warningErrorForm.CheckWarningsErrorsExist(warningErrorToCheck);
            warningErrorForm.btnCancel.DoClick();
            //releaseRequestAddForm.btnCancel.DoClick();
            _releaseRequestAddForm.DoCancel();
            
        }


    }

}
