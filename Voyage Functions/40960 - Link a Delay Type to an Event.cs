using System;
using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Voyage_Functions;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Voyage_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40960 : MTNBase
    {
        VoyageEnquiryForm _voyageEnquiryForm;
        VoyageDefinitionForm _voyageDefinitionForm;
        VoyageEventsForm _voyageEventsForm;
        ConfirmationFormYesNo _confirmationFormYesNo;

        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }
        
        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void CreateDelayFromLinkedVoyageEvent()
        {
            MTNInitialize();
            
            string uniqueId = "40960" + GetUniqueId();

            // 1. Navigate to Voyage Enquiry, find voyage MSCK000002 and double click
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry");
            _voyageEnquiryForm = new VoyageEnquiryForm();

            _voyageEnquiryForm.FindVoyageByVoyageCode(@"MSCK000005");
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date  MTNControlBase.FindClickRowInTable(_voyageEnquiryForm.tblVoyages, @"Code^MSCK000005", ClickType.DoubleClick);
            _voyageEnquiryForm.TblVoyages.FindClickRow(new[] { @"Code^MSCK000005" }, ClickType.DoubleClick);

            // 2. Open voyage events button 
            _voyageDefinitionForm = new VoyageDefinitionForm();
            _voyageDefinitionForm.DoVoyageEvents();
            // 3. Add new event and save. Close the form
            _voyageEventsForm = new VoyageEventsForm();
            _voyageEventsForm.DoNewEvent();
            _voyageEventsForm.cmbEvent.SetValue(@"EVENT_40960");
            _voyageEventsForm.txtRemarks.SetValue(uniqueId);
            _voyageEventsForm.DoSaveEvent();
            _voyageEventsForm.DoCloseForm();

            // 4. Refresh the voyage data by searching again and tab to Movements/Delays
            _voyageDefinitionForm.cmbVoyageCodeCombo.SetValue(@"MSC KATYA R. - (MSCK000005)");

            // 5. Find the linked delays in the movement and delys table
            _voyageDefinitionForm.MovementsDelaysTab();
            Console.WriteLine($"uniqueId: {uniqueId}");
            _voyageDefinitionForm.TblMovementDelays.FindClickRow(new[]
                { $"Delay Type^DELAY_40960~remarks^{uniqueId}\r\nCreated from Voyage Event~Voyage Event^EVENT_40960" });

            // 6. Cleanup - delete the delay and event
            _voyageDefinitionForm.btnMovementDelaysDelete.DoClick();
            _confirmationFormYesNo = new ConfirmationFormYesNo(formTitle:  @"Confirm Deletion");
            _confirmationFormYesNo.btnYes.DoClick();
            _voyageDefinitionForm.DoVoyageEvents();
            _voyageEventsForm = new VoyageEventsForm();
            // Monday, 17 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_voyageEventsForm.tblVoyageEvents,@"Event Type^EVENT_40960~Remarks^" + uniqueId,rowHeight: 16);
            _voyageEventsForm.TblVoyageEvents.FindClickRow(new[] { $"Event Type^EVENT_40960~Remarks^{uniqueId}" });
            _voyageEventsForm.DoDeleteEvent();
            

        }



    }

}
