using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ItemFramework
{
	public class Crafter : MonoBehaviour
	{
		public Container input;
		public Container output;


		public ItemStack[] CraftRecipe(Container incommingContainer)
		{
			//var incommingIngredients = incommingContainer.GetAll();
            var recipes = CraftingManager.Instance.Recipes;

			var selectedRecipes = new List<CraftingRecipe>();
			foreach (var recipe in recipes)
			{
				var recipeIngredientsList = recipe.ingredients.ToList();
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

			/*for (int i = 0; i < selectedRecipes.Count; i++)
			{
				var selectedRecipe = selectedRecipes[i];
				bool viable2 = true;
				foreach (var ingredient in selectedRecipe.ingredients)
				{
					if (!incommingIngredients.Contains(ingredient))
						viable2 = false;
				}
				if (!viable2)
				{
					selectedRecipes.Remove(selectedRecipe);
					i--;
				}
			}*/

			var firstRecipe = selectedRecipes.FirstOrDefault();
			if (firstRecipe == null) return null;

			foreach (ItemStack ingredient in firstRecipe.ingredients)
			{
				incommingContainer.Remove(ingredient);
			}

			return firstRecipe.output;
		}
	}
}