using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Guid = System.Guid;

namespace ItemFramework {
    public class Container : MonoBehaviour {
        private static Dictionary<Guid, Container> _dict = new Dictionary<Guid, Container>();

        public static Container GetContainerById(Guid id) {
            if (id == Guid.Empty) {
                Debug.LogWarning("Trying to get Container with empty Guid");
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
        public Guid?[] Items { get; set; }
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
        public ItemStack Add(int index, ItemStack type) { //TODO take max stack sizes into account

            //If the index is null just add the new items
            if (Items[index].HasValue) {
                Items[index] = type.Id;
            } else //If it's not null find a different index to place it at
              {
                var indexes = new List<int>();
                for (var i = 0; i < Items.Length; i++) {
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

        /// <summary>
        /// Add item stack to the first empty slot in inventory
        /// If there are no empty slots, returns the item stack.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ItemStack Add(ItemStack type) {
            for (int i = 0; i < Items.Length; i++) {
                if (Items[i].HasValue) {
                    Items[i] = type.Id;
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
        public ItemStack Replace(int index, ItemStack type) {
            ItemStack tempStack = Get(index);
            Items[index] = type.Id;
            return tempStack;
        }

        /// <summary>
        /// Takes an index and returns what's in that slot
        /// </summary>
        /// <param name="index">The inventory index to look in</param>
        /// <returns></returns>
        public ItemStack Remove(int index) {
            ItemStack tempStack = Get(index);
            Items[index] = null;
            return tempStack;
        }

        /// <summary>
        /// Removes a set amount of items from a stack and returns them as a new stack
        /// </summary>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public ItemStack Remove(int index, int amount) {
            //If there are no items to remove, remove no items
            if (Items[index].HasValue)
                return null;

            //If asked to remove the amount or more of the stack, just remove the whole stack
            ItemStack tempStack = Get(index);
            if (amount >= tempStack.Amount) {
                Items[index] = null;
                return tempStack;
            }

            //If asked to remove part of the stack, create a new stack of the requested amount and remove said amount from the original stack
            ItemStack returnStack = new ItemStack {
                Item = new Item { Name = tempStack.name },
                Amount = amount
            };

            tempStack.Amount -= amount;
            return tempStack;
        }

        /// <summary>
        /// find an itemstack matching current type and return it
        /// </summary>
        /// <param name="removeable"></param>
        /// <returns></returns>
        public ItemStack Remove(ItemStack removeable) {
            var i = 0;
            ItemStack tempStack = null;
            for (i = 0; i < Items.Length; i++) {
                tempStack = Get(i);
                if (tempStack != null && tempStack.name == removeable.name) {
                    if (removeable.Amount >= tempStack.Amount) {
                        Items[i] = null;
                        return tempStack;
                    }

                    tempStack.Amount -= removeable.Amount;
                    return removeable;
                    break;
                }
            }
            return null;
        }


        public bool Contains(ItemStack stack) {
            return Items.Any(x => x.HasValue ? ItemStack.GetItemStackById(x.Value).Item.name == stack.name : false);
        }


        public int Contains(Item type) {
            Guid? tempStackId = Items.FirstOrDefault(x => x.HasValue ? ItemStack.GetItemStackById(x.Value).Item.name == type.name : false);
            return tempStackId.HasValue ? ItemStack.GetItemStackById(tempStackId.Value).Amount : 0;
        }

        public ItemStack Get(int index) {
            ItemStack itemStack = null;
            if (Items[index].HasValue) {
                itemStack = ItemStack.GetItemStackById(Items[index].Value);
            }
            return itemStack;
        }

    }
}