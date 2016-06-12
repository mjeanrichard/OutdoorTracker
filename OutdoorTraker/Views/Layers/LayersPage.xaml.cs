﻿using Windows.UI.Xaml.Controls;

using OutdoorTraker.Common;
using OutdoorTraker.Views.Settings;

namespace OutdoorTraker.Views.Layers
{
	public class LayersPageBase : AppPage<LayersViewModel>
	{
	}

	public sealed partial class LayersPage : LayersPageBase
	{
		public LayersPage()
		{
			InitializeComponent();
		}

		private void LayersGridView_OnItemClick(object sender, ItemClickEventArgs e)
		{
			if (e.ClickedItem != null)
			{
				ViewModel.SelectedLayer = (MapLayerModel)e.ClickedItem;
				Frame.GoBack();
			}
		}
	}
}