using ItemFramework.Db;
using System.Collections.Generic;
using UnityEngine;
using Guid = System.Guid;
using System;
using System.ComponentModel;

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

		/// <summary>
		/// Get ItemStack by Id
		/// </summary>
		/// <param name="id">ItemStack Id</param>
		/// <returns>ItemStack if found, otherwise null</returns>
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
				if (!stacks[i].IsLocked)
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
		[DefaultValue(false)]
		private bool isLocked;
		[DbProperty("limited")]
		[DefaultValue(true)]
		private bool isLimited = true;
		private bool isTemp;

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
				if (value != null)
				{
					if (amount == 0)
					{
						Amount = 1;
					}
					else if (isLimited && amount > value.StackSize)
					{
						Amount = value.StackSize;
					}
				}
				else
				{
					amount = 0;

					if (Empty != null)
					{
						Empty.Invoke(Id);
					}

					DbManager.Instance.Handler.Delete(this);

					if (Id != Guid.Empty)
					{
						dict.Remove(Id);
					}
				}
				item = value;
				if (!IsTemp)
				{
					DbManager.Instance.Handler.Save();
				}
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
					value = item.StackSize;
				}
				if (value == 0)
				{
					item = null;

					if (Empty != null)
					{
						Empty.Invoke(Id);
					}

					DbManager.Instance.Handler.Delete(this);

					if (Id != Guid.Empty)
					{
						dict.Remove(Id);
					}
				}
				amount = value;
				if (!IsTemp)
				{
					DbManager.Instance.Handler.Save();
				}
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
				if (!IsTemp)
				{
					DbManager.Instance.Handler.Save();
				}
			}
		}

		public bool IsLocked
		{
			get
			{
				return isLocked;
			}
		}

		public bool IsTemp
		{
			get
			{
				return isTemp;
			}
		}

		public ItemStack(bool temp = false)
		{
			if (!temp)
			{
				SaveToDb();
			}
			isTemp = temp;
		}

		public ItemStack(Item item, int amount = 1, bool isLocked = false, bool temp = false) : this(temp)
		{
			Amount = amount;
			Item = item;
			this.isLocked = isLocked;
		}

		public ItemStack(Item item, bool isLocked, bool temp) : this(item, 1, isLocked, temp) { }

		public void SetLocked()
		{
			isLocked = true;
			if (!IsTemp)
			{
				DbManager.Instance.Handler.Save();
			}
		}

		public ItemStack Clone(bool temp = false)
		{
			ItemStack clone = new ItemStack(Item, Amount, false, temp);
			// Apply amount again if not limited, as ItemStacks are defaulted to limited
			if (!isLimited)
			{
				clone.IsLimited = false;
				clone.Amount = amount;
			}
			return clone;
		}

		public ItemStack GetPersistant()
		{
			if (IsTemp)
			{
				return Clone();
			}
			return this;
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