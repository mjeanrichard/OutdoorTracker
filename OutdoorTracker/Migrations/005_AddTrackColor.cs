using System.Collections.Generic;

using OutdoorTracker.Database;

namespace OutdoorTracker.Migrations
{
    [DbScript("005_AddTrackColor")]
    public class AddTrackColor : SqlBasedScript
    {
        protected override IEnumerable<string> GetSql()
        {
            yield return @"ALTER TABLE Tracks ADD Color INTEGER NOT NULL DEFAULT 4294901760;";
        }
    }
}