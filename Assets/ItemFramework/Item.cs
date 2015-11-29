using UnityEngine;

namespace ItemFramework
{
    public class Item
    {

        public string Name { get; set; }
        public int StackSize { get; set; }

        public virtual void Use()
        {

        }

        public override string ToString()
        {
            return "Item[Name=" + Name + "]";
        }

    }
}