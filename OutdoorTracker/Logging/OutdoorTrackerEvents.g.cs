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

using Microsoft.Diagnostics.Tracing;

namespace OutdoorTracker.Logging
{
    /*****************************************************************/
    /* WARNING! THIS CODE IS AUTOMATICALLY GENERATED! DO NOT MODIFY! */
    /*****************************************************************/
    [EventSource(Name = "OutdoorTracker")]
    [System.CodeDom.Compiler.GeneratedCode("Alphaleonis EventSource Generator (C#)", "0.6.1.0")]
    public sealed class OutdoorTrackerEvents : OutdoorTrackerEventsBase
    {
        #region Singleton Accessor

        /*****************************************************************/
        /* WARNING! THIS CODE IS AUTOMATICALLY GENERATED! DO NOT MODIFY! */
        /*****************************************************************/
        private static readonly OutdoorTrackerEvents s_instance = new OutdoorTrackerEvents();

        /*****************************************************************/
        /* WARNING! THIS CODE IS AUTOMATICALLY GENERATED! DO NOT MODIFY! */
        /*****************************************************************/
        public static OutdoorTrackerEventsBase Log
        {
            get
            {
                return s_instance;
            }
        }

        #endregion

        #region Event Methods

        /*****************************************************************/
        /* WARNING! THIS CODE IS AUTOMATICALLY GENERATED! DO NOT MODIFY! */
        /*****************************************************************/
        [Event(100, Message = "Created a new UnitOfWork {0} (Transient: {1}).", Level = EventLevel.Informational)]
        public override void UnitOfWorkCreated(int id, bool isTransient, bool isReadonly)
        {
            if (IsEnabled())
            {
                WriteEvent(100, id, isTransient, isReadonly);
            }
        }

        /*****************************************************************/
        /* WARNING! THIS CODE IS AUTOMATICALLY GENERATED! DO NOT MODIFY! */
        /*****************************************************************/
        [Event(101, Message = "Disposed a new UnitOfWork {0}.", Level = EventLevel.Informational)]
        public override void UnitOfWorkDisposed(int id)
        {
            if (IsEnabled())
            {
                WriteEvent(101, id);
            }
        }

        #endregion

    }
}
