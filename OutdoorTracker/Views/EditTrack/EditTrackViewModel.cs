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
using System.Threading.Tasks;

using Windows.UI.Xaml.Navigation;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Services;
using OutdoorTracker.Tracks;

namespace OutdoorTracker.Views.EditTrack
{
    public class EditTrackViewModel : BaseViewModel
    {
        private readonly NavigationService _navigationService;
        private readonly TrackRecorder _trackRecorder;
        private readonly IUnitOfWork _unitOfWork;
        private int? _trackId;
        private Track _track;

        public EditTrackViewModel()
        {
        }

        public EditTrackViewModel(NavigationEventArgs parameter, IUnitOfWork unitOfWork, NavigationService navigationService, TrackRecorder trackRecorder)
            : this()
        {
            _unitOfWork = unitOfWork;
            _navigationService = navigationService;
            _trackRecorder = trackRecorder;

            CancelCommand = new RelayCommand(() => _navigationService.GoBack());
            SaveCommand = new RelayCommand(async () => await SaveTrack());
            CreateCommand = new RelayCommand(async () => await CreateNewTrack());

            _trackId = parameter.Parameter as int?;
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CreateCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public bool IsNew => !_trackId.HasValue;

        public Track Track
        {
            get { return _track; }
            set
            {
                _track = value;
                OnPropertyChanged();
            }
        }

        private async Task CreateNewTrack()
        {
            if (_trackRecorder.IsTracking)
            {
                _trackRecorder.StopTracking();
            }
            _unitOfWork.Tracks.Add(Track);
            await _unitOfWork.SaveChangesAsync();
            await _trackRecorder.StartTracking(Track);
            _navigationService.NavigateToMap();
            _navigationService.RemoveLastFrame();
        }

        private async Task SaveTrack()
        {
            await _unitOfWork.SaveChangesAsync();
            _navigationService.GoBack();
        }

        protected override async Task InitializeInternalAsync()
        {
            if (_trackId.HasValue)
            {
                int trackId = _trackId.Value;
                Track track = await _unitOfWork.Tracks.SingleOrDefaultAsync(t => t.Id == trackId);
                if (track == null)
                {
                    _navigationService.GoBack();
                }
                Track = track;
            }
            else
            {
                Track = new Track();
                Track.Name = $"Track {DateTime.Now:s}";
                Track.ShowOnMap = true;
            }
        }
    }
}