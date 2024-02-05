
using BiDegree.Shared.Models;

namespace BiDegree.Api.Features.Pictures
{
    public interface IPicturesData
    {
        List<DisplayItem> DisplayItemList(string folderName, bool shuffled = true);
        (FileStream imageStream, string contentType) GetFile(string folder, string pictureName);
        string[] GetFiles();
    }
}
