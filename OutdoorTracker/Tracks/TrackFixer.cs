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

using System;
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
            double maxLatitude = 0;
            double minLatitude = double.MaxValue;
            double maxLongitude = 0;
            double minLongitude = double.MaxValue;

            double totalDistance = 0;
            Wgs84Location previousLocation = null;

            TrackPoint[] trackPoints = await unitOfWork.TrackPoints.Where(p => p.TrackId == track.Id).OrderBy(tp => tp.Number).ToArrayAsync().ConfigureAwait(false);
            for (int index = 0; index < trackPoints.Length; index++)
            {
                TrackPoint trackPoint = trackPoints[index];

                Wgs84Location currentLocation = trackPoint.Location;

                maxLatitude = Math.Max(maxLatitude, currentLocation.Latitude);
                maxLongitude = Math.Max(maxLongitude, currentLocation.Longitude);
                minLatitude = Math.Min(minLatitude, currentLocation.Latitude);
                minLongitude = Math.Min(minLongitude, currentLocation.Longitude);

                if (previousLocation != null)
                {
                    totalDistance += previousLocation.DistanceTo(currentLocation);
                }
                previousLocation = currentLocation;
            }
            if (trackPoints.Length > 0)
            {
                track.Date = trackPoints[0].Time;
            }
            track.MaxLatitude = maxLatitude;
            track.MaxLongitude = maxLongitude;
            track.MinLatitude = minLatitude;
            track.MinLongitude = minLongitude;
            track.FlatLength = totalDistance;
        }
    }
}