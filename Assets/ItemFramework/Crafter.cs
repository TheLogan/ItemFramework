using System.Linq;

namespace ItemFramework
{
	public abstract class Crafter
	{
		/// <summary>
		/// Incoming Container of crafting ingredients
		/// </summary>
		public Container input;
		/// <summary>
		/// Outgoing Container of crafting results
		/// </summary>
		public Container output;

		/// <summary>
		/// Get available CraftingRecipes for this Crafter
		/// </summary>
		/// <returns>Available CraftingRecipes</returns>
		protected virtual CraftingRecipe[] GetRecipes()
		{
			return CraftingManager.Instance.Recipes.ToArray();
		}

		/// <summary>
		/// Internal CraftRecipe.
		/// Performs the actual crafting of the recipe.
		/// </summary>
		/// <param name="recipe">CraftingRecipe</param>
		/// <param name="container">Input container</param>
		/// <param name="simulate">If true; does not subtract the ingredients from the container</param>
		/// <returns></returns>
		private ItemStack[] PerformCraftRecipe(CraftingRecipe recipe, Container container = null, bool simulate = false)
		{
			if (!simulate)
			{
				container.Remove(ItemStack.CloneMultiple(true, recipe.RecipeIngredients));
			}

			return ItemStack.GetPersistantMultiple(recipe.Output);
		}

		/// <summary>
		/// Craft a specific CraftingRecipe.
		/// Will check if the CraftingRecipe is allowed on this Crafter.
		/// </summary>
		/// <param name="recipe">CraftingRecipe</param>
		/// <param name="container">Force input container; if null, uses input</param>
		/// <param name="simulate">If true; does not subtract the ingredients from the container</param>
		/// <returns></returns>
		public ItemStack[] CraftRecipe(CraftingRecipe recipe, Container container = null, bool simulate = false)
		{
			if (container == null)
			{
				container = input;
			}

			var recipes = GetRecipes();
			if (!recipes.Contains(recipe))
			{
				return null;
			}

			return PerformCraftRecipe(recipe, container, simulate);
		}

		/// <summary>
		/// Craft the first found recipe for the input.
		/// </summary>
		/// <param name="container">Force input container; if null, uses input</param>
		/// <param name="simulate">If true; does not subtract the ingredients from the container</param>
		/// <returns>Result of recipe; if no result, null</returns>
		public ItemStack[] CraftRecipe(Container container = null, bool simulate = false)
		{
			if (container == null)
			{
				container = input;
			}

			CraftingRecipe recipe = GetFirstRecipe(container);

			if (recipe == null) return null;

			return PerformCraftRecipe(recipe, container, simulate);
		}

		/// <summary>
		/// Get the first found recipe for the input.
		/// </summary>
		/// <param name="container">Force input container; if null, uses input</param>
		/// <returns>The first found recipe; if no recipe found, null</returns>
		public CraftingRecipe GetFirstRecipe(Container container = null)
		{
			if (container == null)
			{
				container = input;
			}

			var recipes = GetRecipes();

			return recipes.FirstOrDefault(recipe => recipe.CheckRecipe(container));
		}

		private bool DoAutoCraft(CraftingRecipe recipe, Container container)
		{
			if (recipe.CheckRecipe(container))
			{
				if (container.CanAdd(recipe.Output))
				{
					container.Add(CraftRecipe(recipe, container));
					return true;
				}
			}

			CraftingRecipe[] recipes = CraftingManager.Instance.GetRecipes((CraftingRecipe subRecipe) => {
				// TODO: Make this work

				bool found = false;

				return found;
			});

			return false;
		}

		public ItemStack[] AutoCraft(CraftingRecipe endResult, Container container = null)
		{
			if (container == null)
			{
				container = input;
			}

			if (endResult.CheckRecipe(container))
			{
				if (container.CanAdd(endResult.Output))
				{
					container.Add(CraftRecipe(endResult, container));
					return endResult.Output;
				}
			}

			if (DoAutoCraft(endResult, container))
			{
				return endResult.Output;
			}

			return null;
		}
	}
}