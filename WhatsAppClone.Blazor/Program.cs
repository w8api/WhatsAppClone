using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace WhatsAppClone.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyTypes = assembly.GetTypes();

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            #region Dependency injection

            // httpclient
            var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:44317/api/") };
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", "IlAvXQ0Ryz0YqTjKq2twQ28AWPDKoBp1WrQJKDE3OhT26ylwcUUVS6TQm9YIMqnxJ8C9DZ9nx0itjGEinRrTrUjaY9gKo6qh4fs7");
            builder.Services.AddScoped(sp => httpClient);

            // browserInterop
            // add all base interop
            foreach (var stateType in assemblyTypes
                .Where(w => w.IsSubclassOf(typeof(JsInterop.BaseInterop))))
            {
                builder.Services.AddScoped(stateType);
            }

            // webhook signalr conn
            builder.Services.AddScoped<Features.Shared.WebhookConnection>();

            #endregion

            await builder.Build().RunAsync();
        }
    }
}
