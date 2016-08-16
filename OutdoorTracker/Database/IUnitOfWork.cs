using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Layers;
using OutdoorTracker.Tracks;

namespace OutdoorTracker.Database
{
	public interface IUnitOfWork : IDisposable
	{
		DbSet<Track> Tracks { get; }
		DbSet<TrackPoint> TrackPoints { get; }
		DbSet<MapConfiguration> MapConfigurations { get; }

		Task SaveChangesAsync();
	}

	public interface IReadonlyUnitOfWork : IDisposable
	{
		IQueryable<Track> Tracks { get; }
		IQueryable<TrackPoint> TrackPoints { get; }
		IQueryable<MapConfiguration> MapConfigurations { get; }
	}
}