using System.Linq;
using System.ComponentModel;
using ItemFramework;
using Debug = UnityEngine.Debug;
using System.Collections;
using UnityEngine;

public class Furnace : Crafter
{
	private const float TICK_TIME = 0.05f;
	public Container fuel;

	void Start()
	{
		input = new Container();
		output = new Container(1);
		fuel = new Container(1);
		input.Validator += OnInputValidate;
		input.Changed += OnInputChanged;
		fuel.Validator += OnFuelValidate;
		fuel.Changed += OnFuelChanged;
	}

	private void OnInputValidate(ItemStack itemStack, CancelEventArgs args)
	{
		if (!(itemStack.Item is ItemOre))
		{
			args.Cancel = true;
		}
	}

	private void OnFuelValidate(ItemStack itemStack, CancelEventArgs args)
	{
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

	protected override CraftingRecipe[] GetRecipes()
	{
		return CraftingManager.Instance.Recipes.Where(x => x is FurnaceRecipe).ToArray();
	}

	private void TryCraft()
	{
		if (input.GetAll().Length > 0)
		{
			if (BurnTime == 0)
			{
				ItemStack fuelStack = fuel.Get(0);
				if (fuelStack != null && fuelStack.Amount > 0)
				{
					FurnaceRecipe recipe = (FurnaceRecipe)GetFirstRecipe(input);
					if (recipe != null && output.CanAdd(recipe.Output))
					{
						CurrentRecipe = recipe;
						StartCoroutine("Burn", ((IBurnable)fuelStack.Item).BurnTime);
						fuelStack.Amount--;
					}
				}
			}
			else if (CurrentRecipe == null)
			{
				FurnaceRecipe recipe = (FurnaceRecipe)GetFirstRecipe(input);
				if (recipe != null && output.CanAdd(recipe.Output))
				{
					CurrentRecipe = recipe;
				}
			}
		}
	}

	private void OnInputChanged()
	{
		//Debug.Log("Input changed => " + input.ToString());
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
					ItemStack[] result = CraftRecipe(input);
					if (result != null)
					{
						output.Add(result);
					}
					this.ProgressTimeElapsed -= this.CurrentRecipe.ProgressTime;
					this.CurrentRecipe = null;

					FurnaceRecipe recipe = (FurnaceRecipe)GetFirstRecipe(input);
					if (recipe != null && output.CanAdd(recipe.Output))
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
