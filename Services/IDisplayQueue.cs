using BiDegree.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public interface IDisplayQueue
    {
        Task<List<DisplayItem>> GetDisplayQueueAsync();
        Task<double> GetDisplayTimeAsync();
        Task<DisplayItem> GetNextItemAsync();
        Task<bool> IsDebugModeAsync();
    }
}