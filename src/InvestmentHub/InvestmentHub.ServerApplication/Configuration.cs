using System;

namespace InvestmentHub.ServerApplication
{
    public class Configurations : IConfigurations
    {
        public const string ConfigurationKey = "ApplicationConfigurations";

        public string SqlConnectionString { get; set; }
        public TimeSpan DefaultCancellationTokenExpiration { get; set; }
        public string SymmetricKey { get; set; }
    }
}
