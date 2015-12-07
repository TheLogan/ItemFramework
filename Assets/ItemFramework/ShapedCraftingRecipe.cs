using UnityEngine;
using System.Collections;
using System.Linq;

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
			var crafterWidth = input.Width;

			var recipeHeight = Mathf.RoundToInt(RecipeIngredients.Length / width);
			var crafterHeight = Mathf.RoundToInt(input.ItemStacks.Length / crafterWidth);

			for (int y = 0; y < crafterHeight - recipeHeight; y++)
			{
				for (int x = 0; x < crafterWidth - width; x++)
				{
					var crafterItemIndex = y * crafterWidth + x;

					if (RecipeIngredients[0].GetType() == input.ItemStacks[crafterItemIndex].GetType() && RecipeIngredients[0].Amount <= input.ItemStacks[crafterItemIndex].Amount)
					{
						//We can continue
						for (int y2 = 0; y2 < recipeHeight; y2++)
						{
							for (int x2 = 0; x2 < width; x2++)
							{
								
							}
						}
					}
				}
			}



			var inputStacks = input.ItemStacks;
			return !RecipeIngredients.Where((t, i) => t.Item.GetType() != inputStacks[i].GetType() ||
			t.Amount > inputStacks[i].Amount).Any();
		}
	}
}