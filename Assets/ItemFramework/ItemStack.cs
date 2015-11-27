using System;
using System.Collections.Generic;
using UnityEngine;
using Guid = System.Guid;

namespace ItemFramework {
    public class ItemStack : MonoBehaviour {
        private static Dictionary<Guid, ItemStack> _dict = new Dictionary<Guid, ItemStack>();

        public static ItemStack GetItemStackById(Guid id) {
            if (id == Guid.Empty) {
                Debug.LogWarning("Trying to get ItemStack with empty Guid");
            }
            return _dict[id];
        }

        private Guid _id;

        public Guid Id {
            get {
                return _id;
            }
            set {
                if (value != Guid.Empty) {
                    if (_id != Guid.Empty) {
                        _dict.Remove(_id);
                    }
                    _dict.Add(value, this);
                    _id = value;
                }
            }
        }
        public Item Item { get; set; }
        public int Amount { get; set; }
    }
}