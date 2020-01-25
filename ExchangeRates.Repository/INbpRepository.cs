using ExchangeRates.Model;

namespace ExchangeRates.Repository
{
    public interface INbpRepository
    {
        IExchangeTableRepository ExchangeTables { get; }
    }
}
