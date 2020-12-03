using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WhatsAppClone.Blazor.Features.Common
{
    public record LocationReadModel
    {
        [JsonPropertyName("latitude")]
        public int Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public int Longitude { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public record IdReadModel
    {
        [JsonPropertyName("serialized")]
        public string Serialized { get; set; }
    }
}