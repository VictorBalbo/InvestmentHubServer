using Microsoft.Extensions.DependencyInjection;

namespace InvestmentHub.ServerApplication.Services
{
    public class Registrable : IRegistrable
    {
        public void RegisterTo(IServiceCollection services)
        {
            services.AddSingleton<IAssetUpdateService, AssetUpdateService>();
        }
    }
}
