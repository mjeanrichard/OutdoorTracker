using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Microsoft.Graphics.Canvas;

using UniversalMapControl;

namespace OutdoorTraker.Controls
{
	public class CanvasItemsLayer : CanvasMapLayer
	{
		public CanvasItemsLayer()
		{
			Items = new ObservableCollection<ICanvasItem>();
			Items.CollectionChanged += ItemsChanged;
		}

		private void ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
			{
				foreach (ICanvasItem oldItem in e.OldItems)
				{
					oldItem.LayoutChanged -= ItemLayoutChanged;
				}
			}
			if (e.NewItems != null)
			{
				foreach (ICanvasItem canvasItem in e.NewItems)
				{
					canvasItem.LayoutChanged += ItemLayoutChanged;
				}
			}
		}

		private void ItemLayoutChanged(object sender, EventArgs e)
		{
			Invalidate();
		}

		public ObservableCollection<ICanvasItem> Items { get; set; }

		protected override void DrawInternal(CanvasDrawingSession drawingSession, Map parentMap)
		{
			foreach (ICanvasItem canvasItem in Items)
			{
				canvasItem.Draw(drawingSession, this);
			}
		}

		protected override void InvalidateScaledValues()
		{
			foreach (ICanvasItem canvasItem in Items)
			{
				canvasItem.ParentInvalidated();
			}
		}
	}
}