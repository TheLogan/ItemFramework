using Newtonsoft.Json;
using System;

namespace ItemFramework.Db
{
	/// <summary>
	/// Attribute to define the object's name in Db
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class DbObjectAttribute : Attribute
	{
		// Tell the JSON.NET library that properties and fields in the class must have a property attribute.
		public readonly MemberSerialization MemberSerialization = MemberSerialization.OptIn;

		public string Name;

		public DbObjectAttribute(string name)
		{
			this.Name = name;
		}
	}
}