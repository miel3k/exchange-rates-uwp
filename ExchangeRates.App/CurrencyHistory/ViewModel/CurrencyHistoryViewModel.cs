﻿using ExchangeRates.App.Base;
using ExchangeRates.Model;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ExchangeRates.App.CurrencyHistory.ViewModel
{
    public class CurrencyHistoryViewModel : BindableViewModel, IProgress<float>
    {
        public CurrencyHistoryViewModel(string currencyCode)
        {
            CurrencyCode = currencyCode;
            var fromDate = localSettings.Values["FromDate"];
            if (fromDate != null)
            {
                StartDate = (DateTimeOffset)fromDate;
            }
            var toDate = localSettings.Values["ToDate"];
            if (toDate != null)
            {
                EndDate = (DateTimeOffset)toDate;
            }
            Task.Run(LoadCurrencyTableAsync);
        }

        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        private bool _isChartVisible = false;

        public bool IsChartVisible
        {
            get => _isChartVisible;
            set => Set(ref _isChartVisible, value);
        }

        private float _progress = 0.0f;

        public float Progress
        {
            get => _progress;
            set => Set(ref _progress, value);
        }

        private string _currencyCode;

        public string CurrencyCode
        {
            get => _currencyCode;
            set => Set(ref _currencyCode, value);
        }

        private DateTimeOffset _startDate = DateTime.Now.Subtract(TimeSpan.FromDays(5));

        public DateTimeOffset StartDate
        {
            get => _startDate;
            set
            {
                //TODO Check if is Sunday etc. or inform user
                Set(ref _startDate, value);
                localSettings.Values["FromDate"] = value;
                Task.Run(LoadCurrencyTableAsync);
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
                localSettings.Values["ToDate"] = value;
                Task.Run(LoadCurrencyTableAsync);
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

        public async Task<bool> IsFilePresent(string fileName)
        {
            var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
            return item != null;
        }

        public async Task LoadCurrencyTableAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Progress = 0.0f;
                IsChartVisible = false;
            });
            bool fileExists = await IsFilePresent("file.txt");
            if (fileExists)
            {
                StorageFile oldFile = await ApplicationData.Current.LocalFolder.GetFileAsync("file.txt");
                await oldFile.DeleteAsync();
            }
            StorageFile newFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("file.txt");
            CurrencyTable currencyTable = await App.Repository.Currency.GetAsync(
                _currencyCode,
                new DateTime(_startDate.Ticks),
                new DateTime(_endDate.Ticks),
                newFile.Path,
                this
            );
            if (currencyTable != null)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    IsChartVisible = true;
                    Table = currencyTable;
                });
            }
        }

        private float _masterProgress = 0.0f;

        public async Task RefreshProgress()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                var f = (float)_masterProgress * 100;
                var i = (int)f;
                Progress = (float)i;
            });
        }

        public void Report(float value)
        {
            _masterProgress = value;
            Task.Run(RefreshProgress);
        }
    }
}
