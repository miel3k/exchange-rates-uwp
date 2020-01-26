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

namespace ExchangeRates.App.ExchangeTable
{
    public class ExchangeTableViewModel : BindableViewModel
    {
        public ExchangeTableViewModel() => Task.Run(GetExchangeTableAsync);

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

        private ObservableCollection<Rate> _rates;

        public ObservableCollection<Rate> Rates
        {
            get => _rates;
            set
            {
                if (_rates != value)
                {
                    value.CollectionChanged += Rates_Changed;
                }
                _rates = value;
                OnPropertyChanged();
            }
        }

        private void Rates_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Rates));
        }

        private DateTimeOffset _effectiveDate;

        public DateTimeOffset EffectiveDate
        {
            get => _effectiveDate;
            set => Set(ref _effectiveDate, value);
        }

        public async Task GetExchangeTableAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);

            var tables = await App.Repository.ExchangeTables.GetAsync(DateTime.Now.Subtract(TimeSpan.FromDays(3)));
            var table = tables.First();

            if (table != null)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    ExchangeTable = table;
                    EffectiveDate = table.EffectiveDate;
                    IsLoading = false;
                    Rates.Clear();
                    foreach (var r in table.Rates)
                    {
                        Rates.Add(r);
                    }
                });
            }
        }
    }
}
