﻿using System.ComponentModel;
using ItemFramework;
using System.Collections;
using UnityEngine;

public class Furnace : CrafterComponent
{
	private const float TICK_TIME = 0.05f;
	public Container fuel;

	void Start()
	{
		Crafter = new FurnaceCrafter();
		Crafter.input.Validator += OnInputValidate;
		Crafter.input.Changed += OnInputChanged;

		fuel = new Container(1);
		fuel.Validator += OnFuelValidate;
		fuel.Changed += OnFuelChanged;
	}

	private void OnInputValidate(int index, ItemStack itemStack, CancelEventArgs args)
	{
		// If the Item do not inherit the abstract ItemOre class, cancel
		if (!(itemStack.Item is ItemOre))
		{
			args.Cancel = true;
		}
	}

	private void OnFuelValidate(int index, ItemStack itemStack, CancelEventArgs args)
	{
		// If the Item do not implement the IBurnable interface, cancel
		if (!(itemStack.Item is IBurnable))
		{
			args.Cancel = true;
		}
	}

	public FurnaceRecipe CurrentRecipe { get; private set; }
	public float ProgressTimeElapsed { get; private set; }
	public float Progress { get; private set; }
	public float BurnTime { get; private set; }
	public float BurnProgress { get; private set; }

	private void TryCraft()
	{
		if (Crafter.input.GetAllItemStacks().Length > 0)
		{
			if (BurnTime == 0)
			{
				ItemStack fuelStack = fuel.Get(0);
				if (fuelStack != null && fuelStack.Amount > 0)
				{
					FurnaceRecipe recipe = (FurnaceRecipe)Crafter.GetFirstRecipe();
					if (recipe != null && ((FurnaceCrafter)Crafter).output.CanAdd(recipe.Output))
					{
						CurrentRecipe = recipe;
						StartCoroutine("Burn", ((IBurnable)fuelStack.Item).BurnTime);
						fuelStack.Amount--;
					}
				}
			}
			else if (CurrentRecipe == null)
			{
				FurnaceRecipe recipe = (FurnaceRecipe)Crafter.GetFirstRecipe();
				if (recipe != null && ((FurnaceCrafter)Crafter).output.CanAdd(recipe.Output))
				{
					CurrentRecipe = recipe;
				}
			}
		}
	}

	private void OnInputChanged()
	{
		TryCraft();
	}

	private void OnFuelChanged()
	{
		TryCraft();
	}

	private IEnumerator Burn(int burntime)
	{
		this.BurnTime += burntime;
		this.BurnProgress = 1;
		while (this.BurnTime > 0)
		{
			yield return new WaitForSeconds(TICK_TIME);
			if (this.CurrentRecipe != null)
			{
				this.ProgressTimeElapsed += TICK_TIME;
				if (this.ProgressTimeElapsed >= this.CurrentRecipe.ProgressTime)
				{
					ItemStack[] result = Crafter.CraftRecipe();
					if (result != null)
					{
						((FurnaceCrafter)Crafter).output.Add(result);
					}
					this.ProgressTimeElapsed -= this.CurrentRecipe.ProgressTime;
					this.CurrentRecipe = null;

					FurnaceRecipe recipe = (FurnaceRecipe)Crafter.GetFirstRecipe();
					if (recipe != null && ((FurnaceCrafter)Crafter).output.CanAdd(recipe.Output))
					{
						CurrentRecipe = recipe;
					}
					else
					{
						this.ProgressTimeElapsed = 0;
						this.Progress = 0;
					}
				}
				this.Progress = this.ProgressTimeElapsed / Mathf.Max(this.CurrentRecipe != null ? this.CurrentRecipe.ProgressTime : 1, 1);
			}
			this.BurnTime -= TICK_TIME;
			this.BurnProgress = this.BurnTime / burntime;
		}
		if (CurrentRecipe != null)
		{
			ItemStack fuelStack = fuel.Get(0);
			if (fuelStack != null && fuelStack.Amount > 0)
			{
				StartCoroutine("Burn", ((IBurnable)fuelStack.Item).BurnTime);
				fuelStack.Amount--;
			}
			else
			{
				this.CurrentRecipe = null;
				this.Progress = 0;
				this.ProgressTimeElapsed = 0;
			}
		}
		else
		{
			this.CurrentRecipe = null;
			this.Progress = 0;
			this.ProgressTimeElapsed = 0;
		}
	}
}
