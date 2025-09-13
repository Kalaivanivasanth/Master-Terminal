using DataObjects.LogInOutBO;
using HardcodedData.SystemData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MTNBaseClasses.BaseClasses.MasterTerminal;
using MTNForms.Controls;
using MTNForms.FormObjects;
using MTNForms.FormObjects.System_Items.System_Ops;
using MTNGlobal;
using MTNGlobal.EnumsStructs;
using MTNUtilityClasses.Classes;
using NuGet.Frameworks;

namespace MTNAutomationTests.TestCases.Master_Terminal.Yard_Functions
{
    [TestClass, TestCategory(TestCategories.MTN)]
    public class TestCase51348 : MTNBase
    {
        private CargoTypesForm _cargoTypesForm;
        private RecipeMaintenanceForm _recipeMaintenanceForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) => BaseClassInitialize_New(testContext);
        
        [TestInitialize]
        public new void TestInitialize() {}
        
        [TestCleanup]
        public new void TestCleanup()
        {
            base.TestCleanup();
        }

        void MTNInitialize() => LogInto<MTNLogInOutBO>();

        [TestMethod]
        public void BulkBlendingCreateEditDeleteRecipe()
        {
            MTNInitialize();
            
            // Step 1 Open System Items | System Ops | Cargo Types
            FormObjectBase.NavigationMenuSelection(@"System Ops|Cargo Types");
            _cargoTypesForm = new CargoTypesForm();

            // Step 2 Select cargo type where Abbreviation = ALIG and Description = Aluminium Ingots
            MTNControlBase.SetValueInEditTable(_cargoTypesForm.tblSearch, @"Abbreviation", @"ALIG");
            _cargoTypesForm.DoSearch();

            // Thursday, 27 February 2025 navmh5 Can be removed 6 months after specified date  MTNControlBase.FindClickRowInTable(_cargoTypesForm.tblCargoDetails,
             // Thursday, 27 February 2025 navmh5 Can be removed 6 months after specified date     @"Abbreviation^ALIG~Description^Aluminium Ingots");
            _cargoTypesForm.tblCargoDetails1.FindClickRow(new[] { "Abbreviation^ALIG~Description^Aluminium Ingots" });

            // Step 3 Click the Edit button
            _cargoTypesForm.DoEdit();

            // Step 4 Click Blending Recipes tab
            _cargoTypesForm.GetBlendingRecipesTab();
            if (_cargoTypesForm.TblBlendingRecipes.NumberOfRowsInTable() > 1)
                _cargoTypesForm.TblBlendingRecipes.DeleteRowsFromTable(new[] { "Code^RECIPE1~Description^Test_Recipe" },
                    _cargoTypesForm.btnDeleteRecipe.GetElement(), out var rowsNotFound);
            _cargoTypesForm.DoSave();
            
            // Step 5 Click Add button
            _cargoTypesForm.DoEdit();
            _cargoTypesForm.GetBlendingRecipesTab();
            _cargoTypesForm.btnAddRecipe.DoClick();

            // Step 6 Enter Code = RECIPE1, Description = Test_Recipe, Cargo Sub Type = ALIG_BIGPACKS, Weight = 6000.000 lbs, Volume = 200.0000 m3, 
            //    Quantity = 4, Recipe can be scaled/ factored - Ticked, Instructions - Mix Clinker and Metal
            _recipeMaintenanceForm = new RecipeMaintenanceForm(@"Recipe Maintenance");
            _recipeMaintenanceForm.GetRecipeFields();
            _recipeMaintenanceForm.txtCode.SetValue(@"RECIPE1");
            _recipeMaintenanceForm.txtDescription.SetValue(@"Test_Recipe");
            _recipeMaintenanceForm.cmbCargoSubType.SetValue(CargoSubtype.ALIGBIGPACKS, doDownArrow: true);
           
            _recipeMaintenanceForm.txtQuantity.SetValue(@"4");
            _recipeMaintenanceForm.chkRecipeScaledFactored.DoClick();

            _recipeMaintenanceForm.txtInstructions.SetValue(@"Mix Clinker and Metal");
            // Step 7 Click Add Component button
            _recipeMaintenanceForm.btnAddComponent.DoClick();

            // Step 8 Enter Cargo type - Clinker, Amount - 2.000 MT, Sequence - 1
            _recipeMaintenanceForm.GetComponentFields();
            _recipeMaintenanceForm.cmbComponentCargoType.SetValue(@"Clinker");
            _recipeMaintenanceForm.SetComponentAmount("2.000", false);
            _recipeMaintenanceForm.txtComponentSequence.SetValue(@"1");
            // Step 9 Click Update Component button
            _recipeMaintenanceForm.btnUpdateComponent.DoClick();

            // Step 10 Click Add Component button
            _recipeMaintenanceForm.btnAddComponent.DoClick();

            // Step 11 Enter Cargo Type - METAL, Amount - 5, Sequence - 2
            _recipeMaintenanceForm.GetComponentFields();
            _recipeMaintenanceForm.cmbComponentCargoType.SetValue(@"METAL");
            _recipeMaintenanceForm.SetComponentAmount("5");
            _recipeMaintenanceForm.txtComponentSequence.SetValue(@"2");
            // Step 12 Click Update Component button
            _recipeMaintenanceForm.btnUpdateComponent.DoClick();

            // Step 13 Click Save Recipe button
            _recipeMaintenanceForm.btnSaveRecipe.DoClick();

            // Step 14 Click Save button
            _cargoTypesForm.DoSave();

            // Step 15 Click Edit button
            _cargoTypesForm.DoEdit();

            // Step 16 Click Blending Recipes tab
            _cargoTypesForm.GetBlendingRecipesTab();

            // Step 17 Select where Code - RECIPE1 and Description - Test_Recipe
            // Thursday, 27 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_cargoTypesForm.tblBlendingRecipes, @"Code^RECIPE1~Description^Test_Recipe");
            _cargoTypesForm.TblBlendingRecipes.FindClickRow(new[] { "Code^RECIPE1~Description^Test_Recipe" });

            // Step 18 Click Update button
            _cargoTypesForm.btnUpdateRecipe.DoClick();

            // Step 19 Select where Component Cargo Type - Clinker
            _recipeMaintenanceForm = new RecipeMaintenanceForm(@"Recipe Maintenance");
            _recipeMaintenanceForm.SetFocusToForm();
            //_recipeMaintenanceForm.GetComponentsTable();
            // Thursday, 27 February 2025 navmh5 Can be removed 6 months after specified date MTNControlBase.FindClickRowInTable(_recipeMaintenanceForm.tblComponents, @"Component Cargo Type^Clinker");
            _recipeMaintenanceForm.TblComponents.FindClickRow(new[] { "Component Cargo Type^Clinker" });

            // Step 20 Click Delete Component button
            _recipeMaintenanceForm.GetComponentFields();
            _recipeMaintenanceForm.btnDeleteComponent.DoClick();

            // Step 21 Click Save Recipe button
            _recipeMaintenanceForm.GetRecipeFields();
            _recipeMaintenanceForm.btnSaveRecipe.DoClick();

            // Step 22 Click Save button
            _cargoTypesForm.SetFocusToForm();
            _cargoTypesForm.DoSave();

            // Step 23 Click View button
            // Tuesday, 28 January 2025 navmh5 MTNControlBase.FindClickRowInTable(_cargoTypesForm.tblBlendingRecipes, @"Code^RECIPE1~Description^Test_Recipe");
            _cargoTypesForm.TblBlendingRecipes.FindClickRow(new[] { "Code^RECIPE1~Description^Test_Recipe" });
            _cargoTypesForm.btnUpdateRecipe.DoClick();

            // Step 24 Check that Deleted component is not there and Close recipe Maintenance form
            _recipeMaintenanceForm = new RecipeMaintenanceForm(@"Recipe Maintenance");
            _recipeMaintenanceForm.SetFocusToForm();
            // Tuesday, 28 January 2025 navmh5 _recipeMaintenanceForm.GetComponentsTable();
            // Tuesday, 28 January 2025 navmh5 Assert.IsFalse(MTNControlBase.FindClickRowInTable(_recipeMaintenanceForm.tblComponents, @"Component Cargo Type^Clinker", ClickType.None, doAssert: false));
            var found =_recipeMaintenanceForm.TblComponents.FindClickRow(new[] { "Component Cargo Type^Clinker" }, ClickType.None, doAssert: false);
            Assert.IsFalse(string.IsNullOrEmpty(found), "Component was found");
            _recipeMaintenanceForm.GetRecipeFields();
            _recipeMaintenanceForm.btnClose.DoClick();

            // Step 25 Click Edit button
            _cargoTypesForm.SetFocusToForm();
            _cargoTypesForm.DoEdit();

            // Step 26 Click Blending Recipes tab
            _cargoTypesForm.GetBlendingRecipesTab();

            // Step 28 Click Delete button
           if (_cargoTypesForm.TblBlendingRecipes.NumberOfRowsInTable() > 0)
               _cargoTypesForm.TblBlendingRecipes.DeleteRowsFromTable(new[] { "Code^RECIPE1~Description^Test_Recipe" },
                   _cargoTypesForm.btnDeleteRecipe.GetElement(), out var rowsNotFound);

            // Step 29 Click Save button
            _cargoTypesForm.DoSave();
        }
    }
}
