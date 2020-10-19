using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace InvestmentHub.ServerApplication
{
    public class Registrable : IRegistrable
    {
        public void RegisterTo(IServiceCollection services)
        {
            var registrables = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t =>
                    t != GetType() &&
                    !t.IsAbstract &&
                    typeof(IRegistrable).IsAssignableFrom(t))
                .Select(t => (IRegistrable)Activator.CreateInstance(t));

            foreach (var registrable in registrables)
            {
                registrable.RegisterTo(services);
            }
        }
    }
}
