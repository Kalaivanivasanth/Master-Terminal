using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Gate_Functions;
using MTNForms.FormObjects.Message_Dialogs;
using MTNForms.FormObjects.Misc;
using System;
using DataObjects.LogInOutBO;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40538 : MTNBase
    {

        PreNoteForm _preNoteForm;
        EDIOperationsForm _ediOperationsForm;
        ConfirmationFormYesNo _confirmationFormYesNo;

        const  string EDIFile1 = "40538_EDI_KiwiRail1.xml";
        const  string EDIFile2 = "40538_EDI_KiwiRail2.xml";
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}
     

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize()
        {
            CreateDataFile(EDIFile1,
               "<message xmlns:tns='http://www.kiwirail.co.nz/GateInOutSchema_v_2_0'>\n    <header>\n        <messageID>GATEOUT20180308163019892</messageID>\n        <timestamp>2018-03-08T16:30:19</timestamp>\n        <senderID>KIWIRAIL</senderID>\n        <receiverID>TP1</receiverID>\n    </header>\n    <gateEvent gateType='GATEOUT'>\n        <booker>\n            <name>SOUTH PORT NO. 3 TRUST</name>\n            <address1>OPEN COUNTRY DAIRY</address1>\n            <address2>ISLAND HARBOUR</address2>\n            <phone></phone>\n            <fax></fax>\n            <email>OCDADMIN@SOUTHPORT.CO.NZ</email>\n            <suburb></suburb>\n            <location>\n                <location>BLUFF</location>\n                <UNLocation>NZBLU</UNLocation>\n            </location>\n        </booker>\n        <freightPayer>\n            <name>OPEN COUNTRY DAIRY LIMITED</name>\n            <address1>ACCOUNTS PAYABLE</address1>\n            <address2>P O BOX 11 159</address2>\n            <phone>(09) 589 3208</phone>\n            <fax>09 525 0347</fax>\n            <email>MALA.NOSA@OPENCOUNTRY.CO.NZ</email>\n            <suburb>ELLERSLIE</suburb>\n            <location>\n                <location>AUCKLAND</location>\n                <UNLocation>NZAKL</UNLocation>\n            </location>\n        </freightPayer>\n        <receiver>\n            <name>SOUTH PORT NO. 3 TRUST</name>\n            <address1>OPEN COUNTRY DAIRY</address1>\n            <address2>ISLAND HARBOUR</address2>\n            <phone>03-212-8159</phone>\n            <fax>03-212-8685</fax>\n            <email>SOUTHPORTSHED3@OPENCOUNTRY.CO.NZ</email>\n            <suburb></suburb>\n            <location>\n                <location>BLUFF</location>\n                <UNLocation>NZBLU</UNLocation>\n            </location>\n        </receiver>\n        <sender>\n            <name>SOUTH FREIGHT</name>\n            <address1>STRATHALLAN STREET</address1>\n            <address2></address2>\n            <phone>03 455 0305</phone>\n            <fax>03 455 0306</fax>\n            <email>CONTAINERYARD@PORTOTAGO.CO.NZ</email>\n            <suburb></suburb>\n            <location>\n                <location>DUNEDIN</location>\n                <UNLocation>NZDUD</UNLocation>\n            </location>\n        </sender>\n        <equipment equipmentType='CONTAINER'>\n            <equipmentID>JLG40538A01</equipmentID>\n            <IMEX>\n                <shipper></shipper>\n                <shipName></shipName>\n                <voyageNumber></voyageNumber>\n                <overseasDestination></overseasDestination>\n                <shippingReference>RLS40538</shippingReference>\n                <cutOffTimestamp />\n                <releaseNumber>RLS40538</releaseNumber>\n                <customsClearanceNumber></customsClearanceNumber>\n                <exportEntryNumber></exportEntryNumber>\n            </IMEX>\n            <tareWeight>2900</tareWeight>\n            <netWeight>0111</netWeight>\n            <sizeType>2210</sizeType>\n            <overGauge>false</overGauge>\n            <orderNumber></orderNumber>\n            <loaded>false</loaded>\n            <gate transitType='TRUCK'>\n                <truck>\n                    <truckID></truckID>\n                </truck>\n            </gate>\n            <service>\n                <serviceID>L937</serviceID>\n                <serviceDate>2018-03-05</serviceDate>\n                <origin>\n                    <location>DNDN</location>\n                    <UNLocation>NZDUD</UNLocation>\n                </origin>\n                <destination>\n                    <location>BLUFF</location>\n                    <UNLocation>NZBLU</UNLocation>\n                </destination>\n            </service>\n            <associatedWagon></associatedWagon>\n            <associatedGenerator></associatedGenerator>\n        </equipment>\n        <location>\n            <location></location>\n            <UNLocation></UNLocation>\n        </location>\n        <timestamp>2018-03-08T16:30:19</timestamp>\n        <commodityCode>GEN</commodityCode>\n        <comments></comments>\n        <bookingID>BOOK40538</bookingID>\n        <origin>\n            <location>DNDN</location>\n            <UNLocation>NZDUD</UNLocation>\n        </origin>\n        <destination>\n            <location>BLUFF</location>\n            <UNLocation>NZBLU</UNLocation>\n        </destination>\n        <operation>CREATE</operation>\n        <customerReference>BOOKING40538</customerReference>\n        <commodityDescription>MTY CONTAINERS</commodityDescription>\n    </gateEvent>\n</message>\n\n");

            //Create Kiwi Rail File 2 A02
            CreateDataFile(EDIFile2,
                "<message xmlns:tns='http://www.kiwirail.co.nz/GateInOutSchema_v_2_0'>\n    <header>\n        <messageID>GATEOUT20180308163019892</messageID>\n        <timestamp>2018-03-08T16:30:19</timestamp>\n        <senderID>KIWIRAIL</senderID>\n        <receiverID>TP1</receiverID>\n    </header>\n    <gateEvent gateType='GATEOUT'>\n        <booker>\n            <name>SOUTH PORT NO. 3 TRUST</name>\n            <address1>OPEN COUNTRY DAIRY</address1>\n            <address2>ISLAND HARBOUR</address2>\n            <phone></phone>\n            <fax></fax>\n            <email>OCDADMIN@SOUTHPORT.CO.NZ</email>\n            <suburb></suburb>\n            <location>\n                <location>BLUFF</location>\n                <UNLocation>NZBLU</UNLocation>\n            </location>\n        </booker>\n        <freightPayer>\n            <name>OPEN COUNTRY DAIRY LIMITED</name>\n            <address1>ACCOUNTS PAYABLE</address1>\n            <address2>P O BOX 11 159</address2>\n            <phone>(09) 589 3208</phone>\n            <fax>09 525 0347</fax>\n            <email>MALA.NOSA@OPENCOUNTRY.CO.NZ</email>\n            <suburb>ELLERSLIE</suburb>\n            <location>\n                <location>AUCKLAND</location>\n                <UNLocation>NZAKL</UNLocation>\n            </location>\n        </freightPayer>\n        <receiver>\n            <name>SOUTH PORT NO. 3 TRUST</name>\n            <address1>OPEN COUNTRY DAIRY</address1>\n            <address2>ISLAND HARBOUR</address2>\n            <phone>03-212-8159</phone>\n            <fax>03-212-8685</fax>\n            <email>SOUTHPORTSHED3@OPENCOUNTRY.CO.NZ</email>\n            <suburb></suburb>\n            <location>\n                <location>BLUFF</location>\n                <UNLocation>NZBLU</UNLocation>\n            </location>\n        </receiver>\n        <sender>\n            <name>SOUTH FREIGHT</name>\n            <address1>STRATHALLAN STREET</address1>\n            <address2></address2>\n            <phone>03 455 0305</phone>\n            <fax>03 455 0306</fax>\n            <email>CONTAINERYARD@PORTOTAGO.CO.NZ</email>\n            <suburb></suburb>\n            <location>\n                <location>DUNEDIN</location>\n                <UNLocation>NZDUD</UNLocation>\n            </location>\n        </sender>\n        <equipment equipmentType='CONTAINER'>\n            <equipmentID>JLG40538A02</equipmentID>\n            <IMEX>\n                <shipper>MSK</shipper>\n                <shipName></shipName>\n                <voyageNumber></voyageNumber>\n                <overseasDestination></overseasDestination>\n                <shippingReference>RLS40538A</shippingReference>\n                <cutOffTimestamp />\n                <releaseNumber>RLS40538A</releaseNumber>\n                <customsClearanceNumber></customsClearanceNumber>\n                <exportEntryNumber></exportEntryNumber>\n            </IMEX>\n            <tareWeight>2900</tareWeight>\n            <netWeight>0111</netWeight>\n            <sizeType>2210</sizeType>\n            <overGauge>false</overGauge>\n            <orderNumber></orderNumber>\n            <loaded>false</loaded>\n            <gate transitType='TRUCK'>\n                <truck>\n                    <truckID></truckID>\n                </truck>\n            </gate>\n            <service>\n                <serviceID>L937</serviceID>\n                <serviceDate>2018-03-05</serviceDate>\n                <origin>\n                    <location>DNDN</location>\n                    <UNLocation>NZDUD</UNLocation>\n                </origin>\n                <destination>\n                    <location>BLUFF</location>\n                    <UNLocation>NZBLU</UNLocation>\n                </destination>\n            </service>\n            <associatedWagon></associatedWagon>\n            <associatedGenerator></associatedGenerator>\n        </equipment>\n        <location>\n            <location></location>\n            <UNLocation></UNLocation>\n        </location>\n        <timestamp>2018-03-08T16:30:19</timestamp>\n        <commodityCode>GEN</commodityCode>\n        <comments></comments>\n        <bookingID>BOOK40538A</bookingID>\n        <origin>\n            <location>DNDN</location>\n            <UNLocation>NZDUD</UNLocation>\n        </origin>\n        <destination>\n            <location>BLUFF</location>\n            <UNLocation>NZBLU</UNLocation>\n        </destination>\n        <operation>CREATE</operation>\n        <customerReference>BOOKING40538A</customerReference>\n        <commodityDescription>MTY CONTAINERS</commodityDescription>\n    </gateEvent>\n</message>\n\n\n\n");

            // Monday, 27 January 2025 navmh5  MTNSignon(TestContext);
            LogInto<MTNLogInOutBO>();
        }


        [TestMethod]
        public void LoadKiwiRailPreNote()
        {
            MTNInitialize();
            
           // 1. Open pre-Notes form, find existing pre-notes and delete if any found
           // also need to keep the form open for later.
           // Monday, 27 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"Gate Functions|Pre-Notes");
           FormObjectBase.MainForm.OpenPreNotesFromToolbar();
           _preNoteForm = new PreNoteForm(formTitle: @"Pre-Notes TT1");
           _preNoteForm.DeletePreNotes(@"JLG40538A", @"ID^JLG40538A0");


            // 2. Load the Kiwi rail file 1 via EDI Operations
            // Monday, 27 January 2025 navmh5 FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations", forceReset: true);
            FormObjectBase.MainForm.OpenEDIOperationsFromToolbar();
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            _ediOperationsForm.DeleteEDIMessages(@"Prenote",@"40538",@"Loaded");

            _ediOperationsForm.LoadEDIMessageFromFile(EDIFile1);
            _ediOperationsForm.ChangeEDIStatus(EDIFile1, @"Loaded", @"Verify");
            _ediOperationsForm.ChangeEDIStatus(EDIFile1, @"Verify warnings", @"Load To DB");
            _ediOperationsForm.ChangeEDIStatus(EDIFile1, @"DB Loaded", @"Delete");

            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Deleting EDI Data");
            _confirmationFormYesNo.btnYes.DoClick();
            
            // 3. Load the Kiwi rail file 2 via EDI Operations
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");
            _ediOperationsForm.LoadEDIMessageFromFile(EDIFile2);
            _ediOperationsForm.ChangeEDIStatus(EDIFile2, @"Loaded", @"Verify");
            _ediOperationsForm.ChangeEDIStatus(EDIFile2, @"Verify warnings", @"Load To DB");
            _ediOperationsForm.ChangeEDIStatus(EDIFile2, @"DB Loaded", @"Delete");
            _confirmationFormYesNo = new ConfirmationFormYesNo(@"Deleting EDI Data");
            _confirmationFormYesNo.btnYes.DoClick();
            _ediOperationsForm.CloseForm();

            // 3. Check prenotes have been added. 
            _preNoteForm.SetFocusToForm();
            // MTNControlBase.FindClickRowInTable(_preNoteForm.tblPreNotes, "ID^JLG40538A01~Transport Mode^Rail~Booking^BOOKING40538~Operator^MSC~IMEX Status^Storage", rowHeight: 18);
            // MTNControlBase.FindClickRowInTable(_preNoteForm.tblPreNotes, "ID^JLG40538A02~Transport Mode^Rail~Booking^BOOKING40538A~Operator^MSK~IMEX Status^Storage", rowHeight: 18);
            _preNoteForm.TblPreNotes.FindClickRow([
                "ID^JLG40538A01~Transport Mode^Rail~Booking^BOOKING40538~Operator^MSC~IMEX Status^Storage",
                "ID^JLG40538A02~Transport Mode^Rail~Booking^BOOKING40538A~Operator^MSK~IMEX Status^Storage"
            ]);        

        }




    }

}
