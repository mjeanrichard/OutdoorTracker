using System;
using System.Linq;
using System.Threading.Tasks;

using OutdoorTracker.Layers;
using OutdoorTracker.Tracks;

namespace OutdoorTracker.Database
{
    public interface IReadonlyUnitOfWork : IDisposable
    {
        IQueryable<Track> Tracks { get; }
        IQueryable<TrackPoint> TrackPoints { get; }
        IQueryable<MapConfiguration> MapConfigurations { get; }
        Task LoadTrackPointsAsync(Track track);
    }
}