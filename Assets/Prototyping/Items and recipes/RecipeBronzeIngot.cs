using System;
using ItemFramework;

public class RecipeBronzeIngot : CraftingRecipe
{
	public RecipeBronzeIngot() : base()
	{
		Ingredients = new ItemStack[2]
		{
			new ItemStack(new ItemCopperOre(), 3),
			new ItemStack(new ItemTinOre())
		};
		Output = new ItemStack[1]
		{
			new ItemStack(new ItemBronzeIngot())
		};
	}
}
