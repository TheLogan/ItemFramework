using UnityEngine;
using System.Collections;

namespace ItemFramework {
    public class CraftingRecipe : MonoBehaviour {
        public ItemStack[] ingredients;
        public ItemStack output;


        public bool CheckRecipe(ItemStack[] input) {
            return false;
        }
    }
}