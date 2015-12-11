using System.Linq;
using Guid = System.Guid;
using InvalidOperationException = System.InvalidOperationException;

namespace ItemFramework.Db
{
	/// <summary>
	/// Object to be saved in Db
	/// </summary>
	public abstract class DbObject
	{
		/// <summary>
		/// Database Id of object
		/// </summary>
		[DbProperty("id")]
		public Guid id { get; protected set; }

		/// <summary>
		/// Database Id of object
		/// </summary>
		public abstract Guid Id { get; internal set; }

		/// <summary>
		/// Whether the object is in the Db
		/// </summary>
		public bool IsInDb
		{
			get
			{
				return Id != Guid.Empty;
			}
		}

		/// <summary>
		/// Load this object from Db
		/// </summary>
		/// <returns></returns>
		public object LoadFromDb()
		{
			if (IsInDb)
			{
				return DbManager.Instance.Handler.Load(Id);
			}
			return null;
		}

		/// <summary>
		/// Save this object to Db
		/// </summary>
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

		/// <summary>
		/// Remove this object from Db
		/// </summary>
		public void RemoveFromDb()
		{
			if (IsInDb)
			{
				DbManager.Instance.Handler.Remove(this);
			}
		}
	}
}
