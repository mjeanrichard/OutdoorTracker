using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;

using Microsoft.EntityFrameworkCore;

using OutdoorTraker.Common;
using OutdoorTraker.Database;
using OutdoorTraker.Layers;
using OutdoorTraker.Services;
using OutdoorTraker.Tracks;

using UniversalMapControl.Behaviors;
using UniversalMapControl.Interfaces;
using UniversalMapControl.Projections;

namespace OutdoorTraker.Views.Map
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
			SetMapCenter(new SwissGridLocation(600000, 200000));
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

			GotoGpsCommand = new RelayCommand(GotoCurrentLocation);
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
			get { return _geoLocationService.HasCompass && _headingMode == HeadingMode.NorthUp; }
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
			get { return _settingsManager.ShowAccuracy && ShowCurrentPosition && LocationModel.LocationAccuracy == LocationAccuracy.High; }
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
			if (e.PropertyName == nameof(LocationData.LocationAccuracy) || e.PropertyName == nameof(LocationData.Accuracy))
			{
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
		}

		private void GotoCurrentLocation()
		{
			if (LocationModel.LocationAccuracy != LocationAccuracy.None)
			{
				SetMapCenter(LocationModel.Location);
				_settingsManager.CenterOnPosition = true;
			}
		}

		private void SetMapCenter(ILocation location)
		{
			_mapCenter = location;
			DispatcherHelper.InvokeOnUI(() => OnPropertyChanged(nameof(MapCenter)));
		}

		protected override async Task InitializeInternal()
		{
			await ConfigureMap();

			await _geoLocationService.Initialize();

			Tracks = new ObservableCollection<Track>(await _readonlyUnitOfWork.Tracks.Where(t => t.ShowOnMap).Include(t => t.Points).ToListAsync());
			TrackRecorderUpdated();
			OnPropertyChanged(nameof(Tracks));

			IsMapInitialized = true;
			OnPropertyChanged(nameof(IsMapInitialized));
		}

		private async Task ConfigureMap()
		{
			MapConfiguration = await _mapDefinitionManager.GetCurrentConfiguration();
			OnPropertyChanged(nameof(MapConfiguration));

			ILocation lastMapCenter = _settingsManager.GetLastMapCenter();
			SetMapCenter(lastMapCenter);

			double zoomFactor = _settingsManager.ZoomFactor;
			double cartesianScaleFactor = MapConfiguration.Projection.CartesianScaleFactor(lastMapCenter);
			ZoomLevel = MapConfiguration.Projection.GetZoomLevel(zoomFactor / cartesianScaleFactor);
			Heading = _settingsManager.LastHeading;
		}

		public override Task Leave()
		{
			_settingsManager.HeadingMode = _headingMode;
			LocationModel.PropertyChanged -= LocationModelPropertyChanged;
			if (MapConfiguration != null)
			{
				double zoomFactor = MapConfiguration.Projection.GetZoomFactor(_zoomLevel);
				double cartesianScaleFactor = MapConfiguration.Projection.CartesianScaleFactor(MapCenter);
				_settingsManager.SetLastPosition(MapCenter, zoomFactor * cartesianScaleFactor, Heading);
			}
			return Task.CompletedTask;
		}
	}
}