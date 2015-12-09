using ItemFramework;

namespace ItemFrameworkTest
{
	class Common
	{
		public static readonly string ItemName = "UnitTest";
		public static readonly int ItemStackSize = 64;

		public static Item GetItem()
		{
			return new UnitTestItem()
			{
				Name = ItemName,
				StackSize = ItemStackSize
			};
		}

		public static ItemStack GetItemStack(int amount)
		{
			return new ItemStack(GetItem(), amount, false, true);
		}
	}

	class UnitTestItem : Item
	{
		public UnitTestItem(int stackSize = -1)
		{
			Name = Common.ItemName;
			StackSize = stackSize <= 0 ? Common.ItemStackSize : stackSize;
		}
	}

	class StoneItem : Item
	{
		public StoneItem(int stackSize = 16)
		{
			Name = "Stone";
			StackSize = stackSize;
		}
	}

	class StickItem : Item
	{
		public StickItem(int stackSize = 4)
		{
			Name = "Stick";
			StackSize = stackSize;
		}
	}

	class SpadeItem : Item
	{
		public SpadeItem(int stackSize = 1)
		{
			Name = "Spade";
			StackSize = stackSize;
		}
	}
}
