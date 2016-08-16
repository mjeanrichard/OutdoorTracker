using System.Collections.Generic;

using OutdoorTracker.Database;

namespace OutdoorTracker.Migrations
{
	[DbScript("002_AddMapDefinitions")]
	public class AddMapDefinitions : SqlBasedScript
	{
		protected override IEnumerable<string> GetSql()
		{
			yield return @"CREATE TABLE [MapConfigurations] (
[Id] INTEGER PRIMARY KEY AUTOINCREMENT,
[Name] TEXT NOT NULL,
[Json] TEXT NOT NULL,
[Version] INTEGER NOT NULL);

CREATE UNIQUE INDEX [UQ_Name] ON [MapConfigurations] ([Name] ASC);
";
		}
	}
}