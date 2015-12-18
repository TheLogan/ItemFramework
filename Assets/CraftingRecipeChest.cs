using ItemFramework;

class CraftingRecipeChest : ShapedCraftingRecipe
{
	public CraftingRecipeChest()
	{
		RecipeIngredients = new ItemStack[9]
		{
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true),
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true),
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true),
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true),
			null,
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true),
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true),
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true),
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true)
		};
		Output = new ItemStack[1]
		{
			new ItemStack(FrameworkRegistry.GetItem("Chest"), 1, true, true)
		};
		width = 3;
	}
}