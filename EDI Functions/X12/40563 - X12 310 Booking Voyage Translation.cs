using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects.EDI_Functions;
using MTNForms.FormObjects.Misc;
using MTNUtilityClasses.Classes;
using System;
using MTNForms.FormObjects;

namespace MTNAutomationTests.TestCases.Master_Terminal.EDI_Functions.X12
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase40563 : MTNBase
    {

        EDIOperationsForm _ediOperationsForm;

        const string EDIFile1 = "M_40563_X12_301.edi";
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            searchFor = @"M_40563_";
        }

        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize()
        {
            //Create CUSCAR EDI file
            CreateDataFile(EDIFile1,
                "ISA*00*          *00*          *ZZ*SMLU           *ZZ*USCHT          *180622*1555*U*00401*000033421*0*P*>\r\nGS*RO*SMLU*USCHT*20180622*155501*33421*X*004010\r\nST*301*0001\r\nB1*MSL*5305389A*20180622*U\r\nG61*IC*Joseph Hardisty*TE*4107636043\r\nY3*5305389A*SMLU*20180702*20180709***20180630*100000**DD\r\nY4*5305389A*5305389A***1*L5G1\r\nN9*SI*5305389A\nN9*PO*5305389A\r\nN9*BN*NJ5305389A\r\nN1*CN*OSCAR UMANA*25*867280\r\nN4*LA LIBERTAD, EL SALVADOR**     \r\nN1*SH*LKQ HEAVY TRUCK-MARYLAND*25*889815\r\nN4*EASTON*MD*21601\r\nR4*R*UN*USESN*EASTON*US***MD\r\nR4*E*UN*SVSAL*SAN SALVADOR*SV\r\nR4*L*UN*USPHL*PHILADELPHIA*US***PA\r\nR4*D*UN*DOHAI*RIO HAINA*DO\r\nLX*1\r\nN7*MSL*849993*61800.00*G****3420.00*E********L*2*DD***L5G1\r\nL0*1***61800.000*G*3420.000*E*1*CNT**L*DD\r\nL5*1*USED TIRES\r\nV1*7616353*JOLLY GRIGIO*LR*005*MSL***L\r\nV9*ACC*ACCEPTED*20180622*155422\r\nSE*23*0001\r\nST*301*0002\r\nB1*MSL*5311688A*20180622*N\nG61*IC*Jorge Pozo*TE*3056409859\r\nY3*5311688A*SMLU*20180702*20180709***20180630*100000**HP\r\nY4*5311688A*5311688A***1*L5G1\r\nN9*SI*5311688A\r\nN9*PO*AG11\r\nN9*BN*NJ5311688A\r\nN1*SH*UPPER LOGISTICS INC*25*904397\r\nN4*MIAMI*FL*33182\r\nR4*R*UN*USPHL*PHILADELPHIA*US***PA\r\nR4*E*UN*GTSTC*PUERTO SANTO TOMAS DE CA*GT\r\nR4*L*UN*USPHL*PHILADELPHIA*US***PA\r\nR4*D*UN*DOHAI*RIO HAINA*DO\r\nLX*1\r\nL0*1*******1*CNT***HP\r\nL5*1*USED CLOTHING  INTERMODAL WILL ASSIGN\r\nV1*7616353*JOLLY GRIGIO*LR*005*MSL***L\r\nV9*ACC*ACCEPTED*20180622*155422\r\nSE*20*0002\r\nGE*2*33421\r\nIEA*1*000033421");

            MTNSignon(TestContext);
        }


        [TestMethod]
        public void X12301VoyageTranlsation()
        {

            MTNInitialize();
            
            /*
            To test that: when an edi file of type X12 301 Booking Confirmation is loaded, 
            EDI Data Translations use the Voyage Codes to match the correct voyage 
            and do not use the Inward Code (specified on Voyage Definition) for matching the voyage.
            */

            // 1. Open EDI Operations
            FormObjectBase.NavigationMenuSelection(@"EDI Functions|EDI Operations");
            _ediOperationsForm = new EDIOperationsForm(@"EDI Operations TT1");

            // 1b. Delete Existing EDI messages relating to this test.
            _ediOperationsForm.DeleteEDIMessages(EDIOperationsDataType.GateDocument, @"40563", EDIOperationsStatusType.Loaded);

            // 2. Load COPARN file
            _ediOperationsForm.LoadEDIMessageFromFile(EDIFile1);

            //MTNControlBase.FindClickRowInTable(ediOperationsForm.tblEDIMessages, @"Status^Loaded~File Name^" + ediFile1, rowHeight: 16, xOffset: -1);
            _ediOperationsForm.TblEDIMessages.FindClickRow(@"Status^Loaded~File Name^" + EDIFile1);
            //ediOperationsForm.tabEDIDetails.Click();
            _ediOperationsForm.ClickEDIDetailsTab();
           
            
            _ediOperationsForm.GetTabTableGeneric(@"Booking Header", @"4036");
            MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails,
                @"ID^5311688A~Operator^MSL~Voyage^GRIG001~Destination^GTSTC~Discharge Port^DOHAI~Vessel^JOLLY GRIGIO",
                clickType: ClickType.None, rowHeight: 16);
            MTNControlBase.FindClickRowInTable(_ediOperationsForm.tblEDIDetails,
                @"ID^5305389A~Operator^MSL~Voyage^GRIG001~Destination^SVSAL~Discharge Port^DOHAI~Vessel^JOLLY GRIGIO",
                clickType: ClickType.None, rowHeight: 16);

        }



    }

}
