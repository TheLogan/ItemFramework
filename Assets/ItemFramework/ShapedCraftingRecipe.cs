using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Utilities;

namespace ItemFramework
{
	public class ShapedCraftingRecipe : CraftingRecipe
	{
		public int width;

		/// <summary>
		/// Checks the recipe shape against the layout in the crafting layout
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public override bool CheckRecipe(Container input)
		{
			if (input.Width < width) return false;
			if (input.GetAllItemStacks().Length != RecipeIngredients.Count(x => x != null)) return false;

			var inputStacks = input.ItemStacks;

			int inputFirstItemIndex = inputStacks.IndexOf(x => x != null);
			int recipeFirstItemIndex = RecipeIngredients.IndexOf(x => x != null);

			int inputStartIndex = inputFirstItemIndex - recipeFirstItemIndex;
			int inputColumnIndex = inputStartIndex % input.Width;

			foreach (var ingredient in RecipeIngredients)
			{
				if ((ingredient != null && inputStacks[inputStartIndex] == null) ||
					(ingredient == null && inputStacks[inputStartIndex] != null))
				{
					return false;
				}
				if (ingredient != null && inputStacks[inputStartIndex] != null)
				{
					if (ingredient.Item.GetType() != inputStacks[inputStartIndex].Item.GetType())
					{
						return false;
					}
					if (ingredient.Amount > inputStacks[inputStartIndex].Amount)
					{
						return false;
					}
				}
				inputStartIndex++;
				if (inputStartIndex%input.Width == 0)
				{
					inputStartIndex += inputColumnIndex;
				}
			}

			return true;

			/*////////////////////////////////////////////////*/




			/*var crafterWidth = input.Width;

			var recipeHeight = Mathf.RoundToInt(RecipeIngredients.Length / width);
			var crafterHeight = Mathf.RoundToInt(input.ItemStacks.Length / crafterWidth);

			for (int y = 0; y <= crafterHeight - recipeHeight; y++)
			{
				for (int x = 0; x <= crafterWidth - width; x++)
				{
					var crafterItemIndex = y * crafterWidth + x;

					var itemIndexes = new List<int>();
					bool viable = true;
					for (int y2 = 0; y2 < recipeHeight; y2++)
					{
						for (int x2 = 0; x2 < width; x2++)
						{
							var recipeIndex = width * y2 + x2;
							var tempCraftIndex = (y + y2) * input.Width + x + x2;

							if (RecipeIngredients[recipeIndex] != null && input.ItemStacks[tempCraftIndex] != null)
							{
								if (RecipeIngredients[recipeIndex].GetType() != input.ItemStacks[tempCraftIndex].GetType() ||
									RecipeIngredients[recipeIndex].Amount > input.ItemStacks[tempCraftIndex].Amount)
								{
									viable = false;
								}
								else
								{
									itemIndexes.Add(tempCraftIndex);
								}
							}

						}
					}
					if (viable)
					{
						for (int i = 0; i < input.ItemStacks.Length; i++)
						{
							if (!itemIndexes.Contains(i) && input.ItemStacks[i] != null)
							{
								viable = false;
								return false;
							}
						}
						return true;
					}
				}
			}



			var inputStacks = input.ItemStacks;
			return !RecipeIngredients.Where((t, i) => t.Item.GetType() != inputStacks[i].GetType() ||
			t.Amount > inputStacks[i].Amount).Any();*/
		}
	}
}