using System;

using Microsoft.Graphics.Canvas;

namespace OutdoorTraker.Controls
{
	public interface ICanvasItem
	{
		event EventHandler LayoutChanged; 
		void Draw(CanvasDrawingSession drawingSession, CanvasItemsLayer canvasItemsLayer);
		void ParentInvalidated();
	}
}