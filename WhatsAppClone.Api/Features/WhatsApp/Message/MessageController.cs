using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using WhatsAppClone.Common.Features.WhatsApp.Message;

namespace WhatsAppClone.Api.Features.WhatsApp.Message
{
    [ApiController]
    [Route("/api/whatsapp/messages")]
    public class MessageController : ControllerBase
    {
        readonly IHubContext<WebhookHub> _webhookHub;

        public MessageController(IHubContext<WebhookHub> webhookHub)
        {
            _webhookHub = webhookHub;
        }

        /// <summary>
        /// The message ACK webhook
        /// </summary>
        /// <returns></returns>
        [HttpPost("ack")]
        public async Task MessageAckWebhook(MessageAckDto messageAck)
            => await _webhookHub.Clients.All.SendAsync("MessageAck", messageAck);

        /// <summary>
        /// The message receivedK webhook
        /// </summary>
        /// <returns></returns>
        [HttpPost("")]
        public async Task MessageReceived(MessageReceivedDto messageReceived)
            => await _webhookHub.Clients.All.SendAsync("MessageReceived", messageReceived);
    }
}
