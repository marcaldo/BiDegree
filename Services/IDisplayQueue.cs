using BiDegree.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public interface IDisplayQueue
    {
        Task<(List<DisplayItem> displayItems1, List<DisplayItem> displayItems2)> GetDisplayQueuesAsync();
    }
}