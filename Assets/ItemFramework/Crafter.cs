using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ItemFramework
{
	public abstract class Crafter : MonoBehaviour
	{
		public Container input;
		public Container output;

		protected virtual CraftingRecipe[] GetRecipes()
		{
			return CraftingManager.Instance.Recipes.ToArray();
		}

		public ItemStack[] CraftRecipe(Container incommingContainer, bool simulate = false)
		{
			var recipes = GetRecipes();
			var selectedRecipes = recipes.Where(recipe => recipe.CheckRecipe(incommingContainer)).ToList();
			
			if (selectedRecipes.Count == 0)
				return null;

			var firstRecipe = selectedRecipes.FirstOrDefault();
			if (firstRecipe == null) return null;

			if (!simulate)
			{
				ItemStack[] ingredients = new ItemStack[firstRecipe.RecipeIngredients.Length];

				for (int i = 0, j = ingredients.Length; i < j; i++)
				{
					ingredients[i] = firstRecipe.RecipeIngredients[i].Clone(true);
				}

				incommingContainer.Remove(ingredients);
			}

			return firstRecipe.Output;
		}

		public CraftingRecipe GetFirstRecipe(Container incommingContainer)
		{
			var recipes = GetRecipes();

			var selectedRecipes = new List<CraftingRecipe>();
			foreach (var recipe in recipes)
			{
				var recipeIngredientsList = recipe.RecipeIngredients.ToList();
				var viable = true;
				foreach (var recipeIngredient in recipeIngredientsList)
				{
					var recipeIngredientType = recipeIngredient.Item.GetType();
					if (incommingContainer.Contains(recipeIngredientType) < recipeIngredient.Amount)
					{
						viable = false;
					}
				}
				if (viable)
					selectedRecipes.Add(recipe);
			}

			if (selectedRecipes.Count == 0)
				return null;

			var firstRecipe = selectedRecipes.FirstOrDefault();
			if (firstRecipe == null) return null;

			return firstRecipe;
		}
	}
}