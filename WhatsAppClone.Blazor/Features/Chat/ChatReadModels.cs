using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WhatsAppClone.Blazor.Features.Common;

namespace WhatsAppClone.Blazor.Features.Chat
{
    public record ChatReadModel
    {
        [JsonPropertyName("lastMessage")]
        public Message.MessageReadModel LastMessage { get; set; }

        [JsonPropertyName("contactPictureUrl")]
        public string ContactPictureUrl { get; set; }

        [JsonPropertyName("id")]
        public IdReadModel Id { get; set; }

        [JsonPropertyName("serializedId")]
        public string SerializedId { get; set; }

        [JsonPropertyName("formattedTitle")]
        public string FormattedTitle { get; set; }

        [JsonPropertyName("isGroup")]
        public bool IsGroup { get; set; }

        [JsonPropertyName("isReadOnly")]
        public bool IsReadOnly { get; set; }

        [JsonPropertyName("unreadCount")]
        public int UnreadCount { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("isArchive")]
        public bool IsArchive { get; set; }

        [JsonPropertyName("pinned")]
        public bool Pinned { get; set; }

        [JsonPropertyName("isMuted")]
        public bool IsMuted { get; set; }

        [JsonPropertyName("muteExpiration")]
        public DateTime MuteExpiration { get; set; }
    }


}
