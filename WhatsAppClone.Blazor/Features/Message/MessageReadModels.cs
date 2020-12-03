using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WhatsAppClone.Blazor.Features.Common;

namespace WhatsAppClone.Blazor.Features.Message
{
    public record MessageReadModel
    {
        [JsonPropertyName("isStatus")]
        public bool IsStatus { get; set; }

        [JsonPropertyName("ack")]
        public int Ack { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("broadcast")]
        public bool Broadcast { get; set; }

        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("fromMe")]
        public bool FromMe { get; set; }

        [JsonPropertyName("hasMedia")]
        public bool HasMedia { get; set; }

        [JsonPropertyName("hasQuotedMsg")]
        public bool HasQuotedMsg { get; set; }

        [JsonPropertyName("id")]
        public IdMessageReadModel Id { get; set; }

        [JsonPropertyName("isForwarded")]
        public bool IsForwarded { get; set; }

        [JsonPropertyName("location")]
        public LocationReadModel Location { get; set; }

        [JsonPropertyName("mediaKey")]
        public string MediaKey { get; set; }

        [JsonPropertyName("mentionedIds")]
        public List<string> MentionedIds { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("type")]
        public EMessageType Type { get; set; }
    }

    public record IdMessageReadModel
    {
        [JsonPropertyName("serialized")]
        public string Serialized { get; set; }

        [JsonPropertyName("fromMe")]
        public bool FromMe { get; set; }
    }

    public enum EMessageType
    {
        TEXT,
        AUDIO,
        VOICE,
        IMAGE,
        VIDEO,
        DOCUMENT,
        STICKER,
        LOCATION,
        CONTACT_CARD,
        CONTACT_CARD_MULTI,
        REVOKED,
        UNKNOWN
    }
}
