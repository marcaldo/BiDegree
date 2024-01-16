using BiDegree.Models;
using BiDegree.Shared;
using Blazored.LocalStorage;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public class ApiClientService : IApiClientService
    {
        readonly HttpClient _httpClient;
        readonly ILocalStorageService _localStorage;

        public ApiClientService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public async Task<IEnumerable<string>> GetFileList()
        {
            var folderName = await _localStorage.GetItemAsync<string>(Constants.KeyName_DriveApiKey);

            var url = "api/weatherforecast";
            return await _httpClient.GetFromJsonAsync<IEnumerable<string>>(url);
        }
    }
}
