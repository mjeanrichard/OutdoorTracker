using System.Collections.Generic;

using OutdoorTracker.Database;

namespace OutdoorTracker.Migrations
{
    [DbScript("007_AddTrackSpeed")]
    public class AddTrackSpeed : SqlBasedScript
    {
        protected override IEnumerable<string> GetSql()
        {
            yield return @"ALTER TABLE TrackPoints ADD Speed REAL NULL;";
        }
    }
}