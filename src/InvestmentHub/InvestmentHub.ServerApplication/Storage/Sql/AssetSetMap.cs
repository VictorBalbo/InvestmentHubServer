using InvestmentHub.Models;
using Take.Elephant.Sql;
using Take.Elephant.Sql.Mapping;

namespace InvestmentHub.ServerApplication.Storage.Sql
{
    internal class AssetSetMap : SqlSetMap<string, Asset>, IAssetSetMap
    {
        private const string TableName = "Assets";

        private static readonly ITable AssetsTable = TableBuilder
            .WithName(TableName)
            .WithColumnsFromTypeProperties<Asset>()
            .WithKeyColumnsNames(nameof(Asset.Identity))
            .WithKeyColumnsNames(nameof(Asset.ProviderName))
            .WithKeyColumnsNames(nameof(Asset.AssetName))
            .WithKeyColumnsNames(nameof(Asset.StorageDate))
            .Build();

        public AssetSetMap(IConfigurations configurations)
            : base(configurations.SqlConnectionString, AssetsTable, new ValueMapper<string>(nameof(Asset.Identity)), new TypeMapper<Asset>(AssetsTable))
        {
        }
    }
}
