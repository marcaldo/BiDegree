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

namespace BiDegree.Features.PhotoFrame
{
    public partial class PictureDisplay
    {
        [Inject] IGoogleApi GoogleApi { get; set; }
        [Inject] ILocalStorageService LocalStorage { get; set; }
        [Inject] NavigationManager Navigation { get; set; }


        private string ItemLink { get; set; }
        private string folderId;
        private bool isDebugMode = false;
        private double duration = Constants.DefaultShowTime;

        static Dictionary<int, string> displayQueue;
        private DriveFileList driveFileList;

        protected override async Task OnInitializedAsync()
        {
            Navigation.TryGetQueryString("debug", out isDebugMode);

            folderId = await LocalStorage.GetItemAsync<string>(Constants.DriveFolderId);

            driveFileList = await GoogleApi.GetDriveFileList(folderId);
            SetDisplayList();

            var storedDuration = await LocalStorage.GetItemAsync<double?>(Constants.ShowTime);
            duration = storedDuration is null
                ? Constants.DefaultShowTime
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

            //ItemLink = "https://drive.google.com/uc?id=11Tg3qnDAY6DLfMs_tHek2U5MItMsXnJi"; // Fergui
            ItemLink = item.Value;
            displayQueue.Remove(item.Key);

            StateHasChanged();
        }

        private void SetDisplayList()
        {
            var totalItems = driveFileList.items.Count();

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

        private Dictionary<int, string> CreateTempQueue(DriveFileList driveFileList)
        {
            Dictionary<int, string> tempQueue = new();

            if (!driveFileList.items.Any()) { return tempQueue; }

            int maxRandomNumber = driveFileList.items.Count() * 2;

            foreach (var driveFile in driveFileList.items)
            {
                if (!driveFile.webContentLink.Contains("&"))
                {
                    continue;
                }

                var ampPos = driveFile.webContentLink.IndexOf("&");
                var link = driveFile.webContentLink.Substring(0, ampPos);

                Random rnd = new Random();
                var itemAdded = false;

                while (!itemAdded)
                {
                    // Arbitraty random numbers (more that items count) just to set an order.
                    int rndPosition = rnd.Next(1, maxRandomNumber);

                    if (!tempQueue.ContainsKey(rndPosition))
                    {
                        tempQueue.Add(rndPosition, link);
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
