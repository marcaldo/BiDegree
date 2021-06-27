using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BiDegree.Shared
{
    public class DebugMode : IDebugMode
    {
        [Inject] ILocalStorageService LocalStorage { get; set; }

        public int PictureCount { get; set; }
        public bool IsActive { get; set; }
    }
}
