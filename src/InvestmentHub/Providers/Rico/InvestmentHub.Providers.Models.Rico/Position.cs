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
        public double TotalValue { get; set; }
        public double NetValue { get; set; }
        public double Alocation { get; set; }
        public double InvestedAlocation { get; set; }
    }
}
