using InvestmentHub.Providers;
using InvestmentHub.Providers.Rico;
using Microsoft.Extensions.DependencyInjection;

namespace InvestmentHub.ServerApplication.Providers
{
    internal class Registrable : IRegistrable
    {
        public void RegisterTo(IServiceCollection services)
        {
            services
                .AddSingleton<IProviderContainer, ProviderContainer>()
                .AddSingleton<IProvider, RicoProvider>();
        }
    }
}
