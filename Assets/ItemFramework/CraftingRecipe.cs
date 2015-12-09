using System.Collections.Generic;
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
			if (input.GetAllItemStacks().Length != RecipeIngredients.Count(x => x != null)) return false;
			
			var clonedItemStacks = ItemStack.CloneMultiple(true, input.GetAllItemStacks()).ToList();
			for (int i = 0; i < RecipeIngredients.Length; i++)
			{
				var select = clonedItemStacks.Where(x => x.Item.GetType() == recipeIngredients[i].Item.GetType())
					.Where(x => x.Amount >= recipeIngredients[i].Amount)
					.OrderBy(x => x.Amount)
					.FirstOrDefault();

				if (select == null) return false;

				clonedItemStacks.Remove(select);
			}
			return clonedItemStacks.Count == 0;
		}
	}
}