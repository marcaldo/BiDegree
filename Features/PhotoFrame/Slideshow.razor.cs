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
    public partial class Slideshow : ComponentBase
    {
        [Inject] IDisplayQueue DisplayQueue { get; set; }
        readonly System.Timers.Timer timer = new();
        private bool _debugMode = true;
        private string imgTopSrcUrl;
        private string imgBottomSrcUrl;

        protected override async Task OnInitializedAsync()
        {
            var displayTime = await DisplayQueue.GetDisplayTimeAsync();
            timer.Interval = 1000 * displayTime;
            timer.Elapsed += Timer_Elapsed; ;
            timer.Enabled = true;

            var queue = await DisplayQueue.GetDisplayQueueAsync();

            await SetInitialItems();
        }

        private async Task SetInitialItems()
        {

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
