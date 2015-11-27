namespace ItemFramework
{
	public abstract class CraftingRecipe
	{
		public ItemStack[] ingredients;
		public ItemStack[] output;

		public bool CheckRecipe(ItemStack[] input)
		{
			return false; //FIXME write this
		}
	}
}