using UnityEngine;
using System.Collections;
using System.Linq;

namespace ItemFramework
{
	public class ShapedCraftingRecipe : CraftingRecipe
	{
		public int width;
		
		public override bool CheckRecipe(Container input)
		{
			

			var inputStacks = input.ItemStacks;
			return !Ingredients.Where((t, i) => t.Item.GetType() != inputStacks[i].GetType() ||
			t.Amount > inputStacks[i].Amount).Any(); 
		}
	}
}