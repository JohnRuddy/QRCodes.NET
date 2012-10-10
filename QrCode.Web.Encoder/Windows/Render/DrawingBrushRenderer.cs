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
    public class DrawingBrushRenderer
    {
        /// <summary>
        /// 
        /// </summary>
        private Brush m_DarkBrush;

        /// <summary>
        /// 
        /// </summary>
        private ISizeCalculation m_ISize;

        /// <summary>
        /// 
        /// </summary>
        private Brush m_LightBrush;

        /// <summary>
        /// Initialize Renderer. Default brushes will be black and white.
        /// </summary>
        /// <param name="iSize">Size of the i.</param>
        /// <remarks></remarks>
        public DrawingBrushRenderer(ISizeCalculation iSize)
            : this(iSize, Brushes.Black, Brushes.White)
        {
        }

        /// <summary>
        /// Initialize Renderer.
        /// </summary>
        /// <param name="iSize">Size of the i.</param>
        /// <param name="darkBrush">The dark brush.</param>
        /// <param name="lightBrush">The light brush.</param>
        /// <remarks></remarks>
        public DrawingBrushRenderer(ISizeCalculation iSize, Brush darkBrush, Brush lightBrush)
        {
            m_ISize = iSize;
            m_DarkBrush = darkBrush;
            m_LightBrush = lightBrush;
        }

        /// <summary>
        /// Gets or sets the dark brush.
        /// </summary>
        /// <value>The dark brush.</value>
        /// <remarks></remarks>
        public Brush DarkBrush
        {
            get { return m_DarkBrush; }
            set { m_DarkBrush = value; }
        }

        /// <summary>
        /// Gets or sets the light brush.
        /// </summary>
        /// <value>The light brush.</value>
        /// <remarks></remarks>
        public Brush LightBrush
        {
            get { return m_LightBrush; }
            set { m_LightBrush = value; }
        }

        /// <summary>
        /// Gets or sets the size of the I.
        /// </summary>
        /// <value>The size of the I.</value>
        /// <remarks></remarks>
        public ISizeCalculation ISize
        {
            get { return m_ISize; }
            set { m_ISize = value; }
        }

        /// <summary>
        /// Draw QrCode to DrawingBrush
        /// </summary>
        /// <param name="QrMatrix">The qr matrix.</param>
        /// <returns>DrawingBrush, Stretch = uniform</returns>
        /// <remarks>LightBrush will not use by this method, DrawingBrush will only contain DarkBrush part.
        /// Use LightBrush to fill background of main uielement for more flexible placement</remarks>
        public DrawingBrush DrawBrush(BitMatrix QrMatrix)
        {
            if (QrMatrix == null)
            {
                return ConstructDrawingBrush(null);
            }


            GeometryDrawing qrCodeDrawing = ConstructQrDrawing(QrMatrix, 0, 0);

            return ConstructDrawingBrush(qrCodeDrawing);
        }

        /// <summary>
        /// Construct QrCode geometry. It will only include geometry for Dark colour module
        /// </summary>
        /// <param name="QrMatrix">The qr matrix.</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offSetY">The off set Y.</param>
        /// <returns>QrCode dark colour module geometry. Size = QrMatrix width x width</returns>
        /// <remarks></remarks>
        public StreamGeometry DrawGeometry(BitMatrix QrMatrix, int offsetX, int offSetY)
        {
            int width = QrMatrix == null ? 21 : QrMatrix.Width;

            var qrCodeStream = new StreamGeometry();
            qrCodeStream.FillRule = FillRule.EvenOdd;

            if (QrMatrix == null)
                return qrCodeStream;

            using (StreamGeometryContext qrCodeCtx = qrCodeStream.Open())
            {
                int preX = -1;

                for (int y = 0; y < width; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (QrMatrix[x, y])
                        {
                            //Set start point if preX == -1
                            if (preX == -1)
                                preX = x;
                            //If this is last module in that row. Draw rectangle
                            if (x == width - 1)
                            {
                                qrCodeCtx.DrawRectGeometry(new Int32Rect(preX + offsetX, y + offSetY, x - preX + 1, 1));
                                preX = -1;
                            }
                        }
                        else if (!QrMatrix[x, y] && preX != -1)
                        {
                            //Here will be first light module after sequence of dark module.
                            //Draw previews sequence of dark Module
                            qrCodeCtx.DrawRectGeometry(new Int32Rect(preX + offsetX, y + offSetY, x - preX, 1));
                            preX = -1;
                        }
                    }
                }
            }
            qrCodeStream.Freeze();

            return qrCodeStream;
        }

        /// <summary>
        /// Construct DrawingBrush with input Drawing
        /// </summary>
        /// <param name="drawing">The drawing.</param>
        /// <returns>DrawingBrush where Stretch = uniform</returns>
        /// <remarks></remarks>
        private DrawingBrush ConstructDrawingBrush(Drawing drawing)
        {
            var qrCodeBrush = new DrawingBrush();
            qrCodeBrush.Stretch = Stretch.Uniform;
            qrCodeBrush.Drawing = drawing;
            return qrCodeBrush;
        }


        /// <summary>
        /// Constructs the qr drawing.
        /// </summary>
        /// <param name="QrMatrix">The qr matrix.</param>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offSetY">The off set Y.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private GeometryDrawing ConstructQrDrawing(BitMatrix QrMatrix, int offsetX, int offSetY)
        {
            StreamGeometry qrCodeStream = DrawGeometry(QrMatrix, offsetX, offSetY);

            var qrCodeDrawing = new GeometryDrawing();
            qrCodeDrawing.Brush = m_DarkBrush;

            qrCodeDrawing.Geometry = qrCodeStream;

            return qrCodeDrawing;
        }

        /// <summary>
        /// Constructs the QZ drawing.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private GeometryDrawing ConstructQZDrawing(int width)
        {
            var quietZoneSG = new StreamGeometry();
            quietZoneSG.FillRule = FillRule.EvenOdd;

            using (StreamGeometryContext qrCodeCtx = quietZoneSG.Open())
            {
                qrCodeCtx.DrawRectGeometry(new Int32Rect(0, 0, width, width));
            }
            quietZoneSG.Freeze();

            var quietZoneDrawing = new GeometryDrawing();
            quietZoneDrawing.Brush = m_LightBrush;
            quietZoneDrawing.Geometry = quietZoneSG;

            return quietZoneDrawing;
        }

        /// <summary>
        /// Write image file to stream
        /// Default DPI will be 96, 96
        /// </summary>
        /// <param name="QrMatrix">The qr matrix.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <param name="stream">The stream.</param>
        /// <remarks></remarks>
        public void WriteToStream(BitMatrix QrMatrix, ImageFormatEnum imageFormat, Stream stream)
        {
            WriteToStream(QrMatrix, imageFormat, stream, new Point(96, 96));
        }

        /// <summary>
        /// Write image file to stream
        /// </summary>
        /// <param name="QrMatrix">The qr matrix.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="DPI">DPI = DPI.X, DPI.Y(Dots per inch)</param>
        /// <remarks></remarks>
        public void WriteToStream(BitMatrix QrMatrix, ImageFormatEnum imageFormat, Stream stream, Point DPI)
        {
            BitmapSource bitmapSource = WriteToBitmapSource(QrMatrix, DPI);

            BitmapEncoder encoder = imageFormat.ChooseEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            encoder.Save(stream);
        }

        /// <summary>
        /// Writes to bitmap source.
        /// </summary>
        /// <param name="QrMatrix">The qr matrix.</param>
        /// <param name="DPI">The DPI.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public BitmapSource WriteToBitmapSource(BitMatrix QrMatrix, Point DPI)
        {
            int width = QrMatrix == null ? 21 : QrMatrix.Width;
            DrawingSize dSize = m_ISize.GetSize(width);
            var quietZone = (int) dSize.QuietZoneModules;

            GeometryDrawing quietZoneDrawing = ConstructQZDrawing(2*quietZone + width);
            GeometryDrawing qrDrawing = ConstructQrDrawing(QrMatrix, quietZone, quietZone);

            var qrGroup = new DrawingGroup();
            qrGroup.Children.Add(quietZoneDrawing);
            qrGroup.Children.Add(qrDrawing);

            DrawingBrush qrBrush = ConstructDrawingBrush(qrGroup);

            PixelFormat pixelFormat = PixelFormats.Pbgra32;
            var renderbmp = new RenderTargetBitmap(dSize.CodeWidth, dSize.CodeWidth, DPI.X, DPI.Y, pixelFormat);

            var drawingVisual = new DrawingVisual();
            using (DrawingContext dContext = drawingVisual.RenderOpen())
            {
                dContext.DrawRectangle(qrBrush, null,
                                       new Rect(0, 0, (dSize.CodeWidth/DPI.X)*96, (dSize.CodeWidth/DPI.Y)*96));
            }

            renderbmp.Render(drawingVisual);

            return renderbmp;
        }
    }
}