using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SettlementService.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public Guid BookingId { get; set; }

        [Required]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan BookingTime { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
