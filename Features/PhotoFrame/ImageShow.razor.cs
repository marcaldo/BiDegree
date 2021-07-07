using BiDegree.Models;
using BiDegree.Services;
using BiDegree.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiDegree.Features.PhotoFrame
{
    public partial class ImageShow
    {
        [Inject] IJSRuntime JS { get; set; }
        [Inject] ILocalStorageService LocalStorage { get; set; }
        [Inject] IGoogleDriveApi GoogleDriveApi { get; set; }
        private int _itemNumber = -1;
        protected override async Task OnInitializedAsync()
        {
#if DEBUG
            // System.Threading.Thread.Sleep(10000);
#endif

            var displayItems = await GetDisplayQueueAsync();

            await ShowAsync(displayItems);
        }

        private async Task ShowAsync(List<DisplayItem> queue)
        {
            await JS.InvokeVoidAsync("runQueue", queue, 10000);
        }

        public async Task<List<DisplayItem>> GetDisplayQueueAsync()
        {
            //return await GetShuffledList();

            return await GetNaturalOrderList();
        }

        private async Task<List<DisplayItem>> GetNaturalOrderList()
        {
            Dictionary<int, DisplayItem> tempNumeredItemList = new();

            DriveFileList driveFileList = await GetDriveFileList();

            if (!driveFileList.items.Any())
            {
                return (new List<DisplayItem>());
            }

            int itemNum = 0;

            foreach (var driveFile in driveFileList.items.OrderBy(i => i.title))
            {
                if (!driveFile.webContentLink.Contains("&"))
                {
                    continue;
                }

                var link = driveFile.webContentLink.Substring(0, driveFile.webContentLink.IndexOf("&"));

                var displayItemType = driveFile.mimeType.ToLower().Contains("video")
                          ? DisplayItemType.Video
                          : DisplayItemType.Image;

                tempNumeredItemList.Add(itemNum, new DisplayItem
                {
                    ItemNumber = itemNum,
                    SourceUrl = link,
                    ItemType = displayItemType,
                    Title = driveFile.title
                });

                itemNum++;
            }

            var items = tempNumeredItemList
                .Select(d => d.Value);

            return items.ToList();

        }
        private async Task<List<DisplayItem>> GetShuffledList()
        {
            Dictionary<int, DisplayItem> tempNumeredItemList = new();

            DriveFileList driveFileList = await GetDriveFileList();

            if (!driveFileList.items.Any())
            {
                return (new List<DisplayItem>());
            }

            const int randomNumbesQuantityMultiplier = 2;
            int maxRandomNumber = driveFileList.items.Length * randomNumbesQuantityMultiplier;

            int itemNum = 0;

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

            foreach (var item in shuffledList)
            {
                item.ItemNumber = itemNum++;
            }

            return shuffledList.ToList();

        }

        private async Task<DriveFileList> GetDriveFileList()
        {
            string folderId = await LocalStorage.GetItemAsync<string>(Constants.KeyName_DriveFolderId);
            return await GoogleDriveApi.GetDriveFileList(folderId);
        }

        [JSInvokable]
        private void DisplayItem(DisplayItem displayItem)
        {
            _itemNumber = displayItem.ItemNumber;
            StateHasChanged();
        }
    }
}
