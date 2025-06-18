using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase43938 : MTNBase
    {

        EDIOperationsForm ediOperationsForm;
        protected static string ediFile1 = "M_43938_BookingUpdate.xml";

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_43948_";
        }


        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //Create Booking update EDI file
            CreateDataFile(ediFile1,
                " <?xml version='1.0'?>\n<JMTInternalBooking>\n	<AllBookingHeader>\n		<BookingHeader>\n			<cargoTypeDescr>ISO Container</cargoTypeDescr>\n			<dischargePort>NZBLU</dischargePort>\n			<id>JLGBOOK43938</id>\n			<messageMode>U</messageMode>\n			<operatorCode>MSC</operatorCode>\n			<vesselName>MSCK</vesselName>\n			<voyageCode>GRIG001</voyageCode>\n				<AllBookingItem>\n					<BookingItem>\n						<weight>18830.0000</weight>\n						<number>1</number>\n						<cargoTypeDescr>ISO Container</cargoTypeDescr>\n						<commodity>APPL</commodity>\n						<fullOrMT>F</fullOrMT>\n						<id>JLGBOOK43938</id>\n						<isoType>2200</isoType>\n						<messageMode>U</messageMode>\n					</BookingItem>\n				</AllBookingItem>\n			</BookingHeader>\n	</AllBookingHeader>\n</JMTInternalBooking>");

            MTNSignon(TestContext);
        }

        [TestMethod]
        public void ChangingVoyageOnBookingOfPlannedCargo()
        {
            MTNInitialize();
            
            // 1. Open EDI Operations
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // 1b. Delete Existing EDI messages relating to this test.
            ediOperationsForm.DeleteEDIMessages(@"Booking Reference Multi Line", @"43938", ediStatus: @"Loaded");

            // 2. Load Booking Update
            ediOperationsForm.LoadEDIMessageFromFile(ediFile1);

            // 3. Verify the file
            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, clickType: ClickType.ContextClick, rowHeight: 16);
            ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + ediFile1, clickType: ClickType.ContextClick);
            ediOperationsForm.ContextMenuSelect(@"Verify");

            // 4. Confirm an error is thrown
            ediOperationsForm.ShowErrorWarningMessages();
            MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIErrorWarningMessages, @"ID^JLGBOOK43938~Type^Error~Property^voyage~Error Message^Code :90730. JLG43938A01 is currently planned/queued to MSCK000002. This needs to be removed before any Split/Roll.", clickType:ClickType.None, rowHeight: 16);


        }



    }

}
