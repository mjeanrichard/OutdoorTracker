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

using Windows.UI.Popups;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Resources;
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
        private string _busyText;

        public TracksViewModel()
        {
            ImportGpxTrackCommand = new RelayCommand(async () => await ImportGpxTrack());
            EditTrackCommand = new RelayCommand(EditTrack, () => SelectedTracks != null && SelectedTracks.Count == 1);
            DeleteTrackCommand = new RelayCommand(async () => await DeleteTrack(), () => SelectedTracks != null && SelectedTracks.Count > 0);
            StartTrackingCommand = new RelayCommand(async () => await StartTracking());
            ExportTracksCommand = new RelayCommand(async () => await ExportTracks(), () => SelectedTracks != null && SelectedTracks.Count > 0);
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

        public string BusyText
        {
            get { return _busyText; }
            set
            {
                _busyText = value;
                OnPropertyChanged();
            }
        }

        private async Task ExportTracks()
        {
            using (MarkBusy())
            {
                BusyText = "Exporting selected tracks...";
                if (SelectedTracks.Any())
                {
                    await _trackImporter.ExportTracks(SelectedTracks.Select(t => t.Id).ToArray());
                }
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
            IList<Track> selectedTracks = SelectedTracks;
            if (selectedTracks.Count == 0)
            {
                return;
            }

            BusyText = Messages.TracksPage.DeletingText;
            using (MarkBusy())
            {
                string message;
                string title;
                if (selectedTracks.Count == 1)
                {
                    message = Messages.TracksPage.DeleteSingleMessage(selectedTracks[0].Name);
                    title = Messages.TracksPage.DeleteSingleTitle;
                }
                else
                {
                    message = Messages.TracksPage.DeleteMessage;
                    title = Messages.TracksPage.DeleteTitle;
                }
                if (await AskDelete(message, title))
                {
                    await Task.Run(async () =>
                    {
                        using (MarkBusy())
                        {
                            foreach (Track selectedTrack in selectedTracks)
                            {
                                if (_trackRecorder.IsTracking && _trackRecorder.RecordingTrack.Id == selectedTrack.Id)
                                {
                                    _trackRecorder.StopTracking();
                                }
                                _unitOfWork.TrackPoints.RemoveRange(await _unitOfWork.TrackPoints.Where(p => p.TrackId == selectedTrack.Id).ToArrayAsync());
                                _unitOfWork.Tracks.Remove(await _unitOfWork.Tracks.SingleAsync(t => t.Id == selectedTrack.Id));
                                await _unitOfWork.SaveChangesAsync();
                            }
                        }
                    });
                    foreach (Track selectedTrack in selectedTracks)
                    {
                        Tracks.Remove(selectedTrack);
                    }
                }
            }
        }

        private async Task<bool> AskDelete(string message, string title)
        {
            var dialog = new MessageDialog(message, title);

            dialog.Commands.Add(new UICommand(Messages.TracksPage.YesDelete) { Id = true });
            dialog.Commands.Add(new UICommand(Messages.Dialog.Cancel) { Id = false });

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
            if (SelectedTracks == null || SelectedTracks.Count != 1)
            {
                return;
            }
            _navigationService.NavigateToEditTrack(SelectedTracks[0].Id);
        }

        private async Task ImportGpxTrack()
        {
            using (MarkBusy())
            {
                BusyText = "Importing tracks...";
                IEnumerable<Track> importedTracks = await _trackImporter.ImportTracks();
                foreach (Track importedTrack in importedTracks)
                {
                    Tracks.Add(importedTrack);
                }
            }
        }

        protected override async Task LoadData()
        {
            Tracks = new ObservableCollection<Track>(await _unitOfWork.Tracks.ToListAsync());
        }
    }
}