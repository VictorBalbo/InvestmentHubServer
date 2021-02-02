using Microsoft.Extensions.DependencyInjection;

namespace InvestmentHub.ServerApplication.Providers
{
    internal class Registrable : IRegistrable
    {
        public void RegisterTo(IServiceCollection services)
        {
            services
                .AddSingleton<IProviderFactory, ProviderFactory>();
        }
    }
}
