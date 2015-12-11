using ItemFramework;
using ItemFramework.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ShapelessCraftingTest
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
		/// This recipe should work, it has the correct amount of stacks and the correct amount of items
		/// </summary>
		[TestMethod]
		public void CorrectShapelessRecipeTest()
		{
			var stick = new TestItemStick();
			var stone = new TestItemStone();
			
			var stoneStack = new ItemStack(stone, 12, false, true);
			var stoneStack2 = new ItemStack(stone, 12, false, true);
			var stickStack = new ItemStack(stick, 12, false, true);
			
			var recipe = new TestSpadeRecipe();
			
			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(0, stoneStack);
			craftingField.Add(1, stickStack);
			craftingField.Add(2, stoneStack2);

			Assert.IsTrue(recipe.CheckRecipe(craftingField));
		}

		/// <summary>
		/// This recipe should fail, it has the correct types of items in the correct amounts of stacks, 
		/// however it has the incorrect amounts of items in those stacks
		/// </summary>
		[TestMethod]
		public void IncorrectShapelessAmount()
		{
			var stick = new TestItemStick();
			var stone = new TestItemStone();

			var stoneStack = new ItemStack(stone, 2, false, true);
			var stoneStack2 = new ItemStack(stone, 2, false, true);
			var stickStack = new ItemStack(stick, 12, false, true);

			var recipe = new TestSpadeRecipe();

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(0, stoneStack);
			craftingField.Add(1, stickStack);
			craftingField.Add(2, stoneStack2);

			Assert.IsFalse(recipe.CheckRecipe(craftingField));
		}

		/// <summary>
		/// This recipe should fail, it has more than enough items,
		/// However there are too few stacks, if the stacks were split up it would work however, but they aren't
		/// </summary>
		[TestMethod]
		public void IncorrectShapelessInputTest()
		{
			var stick = new TestItemStick();
			var stone = new TestItemStone();

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
			var stick = new TestItemStick();
			var stone = new TestItemStone();
			
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


		#region auxiliary classes

		/// <summary>
		/// This is the recipe class used for testing, it's derived from the base CraftingRecipe class
		/// </summary>
		class TestSpadeRecipe : CraftingRecipe
		{
			public TestSpadeRecipe() : base()
			{
				RecipeIngredients = new ItemStack[3]
				{
					new ItemStack(new TestItemStick(), 10, true, true),
					new ItemStack(new TestItemStone(), 10, true, true),
                    new ItemStack(new TestItemStone(), 3, true, true)
				};
				
				Output = new ItemStack[1]
				{
					new ItemStack(new TestItemSpade(), true, true)
				};
			}
		}

		/// <summary>
		/// Test stick item used in the recipe
		/// </summary>
		class TestItemStick : Item
		{
			public TestItemStick()
			{
				Name = "Stick item";
				StackSize = 64;
			}
		}

		/// <summary>
		/// Test stone item used in the recipe
		/// </summary>
		class TestItemStone : Item
		{
			public TestItemStone()
			{
				Name = "Stone item";
				StackSize = 32;
			}
		}

		/// <summary>
		/// Test spade item used as recipe output
		/// </summary>
		class TestItemSpade : Item
		{
			public TestItemSpade()
			{
				Name = "Spade";
				StackSize = 1;
			}
		}

		#endregion
	}
}

