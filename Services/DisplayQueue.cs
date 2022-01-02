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
        private readonly IGoogleDriveApi _googleDriveApi;
        private readonly ILocalStorageService _localStorage;
        private readonly StateContainer _stateContainer;
        private List<DisplayItem> _queue;
        private static int _imageCountToShowWeather;

        public DisplayQueue(IGoogleDriveApi googleDriveApi, ILocalStorageService localStorageService, StateContainer stateContainer)
        {
            _googleDriveApi = googleDriveApi;
            _localStorage = localStorageService;
            _stateContainer = stateContainer;
            _queue = new List<DisplayItem>();
        }

        public async Task<bool> IsDebugModeAsync()
        {
            var debugMode = await _localStorage.GetItemAsync<bool?>(Constants.KeyName_Dev_DebugMode);
            return debugMode != null && (bool)debugMode;
        }

        private async Task<bool> IsShuffled()
        {
            var displayInOrder = await _localStorage.GetItemAsync<bool?>(Constants.KeyName_DisplayInOrder);
            return displayInOrder == null || !(bool)displayInOrder;
        }

        private async Task<(int imageCount, int duration)> GetWeatherExtendedValuesAsync()
        {
            var showFullWeatherInPicturesValue = await _localStorage.GetItemAsStringAsync(Constants.KeyName_WeatherExtended);
            if (showFullWeatherInPicturesValue.Contains("."))
            {
                var storedValues = showFullWeatherInPicturesValue.Split('.');

                _ = int.TryParse(storedValues[0], out int imageCountToShowWeather);
                _ = int.TryParse(storedValues[1], out int duration);

                if(imageCountToShowWeather > 0 && duration > 0)
                {
                    return (imageCountToShowWeather, duration);
                }
            }

            return (0, 0);

        }
        public async Task<double> GetDisplayTimeAsync()
        {
            var storedDuration = await _localStorage.GetItemAsync<double?>(Constants.KeyName_ShowTime);
            var displayTime = storedDuration is null
                ? Constants.DefaultValue_ShowTime
                : (double)storedDuration;

            displayTime *= 1000;

            return displayTime;
        }

        public async Task<DisplayItem> GetNextItemAsync()
        {
            (int imageCount, int duration)? weatherExtendedValues = default;

            if (_queue.Count == 0)
            {
                _queue = await GetDisplayQueueAsync();

                weatherExtendedValues = await GetWeatherExtendedValuesAsync();
                if (weatherExtendedValues.HasValue)
                {
                    _imageCountToShowWeather = weatherExtendedValues.Value.imageCount;
                }
            }

            var displayItem = _queue.FirstOrDefault();

            if (_imageCountToShowWeather-- == 0)
            {
                _stateContainer.DisplayWeatherWidgetType = DisplayWeatherWidgetType.Extended;
                return displayItem;
            }
            
            _stateContainer.DisplayWeatherWidgetType = DisplayWeatherWidgetType.Standard;

            _queue.Remove(displayItem);

            await StoreQueue(_queue);

            return displayItem;
        }

        public async Task<List<DisplayItem>> GetDisplayQueueAsync()
        {
            if (_queue is null || _queue.Count == 0)
            {
                if (await IsShuffled())
                {
                    var storedQueue = await GetStoredQueue();
                    if (storedQueue?.Count > 0)
                    {
                        _queue = storedQueue;
                    }
                    else
                    {
                        _queue = await GetShuffledList();
                    }
                }
                else
                {
                    _queue = await GetNaturalOrderList();
                }
            }

            return _queue;
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
                    FileSize = fileSize.ToString("#.##")
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
                                FileSize = fileSize.ToString("#.##")
                            });
                        }
                        catch (Exception ex)
                        {
                            throw ex;
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
                item.ItemNumber = ++itemNum;

                //Console.WriteLine($"{item.ItemNumber} {item.Title}");
            }

            return shuffledList.ToList();

        }

        private async Task StoreQueue(List<DisplayItem> queue)
        {
            await _localStorage.SetItemAsync(Constants.KeyName_DisplayQueue, queue);
        }

        private async Task<List<DisplayItem>> GetStoredQueue()
        {
            var queue = await _localStorage.GetItemAsync<List<DisplayItem>>(Constants.KeyName_DisplayQueue);
            return queue;

        }

        private async Task<DriveFileList> GetDriveFileList()
        {
            string folderId = await _localStorage.GetItemAsync<string>(Constants.KeyName_DriveFolderId);
            return await _googleDriveApi.GetDriveFileList(folderId);
        }

    }
}
