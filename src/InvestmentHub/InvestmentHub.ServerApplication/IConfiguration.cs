using System;

namespace InvestmentHub.ServerApplication
{
    public interface IConfigurations
    {
        public string SqlConnectionString { get; }
        public TimeSpan DefaultCancellationTokenExpiration { get; }
        public string SymmetricKey { get; }
    }
}