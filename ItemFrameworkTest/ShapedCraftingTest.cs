using ItemFramework;
using ItemFramework.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
			var stone = new StoneItem();
			var stick = new StickItem();
			var spade = new SpadeItem();


			var stoneStack = new ItemStack(stone, 1, false, true);
			var stickStack = new ItemStack(stick, 1, false, true);

			var shapedRecipe = new ShapedTestingRecipe()
			{
				RecipeIngredients = new ItemStack[]
				{
					null, stoneStack, null,
					null, stickStack, null,
					null, stickStack, null
				},
				width = 3,
				Output = new ItemStack[]
				{
					new ItemStack()
					{
						Item = spade,
						Amount =1
					}
				}
			};

			var craftingField = new Container(16) {Width = 4};
			craftingField.Add(6, new ItemStack(stone));
			craftingField.Add(10, new ItemStack(stick));
			craftingField.Add(14, new ItemStack(stick));

			Assert.IsTrue(shapedRecipe.CheckRecipe(craftingField));
		}

		[TestMethod]
		public void IncorrectInputItemsPlacementTest()
		{
			var stone = new StoneItem();
			var stick = new StickItem();
			var spade = new SpadeItem();


			var stoneStack = new ItemStack(stone, 1, false, true);
			var stickStack = new ItemStack(stick, 1, false, true);

			var shapedRecipe = new ShapedTestingRecipe()
			{
				RecipeIngredients = new ItemStack[]
				{
					null, stoneStack, null,
					null, stickStack, null,
					null, stickStack, null
				},
				width = 3,
				Output = new ItemStack[]
				{
					new ItemStack()
					{
						Item = spade,
						Amount =1
					}
				}
			};

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(1, new ItemStack(stone));
			craftingField.Add(6, new ItemStack(stone));
			craftingField.Add(10, new ItemStack(stick));
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
			var stone = new StoneItem();
			var stick = new StickItem();
			var spade = new SpadeItem();


			var stoneStack = new ItemStack(stone, 1, false, true);
			var stickStack = new ItemStack(stick, 1, false, true);

			var shapedRecipe = new ShapedTestingRecipe()
			{
				RecipeIngredients = new ItemStack[]
				{
					null, stoneStack, 
					null, stickStack, 
					stickStack, null
				},
				width = 2,
				Output = new ItemStack[]
				{
					new ItemStack()
					{
						Item = spade,
						Amount =1
					}
				}
			};

			var craftingField = new Container(16) { Width = 4 };
			craftingField.Add(6, new ItemStack(stone));
			craftingField.Add(10, new ItemStack(stick));
			craftingField.Add(14, new ItemStack(stick));

			Assert.IsFalse(shapedRecipe.CheckRecipe(craftingField));
		}

		class ShapedTestingRecipe : ShapedCraftingRecipe
		{
			
		}
	}
}
