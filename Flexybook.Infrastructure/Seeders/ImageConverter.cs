namespace Flexybook.Infrastructure.Seeders
{
    /// <summary>
    /// Handles conversion of image files to Base64-encoded data URIs.
    /// </summary>
    public static class ImageConverter
    {
        /// <summary>
        /// Converts an image file to a Base64-encoded data URI string.
        /// </summary>
        /// <param name="imagePath">The relative path to the image file (e.g., "/images/photo.jpg").</param>
        /// <returns>A Base64-encoded data URI string, or empty string if conversion fails.</returns>
        public static string ConvertToBase64(string imagePath)
        {
            try
            {
                var fullPath = BuildFullPath(imagePath);
                
                if (!File.Exists(fullPath))
                    return string.Empty;

                var imageBytes = File.ReadAllBytes(fullPath);
                var mimeType = GetMimeType(fullPath);

                return BuildDataUri(imageBytes, mimeType);
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string BuildFullPath(string imagePath)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
        }

        private static string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "image/jpeg"
            };
        }

        private static string BuildDataUri(byte[] imageBytes, string mimeType)
        {
            var base64String = Convert.ToBase64String(imageBytes);
            return $"data:{mimeType};base64,{base64String}";
        }
    }
}
