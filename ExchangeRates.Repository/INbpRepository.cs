using ExchangeRates.Model;

namespace ExchangeRates.Repository
{
    public interface INbpRepository
    {
        ITableRepository Tables { get; }
        ICurrencyRepository Currency { get; }
    }
}
