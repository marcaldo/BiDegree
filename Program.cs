using BiDegree.Services;
using BiDegree.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
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
            builder.Services.AddScoped<IGoogleDriveApi, GoogleDriveAPI>();
            builder.Services.AddScoped<IDebugMode, DebugMode>();
            builder.Services.AddSingleton<StateContainer>();
            builder.Services.AddScoped<ISettingValuesService, SettingValuesService>();
            builder.Services.AddMudServices();

            builder.Services.AddBlazoredLocalStorage(config =>
                config.JsonSerializerOptions.WriteIndented = true);

            builder.Services.AddScoped<IDisplayQueue, DisplayQueue>();


            await builder.Build().RunAsync();
        }
    }
}
