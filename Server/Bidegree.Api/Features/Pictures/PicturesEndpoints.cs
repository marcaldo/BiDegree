namespace BiDegree.Api.Features.Pictures
{
    internal static class PicturesEndpoints
    {
        public static void MapPictures(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/pictures", (IPicturesData picturesData) =>
            {
                var pictures = picturesData.GetFiles();

                //var pictures = new[]
                //{
                //    new Picture("https://www.bidegree.com/images/1.jpg", "First Picture"),
                //    new Picture("https://www.bidegree.com/images/2.jpg", "Second Picture"),
                //    new Picture("https://www.bidegree.com/images/3.jpg", "Third Picture"),
                //    new Picture("https://www.bidegree.com/images/4.jpg", "Fourth Picture"),
                //    new Picture("https://www.bidegree.com/images/5.jpg", "Fifth Picture"),
                //};

                return pictures;

            }).WithName("GetPictures")
              .WithOpenApi(); ;
        }

        public record Picture(string Url, string Description);
    }
}
