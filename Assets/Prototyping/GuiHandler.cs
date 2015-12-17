using System;
using ItemFramework;
using ItemFramework.Db;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GuiHandler : MonoBehaviour, IMod
{
	public Text TinText;
	public Text CopperText;
	public Text BronzeText;
	public Text FuelText;
	public Text BurnProgressText;
	public Text ProgressText;
	public Furnace FurnaceScript;

	void Awake()
	{
		FrameworkRegistry.RegisterMod(this);
		DbManager.Instance.Handler = new DbFileHandler("test.json");
	}

	public void AddItems()
	{
		FurnaceScript.Crafter.input.Add(new ItemStack(FrameworkRegistry.GetItem("CopperOre"), 64));
		FurnaceScript.Crafter.input.Add(new ItemStack(FrameworkRegistry.GetItem("TinOre"), 16));
	}

	public void AddFuel()
	{
		FurnaceScript.fuel.Add(new ItemStack(FrameworkRegistry.GetItem("Coal"), true, true));
	}

	void Update()
	{
		TinText.text = "Tin Ore : " + FurnaceScript.Crafter.input.Count(typeof(ItemTinOre)).ToString();
		CopperText.text = "Copper Ore : " + FurnaceScript.Crafter.input.Count(typeof(ItemCopperOre)).ToString();
		BronzeText.text = "Bronze Ingot : " + FurnaceScript.Crafter.output.Count(typeof(ItemBronzeIngot)).ToString();
		ItemStack fuelStack = FurnaceScript.fuel.Get(0);
		FuelText.text = "Fuel : " + (fuelStack != null ? fuelStack.Amount : 0).ToString();
		BurnProgressText.text = "Burn progress : " + Mathf.RoundToInt(FurnaceScript.BurnProgress * 100) + "%";
		ProgressText.text = "Progress : " + Mathf.RoundToInt(FurnaceScript.Progress * 100) + "%";
	}

	public Dictionary<string, Type> RegisterItems()
	{
		return new Dictionary<string, Type>()
		{
			{ "CopperOre", typeof(ItemCopperOre) },
			{ "TinOre", typeof(ItemTinOre) },
			{ "BronzeIngot", typeof(ItemBronzeIngot) },
			{ "Coal", typeof(ItemCoal) }
		};
	}

	public Type[] RegisterRecipes()
	{
		return new[] {
			typeof(RecipeBronzeIngot)
		};
	}
}
