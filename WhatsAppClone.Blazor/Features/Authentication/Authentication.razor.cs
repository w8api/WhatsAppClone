using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WhatsAppClone.Blazor.Features.Shared;
using WhatsAppClone.Blazor.JsInterop;
using WhatsAppClone.Common.Features.WhatsApp;

namespace WhatsAppClone.Blazor.Features.Authentication
{
    public class AuthenticationBase : ComponentBaseExtended, IDisposable
    {

        #region Vars

        Guid _accountId = Guid.Empty;

        #endregion

        #region Injects

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        BrowserInterop BrowserInterop { get; set; }

        [Inject]
        HttpClient HttpClient { get; set; }

        #endregion

        #region Props

        public string QrCodeBase64 { get; set; }

        public static event Func<QrCodeDto, Task> OnQrCodeReceivedEvent;

        public static event Func<AuthenticatedDto, Task> OnAuthenticatedEvent;

        #endregion

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                var accountId = await BrowserInterop.LocalStorageGet<Guid?>("AccountId");

                if (accountId.GetValueOrDefault() != Guid.Empty)
                {
                    NavigateToChat();
                    return;
                }

                OnQrCodeReceivedEvent += AuthenticationBase_OnQrCodeReceivedEvent;
                OnAuthenticatedEvent += AuthenticationBase_OnAuthenticatedEvent;

                // initialize a new account if there isn't on localstorage above
                if (_accountId == Guid.Empty) _accountId = Guid.NewGuid();

                await HttpClient.PostAsync($"account/{_accountId}", null);
            }
        }

        void NavigateToChat()
            => NavigationManager.NavigateTo("/chat");

        private async Task AuthenticationBase_OnAuthenticatedEvent(AuthenticatedDto arg)
        {
            await BrowserInterop.LocalStorageSet("AccountId", arg.AccountId);
            NavigateToChat();
        }

        private async Task AuthenticationBase_OnQrCodeReceivedEvent(QrCodeDto arg)
        {
            QrCodeBase64 = arg.QrCode;
            await InvokeAsync(() => StateHasChanged());
        }

        public static async Task NotifyQrCodeReceived(QrCodeDto qrCodeDto)
            => await OnQrCodeReceivedEvent?.Invoke(qrCodeDto);

        public static async Task NotifyAuthenticated(AuthenticatedDto authenticatedDto)
            => await OnAuthenticatedEvent?.Invoke(authenticatedDto);

        public void Dispose()
        {
            OnQrCodeReceivedEvent -= AuthenticationBase_OnQrCodeReceivedEvent;
            OnAuthenticatedEvent -= AuthenticationBase_OnAuthenticatedEvent;
            GC.SuppressFinalize(this);
        }
    }
}
