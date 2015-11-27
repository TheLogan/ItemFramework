using UnityEngine;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
	public List<CraftingRecipe> Recipes;

	private static CraftingManager instance;

	public static CraftingManager Instance
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

	public void Register(CraftingRecipe recipe)
	{
		
	}


}
