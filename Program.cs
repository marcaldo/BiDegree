using BiDegree.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BiDegree
{
    public class Program
    {
        public IConfiguration Configuration { get; }

        public static async Task Main(string[] args)
        {

        var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IWeatherApi, OpenWeatherApi>();
            builder.Services.AddScoped<IGoogleApi, GoogleAPI>();
            builder.Services.AddSingleton<StateContainer>();

            builder.Services.AddBlazoredLocalStorage(config =>
                config.JsonSerializerOptions.WriteIndented = true);


            await builder.Build().RunAsync();
        }
    }
}
