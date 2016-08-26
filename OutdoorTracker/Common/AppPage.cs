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

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Microsoft.Practices.Unity;

namespace OutdoorTracker.Common
{
    public abstract class AppPage<TModel> : Page, IAppPage where TModel : BaseViewModel
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

        BaseViewModel IAppPage.ViewModel => ViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _pageContainer = DependencyContainer.Current.CreateChildContainer();
            ViewModel = _pageContainer.Resolve<TModel>(new TypedParameterOverride<NavigationEventArgs>(e));

            Frame rootFrame = Window.Current.Content as Frame;
            if ((rootFrame != null) && rootFrame.CanGoBack)
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