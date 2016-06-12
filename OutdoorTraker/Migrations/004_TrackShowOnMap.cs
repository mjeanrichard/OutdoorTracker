using System.Collections.Generic;

using OutdoorTraker.Database;

namespace OutdoorTraker.Migrations
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