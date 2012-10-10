using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace QrCode.Web.Encoder.Windows.Render
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class GraphicsRenderer
    {
        /// <summary>
        /// 
        /// </summary>
        private Brush m_DarkBrush;

        /// <summary>
        /// 
        /// </summary>
        private Brush m_LightBrush;

        /// <summary>
        /// 
        /// </summary>
        private ISizeCalculation m_iSize;

        /// <summary>
        /// Initialize Renderer. Default brushes will be black and white.
        /// </summary>
        /// <param name="iSize">Size of the i.</param>
        /// <remarks></remarks>
        public GraphicsRenderer(ISizeCalculation iSize)
            : this(iSize, Brushes.Black, Brushes.White)
        {
        }

        /// <summary>
        /// Initialize Renderer
        /// </summary>
        /// <param name="iSize">Size of the i.</param>
        /// <param name="darkBrush">The dark brush.</param>
        /// <param name="lightBrush">The light brush.</param>
        /// <remarks></remarks>
        public GraphicsRenderer(ISizeCalculation iSize, Brush darkBrush, Brush lightBrush)
        {
            m_iSize = iSize;

            m_DarkBrush = darkBrush;
            m_LightBrush = lightBrush;
        }

        /// <summary>
        /// DarkBrush for drawing Dark module of QrCode
        /// </summary>
        /// <value>The dark brush.</value>
        /// <remarks></remarks>
        public Brush DarkBrush
        {
            set { m_DarkBrush = value; }
            get { return m_DarkBrush; }
        }

        /// <summary>
        /// LightBrush for drawing Light module and QuietZone of QrCode
        /// </summary>
        /// <value>The light brush.</value>
        /// <remarks></remarks>
        public Brush LightBrush
        {
            set { m_LightBrush = value; }
            get { return m_LightBrush; }
        }

        /// <summary>
        /// ISizeCalculation for the way to calculate QrCode's pixel size.
        /// Ex for ISizeCalculation:FixedCodeSize, FixedModuleSize
        /// </summary>
        /// <value>The size calculator.</value>
        /// <remarks></remarks>
        public ISizeCalculation SizeCalculator
        {
            set { m_iSize = value; }
            get { return m_iSize; }
        }

        /// <summary>
        /// Drawing Bitmatrix to winform graphics.
        /// Default position will be 0, 0
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="QrMatrix">The qr matrix.</param>
        /// <exception cref="ArgumentNullException">DarkBrush or LightBrush is null</exception>
        /// <remarks></remarks>
        public void Draw(Graphics graphics, BitMatrix QrMatrix)
        {
            Draw(graphics, QrMatrix, new Point(0, 0));
        }

        /// <summary>
        /// Drawing Bitmatrix to winform graphics.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="QrMatrix">Draw background only for null matrix</param>
        /// <param name="offset">The offset.</param>
        /// <exception cref="ArgumentNullException">DarkBrush or LightBrush is null</exception>
        /// <remarks></remarks>
        public void Draw(Graphics graphics, BitMatrix QrMatrix, Point offset)
        {
            int width = QrMatrix == null ? 21 : QrMatrix.Width;

            DrawingSize size = m_iSize.GetSize(width);

            graphics.FillRectangle(m_LightBrush, offset.X, offset.Y, size.CodeWidth, size.CodeWidth);

            if (QrMatrix == null || size.ModuleSize == 0)
                return;

            int padding = (size.CodeWidth - (size.ModuleSize*width))/2;

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
                            var modulePosition = new Point(preX*size.ModuleSize + padding + offset.X,
                                                           y*size.ModuleSize + padding + offset.Y);
                            var rectSize = new Size((x - preX + 1)*size.ModuleSize, size.ModuleSize);
                            graphics.FillRectangle(m_DarkBrush, modulePosition.X, modulePosition.Y, rectSize.Width,
                                                   rectSize.Height);
                            preX = -1;
                        }
                    }
                    else if (!QrMatrix[x, y] && preX != -1)
                    {
                        //Here will be first light module after sequence of dark module.
                        //Draw previews sequence of dark Module
                        var modulePosition = new Point(preX*size.ModuleSize + padding + offset.X,
                                                       y*size.ModuleSize + padding + offset.Y);
                        var rectSize = new Size((x - preX)*size.ModuleSize, size.ModuleSize);
                        graphics.FillRectangle(m_DarkBrush, modulePosition.X, modulePosition.Y, rectSize.Width,
                                               rectSize.Height);
                        preX = -1;
                    }
                }
            }
        }

        /// <summary>
        /// Saves QrCode Image to specified stream in the specified format
        /// </summary>
        /// <param name="QrMatrix">The qr matrix.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="DPI">The DPI.</param>
        /// <exception cref="ArgumentNullException">Stream or Format is null</exception>
        ///   
        /// <exception cref="ExternalException">The image was saved with the wrong image format</exception>
        /// <remarks>You should avoid saving an image to the same stream that was used to construct. Doing so might damage the stream
        /// If any additional data has been written to the stream before saving the image, the image data in the stream will be corrupted</remarks>
        public void WriteToStream(BitMatrix QrMatrix, ImageFormat imageFormat, Stream stream, Point DPI)
        {
            if (imageFormat == ImageFormat.Emf || imageFormat == ImageFormat.Wmf)
            {
                CreateMetaFile(QrMatrix, stream);
            }
            else if (imageFormat != ImageFormat.Exif
                     && imageFormat != ImageFormat.Icon
                     && imageFormat != ImageFormat.MemoryBmp)
            {
                DrawingSize size = m_iSize.GetSize(QrMatrix == null ? 21 : QrMatrix.Width);

                using (var bitmap = new Bitmap(size.CodeWidth, size.CodeWidth))
                {
                    if (DPI.X != 96 || DPI.Y != 96)
                        bitmap.SetResolution(DPI.X, DPI.Y);
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        Draw(graphics, QrMatrix);
                        bitmap.Save(stream, imageFormat);
                    }
                }
            }
        }

        /// <summary>
        /// Writes to stream.
        /// </summary>
        /// <param name="QrMatrix">The qr matrix.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <param name="stream">The stream.</param>
        /// <remarks></remarks>
        public void WriteToStream(BitMatrix QrMatrix, ImageFormat imageFormat, Stream stream)
        {
            WriteToStream(QrMatrix, imageFormat, stream, new Point(96, 96));
        }

        /// <summary>
        /// Using MetaFile Class to create metafile.
        /// temp control create to use as object to get temp graphics for Hdc.
        /// Drawing on the metaGraphics will record as vector metaFile.
        /// </summary>
        /// <param name="QrMatrix">The qr matrix.</param>
        /// <param name="stream">The stream.</param>
        /// <remarks></remarks>
        private void CreateMetaFile(BitMatrix QrMatrix, Stream stream)
        {
            using (var gControl = new Control())
            using (Graphics newGraphics = gControl.CreateGraphics())
            {
                IntPtr hdc = newGraphics.GetHdc();
                using (var metaFile = new Metafile(stream, hdc))
                using (Graphics metaGraphics = Graphics.FromImage(metaFile))
                {
                    Draw(metaGraphics, QrMatrix);
                }
            }
        }
    }
}