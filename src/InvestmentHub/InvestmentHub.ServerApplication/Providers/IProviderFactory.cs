using InvestmentHub.Providers;

namespace InvestmentHub.ServerApplication.Providers
{
    public interface IProviderFactory
    {
        IProvider GetProvider(string name);
    }
}
