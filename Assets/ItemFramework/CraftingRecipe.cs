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

		public virtual bool CheckRecipe(ItemStack[] input)
		{
			return false; //FIXME write this
		}
	}
}