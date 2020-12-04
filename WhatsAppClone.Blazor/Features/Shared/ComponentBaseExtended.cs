using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace WhatsAppClone.Blazor.Features.Shared
{

    public class ComponentBaseExtended : ComponentBase
    {
        static bool _isAppInitialized = false;

        #region Injects

        [Inject]
        WebhookConnection WebhookConnection { get; set; }

        #endregion


        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (!_isAppInitialized)
            {
                _isAppInitialized = true;
                await WebhookConnection.Start();
            }
        }
    }
}
