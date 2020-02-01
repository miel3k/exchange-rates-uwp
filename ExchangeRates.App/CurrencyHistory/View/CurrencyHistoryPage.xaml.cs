using ExchangeRates.App.CurrencyHistory.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace ExchangeRates.App.CurrencyHistory
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class CurrencyHistoryPage : Page
    {

        public CurrencyHistoryPage()
        {
            InitializeComponent();
            KeyboardAccelerator GoBack = new KeyboardAccelerator
            {
                Key = VirtualKey.GoBack
            };
            GoBack.Invoked += BackInvoked;
            KeyboardAccelerator AltLeft = new KeyboardAccelerator
            {
                Key = VirtualKey.Left
            };
            AltLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(GoBack);
            this.KeyboardAccelerators.Add(AltLeft);
            // ALT routes here
            AltLeft.Modifiers = VirtualKeyModifiers.Menu;
        }

        private CurrencyHistoryViewModel _viewModel;

        public CurrencyHistoryViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != value)
                {
                    _viewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currencyCode = (string)e.Parameter;
            ViewModel = new CurrencyHistoryViewModel(currencyCode);
            base.OnNavigatedTo(e);
        }

        private void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                (sender as CommandBar).DefaultLabelPosition = CommandBarDefaultLabelPosition.Bottom;
            }
            else
            {
                (sender as CommandBar).DefaultLabelPosition = CommandBarDefaultLabelPosition.Right;
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime fromDate = new DateTime(ViewModel.StartDate.Ticks);
            DateTime toDate = new DateTime(ViewModel.EndDate.Ticks);
            int dateComparison = DateTime.Compare(fromDate.Date, toDate.Date);
            if (dateComparison >= 0)
            {
                inAppNotification.Show("Incorrect period provided!", 3000);
                return;
            }
            await ViewModel.LoadCurrencyTableAsync();
        }

        private void SaveChartButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryChart.Save();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            On_BackRequested();
        }

        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private void CalendarDatePicker_DayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs e)
        {
            bool isBlackout;
            if (e.Item.Date.DayOfWeek == DayOfWeek.Sunday || e.Item.Date.DayOfWeek == DayOfWeek.Saturday)
            {
                isBlackout = true;
            }
            else
            {
                isBlackout = false;
            }
            e.Item.IsBlackout = isBlackout;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
