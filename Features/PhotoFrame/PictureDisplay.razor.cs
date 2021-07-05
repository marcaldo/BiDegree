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
using System.Threading;

namespace BiDegree.Features.PhotoFrame
{
    public partial class PictureDisplay : ComponentBase
    {
        [Inject] IGoogleApi GoogleApi { get; set; }
        [Inject] ILocalStorageService LocalStorage { get; set; }
        [Inject] IJSRuntime JS { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] IDebugMode DebugMode { get; set; }

        readonly System.Timers.Timer timer = new();

        private string itemLink;
        private string itemLink2;
        private bool isVideo = false;
        private double duration = Constants.DefaultValue_ShowTime;
        private readonly string startTime = @DateTime.Now.ToLongTimeString();

        private Dictionary<int, DisplayItem> displayQueue;

        protected override async Task OnInitializedAsync()
        {
#if DEBUG
            //Thread.Sleep(10000);
#endif

            var debugModeSetored = await LocalStorage.GetItemAsync<bool?>(Constants.KeyName_Dev_DebugMode);
            DebugMode.IsActive = debugModeSetored != null && (bool)debugModeSetored;

            await SetDisplayList();

            var storedDuration = await LocalStorage.GetItemAsync<double?>(Constants.KeyName_ShowTime);
            duration = storedDuration is null
                ? Constants.DefaultValue_ShowTime
                : (double)storedDuration;

            timer.Interval = 1000 * duration;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;

            await ShowNext();
        }


        public async Task ShowNext()
        {
            var item = displayQueue.FirstOrDefault();

            if (item.Key == 0)  // reload
            {
                //await SetDisplayList();
                //item = displayQueue.FirstOrDefault();
                Navigation.NavigateTo("photos", true);
            }

            itemLink = item.Value.SourceUrl;
            isVideo = item.Value.IsVideo;

            if (isVideo)
            {
                await JS.InvokeVoidAsync("playVideo");
            }

            displayQueue.Remove(item.Key);

            var item2 = displayQueue.FirstOrDefault();
            itemLink2 = item2.Value.SourceUrl;



            if (DebugMode.IsActive)
            {
                ++DebugMode.PictureCount;
                await LocalStorage.SetItemAsync(Constants.KeyName_Dev_PictureCount, DebugMode.PictureCount);
            }

            timer.Start();

            StateHasChanged();
        }

        private async Task SetDisplayList()
        {
            string folderId = await LocalStorage.GetItemAsync<string>(Constants.KeyName_DriveFolderId);

            DriveFileList driveFileList = await GoogleApi.GetDriveFileList(folderId);

            var tempQueue = CreateTempRandomQueue(driveFileList);

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

        private static Dictionary<int, DisplayItem> CreateTempRandomQueue(DriveFileList driveFileList)
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

                var itemAdded = false;

                while (!itemAdded)
                {
                    Random rnd = new();

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
            timer.Stop();

            ShowNext().Wait();
        }

    }
}
