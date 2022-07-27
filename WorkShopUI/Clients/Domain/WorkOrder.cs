using System.Text.Json.Serialization;

namespace WorkShopUI.Clients.Domain
{
    public class WorkOrder : ResourceObject
    {
        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("orderDate")]
        public string OrderDate { get; set; }

        [JsonPropertyName("plateNumber")]
        public string PlateNumber { get; set; }

        [JsonPropertyName("odometerMeasurement")]
        public string OdometerMeasurement { get; set; }

        [JsonPropertyName("odometerValue")]
        public double OdometerValue { get; set; }

        [JsonPropertyName("gasAmount")]
        public double GasAmount { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("carLine")]
        public CarLine CarLine { get; set; }

        [JsonPropertyName("contact")]
        public Contact Contact { get; set; }
    }
}