using BiDegree.Shared.Models;
using MudBlazor;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;

namespace BiDegree.Api.Features.Pictures
{
    public class PicturesData : IPicturesData
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


        public List<DisplayItem> DisplayItemList(string folderName, bool shuffled = true)
        {
            List<DisplayItem> displayItems = new();
            var fileNames = GetFileNames(folderName);

            foreach (var fileName in fileNames)
            {
                var img = GetImage(PhysicalPath(folderName, fileName)!);

                IExifValue<ushort>? exifOrientation = null;

                _ = img.Metadata.ExifProfile?.TryGetValue(ExifTag.Orientation, out exifOrientation);

                _ = int.TryParse(exifOrientation?.Value.ToString(), out int orientation);

                img.Mutate(x => x.AutoOrient());

                displayItems.Add(new DisplayItem
                {
                    FileName = fileName,
                    Width = img.Width,
                    Height = img.Height,
                    Rotation = orientation == 1 ? 0 : orientation
                });
            }

            return displayItems;
        }

        static string[] GetFileNames(string folderName)
        {
            string? picturesFolder = PhysicalPath(folderName);

            if (picturesFolder is null) return Array.Empty<string>();

            DirectoryInfo directoryInfo = new(picturesFolder);

            var files = directoryInfo.GetFiles();

            List<string> fileNames = new();

            foreach (var file in files)
            {
                fileNames.Add(file.Name);
            }

            return fileNames.ToArray();
        }

        static string PhysicalPath(string folderName, string? fileName = null)
        {
            var path = Path.Combine(Environment.CurrentDirectory,
                @"../../Client/wwwroot/localphotos",
                folderName);

            if (fileName is not null)
            {
                path = Path.Combine(path, fileName);
            }

            return path;
        }

        public static Image GetImage(string imageFullName)
        {
            using var imageStream = File.OpenRead(imageFullName);
            var image = Image.Load(imageStream);

            return image;
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
