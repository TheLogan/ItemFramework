using System.Collections.Generic;
using UnityEngine;
using Guid = System.Guid;

namespace ItemFramework
{
	public class ItemStack
	{
		private static Dictionary<Guid, ItemStack> dict = new Dictionary<Guid, ItemStack>();

		public static ItemStack GetItemStackById(Guid id)
		{
			if (id == Guid.Empty)
			{
				Debug.LogWarning("Trying to get ItemStack with empty Guid");
			}
			return dict[id];
		}

		/// <summary>
		/// Make clones of multiple ItemStacks
		/// </summary>
		/// <param name="stacks">ItemStacks</param>
		/// <returns>Cloned ItemStacks</returns>
		public static ItemStack[] CloneMultiple(params ItemStack[] stacks)
		{
			ItemStack[] clonedStacks = new ItemStack[stacks.Length];
			for (int i = 0, j = stacks.Length; i < j; i++)
			{
				clonedStacks[i] = stacks[i].Clone();
			}
			return clonedStacks;
		}

		/// <summary>
		/// Try to destroy ItemStacks
		/// </summary>
		/// <param name="stacks">ItemStacks</param>
		public static void TryDestroy(params ItemStack[] stacks)
		{
			for (int i = 0, j = stacks.Length; i < j; i++)
			{
				if (!stacks[i].IsLocked())
				{
					stacks[i].Amount = 0;
					stacks[i].Item = null;
					dict.Remove(stacks[i].Id);
				}
			}
		}

		/// <summary>
		/// Lock multiple ItemStacks
		/// </summary>
		/// <param name="stacks">ItemStacks</param>
		public static void LockMultiple(params ItemStack[] stacks)
		{
			for (int i = 0, j = stacks.Length; i < j; i++)
			{
				stacks[i].SetLocked();
			}
		}

		private bool isLocked;
		private bool isLimited = true;
		private Guid id;

		public Guid Id
		{
			get
			{
				return id;
			}
			set
			{
				if (isLocked)
				{
					throw new System.InvalidOperationException("Can't modify locked ItemStack");
				}
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
		private Item item;
		private int amount;

		public Item Item
		{
			get
			{
				return item;
			}

			set
			{
				if (isLocked)
				{
					throw new System.InvalidOperationException("Can't modify locked ItemStack");
				}
				if (amount == 0)
				{
					Amount = 1;
				}
				else if (isLimited && amount > value.StackSize)
				{
					Amount = value.StackSize;
				}
				item = value;
			}
		}

		public int Amount
		{
			get
			{
				return amount;
			}

			set
			{
				if (isLocked)
				{
					throw new System.InvalidOperationException("Can't modify locked ItemStack");
				}
				if (isLimited && item != null && item.StackSize < value)
				{
					amount = item.StackSize;
					return;
				}
				if (value == 0)
				{
					item = null;
				}
				amount = value;
			}
		}

		public bool IsLimited
		{
			get
			{
				return isLimited;
			}

			set
			{
				if (value)
				{
					if (item != null && item.StackSize < Amount)
					{
						// Ignore isLocked
						amount = item.StackSize;
					}
				}
				isLimited = value;
			}
		}

		public ItemStack()
		{
			Id = Guid.NewGuid();
		}

		public ItemStack(Item item, int amount = 1, bool locked = false)
		{
			Amount = amount;
			Item = item;
			Id = Guid.NewGuid();
			this.isLocked = locked;
		}

		public void SetLocked()
		{
			isLocked = true;
		}

		public bool IsLocked()
		{
			return isLocked;
		}

		public ItemStack Clone()
		{
			return new ItemStack(Item, Amount);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ItemStack)) return false;
			var other = (ItemStack)obj;
			return Item.GetType() == other.Item.GetType() && Amount == other.Amount;
		}

		public override string ToString()
		{
			return "ItemStack[" + Item.ToString() + ",Amount=" + Amount + "]";
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = (int)2166136261;
				hash = hash * 16777619 ^ Item.GetHashCode();
				hash = hash * 16777619 ^ Amount.GetHashCode();
				return hash;
			}
		}
	}
}