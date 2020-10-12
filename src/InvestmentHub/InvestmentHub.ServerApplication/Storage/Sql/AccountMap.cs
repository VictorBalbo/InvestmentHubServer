using InvestmentHub.Models;
using Take.Elephant.Sql;
using Take.Elephant.Sql.Mapping;

namespace InvestmentHub.ServerApplication.Storage.Sql
{
    public class AccountMap : SqlMap<string, Account>, IAccountMap
    {
        public const string TABLE_NAME = "Accounts";

        public static ITable AccountTable = TableBuilder
            .WithName(TABLE_NAME)
            .WithColumnsFromTypeProperties<Account>()
            .WithKeyColumnsNames(nameof(Account.Email))
            .Build();

        public AccountMap(IConfigurations configurations)
            : base(configurations.SqlConnectionString, AccountTable, new ValueMapper<string>(nameof(Account.Email)), new TypeMapper<Account>(AccountTable))
        {
        }
    }
}