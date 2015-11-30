using ItemFramework;
using UnityEngine;
using UnityEngine.UI;

public class GuiHandler : MonoBehaviour
{
	public Text TinText;
	public Text CopperText;
	public Text BronzeText;
	public Text FuelText;
	public Text BurnProgressText;
	public Text ProgressText;
	public Furnace FurnaceScript;

	public void AddItems()
	{
		FurnaceScript.input.Add(new ItemStack(new ItemCopperOre(), 64));
		FurnaceScript.input.Add(new ItemStack(new ItemTinOre(), 16));
	}

	public void AddFuel()
	{
		FurnaceScript.fuel.Add(new ItemStack(new ItemCoal()));
	}

	void Update()
	{
		TinText.text = "Tin Ore : " + FurnaceScript.input.Contains(typeof(ItemTinOre)).ToString();
		CopperText.text = "Copper Ore : " + FurnaceScript.input.Contains(typeof(ItemCopperOre)).ToString();
		BronzeText.text = "Bronze Ingot : " + FurnaceScript.output.Contains(typeof(ItemBronzeIngot)).ToString();
		ItemStack fuelStack = FurnaceScript.fuel.Get(0);
		FuelText.text = "Fuel : " + (fuelStack != null ? fuelStack.Amount : 0).ToString();
		BurnProgressText.text = "Burn progress : " + Mathf.RoundToInt(FurnaceScript.BurnProgress * 100) + "%";
		ProgressText.text = "Progress : " + Mathf.RoundToInt(FurnaceScript.Progress * 100) + "%";
	}
}
