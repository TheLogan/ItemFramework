using ItemFramework;

class CraftingRecipeCraftingTable : ShapedCraftingRecipe
{
	public CraftingRecipeCraftingTable()
	{
		RecipeIngredients = new ItemStack[4]
		{
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true),
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true),
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true),
			new ItemStack(FrameworkRegistry.GetItem("WoodPlank"), 1, true, true)
		};

		Output = new ItemStack[1]
		{
			new ItemStack(FrameworkRegistry.GetItem("CraftingTable"), 1, true, true)
		};

		width = 2;
	}
}