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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI;

using Microsoft.Toolkit.Uwp;

namespace OutdoorTracker.Tracks
{
    public class Track : INotifyPropertyChanged
    {
        public Track()
        {
            Points = new ObservableCollection<TrackPoint>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool ShowOnMap { get; set; }
        public ObservableCollection<TrackPoint> Points { get; set; }
        public uint ColorValue { get; set; }
        public float Width { get; set; }

        public Color Color
        {
            get
            {
                return Color.FromArgb(
                    (byte)((ColorValue & 0xFF000000) >> 24),
                    (byte)((ColorValue & 0x00FF0000) >> 16),
                    (byte)((ColorValue & 0x0000FF00) >> 8),
                    (byte)(ColorValue & 0x000000FF)
                );
            }
            set
            {
                uint colorValue = (uint)value.ToInt();
                if (colorValue != ColorValue)
                {
                    ColorValue = colorValue;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}