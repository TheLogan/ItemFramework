using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Container : MonoBehaviour {

	public int Id { get; set; }
	public ItemStack[]	Items { get; set; }
	public int Width { get; set; }

	/// <summary>
	/// Will attempt to add itemstack to the index, if the index is empty it will succeed,
	/// if the index is not empty it will attempt to place at the nearest empty, if there are non it will fail,
	/// if you rather want to replace the current stack at index with a new one, use Replace.
	/// Returns unplacable items, usually because there are not enough empty spaces
	/// </summary>
	/// <param name="index">The index in the inventory the item should be placed on</param>
	/// <param name="type">The item type and amount in the shape of an item stack</param>
	/// <returns></returns>
	public ItemStack Add(int index, ItemStack type)
	{ //TODO take max stack sizes into account

		//If the index is null just add the new items
		if (Items[index] == null) 
		{
			Items[index] = type;
		}
		else //If it's not null find a different index to place it at
		{
			var indexes = new List<int>();
			for(var i = 0; i<Items.Length; i++)
			{
				//Find all empty places in the inventory
				if(Items[i] == null)
					indexes.Add(i);
			}
			//If there are no empty places, just return the itemStack.
			if (indexes.Count == 0)
				return type;

			var closest = indexes.Aggregate((x, y) => Mathf.Abs(x - index) < Mathf.Abs(y - index) ? x : y); //Find the closest empty index
			Items[closest] = type;
			return null;
		}
		return null;
	}

	/// <summary>
	/// Add item stack to the first empty slot in inventory
	/// If there are no empty slots, returns the item stack.
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public ItemStack Add(ItemStack type)
	{
		for (int i = 0; i < Items.Length; i++)
		{
			if (Items[i] == null)
			{
				Items[i] = type;
				return null;
			}
		}
		return type;
	}

	/// <summary>
	/// Replaces an item stack at a given index with a new item stack
	/// </summary>
	/// <param name="index"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public ItemStack Replace(int index, ItemStack type)
	{
		var tempStack = Items[index];
		Items[index] = type;
		return tempStack;
	}

	/// <summary>
	/// Takes an index and returns what's in that slot
	/// </summary>
	/// <param name="index">The inventory index to look in</param>
	/// <returns></returns>
	public ItemStack Remove(int index)
	{
		var tempStack = Items[index];
		Items[index] = null;
		return tempStack;
	}

	/// <summary>
	/// Removes a set amount of items from a stack and returns them as a new stack
	/// </summary>
	/// <param name="index"></param>
	/// <param name="amount"></param>
	/// <returns></returns>
	public ItemStack Remove(int index, int amount)
	{
		//If there are no items to remove, remove no items
		if (Items[index] == null)
			return null;

		//If asked to remove the amount or more of the stack, just remove the whole stack
		ItemStack tempStack;
		if (amount >= Items[index].Amount)
		{
			tempStack = Items[index];
			Items[index] = null;
			return tempStack;
		}
		
		//If asked to remove part of the stack, create a new stack of the requested amount and remove said amount from the original stack
		tempStack = new ItemStack
		{
			Item = new Item { Name = Items[index].name },
			Amount = amount
		};

		Items[index].Amount -= amount;
		return tempStack;
	}
	
	/// <summary>
	/// find an itemstack matching current type and return it
	/// </summary>
	/// <param name="removeable"></param>
	/// <returns></returns>
	public ItemStack Remove(ItemStack removeable)
	{
		var i = 0;
		ItemStack tempStack = null;
		for (i = 0; i < Items.Length; i++)
		{
			if (Items[i].name == removeable.name)
			{
				tempStack = Items[i];
				break;
			}
		}
		if (tempStack == null)
			return null;

		if (removeable.Amount >= tempStack.Amount)
		{
			Items[i] = null;
			return tempStack;
		}

		Items[i].Amount -= removeable.Amount;
		return removeable;
	}
	
	
	public bool Contains(ItemStack stack)
	{
		return Items.Any(x => x.Item.name == stack.name);
	}


	public int Contains(Item type)
	{
		var tempStack = Items.FirstOrDefault(itemStack => itemStack.name == type.name);
		return tempStack == null ? 0 : tempStack.Amount;
	}

}
