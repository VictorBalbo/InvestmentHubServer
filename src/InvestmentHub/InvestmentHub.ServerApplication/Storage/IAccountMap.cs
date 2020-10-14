using InvestmentHub.Models;
using Take.Elephant;

namespace InvestmentHub.ServerApplication.Storage
{
    internal interface IAccountMap : IMap<string, Account>
    {
    }
}