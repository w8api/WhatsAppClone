using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsAppClone.Common.Features.WhatsApp
{
    public record QrCodeDto(Guid AccountId, string QrCode);

    public record AuthenticatedDto(Guid AccountId);

    public record StateChangedDto(Guid AccountId, object State);

}
