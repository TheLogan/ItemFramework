using System.Linq;
using Guid = System.Guid;
using InvalidOperationException = System.InvalidOperationException;

namespace ItemFramework.Db
{
	public abstract class DbObject
	{
		[DbProperty("id")]
		public Guid id;

		public abstract Guid Id { get; internal set; }

		public bool IsInDb
		{
			get
			{
				return Id != Guid.Empty;
			}
		}

		public object LoadFromDb()
		{
			if (IsInDb)
			{
				return DbManager.Instance.Handler.Load(Id);
			}
			return null;
		}

		public void SaveToDb()
		{
			var dataContract = GetType().GetCustomAttributes(typeof(DbObjectAttribute), true).FirstOrDefault();

			if (dataContract == null)
			{
				throw new InvalidOperationException("Object is missing DbObject attribute");
			}

			if (IsInDb)
			{
				DbManager.Instance.Handler.Save();
			}
			else
			{
				DbManager.Instance.Handler.Create(this);
			}
		}

		public void DeleteFromDb()
		{
			if (IsInDb)
			{
				DbManager.Instance.Handler.Delete(this);
			}
		}
	}
}
