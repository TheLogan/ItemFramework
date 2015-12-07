using ItemFramework;
using ItemFramework.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ContainerTest
	{
		[TestInitialize]
		public void Init()
		{
			DbManager.Instance.Handler = new DbFileHandler("unittest.json");
		}

		[TestMethod]
		public void ItemContainerAdditionTest()
		{
			int q1 = (int)(Common.ItemStackSize * 0.8), q2 = (int)(Common.ItemStackSize * 0.6);
			ItemStack i1 = Common.GetItemStack(q1);
			ItemStack i2 = Common.GetItemStack(q2);
			Container container = new Container(3);
			Assert.AreEqual(3, container.Items.Length);
			container.Add(i1, i2);
			Assert.AreEqual(Common.ItemStackSize, container.Get(0).Amount);
			Assert.AreEqual((int)(Common.ItemStackSize * 0.4), container.Get(1).Amount);
		}

		[TestMethod]
		public void ItemContainerSubtractionTest()
		{
			// TODO: Write this
		}
	}
}
