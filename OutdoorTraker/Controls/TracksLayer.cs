﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

using Windows.UI;
using Windows.UI.Xaml;

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

using OutdoorTraker.Common;
using OutdoorTraker.Tracks;

using UniversalMapControl;
using UniversalMapControl.Interfaces;

namespace OutdoorTraker.Controls
{
	public class TracksLayer : CanvasMapLayer
	{
		public static readonly DependencyProperty TracksProperty = DependencyProperty.Register("Tracks", typeof(object), typeof(TracksLayer), new PropertyMetadata(default(object), TracksChanged));

		private volatile bool _isPathValid = false;

		private static void TracksChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TracksLayer tracksLayer = d as TracksLayer;
			if (tracksLayer != null)
			{
				tracksLayer.OnTracksChanged((ObservableCollection<Track>)e.OldValue, (ObservableCollection<Track>)e.NewValue);
			}
		}

		private void OnTracksChanged(ObservableCollection<Track> oldValue, ObservableCollection<Track> newValue)
		{
			if (oldValue != null)
			{
				foreach (Track oldTrack in oldValue)
				{
					oldTrack.Points.CollectionChanged -= TrackPointsCollectionChanged;
				}
				oldValue.CollectionChanged -= TracksCollectionChanged;
			}
			if (newValue != null)
			{
				newValue.CollectionChanged += TracksCollectionChanged;
				foreach (Track newTrack in newValue)
				{
					newTrack.Points.CollectionChanged += TrackPointsCollectionChanged;
				}
			}
			InvalidateTrack();
		}

		private void TracksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (Track oldTrack in e.OldItems)
				{
					oldTrack.Points.CollectionChanged -= TrackPointsCollectionChanged;
				}
			}
			if (e.NewItems != null)
			{
				foreach (Track newTrack in e.NewItems)
				{
					newTrack.Points.CollectionChanged += TrackPointsCollectionChanged;
				}
			}
			InvalidateTrack();
		}

		private void TrackPointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			InvalidateTrack();
		}

		public void InvalidateTrack()
		{
			_isPathValid = false;
			DispatcherHelper.InvokeOnUI(Invalidate);
		}

		private void BuildPathGeometry(CanvasDevice device)
		{
			List<CanvasGeometry> oldList = _trackPaths;
			_trackPaths = new List<CanvasGeometry>();
			if (oldList != null)
			{
				foreach (CanvasGeometry trackPath in oldList)
				{
					trackPath.Dispose();
				}
			}
			
			IProjection projection = ParentMap.ViewPortProjection;

			foreach (Track track in Tracks)
			{
				if (track.Points.Any())
				{
					CanvasGeometry trackPath = BuildPathFromTrack(projection, track, device);
					_trackPaths.Add(trackPath);
				}
			}
			_isPathValid = true;
		}

		private CanvasGeometry BuildPathFromTrack(IProjection projection, Track first, CanvasDevice device)
		{
			CanvasPathBuilder pathBuilder = new CanvasPathBuilder(device);
			TrackPoint[] trackPoints = first.Points.OrderBy(p => p.Number).ToArray();
			CartesianPoint point = projection.ToCartesian(trackPoints[0].Location);
			pathBuilder.BeginFigure(point.ToVector());
			for (int i = 1; i < trackPoints.Length; i++)
			{
				TrackPoint trackPoint = trackPoints[i];
				point = projection.ToCartesian(trackPoint.Location);
				pathBuilder.AddLine(point.ToVector());
			}
			pathBuilder.EndFigure(CanvasFigureLoop.Open);
			return CanvasGeometry.CreatePath(pathBuilder);
		}

		private List<CanvasGeometry> _trackPaths;

		public ObservableCollection<Track> Tracks
		{
			get { return (ObservableCollection<Track>)GetValue(TracksProperty); }
			set { SetValue(TracksProperty, value); }
		}

		protected override void DrawInternal(CanvasDrawingSession drawingSession, Map parentMap)
		{
			if (Tracks == null)
			{
				return;
			}
			if (!_isPathValid)
			{
				BuildPathGeometry(drawingSession.Device);
			}
			float scale = (float)(2 / parentMap.ViewPortProjection.GetZoomFactor(parentMap.ZoomLevel));
			foreach (CanvasGeometry trackPath in _trackPaths)
			{
				drawingSession.DrawGeometry(trackPath, Colors.Red, scale);
			}
		}
	}
}