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
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OutdoorTracker.Tracks.Gpx
{
    [XmlType(Namespace = GpxV11Namespace)]
    [XmlRoot("gpx", Namespace = GpxV11Namespace, IsNullable = false)]
    public class GpxTypeV11 : GpxType
    {
    }

    [XmlType(Namespace = GpxV10Namespace)]
    [XmlRoot("gpx", Namespace = GpxV10Namespace, IsNullable = false)]
    public class GpxTypeV10 : GpxType
    {
    }

    public abstract class GpxType
    {
        public const string GpxV10Namespace = "http://www.topografix.com/GPX/1/0";
        public const string GpxV11Namespace = "http://www.topografix.com/GPX/1/1";

        protected GpxType()
        {
            Tracks = new List<GpxTrackType>();
            Waypoints = new List<WaypointType>();
        }

        [XmlElement("trk")]
        public List<GpxTrackType> Tracks { get; set; }

        [XmlElement("wpt")]
        public List<WaypointType> Waypoints { get; set; }
    }
}