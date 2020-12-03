using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace WhatsAppClone.Blazor.JsInterop
{
    public class DomInterop : BaseInterop
    {
        const string jsName = "domFunctions";

        public DomInterop(IJSRuntime jsRuntime)
            : base(jsRuntime)
        { }

        /// <summary>
        /// Element.Focus
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public async ValueTask Focus(object element)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.focus", element);

        /// <summary>
        /// Element blur
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public async ValueTask Blur(object element)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.blur", element);

        /// <summary>
        /// Select element method
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public async ValueTask Select(object element)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.select", element);

        /// <summary>
        /// Click element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public async ValueTask Click(object element)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.click", element);

        /// <summary>
        /// Scroll to the bottom of the div
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public async ValueTask ScrollToBottom(object element)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.scrollToBottom", element);

        /// <summary>
        /// Returns the scroll
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public async ValueTask<double> ScrollWidth(object element)
            => await JsRuntime.InvokeAsync<double>($"{jsName}.scrollWidth", element);

        /// <summary>
        /// Scroll to value passed
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async ValueTask ScrollLeft(object element, double value)
            => await JsRuntime.InvokeVoidAsync($"{jsName}.scrollLeft", element, value);
    }
}
