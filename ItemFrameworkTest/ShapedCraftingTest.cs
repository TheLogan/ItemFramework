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
				Output = new ItemStack[]
				{
					new ItemStack()
					{
						Item = new TestItemSpade(),
						Amount = 1
					}
				};
			}

		}

		class TestItemStone : Item
		{
			public TestItemStone()
			{
				Name = "Stone";
				StackSize = 64;
			}
		}

		class TestItemStick : Item
		{
			public TestItemStick()
			{
				Name = "Stick";
				StackSize = 64;
			}
		}

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
