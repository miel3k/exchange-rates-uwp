namespace ExchangeRates.Repository
{
    public static class Constants
    {
        public const string NbpApiUrl = @"http://api.nbp.pl/api/";
        public const string CannotReadStreamError = "Cannot read stream";
        public const string CannotWriteStreamError = "Cannot write stream";
        public const string DatePattern = @"yyyy-MM-dd";
        public const int DefaultBufferSize = 64;
    }
}
