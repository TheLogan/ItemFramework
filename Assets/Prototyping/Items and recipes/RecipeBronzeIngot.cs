using System;
using ItemFramework;

public class RecipeBronzeIngot : FurnaceRecipe
{
	public RecipeBronzeIngot() : base()
	{
		RecipeIngredients = new ItemStack[2]
		{
			new ItemStack(new ItemCopperOre(), 3, true, true),
			new ItemStack(new ItemTinOre(), true, true)
		};
		Output = new ItemStack[1]
		{
			new ItemStack(new ItemBronzeIngot(), true, true)
		};
		ProgressTime = 15;
	}
}
