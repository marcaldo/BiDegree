using BiDegree.Models;
using BiDegree.Shared;
using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
            return await GetFileList();

            var gApiKey = await _localStorage.GetItemAsync<string>(Constants.KeyName_DriveApiKey);

            // see https://developers.google.com/drive/api/v2/reference/files/list

            var url = $"https://www.googleapis.com/drive/v2/files?q='{folderId}'+in+parents&maxResults=1000&key={gApiKey}";
            var driveFileList = await _httpClient.GetFromJsonAsync<DriveFileList>(url);

            return driveFileList;
        }

        public async Task<DriveFileList> GetFileList()
        {
            var folderName = await _localStorage.GetItemAsync<string>(Constants.KeyName_DriveFolderId);

            var files = await _httpClient.GetFromJsonAsync<PictureFile[]>("/localphotos/photofiles.json");

            HashSet<Item> randomFiles = new HashSet<Item>();

            int maxRandomNumber = files.Length;

            for (int i = 0; i < maxRandomNumber; i++)
            {
                Random rnd = new();
                int rndPosition = rnd.Next(1, maxRandomNumber);
                var pictureFile = files[rndPosition];
                bool isVideo = pictureFile.name.ToLower().EndsWith(".mp4");

                randomFiles.Add(new Item
                {
                    downloadUrl = $"/localphotos/{folderName}/{pictureFile.name}",
                    mimeType = isVideo ? "video" : "image"
                });
            }


            DriveFileList driveFileList = new()
            {
                items = randomFiles.ToArray()
            };

            return driveFileList;
        }

        public class PictureFile
        {
            public  string name { get; set; }
        }
    }
}
