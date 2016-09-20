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
using System.Diagnostics.Tracing;

namespace OutdoorTracker.Logging
{
    [EventSource(Name = "OutdoorTracker")]
    public class OutdoorTrackerEvents : EventSource
    {
        public static readonly OutdoorTrackerEvents Log = new OutdoorTrackerEvents();

        #region 100 - Infrastructure

        [Event(100, Message = "Created a new UnitOfWork {0} (Transient: {1}).", Level = EventLevel.Informational)]
        public void UnitOfWorkCreated(int id, bool isTransient, bool isReadonly)
        {
            WriteEvent(100, id, isTransient, isReadonly);
        }

        [Event(101, Message = "Disposed a new UnitOfWork {0}.", Level = EventLevel.Informational)]
        public void UnitOfWorkDisposed(int id)
        {
            WriteEvent(101, id);
        }

        #endregion

        #region 200 - Navigation

        [Event(200, Message = "Exception while accessing the compass.", Level = EventLevel.Warning)]
        protected void CompassAccessException(string message, string exception)
        {
            WriteEvent(200, message, exception);
        }

        [NonEvent]
        public void CompassAccessException(Exception exception)
        {
            if (IsEnabled())
            {
                CompassAccessException(exception.Message, exception.ToString());
            }
        }

        [Event(201, Message = "Found no compass.", Level = EventLevel.Informational)]
        public void CompassNotFound()
        {
            WriteEvent(201);
        }

        [Event(202, Message = "Found compass.", Level = EventLevel.Informational)]
        public void CompassFound()
        {
            WriteEvent(202);
        }

        [Event(220, Message = "Initializing Location.", Level = EventLevel.Informational)]
        public void LocationInitializing()
        {
            WriteEvent(220);
        }

        [Event(221, Message = "Location Access was answered with '{0}'.", Level = EventLevel.Informational)]
        public void LocationAccessState(string accessState)
        {
            WriteEvent(221, accessState);
        }

        #endregion

        #region 300 - Map

        [Event(300, Message = "Failed to load MapLayer '{0}'.", Level = EventLevel.Informational)]
        protected void MapDefinitionGetCurrentFailed(string mapLayerName, string message, string exception)
        {
            WriteEvent(300, message, exception);
        }

        [NonEvent]
        public void MapDefinitionGetCurrentFailed(string mapLayerName, Exception ex)
        {
            if (IsEnabled())
            {
                MapDefinitionGetCurrentFailed(mapLayerName, ex.Message, ex.ToString());
            }
        }

        [Event(310, Message = "Failed to import map layer (invalid json) '{1}'.", Level = EventLevel.Warning)]
        protected void MapDefinitionImportFailedInvalidJson(string json, string message, string exception)
        {
            WriteEvent(310, json, message, exception);
        }

        [NonEvent]
        public void MapDefinitionImportFailedInvalidJson(string json, Exception ex)
        {
            if (IsEnabled())
            {
                MapDefinitionImportFailedInvalidJson(json, ex.Message, ex.ToString());
            }
        }

        [Event(311, Message = "Exception while importing map json '{1}'.", Level = EventLevel.Error)]
        protected void MapDefinitionImportFailed(string json, string message, string exception)
        {
            WriteEvent(311, json, message, exception);
        }

        [NonEvent]
        public void MapDefinitionImportFailed(string json, Exception ex)
        {
            if (IsEnabled())
            {
                MapDefinitionImportFailed(json, ex.Message, ex.ToString());
            }
        }

        [Event(312, Message = "Exception while reading map configuration '{0}'.", Level = EventLevel.Error)]
        protected void MapDefinitionOpenFileFailed(string message, string exception)
        {
            WriteEvent(312, message, exception);
        }

        [NonEvent]
        public void MapDefinitionOpenFileFailed(Exception ex)
        {
            if (IsEnabled())
            {
                MapDefinitionOpenFileFailed(ex.Message, ex.ToString());
            }
        }

        #endregion

        #region 400 - Settings

        [Event(400, Message = "Could not read setting '{0}'.", Level = EventLevel.Warning)]
        protected void SettingsGetValueFailure(string name, string message, string exception)
        {
            WriteEvent(400, name, message, exception);
        }

        [NonEvent]
        public void SettingsGetValueFailure(string name, Exception ex)
        {
            if (IsEnabled())
            {
                SettingsGetValueFailure(name, ex.Message, ex.ToString());
            }
        }

        #endregion

        #region 500 - Tracks

        [Event(500, Message = "Could not import Tracks '{0}'.", Level = EventLevel.Warning)]
        protected void TrackImportError(string fileExtension, string message, string exception)
        {
            WriteEvent(500, fileExtension, message, exception);
        }

        [NonEvent]
        public void TrackImportError(string fileExtension, Exception ex)
        {
            if (IsEnabled())
            {
                TrackImportError(fileExtension, ex.Message, ex.ToString());
            }
        }

        [Event(550, Message = "Could not export Tracks '{0}'.", Level = EventLevel.Warning)]
        protected void TrackExportError(string fileExtension, string message, string exception)
        {
            WriteEvent(550, fileExtension, message, exception);
        }

        [NonEvent]
        public void TrackExportError(string fileExtension, Exception ex)
        {
            if (IsEnabled())
            {
                TrackExportError(fileExtension, ex.Message, ex.ToString());
            }
        }

        #endregion

        #region 900 - System Errors

        [Event(900, Message = "Unhandled Exception {0}.", Level = EventLevel.Error)]
        private void UnhandledException(string message, string exception)
        {
            WriteEvent(900, message, exception);
        }

        [NonEvent]
        public void UnhandledException(string message, Exception ex)
        {
            UnhandledException(message, ex.ToString());
        }

        #endregion
    }
}