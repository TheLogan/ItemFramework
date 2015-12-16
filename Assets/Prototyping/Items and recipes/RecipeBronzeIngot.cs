using System;
using ItemFramework;

public class RecipeBronzeIngot : FurnaceRecipe
{
	public RecipeBronzeIngot()
	{
		RecipeIngredients = new ItemStack[2]
		{
			new ItemStack(FrameworkRegistry.GetItem("CopperOre"), 3, true, true),
			new ItemStack(FrameworkRegistry.GetItem("TinOre"), true, true)
		};
		Output = new ItemStack[1]
		{
			new ItemStack(FrameworkRegistry.GetItem("BronzeIngot"), true, true)
		};
		ProgressTime = 15;
	}
}
