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