using System;
using System.Collections;
using ItemFramework;

public class Furnace : Crafter {

	void Start()
	{
		input.Changed += OnInputChanged;
	}

	private void OnInputChanged()
	{
		ItemStack[] result = CraftRecipe(input);
		if (result != null)
		{
			output.Add(result);
		}
	}
}
