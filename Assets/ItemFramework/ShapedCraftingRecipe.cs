using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace ItemFramework
{
	/// <summary>
	/// Shaped version of CraftingRecipe
	/// </summary>
	public abstract class ShapedCraftingRecipe : CraftingRecipe
	{
		public int width;

		/// <summary>
		/// Checks the recipe shape against the layout in the crafting layout
		/// </summary>
		/// <param name="input">The crafting grid to check against</param>
		/// <returns></returns>
		public override bool CheckRecipe(Container input)
		{
			// If the recipe is wider than the crafting field, it will fail
			if (input.Width < width) return false;

			// If the amount of item stacks on the recipe and the crafting list aren't the same the recipe will fail.
			if (input.GetAllItemStacks().Length != RecipeIngredients.Count(x => x != null)) return false;

			// Setup a shortcut as we use it a lot
			var inputStacks = input.ItemStacks;

			// Get the first item in the recipe, and the first item in the
			// crafting window, this makes it easier to match them up against each other
			int inputFirstItemIndex = inputStacks.IndexOf(x => x != null);
			int recipeFirstItemIndex = RecipeIngredients.IndexOf(x => x != null);

			// Match the recipe indexes to the crafters indexes
			int inputCurrentPosition = inputFirstItemIndex - recipeFirstItemIndex;

			// Gets the start column and end column of the recipe in comparison to
			// the crafting grid
			int inputColumnStartIndex = inputCurrentPosition % input.Width;
			int inputColumnEndIndex = inputColumnStartIndex + width - 1;

			// Ensure the recipe doesn't extend outside the crafting grid
			if (inputColumnEndIndex >= input.Width) return false;

			// Loop through the ingredients
			for (int i = 0, j = RecipeIngredients.Length; i < j; i++)
			{
				// Ensure the current recipe position is not beyond the length of the crafting grid
				if (inputCurrentPosition >= inputStacks.Length) return false;

				// The current ingredient
				var ingredient = RecipeIngredients[i];

				// If the ingredient is null and the ItemStack at the current position in the
				// input isn't, or vice versa the recipe will fail
				if ((ingredient != null && inputStacks[inputCurrentPosition] == null) ||
					(ingredient == null && inputStacks[inputCurrentPosition] != null))
				{
					return false;
				}

				// If the item type doesn't match or there isn't enough items on the current position
				// the recipe will fail
				if (ingredient.Item.GetType() != inputStacks[inputCurrentPosition].Item.GetType())
				{
					return false;
				}
				if (ingredient.Amount > inputStacks[inputCurrentPosition].Amount)
				{
					return false;
				}

				// If we are at the edge of the recipe jump to the next row of the recipe
				if (inputCurrentPosition % input.Width == inputColumnEndIndex)
				{
					inputCurrentPosition += input.Width - width;
				}

				inputCurrentPosition++;
			}

			return true;
		}
	}
}