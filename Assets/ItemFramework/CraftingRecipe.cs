using UnityEngine;
using System.Collections;

public class CraftingRecipe : MonoBehaviour
{
	public ItemStack[] ingredients;
	public ItemStack output;


	public bool CheckRecipe(ItemStack[] input)
	{
		return false;
	}
}
