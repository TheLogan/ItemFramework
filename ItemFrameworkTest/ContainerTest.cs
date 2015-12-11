using ItemFramework;
using ItemFramework.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ContainerTest
	{
		private Common common = new Common();

		[TestInitialize]
		public void Init()
		{
			FrameworkRegistry.RegisterMod(common);
			DbManager.Instance.Handler = new DbFileHandler("unittest.json");
		}

		[TestMethod]
		public void ItemContainerAdditionTest()
		{
			int q1 = (int)(Common.ItemStackSize * 0.8), q2 = (int)(Common.ItemStackSize * 0.6), q3 = (int)(Common.ItemStackSize * 0.4);
			ItemStack i1 = common.GetItemStack(q1);
			ItemStack i2 = common.GetItemStack(q2);
			Container container = new Container(3);
			Assert.AreEqual(3, container.Items.Length);
			container.Add(i1, i2);
			Assert.AreEqual(Common.ItemStackSize, container.Get(0).Amount);
			Assert.AreEqual(q3, container.Get(1).Amount);
			Assert.AreEqual(Common.ItemStackSize + q3, container.Count(common.GetItem().GetType()));
		}

		[TestMethod]
		public void ItemContainerSubtractionTest()
		{
			int q1 = (int)(Common.ItemStackSize * 0.8), q2 = (int)(Common.ItemStackSize * 0.6), q3 = (int)(Common.ItemStackSize * 0.4);
			ItemStack i1 = common.GetItemStack(q1);
			ItemStack i2 = common.GetItemStack(q2);
			ItemStack i3 = common.GetItemStack(q3);
			Container container = new Container(3);
			Assert.AreEqual(3, container.Items.Length);
			container.Add(i1, i2);
			container.Remove(i3);
			Assert.AreEqual(Common.ItemStackSize - q3, container.Get(0).Amount);
			Assert.AreEqual(q3, container.Get(1).Amount);
			Assert.AreEqual(Common.ItemStackSize, container.Count(common.GetItem().GetType()));
		}
	}
}
