using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
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
    public class TestCase41052 : MTNBase
    {
        ReleaseRequestForm _releaseRequestForm;
        ReleaseRequestAddForm _releaseRequestAddForm;
        ConfirmationFormOK _confirmationFormOK;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void ReleaseRequestChangingCargoType()
        {
            MTNInitialize();
        
            // this test seems to require a slower type speed for all the fields.
            int intWaitTime = 300;

            //Iteration 1
            // 1. Navigate to Release Requests and create a new release request
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Release Requests");
            _releaseRequestForm = new ReleaseRequestForm(@"Release Requests TT1");

            _releaseRequestForm.DeleteReleaseRequests(@"41052", @"Release No^41052");
            //releaseRequestForm.btnNew.DoClick();
            _releaseRequestForm.DoNew();

            // 2. Add release details
            _releaseRequestAddForm = new ReleaseRequestAddForm(@"New request... TT1");

            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release No", @"41052");
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release Type", @"Road", rowDataType: EditRowDataType.ComboBox, waitTime: intWaitTime);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Voyage", @"MSCK000002	MSC KATYA R.", rowDataType: EditRowDataType.ComboBoxEdit, waitTime: intWaitTime, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Operator", @"MSC", rowDataType: EditRowDataType.ComboBoxEdit, waitTime: intWaitTime, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Release By Type", @"1", rowDataType: EditRowDataType.CheckBox);

            // 3. Click on release request item and add cargo type details and save
            _releaseRequestAddForm.ClickOnReleaseRequestItem(@"1 x Unspecified ISO type (0 released )");

            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, "Cargo Type", CargoType.ISOContainer, rowDataType: EditRowDataType.ComboBoxEdit, waitTime: intWaitTime);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"ISO Type", @"2200	GENERAL", rowDataType: EditRowDataType.ComboBoxEdit, waitTime: intWaitTime, doDownArrow: true);
            //MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"ISO Group", @"22G0	20 Standard Dry", rowDataType: EditRowDataType.ComboBoxEdit, waitTime: intWaitTime, doDownArrow: true);
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Total Requested", @"2");

            //releaseRequestAddForm.btnSave.DoClick();
            _releaseRequestAddForm.DoSave();
            _confirmationFormOK = new ConfirmationFormOK(@"Release Request Added",automationIdMessage: @"3",automationIdOK: @"4");
            _confirmationFormOK.btnOK.DoClick();

            // 4. close release request firm and re-open release request form
            _releaseRequestForm.SetFocusToForm();
            _releaseRequestForm.CloseForm();

            //Iteration 2
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Release Requests", forceReset: true);

            // 5. Find release 41052 created before and put in edit mode
            _releaseRequestForm = new ReleaseRequestForm(formTitle: @"Release Requests TT1");

            //MTNControlBase.SetValue(releaseRequestForm.cmbView, @"Active");
            _releaseRequestForm.cmbView.SetValue(@"Active");
            //MTNControlBase.SetValue(releaseRequestForm.cmbType, @"Road");
            _releaseRequestForm.cmbType.SetValue(@"Road");
            MTNControlBase.SetValueInEditTable(_releaseRequestForm.tblSearchTable, @"Release No", @"41052", waitTime: intWaitTime);
            //releaseRequestForm.btnFind.DoClick();
            //releaseRequestForm.btnEdit.DoClick();
            _releaseRequestForm.DoSearch();
            _releaseRequestForm.DoEdit();
            
            // 6. Change the cargo type from container to motor vehicle and save
            _releaseRequestAddForm = new ReleaseRequestAddForm(@"Editing request 41052... TT1");
            _releaseRequestAddForm.ClickOnReleaseRequestItem(@"2 x 2200  - GENERAL (0 released )");
            MTNControlBase.SetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Cargo Type", @"Motor Vehicle", rowDataType: EditRowDataType.ComboBoxEdit, waitTime: intWaitTime);
            //Keyboard.Press(VirtualKeyShort.TAB);
            //releaseRequestAddForm.btnSave.DoClick();
            _releaseRequestAddForm.DoSave();

            // 7. close and re-open the release request form
            _releaseRequestForm.CloseForm();


            //Iteration 3
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Release Requests", forceReset: true);

            // 8. Open release request 41052 and put in edit mode
            _releaseRequestForm = new ReleaseRequestForm(formTitle: @"Release Requests TT1");
            //MTNControlBase.SetValue(releaseRequestForm.cmbView, @"Active");
            _releaseRequestForm.cmbView.SetValue(@"Active");
            //MTNControlBase.SetValue(releaseRequestForm.cmbType, @"Road");
            _releaseRequestForm.cmbType.SetValue(@"Road");
            MTNControlBase.SetValueInEditTable(_releaseRequestForm.tblSearchTable, @"Release No", @"41052", waitTime: intWaitTime);

            //releaseRequestForm.btnFind.DoClick();
            //releaseRequestForm.btnEdit.DoClick();
            _releaseRequestForm.DoSearch();
            _releaseRequestForm.DoEdit();

            // 9. perform some checks
            _releaseRequestAddForm = new ReleaseRequestAddForm(@"Editing request 41052... TT1");
            _releaseRequestAddForm.ClickOnReleaseRequestItem(@"2 x Motor Vehicle (0 released )");

            string dataValue = MTNControlBase.GetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"Cargo Type");
            Assert.IsTrue(dataValue == @"Motor Vehicle", "Field = Cargo Type is expected to be Motor Vehicle and is : " + dataValue);
            dataValue = MTNControlBase.GetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"ISO Type");
            Assert.IsTrue(dataValue.Length == 0 , "Field = ISO Type field has been found in the table but is not expected: " + dataValue);
            dataValue = MTNControlBase.GetValueInEditTable(_releaseRequestAddForm.tblReleaseRequestData, @"ISO Group");
            Assert.IsTrue(dataValue.Length == 0 , "Field = ISO Group field has been found in the table but is not expected: " + dataValue);

            // 10. Close out of the form and delete the release request
            //releaseRequestAddForm.btnCancel.DoClick();
            _releaseRequestAddForm.DoCancel();
            
        }


    }

}
