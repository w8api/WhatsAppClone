using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WhatsAppClone.Common.Features.WhatsApp.Common;

namespace WhatsAppClone.Common.Features.WhatsApp.Chat
{
    public record ChatReadModel
    {
        public Message.MessageReadModel LastMessage { get; set; }

        public string ContactPictureUrl { get; set; }

        public IdReadModel Id { get; set; }

        public string SerializedId { get; set; }

        public string FormattedTitle { get; set; }

        public bool IsGroup { get; set; }

        public bool IsReadOnly { get; set; }

        public int UnreadCount { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsArchive { get; set; }

        public bool Pinned { get; set; }

        public bool IsMuted { get; set; }

        public DateTime MuteExpiration { get; set; }
    }
}
