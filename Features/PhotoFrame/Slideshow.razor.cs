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
    public partial class Slideshow : ComponentBase, IDisposable
    {
        [Inject] IDisplayQueue DisplayQueue { get; set; }
        readonly System.Timers.Timer timer = new();
        private bool _debugMode = true;
        private string imgTopSrc;
        private string imgBottomSrc;
        private string imgTopClass;
        private string imgBottomClass;
        private const string TRANSPARENT = "transparent";
        private const string VISIBLE = "";
        private CurrentDisplay Current = CurrentDisplay.None;
        private ViewStatus _viewStatus = new();

        protected override async Task OnInitializedAsync()
        {
            var displayTime = await DisplayQueue.GetDisplayTimeAsync();

            timer.Interval = displayTime;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;

            var queue = await DisplayQueue.GetDisplayQueueAsync();

            await SetInitialItems();
        }

        private async Task SetInitialItems()
        {
            _viewStatus.CurrentDisplay = CurrentDisplay.ImgTop;

            imgBottomClass = TRANSPARENT;

            var item = await DisplayQueue.GetNextItemAsync();
            imgTopSrc = item.SourceUrl;

            item = await DisplayQueue.GetNextItemAsync();
            imgBottomSrc = item.SourceUrl;

            StateHasChanged();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            imgTopClass = TRANSPARENT;
            imgBottomClass = VISIBLE;

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

        public void Dispose()
        {
            timer?.Dispose();
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
