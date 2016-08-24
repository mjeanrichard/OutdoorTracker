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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Services;
using OutdoorTracker.Tracks;

namespace OutdoorTracker.Views.Tracks
{
    public class TracksViewModel : BaseViewModel
    {
        private readonly TrackRecorder _trackRecorder;
        private readonly TrackImporter _trackImporter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly NavigationService _navigationService;
        private ObservableCollection<Track> _tracks;
        private IList<Track> _selectedTracks;

        public TracksViewModel()
        {
            ImportGpxTrackCommand = new RelayCommand(async () => await ImportGpxTrack());
            EditTrackCommand = new RelayCommand(EditTrack, () => (SelectedTracks != null) && (SelectedTracks.Count == 1));
            DeleteTrackCommand = new RelayCommand(async () => await DeleteTrack(), () => (SelectedTracks != null) && (SelectedTracks.Count > 0));
            StartTrackingCommand = new RelayCommand(async () => await StartTracking());
            ExportTracksCommand = new RelayCommand(async () => await ExportTracks(), () => (SelectedTracks != null) && (SelectedTracks.Count > 0));
            ToggleTrackVisibilityCommand = new ParameterCommand<Track>(async t => await ToggleTrackVisibility(t));
            _selectedTracks = new List<Track>();
        }

        public TracksViewModel(TrackImporter trackImporter, IUnitOfWork unitOfWork, NavigationService navigationService, TrackRecorder trackRecorder)
            : this()
        {
            _trackRecorder = trackRecorder;
            _trackImporter = trackImporter;
            _unitOfWork = unitOfWork;
            _navigationService = navigationService;
        }


        public ParameterCommand<Track> ToggleTrackVisibilityCommand { get; set; }

        public ObservableCollection<Track> Tracks
        {
            get { return _tracks; }
            private set
            {
                _tracks = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ExportTracksCommand { get; set; }
        public RelayCommand StartTrackingCommand { get; }
        public RelayCommand ImportGpxTrackCommand { get; }
        public RelayCommand EditTrackCommand { get; }
        public RelayCommand DeleteTrackCommand { get; }

        public IList<Track> SelectedTracks
        {
            get { return _selectedTracks; }
            set
            {
                _selectedTracks = value;
                EditTrackCommand.RaiseCanExecuteChanged();
                DeleteTrackCommand.RaiseCanExecuteChanged();
                ExportTracksCommand.RaiseCanExecuteChanged();
            }
        }

        public Track SelectedTrack { get; set; }

        private async Task ExportTracks()
        {
            if (SelectedTracks.Any())
            {
                await _trackImporter.ExportTracks(SelectedTracks.Select(t => t.Id).ToArray());
            }
        }

        private async Task StartTracking()
        {
            if (_trackRecorder.IsTracking)
            {
                if (!await AskStopTracking())
                {
                    return;
                }
            }
            _navigationService.NavigateToNewTrack();
        }


        private async Task ToggleTrackVisibility(Track track)
        {
            if (track != null)
            {
                track.ShowOnMap = !track.ShowOnMap;
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private async Task DeleteTrack()
        {
            int count = SelectedTracks.Count();
            if (count == 0)
            {
                return;
            }
            string message;
            if (count == 1)
            {
                message = $"Are you sure that you want to delete the track '{SelectedTracks[0].Name}'?";
            }
            else
            {
                message = $"Are you sure that you want to delete the selected tracks.";
            }
            if (await AskDelete(message))
            {
                foreach (Track selectedTrack in SelectedTracks)
                {
                    _unitOfWork.TrackPoints.RemoveRange(_unitOfWork.TrackPoints.Where(p => p.Track == selectedTrack));
                    _unitOfWork.Tracks.Remove(selectedTrack);
                    await _unitOfWork.SaveChangesAsync();
                    Tracks.Remove(selectedTrack);
                }
            }
        }

        private async Task<bool> AskDelete(string message)
        {
            var dialog = new MessageDialog(message);

            dialog.Commands.Add(new UICommand("Delete") { Id = true });
            dialog.Commands.Add(new UICommand("Cancel") { Id = false });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            IUICommand result = await dialog.ShowAsync();
            return (bool)result.Id;
        }

        private async Task<bool> AskStopTracking()
        {
            var dialog = new MessageDialog("You are already recoring a track. Do you want to stop recording and create a new track?");

            dialog.Commands.Add(new UICommand("Yes") { Id = true });
            dialog.Commands.Add(new UICommand("Cancel") { Id = false });

            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            IUICommand result = await dialog.ShowAsync();
            return (bool)result.Id;
        }


        private void EditTrack()
        {
            if ((SelectedTracks == null) || (SelectedTracks.Count != 1))
            {
                return;
            }
            _navigationService.NavigateToEditTrack(SelectedTracks[0].Id);
        }

        private async Task ImportGpxTrack()
        {
            using (MarkBusy())
            {
                IEnumerable<Track> importedTracks = await _trackImporter.ImportTracks();
                foreach (Track importedTrack in importedTracks)
                {
                    Tracks.Add(importedTrack);
                }
            }
        }

        protected override async Task InitializeInternal()
        {
            Tracks = new ObservableCollection<Track>(await _unitOfWork.Tracks.ToListAsync());
        }
    }
}