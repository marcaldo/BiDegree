using System.Threading.Tasks;

namespace BiDegree.Shared
{
    public interface IDebugMode
    {
        bool IsActive { get; set; }
        int PictureCount { get; set; }
    }
}