using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItemFramework;
using ItemFramework.Db;

namespace ItemFrameworkTest
{
	[TestClass]
	public class ItemStackTest
	{
		private Common common = new Common();

		/// <summary>
		/// Set up the database for use in the TestMethods
		/// </summary>
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
		public void ItemStackTypeTest()
		{
			Item item = common.GetItem();
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
			Item item = common.GetItem();
			ItemStack itemStack = new ItemStack();
			itemStack.Item = item;
			itemStack.Amount = Common.ItemStackSize * 2;
			Assert.AreEqual(item.StackSize, itemStack.Amount);
		}

		[TestMethod]
		public void ItemStackLimitlessSizeTest()
		{
			Item item = common.GetItem();
			ItemStack itemStack = new ItemStack();
			itemStack.Item = item;
			itemStack.IsLimited = false;
			itemStack.Amount = Common.ItemStackSize * 2;
			Assert.AreEqual(item.StackSize * 2, itemStack.Amount);
		}
	}
}
