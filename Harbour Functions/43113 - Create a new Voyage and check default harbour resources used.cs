using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.Harbour_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Voyage_Functions.Voyage_Admin;
using MTNUtilityClasses.Classes;
using System;
using System.Drawing;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.Harbour_Functions
{
    /// --------------------------------------------------------------------------------------------------
    /// Date         Person          Reason for change
    /// ==========   ======          =================================
    /// 22/06/2022   navmp4          Initial Creation
    /// --------------------------------------------------------------------------------------------------
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43113 : MTNBase
    {

        VoyageEnquiryForm _voyageEnquiryForm;
        HarbourManagementForm _harbourManagement;
        MovementMaintenanceFromGraphicalViewForm _movementMaintenanceGraphicalForm;
        ConfirmationFormYesNo _confirmationFormYesNo;
        VoyageResourcesUsedForm _voyageResourcesUsedForm;
        HMSMovementsTableForm _hMsMovementsTable;
        VoyageDefinitionForm _voyageDefinitionForm;


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void CreateANewVoyageAndCheckDefaultHarbourResourcesUsed()
        {
            
            MTNInitialize();
            
            DateTime todayDate = DateTime.Today.Date.AddDays(-10);
            var dateTodayMinus4 = todayDate.Date.AddDays(-4).ToString("ddMMyyyy");
            var dateToday = todayDate.ToString("ddMMyyyy");
            var arrivalDate = todayDate.Date.AddDays(-2).ToString("ddMMyyyy");
            var departureDate = todayDate.Date.AddDays(-1).ToString("ddMMyyyy");
            

            // Open Voyage Enquiry and delete the voyage
            FormObjectBase.NavigationMenuSelection(@"Voyage Functions|Admin|Voyage Enquiry");
            _voyageEnquiryForm = new VoyageEnquiryForm();

            _voyageEnquiryForm.DeleteVoyageByVoyageCode(@"HMS000001");

            // Open Harbour Management form
            FormObjectBase.NavigationMenuSelection(@"Harbour Functions|Graphical View");
            _harbourManagement = new HarbourManagementForm(@"Harbour Management HMSAVON TT1");

            //harbourManagement.btnShowRangeSelector.DoClick();
            _harbourManagement.DoToolbarClick(_harbourManagement.MainToolbar,    
                (int)HarbourManagementForm.Toolbar.MainToolbar.ShowRangeSelector, 
                HarbourManagementForm.Toolbar.MainToolbar.ShowRangeSelector.ToName());
            _harbourManagement.GetSearcherRangesTab();

            MTNControlBase.SetValueInEditTable(_harbourManagement.tblSearcher, @"From Date", dateTodayMinus4);
            MTNControlBase.SetValueInEditTable(_harbourManagement.tblSearcher, @"To Date", dateToday);

            //harbourManagement.btnSearchRange.DoClick();
            _harbourManagement.DoToolbarClick(_harbourManagement.SearchToolbar,
                (int)HarbourManagementForm.Toolbar.SearcherToolbar.SearchRange, 
                HarbourManagementForm.Toolbar.SearcherToolbar.SearchRange.ToName());

            Point clickPoint = new Point(_harbourManagement.grpBerth.BoundingRectangle.X + 90,
            _harbourManagement.grpBerth.BoundingRectangle.Y + 100);
            Mouse.RightClick(clickPoint);

            _harbourManagement.ContextMenuSelect(@"Select Wharves...");
            _harbourManagement.ShowWharves();
            _harbourManagement.listSelect.BtnAllLeft.DoClick();
            _harbourManagement.listSelect.MoveItemsBetweenList(_harbourManagement.listSelect.LstLeft, new [] { "Brunt Quay" });
            _harbourManagement.btnShowAreas.DoClick();

            //harbourManagement.btnNew.DoClick();
            _harbourManagement.DoToolbarClick(_harbourManagement.MainToolbar,    
                (int)HarbourManagementForm.Toolbar.MainToolbar.AddNewActualVoyage, 
                HarbourManagementForm.Toolbar.MainToolbar.AddNewActualVoyage.ToName());

            _voyageDefinitionForm = new VoyageDefinitionForm();
            _voyageDefinitionForm.cmbVessel.SetValue(@"HMS TESTING VESSEL - (HMS0001)");
            _voyageDefinitionForm.txtVoyageCode.SetValue(@"HMS000001");
            _voyageDefinitionForm.cmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _voyageDefinitionForm.TxtArrivalDate.SetValue(arrivalDate);
            _voyageDefinitionForm.TxtArrivalTime.SetValue(@"10:00");
            _voyageDefinitionForm.TxtDepartureDate.SetValue(departureDate);
            _voyageDefinitionForm.TxtDepartureTime.SetValue(@"12:00");

            _voyageDefinitionForm.BerthageDefaultsTab();
            _voyageDefinitionForm.cmbToOtherBerth.SetValue(@"Brunt Quay (BQ)");
            //voyageDefinitionForm.btnSaveVoyage.DoClick();
            _voyageDefinitionForm.DoSave();
            // voyageDefinitionForm.btnVoyageResourcesUsed.DoClick();
            _voyageDefinitionForm.DoOpenVoyageResourcesUsed();
            _voyageResourcesUsedForm = new VoyageResourcesUsedForm(@"HMS000001 HMS TESTING VESSEL - Voyage Resources Used");
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Pilot for test 43113~Resource^Pilotage In / Out~Units Used^1~GenInvNow^0", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Port Access~Resource^Port Access~Units Used^1~GenInvNow^0", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Ship lines for test 43113~Resource^Ship Lines In / Out~Units Used^3~GenInvNow^0", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Tug for test 43113~Resource^Tugs Arrival / Departure~Units Used^~GenInvNow^0", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Berthage for test 43113~Resource^Berthage 24 hrs for 43113~Units Used^1~GenInvNow^0", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Pilot for test 43113~Resource^Pilotage In / Out~Units Used^1~GenInvNow^0", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Ship lines for test 43113~Resource^Ship Lines In / Out~Units Used^3~GenInvNow^0", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Tug for test 43113~Resource^Tugs Arrival / Departure~Units Used^~GenInvNow^0", ClickType.Click);
            _voyageResourcesUsedForm.TblResourcesUsed.FindClickRow([
                "Resource Type^Pilot for test 43113~Resource^Pilotage In / Out~Units Used^1~GenInvNow^0",
                "Resource Type^Port Access~Resource^Port Access~Units Used^1~GenInvNow^0",
                "Resource Type^Ship lines for test 43113~Resource^Ship Lines In / Out~Units Used^3~GenInvNow^0",
                "Resource Type^Tug for test 43113~Resource^Tugs Arrival / Departure~Units Used^~GenInvNow^0",
                "Resource Type^Berthage for test 43113~Resource^Berthage 24 hrs for 43113~Units Used^1~GenInvNow^0",
                "Resource Type^Pilot for test 43113~Resource^Pilotage In / Out~Units Used^1~GenInvNow^0",
                "Resource Type^Ship lines for test 43113~Resource^Ship Lines In / Out~Units Used^3~GenInvNow^0",
                "Resource Type^Tug for test 43113~Resource^Tugs Arrival / Departure~Units Used^~GenInvNow^0"
            ], ClickType.Click);

            _voyageResourcesUsedForm.CloseForm();
            _voyageDefinitionForm.CloseForm();

            clickPoint = new Point(_harbourManagement.grpBerth.BoundingRectangle.X + 50,
            _harbourManagement.grpBerth.BoundingRectangle.Y + 80);
            Mouse.RightClick(clickPoint);

            _harbourManagement.ContextMenuSelect(@"Movements Table...");

            _hMsMovementsTable = new HMSMovementsTableForm(@"HMS Movements Table TT1");
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_hMsMovementsTable.tblMovements, @"Move^Arrival", ClickType.DoubleClick);
            _hMsMovementsTable.TblMovements.FindClickRow(new[] { "Move^Arrival" }, ClickType.DoubleClick);

            // Which Opens Movement Maintenance form
            _movementMaintenanceGraphicalForm = new MovementMaintenanceFromGraphicalViewForm(@"Movement Maintenance TT1");
            // Go to Marine Services tab
            _movementMaintenanceGraphicalForm.MarineServicesTab();

            // Add Pilots from the Pilot and Pilot Launch dropdown
            // Note: Here we have used slightly a different method to select the items from the dropdown as this table is using vertical headers which doesn't have a method yet.
            // we are considering to write a new method in the future
            AddPilot(@"PILOT1 (PILOT1)", @"");

            // Click the checkboxes of the Tugs required in this case check "Huria Matenga 2 (HM2) nad Toia (TOI)
            CheckTugsRequired();

            _movementMaintenanceGraphicalForm.DoApplyAndClose();
            
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm 'ACTUAL' Time");
            _confirmationFormYesNo.btnYes.DoClick();

            _hMsMovementsTable.SetFocusToForm();

            // Double click on the Departure movement
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_hMsMovementsTable.tblMovements, @"Move^Departure", ClickType.DoubleClick, rowHeight: 14);
            _hMsMovementsTable.TblMovements.FindClickRow(new[] { "Move^Departure" }, ClickType.DoubleClick);

            // Which Opens Movement Maintenance form
            _movementMaintenanceGraphicalForm = new MovementMaintenanceFromGraphicalViewForm(@"Movement Maintenance TT1");
            // Go to Marine Services tab
            _movementMaintenanceGraphicalForm.MarineServicesTab();

            // Add Pilots from the Pilot and Pilot Launch dropdown
            // Note: Here we have used slightly a different method to select the items from the dropdown as this table is using vertical headers which doesn't have a method yet.
            // we are considering to write a new method in the future
            AddPilot(@"PILOT2 for test 43113 (PILOT2)", @"");

            // Click the checkboxes of the Tugs required in this case check "Huria Matenga 2 (HM2) nad Toia (TOI)
            CheckTugsRequired();

            //movementMaintenanceGraphicalForm.btnApplyandClose.DoClick();
            //movementMaintenanceGraphicalForm.DoToolbarClick(movementMaintenanceGraphicalForm.MainToolbar,
            //    (int)MovementMaintenanceFromGraphicalViewForm.Toolbar.MainToolbar.ApplyCloseForm, 
            //    "Apply and Close");
            _movementMaintenanceGraphicalForm.DoApplyAndClose();
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Confirm 'ACTUAL' Time");
            _confirmationFormYesNo.btnYes.DoClick();

            _hMsMovementsTable.CloseForm();

            _harbourManagement.SetFocusToForm();
            //harbourManagement.btnSave.DoClick();
            _harbourManagement.DoSave();

            clickPoint = new Point(_harbourManagement.grpBerth.BoundingRectangle.X + 50,
                _harbourManagement.grpBerth.BoundingRectangle.Y + 80);
            Mouse.Click(clickPoint);

            //harbourManagement.btnVoyageResourcesUsed.DoClick();
            _harbourManagement.DoVoyageResourcesUsed();
            _voyageResourcesUsedForm = new VoyageResourcesUsedForm(@"HMS000001 HMS TESTING VESSEL - Voyage Resources Used");

            _voyageResourcesUsedForm.btnGenerateHarbourInvoices.DoClick();
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Harbour Invoice Generation");
            _confirmationFormYesNo.btnYes.DoClick();
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Pilot for test 43113~Resource^Pilotage In / Out~Units Used^1~GenInvNow^1", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Port Access~Resource^Port Access~Units Used^1~GenInvNow^1", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Ship lines for test 43113~Resource^Ship Lines In / Out~Units Used^3~GenInvNow^1", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Tug for test 43113~Resource^Tugs Arrival / Departure~Units Used^~GenInvNow^1", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Berthage for test 43113~Resource^Berthage 24 hrs for 43113~Units Used^1~GenInvNow^1", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Pilot for test 43113~Resource^Pilotage In / Out~Units Used^1~GenInvNow^1", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Ship lines for test 43113~Resource^Ship Lines In / Out~Units Used^3~GenInvNow^1", ClickType.Click);
            // MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Tug for test 43113~Resource^Tugs Arrival / Departure~Units Used^~GenInvNow^1", ClickType.Click);
            _voyageResourcesUsedForm.TblResourcesUsed.FindClickRow([
                "Resource Type^Pilot for test 43113~Resource^Pilotage In / Out~Units Used^1~GenInvNow^1",
                "Resource Type^Port Access~Resource^Port Access~Units Used^1~GenInvNow^1",
                "Resource Type^Ship lines for test 43113~Resource^Ship Lines In / Out~Units Used^3~GenInvNow^1",
                "Resource Type^Tug for test 43113~Resource^Tugs Arrival / Departure~Units Used^~GenInvNow^1",
                "Resource Type^Berthage for test 43113~Resource^Berthage 24 hrs for 43113~Units Used^1~GenInvNow^1",
                "Resource Type^Pilot for test 43113~Resource^Pilotage In / Out~Units Used^1~GenInvNow^1",
                "Resource Type^Ship lines for test 43113~Resource^Ship Lines In / Out~Units Used^3~GenInvNow^1",
                "Resource Type^Tug for test 43113~Resource^Tugs Arrival / Departure~Units Used^~GenInvNow^1"
            ]);
           _voyageResourcesUsedForm.CloseForm();


            clickPoint = new Point(_harbourManagement.grpBerth.BoundingRectangle.X + 50,
               _harbourManagement.grpBerth.BoundingRectangle.Y + 80);
            Mouse.RightClick(clickPoint);

            _harbourManagement.ContextMenuSelect(@"Movements Table...");

            _hMsMovementsTable = new HMSMovementsTableForm(@"HMS Movements Table TT1");
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_hMsMovementsTable.tblMovements, @"Move^Departure", ClickType.DoubleClick, rowHeight: 14);
            _hMsMovementsTable.TblMovements.FindClickRow(new[] { "Move^Departure" }, ClickType.DoubleClick);

            // Which Opens Movement Maintenance form
            _movementMaintenanceGraphicalForm = new MovementMaintenanceFromGraphicalViewForm(@"Movement Maintenance TT1");
            // Go to Marine Services tab
            _movementMaintenanceGraphicalForm.MarineServicesTab();

            clickPoint = new Point(_movementMaintenanceGraphicalForm.tblPilot.BoundingRectangle.X + 1,
               _movementMaintenanceGraphicalForm.tblPilot.BoundingRectangle.Y + 20);
            Mouse.Click(clickPoint);

            clickPoint = new Point(_movementMaintenanceGraphicalForm.tblPilot.BoundingRectangle.X + 1,
             _movementMaintenanceGraphicalForm.tblPilot.BoundingRectangle.Y + 1);
            Mouse.RightClick(clickPoint);

            _movementMaintenanceGraphicalForm.ContextMenuSelect(@"Delete Pilot");
            // Add Pilots from the Pilot and Pilot Launch dropdown
            // Note: Here we have used slightly a different method to select the items from the dropdown as this table is using vertical headers which doesn't have a method yet.
            // we are considering to write a new method in the future
            AddPilot(@"PILOT3 for test 43113 (PILOT3)", @"");

            _movementMaintenanceGraphicalForm.ResourcesUsedTab();

            /*clickPoint = new Point(_movementMaintenanceGraphicalForm.tblResources.BoundingRectangle.X + 5,
              _movementMaintenanceGraphicalForm.tblResources.BoundingRectangle.Y + 100);*/
            clickPoint = new Point(
                _movementMaintenanceGraphicalForm.tblResources.BoundingRectangle.Left +
                (_movementMaintenanceGraphicalForm.tblResources.BoundingRectangle.Width / 2),
                _movementMaintenanceGraphicalForm.tblResources.BoundingRectangle.Top +
                (_movementMaintenanceGraphicalForm.tblResources.BoundingRectangle.Height / 2));
            Mouse.RightClick(clickPoint);

            _movementMaintenanceGraphicalForm.ContextMenuSelect(@"Add Resource");
            AddResources();         

            //movementMaintenanceGraphicalForm.btnApplyandClose.DoClick();
            //movementMaintenanceGraphicalForm.DoToolbarClick(movementMaintenanceGraphicalForm.MainToolbar,
            //    (int)MovementMaintenanceFromGraphicalViewForm.Toolbar.MainToolbar.ApplyCloseForm, 
             //   "Apply and Close");
             _movementMaintenanceGraphicalForm.DoApplyAndClose();
            _hMsMovementsTable.CloseForm();

            //harbourManagement.btnSave.DoClick();
            _harbourManagement.DoSave();


            clickPoint = new Point(_harbourManagement.grpBerth.BoundingRectangle.X + 50,
                _harbourManagement.grpBerth.BoundingRectangle.Y + 80);
            Mouse.Click(clickPoint);

            //harbourManagement.btnVoyageResourcesUsed.DoClick();
            _harbourManagement.DoVoyageResourcesUsed();
            _voyageResourcesUsedForm = new VoyageResourcesUsedForm(@"HMS000001 HMS TESTING VESSEL - Voyage Resources Used");
            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Power Adapter~Resource^Power Adapter~Units Used^1~GenInvNow^0", ClickType.Click);
            _voyageResourcesUsedForm.TblResourcesUsed.FindClickRow(new[] { "Resource Type^Power Adapter~Resource^Power Adapter~Units Used^1~GenInvNow^0" });
            _voyageResourcesUsedForm.btnGenerateHarbourInvoices.DoClick();
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Harbour Invoice Generation");
            _confirmationFormYesNo.btnYes.DoClick();

            // Monday, 24 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_voyageResourcesUsedForm.tblResourcesUsed, @"Resource Type^Power Adapter~Resource^Power Adapter~Units Used^1~GenInvNow^1", ClickType.Click);
            _voyageResourcesUsedForm.TblResourcesUsed.FindClickRow(new[]
                { "Resource Type^Power Adapter~Resource^Power Adapter~Units Used^1~GenInvNow^1" });

            _voyageResourcesUsedForm.CloseForm();


        }

        /// <summary>
        /// Add Tugs Required on the Movement Maintenance form
        /// </summary>       
        public void CheckTugsRequired()
        {

            Point point = new Point(_movementMaintenanceGraphicalForm.tblTugs.BoundingRectangle.X + 1,
                _movementMaintenanceGraphicalForm.tblTugs.BoundingRectangle.Y + 1);
            Mouse.Click(point);

            // click each checkbox
            for (int index = 0; index < -1; index++)
            {
                Keyboard.Press(VirtualKeyShort.DOWN);
                Keyboard.Press(VirtualKeyShort.SPACE);
                Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(100));
            }

            Keyboard.Press(VirtualKeyShort.TAB);

        }
        /// <summary>
        /// Add Pilots on the Movement Maintenance form
        /// </summary>
        /// <param name="pilot"></param>
        /// <param name="pilotlaunch"></param>
        public void AddPilot(string pilot, string pilotlaunch)
        {
            Point point = new Point(_movementMaintenanceGraphicalForm.tblPilot.BoundingRectangle.X + 1,
                _movementMaintenanceGraphicalForm.tblPilot.BoundingRectangle.Y + 1);
            Mouse.RightClick(point);

            _movementMaintenanceGraphicalForm.ContextMenuSelect(@"Add Pilot");

            point = new Point(_movementMaintenanceGraphicalForm.tblPilot.BoundingRectangle.X + 5,
               _movementMaintenanceGraphicalForm.tblPilot.BoundingRectangle.Y + 15);
            Mouse.Click(point);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
            Keyboard.Type(pilot);
            Keyboard.Press(VirtualKeyShort.TAB);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
            Keyboard.Type(pilotlaunch);

        }
        /// <summary>
        /// Add Resources on the Movement Maintenance form
        /// </summary>
        /// <param name="resourcetype"></param>
        /// <param name="resource"></param>
        /// <param name="units"></param>
        public void AddResources()
        {
            DateTime date = DateTime.Today.Date;
            date = date.AddDays(-1);
            
            Point clickPoint = new Point(_movementMaintenanceGraphicalForm.tblResources.BoundingRectangle.X + 5,
             _movementMaintenanceGraphicalForm.tblResources.BoundingRectangle.Y + 15);
            Mouse.Click(clickPoint);

            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(3));
            Keyboard.Type(@"Power Adapter (POWERAD)");
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(2));
            Keyboard.Press(VirtualKeyShort.TAB);

            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(5));
            Keyboard.Type(@"Power Adapter (POWERAD)");
            Wait.UntilInputIsProcessed(TimeSpan.FromSeconds(2));
            Keyboard.Press(VirtualKeyShort.TAB);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
            Keyboard.Type(@"1");
            Keyboard.Press(VirtualKeyShort.TAB);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
            Keyboard.Type(date.ToString(@"ddMMyyyy"));
            Keyboard.Press(VirtualKeyShort.TAB);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(3000));
            Keyboard.Type(date.ToString(@"hhmm"));
            Keyboard.Press(VirtualKeyShort.TAB);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(2000));
            Keyboard.Type(date.ToString(@"ddMMyyyy"));
            Keyboard.Press(VirtualKeyShort.TAB);

            Wait.UntilInputIsProcessed(TimeSpan.FromMilliseconds(3000));
            Keyboard.Type(date.AddMinutes(30).ToString(@"hhmm"));
        }
    }

}
