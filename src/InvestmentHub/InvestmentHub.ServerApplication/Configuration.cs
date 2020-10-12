using System;

namespace InvestmentHub.ServerApplication
{
    public class Configurations : IConfigurations
    {
        public const string ConfigurationKey = "ApplicationConfigurations";

        public string SqlConnectionString => @"Server=(localdb)\MSSQLLocalDB;Database=Elephant;Integrated Security=true";
        public TimeSpan DefaultCancellationTokenExpiration => TimeSpan.FromMinutes(1);
        public string SymmetricKey => "3037b4d3-9b4e-46ae-8249-3efa51ca5afc";
    }
}