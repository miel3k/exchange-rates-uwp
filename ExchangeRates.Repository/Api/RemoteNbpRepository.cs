using ExchangeRates.Model;

namespace ExchangeRates.Repository.Remote
{
    public class RemoteNbpRepository : INbpRepository
    {
        private readonly string _url;

        public RemoteNbpRepository(string url) => _url = url;

        public IExchangeTableRepository ExchangeTables => new RemoteExchangeTableRepository(_url);
    }
}
