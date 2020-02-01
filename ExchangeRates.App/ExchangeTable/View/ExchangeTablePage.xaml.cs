using ExchangeRates.App.CurrencyHistory;
using ExchangeRates.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ExchangeRates.App.ExchangeTable
{
    public sealed partial class ExchangeTablePage : Page
    {
        public ExchangeTablePage() => InitializeComponent();

        private ExchangeTableViewModel _viewModel;

        public ExchangeTableViewModel ViewModel
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
            ViewModel = new ExchangeTableViewModel();
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

        private async void RefreshButton_Click(object sender, RoutedEventArgs e) =>
            await ViewModel.LoadExchangeTableAsync();

        private void RateItem_Click(object sender, ItemClickEventArgs e)
        {
            Rate item = (Rate)e.ClickedItem;
            Frame.Navigate(typeof(CurrencyHistoryPage), item.Code);
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
