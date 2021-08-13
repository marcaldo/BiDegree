using BiDegree.Models;
using BiDegree.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BiDegree.Features.PhotoFrame
{
    public partial class Slideshow : ComponentBase
    {
        [Inject] IDisplayQueue DisplayQueue { get; set; }
        [Inject] IJSRuntime JS { get; set; }
        [Parameter] public bool DebugMode { get; set; } = false;

        private const string TRANSPARENT = "transparent";
        private const string VISIBLE = "";
        private const string VIDEO_TOP_ID = "videoTop";
        private const string VIDEO_BOTTOM_ID = "videoBottom";

        private DisplayItem _imgTop = null;
        private DisplayItem _imgBottom = null;

        private DisplayItem _videoTop = null;
        private DisplayItem _videoBottom = null;

        private DisplayItem _nextItem = null;

        protected override async Task OnInitializedAsync()
        {
            await SetInitialItems();
        }

        private async Task SetInitialItems()
        {
            _imgTop = new DisplayItem { CssClass = TRANSPARENT };
            _imgBottom = new DisplayItem { CssClass = TRANSPARENT };
            _videoTop = new DisplayItem { CssClass = TRANSPARENT };
            _videoBottom = new DisplayItem { CssClass = TRANSPARENT };

            var firstItem = await DisplayQueue.GetNextItemAsync();

            switch (firstItem.ItemType)
            {
                case DisplayItemType.Image:
                    _imgTop = firstItem;
                    _imgTop.CssClass = VISIBLE;
                    break;
                case DisplayItemType.Video:
                    _videoTop = firstItem;
                    _videoTop.CssClass = VISIBLE;
                    await JS.InvokeVoidAsync("playVideo", VIDEO_TOP_ID, true);
                    break;
                case DisplayItemType.Weather:
                    break;
                default:
                    break;
            }

            StateHasChanged();
        }


        public async Task LoadNextInBackground()
        {
            try
            {
                _nextItem = await DisplayQueue.GetNextItemAsync();
                //Console.WriteLine($"Loading {_nextItem.Title}...");
            }
            catch
            {
                Console.WriteLine($"ERROR fetching next item to load.");
            }

            if (_nextItem.ItemType == DisplayItemType.Image)
            {
                if (_imgTop.CssClass == VISIBLE)
                {
                    _imgBottom = _nextItem;
                    _imgBottom.CssClass = TRANSPARENT;
                }
                else
                {
                    _imgTop = _nextItem;
                    _imgTop.CssClass = TRANSPARENT;
                }
            }

            if (_nextItem.ItemType == DisplayItemType.Video)
            {
                string videoId;

                if (_videoTop.CssClass == VISIBLE)
                {
                    _videoBottom = _nextItem;
                    _videoBottom.CssClass = TRANSPARENT;
                    videoId = VIDEO_BOTTOM_ID;
                }
                else
                {
                    _videoTop = _nextItem;
                    _videoTop.CssClass = TRANSPARENT;
                    videoId = VIDEO_TOP_ID;
                }

                await JS.InvokeVoidAsync("playVideo", videoId, false);

            }
            
            StateHasChanged();

        }


        public async Task ShowNext()
        {
            if (_nextItem.ItemType == DisplayItemType.Image)
            {
                _videoTop.CssClass = TRANSPARENT;
                _videoBottom.CssClass = TRANSPARENT;

                if (_imgTop.CssClass == VISIBLE)
                {
                    _imgTop.CssClass = TRANSPARENT;
                    _imgBottom.CssClass = VISIBLE;

                    //Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Showing I BOTTOM.");

                }
                else
                {
                    _imgTop.CssClass = VISIBLE;
                    _imgBottom.CssClass = TRANSPARENT;

                    //Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Showing I TOP.");

                }
            }

            if (_nextItem.ItemType == DisplayItemType.Video)
            {

                _imgTop.CssClass = TRANSPARENT;
                _imgBottom.CssClass = TRANSPARENT;

                if (_videoTop.CssClass == VISIBLE)
                {
                    await JS.InvokeVoidAsync("playVideo", VIDEO_BOTTOM_ID, true);

                    _videoTop.CssClass = TRANSPARENT;
                    _videoBottom.CssClass = VISIBLE;

                    //Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Showing V BOTTOM.");
                }
                else
                {
                    await JS.InvokeVoidAsync("playVideo", VIDEO_TOP_ID, true);

                    _videoTop.CssClass = VISIBLE;
                    _videoBottom.CssClass = TRANSPARENT;

                    //Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Showing V TOP.");
                }
            }


            StateHasChanged();

        }

    }

    enum CurrentDisplay
    {
        None,
        ImgTop, ImgBottom,
        VideoTop, VideoBottom,
        Weather
    }
}
