using BiDegree.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiDegree.Services;
using Blazored.LocalStorage;
using BiDegree.Shared;
using System.Timers;
using Microsoft.JSInterop;

namespace BiDegree.Features.PhotoFrame
{
    public partial class PictureDisplay
    {
        [Inject] IGoogleApi GoogleApi { get; set; }
        [Inject] ILocalStorageService LocalStorage { get; set; }
        [Inject] IJSRuntime JS { get; set; }


        private string itemLink;
        private bool isVideo = false;
        private string folderId;
        private bool isDebugMode = false;
        private double duration = Constants.DefaultValue_ShowTime;
        private int picsShown = 0;

        static Dictionary<int, DisplayItem> displayQueue;
        private DriveFileList driveFileList;

        protected override async Task OnInitializedAsync()
        {
            isDebugMode = await LocalStorage.GetItemAsync<bool>(Constants.KeyName_DevMode);
            await LocalStorage.RemoveItemAsync(Constants.KeyName_PicturesShown);

            folderId = await LocalStorage.GetItemAsync<string>(Constants.KeyName_DriveFolderId);

            driveFileList = await GoogleApi.GetDriveFileList(folderId);
            SetDisplayList();

            var storedDuration = await LocalStorage.GetItemAsync<double?>(Constants.KeyName_ShowTime);
            duration = storedDuration is null
                ? Constants.DefaultValue_ShowTime
                : (double)storedDuration;

            Timer timer = new();
            timer.Interval = 1000 * duration;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;


            await ShowNext();
        }


        public async Task ShowNext()
        {
            var item = displayQueue.FirstOrDefault();

            if (item.Key == 0)
            {
                driveFileList = await GoogleApi.GetDriveFileList(folderId);
                SetDisplayList();
                item = displayQueue.FirstOrDefault();
            }

            itemLink = item.Value.SourceUrl;
            isVideo = item.Value.IsVideo;

            if (isVideo)
            {
                await JS.InvokeVoidAsync("playVideo");
            }

            displayQueue.Remove(item.Key);

            ++picsShown;
            if (isDebugMode)
            {
                await LocalStorage.SetItemAsync(Constants.KeyName_PicturesShown, picsShown);
            }

            StateHasChanged();
        }

        private void SetDisplayList()
        {
            var totalItems = driveFileList.items.Length;

            var tempQueue = CreateTempQueue(driveFileList);

            if (tempQueue.Count > 0)
            {
                displayQueue = new();

                foreach (var tempItem in tempQueue.OrderBy(i => i.Key))
                {
                    displayQueue.Add(tempItem.Key, tempItem.Value);
                }

                tempQueue.Clear();
            }
        }

        private Dictionary<int, DisplayItem> CreateTempQueue(DriveFileList driveFileList)
        {
            Dictionary<int, DisplayItem> tempQueue = new();

            if (!driveFileList.items.Any()) { return tempQueue; }

            const int randomNumbesQuantityMultiplier = 2;
            int maxRandomNumber = driveFileList.items.Length * randomNumbesQuantityMultiplier;

            foreach (var driveFile in driveFileList.items)
            {
                if (!driveFile.webContentLink.Contains("&"))
                {
                    continue;
                }

                var ampPos = driveFile.webContentLink.IndexOf("&");
                var link = driveFile.webContentLink.Substring(0, ampPos);

                Random rnd = new();
                var itemAdded = false;

                while (!itemAdded)
                {
                    // Arbitraty random numbers (more that items count) just to set an order.
                    int rndPosition = rnd.Next(1, maxRandomNumber);

                    if (!tempQueue.ContainsKey(rndPosition))
                    {
                        tempQueue.Add(rndPosition, new DisplayItem
                        {
                            SourceUrl = link,
                            IsVideo = driveFile.mimeType.ToLower().Contains("video")
                        });

                        itemAdded = true;
                    }
                }
            }

            return tempQueue;
        }

        private void Timer_Elapsed(object _, ElapsedEventArgs e)
        {
            ShowNext().Wait();
        }
    }
}
