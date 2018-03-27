using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageCompressor.Core
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
		/// <exception cref="ArgumentOutOfRangeException">When qualityLevel is higher than 100</exception>
		public static void Compress(string imagePath, string imageCompressedPath, int maxWidthPixels, byte qualityLevel)
		{
			if (qualityLevel > 100)
				throw new ArgumentOutOfRangeException(nameof(qualityLevel), "Value must be 0 to 100");

			using (var fs = new FileStream(imagePath, FileMode.Open))
			{
				// Bitmap
				var imageBitmap = new Bitmap(fs);

				// Image encoder
				ImageCodecInfo imageEncoder = GetEncoder(imageBitmap.RawFormat);

				// Rezise image
				imageBitmap = Resize(imageBitmap, maxWidthPixels);

				// Creating Encoder
				var encoderParameters = new EncoderParameters(1);
				encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityLevel);

				// Saving image in "imageCompressedPath"
				imageBitmap.Save(imageCompressedPath, imageEncoder, encoderParameters);

				imageBitmap.Dispose();
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
			Compress(imagePath, imageCompressedPath, maxWidthPixels, (byte)imageQuality);
		}

		/// <summary>
		/// Get Image encoder
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
		/// <param name="maxWidth">Max image width in pixels</param>
		/// <returns></returns>
		private static Bitmap Resize(Image image, int maxWidth)
		{
			if (image.Width <= maxWidth)
				return new Bitmap(image);

			double scale = image.Width / (double)maxWidth;
			int newHeight = (int)(image.Height / scale);
			return new Bitmap(image, maxWidth, newHeight);
		}
	}
}