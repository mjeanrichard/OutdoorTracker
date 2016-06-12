using System;

namespace OutdoorTraker.Database
{
	public sealed class DbScriptAttribute : Attribute
	{
		public DbScriptAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}