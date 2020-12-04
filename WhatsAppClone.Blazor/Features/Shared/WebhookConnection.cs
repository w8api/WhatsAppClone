using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using WhatsAppClone.Common.Features.WhatsApp;
using WhatsAppClone.Common.Features.WhatsApp.Message;

namespace WhatsAppClone.Blazor.Features.Shared
{
    public class WebhookConnection : IAsyncDisposable
    {
        HubConnection _hubConnection;

        public async Task Start()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44338/webhookhub")
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<MessageReceivedDto>("MessageReceived", async message => await Chat.ChatBase.NotifyMessageReceived(message));
            _hubConnection.On<QrCodeDto>("QrCode", async qrCode => await Authentication.AuthenticationBase.NotifyQrCodeReceived(qrCode));
            _hubConnection.On<AuthenticatedDto>("Authenticated", async authenticated => await Authentication.AuthenticationBase.NotifyAuthenticated(authenticated));

            await _hubConnection.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
