﻿using ItemFramework.Db;
using System.Collections.Generic;
using UnityEngine;
using Guid = System.Guid;

namespace ItemFramework
{
	public delegate void ItemStackEmptyEvent(Guid itemStackId);

	[DbObject("itemstack")]
	public class ItemStack : DbObject
	{
		private static Dictionary<Guid, ItemStack> dict = new Dictionary<Guid, ItemStack>();
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
						DbManager.Instance.Handler.Delete(this);
					}
					dict.Add(value, this);
					id = value;
				}
			}
		}

		public static ItemStack GetById(Guid id)
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
		public static ItemStack[] CloneMultiple(bool temp = false, params ItemStack[] stacks)
		{
			ItemStack[] clonedStacks = new ItemStack[stacks.Length];
			for (int i = 0, j = stacks.Length; i < j; i++)
			{
				clonedStacks[i] = stacks[i].Clone(temp);
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

		[DbProperty("locked")]
		private bool isLocked;
		[DbProperty("limited")]
		private bool isLimited = true;

		[DbProperty("item")]
		private Item item;
		[DbProperty("amount")]
		private int amount;

		public static event ItemStackEmptyEvent Empty;

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
				if (value == null)
				{
					if (Empty != null)
					{
						Empty.Invoke(Id);
					}
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
					if (Empty != null)
					{
						Empty.Invoke(Id);
					}
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

		public ItemStack(bool temp = false)
		{
			if (!temp)
			{
				SaveToDb();
			}
		}

		public ItemStack(Item item, bool isLocked, bool temp) : this(item, 1, isLocked, temp) { }

		public ItemStack(Item item, int amount = 1, bool isLocked = false, bool temp = false) : this(temp)
		{
			Amount = amount;
			Item = item;
			this.isLocked = isLocked;
		}

		public void SetLocked()
		{
			isLocked = true;
		}

		public bool IsLocked()
		{
			return isLocked;
		}

		public ItemStack Clone(bool temp = false)
		{
			return new ItemStack(Item, Amount, false, temp);
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (!(obj is ItemStack)) return false;
			var other = (ItemStack)obj;
			if ((Item == null && other.Item != null) || (Item != null && other.Item == null)) return false;
			return ((Item == null && other.Item == null) || (Item.GetType() == other.Item.GetType())) && Amount == other.Amount;
		}

		public override string ToString()
		{
			return "ItemStack[Id=" + Id + ",Item=" + Item.ToString() + ",Amount=" + Amount + "]";
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