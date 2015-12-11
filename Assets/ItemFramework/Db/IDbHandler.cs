using Guid = System.Guid;

namespace ItemFramework.Db
{
	public interface IDbHandler
	{
		void Create(DbObject obj);
		object Load(Guid id);
		void Save();
		void Delete(DbObject obj);
	}
}