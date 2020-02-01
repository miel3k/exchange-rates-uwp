using ExchangeRates.App.Base;
using ExchangeRates.Model;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ExchangeRates.App.ExchangeTable
{
    public class ExchangeTableViewModel : BindableViewModel
    {
        public ExchangeTableViewModel()
        {
            var tableDate = localSettings.Values["TableDate"];
            if (tableDate != null)
            {
                SelectedDate = (DateTimeOffset)tableDate;
            }
            Task.Run(LoadExchangeTableAsync);
        }

        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        private bool _isRefreshButtonVisible = false;

        public bool IsRefreshButtonVisible
        {
            get => _isRefreshButtonVisible;
            set => Set(ref _isRefreshButtonVisible, value);
        }

        private bool _isLoading = true;

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        private bool _isDataEmptyMessageVisible = false;

        public bool IsDataEmptyMessageVisible
        {
            get => _isDataEmptyMessageVisible;
            set => Set(ref _isDataEmptyMessageVisible, value);
        }

        private Table _exchangeTable;

        public Table ExchangeTable
        {
            get => _exchangeTable;
            set => Set(ref _exchangeTable, value);
        }

        public ObservableCollection<Rate> Rates { get; private set; } = new ObservableCollection<Rate>();

        private DateTimeOffset _selectedDate = DateTime.Now;

        public DateTimeOffset SelectedDate
        {
            get => _selectedDate;
            set
            {
                Set(ref _selectedDate, value);
                localSettings.Values["TableDate"] = value;
                Task.Run(LoadExchangeTableAsync);
            }
        }

        public DateTimeOffset MaxDate
        {
            get { return DateTime.Now; }
        }

        public async Task LoadExchangeTableAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                IsLoading = true;
                IsRefreshButtonVisible = false;
                IsDataEmptyMessageVisible = true;
                Rates.Clear();
            });

            var tables = await App.Repository.Tables.GetAsync(new DateTime(SelectedDate.Ticks));

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                IsLoading = false;
                IsRefreshButtonVisible = true;
                if (tables != null)
                {
                    IsDataEmptyMessageVisible = true;
                    var table = tables.FirstOrDefault();
                    ExchangeTable = table;
                    foreach (var r in table.Rates)
                    {
                        Rates.Add(r);
                    }
                }
                else
                {
                    IsDataEmptyMessageVisible = false;
                }
            });
        }
    }
}
