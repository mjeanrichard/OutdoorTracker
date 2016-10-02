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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.HockeyApp;

using OutdoorTracker.Helpers;

namespace OutdoorTracker.Common
{
    public class ErrorReporter
    {
        public static void Initialize()
        {
#if !DEBUG
            HockeyClient.Current.Configure("b2c844d2de1245bf8e2495ed20350fd8").SetExceptionDescriptionLoader(DescriptionLoader);
#endif
        }

        private static string DescriptionLoader(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"HResult: {exception.HResult}");
            DecoratedHockeyAppException hockeyAppEx = exception as DecoratedHockeyAppException;
            if (hockeyAppEx != null)
            {
                exception = hockeyAppEx.InnerException;
                sb.AppendLine($"Original Exception Type: {exception.GetType().Name}");
                foreach (KeyValuePair<string, string> property in hockeyAppEx.Properties)
                {
                    sb.AppendLine($"{property.Key}: {property.Value}");
                }
            }
            sb.AppendLine();
            sb.AppendLine($"Exception String:");
            sb.AppendLine($"===========================");
            sb.AppendLine(exception.ToString());
            sb.AppendLine($"===========================");
            return sb.ToString();
        }

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
            //Workaround to the missing properties in the HockeyApp-UI.
            if (properties != null)
            {
                exception = new DecoratedHockeyAppException(exception, properties.ToArray());
            }
            HockeyClient.Current.TrackException(exception, properties);
#endif
        }

        public void TrackEvent(TrackEvents trackEvent, Dictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
#if !DEBUG
            HockeyClient.Current.TrackEvent(trackEvent.ToString("G"), properties, metrics);
#endif
        }
    }

    /// <summary>
    /// Exception Wrapper to add additional information to Exceptions that are logged to HockeyApp.
    /// This can be removed when HockeyAPp finally supports properties.
    /// </summary>
    public class DecoratedHockeyAppException : Exception
    {
        public DecoratedHockeyAppException(Exception innerException, params KeyValuePair<string, string>[] properties) : base(innerException?.Message ?? string.Empty, innerException)
        {
            Properties = properties;
        }

        public override IDictionary Data
        {
            get { return InnerException.Data; }
        }

        public override string HelpLink
        {
            get { return InnerException.HelpLink; }
            set { InnerException.HelpLink = value; }
        }

        public override string Message
        {
            get { return InnerException.Message; }
        }

        public override string Source
        {
            get { return InnerException.Source; }
            set { InnerException.Source = value; }
        }

        public override string StackTrace
        {
            get { return InnerException.StackTrace; }
        }

        public override string ToString()
        {
            return InnerException.ToString();
        }

        public KeyValuePair<string, string>[] Properties { get; }

        public override Exception GetBaseException()
        {
            return InnerException.GetBaseException();
        }
    }
}