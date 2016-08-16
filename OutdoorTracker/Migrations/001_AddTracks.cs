using System.Collections.Generic;

using OutdoorTracker.Database;

namespace OutdoorTracker.Migrations
{
	[DbScript("001_AddTracks")]
	public class AddTracks : SqlBasedScript
	{
		protected override IEnumerable<string> GetSql()
		{
			yield return @"CREATE TABLE [Tracks] (
[Id] INTEGER PRIMARY KEY AUTOINCREMENT,
[Name] TEXT NULL);";

			yield return @"CREATE TABLE [TrackPoints] (
[Id] INTEGER PRIMARY KEY AUTOINCREMENT,
[TrackId] INTEGER NOT NULL,
[Number] INTEGER NOT NULL,
[Latitude] REAL NOT NULL,
[Longitude] REAL NOT NULL,
[Altitude] REAL NULL,
[Time] TEXT NOT NULL,
FOREIGN KEY ([TrackId]) REFERENCES [Tracks] ([Id]));";
		}
	}
}