using System;

namespace ItemFramework.Db
{
	/// <summary>
	/// Attribute to define the name of the property in the Db
	/// </summary>
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
