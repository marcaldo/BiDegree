using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BiDegree.Models;
using BiDegree.Shared;
using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BiDegree.Services
{
    public class GoogleDriveAPI : IGoogleDriveApi
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public GoogleDriveAPI(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<DriveFileList> GetDriveFileList(string folderId)
        {
            var gApiKey = await _localStorage.GetItemAsync<string>(Constants.KeyName_DriveApiKey);

            var url = $"https://www.googleapis.com/drive/v2/files?q='{folderId}'+in+parents&maxResults=1000&key={gApiKey}";
            return await _httpClient.GetFromJsonAsync<DriveFileList>(url);
        }

    }
}
