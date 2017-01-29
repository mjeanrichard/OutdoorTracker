using System.Collections.Generic;

using OutdoorTracker.Database;

namespace OutdoorTracker.Migrations
{
    [DbScript("006_AddTrackWidth")]
    public class AddTrackWidth : SqlBasedScript
    {
        protected override IEnumerable<string> GetSql()
        {
            yield return @"ALTER TABLE Tracks ADD Width REAL NOT NULL DEFAULT 1;";
        }
    }
}