using ExchangeRates.App.ExchangeTable;
using ExchangeRates.Repository;
using ExchangeRates.Repository.Remote;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ExchangeRates.App
{
    sealed partial class App : Application
    {
        public static INbpRepository Repository { get; private set; }
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            SetupRepository();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(ExchangeTablePage));
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var navigationState = localSettings.Values["NavigationState"];
            if (navigationState != null)
            {
                rootFrame.SetNavigationState((string)navigationState);
            }
            else
            {
                rootFrame.Navigate(typeof(ExchangeTablePage));
            }
        }
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (Window.Current.Content is Frame frame)
            {
                string currentNavigationState = frame.GetNavigationState();
                localSettings.Values["NavigationState"] = currentNavigationState;
            }
            else
            {
                localSettings.Values.Remove("NavigationState");
            }
            deferral.Complete();
        }

        public static void SetupRepository() =>
            Repository = new RemoteNbpRepository(Constants.NbpApiUrl);
    }
}
