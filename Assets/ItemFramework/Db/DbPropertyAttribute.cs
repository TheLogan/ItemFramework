using System;

namespace ItemFramework.Db
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public class DbPropertyAttribute : Attribute
	{
		public string Name;

		public DbPropertyAttribute(string name)
		{
			this.Name = name;
		}
	}
}
