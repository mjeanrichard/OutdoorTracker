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
using System.Diagnostics;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Microsoft.HockeyApp;
using Microsoft.Practices.Unity;

using OutdoorTracker.Common;
using OutdoorTracker.Database;
using OutdoorTracker.Logging;
using OutdoorTracker.Services;
using OutdoorTracker.Views.Map;

namespace OutdoorTracker
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            DependencyContainer.InitializeContainer(this);
            InitializeComponent();
            Suspending += OnSuspending;
            Resuming += OnResuming;
            UnhandledException += OnUnhandledException;

            HockeyClient.Current.Configure("b2c844d2de1245bf8e2495ed20350fd8");
        }

        public Frame RootFrame { get; set; }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            OutdoorTrackerEvents.Log.UnhandledException(e.Message, e.Exception);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            DispatcherHelper.Init();

            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            RootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (RootFrame == null)
            {
                RootFrame = new Frame();
                RootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = RootFrame;
            }

            if (e.PreviousExecutionState == ApplicationExecutionState.NotRunning)
            {
                await DependencyContainer.Current.Resolve<DbInitializer>().InitDatabase();
            }

            if (RootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page
                RootFrame.Navigate(typeof(MapPage), e.Arguments);
            }


            Window.Current.Activate();

            await CheckTrackingSession();
        }

        private async Task CheckTrackingSession()
        {
            TrackRecorder trackRecorder = DependencyContainer.Current.Resolve<TrackRecorder>();
            await trackRecorder.CheckExistingSession().ConfigureAwait(false);
        }


        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            BaseViewModel baseViewModel = (((Frame)Window.Current.Content)?.Content as IAppPage)?.ViewModel;
            if (baseViewModel != null)
            {
                await baseViewModel.Suspending();
            }

            deferral.Complete();
        }

        private async void OnResuming(object sender, object e)
        {
            BaseViewModel baseViewModel = (((Frame)Window.Current.Content)?.Content as IAppPage)?.ViewModel;
            if (baseViewModel != null)
            {
                await baseViewModel.Resuming();
            }
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                return;
            }

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (rootFrame.CanGoBack && (e.Handled == false))
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }
    }
}