using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Crafter : MonoBehaviour
{
	public Container input;
	public Container output;


	public ItemStack[] CraftRecipe(ItemStack[] incommingIngredients)
	{
		var recipes = CraftingManager.Instance.Recipes;
		
		var selectedRecipes = new List<CraftingRecipe>();
		foreach (var recipe in recipes)
		{
			var recipeIngredientsList = recipe.ingredients.ToList();
			var viable = true;
			foreach (var incommingIngredient in incommingIngredients)
			{
				if (!recipeIngredientsList.Contains(incommingIngredient))
				{
					viable = false;
				}
			}
			if(viable)
				selectedRecipes.Add(recipe);
		}

		if (selectedRecipes.Count == 0)
			return null;


		foreach (var selectedRecipe in selectedRecipes)
		{
			bool viable2 = true;
			foreach (var ingredient in selectedRecipe.ingredients)
			{
				if (!incommingIngredients.Contains(ingredient))
					viable2 = false;
			}
			if (!viable2)
				selectedRecipes.Remove(selectedRecipe);
		}

		var firstRecipe = selectedRecipes.FirstOrDefault();
		return firstRecipe != null ? firstRecipe.output : null;
	}
}
