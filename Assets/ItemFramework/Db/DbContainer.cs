using Guid = System.Guid;

namespace ItemFramework.Db
{
	public abstract class DbContainer : DbObject
	{
		public static Container LoadFromDb(Guid id)
		{
			object obj = DbManager.Instance.Handler.Load(id);

			if (obj is Container)
			{
				return (Container)obj;
			}

			return null;
		}
	}
}
