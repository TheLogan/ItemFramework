using UnityEngine;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
	public List<CraftingRecipe> Recipes;

	private CraftingManager instance;

	public CraftingManager Instance
	{
		get
		{
			if (instance == null)
			{
				var go = new GameObject("CraftingManager");
				instance = go.AddComponent<CraftingManager>();
			}
			return instance;
		}
	}

	//	+ Recipes : List<CraftingRecipes>
	//- instance : CraftingManager
	//+ Instance() : CraftingManager

	public void Register(CraftingRecipe recipe)
	{
		
	}


//		+ Register(recipe : CraftingRecipe) : void
}
