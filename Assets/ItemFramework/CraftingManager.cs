using UnityEngine;
using System.Collections.Generic;

namespace ItemFramework
{
	public class CraftingManager : MonoBehaviour
	{
		public List<CraftingRecipe> Recipes = new List<CraftingRecipe>();

		private static CraftingManager instance;

		public static CraftingManager Instance
		{
			get
			{
				if (instance == null)
				{
					var go = new GameObject("CraftingManager");
					instance = go.AddComponent<CraftingManager>();
				}
				return instance;
			}
		}

		private CraftingManager()
		{
			Register(new RecipeBronzeIngot());
		}

		public void Register(CraftingRecipe recipe)
		{
			if (!Recipes.Contains(recipe))
			{
				Recipes.Add(recipe);
			}
		}

	}
}