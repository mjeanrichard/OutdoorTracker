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

using Microsoft.Diagnostics.Tracing;

namespace OutdoorTracker.Logging
{
    [TemplateEventSource(Name = "OutdoorTracker", AllowUnsafeCode = false)]
    public abstract class OutdoorTrackerEventsBase : EventSource
    {
        [TemplateEvent(100, Message = "Created a new UnitOfWork {0} (Transient: {1}).", Level = EventLevel.Informational)]
        public abstract void UnitOfWorkCreated(int id, bool isTransient, bool isReadonly);

        [TemplateEvent(101, Message = "Disposed a new UnitOfWork {0}.", Level = EventLevel.Informational)]
        public abstract void UnitOfWorkDisposed(int id);

        [TemplateEvent(101, Message = "Disposed a new UnitOfWork {0}.", Level = EventLevel.Informational)]
        public abstract void UnitOfWorkDisposed(int id, Exception ex);
    }
}