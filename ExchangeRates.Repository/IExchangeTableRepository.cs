using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Model
{
    public interface IExchangeTableRepository
    {

        Task<Table> GetAsync(DateTime date);
    }
}
