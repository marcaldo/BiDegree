namespace BiDegree.Api.Features.Pictures
{
    public class PicturesData        :IPicturesData
    {
        public string[] GetFiles()
        {
            // Get the file list from the folder "Store/Pictures/"
            var picturesFolder = $"{Environment.CurrentDirectory}/Store/Pictures/PhotoFrameDev";

            DirectoryInfo directoryInfo = new DirectoryInfo(picturesFolder);

            var files = directoryInfo.GetFiles();

            List<string> fileNames = new();

            foreach (var file in files)
            {
                fileNames.Add(file.Name);
            }

            return fileNames.ToArray(); 
        }
    }
}
