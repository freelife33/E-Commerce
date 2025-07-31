namespace ECommerce.Web.Helpers
{
    public static class PathHelper
    {
        public static string ToImagePath(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "/images/no-image.png";

            return "/" + path.TrimStart('/');
        }
    }
}
