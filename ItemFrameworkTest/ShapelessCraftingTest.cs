using ItemFramework;
using ItemFramework.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ShapelessCraftingTest
	{
		private Common common = new Common();

		/// <summary>
		/// Set up the database for use in the TestMethods
		/// </summary>
		[TestInitialize]
		public void Init()
		{
			FrameworkRegistry.RegisterMod(common);
			DbManager.Instance.Handler = new DbFileHandler("unittest.json");
		}

		[TestCleanup]
		public void CleanUp()
		{
			DbManager.Instance.Handler.Clear();
		}
		
		/// <summary>
		/// This recipe should work, it has the correct amount of stacks and the correct amount of items
		/// </summary>
		[TestMethod]
		public void CorrectShapelessRecipeTest()
		{
			var stick = FrameworkRegistry.GetItem("Stick");
			var stone = FrameworkRegistry.GetItem("Stone");
			
			var stoneStack = new ItemStack(stone, 12, false, true);
			var stoneStack2 = new ItemStack(stone, 12, false, true);
			var stickStack = new ItemStack(stick, 12, false, true);
			
			var recipe = new TestSpadeRecipe();
			
			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(0, stoneStack);
			craftingField.Add(4, stoneStack2);
			craftingField.Add(12, stickStack);

			Assert.IsTrue(recipe.CheckRecipe(craftingField));
		}

		/// <summary>
		/// This recipe should fail, it has the correct types of items in the correct amounts of stacks, 
		/// however it has the incorrect amounts of items in those stacks
		/// </summary>
		[TestMethod]
		public void IncorrectShapelessAmount()
		{
			var stick = FrameworkRegistry.GetItem("Stick");
			var stone = FrameworkRegistry.GetItem("Stone");

			var stoneStack = new ItemStack(stone, 2, false, true);
			var stoneStack2 = new ItemStack(stone, 2, false, true);
			var stickStack = new ItemStack(stick, 12, false, true);

			var recipe = new TestSpadeRecipe();

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(0, stoneStack);
			craftingField.Add(4, stickStack);
			craftingField.Add(12, stoneStack2);

			Assert.IsFalse(recipe.CheckRecipe(craftingField));
		}

		/// <summary>
		/// This recipe should fail, it has more than enough items,
		/// However there are too few stacks, if the stacks were split up it would work however, but they aren't
		/// </summary>
		[TestMethod]
		public void IncorrectShapelessInputTest()
		{
			var stick = FrameworkRegistry.GetItem("Stick");
			var stone = FrameworkRegistry.GetItem("Stone");

			var stoneStack = new ItemStack(stone, 30, false, true);
			var stickStack = new ItemStack(stick, 30, false, true);

			var recipe = new TestSpadeRecipe();

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(1, stoneStack);
			craftingField.Add(2, stickStack);
			
			Assert.IsFalse(recipe.CheckRecipe(craftingField));
		}

		/// <summary>
		/// This recipe should fail, it has the right amount of stacks with enough of each item,
		/// However, one stack is of the wrong type
		/// </summary>
		[TestMethod]
		public void IncorrectShapelessStacksTest()
		{
			var stick = FrameworkRegistry.GetItem("Stick");
			var stone = FrameworkRegistry.GetItem("Stone");

			var stoneStack = new ItemStack(stone, 30, false, true);
			var stickStack = new ItemStack(stick, 30, false, true);
			var stickStack2 = new ItemStack(stick, 30, false, true);

			var recipe = new TestSpadeRecipe();

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(1, stoneStack);
			craftingField.Add(2, stickStack);
			craftingField.Add(3, stickStack2);

			Assert.IsFalse(recipe.CheckRecipe(craftingField));
		}
	}
}

