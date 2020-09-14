using System;
using System.Text.Json.Serialization;

namespace InvestmentHub.Providers.Models.Rico
{
    public class Position
    {
        [JsonPropertyName("productType")]
        public string ProductTypeString { get; set; }

        [JsonIgnore]
        public InvestmentType ProductType
        {
            get
            {
                if (Enum.TryParse<InvestmentType>(ProductTypeString, true, out var result))
                {
                    return result;
                }

                return InvestmentType.UNKNOWN;
            }
        }

        public string ProductTypeName { get; set; }
        public float TotalValue { get; set; }
        public float Alocation { get; set; }
        public float InvestedAlocation { get; set; }
    }
}