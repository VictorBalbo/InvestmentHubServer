using Microsoft.Extensions.DependencyInjection;

namespace InvestmentHub.ServerApplication.Storage.Sql
{
    public class Registrable : IRegistrable
    {
        public void RegisterTo(IServiceCollection services)
        {
            services
                .AddSingleton<IAccountMap, AccountMap>()
                .AddSingleton<IAssetSetMap, AssetSetMap>();
        }
    }
}