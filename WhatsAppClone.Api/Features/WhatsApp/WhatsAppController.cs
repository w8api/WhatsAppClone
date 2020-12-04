using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WhatsAppClone.Common.Features.WhatsApp;

namespace WhatsAppClone.Api.Features.WhatsApp
{
    [ApiController]
    [Route("/api/whatsapp")]
    public class WhatsAppController : ControllerBase
    {
        readonly IHubContext<WebhookHub> _webhookHub;

        public WhatsAppController(IHubContext<WebhookHub> webhookHub)
        {
            _webhookHub = webhookHub;
        }

        /// <summary>
        /// The Qr-Code webhook
        /// </summary>
        /// <returns></returns>
        [HttpPost("qr-code")]
        public async Task QrCodeWebhook(QrCodeDto qrCode)
            => await _webhookHub.Clients.All.SendAsync("QrCode", qrCode);

        /// <summary>
        /// The Authenticated webhook
        /// </summary>
        /// <returns></returns>
        [HttpPost("authenticated")]
        public async Task AuthenticatedWebhook(AuthenticatedDto authenticatedDto)
            => await _webhookHub.Clients.All.SendAsync("Authenticated", authenticatedDto);

        /// <summary>
        /// The state changed webhook
        /// </summary>
        /// <returns></returns>
        [HttpPost("state-changed")]
        public async Task StateChangedWebhook(StateChangedDto stateChangedDto)
            => await _webhookHub.Clients.All.SendAsync("StateChanged", stateChangedDto);
    }
}
