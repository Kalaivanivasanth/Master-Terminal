using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Prenotes
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase63004 : MTNBase
    {
        PreNoteForm _preNoteForm;
        private WarningErrorForm _warningError;
        private ConfirmationFormYesNo _confirmationForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
           LogInto<MTNLogInOutBO>();
        }

        /// <summary>
        /// To test that if the user is pre-noting multiple hazardous containers against a booking and hit "Save-Next" between creating them, 
        /// the hazardous info should populate in the form.
        /// </summary>
        [TestMethod]
        public void SaveNextButtonPopulatesHazardDetailsonSubsequentPrenotes()
        {

            MTNInitialize();

            // Step 1 Open Gate Functions | Pre-Notes
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Pre-Notes");

            // Step 2 Enter Cargo Id -@cargoID and click the Search button
            _preNoteForm = new PreNoteForm($"Pre-Notes {terminalId}");
            MTNControlBase.SetValueInEditTable(_preNoteForm.tblPreNoteSearch, @"Cargo Id", @"JLG63004");
            _preNoteForm.DoSearch();

            var rowFound = MTNControlBase.FindClickRowInTable(_preNoteForm.TblPreNotes.GetElement(), "ID^JLG63004A01", rowHeight: 18, doAssert: false, clickType: ClickType.None);

            if (rowFound)
            {
                _preNoteForm.DoSelectAll();
                _preNoteForm.DoDelete();

                _confirmationForm = new ConfirmationFormYesNo(@"Confirm Deletion");
                _confirmationForm.btnYes.DoClick();

                _warningError = new WarningErrorForm(formTitle: $"Warnings for Delete Pre-Note {terminalId}");
                _warningError.btnSave.DoClick();
            }

            // Step 3 Create a new Pre-Note using the booking reference
            _preNoteForm.DoNew();
            RoadGateDetailsReceiveForm _preNoteDetailsForm = new RoadGateDetailsReceiveForm(formTitle: $"PreNote Full Container {terminalId}");

            _preNoteDetailsForm.TxtBooking.SetValue(@"BOOK63004");
            PreNotePickerForm _preNotePickerForm = new PreNotePickerForm(@"Picker");
            _preNotePickerForm.TblPickerItems.FindClickRow(new[] { "Cargo Type^ISO Container~Description^10 x 2200  GENERAL" });
            _preNotePickerForm.btnOK.DoClick();

            _preNoteDetailsForm.TxtCargoId.SetValue(@"JLG63004A01");
            _preNoteDetailsForm.MtTotalWeight.SetValueAndType("4000");

            // Step 4 Validate the Hazard Details field and click the Save-Next button
            _preNoteDetailsForm.TxtHazardDetails.ValidateText("2.1 (1030) III");
            _preNoteDetailsForm.BtnSaveNext.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: $"Warnings for Pre-Notes {terminalId}");
            warningErrorForm.btnSave.DoClick();

            // Step 5 Validate the Hazard Details field and click the Save button
            _preNoteDetailsForm.TxtHazardDetails.ValidateText("2.1 (1030) III");
            _preNoteDetailsForm.TxtCargoId.SetValue(@"JLG63004A02");
            _preNoteDetailsForm.MtTotalWeight.SetValueAndType("4000");

            _preNoteDetailsForm.BtnSave.DoClick();

            warningErrorForm = new WarningErrorForm(formTitle: $"Warnings for Pre-Notes {terminalId}");
            warningErrorForm.btnSave.DoClick();

        }
       
    }
}

