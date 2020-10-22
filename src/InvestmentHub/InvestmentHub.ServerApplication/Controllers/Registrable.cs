using Microsoft.Extensions.DependencyInjection;

namespace InvestmentHub.ServerApplication.Controllers
{
    public class Registrable : IRegistrable
    {
        public void RegisterTo(IServiceCollection services)
        {
            services
                .AddSingleton<AuthenticationController, AuthenticationController>();
        }
    }
}
