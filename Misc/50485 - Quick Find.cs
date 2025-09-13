using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Quick_Find;
using MTNUtilityClasses.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;
using Keyboard = MTNUtilityClasses.Classes.MTNKeyboard;

namespace MTNAutomationTests.TestCases.Master_Terminal.Misc
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase50485 : MTNBase
    {

        private QuickFindForm _quickFindForm;

        readonly (string Command, string Form, string Title)[] _quickFindFormDetails = [
            ( "F BOL", "VoyageBOLMaintenanceForm", "Voyage BOL Maintenance TT1" ),
            ( "F BOOK","BookingForm","Booking TT1" ),
            ( "F BULK","ReleaseRequestForm","Release Requests TT1" ),
            ( "F CARGO","CargoEnquiryForm","Cargo Enquiry TT1" ),
            ( "F EDI","EDIOperationsForm","EDI Operations TT1" ),
            //( "F GATE","RoadGateForm","Road Gate TT1" ),
            ( "F JOB","VoyageJobOperationsForm","Job Operations TT1" ),
            ( "F LOLO","LOLOPlanningForm","ACTUAL Bay View, by Discharge Port TT1" ),
            ( "F OOW","OrderOfWorkForm","Order of Work TT1" ),
            ( "F PACK","PackUnpackForm","Pack/Unpack TT1" ),
            ( "F PRE","PreNoteForm","Pre-Notes TT1" ),
            ( "F RAIL","RailOperationsForm","000000 Rail Area  - ACTUAL RAIL OPERATIONS TT1" ),
            ( "F ROAD","RoadOperationsForm","Road Operations TT1" ),
            ( "F RTQ","RadioTelemetryQueueForm","Radio Telemetry Queue TT1" ),
            ( "F SAT","SatelliteViewForm","Satellite View - Default TT1" ),
            ////( "F SEL","SelectorQueryForm",string.Empty ),
            ////( "F S Commodities","SystemAdminForm","System Administration TT1" ),
            ////( "F T Machine","TerminalAdministrationForm",null ),
            ////( "F TA","TerminalAreaDefinitionForm","Terminal Area Definition TT1" ),
            //( "F VBS","VehicleBookingsForm","Vehicle Bookings -Day View TT1" ),       // doesn't do anything
            //( "F VV","VehicleVisitForm","Vehicle Visit Enquiry TT1" ),
            ( "F VOY","VoyageEnquiryForm","Voyage Enquiry TT1" ),
            ( "F VOPS","VoyageOperationsForm","Voyage Operations TT1" ),
            //( "F HH",string.Empty, string.Empty ),        // We should no longer be using the HH apps moved to web so won't test this
            //( "F VM FK01",string.Empty, string.Empty ),   // We should no longer be using the VMT apps moved to web so won't test this
            ( "F USER","QuickFindUserPreferenceForm","Quick Form User Preferences" ),
            ( "A MKBS01","LocationEnquiryForm","MKBS01 Location Enquiry TT1" ),
            ( "L GBRF01 1010","LocationEnquiryForm","1010 Location Enquiry TT1" ),
            ( "B NAVBOOK50485A001","BookingForm","Editing Booking NAVBOOK50485A001 TT1" ),
            //( "T 50485A","VehicleVisitForm","Vehicle Visit Enquiry TT1" ),
            ( "C 50485A01","CargoEnquiryDirectForm","NAV50485A01 TT1" ),
            ////( "I 13","InvoiceLinesForm",null ), // 40 in hmast at the moment, 13 in XMAST
            ( "R NAV50485RRA001","ReleaseRequestAddForm","Editing request NAV50485RRA001... TT1" ),
            ( "G NAV","CargoEnquiryDirectForm","NAV50485A06 TT1" ),
            ( "N NAV50485A02","CargoEnquiryDirectForm","NAV50485A02 TT1" ), ];

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) => BaseClassInitialize_New(context);
        
        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void QuickFindFormTests()
        {
            MTNInitialize();
            
            /*SelectUsingQuickFind("?");
            var quickFindMainHelp = new QuickFindMainHelp("Valid quick action formats:");
            quickFindMainHelp.ValidateHelpText();
            quickFindMainHelp.btnOK.DoClick();
            
            SelectUsingQuickFind("F ?");
            var quickFindFormHelp = new QuickFindFormHelp("Valid 'FORM quick open' requests:");
            //quickFindFormHelp.ValidateHelpText();
            quickFindFormHelp.btnOK.DoClick();*/

           CheckCanOpenFormAndClose(_quickFindFormDetails);

        }

        private void CheckCanOpenFormAndClose((string Command, string Form, string Title)[] quickFindFormDetails)
        {
            var formsNotAbleToOpen = string.Empty;
            foreach (var quickFindFormDetail in quickFindFormDetails)
            {
                Console.WriteLine($"Processing: {quickFindFormDetail.Command}  {quickFindFormDetail.Form}  {quickFindFormDetail.Title}");
                SelectUsingQuickFind(quickFindFormDetail.Command);
                
                var type = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(ty => ty.Name == quickFindFormDetail.Form);
                
                if (type == null)
                {
                    formsNotAbleToOpen += $"- Unable to find definition for {quickFindFormDetail.Form}\r\n";
                    continue;
                }

                object formObject = null;
                try
                {
                    // Use FormObjectBase.CreateForm static method to instantiate form by type
                   // if (!string.IsNullOrEmpty(quickFindFormDetail.Title))
                    //{
                        var constructor = type.GetConstructor([typeof(string)]);
                        if (constructor != null)
                        {
                            // Use reflection to invoke the static method
                            var formObjectBaseType = typeof(FormObjectBase);
                            var createFormMethod = formObjectBaseType.GetMethod("CreateForm", 
                                BindingFlags.Public | BindingFlags.Static, null,
                                [typeof(Type), typeof(string)], null);

                            if (createFormMethod != null)
                            {
                                formObject = createFormMethod.Invoke(null, [type, quickFindFormDetail.Title]);
                            }
                        }
                    //}
                    //else
                    //{
                        // Create without title parameter
                     //   var formObjectBaseType = typeof(FormObjectBase);
                      //  var createFormMethod = formObjectBaseType.GetMethod("CreateForm", 
                       //     BindingFlags.Public | BindingFlags.Static, null,
                        //    [typeof(Type)], null);

                   //     if (createFormMethod != null)
                    //        formObject = createFormMethod.Invoke(null, [type]);
                    //}

                    //if (formObject == null)
                   // {
                   //     formsNotAbleToOpen += $"- Unable to create instance of {quickFindFormDetail.Form} with title {quickFindFormDetail.Title}\r\n";
                    //    continue;
                    //}
                }
                catch (Exception ex)
                {
                    formsNotAbleToOpen += $"- Error creating instance of {quickFindFormDetail.Form}: {ex.Message}\r\n";
                    continue;
                }

                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(500));
                var keys = type.Name.Equals("QuickFindUserPreferenceForm")
                    ? new[] { VirtualKeyShort.ALT, VirtualKeyShort.F4 } // Close user preferences form
                    : new[] { VirtualKeyShort.CONTROL, VirtualKeyShort.F4 }; // Close regular forms

                // Apply the selected key combination
                Console.WriteLine(
                    $"formObject: {formObject}  | {formObject.GetType()} | {formObject.GetType().Name} | {formObject.GetType().GetMethod("SetFocusToForm", Type.EmptyTypes)}");
                formObject?.GetType().GetMethod("SetFocusToForm", Type.EmptyTypes)?.Invoke(formObject, null);
                Keyboard.TypeSimultaneously(keys);
            }
            
            if (!string.IsNullOrEmpty(formsNotAbleToOpen))
                Assert.Fail(
                    $"{GetType().Name}::{MethodBase.GetCurrentMethod()?.Name} - The following issues where found while running 'Quick Find':\r\n{formsNotAbleToOpen}");
        }


        private void SelectUsingQuickFind(string stringToFind)
        {
            if (_quickFindForm == null)
            {
                Keyboard.TypeSimultaneously(VirtualKeyShort.F10);
                _quickFindForm = new QuickFindForm();
            }
            else
                _quickFindForm.SetFocusToForm();

            _quickFindForm.txtFind.Focus();
            Keyboard.Type(stringToFind, 200);
           _quickFindForm.btnFind.DoClick();
           
        }

    }

}
