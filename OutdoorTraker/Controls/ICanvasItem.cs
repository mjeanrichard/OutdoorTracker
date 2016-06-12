using System;

using Microsoft.Graphics.Canvas;

using UniversalMapControl;

namespace OutdoorTraker.Controls
{
	public interface ICanvasItem
	{
		event EventHandler LayoutChanged; 
		void Draw(CanvasDrawingSession drawingSession, Map parentMap, float scale);
		void ParentInvalidated();
	}
}