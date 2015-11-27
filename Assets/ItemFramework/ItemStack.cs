using System.Collections.Generic;
using UnityEngine;
using Guid = System.Guid;

namespace ItemFramework {
    public class ItemStack
	{
        private static Dictionary<Guid, ItemStack> dict = new Dictionary<Guid, ItemStack>();
		
        public static ItemStack GetItemStackById(Guid id) {
            if (id == Guid.Empty) {
                Debug.LogWarning("Trying to get ItemStack with empty Guid");
            }
            return dict[id];
        }

        private Guid id;

        public Guid Id {
            get {
                return id;
            }
            set {
                if (value != Guid.Empty) {
                    if (id != Guid.Empty) {
                        dict.Remove(id);
                    }
                    dict.Add(value, this);
                    id = value;
                }
            }
        }
        public Item Item { get; set; }
        public int Amount { get; set; }

	    public ItemStack()
	    {
		    Id = Guid.NewGuid();
	    }

	    public ItemStack(Item item, int amount = 1)
	    {
		    Amount = amount;
		    Item = item;
			Id = Guid.NewGuid();
		}

	    public override bool Equals(object obj)
	    {
		    if (!(obj is ItemStack)) return false;
		    var other = (ItemStack) obj;
		    return Item.GetType() == other.Item.GetType() && Amount == other.Amount;
	    }
	}
}