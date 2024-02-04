namespace BiDegree.Api.Features.Pictures
{
    internal static class PicturesEndpoints
    {
        public static void MapPictures(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/pictures", (IPicturesData picturesData) =>
            {
                var pictures = picturesData.GetFiles();
                return pictures;

            }).WithName("GetPictures")
              .WithOpenApi();


            app.MapGet("/api/GetFile", (IPicturesData picturesData) =>
            {
                
                var (imageStream, contentType) = picturesData.GetFile("PhotoFrameDev", "03-dino.jpg");
                return (imageStream, contentType);
            });
        }
    }
}
