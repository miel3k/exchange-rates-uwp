using ExchangeRates.App.Base;
using ExchangeRates.Model;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
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

        public async Task GetExchangeTableAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);

            var table = await App.Repository.ExchangeTables.GetAsync(DateTime.Now);

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                ExchangeTable = table;
                IsLoading = false;
            });
        }
    }
}
