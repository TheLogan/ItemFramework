using Guid = System.Guid;

namespace ItemFramework.Db
{
	public interface IDbHandler
	{
		/// <summary>
		/// Create object in Db
		/// </summary>
		/// <param name="obj">The object</param>
		void Create(DbObject obj);
		/// <summary>
		/// Load specific item from Db
		/// </summary>
		/// <param name="id">Id of the item</param>
		/// <returns>Object if found; otherwise null</returns>
		object Load(Guid id);
		/// <summary>
		/// Save db
		/// </summary>
		void Save();
		/// <summary>
		/// Remove object from Db
		/// </summary>
		/// <param name="obj">The object</param>
		void Remove(DbObject obj);
		/// <summary>
		/// Remove all objects from Db
		/// </summary>
		void Clear();
	}
}