using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using QrCode.Web.Encoder.Windows.Render;

namespace QrCode.Web.Encoder.Windows.WPF
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class QrCodeGeoControl : Control
    {
        /// <summary>
        /// 
        /// </summary>
        private QrCode m_QrCode = new QrCode();

        /// <summary>
        /// 
        /// </summary>
        private bool m_isLocked;

        /// <summary>
        /// 
        /// </summary>
        private int m_width = 21;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Controls.Control"/> class.
        /// </summary>
        /// <remarks></remarks>
        static QrCodeGeoControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (QrCodeGeoControl),
                                                     new FrameworkPropertyMetadata(typeof (QrCodeGeoControl)));
            HorizontalAlignmentProperty.OverrideMetadata(typeof (QrCodeGeoControl),
                                                         new FrameworkPropertyMetadata(HorizontalAlignment.Center));
            VerticalAlignmentProperty.OverrideMetadata(typeof (QrCodeGeoControl),
                                                       new FrameworkPropertyMetadata(VerticalAlignment.Center));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Controls.Control"/> class.
        /// </summary>
        /// <remarks></remarks>
        public QrCodeGeoControl()
        {
            UpdateGeometry();
            UpdatePadding();
        }

        /// <summary>
        /// Return whether if class is locked
        /// </summary>
        /// <remarks></remarks>
        public bool IsLocked
        {
            get { return m_isLocked; }
        }

        /// <summary>
        /// Occurs when [qr matrix changed].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler QrMatrixChanged;

        /// <summary>
        /// Occure when ErrorCorrectLevel or Text changed
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private static void OnMatrixValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var geoControl = (QrCodeGeoControl) d;
            geoControl.UpdateGeometry();
            geoControl.UpdatePadding();
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
        /// Update Geometry if is unlocked.
        /// </summary>
        /// <remarks></remarks>
        internal void UpdateGeometry()
        {
            if (m_isLocked)
                return;
            new QrEncoder(ErrorCorrectLevel).TryEncode(Text, out m_QrCode);
            OnQrMatrixChanged(new EventArgs());
            m_width = m_QrCode.Matrix == null ? 21 : m_QrCode.Matrix.Width;
            QrGeometry =
                new DrawingBrushRenderer(new FixedCodeSize(200, QuietZoneModule), DarkBrush, LightBrush).DrawGeometry(
                    m_QrCode.Matrix, 0, 0);
        }

        /// <summary>
        /// Called when [quiet zone pixel size changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private static void OnQuietZonePixelSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((QrCodeGeoControl) d).UpdatePadding();
        }

        /// <summary>
        /// This method is use to update QuietZone after use method SetQuietZoneModule
        /// </summary>
        /// <remarks></remarks>
        internal void UpdatePadding()
        {
            if (m_isLocked)
                return;
            double length = ActualWidth < ActualHeight ? ActualWidth : ActualHeight;
            double moduleSize = length/(m_width + 2*(int) QuietZoneModule);
            Padding = new Thickness(moduleSize*(int) QuietZoneModule);
        }

        /// <summary>
        /// Called to arrange and size the content of a <see cref="T:System.Windows.Controls.Control"/> object.
        /// </summary>
        /// <param name="arrangeBounds">The computed size that is used to arrange the content.</param>
        /// <returns>The size of the control.</returns>
        /// <remarks></remarks>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            double width = arrangeBounds.Width < arrangeBounds.Height ? arrangeBounds.Width : arrangeBounds.Height;
            double moduleSize = width/(m_width + 2*(int) QuietZoneModule);
            Padding = new Thickness(moduleSize*(int) QuietZoneModule);
            return base.ArrangeOverride(arrangeBounds);
        }

        /// <summary>
        /// If Class is locked, it won't update Geometry or quietzone.
        /// </summary>
        /// <remarks></remarks>
        public void Lock()
        {
            m_isLocked = true;
        }

        /// <summary>
        /// Unlock class will cause class to update Geometry and quietZone.
        /// </summary>
        /// <remarks></remarks>
        public void Unlock()
        {
            m_isLocked = false;
            UpdateGeometry();
            UpdatePadding();
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

        #region LightBrush

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LightBrushProperty =
            DependencyProperty.Register("LightBrush", typeof (Brush), typeof (QrCodeGeoControl),
                                        new UIPropertyMetadata(Brushes.White));

        /// <summary>
        /// Gets or sets the light brush.
        /// </summary>
        /// <value>The light brush.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Qr Code")]
        public Brush LightBrush
        {
            get { return (Brush) GetValue(LightBrushProperty); }
            set { SetValue(LightBrushProperty, value); }
        }

        #endregion

        #region DarkBrush

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DarkBrushProperty =
            DependencyProperty.Register("DarkBrush", typeof (Brush), typeof (QrCodeGeoControl),
                                        new UIPropertyMetadata(Brushes.Black));

        /// <summary>
        /// Gets or sets the dark brush.
        /// </summary>
        /// <value>The dark brush.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Qr Code")]
        public Brush DarkBrush
        {
            get { return (Brush) GetValue(DarkBrushProperty); }
            set { SetValue(DarkBrushProperty, value); }
        }

        #endregion

        #region QuietZone

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty QuietZoneModuleProperty =
            DependencyProperty.Register("QuietZoneModule", typeof (QuietZoneModules), typeof (QrCodeGeoControl),
                                        new UIPropertyMetadata(QuietZoneModules.Two, OnQuietZonePixelSizeChanged));

        /// <summary>
        /// Gets or sets the quiet zone module.
        /// </summary>
        /// <value>The quiet zone module.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Qr Code")]
        public QuietZoneModules QuietZoneModule
        {
            get { return (QuietZoneModules) GetValue(QuietZoneModuleProperty); }
            set { SetValue(QuietZoneModuleProperty, value); }
        }

        #endregion

        #region ErrorCorrectionLevel

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ErrorCorrecLeveltProperty =
            DependencyProperty.Register("ErrorCorrectLevel", typeof (ErrorCorrectionLevel), typeof (QrCodeGeoControl),
                                        new UIPropertyMetadata(ErrorCorrectionLevel.M, OnMatrixValueChanged));

        /// <summary>
        /// Gets or sets the error correct level.
        /// </summary>
        /// <value>The error correct level.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Qr Code")]
        public ErrorCorrectionLevel ErrorCorrectLevel
        {
            get { return (ErrorCorrectionLevel) GetValue(ErrorCorrecLeveltProperty); }
            set { SetValue(ErrorCorrecLeveltProperty, value); }
        }

        #endregion

        #region Text

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (QrCodeGeoControl),
                                        new UIPropertyMetadata(string.Empty, OnMatrixValueChanged));

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        /// <remarks></remarks>
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Qr Code")]
        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region QrGeometry

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty QrGeometryProperty =
            DependencyProperty.Register("QrGeometry", typeof (Geometry), typeof (QrCodeGeoControl), null);

        /// <summary>
        /// Gets the qr geometry.
        /// </summary>
        /// <remarks></remarks>
        public Geometry QrGeometry
        {
            get { return (Geometry) GetValue(QrGeometryProperty); }
            private set { SetValue(QrGeometryProperty, value); }
        }

        #endregion
    }
}