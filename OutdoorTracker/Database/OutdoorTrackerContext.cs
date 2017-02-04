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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using OutdoorTracker.Layers;
using OutdoorTracker.Logging;
using OutdoorTracker.Tracks;

namespace OutdoorTracker.Database
{
    public class OutdoorTrackerContext : DbContext, IUnitOfWork
    {
        private static int _lastId = 0;

        public OutdoorTrackerContext(bool isReadonly, bool isTransient)
        {
            Id = Interlocked.Increment(ref _lastId);
            OutdoorTrackerEvents.Log.UnitOfWorkCreated(Id, isReadonly, isTransient);
        }

        protected int Id { get; }

        public DbSet<Track> Tracks { get; protected set; }

        public DbSet<DbVersion> DbVersions { get; protected set; }
        public DbSet<TrackPoint> TrackPoints { get; protected set; }
        public DbSet<MapConfiguration> MapConfigurations { get; protected set; }

        public virtual async Task LoadTrackPointsAsync(Track track)
        {
            await Entry(track).Collection(t => t.Points).Query().OrderBy(t => t.Number).LoadAsync();
        }

        public override void Dispose()
        {
            base.Dispose();
            OutdoorTrackerEvents.Log.UnitOfWorkDisposed(Id);
        }

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
            modelBuilder.Entity<Track>().Ignore(t => t.Color);
            modelBuilder.Entity<Track>().Property(t => t.ColorValue).HasColumnName(nameof(Track.Color));

            modelBuilder.Entity<TrackPoint>().ToTable("TrackPoints");

            modelBuilder.Entity<MapConfiguration>().ToTable("MapConfigurations");
            modelBuilder.Entity<MapConfiguration>().Ignore(m => m.Projection).Ignore(m => m.LayerConfig);
        }
    }

    public class ReadOnlyOutdoorTrackerContext : OutdoorTrackerContext, IReadonlyUnitOfWork
    {
        public ReadOnlyOutdoorTrackerContext() : base(true, false)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

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

        public async override Task LoadTrackPointsAsync(Track track)
        {
            Attach(track);
            await base.LoadTrackPointsAsync(track);
            Entry(track).State = EntityState.Detached;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}