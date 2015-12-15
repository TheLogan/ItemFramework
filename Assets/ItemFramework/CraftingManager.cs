using System.Collections.Generic;

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

	}
}