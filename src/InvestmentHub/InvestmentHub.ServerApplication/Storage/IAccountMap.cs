using InvestmentHub.Models;
using Take.Elephant;

namespace InvestmentHub.ServerApplication.Storage
{
    public interface IAccountMap : IMap<string, Account>
    {
    }
}