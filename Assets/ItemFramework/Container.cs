using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Guid = System.Guid;
using String = System.String;
using Type = System.Type;
using Serializable = System.SerializableAttribute;
using System.ComponentModel;

namespace ItemFramework
{
	public delegate void ContainerChangedEvent();

	public delegate void ContainerValidatorEvent(ItemStack itemStack, CancelEventArgs args);

	[Serializable]
	public class Container
	{
		private static Dictionary<Guid, Container> dict = new Dictionary<Guid, Container>();

		public static Container GetContainerById(Guid id)
		{
			if (id == Guid.Empty)
			{
				Debug.LogWarning("Trying to get Container with empty Guid");
			}
			return dict[id];
		}

		private Guid id;
		private Guid?[] items;

		public Guid Id
		{
			get
			{
				return id;
			}
			set
			{
				if (value != Guid.Empty)
				{
					if (id != Guid.Empty)
					{
						dict.Remove(id);
					}
					dict.Add(value, this);
					id = value;
				}
			}
		}

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

		public ItemStack[] ItemStacks
		{
			get
			{
				List<ItemStack> itemStacks = new List<ItemStack>();
				foreach (var item in Items)
				{
					itemStacks.Add(item.HasValue ? ItemStack.GetItemStackById(item.Value) : null);
				}
				return itemStacks.ToArray();
			}
		}

		public event ContainerValidatorEvent Validator;
		public event ContainerChangedEvent Changed;

		public int Slots { get; private set; }
		public int Width { get; set; }

		public Container(int slots = 20)
		{
			Slots = slots;

			ItemStack.Empty += ItemStack_Empty;
		}

		private void ItemStack_Empty(Guid itemStackId)
		{
			if (Items.Contains(itemStackId))
			{
				items[System.Array.IndexOf(items, itemStackId)] = null;
			}
		}

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
			if (Items[index].HasValue)
			{
				Items[index] = type.Id;
			}
			else //If it's not null find a different index to place it at
			{
				var indexes = new List<int>();
				for (var i = 0; i < Items.Length; i++)
				{
					//Find all empty places in the inventory
					if (Items[i].HasValue)
						indexes.Add(i);
				}
				//If there are no empty places, just return the itemStack.
				if (indexes.Count == 0)
					return type;

				var closest = indexes.Aggregate((x, y) => Mathf.Abs(x - index) < Mathf.Abs(y - index) ? x : y); //Find the closest empty index
				Items[closest] = type.Id;
				return null;
			}
			return null;
		}

		public bool CanAdd(params ItemStack[] stacks)
		{
			ItemStack[] clonedStacks = ItemStack.CloneMultiple(stacks);
			for (int i = 0, j = stacks.Length; i < j; i++)
			{
				ItemStack stack = clonedStacks[i];

				// Validate the ItemStack
				if (Validator != null)
				{
					var eventArgs = new CancelEventArgs();
					Validator(stack, eventArgs);
					if (eventArgs.Cancel)
					{
						return false;
					}
				}

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
		/// Add item stack to the first empty slot in inventory
		/// If there are no empty slots, returns the item stack.
		/// </summary>
		/// <param name="stacks"></param>
		/// <returns></returns>
		public ItemStack[] Add(params ItemStack[] stacks)
		{
			List<ItemStack> clonedStacks = new List<ItemStack>(ItemStack.CloneMultiple(stacks));
			bool containerChanged = false;
			foreach (ItemStack stack in clonedStacks)
			{
				// Validate the ItemStack
				if (Validator != null)
				{
					var eventArgs = new CancelEventArgs();
					Validator(stack, eventArgs);
					if (eventArgs.Cancel)
					{
						continue;
					}
				}

				// First add to already existing stacks
				for (int i = 0; i < Items.Length; i++)
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
				for (int i = 0; i < Items.Length; i++)
				{
					if (!Items[i].HasValue)
					{
						if (stack.Amount > stack.Item.StackSize)
						{
							Items[i] = new ItemStack(stack.Item, stack.Item.StackSize).Id;
							stack.Amount -= stack.Item.StackSize;
						}
						else
						{
							Items[i] = stack.Clone().Id;
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
				}
			}
			return clonedStacks.ToArray();
		}

		/// <summary>
		/// Replaces an item stack at a given index with a new item stack
		/// </summary>
		/// <param name="index"></param>
		/// <param name="type"></param>
		/// <returns></returns>
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
		/// Takes an index and returns what's in that slot
		/// </summary>
		/// <param name="index">The inventory index to look in</param>
		/// <returns></returns>
		public ItemStack Remove(int index)
		{
			ItemStack tempStack = Get(index);
			Items[index] = null;
			if (Changed != null)
			{
				Changed.Invoke();
			}
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
			if (Items[index].HasValue)
				return null;

			//If asked to remove the amount or more of the stack, just remove the whole stack
			var tempStack = Get(index);
			if (amount >= tempStack.Amount)
			{
				Items[index] = null;
				if (Changed != null)
				{
					Changed.Invoke();
				}
				return tempStack;
			}

			//If asked to remove part of the stack, create a new stack of the requested amount and remove said amount from the original stack
			var returnStack = new ItemStack
			{
				Item = new Item { Name = tempStack.Item.Name },
				Amount = amount
			};

			tempStack.Amount -= amount;
			if (Changed != null)
			{
				Changed.Invoke();
			}
			return tempStack;
		}

		/// <summary>
		/// find an itemstack matching current type and return it
		/// </summary>
		/// <param name="stacks"></param>
		/// <returns></returns>
		public ItemStack[] Remove(params ItemStack[] stacks)
		{
			bool containerChanged = false;
			List<ItemStack> removedItemStacks = new List<ItemStack>();
			foreach (ItemStack stack in stacks)
			{
				if (stack.Amount <= 0) continue;

				ItemStack removedItemStack = new ItemStack(stack.Item, 0);

				for (var i = 0; i < Items.Length; i++)
				{
					var tempStack = Get(i);
					if (tempStack != null && tempStack.Item.GetType() == stack.Item.GetType())
					{
						int amountToRemove = Mathf.Min(tempStack.Amount, stack.Amount);
						tempStack.Amount -= amountToRemove;
						stack.Amount -= amountToRemove;
						removedItemStack.Amount += amountToRemove;
						containerChanged = true;
						if (tempStack.Amount == 0)
						{
							Items[i] = null;
						}
						if (stack.Amount == 0)
						{
							break;
						}
					}
				}

				removedItemStacks.Add(removedItemStack);
			}

			if (containerChanged && Changed != null)
			{
				Changed.Invoke();
			}

			return removedItemStacks.ToArray();
		}


		public bool Contains(ItemStack stack)
		{
			return Items.Any(x => x.HasValue && ItemStack.GetItemStackById(x.Value).Item.Name == stack.Item.Name);
		}


		public int Contains(Type t)
		{
			return Items.Where(x => x.HasValue && ItemStack.GetItemStackById(x.Value).Item.GetType() == t).Sum(x => ItemStack.GetItemStackById(x.Value).Amount);
			//            return tempStackId.HasValue ? ItemStack.GetItemStackById(tempStackId.Value).Amount : 0;
		}

		public ItemStack Get(int index)
		{
			ItemStack itemStack = null;
			if (Items[index].HasValue)
			{
				itemStack = ItemStack.GetItemStackById(Items[index].Value);
			}
			return itemStack;
		}

		public ItemStack[] GetAll()
		{
			List<ItemStack> itemStacks = new List<ItemStack>();
			foreach (var item in Items)
			{
				if (item.HasValue)
				{
					itemStacks.Add(ItemStack.GetItemStackById(item.Value));
				}
			}
			return itemStacks.ToArray();
		}

		public override string ToString()
		{
			ItemStack[] itemStacks = GetAll();
			string[] itemStackStrings = new string[itemStacks.Length];
			for (int i = 0, j = itemStacks.Length; i < j; i++)
			{
				itemStackStrings[i] = itemStacks[i].ToString();
			}
			return "Container[" + String.Join(",", itemStackStrings) + "]";
		}
	}
}