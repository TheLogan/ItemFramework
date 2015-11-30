using ItemFramework;

namespace ItemFrameworkTest
{
	class Common
	{
		public static readonly string ItemName = "UnitTest";
		public static readonly int ItemStackSize = 64;

		public static Item GetItem()
		{
			return new Item()
			{
				Name = ItemName,
				StackSize = ItemStackSize
			};
		}

		public static ItemStack GetItemStack(int amount)
		{
			return new ItemStack()
			{
				Item = GetItem(),
				Amount = amount
			};
		}
	}
}
