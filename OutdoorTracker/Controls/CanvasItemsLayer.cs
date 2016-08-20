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
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Microsoft.Graphics.Canvas;

using UniversalMapControl;

namespace OutdoorTracker.Controls
{
	public class CanvasItemsLayer : CanvasMapLayer
	{
		public CanvasItemsLayer()
		{
			Items = new ObservableCollection<ICanvasItem>();
			Items.CollectionChanged += ItemsChanged;
		}

		public ObservableCollection<ICanvasItem> Items { get; set; }

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