namespace ItemFramework.Db
{
	public interface IDbHandler
	{
		void Create(DbObject obj);
		void Load();
		void Save();
		void Delete(DbObject obj);
	}
}