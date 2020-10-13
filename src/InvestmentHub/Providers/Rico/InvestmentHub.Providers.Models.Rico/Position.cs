using System;
using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.Rico
{
    public class Position
    {
        [JsonPropertyName("productType")]
        public string ProductTypeString { get; set; }

        [JsonIgnore]
        public ProductType ProductType
        {
            get
            {
                if (Enum.TryParse<ProductType>(ProductTypeString, true, out var result))
                {
                    return result;
                }

                return ProductType.UNKNOWN;
            }
        }

        public string ProductTypeName { get; set; }
        public float TotalValue { get; set; }
        public float Alocation { get; set; }
        public float InvestedAlocation { get; set; }
    }
}