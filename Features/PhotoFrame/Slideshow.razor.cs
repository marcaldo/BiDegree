using BiDegree.Models;
using BiDegree.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BiDegree.Features.PhotoFrame
{
    public partial class Slideshow : ComponentBase
    {
        [Inject] IDisplayQueue DisplayQueue { get; set; }
        [Parameter] public bool DebugMode { get; set; } = false;

        private const string TRANSPARENT = "transparent";
        private const string VISIBLE = "";

        private DisplayItem _imgTop = null;
        private DisplayItem _imgBottom = null;

        private DisplayItem _videoTop = null;
        private DisplayItem _videoBottom = null;

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
            var nextItem = await DisplayQueue.GetNextItemAsync();

            if (nextItem.ItemType == DisplayItemType.Image)
            {
                _videoTop.CssClass = TRANSPARENT;
                _videoBottom.CssClass = TRANSPARENT;

                if (_imgTop.CssClass == VISIBLE)
                {
                    _imgBottom = nextItem;
                    _imgBottom.CssClass = TRANSPARENT;
                }
                else
                {
                    _imgTop = nextItem;
                    _imgTop.CssClass = TRANSPARENT;
                }
            }

            if (nextItem.ItemType == DisplayItemType.Video)
            {
                _imgTop.CssClass = TRANSPARENT;
                _imgBottom.CssClass = TRANSPARENT;

                if (_videoTop.CssClass == VISIBLE)
                {
                    _videoBottom = nextItem;
                    _videoBottom.CssClass = TRANSPARENT;
                }
                else
                {
                    _videoTop = nextItem;
                    _videoTop.CssClass = TRANSPARENT;
                }
            }

            StateHasChanged();
        }


        public void ShowNext()
        {
            if (_imgTop.CssClass == VISIBLE)
            {
                _imgTop.CssClass = TRANSPARENT;
                _imgBottom.CssClass = VISIBLE;

                //Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Hiding TOP.");

            }
            else
            {
                _imgTop.CssClass = VISIBLE;
                _imgBottom.CssClass = TRANSPARENT;

                //Console.WriteLine($"{DateTime.Now.ToLongTimeString()} Hiding BOTTOM.");

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
