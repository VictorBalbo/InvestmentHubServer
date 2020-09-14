using InvestmentHub.Providers.Rico;
using System.Threading.Tasks;

namespace InvestmentHub.ConsoleApplication
{
    public class Program
    {
        public async static Task Main()
        {
            var ricoProvider = new RicoProvider();
            var isLogged = await ricoProvider.LoginAsync("", "", default);
            if (isLogged)
            {
                var savings = await ricoProvider.GetSavingsAsync(default);
            }
        }
    }
}