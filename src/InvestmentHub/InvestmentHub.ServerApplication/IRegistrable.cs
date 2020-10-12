using Microsoft.Extensions.DependencyInjection;

namespace InvestmentHub.ServerApplication
{
    /// <summary>
    /// Defines a type registration service.
    /// </summary>
    public interface IRegistrable
    {
        /// <summary>
        /// Registers to the specified serviceCollection.
        /// </summary>
        /// <param name="services">The Service Collection.</param>
        void RegisterTo(IServiceCollection services);
    }
}