using System;
using System.Windows.Media.Imaging;

namespace QrCode.Web.Encoder.Windows.Render
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public static class ImageFormatExtension
    {
        /// <summary>
        /// Chooses the encoder.
        /// </summary>
        /// <param name="imageFormat">The image format.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static BitmapEncoder ChooseEncoder(this ImageFormatEnum imageFormat)
        {
            switch (imageFormat)
            {
                case ImageFormatEnum.BMP:
                    return new BmpBitmapEncoder();
                case ImageFormatEnum.GIF:
                    return new GifBitmapEncoder();
                case ImageFormatEnum.JPEG:
                    return new JpegBitmapEncoder();
                case ImageFormatEnum.PNG:
                    return new PngBitmapEncoder();
                case ImageFormatEnum.TIFF:
                    return new TiffBitmapEncoder();
                case ImageFormatEnum.WDP:
                    return new WmpBitmapEncoder();
                default:
                    throw new ArgumentOutOfRangeException("imageFormat", imageFormat,
                                                          "No such encoder support for this imageFormat");
            }
        }
    }
}