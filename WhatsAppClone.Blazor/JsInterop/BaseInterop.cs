using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsAppClone.Blazor.JsInterop
{
    public abstract class BaseInterop
    {
        public BaseInterop(IJSRuntime jsRuntime)
        {
            JsRuntime = jsRuntime;
        }

        protected IJSRuntime JsRuntime { get; }
    }
}
