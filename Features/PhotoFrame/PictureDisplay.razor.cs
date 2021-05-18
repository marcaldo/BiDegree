using BiDegree.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiDegree.Services;
using Blazored.LocalStorage;
using BiDegree.Shared;

namespace BiDegree.Features.PhotoFrame
{
    public partial class PictureDisplay
    {
        [Inject] IGoogleApi GoogleApi { get; set; }
        [Inject] ILocalStorageService LocalStorage { get; set; }

        private string ItemLink { get; set; }

        static Dictionary<int, string> displayQueue;

        private DriveFileList driveFileList;

        protected override async Task OnInitializedAsync()
        {
            var folderId = await LocalStorage.GetItemAsync<string>(Constants.DriveFolderId);

            driveFileList = await GoogleApi.GetDriveFileList(folderId);
            SetDisplayList(driveFileList);
        }

        private void SetDisplayList(DriveFileList driveFileList)
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

                ItemLink = displayQueue.First().Value;
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
                int rndPosition = 0;
                var itemAdded = false;

                while (!itemAdded)
                {
                    // Arbitraty random numbers (more that items count) just to set an order.
                    rndPosition = rnd.Next(maxRandomNumber);

                    if (!tempQueue.ContainsKey(rndPosition))
                    {
                        tempQueue.Add(rndPosition, link);
                        itemAdded = true;
                    }
                }
            }

            return tempQueue;
        }
    }
}
