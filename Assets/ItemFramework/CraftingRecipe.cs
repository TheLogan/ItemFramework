﻿using System.Collections.Generic;
using System.Linq;

namespace ItemFramework
{
	public abstract class CraftingRecipe
	{
		private ItemStack[] recipeIngredients;
		private ItemStack[] output;

		public ItemStack[] RecipeIngredients
		{
			get
			{
				return recipeIngredients;
			}
			set
			{
				ItemStack.LockMultiple(value);
				recipeIngredients = value;
			}
		}

		public ItemStack[] Output
		{
			get
			{
				return output;
			}
			set
			{
				ItemStack.LockMultiple(value);
				output = value;
			}
		}

		public virtual bool CheckRecipe(Container input)
		{
			var inputStacks = input.GetAllItemStacks();
			var recipeIngredientsList = RecipeIngredients.ToList();
			foreach (var recipeIngredient in recipeIngredientsList)
			{
				var recipeIngredientType = recipeIngredient.Item.GetType();
				if (input.Contains(recipeIngredientType) < recipeIngredient.Amount)
				{
					return false;
				}
			}
			return true;
		}
	}
}