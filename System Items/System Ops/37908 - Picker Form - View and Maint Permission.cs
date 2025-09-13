using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Gate_Functions.Vehicle;
using MTNForms.FormObjects.Message_Dialogs;
using System;
using System.Linq;
using DataObjects;
using DataObjects.LogInOutBO;
using FlaUI.Core.AutomationElements;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.System_Items.System_Ops
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase37908 : MTNBase
    {
        RoadGatePickerForm _roadGatePickerForm;
        MTNLogInOutBO _mtnLogInOutBO;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();


        [TestMethod]
        public void PickerFormViewAndMaintenance()
        {
            // System Role additional permissions to check
            string[] userSignons = 
            {
               "USERPVMOFF^p@5swordH2^You do not have sufficient security rights to access window Picker. Contact the System administrator to add it if required.",
               "USERPVON^p@5swordH2",
               "USERPMON^p@5swordH2"
            };

            // loop through users
            foreach (string user in userSignons)
            {
                string[] userDetails = user.Split('^');
                
                Console.WriteLine($"userDetails[0]: {userDetails[0]}");
                TestRunDO.GetInstance().SetUserName(userDetails[0]);
                if (_mtnLogInOutBO == null)
                    _mtnLogInOutBO = LogInto<MTNLogInOutBO>();
                else
                    LogInto(_mtnLogInOutBO);

                // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
                // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date roadGateForm = new RoadGateForm();
                roadGateForm = FormObjectBase.Create<RoadGateForm>(openFormAction: FormObjectBase.MainForm.OpenRoadGateFromToolbar);
                roadGateForm.SetRegoCarrierGate("37908");
                roadGateForm.txtNewItem.SetValue(@"JLGBOOK37908A01", 100);

                //  Do specific tests for the required user
                _roadGatePickerForm = null;
                switch (userDetails[0])
                {
                    case @"USERPVMOFF":
                        if (userDetails.Length == 3)
                        {
                            var confirmationFormOK = new ConfirmationFormOK(@"Security Warning",
                                automationIdMessage: @"3", automationIdOK: @"4");
                            Assert.IsTrue(confirmationFormOK.CheckMessageMatch(userDetails[2]),
                                @"TestCase37908 - Security Warning message is different");
                            confirmationFormOK.btnOK.DoClick();
                        }
                        break;
                    case @"USERPVON":
                        _roadGatePickerForm = new RoadGatePickerForm(@"Picker");
                        _roadGatePickerForm.btnOK.DoClick(doMoveMouse: true);
                        
                        try
                        {
                            _roadGatePickerForm.btnCancel.DoClick();
                        }
                        catch
                        {
                            Assert.IsTrue(false,
                                $"TestCase37908 - Signon: {userDetails[0]} failed as able to click the OK button on the Picker");
                        }
                        break;
                    case @"USERPMON":
                        // Step 20
                        _roadGatePickerForm = new RoadGatePickerForm(@"Picker");
                        CheckRowMatches("2 x 2200  GENERAL MSC Booking Item JLGBOOK37908A01");
                        Keyboard.Press(VirtualKeyShort.DOWN);
                        
                        // Step 21
                        CheckRowMatches("2 x 4510  General purpose, upper vents MSC Booking Item JLGBOOK37908A01");
                        Keyboard.Press(VirtualKeyShort.UP);
                        CheckRowMatches("2 x 2200  GENERAL MSC Booking Item JLGBOOK37908A01");

                        // Step 22
                        _roadGatePickerForm.btnOK.DoClick();
                        
                        RoadGateDetailsReceiveForm roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive Full Container " + 
                            roadGateForm.GetTerminalName());
                        Wait.UntilResponsive(roadGateDetailsReceiveForm.GetForm(), TimeSpan.FromMilliseconds(2000));
                        roadGateDetailsReceiveForm.BtnCancel.DoClick();

                        break;
                    default:
                        Assert.IsTrue(false, @"TestCase37908 - Unknown user " + userDetails[0] + " found.");
                        break;
                }

                roadGateForm.btnCancel.DoClick();
                roadGateForm.CloseForm();
                
                VehicleCancelReasonForm vehicleCancelReasonForm = new VehicleCancelReasonForm(@"Vehicle Cancel Reason " + roadGateForm.GetTerminalName());
                vehicleCancelReasonForm.txtReason.SetValue(@"Test step complete");
                vehicleCancelReasonForm.btnOK.DoClick();

                roadGateForm.CloseForm();
          
                // logout.  Doing this way as 2nd and subsequent times getting returns a null object for the button
                if (userDetails[0] != @"USERPMON")  LogOffMTN();

            }
        }

        void CheckRowMatches(string detailsToMatch)
        {
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date var rowSelected = _roadGatePickerForm.tblPickerItems.AsDataGridView().Rows
            var rowSelected = _roadGatePickerForm.TblPickerItems.GetElement().AsDataGridView().Rows
                .Where(r => Miscellaneous.ReturnTextFromTableString(r.Name).Length > 0).ToList();

            Assert.IsTrue(rowSelected.Count() == 1, "TestCase37908 - Should have return only one selected row");
            
            Assert.IsTrue(rowSelected[0].Name.Equals(detailsToMatch),
                $"TestCase37908 - Expected: {detailsToMatch} does NOT match Actual: {rowSelected[0].Name}");

        }

    }

}
