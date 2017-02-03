// 
// Outdoor Tracker - Copyright(C) 2017 Meinard Jean-Richard
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

using OutdoorTracker.Common;
using OutdoorTracker.Database;

using UniversalMapControl.Projections;

namespace OutdoorTracker.Tracks
{
    public class TrackFixer
    {
        private readonly UnitOfWorkFactoy _unitOfWorkFactory;

        public TrackFixer(UnitOfWorkFactoy unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task UpdateAllTracks()
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Create())
            {
                List<Track> tracks = unitOfWork.Tracks.ToList();
                foreach (Track track in tracks)
                {
                    await UpdateTrack(track, unitOfWork);
                    await unitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateTrack(Track track, IUnitOfWork unitOfWork)
        {
            double totalDistance = 0;
            Wgs84Location previousLocation = null;

            TrackPoint[] trackPoints = await unitOfWork.TrackPoints.Where(p => p.TrackId == track.Id).OrderBy(tp => tp.Number).ToArrayAsync().ConfigureAwait(false);
            for (int index = 0; index < trackPoints.Length; index++)
            {
                TrackPoint trackPoint = trackPoints[index];

                Wgs84Location currentLocation = trackPoint.Location;
                if (previousLocation == null)
                {
                    previousLocation = currentLocation;
                    continue;
                }
                totalDistance += previousLocation.DistanceTo(currentLocation);
                previousLocation = currentLocation;
            }
            track.FlatLength = totalDistance;
        }
    }
}