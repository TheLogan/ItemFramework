using System.Collections.Generic;
using System.Linq;

namespace ItemFramework
{
	public abstract class CraftingRecipe
	{
		private ItemStack[] ingredients;
		private ItemStack[] output;

		public ItemStack[] Ingredients
		{
			get
			{
				return ingredients;
			}
			set
			{
				ItemStack.LockMultiple(value);
				ingredients = value;
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
			var inputStacks = input.GetAll();
			var recipeIngredientsList = Ingredients.ToList();
			foreach (var recipeIngredient in recipeIngredientsList)
			{
				var inputIngredient = inputStacks.FirstOrDefault(x => x.GetType() == recipeIngredient.GetType());
				if (inputIngredient == null || inputIngredient.Amount < recipeIngredient.Amount)
					return false;
//				var recipeIngredientType = recipeIngredient.Item.GetType();
//				if (inputContainer.Contains(recipeIngredientType) < recipeIngredient.Amount)
//				{
//					return false;
//				}
			}
			return true;
		}
	}
}