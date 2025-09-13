using FlaUI.Core.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.FormObjects.Gate_Functions.Road_Gate;
using MTNForms.FormObjects.Message_Dialogs;
using System;
using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using HardcodedData.TerminalData;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;

namespace MTNAutomationTests.TestCases.Master_Terminal.Gate_Functions.Road_Gate
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase45201 : MTNBase
    {

        RoadGateDetailsReceiveForm _roadGateDetailsReceiveForm;
        AttachmentReceiveReleaseCargoForm _attachmentReceiveReleaseCargoForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);

        [TestInitialize]
        public new void TestInitialize() {}

        [TestCleanup]
        public new void TestCleanup() => base.TestCleanup();

        void MTNInitialize() => LogInto<MTNLogInOutBO>();


        [TestMethod]
        public void QuantityAndWeightTotalsOnRoadgateForm()
        {
            MTNInitialize();

            //1. Open road gate and enter truck details
            FormObjectBase.NavigationMenuSelection(@"Gate Functions|Road Gate");
            roadGateForm = new RoadGateForm(formTitle: @"Road Gate TT1", vehicleId: @"45201");
            /*roadGateForm.txtRegistration.SetValue(@"45201");
            roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            roadGateForm.cmbGate.SetValue(@"GATE");*/
            roadGateForm.SetRegoCarrierGate("45201");
            roadGateForm.btnReceiveFull.DoClick();

            //2. enter container details (note: total weight 3000lbs)
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive Full Container TT1");
            _roadGateDetailsReceiveForm.CmbIsoType.SetValue(ISOType.ISO2200, additionalWaitTimeout: 2000, doDownArrow: true);
            _roadGateDetailsReceiveForm.TxtCargoId.SetValue(@"JLG45201A01");
            _roadGateDetailsReceiveForm.CmbCommodity.SetValue(Commodity.GEN, doDownArrow: true);
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("3000");
            _roadGateDetailsReceiveForm.CmbImex.SetValue(IMEX.Export, additionalWaitTimeout: 2000, doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbDischargePort.SetValue(Port.LYTNZ, doDownArrow: true);
            //3. add child cargo via attachements with weight
            _roadGateDetailsReceiveForm.BtnAttachments.DoClick();
            _attachmentReceiveReleaseCargoForm = new AttachmentReceiveReleaseCargoForm(@"Attachment Details Container TT1");
            _attachmentReceiveReleaseCargoForm.CmbCargoType.SetValue(CargoType.Clinker, searchSubStringTo: 3, doDownArrow: true);
            _attachmentReceiveReleaseCargoForm.TxtCargoId.SetValue(@"JLG45201A02");
            _attachmentReceiveReleaseCargoForm.CmbCommodity.SetValue(Commodity.CLNK, doDownArrow: true);
            _attachmentReceiveReleaseCargoForm.MtTotalWeight.SetValueAndType("2000", "lbs");
            _attachmentReceiveReleaseCargoForm.BtnSave.DoClick();

            //4. save container details
            _roadGateDetailsReceiveForm.SetFocusToForm();
            _roadGateDetailsReceiveForm.BtnSave.DoClick();
            warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            warningErrorForm.btnSave.DoClick();

            //5. check the total weight label shows to container total weight
            roadGateForm.SetFocusToForm();
            string strLabel = MTNControlBase.GetValue(roadGateForm.lblReceiveItems);
            string strExpected = @"Receive Total - Qty=1, Wgt=3000.000 lbs";
            Assert.IsTrue(strLabel == strExpected, "Receive items label expected to be " + strExpected + " actual value is " + strLabel);

            //6. cancel visit.
            roadGateForm.CancelVehicleVisit(@"Test 45201 - 2");

            //7. add new visit
            //MTNControlBase.SetValue(roadGateForm.txtRegistration, @"45201A");
            roadGateForm.txtRegistration.SetValue(@"45201A");
            roadGateForm.cmbCarrier.SetValue(Carrier.AmericanAutoTpt);
            roadGateForm.cmbGate.SetValue(@"GATE");
            roadGateForm.btnReceiveCargo.DoClick();

            //8. receive general cargo
            _roadGateDetailsReceiveForm = new RoadGateDetailsReceiveForm(@"Receive General Cargo TT1");
            _roadGateDetailsReceiveForm.CmbCargoType.SetValue(CargoType.Metal, doDownArrow: true, searchSubStringTo: CargoType.Metal.Length - 1);
            _roadGateDetailsReceiveForm.MtTotalWeight.SetValueAndType("1000");
            _roadGateDetailsReceiveForm.CmbImex.SetValue(IMEX.Export, additionalWaitTimeout: 2000, doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbVoyage.SetValue(TT1.Voyage.MSCK000002, doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbOperator.SetValue(Operator.MSC,  doDownArrow: true);
            _roadGateDetailsReceiveForm.CmbDischargePort.SetValue(Port.LYTNZ, doDownArrow: true);
            _roadGateDetailsReceiveForm.AddMultipleCargoIds(@"TEST01", @"20");
            Wait.UntilResponsive(_roadGateDetailsReceiveForm.BtnSave.GetElement());

            _roadGateDetailsReceiveForm.BtnSave.DoClick();

            //warningErrorForm = new WarningErrorForm(@"Warnings for Gate In/Out TT1");
            //warningErrorForm.btnSave.DoClick();

            //9. check that receive cargo label is correct
            Wait.UntilInputIsProcessed(waitTimeout: TimeSpan.FromSeconds(10));
            roadGateForm.SetFocusToForm();
            strLabel = MTNControlBase.GetValue(roadGateForm.lblReceiveItems);
            strExpected = @"Receive Total - Qty=20, Wgt=20000.000 lbs";
            Assert.IsTrue(strLabel == strExpected, "Receive items label expected to be " + strExpected + " actual value is " + strLabel);

            //10. cancel visit
            roadGateForm.CancelVehicleVisit(@"Test 45201 - 2");


        }




    }

}
