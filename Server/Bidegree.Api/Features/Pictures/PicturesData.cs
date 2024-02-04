using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;

namespace BiDegree.Api.Features.Pictures
{
    public class PicturesData        :IPicturesData
    {
        public string[] GetFiles()
        {
            var picturesFolder = Path.Combine(Environment.CurrentDirectory, 
                @"../../Client/wwwroot/localphotos/PhotoFrameDevStatic");

            DirectoryInfo directoryInfo = new(picturesFolder);

            var files = directoryInfo.GetFiles();

            List<string> fileNames = new();
           
            foreach (var file in files)
            {
                fileNames.Add(file.Name);
            }


            return fileNames.ToArray(); 
        }

        public (FileStream imageStream, string contentType) GetFile(string folder, string pictureName)
        {
            ////var fileFullName = $"{Environment.CurrentDirectory}/Store/Pictures/{folder}/{pictureName}";
            var fileFullName = Path.Combine(Environment.CurrentDirectory, 
                @"../../Client/wwwroot/localphotos/PhotoFrameDevStatic", 
                pictureName);

            using var imageStream = File.OpenRead(fileFullName);
            var image = Image.Load(imageStream);

            image.Metadata.ExifProfile?.TryGetValue(ExifTag.Orientation, out var exifOrientation);

            image.Mutate(x => x.AutoOrient());

            return (imageStream, "image/jpeg");
        }
    }
}
