using System.Threading.Tasks;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Microsoft.Practices.Unity;

namespace OutdoorTraker.Common
{
	public abstract class AppPage<TModel> : Page where TModel : BaseViewModel
	{
		private TModel _viewModel;
		private IUnityContainer _pageContainer;

		public TModel ViewModel
		{
			get { return _viewModel; }
			protected set
			{
				_viewModel = value;
				DataContext = value;
			}
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			_pageContainer = DependencyContainer.Current.CreateChildContainer();
			ViewModel = _pageContainer.Resolve<TModel>(new TypedParameterOverride<NavigationEventArgs>(e));

			Frame rootFrame = Window.Current.Content as Frame;
			if (rootFrame != null && rootFrame.CanGoBack)
			{
				SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
			}
			else
			{
				SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
			}

			using (ViewModel.MarkBusy())
			{
				await ViewModel.Initialize();
			}
			InitializeCompleted();
		}

		protected virtual void InitializeCompleted()
		{
		}

		protected override async void OnNavigatedFrom(NavigationEventArgs e)
		{
			await ViewModel.Leave();
			_pageContainer.Dispose();
		}
	}
}