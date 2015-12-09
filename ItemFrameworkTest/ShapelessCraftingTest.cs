using ItemFramework;
using ItemFramework.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ShapelessCraftingTest
	{
		[TestInitialize]
		public void Init()
		{
			DbManager.Instance.Handler = new DbFileHandler("unittest.json");
		}

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
					new ItemStack(new ItemBronzeIngot(), true, true)
				};
			}
		}

		
		class TestItemStick : Item
		{
			public TestItemStick()
			{
				Name = "Stick item";
				StackSize = 64;
			}
		}

		class TestItemStone : Item
		{
			public TestItemStone()
			{
				Name = "Stone item";
				StackSize = 32;
			}
		}

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

