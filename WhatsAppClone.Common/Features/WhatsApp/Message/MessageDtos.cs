using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhatsAppClone.Common.Features.WhatsApp.Message;

namespace WhatsAppClone.Common.Features.WhatsApp.Message
{
    public record MessageReceivedDto(Guid AccountId, MessageReadModel Message);

    public record MessageAckDto(Guid AccountId, string MessageId, EMessageAck Ack);
}
