using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItemFramework;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ItemStackTest
	{
		[TestMethod]
		public void ItemStackTypeTest()
		{
			Item item = Common.GetItem();
			ItemStack itemStack = new ItemStack();
			Assert.AreEqual(null, itemStack.Item);
			itemStack.Item = item;
			Assert.AreEqual(item, itemStack.Item);
			itemStack.Amount = 0;
			Assert.AreEqual(null, itemStack.Item);
		}

		[TestMethod]
		public void ItemStackMaxStackSizeTest()
		{
			Item item = Common.GetItem();
			ItemStack itemStack = new ItemStack();
			itemStack.Item = item;
			itemStack.Amount = Common.ItemStackSize * 2;
			Assert.AreEqual(item.StackSize, itemStack.Amount);
		}
	}
}
