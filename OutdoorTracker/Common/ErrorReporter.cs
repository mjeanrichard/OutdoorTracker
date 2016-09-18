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
using System.Collections.Generic;
using System.Diagnostics;

// ReSharper disable once RedundantUsingDirective
using Microsoft.HockeyApp;

namespace OutdoorTracker.Common
{
    public class ErrorReporter
    {
        protected ErrorReporter()
        {
        }

        public static ErrorReporter Current { get; } = new ErrorReporter();

        public void TrackException(Exception exception, Dictionary<string, string> properties = null)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
#else
            HockeyClient.Current.TrackException(exception, properties);
#endif
        }

        public void TrackEvent(string eventName, Dictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
#if !DEBUG
            HockeyClient.Current.TrackEvent(eventName, properties, metrics);
#endif
        }
    }
}