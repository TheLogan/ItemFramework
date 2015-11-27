using UnityEngine;
using System.Collections;

public abstract class CraftingRecipe : MonoBehaviour
{
	public ItemStack[] ingredients;
	public ItemStack[] output;


	public bool CheckRecipe(ItemStack[] input)
	{
		return false;
	}
}
