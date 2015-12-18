using ItemFramework;
using System.Linq;

public class FurnaceCrafter : Crafter
{
	public FurnaceCrafter()
	{
		input = new Container();
		output = new Container(1);
	}

	protected override CraftingRecipe[] GetRecipes()
	{
		return CraftingManager.Instance.Recipes.Where(x => x is FurnaceRecipe).ToArray();
	}
}
