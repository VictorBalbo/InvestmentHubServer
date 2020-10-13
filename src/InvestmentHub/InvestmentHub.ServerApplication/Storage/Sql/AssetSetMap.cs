using InvestmentHub.Models;
using System;
using System.Data;
using Take.Elephant.Sql;
using Take.Elephant.Sql.Mapping;

namespace InvestmentHub.ServerApplication.Storage.Sql
{
    public class AssetSetMap : SqlSetMap<string, Asset>, IAssetSetMap
    {
        public const string TABLE_NAME = "Assets";

        public static ITable AssetsTable = TableBuilder
            .WithName(TABLE_NAME)
            .WithColumnsFromTypeProperties<Asset>()
            .WithColumnFromType<string>(nameof(Account.Email))
            .WithKeyColumnsNames(nameof(Account.Email))
            .WithKeyColumnsNames(nameof(Asset.ProviderName))
            .WithKeyColumnsNames(nameof(Asset.AssetName))
            .WithKeyColumnsNames(nameof(Asset.StorageDate))
            .Build();

        public AssetSetMap(IConfigurations configurations)
            : base(configurations.SqlConnectionString, AssetsTable, new ValueMapper<string>(nameof(Account.Email)), new TypeMapper<Asset>(AssetsTable))
        {
        }
    }
}