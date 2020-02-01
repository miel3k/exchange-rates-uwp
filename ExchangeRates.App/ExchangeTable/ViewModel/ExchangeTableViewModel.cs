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
            var tableDate =  localSettings.Values["TableDate"];
            if(tableDate != null)
            {
                SelectedDate = (DateTimeOffset) tableDate;
            }
            Task.Run(LoadExchangeTableAsync);
        }

        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
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
                //TODO Check if is Sunday etc. or inform user
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
                Rates.Clear();
            });

            var tables = await App.Repository.Tables.GetAsync(new DateTime(SelectedDate.Ticks));
            var table = tables.First();

            if (table != null)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    ExchangeTable = table;
                    IsLoading = false;
                    foreach (var r in table.Rates)
                    {
                        Rates.Add(r);
                    }
                });
            }
        }
    }
}
