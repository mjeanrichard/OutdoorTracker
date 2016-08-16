using System.Collections.Generic;

using OutdoorTracker.Database;

namespace OutdoorTracker.Migrations
{
	[DbScript("004_TrackShowOnMap")]
	public class TrackShowOnMap : SqlBasedScript
	{
		protected override IEnumerable<string> GetSql()
		{
			yield return @"ALTER TABLE Tracks ADD ShowOnMap INTEGER NOT NULL DEFAULT 0;";
		}
	}
}