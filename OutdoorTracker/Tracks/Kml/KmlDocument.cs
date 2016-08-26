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
using System.Xml.Serialization;

namespace OutdoorTracker.Tracks.Kml
{
    [XmlType("Document", Namespace = "http://www.opengis.net/kml/2.2")]
    public class KmlDocument
    {
        public KmlDocument()
        {
            Placemarks = new List<KmlPlacemark>();
        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("Placemark")]
        public List<KmlPlacemark> Placemarks { get; set; }
    }
}