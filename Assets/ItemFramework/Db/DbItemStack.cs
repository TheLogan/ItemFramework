using Guid = System.Guid;

namespace ItemFramework.Db
{
	public abstract class DbItemStack : DbObject
	{
		public static ItemStack LoadFromDb(Guid id)
		{
			object obj = DbManager.Instance.Handler.Load(id);

			if (obj is ItemStack)
			{
				return (ItemStack)obj;
			}

			return null;
		}
	}
}
