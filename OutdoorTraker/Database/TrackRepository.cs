using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OutdoorTraker.Tracks;

namespace OutdoorTraker.Database
{
	public class TrackRepository
	{
		private readonly OutdoorTrakerContext _outdoorTrakerContext;

		public TrackRepository(OutdoorTrakerContext outdoorTrakerContext)
		{
			_outdoorTrakerContext = outdoorTrakerContext;
		}

		public async Task<IEnumerable<TrackPoint>> GetTrackPoints(int trackId)
		{
			return await _outdoorTrakerContext.TrackPoints.AsNoTracking().Where(tp => tp.Track.Id == trackId).ToArrayAsync();
		}

	}
}