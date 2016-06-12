using System;

using Windows.UI.Core;
using Windows.UI.Xaml;

namespace OutdoorTraker.Common
{
	public static class DispatcherHelper
	{
		private static CoreDispatcher _uiDispatcher;

		public static void Init()
		{
			_uiDispatcher = Window.Current.Dispatcher;
		}

		public static void InvokeOnUI(Action action)
		{
			if (action == null)
			{
				return;
			}

			if (_uiDispatcher == null || _uiDispatcher.HasThreadAccess)
			{
				action();
			}
			else
			{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				_uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			}
		}
	}
}