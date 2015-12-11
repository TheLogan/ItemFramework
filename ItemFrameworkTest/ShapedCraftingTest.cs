using ItemFramework;
using ItemFramework.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ShapedCraftingTest
	{
		/// <summary>
		/// Set up the database for use in the TestMethods
		/// </summary>
		[TestInitialize]
		public void Init()
		{
			DbManager.Instance.Handler = new DbFileHandler("unittest.json");
		}


		/// <summary>
		/// This recipe should just work
		/// </summary>
		[TestMethod]
		public void CorrectShapedRecipeTest()
		{
			var stone = new TestItemStone();
			var stick = new TestItemStick();

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
			var stone = new TestItemStone();
			var stick = new TestItemStick();

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
			var stone = new TestItemStone();
			var stick = new TestItemStick();

			var shapedRecipe = new ShapedTestingRecipe();

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(1, new ItemStack(stone));
			craftingField.Add(6, new ItemStack(stone));
			craftingField.Add(10, new ItemStack(stick,12));
			craftingField.Add(14, new ItemStack(stick));

			Assert.IsFalse(shapedRecipe.CheckRecipe(craftingField));

			craftingField.Remove(1);
			var testItem = new UnitTestItem(16);
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
			var stone = new TestItemStone();
			var stick = new TestItemStick();

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
			var stone = new TestItemStone();
			var stick = new TestItemStick();

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
			var stone = new TestItemStone();
			var stick = new TestItemStick();

			var shapedRecipe = new ShapedTestingRecipe();

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(6, new ItemStack(stone));
			craftingField.Add(10, new ItemStack(stick));
			craftingField.Add(14, new ItemStack(stick, 11));

			Assert.IsFalse(shapedRecipe.CheckRecipe(craftingField));
		}

		#region auxiliary classes
		/// <summary>
		/// A temporary class that derives from the Shaped Crafting Recipe
		/// This is used as a recipe for all the tests above.
		/// </summary>
		class ShapedTestingRecipe : ShapedCraftingRecipe
		{
			public ShapedTestingRecipe() : base()
			{
				RecipeIngredients = new ItemStack[]
				{
					new ItemStack(new TestItemStone(), 1, true, true),
					new ItemStack(new TestItemStick(), 1, true, true),
					new ItemStack(new TestItemStick(), 12, true, true)
				};
				width = 1;
				Output = new[]
				{
					new ItemStack()
					{
						Item = new TestItemSpade(),
						Amount = 1
					}
				};
			}
		}
		
		/// <summary>
		/// The stone test item	used in all recipes above
		/// </summary>
		class TestItemStone : Item
		{
			public TestItemStone()
			{
				Name = "Stone";
				StackSize = 64;
			}
		}

		/// <summary>
		/// The stick test item used in all recipes above
		/// </summary>
		class TestItemStick : Item
		{
			public TestItemStick()
			{
				Name = "Stick";
				StackSize = 64;
			}
		}

		/// <summary>
		/// A spade test item which is set as the recipe output
		/// </summary>
		class TestItemSpade : Item
		{
			public TestItemSpade()
			{
				Name = "Spade";
				StackSize = 64;
			}
		}
		#endregion
	}
}
