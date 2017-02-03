using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Database;

namespace OutdoorTracker.Migrations
{
    [DbScript("008_AddTrackInfo")]
    public class AddTrackInfo : SqlBasedScript
    {
        protected override IEnumerable<string> GetSql()
        {
            yield return @"ALTER TABLE Tracks ADD FlatLength REAL NOT NULL DEFAULT 0;";
            yield return @"ALTER TABLE Tracks ADD MinLatitude REAL NOT NULL DEFAULT 0;";
            yield return @"ALTER TABLE Tracks ADD MinLongitude REAL NOT NULL DEFAULT 0;";
            yield return @"ALTER TABLE Tracks ADD MaxLatitude REAL NOT NULL DEFAULT 0;";
            yield return @"ALTER TABLE Tracks ADD MaxLongitude REAL NOT NULL DEFAULT 0;";
        }

        public override async Task Execute(IUnitOfWork context)
        {
            await base.Execute(context);
        }
    }
}