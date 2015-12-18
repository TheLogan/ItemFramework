using ItemFramework;

class CrafterCraftingTable : Crafter
{
	public CrafterCraftingTable()
	{
		input = new Container(9, 3);
		output = new Container(1, 1);
	}
}