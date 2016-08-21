// 
// Outdoor Tracker - Copyright(C) 2016 Meinard Jean-Richard
//  
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Layers;
using OutdoorTracker.Tracks;

namespace OutdoorTracker.Database
{
    public class OutdoorTrackerContext : DbContext, IUnitOfWork, IReadonlyUnitOfWork
    {
        public DbSet<Track> Tracks { get; protected set; }

        IQueryable<TrackPoint> IReadonlyUnitOfWork.TrackPoints
        {
            get { return TrackPoints.AsNoTracking(); }
        }

        IQueryable<MapConfiguration> IReadonlyUnitOfWork.MapConfigurations
        {
            get { return MapConfigurations.AsNoTracking(); }
        }

        IQueryable<Track> IReadonlyUnitOfWork.Tracks
        {
            get { return Tracks.AsNoTracking(); }
        }

        public DbSet<TrackPoint> TrackPoints { get; protected set; }

        public DbSet<MapConfiguration> MapConfigurations { get; protected set; }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=data.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbVersion>().ToTable("Version");

            modelBuilder.Entity<Track>().ToTable("Tracks");
            modelBuilder.Entity<TrackPoint>().ToTable("TrackPoints");

            modelBuilder.Entity<MapConfiguration>().ToTable("MapConfigurations");
            modelBuilder.Entity<MapConfiguration>().Ignore(m => m.Projection).Ignore(m => m.LayerConfig);
        }
    }

    public class ReadOnlyOutdoorTrackerContext : OutdoorTrackerContext
    {
        public ReadOnlyOutdoorTrackerContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}