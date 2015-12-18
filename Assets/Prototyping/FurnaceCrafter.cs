using ItemFramework;
using System.Linq;

public class FurnaceCrafter : Crafter
{
	/// <summary>
	/// Outgoing Container of crafting results
	/// </summary>
	public Container output;

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
