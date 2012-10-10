using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using QrCode.Web.Encoder.Windows.Render;

namespace QrCode.Web.Encoder.Windows.WPF
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class QrCodeImgControl : Control
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly int m_DpiX = 96;

        /// <summary>
        /// 
        /// </summary>
        private readonly int m_DpiY = 96;

        /// <summary>
        /// 
        /// </summary>
        private QrCode m_QrCode = new QrCode();

        /// <summary>
        /// 
        /// </summary>
        private bool m_isFreezed;

        /// <summary>
        /// 
        /// </summary>
        private bool m_isLocked;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Controls.Control"/> class.
        /// </summary>
        /// <remarks></remarks>
        static QrCodeImgControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (QrCodeImgControl),
                                                     new FrameworkPropertyMetadata(typeof (QrCodeImgControl)));
            HorizontalAlignmentProperty.OverrideMetadata(typeof (QrCodeImgControl),
                                                         new FrameworkPropertyMetadata(HorizontalAlignment.Center));
            VerticalAlignmentProperty.OverrideMetadata(typeof (QrCodeImgControl),
                                                       new FrameworkPropertyMetadata(VerticalAlignment.Center));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Controls.Control"/> class.
        /// </summary>
        /// <remarks></remarks>
        public QrCodeImgControl()
        {
            MatrixPoint dpi = GetDPI();
            m_DpiX = dpi.X;
            m_DpiY = dpi.Y;
            EncodeAndUpdateBitmap();
        }

        /// <summary>
        /// Occurs when [qr matrix changed].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler QrMatrixChanged;

        /// <summary>
        /// Gets the DPI.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public MatrixPoint GetDPI()
        {
            int dpix, dpiy;
            PresentationSource source = PresentationSource.FromVisual(this);
            if (source != null)
            {
                Matrix dpi = source.CompositionTarget.TransformToDevice;
                dpix = (int) (96*dpi.M11);
                dpiy = (int) (96*dpi.M22);
                return new MatrixPoint(dpix, dpiy);
            }
            else
                return new MatrixPoint(m_DpiX, m_DpiY);
        }

        /// <summary>
        /// QrCode matrix cache updated.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void OnQrMatrixChanged(EventArgs e)
        {
            if (QrMatrixChanged != null)
                QrMatrixChanged(this, e);
        }

        /// <summary>
        /// Get Qr SquareBitMatrix as two dimentional bool array.
        /// It will be deep copy of control's internal bitmatrix.
        /// </summary>
        /// <returns>null if matrix is null, else full matrix</returns>
        /// <remarks></remarks>
        public SquareBitMatrix GetQrMatrix()
        {
            if (m_QrCode.Matrix == null)
                return null;
            else
            {
                BitMatrix matrix = m_QrCode.Matrix;
                bool[,] internalArray = matrix.InternalArray;
                return new SquareBitMatrix(internalArray);
            }
        }

        #region ReDraw Bitmap, Update Qr Cache

        /// <summary>
        /// Creates the bitmap.
        /// </summary>
        /// <remarks></remarks>
        private void CreateBitmap()
        {
            int pixelWidth = (int) QrCodeWidthInch*m_DpiX;
            int suitableWidth = m_QrCode.Matrix == null
                                    ? CalculateSuitableWidth(pixelWidth, 21)
                                    : CalculateSuitableWidth(pixelWidth, m_QrCode.Matrix.Width);
            PixelFormat pFormat = IsGrayImage ? PixelFormats.Gray8 : PixelFormats.Pbgra32;

            if (WBitmap == null)
            {
                WBitmap = new WriteableBitmap(suitableWidth, suitableWidth, m_DpiX, m_DpiY, pFormat, null);
                return;
            }

            if (WBitmap.PixelHeight != suitableWidth || WBitmap.PixelWidth != suitableWidth || WBitmap.Format != pFormat)
            {
                WBitmap = null;
                WBitmap = new WriteableBitmap(suitableWidth, suitableWidth, m_DpiX, m_DpiY, pFormat, null);
            }
        }

        /// <summary>
        /// Calculates the width of the suitable.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="bitMatrixWidth">Width of the bit matrix.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private int CalculateSuitableWidth(int width, int bitMatrixWidth)
        {
            var isize = new FixedCodeSize(width, QuietZoneModule);
            DrawingSize dSize = isize.GetSize(bitMatrixWidth);
            int gap = dSize.CodeWidth - dSize.ModuleSize*(bitMatrixWidth + 2*(int) QuietZoneModule);

            if (gap == 0)
                return width;
            else if (dSize.CodeWidth/gap < 4)
                return (dSize.ModuleSize + 1)*(bitMatrixWidth + 2*(int) QuietZoneModule);
            else
                return dSize.ModuleSize*(bitMatrixWidth + 2*(int) QuietZoneModule);
        }

        /// <summary>
        /// Updates the source.
        /// </summary>
        /// <remarks></remarks>
        private void UpdateSource()
        {
            CreateBitmap();

            if (WBitmap.PixelWidth != 0 && WBitmap.PixelHeight != 0)
            {
                WBitmap.Clear(LightColor);

                if (m_QrCode.Matrix != null)
                {
                    //WBitmap.
                    new WriteableBitmapRenderer(new FixedCodeSize(WBitmap.PixelWidth, QuietZoneModule), DarkColor,
                                                LightColor).DrawDarkModule(WBitmap, m_QrCode.Matrix, 0, 0);
                }
            }
        }

        /// <summary>
        /// Updates the qr code cache.
        /// </summary>
        /// <remarks></remarks>
        private void UpdateQrCodeCache()
        {
            new QrEncoder(ErrorCorrectLevel).TryEncode(Text, out m_QrCode);
            OnQrMatrixChanged(new EventArgs());
        }

        #endregion

        #region Event method

        /// <summary>
        /// Called when [visual value changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private static void OnVisualValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((QrCodeImgControl) d).UpdateBitmap();
        }

        /// <summary>
        /// Encode and Update bitmap when ErrorCorrectlevel or Text changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private static void OnQrValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((QrCodeImgControl) d).EncodeAndUpdateBitmap();
        }

        #endregion

        #region Update method

        /// <summary>
        /// Encodes the and update bitmap.
        /// </summary>
        /// <remarks></remarks>
        internal void EncodeAndUpdateBitmap()
        {
            if (!IsLocked)
            {
                UpdateQrCodeCache();
                UpdateBitmap();
            }
        }

        /// <summary>
        /// Updates the bitmap.
        /// </summary>
        /// <remarks></remarks>
        internal void UpdateBitmap()
        {
            if (!IsFreezed)
                UpdateSource();
        }

        #endregion

        #region Lock Freeze

        /// <summary>
        /// Return whether if class is locked
        /// </summary>
        /// <remarks></remarks>
        public bool IsLocked
        {
            get { return m_isLocked; }
        }

        /// <summary>
        /// Return whether if class is freezed.
        /// </summary>
        /// <remarks></remarks>
        public bool IsFreezed
        {
            get { return m_isFreezed; }
        }

        /// <summary>
        /// If Class is locked, it won't update QrMatrix cache.
        /// </summary>
        /// <remarks></remarks>
        public void Lock()
        {
            m_isLocked = true;
        }

        /// <summary>
        /// Unlock class will cause class to update QrMatrix Cache and redraw bitmap.
        /// </summary>
        /// <remarks></remarks>
        public void Unlock()
        {
            m_isLocked = false;
            EncodeAndUpdateBitmap();
        }

        /// <summary>
        /// Freeze Class, Any value change to Brush, QuietZoneModule won't cause immediately redraw bitmap.
        /// </summary>
        /// <remarks></remarks>
        public void Freeze()
        {
            m_isFreezed = true;
        }

        /// <summary>
        /// Unfreeze and redraw immediately.
        /// </summary>
        /// <remarks></remarks>
        public void UnFreeze()
        {
            m_isFreezed = false;
            UpdateBitmap();
        }

        #endregion

        #region WBitmap

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty WBitmapProperty =
            DependencyProperty.Register("WBitmap",
                                        typeof (WriteableBitmap),
                                        typeof (QrCodeImgControl),
                                        new UIPropertyMetadata(null, null));

        /// <summary>
        /// Gets the W bitmap.
        /// </summary>
        /// <remarks></remarks>
        public WriteableBitmap WBitmap
        {
            get { return (WriteableBitmap) GetValue(WBitmapProperty); }
            private set { SetValue(WBitmapProperty, value); }
        }

        #endregion

        #region QrCodeWidthInch

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty QrCodeWidthInchProperty =
            DependencyProperty.Register("QrCodeWidth", typeof (double), typeof (QrCodeImgControl),
                                        new UIPropertyMetadata(2.08, OnVisualValueChanged));

        /// <summary>
        /// Gets or sets the qr code width inch.
        /// </summary>
        /// <value>The qr code width inch.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public double QrCodeWidthInch
        {
            get { return (double) GetValue(QrCodeWidthInchProperty); }
            set { SetValue(QrCodeWidthInchProperty, value); }
        }

        #endregion

        #region QuietZoneModule

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty QuietZoneModuleProperty =
            DependencyProperty.Register("QuietZoneModule", typeof (QuietZoneModules), typeof (QrCodeImgControl),
                                        new UIPropertyMetadata(QuietZoneModules.Two, OnVisualValueChanged));

        /// <summary>
        /// Gets or sets the quiet zone module.
        /// </summary>
        /// <value>The quiet zone module.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public QuietZoneModules QuietZoneModule
        {
            get { return (QuietZoneModules) GetValue(QuietZoneModuleProperty); }
            set { SetValue(QuietZoneModuleProperty, value); }
        }

        #endregion

        #region LightColor

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LightColorProperty =
            DependencyProperty.Register("LightColor", typeof (Color), typeof (QrCodeImgControl),
                                        new UIPropertyMetadata(Colors.White, OnVisualValueChanged));

        /// <summary>
        /// Gets or sets the color of the light.
        /// </summary>
        /// <value>The color of the light.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public Color LightColor
        {
            get { return (Color) GetValue(LightColorProperty); }
            set { SetValue(LightColorProperty, value); }
        }

        #endregion

        #region DarkColor

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DarkColorProperty =
            DependencyProperty.Register("DarkColor", typeof (Color), typeof (QrCodeImgControl),
                                        new UIPropertyMetadata(Colors.Black, OnVisualValueChanged));

        /// <summary>
        /// Gets or sets the color of the dark.
        /// </summary>
        /// <value>The color of the dark.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public Color DarkColor
        {
            get { return (Color) GetValue(DarkColorProperty); }
            set { SetValue(DarkColorProperty, value); }
        }

        #endregion

        #region ErrorCorrectionLevel

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ErrorCorrectLevelProperty =
            DependencyProperty.Register("ErrorCorrectLevel", typeof (ErrorCorrectionLevel), typeof (QrCodeImgControl),
                                        new UIPropertyMetadata(ErrorCorrectionLevel.M, OnQrValueChanged));

        /// <summary>
        /// Gets or sets the error correct level.
        /// </summary>
        /// <value>The error correct level.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public ErrorCorrectionLevel ErrorCorrectLevel
        {
            get { return (ErrorCorrectionLevel) GetValue(ErrorCorrectLevelProperty); }
            set { SetValue(ErrorCorrectLevelProperty, value); }
        }

        #endregion

        #region Text

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (QrCodeImgControl),
                                        new UIPropertyMetadata(string.Empty, OnQrValueChanged));

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region IsGrayImage

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsGrayImageProperty =
            DependencyProperty.Register("IsGrayImage", typeof (bool), typeof (QrCodeImgControl),
                                        new UIPropertyMetadata(true, OnVisualValueChanged));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is gray image.
        /// </summary>
        /// <value><c>true</c> if this instance is gray image; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public bool IsGrayImage
        {
            get { return (bool) GetValue(IsGrayImageProperty); }
            set { SetValue(IsGrayImageProperty, value); }
        }

        #endregion
    }
}