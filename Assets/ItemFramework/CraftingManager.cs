using System;
using System.Collections.Generic;
using System.Linq;

namespace ItemFramework
{
	public class CraftingManager
	{
		private static CraftingManager instance;

		public List<CraftingRecipe> Recipes = new List<CraftingRecipe>();

		public static CraftingManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new CraftingManager();
				}
				return instance;
			}
		}

		private CraftingManager() { }

		public void Register(CraftingRecipe recipe)
		{
			if (!Recipes.Contains(recipe))
			{
				Recipes.Add(recipe);
			}
		}

		public CraftingRecipe[] GetRecipes(ItemStack[] output)
		{
			return Recipes.Where(x => x.Output == output).ToArray();
		}

		public CraftingRecipe[] GetRecipes(ItemStack[] input, ItemStack[] output)
		{
			return Recipes.Where(x => x.RecipeIngredients == input && x.Output == output).ToArray();
		}

		public CraftingRecipe[] GetRecipes(Func<CraftingRecipe, bool> predicate)
		{
			return Recipes.Where(predicate).ToArray();
		}
	}
}