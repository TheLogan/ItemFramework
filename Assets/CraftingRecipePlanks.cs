using ItemFramework;

class CraftingRecipePlanks : CraftingRecipe
{
	public CraftingRecipePlanks()
	{
		RecipeIngredients = new ItemStack[1]
		{
			new ItemStack(FrameworkRegistry.GetItem("WoodLog"), 1, true, true)
		};
		Output = new ItemStack[1]
		{
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 4, true, true)
		};
	}
}