using ExchangeRates.Model;
using ExchangeRates.Repository.Api;

namespace ExchangeRates.Repository.Remote
{
    public class RemoteNbpRepository : INbpRepository
    {
        private readonly string _url;

        public RemoteNbpRepository(string url) => _url = url;

        public ITableRepository Tables => new RemoteTableRepository(_url);

        public ICurrencyRepository Currency => new RemoteCurrencyRepository(_url);
    }
}
