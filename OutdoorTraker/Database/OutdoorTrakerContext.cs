using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OutdoorTraker.Layers;
using OutdoorTraker.Tracks;

namespace OutdoorTraker.Database
{
	public class OutdoorTrakerContext : DbContext, IUnitOfWork, IReadonlyUnitOfWork
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

	public class ReadOnlyOutdoorTrakerContext : OutdoorTrakerContext
	{
		public ReadOnlyOutdoorTrakerContext()
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}