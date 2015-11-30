using UnityEngine;
using System.Collections;
using System.Linq;

namespace ItemFramework
{
	public class ShapedCraftingRecipe : CraftingRecipe
	{
		public bool CheckRecipe(ItemStack[] input)
		{
			return !Ingredients.Where((t, i) => t.Item.GetType() != input[i].GetType() ||
			t.Amount > input[i].Amount).Any(); 
		}
	}
}