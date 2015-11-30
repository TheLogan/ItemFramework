using ItemFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ItemTest
	{
		[TestMethod]
		public void ItemInformationTest()
		{
			Item item = Common.GetItem();
			Assert.AreEqual(Common.ItemName, item.Name);
			Assert.AreEqual(Common.ItemStackSize, item.StackSize);
		}
	}
}
