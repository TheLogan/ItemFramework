using System;
using System.Collections.Generic;
using ItemFramework;

namespace ItemFrameworkTest
{
	class Common : IMod
	{
		public static readonly string ItemName = "UnitTest";
		public static readonly int ItemStackSize = 64;

		public Item GetItem()
		{
			return FrameworkRegistry.GetItem(ItemName);
		}

		public ItemStack GetItemStack(int amount)
		{
			return new ItemStack(GetItem(), amount, false, true);
		}

		public Dictionary<string, Type> RegisterItems()
		{
			return new Dictionary<string, Type>()
			{
				{ "UnitTest", typeof(UnitTestItem) },
				{ "Stick", typeof(StickItem) },
				{ "Stone", typeof(StoneItem) },
				{ "Spade", typeof(SpadeItem) }
			};
		}

		public Type[] RegisterRecipes()
		{
			return null;
		}
	}

	class UnitTestItem : Item
	{
		public UnitTestItem()
		{
			Name = Common.ItemName;
			StackSize = Common.ItemStackSize;
		}
	}

	/// <summary>
	/// Test stone item used as recipe input
	/// </summary>
	public class StoneItem : Item
	{
		public StoneItem()
		{
			Name = "Stone";
			StackSize = 64;
		}
	}

	/// <summary>
	/// Test stick item used as recipe input
	/// </summary>
	public class StickItem : Item
	{
		public StickItem()
		{
			Name = "Stick";
			StackSize = 64;
		}
	}

	/// <summary>
	/// Test spade item used as recipe output
	/// </summary>
	public class SpadeItem : Item
	{
		public SpadeItem()
		{
			Name = "Spade";
			StackSize = 1;
		}
	}

	/// <summary>
	/// This is the recipe class used for testing, it's derived from the base CraftingRecipe class
	/// </summary>
	public class TestSpadeRecipe : CraftingRecipe
	{
		public TestSpadeRecipe() : base()
		{
			RecipeIngredients = new ItemStack[3]
			{
					new ItemStack(FrameworkRegistry.GetItem("Stick"), 10, true, true),
					new ItemStack(FrameworkRegistry.GetItem("Stone"), 10, true, true),
					new ItemStack(FrameworkRegistry.GetItem("Stone"), 3, true, true)
			};

			Output = new ItemStack[1]
			{
					new ItemStack(FrameworkRegistry.GetItem("Spade"), true, true)
			};
		}
	}

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
					new ItemStack(FrameworkRegistry.GetItem("Stone"), 1, true, true),
					new ItemStack(FrameworkRegistry.GetItem("Stick"), 1, true, true),
					new ItemStack(FrameworkRegistry.GetItem("Stick"), 12, true, true)
            };
			width = 1;
			Output = new[]
			{
					new ItemStack()
					{
						Item = FrameworkRegistry.GetItem("Spade"),
						Amount = 1
					}
				};
		}
	}
}
