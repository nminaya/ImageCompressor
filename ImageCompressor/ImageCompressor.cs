using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageCompressorLibrary
{
    /// <summary>
    /// Image Compressor
    /// </summary>
    public static class ImageCompressor
    {
        /// <summary>
        /// Compress and resize an image
        /// </summary>
        /// <param name="imagePath">Image path</param>
        /// <param name="imageCompressedPath">New path of image compressed</param>
        /// <param name="maxWidthPixels">Max image width in pixels</param>
        /// <param name="qualityLevel">Quelity level 0 to 100</param>
        public static void Compress(string imagePath, string imageCompressedPath, int maxWidthPixels, short qualityLevel)
        {
            using (var fs = new FileStream(imagePath, FileMode.Open))
            {
                var imageBitmap = new Bitmap(fs);

                // Image encoder
                ImageCodecInfo imageEncoder = GetEncoder(imageBitmap.RawFormat);

                // Rezise image
                imageBitmap = Resize(imageBitmap, maxWidthPixels);
                var encoder = Encoder.Quality;
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(encoder, qualityLevel);

                // Saving image
                imageBitmap.Save(imageCompressedPath, imageEncoder, encoderParameters);
            }
        }

        /// <summary>
        /// Compress and resize an image
        /// </summary>
        /// <param name="imagePath">Image path</param>
        /// <param name="imageCompressedPath">New path of image compressed</param>
        /// <param name="maxWidthPixels">Max image width in pixels</param>
        /// <param name="imageQuality">Quality of Image</param>
        public static void Compress(string imagePath, string imageCompressedPath, int maxWidthPixels, ImageQuality imageQuality)
        {
            Compress(imagePath, imageCompressedPath, maxWidthPixels, (short)imageQuality);
        }

        /// <summary>
        /// Get encoder of Image
        /// </summary>
        /// <param name="format">Image Format</param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().FirstOrDefault(v => v.FormatID == format.Guid);
        }

        /// <summary>
        /// Resize Image
        /// </summary>
        /// <param name="image">Image object</param>
        /// <param name="width">New image Width</param>
        /// <returns></returns>
        private static Bitmap Resize(Image image, int width)
        {
            if (image.Width <= width)
                return new Bitmap(image);

            double scale = image.Width / (double)width;
            int newHeight = (int)(image.Height / scale);
            return new Bitmap(image, width, newHeight);
        }
    }


}