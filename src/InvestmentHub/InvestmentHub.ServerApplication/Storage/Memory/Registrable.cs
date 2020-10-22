using Microsoft.Extensions.DependencyInjection;

namespace InvestmentHub.ServerApplication.Storage.Memory
{
    public class Registrable : IRegistrable
    {
        public void RegisterTo(IServiceCollection services)
        {
            services
                .AddSingleton<IPasswordMap, PasswordMap>(); ;
        }
    }
}
