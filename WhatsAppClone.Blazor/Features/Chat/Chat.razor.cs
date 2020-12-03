using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WhatsAppClone.Blazor.Features.Chat
{
    public class ChatBase : ComponentBase
    {
        #region Vars

        protected ElementReference divMessages, textbox;

        #endregion

        #region Injections

        [Inject]
        HttpClient HttpClient { get; set; }

        [Inject]
        JsInterop.DomInterop DomInterop { get; set; }

        [Inject]
        JsInterop.BrowserInterop BrowserInterop { get; set; }

        #endregion

        #region Props

        protected IEnumerable<ChatReadModel> Chats { get; private set; }

        protected ChatReadModel SelectedChat { get; set; }

        protected IEnumerable<Message.MessageReadModel> SelectedChatMessages { get; private set; }

        protected string Message { get; set; }

        protected bool IsSendingMessage { get; private set; }

        #endregion

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                #region Get Chats

                Chats = await HttpClient.GetFromJsonAsync<IEnumerable<ChatReadModel>>($"/api/whatsapp/chats/accounts/{Constants.ACCOUNT_ID}?shouldIncludeProfilePicture=true&shouldIncludeLastMessage=true");
                StateHasChanged();

                #endregion
            }
        }

        protected async Task SelectChat(ChatReadModel selectedChat)
        {
            SelectedChat = selectedChat;
            await LoadMessagesFromSelectedChat();
            await textbox.FocusAsync();
        }

        async Task LoadMessagesFromSelectedChat()
        {
            SelectedChatMessages = await HttpClient.GetFromJsonAsync<IEnumerable<Message.MessageReadModel>>($"/api/whatsapp/messages/accounts/{Constants.ACCOUNT_ID}/{SelectedChat.SerializedId}");
            StateHasChanged();
            await DomInterop.ScrollToBottom(divMessages);
        }

        protected async Task OnMessageKeyDown(KeyboardEventArgs e)
        {
            if (!IsSendingMessage && e.Code == "Enter")
            {
                if (!string.IsNullOrWhiteSpace(Message))
                {
                    IsSendingMessage = true;

                    var response = await HttpClient.PostAsJsonAsync($"/api/whatsapp/messages/accounts/{Constants.ACCOUNT_ID}/{SelectedChat.SerializedId}/send-text", new { Text = Message });

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
    }
}
