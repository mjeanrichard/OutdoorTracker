using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Tracks;

namespace OutdoorTracker.Database
{
	public class TrackRepository
	{
		private readonly OutdoorTrackerContext _OutdoorTrackerContext;

		public TrackRepository(OutdoorTrackerContext OutdoorTrackerContext)
		{
			_OutdoorTrackerContext = OutdoorTrackerContext;
		}

		public async Task<IEnumerable<TrackPoint>> GetTrackPoints(int trackId)
		{
			return await _OutdoorTrackerContext.TrackPoints.AsNoTracking().Where(tp => tp.Track.Id == trackId).ToArrayAsync();
		}

	}
}