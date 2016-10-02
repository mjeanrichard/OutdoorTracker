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
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Layers;
using OutdoorTracker.Services;
using OutdoorTracker.Tracks;

using UniversalMapControl.Behaviors;
using UniversalMapControl.Interfaces;
using UniversalMapControl.Projections;

namespace OutdoorTracker.Views.Map
{
    public class MapViewModel : BaseViewModel
    {
        private readonly SettingsManager _settingsManager;
        private readonly NavigationService _navigationService;
        private readonly MapDefinitionManager _mapDefinitionManager;
        private readonly GeoLocationService _geoLocationService;
        private readonly TrackRecorder _trackRecorder;
        private readonly IReadonlyUnitOfWork _readonlyUnitOfWork;
        private ILocation _mapCenter;
        private double _zoomLevel;
        private double _heading;
        private HeadingMode _headingMode;

        public MapViewModel()
        {
            LocationModel = new LocationData();
            _mapCenter = new SwissGridLocation(600000, 200000);
            Tracks = new ObservableCollection<Track>();
        }

        public MapViewModel(SettingsManager settingsManager, NavigationService navigationService, MapDefinitionManager mapDefinitionManager, GeoLocationService geoLocationService, TrackRecorder trackRecorder, IReadonlyUnitOfWork readonlyUnitOfWork)
            : this()
        {
            _settingsManager = settingsManager;
            _navigationService = navigationService;
            _mapDefinitionManager = mapDefinitionManager;
            _geoLocationService = geoLocationService;
            _trackRecorder = trackRecorder;
            _readonlyUnitOfWork = readonlyUnitOfWork;
            _trackRecorder.TrackUpdated += (s, e) => TrackRecorderUpdated();

            GotoGpsCommand = new RelayCommand(async () => await GotoCurrentLocation(), () => LocationModel.LocationAccuracy != LocationAccuracy.None);
            StopTrackingCommand = new RelayCommand(StopTracking);
            ShowTracksCommand = new RelayCommand(() => _navigationService.NavigateToTracks());
            ShowSettingsCommand = new RelayCommand(() => _navigationService.NavigateToSettings());
            ShowLayersCommand = new RelayCommand(() => _navigationService.NavigateToLayers());
            ShowLocationInfoCommand = new RelayCommand(() => _navigationService.NavigateToLocationInfo());
            CompassCommand = new RelayCommand(() => ToggleCompass(HeadingMode.Compass));
            NorthUpCommand = new RelayCommand(() => ToggleCompass(HeadingMode.NorthUp));
            LocationModel = _geoLocationService.CurrentLocation;
            LocationModel.PropertyChanged += LocationModelPropertyChanged;

            _headingMode = _settingsManager.HeadingMode;
        }

        public bool ShowNorthUpCommand
        {
            get { return _headingMode != HeadingMode.NorthUp; }
        }

        public bool ShowCompassCommand
        {
            get { return LocationModel.HasCompass && (_headingMode == HeadingMode.NorthUp); }
        }

        public LocationData LocationModel { get; private set; }
        public MapConfiguration MapConfiguration { get; private set; }

        public RelayCommand GotoGpsCommand { get; private set; }
        public RelayCommand ShowLayersCommand { get; private set; }
        public RelayCommand ShowSettingsCommand { get; private set; }
        public RelayCommand ShowLocationInfoCommand { get; private set; }
        public RelayCommand ShowTracksCommand { get; private set; }
        public RelayCommand StopTrackingCommand { get; private set; }
        public RelayCommand NorthUpCommand { get; private set; }
        public RelayCommand CompassCommand { get; private set; }

        public ObservableCollection<Track> Tracks { get; set; }

        public bool IsRecording { get; set; }

        public ILocation MapCenter
        {
            get { return _mapCenter; }
            set
            {
                _mapCenter = value;
                _settingsManager.CenterOnPosition = false;
            }
        }

        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set
            {
                _zoomLevel = value;
                OnPropertyChanged();
            }
        }

        public double Heading
        {
            get { return _headingMode == HeadingMode.NorthUp ? 0 : _heading; }
            set
            {
                _heading = value;
                OnPropertyChanged();
            }
        }

        public bool RotationEnabled
        {
            get { return _settingsManager.RotationEnabled; }
        }

        public bool ShowCurrentPosition
        {
            get { return _settingsManager.ShowLocation && LocationModel.IsLocationValid; }
        }

        public bool ShowAccuracyCircle
        {
            get { return _settingsManager.ShowAccuracy && ShowCurrentPosition && (LocationModel.LocationAccuracy == LocationAccuracy.High); }
        }

        public bool IsMapInitialized { get; private set; }

        public void UpdateTouchInput(TouchMapEventArgs inputData)
        {
            if (Math.Abs(Heading - inputData.Heading) > 0.01)
            {
                Heading = inputData.Heading;
                _headingMode = HeadingMode.Manual;
                OnPropertyChanged(nameof(ShowNorthUpCommand));
                OnPropertyChanged(nameof(ShowCompassCommand));
            }
            _settingsManager.CenterOnPosition = false;
        }

        private void ToggleCompass(HeadingMode compass)
        {
            _headingMode = compass;
            OnPropertyChanged(nameof(Heading));
            OnPropertyChanged(nameof(ShowNorthUpCommand));
            OnPropertyChanged(nameof(ShowCompassCommand));
        }

        private void StopTracking()
        {
            _trackRecorder.StopTracking();
        }

        private void TrackRecorderUpdated()
        {
            IsRecording = _trackRecorder.IsTracking;
            OnPropertyChanged(nameof(IsRecording));
            if (_trackRecorder.IsTracking)
            {
                Track trackingTrack = Tracks.FirstOrDefault(t => t.Id == _trackRecorder.RecordingTrack.Id);
                if (trackingTrack == null)
                {
                    Tracks.Add(_trackRecorder.RecordingTrack);
                }
                else if (trackingTrack != _trackRecorder.RecordingTrack)
                {
                    Tracks.Remove(trackingTrack);
                    Tracks.Add(_trackRecorder.RecordingTrack);
                }
            }
        }

        private void LocationModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == nameof(LocationData.LocationAccuracy)) || (e.PropertyName == nameof(LocationData.Accuracy)))
            {
                GotoGpsCommand.RaiseCanExecuteChanged();
                OnPropertyChanged(nameof(ShowAccuracyCircle));
            }
            else if (e.PropertyName == nameof(LocationData.IsLocationValid))
            {
                OnPropertyChanged(nameof(ShowCurrentPosition));
                OnPropertyChanged(nameof(ShowAccuracyCircle));
            }
            else if (e.PropertyName == nameof(LocationData.Compass))
            {
                if (_headingMode == HeadingMode.Compass)
                {
                    _heading = -LocationModel.Compass.GetValueOrDefault(0);
                    OnPropertyChanged(nameof(Heading));
                }
            }
            if (e.PropertyName == nameof(LocationData.HasCompass))
            {
                OnPropertyChanged(nameof(ShowCompassCommand));
                OnPropertyChanged(nameof(ShowNorthUpCommand));
            }
        }

        private async Task GotoCurrentLocation()
        {
            if (LocationModel.LocationAccuracy != LocationAccuracy.None)
            {
                await SetMapCenter(LocationModel.Location);
                _settingsManager.CenterOnPosition = true;
            }
        }

        private async Task SetMapCenter(ILocation location)
        {
            _mapCenter = location;
            await DispatcherHelper.InvokeOnUiAsync(() => OnPropertyChanged(nameof(MapCenter)));
        }

        protected override async Task InitializeInternal()
        {
            await ConfigureMap().ConfigureAwait(false);

            await _geoLocationService.Initialize().ConfigureAwait(false);

            IsMapInitialized = true;
            OnPropertyChanged(nameof(IsMapInitialized));

            await LoadTracks().ConfigureAwait(false);
        }

        private async Task LoadTracks()
        {
            using (MarkBusy())
            {
                List<Track> collection = await _readonlyUnitOfWork.Tracks.Where(t => t.ShowOnMap).Include(t => t.Points).ToListAsync().ConfigureAwait(false);
                await DispatcherHelper.InvokeOnUiAsync(() =>
                {
                    Tracks = new ObservableCollection<Track>(collection);
                    TrackRecorderUpdated();
                    OnPropertyChanged(nameof(Tracks));
                });
            }
        }

        private async Task ConfigureMap()
        {
            MapConfiguration = await _mapDefinitionManager.GetCurrentConfiguration().ConfigureAwait(false);
            OnPropertyChanged(nameof(MapConfiguration));

            ILocation lastMapCenter = _settingsManager.GetLastMapCenter();
            await SetMapCenter(lastMapCenter);

            double zoomFactor = _settingsManager.ZoomFactor;
            double cartesianScaleFactor = MapConfiguration.Projection.CartesianScaleFactor(lastMapCenter);
            ZoomLevel = MapConfiguration.Projection.GetZoomLevel(zoomFactor / cartesianScaleFactor);
            Heading = _settingsManager.LastHeading;
        }

        public override Task Leave()
        {
            SaveMapPosition();
            return Task.CompletedTask;
        }

        public override Task Suspending()
        {
            SaveMapPosition();
            return Task.CompletedTask;
        }

        private void SaveMapPosition()
        {
            _settingsManager.HeadingMode = _headingMode;
            if (MapConfiguration != null)
            {
                double zoomFactor = MapConfiguration.Projection.GetZoomFactor(_zoomLevel);
                double cartesianScaleFactor = MapConfiguration.Projection.CartesianScaleFactor(MapCenter);
                _settingsManager.SetLastPosition(MapCenter, zoomFactor * cartesianScaleFactor, Heading);
            }
        }
    }
}