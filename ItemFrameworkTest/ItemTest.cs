using ItemFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ItemTest
	{
		private Common common = new Common();

		[TestInitialize]
		public void Init()
		{
			FrameworkRegistry.RegisterMod(common);
		}

		[TestMethod]
		public void ItemInformationTest()
		{
			Item item = common.GetItem();
			Assert.AreEqual(Common.ItemName, item.Name);
			Assert.AreEqual(Common.ItemStackSize, item.StackSize);
		}
	}
}
