namespace ItemFramework.Db
{
	public class DbManager
	{
		private static DbManager instance;
		public static DbManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DbManager();
				}
				return instance;
			}
		}

		private IDbHandler handler;

		public IDbHandler Handler
		{
			get
			{
				return handler;
			}
			set
			{
				handler = value;
			}
		}

		private DbManager() { }
	}
}
