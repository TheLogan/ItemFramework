using System;
using ItemFramework.Db;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Guid = System.Guid;
using String = System.String;
using Type = System.Type;

namespace ItemFramework
{
	/// <summary>
	/// Event for change in Container content
	/// </summary>
	public delegate void ContainerChangedEvent();

	/// <summary>
	/// Event to validate incomming ItemStack to Container
	/// </summary>
	/// <param name="index">Slot index</param>
	/// <param name="itemStack">The ItemStack to validate</param>
	/// <param name="args">Cancelable event - Set "args.Cancel = true;" if ItemStack shouldn't be added</param>
	public delegate void ContainerValidatorEvent(int index, ItemStack itemStack, CancelEventArgs args);

	[System.Serializable]
	[DbObject("containers")]
	public class Container : DbObject
	{
		/// <summary>
		/// Dictionary over Guid to Container
		/// </summary>
		private static Dictionary<Guid, Container> dict = new Dictionary<Guid, Container>();

		/// <summary>
		/// Id of Container
		/// </summary>
		public override Guid Id
		{
			get
			{
				return id;
			}

			internal set
			{
				if (value != Guid.Empty)
				{
					if (id != Guid.Empty)
					{
						dict.Remove(id);
						DbManager.Instance.Handler.Remove(this);
					}
					if (dict.ContainsKey(value))
					{
						dict.Remove(value);
					}
					dict.Add(value, this);
					id = value;
				}
			}
		}

		/// <summary>
		/// Array of ItemStack Ids in the Container
		/// </summary>
		[DbProperty("items")]
		private Guid?[] items;

		/// <summary>
		/// Get the array of ItemStack Ids in the Container
		/// </summary>
		public Guid?[] Items
		{
			get
			{
				if (items == null)
				{
					items = new Guid?[Slots];
				}
				return items;
			}
		}

		/// <summary>
		/// Get the array of ItemStacks in the Container
		/// </summary>
		public ItemStack[] ItemStacks
		{
			get
			{
				List<ItemStack> itemStacks = new List<ItemStack>();
				for (int i = 0, j = Items.Length; i < j; i++)
				{
					var item = Items[i];
					itemStacks.Add(item.HasValue ? ItemStack.GetById(item.Value) : null);
				}
				return itemStacks.ToArray();
			}
		}

		/// <summary>
		/// Event for change in Container content
		/// </summary>
		public event ContainerChangedEvent Changed;

		/// <summary>
		/// Event to validate incomming ItemStack to Container
		/// </summary>
		public event ContainerValidatorEvent Validator;

		/// <summary>
		/// Number of slots in Container
		/// </summary>
		[DbProperty("slots")]
		public int Slots { get; private set; }

		/// <summary>
		/// Width of the Container
		/// </summary>
		[DbProperty("width")]
		public int Width { get; set; }

		/// <summary>
		/// Height of the Container
		/// </summary>
		public int Height
		{
			get
			{
				return Mathf.CeilToInt(Slots / (float)Width);
			}
		}

		/// <summary>
		/// Construct new Container
		/// </summary>
		/// <param name="slots">Number of slots (default 20)</param>
		/// <param name="width">Width of Container (default 10)</param>
		/// <param name="id">Db Id of container</param>
		public Container(int slots = 20, int width = 10, Guid id = new Guid())
		{
			Slots = slots;
			Width = width;

			ItemStack.Empty += onItemStackEmpty;

			if (id == Guid.Empty)
			{
				SaveToDb();
			}
			else
			{
				this.Id = id;
			}
		}

		/// <summary>
		/// Called when an ItemStack empties
		/// </summary>
		/// <param name="itemStack"></param>
		private void onItemStackEmpty(ItemStack itemStack)
		{
			var id = itemStack.Id;
			if (id != Guid.Empty)
			{
				if (Items.Contains(id))
				{
					items[System.Array.IndexOf(items, id)] = null;
				}
			}
		}

		/// <summary>
		/// Add ItemStack to Container at specific slot.
		/// If it's already used, but samt Item, it will try to add the amount to the stack.
		/// </summary>
		/// <param name="index">Index of slot</param>
		/// <param name="stack">ItemStack to add</param>
		/// <returns>Excess or null if the whole ItemStack was added</returns>
		public ItemStack Add(int index, ItemStack stack)
		{
			if (stack == null)
			{
				return null;
			}

			if (stack.IsTemp)
			{
				stack = stack.GetPersistant();
			}

			//If the index is null just add the new items
			if (!Items[index].HasValue)
			{
				Items[index] = stack.GetPersistant().Id;
				return null;
			}
			else //If it's not null find a different index to place it at
			{
				ItemStack tmp = Get(index);
				if (tmp.Item.GetType() == stack.Item.GetType() && (tmp.Amount < tmp.Item.StackSize || !tmp.IsLimited))
				{
					int amountToAdd = stack.Amount;
					if (tmp.IsLimited)
					{
						amountToAdd = Mathf.Min(tmp.Item.StackSize - tmp.Amount, stack.Amount);
					}

					tmp.Amount += amountToAdd;
					stack.Amount -= amountToAdd;

					if (stack.Amount == 0)
					{
						return null;
					}
				}
			}

			return stack;
		}

		/// <summary>
		/// Check if ItemStacks can be added to the Container.
		/// If checking multiple, one ItemStack can make it all fail.
		/// </summary>
		/// <param name="stacks">ItemStacks to check</param>
		/// <returns>Whether the ItemStacks can be added.</returns>
		public bool CanAdd(params ItemStack[] stacks)
		{
			ItemStack[] clonedStacks = ItemStack.CloneMultiple(true, stacks);
			for (int i = 0, j = stacks.Length; i < j; i++)
			{
				ItemStack stack = clonedStacks[i];

				// First add to already existing stacks
				for (int k = 0, l = Items.Length; k < l; k++)
				{
					if (Items[k].HasValue)
					{
						ItemStack tempStack = Get(k);
						if (tempStack.Item.GetType() == stack.Item.GetType())
						{
							if (tempStack.Amount < tempStack.Item.StackSize)
							{
								int amountToAdd = Mathf.Min(tempStack.Item.StackSize - tempStack.Amount, stack.Amount);
								stack.Amount -= amountToAdd;

								if (stack.Amount == 0)
								{
									break;
								}
							}
						}
					}
				}

				// If the amount have been put into the Container, continue to next ItemStack
				if (stack.Amount == 0)
				{
					continue;
				}

				// Then find the first empty slot
				for (int k = 0, l = Items.Length; k < l; k++)
				{
					if (!Items[k].HasValue)
					{
						// Validate the ItemStack
						if (Validator != null)
						{
							var eventArgs = new CancelEventArgs();
							Validator(k, stack, eventArgs);
							if (eventArgs.Cancel)
							{
								continue;
							}
						}

						if (stack.Amount > stack.Item.StackSize)
						{
							stack.Amount -= stack.Item.StackSize;
						}
						else
						{
							stack.Amount = 0;
							break;
						}
					}
				}

				// If the amount have been put into the Container, continue to next ItemStack
				if (stack.Amount == 0)
				{
					continue;
				}

				ItemStack.TryDestroy(clonedStacks);
				return false;
			}

			ItemStack.TryDestroy(clonedStacks);
			return true;
		}

		/// <summary>
		/// Add ItemStacks to the Container.
		/// Fills already containing ItemStacks of same Item first.
		/// </summary>
		/// <param name="stacks">ItemStacks to add</param>
		/// <returns>Excess ItemStacks</returns>
		public ItemStack[] Add(params ItemStack[] stacks)
		{
			List<ItemStack> clonedStacks = new List<ItemStack>(ItemStack.CloneMultiple(true, stacks));
			bool containerChanged = false;
			for (int u = 0, j = clonedStacks.Count; u < j; u++)
			{
				ItemStack stack = clonedStacks[u];
				if (stack == null || stack.Item == null) continue;

				// First add to already existing stacks
				for (int i = 0, l = Items.Length; i < l; i++)
				{
					if (Items[i].HasValue)
					{
						ItemStack tempStack = Get(i);
						if (tempStack.Item.GetType() == stack.Item.GetType())
						{
							if (tempStack.Amount < tempStack.Item.StackSize)
							{
								int amountToAdd = Mathf.Min(tempStack.Item.StackSize - tempStack.Amount, stack.Amount);
								tempStack.Amount += amountToAdd;
								stack.Amount -= amountToAdd;
								containerChanged = true;

								if (stack.Item == null || stack.Amount == 0)
								{
									break;
								}
							}
						}
					}
				}

				// If the amount have been put into the Container, continue to next ItemStack
				if (stack.Item == null || stack.Amount == 0)
				{
					continue;
				}

				// Then find the first empty slot
				for (int k = 0, l = Items.Length; k < l; k++)
				{
					if (!Items[k].HasValue)
					{
						// Validate the ItemStack
						if (Validator != null)
						{
							var eventArgs = new CancelEventArgs();
							Validator(k, stack, eventArgs);
							if (eventArgs.Cancel)
							{
								continue;
							}
						}

						if (stack.Amount > stack.Item.StackSize)
						{
							Items[k] = new ItemStack(stack.Item, stack.Item.StackSize).Id;
							stack.Amount -= stack.Item.StackSize;
						}
						else
						{
							Items[k] = stack.GetPersistant().Id;
							containerChanged = true;
							stack.Amount = 0;
							break;
						}
					}
				}
			}
			ItemStack.TryDestroy(stacks);
			if (containerChanged)
			{
				if (Changed != null)
				{
					Changed.Invoke();
					SaveToDb();
				}
			}
			return clonedStacks.ToArray();
		}

		/// <summary>
		/// Replaces an ItemStack at a given index with a new ItemStack
		/// </summary>
		/// <param name="index">Slot index</param>
		/// <param name="type">ItemStack to replace it with</param>
		/// <returns>Current ItemStack at given index</returns>
		public ItemStack Replace(int index, ItemStack type)
		{
			ItemStack tempStack = Get(index);
			Items[index] = type.Id;
			if (Changed != null)
			{
				Changed.Invoke();
			}
			return tempStack;
		}

		/// <summary>
		/// Removes an ItemStack at a given index
		/// </summary>
		/// <param name="index">Slot index</param>
		/// <returns>ItemStack at given index</returns>
		public ItemStack Remove(int index)
		{
			ItemStack tempStack = Get(index);
			Items[index] = null;
			if (Changed != null)
			{
				Changed.Invoke();
				SaveToDb();
			}
			return tempStack;
		}

		/// <summary>
		/// Removes a set amount from an ItemStack at a given index and returns them as a new ItemStack
		/// </summary>
		/// <param name="index">Slot index</param>
		/// <param name="amount">Amount to remove from ItemStack</param>
		/// <returns>ItemStack containing the removed items</returns>
		public ItemStack Remove(int index, int amount)
		{
			//If there are no items to remove, remove no items
			if (!Items[index].HasValue)
			{
				return null;
			}

			//If asked to remove the amount or more of the stack, just remove the whole stack
			var tempStack = Get(index);
			if (amount >= tempStack.Amount)
			{
				Items[index] = null;

				if (Changed != null)
				{
					Changed.Invoke();
					SaveToDb();
				}

				return tempStack;
			}

			//If asked to remove part of the stack, create a new stack of the requested amount and remove said amount from the original stack
			var returnStack = new ItemStack
			{
				Item = tempStack.Item,
				Amount = amount
			};

			tempStack.Amount -= amount;

			if (Changed != null)
			{
				Changed.Invoke();
				SaveToDb();
			}

			return returnStack;
		}

		/// <summary>
		/// Remove ItemStacks from the Container.
		/// </summary>
		/// <param name="stacks">ItemStacks to remove</param>
		/// <returns>Removed ItemStacks</returns>
		public ItemStack[] Remove(params ItemStack[] stacks)
		{
			bool containerChanged = false;
			List<ItemStack> removedItemStacks = new List<ItemStack>();
			for (int i = 0, j = stacks.Length; i < j; i++)
			{
				var stack = stacks[i];
				if (stack == null || stack.Amount <= 0) continue;

				ItemStack removedItemStack = new ItemStack(stack.Item, 1, false, true);

				for (int k = 0, l = Items.Length; k < l; k++)
				{
					var tempStack = Get(k);
					if (tempStack != null && tempStack.Item.GetType() == stack.Item.GetType())
					{
						int amountToRemove = Mathf.Min(tempStack.Amount, stack.Amount);
						tempStack.Amount -= amountToRemove;
						stack.Amount -= amountToRemove;
						removedItemStack.Amount += amountToRemove;
						containerChanged = true;
						if (tempStack.Amount == 0)
						{
							Items[k] = null;
						}
						if (stack.Amount == 0)
						{
							break;
						}
					}
				}

				removedItemStack.Amount--;

				removedItemStacks.Add(removedItemStack.GetPersistant());
			}

			if (containerChanged && Changed != null)
			{
				Changed.Invoke();
				SaveToDb();
			}

			return removedItemStacks.ToArray();
		}

		/// <summary>
		/// Removes a set amount of a given item from the container
		/// </summary>
		/// <param name="item">Item type</param>
		/// <param name="amount">Amount to remove</param>
		/// <returns>ItemStack containing the removed items</returns>
		public ItemStack Remove(Item item, int amount)
		{
			bool containerChanged = false;

			ItemStack removedItemStack = new ItemStack(item, 1, false, true);

			for (int k = 0, l = Items.Length; k < l; k++)
			{
				var tempStack = Get(k);
				if (tempStack != null && tempStack.Item.GetType() == item.GetType())
				{
					int amountToRemove = Mathf.Min(tempStack.Amount, amount);
					tempStack.Amount -= amountToRemove;
					amount -= amountToRemove;
					removedItemStack.Amount += amountToRemove;
					containerChanged = true;
					if (tempStack.Amount == 0)
					{
						Items[k] = null;
					}
					if (amount == 0)
					{
						break;
					}
				}
			}

			removedItemStack.Amount--;

			if (containerChanged && Changed != null)
			{
				Changed.Invoke();
				SaveToDb();
			}

			return removedItemStack.GetPersistant();
		}


		/// <summary>
		/// Check if the Container contains Item.
		/// </summary>
		/// <param name="t">Type of item to count</param>
		/// <returns>Whether the Container contains Item</returns>
		public bool Contains(Type t)
		{
			return Items.Any(x => x.HasValue && ItemStack.GetById(x.Value).Item.GetType() == t);
		}

		/// <summary>
		/// Get amount of Item.
		/// </summary>
		/// <param name="t">Type of item to count</param>
		/// <returns>Amount of contained Item</returns>
		public int Count(Type t)
		{
			return Items.Where(x => x.HasValue && ItemStack.GetById(x.Value).Item.GetType() == t).Sum(x => ItemStack.GetById(x.Value).Amount);
		}

		/// <summary>
		/// Get ItemStack at a given index.
		/// </summary>
		/// <param name="index">Slot index</param>
		/// <returns>ItemStack at given index</returns>
		public ItemStack Get(int index)
		{
			if (index < 0 || index >= Items.Length) return null;
			ItemStack itemStack = null;
			if (Items[index].HasValue)
			{
				itemStack = ItemStack.GetById(Items[index].Value);
			}
			return itemStack;
		}

		/// <summary>
		/// Get all ItemStacks (except nulls) in Container.
		/// </summary>
		/// <returns>ItemStacks in Container</returns>
		public ItemStack[] GetAllItemStacks()
		{
			List<ItemStack> itemStacks = new List<ItemStack>();
			for (int i = 0, j = Items.Length; i < j; i++)
			{
				var item = Items[i];
				if (item.HasValue)
				{
					itemStacks.Add(ItemStack.GetById(item.Value));
				}
			}
			return itemStacks.ToArray();
		}

		/// <summary>
		/// Load Container with specific Id from Db
		/// </summary>
		/// <param name="id">Id of Container</param>
		/// <returns>Container if found; otherwise null</returns>
		public static Container LoadFromDb(Guid id)
		{
			object obj = DbManager.Instance.Handler.Load(id);

			if (obj is Container)
			{
				return (Container)obj;
			}

			return null;
		}

		/// <summary>
		/// Debugable string for Container
		/// </summary>
		/// <returns>Debugable string</returns>
		public override string ToString()
		{
			ItemStack[] itemStacks = GetAllItemStacks();
			string[] itemStackStrings = new string[itemStacks.Length];
			for (int i = 0, j = itemStacks.Length; i < j; i++)
			{
				itemStackStrings[i] = itemStacks[i].ToString();
			}
			return "Container[Id=" + Id + ",Items=" + String.Join(",", itemStackStrings) + "]";
		}
	}
}