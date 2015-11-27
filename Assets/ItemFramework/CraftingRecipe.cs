using UnityEngine;
using System.Collections;

namespace ItemFramework {
public abstract class CraftingRecipe : MonoBehaviour
{
	public ItemStack[] ingredients;
	public ItemStack[] output;


        public bool CheckRecipe(ItemStack[] input) {
            return false;
        }
    }
}