using DataObjects.LogInOutBO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNForms.FormObjects.General_Functions.Cargo_Enquiry;
using MTNUtilityClasses.Classes;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNForms.Controls;
using MTNForms.FormObjects.General_Functions;
using MTNGlobal;
using MTNGlobal.EnumsStructs;

namespace MTNAutomationTests.TestCases.Master_Terminal.General
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase64832 : MTNBase
    {
        CallAPI callAPI;
        SplitCargoForm splitCargoForm;
        MergeCargoForm mergeCargoForm;
        string dateTime;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            SetupAndLoadInitializeData(TestContext);
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void CheckStorageExpiryDetailsAfterSplittingMergingCargo()
        {
            MTNInitialize();

            // search to find cargo
            FormObjectBase.MainForm.OpenCargoEnquiryFromToolbar();
            cargoEnquiryForm = new CargoEnquiryForm();
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo Type", "Bottles of Beer", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Cargo ID", "MTN64832A1");
            MTNControlBase.SetValueInEditTable(cargoEnquiryForm.tblSearchCriteria, "Site (Current)", "On Site", EditRowDataType.ComboBoxEdit, fromCargoEnquiry: true);
            cargoEnquiryForm.DoSearch();

            cargoEnquiryForm.CargoEnquiryReceiveRelease();
            dateTime = MTNControlBase.GetValueInEditTable(cargoEnquiryForm.tblReceiveRelease, @"Received Date/Time");
            
            // check storage expiry details
            VerifyStorageExpiryDetails();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^MTN64832A1", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^MTN64832A1"], clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Split...");

            splitCargoForm = new SplitCargoForm("Split Cargo TT1");
            
            splitCargoForm.rdoCreatePieceBreakBulkItems.DoClick();
            splitCargoForm.txtQuantity.SetValue("3");
            splitCargoForm.txtNewId.SetValue("MTN64832B");
            splitCargoForm.txtFirstIdSuffix.SetValue("1");

            splitCargoForm.btnOK.DoClick();

            // merge cargo
            string parentCargo = "MTN64832A1";
            string[] cargoToMerge = new string[] { "Select Cargo to Merge into MTN64832A1^MTN64832B1", "Select Cargo to Merge into MTN64832A1^MTN64832B2", "Select Cargo to Merge into MTN64832A1^MTN64832B3" };
            cargoEnquiryForm.MergeCargo(parentCargo, cargoToMerge);

         /*   cargoEnquiryForm.ContextMenuSelect(@"Cargo|Merge...");

            mergeCargoForm = new MergeCargoForm("Merge Cargo TT1");
            mergeCargoForm.tblCargoItems.FindClickRow(new[] { "" });
            mergeCargoForm.btnOK.DoClick();

            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Merge...");

            mergeCargoForm = new MergeCargoForm("Merge Cargo TT1");
            mergeCargoForm.tblCargoItems.FindClickRow(new[] { "MTN64832B2" });
            mergeCargoForm.btnOK.DoClick();

            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Merge...");

            mergeCargoForm = new MergeCargoForm("Merge Cargo TT1");
            mergeCargoForm.tblCargoItems.FindClickRow(new[] { "MTN64832B3" });
            mergeCargoForm.btnOK.DoClick();*/

            // check storage expiry details
            VerifyStorageExpiryDetails();
            // MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^MTN64832A1", clickType: ClickType.ContextClick);
            cargoEnquiryForm.tblData2.FindClickRow(["ID^MTN64832A1"], clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Split...");

            splitCargoForm = new SplitCargoForm("Split Cargo TT1");

            splitCargoForm.rdoCreatePieceBreakBulkItems.DoClick();
            splitCargoForm.txtQuantity.SetValue("3");
            splitCargoForm.txtNewId.SetValue("MTN64832B");
            splitCargoForm.txtFirstIdSuffix.SetValue("1");

            splitCargoForm.btnOK.DoClick();

            // merge cargo  
            cargoEnquiryForm.MergeCargo(parentCargo, cargoToMerge);

/*
            MTNControlBase.FindClickRowInTable(cargoEnquiryForm.tblData, @"ID^MTN64832A1", clickType: ClickType.ContextClick);
            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Merge...");

            mergeCargoForm = new MergeCargoForm("Merge Cargo TT1");
            mergeCargoForm.tblCargoItems.FindClickRow(new[] { "MTN64832B1" });
            mergeCargoForm.btnOK.DoClick();

            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Merge...");

            mergeCargoForm = new MergeCargoForm("Merge Cargo TT1");
            mergeCargoForm.tblCargoItems.FindClickRow(new[] { "MTN64832B2" });
            mergeCargoForm.btnOK.DoClick();

            cargoEnquiryForm.ContextMenuSelect(@"Cargo|Merge...");

            mergeCargoForm = new MergeCargoForm("Merge Cargo TT1");
            mergeCargoForm.tblCargoItems.FindClickRow(new[] { "MTN64832B3" });
            mergeCargoForm.btnOK.DoClick();*/

            // check storage expiry details
            VerifyStorageExpiryDetails();
        }
        private void SetupAndLoadInitializeData(TestContext testContext)
        {
            callAPI = new CallAPI();
            string baseURL = TestContext.GetRunSettingValue(@"BaseUrl");
            string dataToDelete = "<AllCargoOnSite>\r\n<Cargo>\r\n<ID>MTN64832A1</ID>\r\n<CargoTypeDescr>Bottles of Beer</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>MSCK000010</Voyage>\r\n<Operator>MSC</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZAKL</DischargePort>\r\n<Location>MKBS01</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>MTN64832B1</ID>\r\n<CargoTypeDescr>Bottles of Beer</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>MSCK000010</Voyage>\r\n<Operator>MSC</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZAKL</DischargePort>\r\n<Location>MKBS01</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>MTN64832B2</ID>\r\n<CargoTypeDescr>Bottles of Beer</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>MSCK000010</Voyage>\r\n<Operator>MSC</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZAKL</DischargePort>\r\n<Location>MKBS01</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n<Cargo>\r\n<ID>MTN64832B3</ID>\r\n<CargoTypeDescr>Bottles of Beer</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>MSCK000010</Voyage>\r\n<Operator>MSC</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZAKL</DischargePort>\r\n<Location>MKBS01</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>D</Action>\r\n</Cargo>\r\n</AllCargoOnSite>";
            string dataToCreate = "<AllCargoOnSite>\r\n<Cargo>\r\n<ID>MTN64832A1</ID>\r\n<CargoTypeDescr>Bottles of Beer</CargoTypeDescr>\r\n<IMEXStatus>Import</IMEXStatus>\r\n<Voyage>MSCK000010</Voyage>\r\n<Operator>MSC</Operator>\r\n<TotalWeight>5101.0000</TotalWeight>\r\n<DischargePort>NZAKL</DischargePort>\r\n<Location>MKBS01</Location>\r\n<LoadPort>NZCHC</LoadPort>\r\n<Action>A</Action>\r\n<TotalQuantity>20</TotalQuantity>\r\n</Cargo>\r\n</AllCargoOnSite>";
            callAPI.AddDeleteData(dataToDelete, "CargoOnSiteXML", new GetSetArgumentsToCallAPI
            {
                RequestURL = baseURL + "SendEDI?MasterTerminalAPI",
                Authorization = "Bearer"
            });
            callAPI.AddDeleteData(dataToCreate, "CargoOnSiteXML", new GetSetArgumentsToCallAPI
            {
                RequestURL = baseURL + "SendEDI?MasterTerminalAPI",
                Authorization = "Bearer"
            });
        }

        public void VerifyStorageExpiryDetails()
        {
            cargoEnquiryForm.SetFocusToForm();
            cargoEnquiryForm.CargoEnquiryStorageExpiryDetailsTab();
            cargoEnquiryForm.tblStorageExpiryDetails.FindClickRow(new[]
                { "Date Time^" + dateTime + "~Free Days Assigned^0" });
        }

    }
}
