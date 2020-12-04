using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WhatsAppClone.Blazor.Features.Shared;
using WhatsAppClone.Common.Features.WhatsApp.Chat;
using WhatsAppClone.Common.Features.WhatsApp.Message;

namespace WhatsAppClone.Blazor.Features.Chat
{
    public class ChatBase : ComponentBaseExtended, IDisposable
    {
        #region Vars

        protected ElementReference divMessages, textbox;
        Guid _accountId;

        #endregion

        #region Injections

        [Inject]
        HttpClient HttpClient { get; set; }

        [Inject]
        JsInterop.DomInterop DomInterop { get; set; }

        [Inject]
        JsInterop.BrowserInterop BrowserInterop { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        #endregion

        #region Props

        protected IEnumerable<ChatReadModel> Chats { get; private set; }

        protected ChatReadModel SelectedChat { get; set; }

        protected List<MessageReadModel> SelectedChatMessages { get; private set; }

        protected string Message { get; set; }

        protected bool IsSendingMessage { get; private set; }

        public static event Func<MessageReceivedDto, Task> OnMessageReceivedEvent;

        #endregion

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                #region Check the accountid

                var accountId = await BrowserInterop.LocalStorageGet<Guid?>("AccountId");

                if (accountId.GetValueOrDefault() == Guid.Empty)
                {
                    NavigationManager.NavigateTo("/");
                    return;
                }

                _accountId = accountId.Value;

                #endregion

                OnMessageReceivedEvent += ChatBase_OnMessageReceive;
                await BindChats();
                StateHasChanged();
            }
        }

        async Task BindChats()
        {
            Chats = await HttpClient.GetFromJsonAsync<IEnumerable<ChatReadModel>>($"whatsapp/chats/accounts/{_accountId}");
        }

        protected async Task SelectChat(ChatReadModel selectedChat)
        {
            SelectedChat = selectedChat;
            await LoadMessagesFromSelectedChat();
            await textbox.FocusAsync();
        }

        async Task LoadMessagesFromSelectedChat()
        {
            SelectedChatMessages = (await HttpClient.GetFromJsonAsync<IEnumerable<MessageReadModel>>($"whatsapp/messages/accounts/{_accountId}/{SelectedChat.SerializedId}")).ToList();
            StateHasChanged();
            await ScrollMessagesToBottom();
        }

        protected async Task OnMessageKeyDown(KeyboardEventArgs e)
        {
            if (!IsSendingMessage && e.Code == "Enter")
            {
                if (!string.IsNullOrWhiteSpace(Message))
                {
                    IsSendingMessage = true;

                    var response = await HttpClient.PostAsJsonAsync($"whatsapp/messages/accounts/{_accountId}/{SelectedChat.SerializedId}/send-text", new { Text = Message });

                    if (response.IsSuccessStatusCode)
                    {
                        Message = null;
                        await LoadMessagesFromSelectedChat();

                        IsSendingMessage = false;
                        StateHasChanged();

                        await textbox.FocusAsync();
                    }
                    else
                    {
                        await BrowserInterop.Alert($"error sending the message: {response.ReasonPhrase}");
                    }
                }
            }
        }

        #region For outsite events

        private async Task ChatBase_OnMessageReceive(MessageReceivedDto messageReceivedDto)
        {
            var message = messageReceivedDto.Message;

            if (message.From == SelectedChat?.SerializedId)
            {
                SelectedChatMessages.Add(message);
            }

            await BindChats();

            await InvokeAsync(() => StateHasChanged());
            await ScrollMessagesToBottom();
        }

        async Task ScrollMessagesToBottom()
            => await DomInterop.ScrollToBottom(divMessages);

        public static async Task NotifyMessageReceived(MessageReceivedDto messageReceivedDto)
            => await OnMessageReceivedEvent?.Invoke(messageReceivedDto);

        #endregion

        public void Dispose()
        {
            OnMessageReceivedEvent -= ChatBase_OnMessageReceive;
            GC.SuppressFinalize(this);
        }
    }
}
