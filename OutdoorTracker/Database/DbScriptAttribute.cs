using System;

namespace OutdoorTracker.Database
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