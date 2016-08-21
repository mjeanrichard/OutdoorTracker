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

using System.Xml.Serialization;

using Windows.Foundation;

namespace OutdoorTracker.Tracks.Gpx
{
    [XmlType("wptType", Namespace = "http://www.topografix.com/GPX/1/1")]
    public class WaypointType
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("cmt")]
        public string Comment { get; set; }

        [XmlElement("desc")]
        public string Description { get; set; }

        [XmlElement("src")]
        public string Source { get; set; }

        [XmlElement("sym")]
        public string Symbol { get; set; }

        [XmlElement("type")]
        public string Type { get; set; }

        [XmlAttribute("lat")]
        public double Latitude { get; set; }

        [XmlAttribute("lon")]
        public double Longitude { get; set; }

        public Point ToPoint()
        {
            return new Point(Latitude, Longitude);
        }
    }
}