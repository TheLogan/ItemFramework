using ItemFramework;
using ItemFramework.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ShapedCraftingTest
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
		/// This recipe should just work
		/// </summary>
		[TestMethod]
		public void CorrectShapedRecipeTest()
		{
			var stick = FrameworkRegistry.GetItem("Stick");
			var stone = FrameworkRegistry.GetItem("Stone");

			var shapedRecipe = new ShapedTestingRecipe();
			
			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(6, new ItemStack(stone));
			craftingField.Add(10, new ItemStack(stick));
			craftingField.Add(14, new ItemStack(stick, 12));

			Assert.IsTrue(shapedRecipe.CheckRecipe(craftingField));
		}

		/// <summary>
		/// This recipe should work, but is placed in a different position in the crafting grid
		/// </summary>
		[TestMethod]
		public void CorrectShapedRecipeDifferentPlacementTest()
		{
			var stick = FrameworkRegistry.GetItem("Stick");
			var stone = FrameworkRegistry.GetItem("Stone");

			var shapedRecipe = new ShapedTestingRecipe();
			
			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(1, new ItemStack(stone));
			craftingField.Add(5, new ItemStack(stick));
			craftingField.Add(9, new ItemStack(stick, 12));

			Assert.IsTrue(shapedRecipe.CheckRecipe(craftingField));
		}

		/// <summary>
		/// This recipe should fail, it has some items in the correct places, 
		/// however it has an extra itemstack placed in a 4th position, the recipe only takes 3 ingredients
		/// </summary>
		[TestMethod]
		public void IncorrectInputItemsPlacementTest()
		{
			var stick = FrameworkRegistry.GetItem("Stick");
			var stone = FrameworkRegistry.GetItem("Stone");

			var shapedRecipe = new ShapedTestingRecipe();

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(1, new ItemStack(stone));
			craftingField.Add(6, new ItemStack(stone));
			craftingField.Add(10, new ItemStack(stick,12));
			craftingField.Add(14, new ItemStack(stick));

			Assert.IsFalse(shapedRecipe.CheckRecipe(craftingField));

			craftingField.Remove(1);
			var testItem = new UnitTestItem();
			craftingField.Add(1, new ItemStack(testItem));

			Assert.IsFalse(shapedRecipe.CheckRecipe(craftingField));
		}

		/// <summary>
		/// This recipe should fail, it has the correct amount of items, 
		/// however one stack is of the wrong type.
		/// </summary>
		[TestMethod]
		public void CorrectShapedDifferentPlacementTest()
		{
			var stick = FrameworkRegistry.GetItem("Stick");
			var stone = FrameworkRegistry.GetItem("Stone");

			var shapedRecipe = new ShapedTestingRecipe();

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(1, new ItemStack(stone));
			craftingField.Add(5, new ItemStack(stone));
			craftingField.Add(9, new ItemStack(stick, 12));

			Assert.IsFalse(shapedRecipe.CheckRecipe(craftingField));
		}


		/// <summary>
		/// This recipe should fail, it has the correct amount of items in the correct amount of stacks, 
		/// however one stack is placed in the wrong position
		/// </summary>
		[TestMethod]
		public void IncorrectInputItemsTest()
		{
			var stick = FrameworkRegistry.GetItem("Stick");
			var stone = FrameworkRegistry.GetItem("Stone");

			var shapedRecipe = new ShapedTestingRecipe();

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(5, new ItemStack(stone));
			craftingField.Add(9, new ItemStack(stick));
			craftingField.Add(14, new ItemStack(stick,12));

			Assert.IsFalse(shapedRecipe.CheckRecipe(craftingField));
		}


		/// <summary>
		/// This recipe should fail, it has the correct amount of stacks, it has the correct types in the correct positions, 
		/// however it's lacking the correct amount of items in one stack.
		/// </summary>
		[TestMethod]
		public void IncorrectShapedRecipeAmountsTest()
		{
			var stick = FrameworkRegistry.GetItem("Stick");
			var stone = FrameworkRegistry.GetItem("Stone");

			var shapedRecipe = new ShapedTestingRecipe();

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(6, new ItemStack(stone));
			craftingField.Add(10, new ItemStack(stick));
			craftingField.Add(14, new ItemStack(stick, 11));

			Assert.IsFalse(shapedRecipe.CheckRecipe(craftingField));
		}
	}
}
