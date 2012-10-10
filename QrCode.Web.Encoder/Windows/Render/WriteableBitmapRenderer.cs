using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QrCode.Web.Encoder.Windows.Render
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class WriteableBitmapRenderer
    {
        /// <summary>
        /// Initialize renderer
        /// </summary>
        /// <param name="iSize">Size of the i.</param>
        /// <remarks></remarks>
        public WriteableBitmapRenderer(ISizeCalculation iSize)
            : this(iSize, Colors.Black, Colors.White)
        {
        }

        /// <summary>
        /// Initialize renderer
        /// </summary>
        /// <param name="iSize">Size of the i.</param>
        /// <param name="darkColor">Color of the dark.</param>
        /// <param name="lightColor">Color of the light.</param>
        /// <remarks></remarks>
        public WriteableBitmapRenderer(ISizeCalculation iSize, Color darkColor, Color lightColor)
        {
            LightColor = lightColor;
            DarkColor = darkColor;
            ISize = iSize;
        }

        /// <summary>
        /// Gets or sets the color of the dark.
        /// </summary>
        /// <value>The color of the dark.</value>
        /// <remarks></remarks>
        public Color DarkColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the light.
        /// </summary>
        /// <value>The color of the light.</value>
        /// <remarks></remarks>
        public Color LightColor { get; set; }

        /// <summary>
        /// Gets or sets the size of the I.
        /// </summary>
        /// <value>The size of the I.</value>
        /// <remarks></remarks>
        public ISizeCalculation ISize { get; set; }

        /// <summary>
        /// Draw QrCode at given writeable bitmap
        /// </summary>
        /// <param name="wBitmap">The w bitmap.</param>
        /// <param name="matrix">The matrix.</param>
        /// <remarks></remarks>
        public void Draw(WriteableBitmap wBitmap, BitMatrix matrix)
        {
            Draw(wBitmap, matrix, 0, 0);
        }

        /// <summary>
        /// Draw QrCode at given writeable bitmap at offset location
        /// </summary>
        /// <param name="wBitmap">The w bitmap.</param>
        /// <param name="matrix">The matrix.</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        /// <remarks></remarks>
        public void Draw(WriteableBitmap wBitmap, BitMatrix matrix, int offsetX, int offsetY)
        {
            DrawingSize size = matrix == null ? ISize.GetSize(21) : ISize.GetSize(matrix.Width);
            if (wBitmap == null)
                wBitmap = new WriteableBitmap(size.CodeWidth + offsetX, size.CodeWidth + offsetY, 96, 96,
                                              PixelFormats.Gray8, null);
            else if (wBitmap.PixelHeight == 0 || wBitmap.PixelWidth == 0)
                return; //writeablebitmap contains no pixel.
            DrawQuietZone(wBitmap, size.CodeWidth, offsetX, offsetY);
            if (matrix == null)
                return;

            DrawDarkModule(wBitmap, matrix, offsetX, offsetY);
        }

        /// <summary>
        /// Draw quiet zone at offset x,y
        /// </summary>
        /// <param name="wBitmap">The w bitmap.</param>
        /// <param name="pixelWidth">Width of the pixel.</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        /// <remarks></remarks>
        private void DrawQuietZone(WriteableBitmap wBitmap, int pixelWidth, int offsetX, int offsetY)
        {
            wBitmap.FillRectangle(new Int32Rect(offsetX, offsetY, pixelWidth, pixelWidth), LightColor);
        }

        /// <summary>
        /// Draw qrCode dark modules at given position. (It will also include quiet zone area. Set it to zero to exclude quiet zone)
        /// </summary>
        /// <param name="wBitmap">The w bitmap.</param>
        /// <param name="matrix">The matrix.</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        /// <exception cref="ArgumentNullException">Bitmatrix, wBitmap should not equal to null</exception>
        ///   
        /// <exception cref="ArgumentOutOfRangeException">wBitmap's pixel width or height should not equal to zero</exception>
        /// <remarks></remarks>
        public void DrawDarkModule(WriteableBitmap wBitmap, BitMatrix matrix, int offsetX, int offsetY)
        {
            if (matrix == null)
                throw new ArgumentNullException("Bitmatrix");

            DrawingSize size = ISize.GetSize(matrix.Width);

            if (wBitmap == null)
                throw new ArgumentNullException("wBitmap");
            else if (wBitmap.PixelHeight == 0 || wBitmap.PixelWidth == 0)
                throw new ArgumentOutOfRangeException("wBitmap",
                                                      "WriteableBitmap's pixelHeight or PixelWidth are equal to zero");

            int padding = (size.CodeWidth - size.ModuleSize*matrix.Width)/2;

            int preX = -1;
            int moduleSize = size.ModuleSize;

            if (moduleSize == 0)
                return;

            for (int y = 0; y < matrix.Width; y++)
            {
                for (int x = 0; x < matrix.Width; x++)
                {
                    if (matrix[x, y])
                    {
                        if (preX == -1)
                            preX = x;
                        if (x == matrix.Width - 1)
                        {
                            var moduleArea =
                                new Int32Rect(preX*moduleSize + padding + offsetX,
                                              y*moduleSize + padding + offsetY,
                                              (x - preX + 1)*moduleSize,
                                              moduleSize);
                            wBitmap.FillRectangle(moduleArea, DarkColor);
                            preX = -1;
                        }
                    }
                    else if (preX != -1)
                    {
                        var moduleArea =
                            new Int32Rect(preX*moduleSize + padding + offsetX,
                                          y*moduleSize + padding + offsetY,
                                          (x - preX)*moduleSize,
                                          moduleSize);
                        wBitmap.FillRectangle(moduleArea, DarkColor);
                        preX = -1;
                    }
                }
            }
        }

        /// <summary>
        /// Writes to stream.
        /// </summary>
        /// <param name="qrMatrix">The qr matrix.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <param name="stream">The stream.</param>
        /// <remarks></remarks>
        public void WriteToStream(BitMatrix qrMatrix, ImageFormatEnum imageFormat, Stream stream)
        {
            DrawingSize dSize = ISize.GetSize(qrMatrix == null ? 21 : qrMatrix.Width);

            var wBitmap = new WriteableBitmap(dSize.CodeWidth, dSize.CodeWidth, 96, 96, PixelFormats.Gray8, null);

            Draw(wBitmap, qrMatrix);

            BitmapEncoder encoder = imageFormat.ChooseEncoder();
            encoder.Frames.Add(BitmapFrame.Create(wBitmap));
            encoder.Save(stream);
        }
    }
}