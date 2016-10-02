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

namespace OutdoorTracker.Tracks.Kml
{
    [XmlType(Namespace = KmlV21Namespace)]
    [XmlRoot("kml", Namespace = KmlV21Namespace, IsNullable = false)]
    public class KmlTypeV21 : KmlType
    {
    }

    [XmlType(Namespace = KmlV22Namespace)]
    [XmlRoot("kml", Namespace = KmlV22Namespace, IsNullable = false)]
    public class KmlTypeV22 : KmlType
    {
    }

    public abstract class KmlType
    {
        public const string KmlV22Namespace = "http://www.opengis.net/kml/2.2";
        public const string KmlV21Namespace = "http://www.opengis.net/kml/2.1";

        protected KmlType()
        {
        }

        [XmlElement("Document")]
        public KmlDocument Document { get; set; }
    }
}