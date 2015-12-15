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

		[TestCleanup]
		public void CleanUp()
		{
			DbManager.Instance.Handler.Clear();
		}

		[TestMethod]
		public void ContainerSizeTest()
		{
			for (int i = 3; i < 60; i += 4)
			{
				Assert.AreEqual(i, new Container(i).Items.Length);
			}
		}

		[TestMethod]
		public void ItemContainerAdditionTest()
		{
			// Percentage amount in the ItemStacks
			float p1 = 0.8f;
			float p2 = 0.6f;
			float p3 = p1 + p2 - 1;

			// Stack sizes
			int q1 = (int)(Common.ItemStackSize * p1);
			int q2 = (int)(Common.ItemStackSize * p2);
			int q3 = (int)(Common.ItemStackSize * p3);

			// ItemStacks
			ItemStack i1 = common.GetItemStack(q1);
			ItemStack i2 = common.GetItemStack(q2);

			// Container
			Container container = new Container(2);
			
			// Add ItemStacks to the Container
			container.Add(i1, i2);

			// Check that the first ItemStack got filled up
			Assert.AreEqual(Common.ItemStackSize, container.Get(0).Amount);

			// Check that the right amount is in the second ItemStack
			Assert.AreEqual(q3, container.Get(1).Amount);

			// Check that the right amount is returned
			Assert.AreEqual(Common.ItemStackSize + q3, container.Count(common.GetItem().GetType()));
		}

		[TestMethod]
		public void ItemContainerSubtractionTest()
		{
			// Percentage amount in the ItemStacks
			float p1 = 0.8f;
			float p2 = 0.6f;
			float p3 = p1 + p2 - 1;

			// Stack sizes
			int q1 = (int)(Common.ItemStackSize * p1);
			int q2 = (int)(Common.ItemStackSize * p2);
			int q3 = (int)(Common.ItemStackSize * p3);

			// ItemStacks
			ItemStack i1 = common.GetItemStack(q1);
			ItemStack i2 = common.GetItemStack(q2);
			ItemStack i3 = common.GetItemStack(q3);

			// Container
			Container container = new Container(3);
			
			// Add ItemStacks to the Container
			container.Add(i1, i2);

			// Remove ItemStack from the Container
			container.Remove(i3);

			// Check that the right amount is in the first ItemStack
			Assert.AreEqual(Common.ItemStackSize - q3, container.Get(0).Amount);

			// Check that the right amount is in the second ItemStack
			Assert.AreEqual(q3, container.Get(1).Amount);

			// Check that the right amount is returned
			Assert.AreEqual(Common.ItemStackSize, container.Count(common.GetItem().GetType()));
		}

		[TestMethod]
		public void ItemContainerEmptyTest()
		{
			ItemStack i1 = new ItemStack(FrameworkRegistry.GetItem("Spade"), false, true);
			Container container = new Container(1);
			Assert.AreEqual(1, container.Items.Length);
			
			container.Add(i1);

			Assert.AreNotEqual(null, container.Get(0));

            container.Get(0).Amount = 0;

			Assert.AreEqual(null, container.Get(0));
		}

		[TestMethod]
		public void ItemContainerValidatorTest()
		{
			ItemStack i1 = new ItemStack(FrameworkRegistry.GetItem("Spade"), false, true);
			Container container = new Container(1);
			Assert.AreEqual(1, container.Items.Length);

			container.Validator += (ItemStack itemStack, System.ComponentModel.CancelEventArgs args) =>
			{
				args.Cancel = true;
			};

			container.Add(i1);

			Assert.AreEqual(null, container.Get(0));
		}
	}
}
