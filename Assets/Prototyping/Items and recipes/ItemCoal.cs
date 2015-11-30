using ItemFramework;

public class ItemCoal : Item, IBurnable
{
	public ItemCoal()
	{
		Name = "Coal";
		StackSize = 64;
	}

	public int BurnTime
	{
		get
		{
			return 120;
		}
	}
}