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
        //[Inject] NavigationManager NavigationManager { get; set; }
        private bool _shuffled = true;
        private bool _debugMode = false;
        private double displayTime;

        protected override async Task OnInitializedAsync()
        {
#if DEBUG
            System.Threading.Thread.Sleep(10000);
#endif
            var displayInOrder = await LocalStorage.GetItemAsync<bool?>(Constants.KeyName_DisplayInOrder);
            _shuffled = displayInOrder == null || !(bool)displayInOrder;

            var debugMode = await LocalStorage.GetItemAsync<bool?>(Constants.KeyName_Dev_DebugMode);
            _debugMode = debugMode != null && (bool)debugMode;

            var storedDuration = await LocalStorage.GetItemAsync<double?>(Constants.KeyName_ShowTime);
            displayTime = storedDuration is null
                ? Constants.DefaultValue_ShowTime
                : (double)storedDuration;

            displayTime *= 1000;

            await ShowAsync();
        }

        private async Task ShowAsync()
        {
            var queue = await GetDisplayQueueAsync();

            var dotNetObjectReference = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("imageInterop.runQueue", queue, displayTime, dotNetObjectReference);
        }

        public async Task<List<DisplayItem>> GetDisplayQueueAsync()
        {
            if (_shuffled)
            {
                return await GetShuffledList();
            }

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

                _ = float.TryParse(driveFile.fileSize, out var fileSize);

                tempNumeredItemList.Add(itemNum, new DisplayItem
                {
                    ItemNumber = itemNum,
                    SourceUrl = link,
                    ItemType = displayItemType,
                    Title = driveFile.title,
                    Height = driveFile.imageMediaMetadata != null ? driveFile.imageMediaMetadata.height : 0,
                    Width = driveFile.imageMediaMetadata != null ? driveFile.imageMediaMetadata.width : 0,
                    Rotation = driveFile.imageMediaMetadata != null ? driveFile.imageMediaMetadata.rotation : 0,
                    FileSize = fileSize.ToString("#.##"),
                    IsVideo = driveFile.mimeType.ToLower().Contains("video")
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

                        _ = float.TryParse(driveFile.fileSize, out var fileSize);

                        try
                        {
                            tempNumeredItemList.Add(rndPosition, new DisplayItem
                            {
                                SourceUrl = link,
                                ItemType = displayItemType,
                                Title = driveFile.title,
                                Height = driveFile.imageMediaMetadata != null ? driveFile.imageMediaMetadata.height : 0,
                                Width = driveFile.imageMediaMetadata != null ? driveFile.imageMediaMetadata.width : 0,
                                Rotation = driveFile.imageMediaMetadata != null ? driveFile.imageMediaMetadata.rotation : 0,
                                FileSize = fileSize.ToString("#.##"),
                                IsVideo = driveFile.mimeType.ToLower().Contains("video")
                            });
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

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
        public async Task Reload()
        {
            await ShowAsync();
            //NavigationManager.NavigateTo("PhotoFramePage");
            StateHasChanged();
        }
    }
}
