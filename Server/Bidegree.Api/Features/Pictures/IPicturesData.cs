
namespace BiDegree.Api.Features.Pictures
{
    public interface IPicturesData
    {
        (FileStream imageStream, string contentType) GetFile(string folder, string pictureName);
        string[] GetFiles();
    }
}
