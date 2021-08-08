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

        private CurrentDisplay Current = CurrentDisplay.None;
        private ViewStatus _viewStatus = new();


        protected override async Task OnInitializedAsync()
        {
            await SetInitialItems();
        }

        private async Task SetInitialItems()
        {
            _imgTop = await DisplayQueue.GetNextItemAsync();
            _imgBottom = await DisplayQueue.GetNextItemAsync();

            _imgTop.CssClass = VISIBLE;
            _imgBottom.CssClass = TRANSPARENT;

            StateHasChanged();
        }

        public async Task LoadNextInBackground()
        {
            if (_imgTop.CssClass == VISIBLE)
            {
                _imgBottom = await DisplayQueue.GetNextItemAsync();
                _imgBottom.CssClass = TRANSPARENT;

                Console.WriteLine("Loading Background BOTTOM " + _imgBottom.Title);

            }
            else
            {
                _imgTop = await DisplayQueue.GetNextItemAsync();
                _imgTop.CssClass = TRANSPARENT;

                Console.WriteLine("Loading Background TOP " + _imgTop.Title);

            }
        }


        public void ShowNext()
        {
            if (_imgTop.CssClass == VISIBLE)
            {
                _imgTop.CssClass = TRANSPARENT;
                _imgBottom.CssClass = VISIBLE;

                Console.WriteLine("Showing Bottom -> TOP to Transparent.");

            }
            else
            {
                _imgTop.CssClass = VISIBLE;
                _imgBottom.CssClass = TRANSPARENT;

                Console.WriteLine("Showing Top -> BOTTOM to Transparent.");

            }

            StateHasChanged();
        }

        private void SetCurrentDisplay(DisplayItem displayItem, CurrentDisplay currentDisplay)
        {
            _viewStatus.ImgTopClass = TRANSPARENT;
            _viewStatus.ImgBottomClass = TRANSPARENT;

            switch (currentDisplay)
            {
                case CurrentDisplay.ImgTop:
                    _viewStatus.CurrentDisplay = currentDisplay;
                    _viewStatus.ImgTopSrc = displayItem.SourceUrl;
                    _viewStatus.ImgTopClass = VISIBLE;
                    break;
                case CurrentDisplay.ImgBottom:
                    _viewStatus.CurrentDisplay = currentDisplay;
                    _viewStatus.ImgBottomSrc = displayItem.SourceUrl;
                    _viewStatus.ImgBottomClass = VISIBLE;
                    break;
                case CurrentDisplay.VideoTop:
                    break;
                case CurrentDisplay.VideoBottom:
                    break;
                case CurrentDisplay.Weather:
                    break;
                default:
                    break;
            }
        }

    }

    class ViewStatus
    {
        public string ImgTopSrc { get; set; }
        public string ImgBottomSrc { get; set; }
        public string ImgTopClass { get; set; }
        public string ImgBottomClass { get; set; }
        public CurrentDisplay CurrentDisplay { get; set; }

    }
    enum CurrentDisplay
    {
        None,
        ImgTop, ImgBottom,
        VideoTop, VideoBottom,
        Weather
    }
}
