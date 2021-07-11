using BiDegree.Models;
using BiDegree.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public class DisplayQueue : IDisplayQueue
    {
        [Inject] ILocalStorageService LocalStorage { get; set; }
        [Inject] IGoogleDriveApi GoogleDriveApi { get; set; }

        public Task<(List<DisplayItem> displayItems1, List<DisplayItem> displayItems2)> GetDisplayQueuesAsync()
        {
            return GetShuffledLists();
        }

        private async Task<(List<DisplayItem> displayItems1, List<DisplayItem> displayItems2)> GetShuffledLists()
        {
            Dictionary<int, DisplayItem> tempNumeredItemList = new();

            DriveFileList driveFileList = await GetDriveFileList();

            if (!driveFileList.items.Any())
            {
                return (new List<DisplayItem>(), new List<DisplayItem>());
            }

            const int randomNumbesQuantityMultiplier = 2;
            int maxRandomNumber = driveFileList.items.Length * randomNumbesQuantityMultiplier;

            foreach (var driveFile in driveFileList.items)
            {
                if (!driveFile.webContentLink.Contains("&"))
                {
                    continue;
                }

                var link = driveFile.webContentLink.Substring(0, driveFile.webContentLink.IndexOf("&"));

                var itemAdded = false;

                while (!itemAdded)
                {
                    Random rnd = new();

                    // Arbitraty random numbers (more that items count) just to set an order.
                    int rndPosition = rnd.Next(1, maxRandomNumber);

                    if (!tempNumeredItemList.ContainsKey(rndPosition))
                    {
                        var displayItemType = driveFile.mimeType.ToLower().Contains("video")
                            ? DisplayItemType.Video
                            : DisplayItemType.Image;

                        tempNumeredItemList.Add(rndPosition, new DisplayItem
                        {
                            SourceUrl = link,
                            ItemType = displayItemType
                        });

                        itemAdded = true;
                    }
                }
            }

            var shuffledList = tempNumeredItemList
                .OrderBy(d => d.Key)
                .Select(d => d.Value);

            int middle = shuffledList.Count() / 2;

            var shuffledList1 = shuffledList.Take(middle).ToList();
            var shuffledList2 = shuffledList.Take(middle + 1).ToList();

            // Ensure both lists have same length adding the first item from the other.
            if (shuffledList1.Count < shuffledList2.Count)
            {
                _ = shuffledList1.Append(shuffledList2.First());
            }

            if (shuffledList2.Count < shuffledList1.Count)
            {
                _ = shuffledList2.Append(shuffledList1.First());
            }

            return (
                displayItems1: shuffledList1,
                displayItems2: shuffledList2
                );
        }

        private async Task<DriveFileList> GetDriveFileList()
        {
            string folderId = await LocalStorage.GetItemAsync<string>(Constants.KeyName_DriveFolderId);
            return await GoogleDriveApi.GetDriveFileList(folderId);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
