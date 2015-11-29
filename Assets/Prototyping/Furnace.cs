using ItemFramework;
using Debug = UnityEngine.Debug;

public class Furnace : Crafter
{
    void Start()
    {
        input.Changed += OnInputChanged;
    }

    private void OnInputChanged()
    {
        Debug.Log("Input changed => " + input.ToString());
        ItemStack[] simulation = CraftRecipe(input, true);
        if (simulation != null && output.CanAdd(simulation))
        {
            ItemStack[] result = CraftRecipe(input);
            if (result != null)
            {
                output.Add(result);
            }
        }
    }
}
