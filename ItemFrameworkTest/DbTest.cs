using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItemFramework;
using ItemFramework.Db;

namespace ItemFrameworkTest
{
	[TestClass]
	public class DbTest
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
		public void DbLoadTest()
		{
			ItemStack i = common.GetItemStack(10, false);

			Assert.AreEqual(i, ItemStack.LoadFromDb(i.Id));
		}
	}
}
