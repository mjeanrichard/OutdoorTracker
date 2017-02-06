// 
// Outdoor Tracker - Copyright(C) 2017 Meinard Jean-Richard
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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Resources;
using OutdoorTracker.Tracks;

namespace OutdoorTracker.Views.Tracks
{
    public class TrackViewModel : INotifyPropertyChanged
    {
        private readonly UnitOfWorkFactoy _unitOfWorkFactoy;

        public TrackViewModel(Track track, UnitOfWorkFactoy unitOfWorkFactoy)
        {
            _unitOfWorkFactoy = unitOfWorkFactoy;
            Track = track;
            ToggleTrackVisibilityCommand = new AsyncCommand(ToggleTrackVisibility, null);
        }

        public AsyncCommand ToggleTrackVisibilityCommand { get; set; }

        public Track Track { get; }

        public string Name => Track.Name;

        public string Date => Messages.TracksPage.TrackDate(Track.Date);

        public string Length
        {
            get { return Track.LengthString; }
        }

        public bool ShowOnMap
        {
            get { return Track.ShowOnMap; }
        }


        public async Task ToggleTrackVisibility()
        {
            using (IUnitOfWork unitOfWork = _unitOfWorkFactoy.Create())
            {
                Track track = await unitOfWork.Tracks.SingleAsync(t => t.Id == Track.Id);
                track.ShowOnMap = !Track.ShowOnMap;
                await unitOfWork.SaveChangesAsync();
                Track.ShowOnMap = track.ShowOnMap;
            }
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => OnPropertyChanged(nameof(ShowOnMap)));
        }

        public void Refresh()
        {
            DispatcherHelper.ExecuteOnUIThreadAsync(() => OnPropertyChanged(nameof(Length)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}