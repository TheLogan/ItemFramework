using ItemFramework;
using ItemFramework.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ContainerTest
	{
		/// <summary>
		/// Set up the database for use in the TestMethods
		/// </summary>
		[TestInitialize]
		public void Init()
		{
			DbManager.Instance.Handler = new DbFileHandler("unittest.json");
		}

		[TestMethod]
		public void ItemContainerAdditionTest()
		{
			int q1 = (int)(Common.ItemStackSize * 0.8), q2 = (int)(Common.ItemStackSize * 0.6), q3 = (int)(Common.ItemStackSize * 0.4);
			ItemStack i1 = Common.GetItemStack(q1);
			ItemStack i2 = Common.GetItemStack(q2);
			Container container = new Container(3);
			Assert.AreEqual(3, container.Items.Length);
			container.Add(i1, i2);
			Assert.AreEqual(Common.ItemStackSize, container.Get(0).Amount);
			Assert.AreEqual(q3, container.Get(1).Amount);
			Assert.AreEqual(Common.ItemStackSize + q3, container.Count(Common.GetItem().GetType()));
		}

		[TestMethod]
		public void ItemContainerSubtractionTest()
		{
			int q1 = (int)(Common.ItemStackSize * 0.8), q2 = (int)(Common.ItemStackSize * 0.6), q3 = (int)(Common.ItemStackSize * 0.4);
			ItemStack i1 = Common.GetItemStack(q1);
			ItemStack i2 = Common.GetItemStack(q2);
			ItemStack i3 = Common.GetItemStack(q3);
			Container container = new Container(3);
			Assert.AreEqual(3, container.Items.Length);
			container.Add(i1, i2);
			container.Remove(i3);
			Assert.AreEqual(Common.ItemStackSize - q3, container.Get(0).Amount);
			Assert.AreEqual(q3, container.Get(1).Amount);
			Assert.AreEqual(Common.ItemStackSize, container.Count(Common.GetItem().GetType()));
		}
	}
}
