using Microsoft.Extensions.DependencyInjection;

namespace InvestmentHub.ServerApplication.Managers
{
    public class Registrable : IRegistrable
    {
        public void RegisterTo(IServiceCollection services)
        {
            services
                .AddSingleton<IAccountManager, AccountManager>()
                .AddSingleton<IAssetManager, AssetManager>();
        }
    }
}