using DataFiles.UserDefinedFiles;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects;
using MTNUtilityClasses.Classes;
using MTNForms.FormObjects.Yard_Functions.Cargo_Review;
using MTNForms.FormObjects.Gate_Functions.Release_Request;
using MTNForms.Controls;
using MTNGlobal.EnumsStructs;


namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions.Cargo_Review
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase64498 : MTNBase
    {
        private ReleaseRequestAddForm _releaseRequestAddForm;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() { }

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        private void MTNInitialize()
        {
            
            CallJadeScriptToRun(TestContext, "resetData_64498");
            SetupAndLoadInitializeData();
            LogInto<MTNLogInOutBO>();
        }

        [TestMethod]
        public void COOReleaseRequestCreationAndValidation()
        {
            MTNInitialize();
            
            
            //FormObjectBase.NavigationMenuSelection(@"Yard Functions|Cargo Review");
            FormObjectBase.MainForm.OpenCargoReviewFromToolbar();
            CargoReviewForm.DoSelectorQuery(new[]
            {
                new SelectorQueryArguments(CargoReviewForm.Property.Site, CargoReviewForm.Operation.EqualTo,
                    "On Site"),
                new SelectorQueryArguments("Cargo Group Id", "starts with", "CG64498A",
                    SelectorQueryArguments.LineAction.Add),
            });
            CargoReviewForm.DoSearch();
            


        }

        void SetupAndLoadInitializeData()
        {
            var setArgumentsToCallAPI = new GetSetArgumentsToCallAPI
                { RequestURL = $"{TestContext.GetRunSettingValue("BaseUrl")}SendEDI?MasterTerminalAPI", };

            CallAPI.AddDeleteData(new[] {
               new AddDeleteDataArguments { Data = new [] { 
                    new CargoGroupXML { CargoTypeDescr = CargoType.KeyMark, Operator = Operator.MSL, Action = "A", ID = "CG64498A" },
                    new CargoGroupXML { CargoTypeDescr = CargoType.KeyMark, Operator = Operator.MSL, Action = "A", ID = "CG64498B" },
                }, FileType = "CargoGroupXML", ArgumentsToCallAPIs = setArgumentsToCallAPI },
                new AddDeleteDataArguments { Data = new [] {
                    new CargoOnShipXML { ID = "COO64498A01", CargoTypeDescr = CargoType.BagOfSand, CargoSubtype = CargoSubtype.BIGSAND, IMEXStatus = IMEX.Import, Voyage = TT1.Voyage.MSCK000010, Operator = Operator.MSL, TotalWeight = "15999.0000",
                        DischargePort = Port.AKLNZ, Location = TT1.Voyage.MSCK000010, LoadPort = Port.SYDAU, Action = "A", TotalQuantity = "3500", CargoGroupId = "CG64498A" },
                    new CargoOnShipXML { ID = "COO64498A02", CargoTypeDescr = CargoType.BagOfSand, CargoSubtype = CargoSubtype.BIGSAND, IMEXStatus = IMEX.Import, Voyage = TT1.Voyage.MSCK000010, Operator = Operator.MSL, TotalWeight = "15999.0000",
                        DischargePort = Port.AKLNZ, Location = TT1.Voyage.MSCK000010, LoadPort = Port.SYDAU, Action = "A", TotalQuantity = "3500", CargoGroupId = "CG64498A" },
                    new CargoOnShipXML { ID = "COO64498A03", CargoTypeDescr = CargoType.BagOfSand, CargoSubtype = CargoSubtype.BIGSAND, IMEXStatus = IMEX.Import, Voyage = TT1.Voyage.MSCK000010, Operator = Operator.MSL, TotalWeight = "15999.0000",
                        DischargePort = Port.AKLNZ, Location = TT1.Voyage.MSCK000010, LoadPort = Port.SYDAU, Action = "A", TotalQuantity = "3500", CargoGroupId = "CG64498A" },
                    new CargoOnShipXML { ID = "COO64498B01", CargoTypeDescr = CargoType.BagOfSand, CargoSubtype = CargoSubtype.BIGSAND, IMEXStatus = IMEX.Import, Voyage = TT1.Voyage.MSCK000010, Operator = Operator.MSL, TotalWeight = "15999.0000",
                        DischargePort = Port.AKLNZ, Location = TT1.Voyage.MSCK000010, LoadPort = Port.SYDAU, Action = "A", TotalQuantity = "3500", CargoGroupId = "CG64498B" },
                    new CargoOnShipXML { ID = "COO64498B02", CargoTypeDescr = CargoType.BagOfSand, CargoSubtype = CargoSubtype.BIGSAND, IMEXStatus = IMEX.Import, Voyage = TT1.Voyage.MSCK000010, Operator = Operator.MSL, TotalWeight = "15999.0000",
                        DischargePort = Port.AKLNZ, Location = TT1.Voyage.MSCK000010, LoadPort = Port.SYDAU, Action = "A", TotalQuantity = "3500", CargoGroupId = "CG64498B" },
                    new CargoOnShipXML { ID = "COO64498B03", CargoTypeDescr = CargoType.BagOfSand, CargoSubtype = CargoSubtype.BIGSAND, IMEXStatus =IMEX.Import, Voyage = TT1.Voyage.MSCK000010, Operator = Operator.MSL, TotalWeight = "15999.0000",
                        DischargePort = Port.AKLNZ, Location = TT1.Voyage.MSCK000010, LoadPort = Port.SYDAU, Action = "A", TotalQuantity = "3500", CargoGroupId = "CG64498B" },
               }, FileType = "CargoOnShipXML", ArgumentsToCallAPIs = setArgumentsToCallAPI },
            });

        }
    }
}