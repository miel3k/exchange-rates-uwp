using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRates.Model
{
    public interface ITableRepository
    {
        Task<IEnumerable<Table>> GetAsync(DateTime date);
    }
}
