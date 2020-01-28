using ExchangeRates.App.Base;
using ExchangeRates.Model;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.App.CurrencyHistory.ViewModel
{
    public class CurrencyHistoryViewModel : BindableViewModel
    {
        public CurrencyHistoryViewModel(string currencyCode)
        {
            CurrencyCode = currencyCode;
            Task.Run(LoadExchangeTableAsync);
        }

        private string _currencyCode;

        public string CurrencyCode
        {
            get => _currencyCode;
            set => Set(ref _currencyCode, value);
        }

        private DateTimeOffset _startDate = DateTime.Now;

        public DateTimeOffset StartDate
        {
            get => _startDate;
            set
            {
                //TODO Check if is Sunday etc. or inform user
                Set(ref _startDate, value);
                Task.Run(LoadExchangeTableAsync);
            }
        }

        private DateTimeOffset _endDate = DateTime.Now;

        public DateTimeOffset EndDate
        {
            get => _endDate;
            set
            {
                //TODO Check if is Sunday etc. or inform user
                Set(ref _endDate, value);
                Task.Run(LoadExchangeTableAsync);
            }
        }

        private CurrencyTable _table;

        public CurrencyTable Table
        {
            get => _table;
            set => Set(ref _table, value);
        }

        public DateTimeOffset MaxDate
        {
            get { return DateTime.Now; }
        }

        public DateTimeOffset MinDate
        {
            get { return new DateTime(2002, 1, 2); }
        }

        public async Task LoadExchangeTableAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                //IsLoading = true;
                //Rates.Clear();
            });

            var currencyTable = await App.Repository.Currency.GetAsync(_currencyCode, new DateTime(_startDate.Ticks), new DateTime(_endDate.Ticks));

            if (currencyTable != null)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Table = currencyTable;
                    //IsLoading = false;
                    foreach (var r in currencyTable.CurrencyRates)
                    {
                        //Rates.Add(r);
                    }
                });
            }
        }
    }
}
