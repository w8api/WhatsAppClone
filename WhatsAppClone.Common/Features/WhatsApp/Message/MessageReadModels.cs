using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhatsAppClone.Common.Features.WhatsApp.Common;

namespace WhatsAppClone.Common.Features.WhatsApp.Message
{
    public record MessageReadModel
    {
        public bool IsStatus { get; set; }

        public int Ack { get; set; }

        public string Author { get; set; }

        public string Body { get; set; }

        public bool Broadcast { get; set; }

        public string From { get; set; }

        public bool FromMe { get; set; }

        public bool HasMedia { get; set; }

        public bool HasQuotedMsg { get; set; }

        public IdMessageReadModel Id { get; set; }

        public bool IsForwarded { get; set; }

        public LocationReadModel Location { get; set; }

        public string MediaKey { get; set; }

        public List<string> MentionedIds { get; set; }

        public DateTime Timestamp { get; set; }

        public string To { get; set; }

        public EMessageType Type { get; set; }
    }

    public record IdMessageReadModel
    {
        public string Serialized { get; set; }

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

    public enum EMessageAck
    {
        ACK_ERROR = -1,
        ACK_PENDING = 0,
        ACK_SERVER = 1,
        ACK_DEVICE = 2,
        ACK_READ = 3,
        ACK_PLAYED = 4
    }
}
