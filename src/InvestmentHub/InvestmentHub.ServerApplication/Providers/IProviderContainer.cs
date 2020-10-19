using InvestmentHub.Providers;

namespace InvestmentHub.ServerApplication.Providers
{
    public interface IProviderContainer
    {
        IProvider GetProvider(string name);
    }
}
