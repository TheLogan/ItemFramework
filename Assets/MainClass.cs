using System;
using System.Collections.Generic;
using System.Linq;
using ItemFramework;
using UnityEngine;

namespace Assets
{
	class MainClass : MonoBehaviour, IMod
	{
		private Container inventory;
		private CrafterPlayer crafterPlayer;
		private CrafterCraftingTable crafterCraftingTable;

		public void Awake()
		{
			FrameworkRegistry.RegisterMod(this);
		}

		private bool CraftPlanks()
		{
			CraftingRecipe logToPlankRecipe = CraftingManager.Instance.GetRecipes((recipe) =>
			{
				return recipe.Output.FirstOrDefault().Item.GetType() == FrameworkRegistry.GetItem("WoodPlank").GetType();
			}).FirstOrDefault();

			if (logToPlankRecipe != null)
			{
				crafterPlayer.input.Add(inventory.Remove(FrameworkRegistry.GetItem("WoodLog"), 1));

				if (logToPlankRecipe.CheckRecipe(crafterPlayer.input))
				{
					inventory.Add(crafterPlayer.CraftRecipe(logToPlankRecipe));

					Debug.Log("Crafted planks");
					return true;
				}
			}

			return false;
		}

		private bool CraftCraftingTable()
		{
			CraftingRecipe craftingTableRecipe = CraftingManager.Instance.GetRecipes((recipe) =>
			{
				return recipe.Output.FirstOrDefault().Item.GetType() == FrameworkRegistry.GetItem("CraftingTable").GetType();
			}).FirstOrDefault();

			if (craftingTableRecipe != null)
			{
				Item woodPlank = FrameworkRegistry.GetItem("WoodPlank");

				crafterPlayer.input.Add(0, inventory.Remove(woodPlank, 1));
				crafterPlayer.input.Add(1, inventory.Remove(woodPlank, 1));
				crafterPlayer.input.Add(2, inventory.Remove(woodPlank, 1));
				crafterPlayer.input.Add(3, inventory.Remove(woodPlank, 1));

				if (craftingTableRecipe.CheckRecipe(crafterPlayer.input))
				{
					inventory.Add(crafterPlayer.CraftRecipe(craftingTableRecipe));
					crafterCraftingTable = new CrafterCraftingTable();

					Debug.Log("Crafted Crafting Table");

					return true;
				}
			}

			return false;
		}

		private bool CraftChest()
		{
			if (crafterCraftingTable == null)
			{
				return false;
			}

			CraftingRecipe chestRecipe = CraftingManager.Instance.GetRecipes((recipe) =>
			{
				return recipe.Output.FirstOrDefault().Item.GetType() == FrameworkRegistry.GetItem("Chest").GetType();
			}).FirstOrDefault();

			if (chestRecipe != null)
			{
				Item woodPlank = FrameworkRegistry.GetItem("WoodPlank");

				crafterCraftingTable.input.Add(0, inventory.Remove(woodPlank, 1));
				crafterCraftingTable.input.Add(1, inventory.Remove(woodPlank, 1));
				crafterCraftingTable.input.Add(2, inventory.Remove(woodPlank, 1));
				crafterCraftingTable.input.Add(3, inventory.Remove(woodPlank, 1));
				crafterCraftingTable.input.Add(5, inventory.Remove(woodPlank, 1));
				crafterCraftingTable.input.Add(6, inventory.Remove(woodPlank, 1));
				crafterCraftingTable.input.Add(7, inventory.Remove(woodPlank, 1));
				crafterCraftingTable.input.Add(8, inventory.Remove(woodPlank, 1));

				if (chestRecipe.CheckRecipe(crafterCraftingTable.input))
				{
					inventory.Add(crafterCraftingTable.CraftRecipe(chestRecipe));

					Debug.Log("Crafted Chest");

					return true;
				}
			}

			return false;
		}

		public void Start()
		{
			inventory = new Container();
			crafterPlayer = new CrafterPlayer();

			inventory.Add(0, new ItemStack(FrameworkRegistry.GetItem("WoodLog"), 3));

			if (CraftPlanks())
			{
				if (CraftCraftingTable())
				{
					if (CraftPlanks())
					{
						if (CraftPlanks())
						{
							if (CraftChest())
							{
								Debug.Log("Done crafting");
								return;
							}
						}
					}
				}
			}

			Debug.Log("Could not craft chest");
		}

		public Dictionary<string, Type> RegisterItems()
		{
			return new Dictionary<string, Type>()
			{
				{ "WoodLog", typeof(ItemWoodLog) },
				{ "WoodPlank", typeof(ItemWoodPlank) },
				{ "CraftingTable", typeof(ItemCraftingTable) },
				{ "Chest", typeof(ItemChest) }
			};
		}

		public Type[] RegisterRecipes()
		{
			return new[]
			{
				typeof(CraftingRecipePlanks),
				typeof(CraftingRecipeCraftingTable),
				typeof(CraftingRecipeChest)
			};
		}
	}
}
