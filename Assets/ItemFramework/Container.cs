using UnityEngine;

public class Container : MonoBehaviour {

	public int Id { get; set; }
	public ItemStack[]	Items { get; set; }
	public int Width { get; set; }

	public ItemStack Add(int index, ItemStack type)
	{
		return null;
	}

	public ItemStack Add(ItemStack type)
	{
		return null;
	}

	public ItemStack Remove(int index)
	{
		return null;
	}

	public ItemStack Remove(int index, int amount)
	{
		return null;
	}

	public ItemStack Remove(ItemStack removeable)
	{
		return removeable;
	}

	public bool Contains(ItemStack stack)
	{
		return false;
	}

	public int Contains(Item type)
	{
		return 0;
	}

}
