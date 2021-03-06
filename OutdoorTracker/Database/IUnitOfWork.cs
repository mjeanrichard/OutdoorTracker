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

using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using OutdoorTracker.Layers;
using OutdoorTracker.Tracks;

namespace OutdoorTracker.Database
{
    public interface IUnitOfWork : IDisposable
    {
        DbSet<Track> Tracks { get; }
        DbSet<TrackPoint> TrackPoints { get; }
        DbSet<MapConfiguration> MapConfigurations { get; }
        DbSet<DbVersion> DbVersions { get; }

        DatabaseFacade Database { get; }

        Task SaveChangesAsync();
    }
}